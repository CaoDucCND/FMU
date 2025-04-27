using FMU.EventBus.Abstractions;
using FMU.EventBus.Events;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace FMU.EventBus.InMemory
{
    /// <summary>
    /// In-memory implementation of IEventBus for testing and development
    /// </summary>
    public class InMemoryEventBus : IEventBus
    {
        private readonly IEventBusSubscriptionsManager _subsManager;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger<InMemoryEventBus> _logger;

        public InMemoryEventBus(
            IEventBusSubscriptionsManager subsManager,
            IServiceScopeFactory serviceScopeFactory,
            ILogger<InMemoryEventBus> logger)
        {
            _subsManager = subsManager ?? throw new ArgumentNullException(nameof(subsManager));
            _serviceScopeFactory = serviceScopeFactory ?? throw new ArgumentNullException(nameof(serviceScopeFactory));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void Publish(IntegrationEvent @event)
        {
            var eventName = @event.GetType().Name;

            _logger.LogInformation("Publishing event {EventName} in-memory", eventName);

            if (_subsManager.HasSubscriptionsForEvent(eventName))
            {
                var subscriptions = _subsManager.GetHandlersForEvent(eventName);

                foreach (var subscription in subscriptions)
                {
                    ProcessEvent(@event, subscription).GetAwaiter().GetResult();
                }
            }
            else
            {
                _logger.LogWarning("No subscription for in-memory event {EventName}", eventName);
            }
        }

        public void Subscribe<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>
        {
            var eventName = _subsManager.GetEventKey<T>();

            _logger.LogInformation("Subscribing to event {EventName} with {EventHandler}", eventName, typeof(TH).Name);

            _subsManager.AddSubscription<T, TH>();
        }

        public void Unsubscribe<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>
        {
            var eventName = _subsManager.GetEventKey<T>();

            _logger.LogInformation("Unsubscribing from event {EventName}", eventName);

            _subsManager.RemoveSubscription<T, TH>();
        }

        public void SubscribeDynamic<TH>(string eventName) where TH : IDynamicIntegrationEventHandler
        {
            _logger.LogInformation("Subscribing to dynamic event {EventName} with {EventHandler}", eventName, typeof(TH).Name);

            _subsManager.AddDynamicSubscription<TH>(eventName);
        }

        public void UnsubscribeDynamic<TH>(string eventName) where TH : IDynamicIntegrationEventHandler
        {
            _logger.LogInformation("Unsubscribing from dynamic event {EventName}", eventName);

            _subsManager.RemoveDynamicSubscription<TH>(eventName);
        }

        private async Task ProcessEvent(IntegrationEvent @event, SubscriptionInfo subscription)
        {
            // Create a scope for dependency resolution
            using var scope = _serviceScopeFactory.CreateScope();

            // Resolve the handler from the service provider
            var handler = scope.ServiceProvider.GetService(subscription.HandlerType);
            if (handler == null)
            {
                _logger.LogWarning("No handler registered for event {EventName}", @event.GetType().Name);
                return;
            }

            // Handle the event based on its type
            if (subscription.IsDynamic)
            {
                // Cast and invoke dynamic handler
                var eventData = JsonSerializer.Serialize(@event);
                var concreteType = typeof(IDynamicIntegrationEventHandler);

                await (Task)concreteType.GetMethod("Handle")
                    .Invoke(handler, new object[] { JsonSerializer.Deserialize<dynamic>(eventData) });
            }
            else
            {
                // Cast and invoke typed handler
                var eventType = @event.GetType();
                var concreteType = typeof(IIntegrationEventHandler<>).MakeGenericType(eventType);

                await (Task)concreteType.GetMethod("Handle")
                    .Invoke(handler, new object[] { @event });
            }
        }
    }
}
