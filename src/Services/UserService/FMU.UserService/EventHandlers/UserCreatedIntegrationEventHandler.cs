using FMU.EventBus.Abstractions;
using FMU.EventBus.Events;
using FMU.UserService.Application.DTOs;
using FMU.UserService.Application.Interfaces;

namespace FMU.UserService.EventHandlers
{
    public class UserCreatedIntegrationEventHandler : IIntegrationEventHandler<UserCreatedIntegrationEvent>
    {
        private readonly IUserService _userService;
        private readonly IEventBus _eventBus;
        private readonly ILogger<UserCreatedIntegrationEventHandler> _logger;

        public UserCreatedIntegrationEventHandler(
            IUserService userService,
            IEventBus eventBus,
            ILogger<UserCreatedIntegrationEventHandler> logger)
        {
            _userService = userService;
            _eventBus = eventBus;
            _logger = logger;
        }

        public async Task Handle(UserCreatedIntegrationEvent @event)
        {
            _logger.LogInformation("Handling UserCreatedIntegrationEvent: {UserId}, {Username}",
                @event.UserId, @event.Username);

            try
            {
                // Create user profile
                var createProfileDto = new CreateUserDto
                {
                    UserId = @event.UserId,
                    Username = @event.Username,
                    DisplayName = @event.Username // Default display name is the username
                };

                var profileId = string.Empty;// await _userService.CreateUserProfileAsync(createProfileDto);

                _logger.LogInformation("Created user profile {ProfileId} for user {UserId}",
                    profileId, @event.UserId);

                // Publish UserProfileCreated event
                _eventBus.Publish(new UserCreatedIntegrationEvent(
                    @event.UserId,
                    profileId,
                    @event.Username
                ));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error handling UserCreatedIntegrationEvent for user {UserId}",
                    @event.UserId);
                // In a production system, we might want to move this to a dead-letter queue
                // or have a retry mechanism
            }
        }
    }
}
