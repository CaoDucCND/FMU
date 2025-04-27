namespace FMU.EventBus
{
    /// <summary>
    /// Enum defining the types of supported event bus providers
    /// </summary>
    public enum EventBusProviderType
    {
        /// <summary>
        /// In-memory event bus (for development or testing)
        /// </summary>
        InMemory,

        /// <summary>
        /// RabbitMQ event bus
        /// </summary>
        RabbitMQ,

        /// <summary>
        /// Kafka event bus (future implementation)
        /// </summary>
        Kafka,

        /// <summary>
        /// Azure Service Bus (future implementation)
        /// </summary>
        AzureServiceBus,

        /// <summary>
        /// Redis event bus (future implementation)
        /// </summary>
        Redis
    }
}