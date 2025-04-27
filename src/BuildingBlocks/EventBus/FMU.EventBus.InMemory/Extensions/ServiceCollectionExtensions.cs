using FMU.EventBus.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using FMU.EventBus.Extensions;

namespace FMU.EventBus.InMemory.Extensions
{
    /// <summary>
    /// Extension methods for IServiceCollection to register InMemory event bus
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the In-Memory event bus to the service collection
        /// </summary>
        public static IServiceCollection AddInMemoryEventBus(this IServiceCollection services, Action<InMemoryEventBusOptions> configureOptions = null)
        {
            // Configure options
            var options = new InMemoryEventBusOptions();
            configureOptions?.Invoke(options);

            // Add EventBus core services
            services.AddEventBusCore();

            // Register InMemory event bus implementation
            services.AddSingleton<IEventBus, InMemoryEventBus>(sp =>
            {
                var subscriptionManager = sp.GetRequiredService<IEventBusSubscriptionsManager>();
                var serviceScopeFactory = sp.GetRequiredService<IServiceScopeFactory>();
                var logger = sp.GetRequiredService<ILogger<InMemoryEventBus>>();

                return new InMemoryEventBus(subscriptionManager, serviceScopeFactory, logger);
            });

            return services;
        }
    }
}
