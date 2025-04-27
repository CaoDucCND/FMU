namespace FMU.UserService.Application.Interfaces
{
    // Application/Interfaces/IEventPublisher.cs
    public interface IEventPublisher
    {
        Task PublishAsync<T>(T @event) where T : class;
    }
}
