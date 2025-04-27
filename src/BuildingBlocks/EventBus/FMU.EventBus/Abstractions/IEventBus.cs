namespace FMU.EventBus.Abstractions
{
    /// <summary>
    /// Defines the methods required for an event bus implementation
    /// </summary>
    public interface IEventBus
    {
        /// <summary>
        /// Publishes an event to the event bus
        /// </summary>
        void Publish(Events.IntegrationEvent @event);

        /// <summary>
        /// Subscribes to an event with a specific handler
        /// </summary>
        void Subscribe<T, TH>()
            where T : Events.IntegrationEvent
            where TH : IIntegrationEventHandler<T>;

        /// <summary>
        /// Unsubscribes from an event with a specific handler
        /// </summary>
        void Unsubscribe<T, TH>()
            where T : Events.IntegrationEvent
            where TH : IIntegrationEventHandler<T>;

        /// <summary>
        /// Subscribes dynamically to an event using a handler factory
        /// </summary>
        void SubscribeDynamic<TH>(string eventName)
            where TH : IDynamicIntegrationEventHandler;

        /// <summary>
        /// Unsubscribes dynamically from an event
        /// </summary>
        void UnsubscribeDynamic<TH>(string eventName)
            where TH : IDynamicIntegrationEventHandler;
    }
}
