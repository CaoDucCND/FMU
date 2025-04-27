namespace FMU.EventBus.Events
{
    /// <summary>
    /// Base class for all integration events that are exchanged between services
    /// </summary>
    public abstract class IntegrationEvent
    {
        public Guid Id { get; private set; }
        public DateTime CreationDate { get; private set; }

        protected IntegrationEvent()
        {
            Id = Guid.NewGuid();
            CreationDate = DateTime.UtcNow;
        }

        protected IntegrationEvent(Guid id, DateTime createDate)
        {
            Id = id;
            CreationDate = createDate;
        }
    }
}
