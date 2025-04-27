using FMU.UserService.Application.Interfaces;

namespace FMU.UserService.Infrastructure.Messaging
{
    public class MockEventPublisher : IEventPublisher
    {
        private readonly ILogger<MockEventPublisher> _logger;

        public MockEventPublisher(ILogger<MockEventPublisher> logger)
        {
            _logger = logger;
        }

        public Task PublishAsync<T>(T @event) where T : class
        {
            _logger.LogInformation($"[MOCK] Event published: {@event.GetType().Name}");
            return Task.CompletedTask;
        }
    }
}
