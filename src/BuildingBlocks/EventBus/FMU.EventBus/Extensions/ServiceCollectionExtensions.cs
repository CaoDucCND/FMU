using FMU.EventBus.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace FMU.EventBus.Extensions
{
    /// <summary>
    /// Extension methods for IServiceCollection to register EventBus services
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds EventBus core services to the service collection
        /// </summary>
        public static IServiceCollection AddEventBusCore(this IServiceCollection services)
        {
            // Register subscription manager
            services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();

            return services;
        }
    }
}
