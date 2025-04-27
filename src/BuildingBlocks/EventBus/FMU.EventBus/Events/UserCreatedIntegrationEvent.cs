namespace FMU.EventBus.Events
{
    /// <summary>
    /// Event published when a new user is created
    /// </summary>
    public class UserCreatedIntegrationEvent : IntegrationEvent
    {
        public Guid UserId { get; private set; }
        public string Username { get; private set; }
        public string Email { get; private set; }

        public UserCreatedIntegrationEvent(Guid userId, string username, string email)
        {
            UserId = userId;
            Username = username;
            Email = email;
        }
    }
}
