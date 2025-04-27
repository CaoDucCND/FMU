using FMU.EventBus.Events;

namespace FMU.EventBus.Abstractions
{
    /// <summary>
    /// Interface for handling a specific integration event
    /// </summary>
    public interface IIntegrationEventHandler<in TIntegrationEvent>
        where TIntegrationEvent : IntegrationEvent
    {
        /// <summary>
        /// Handles an integration event
        /// </summary>
        Task Handle(TIntegrationEvent @event);
    }

    /// <summary>
    /// Interface for handling dynamic integration events (when the type is not known at compile time)
    /// </summary>
    public interface IDynamicIntegrationEventHandler
    {
        /// <summary>
        /// Handles a dynamic integration event
        /// </summary>
        Task Handle(dynamic eventData);
    }
}
