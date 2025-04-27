namespace FMU.UserService.Application.DTOs
{

    public class UserDto
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string DisplayName { get; set; }
        public string AvatarUrl { get; set; }
        public DateTime LastActive { get; set; }
    }

    public class CreateUserDto
    {
        public Guid UserId { get; set; } // From AuthService
        public string Username { get; set; }
        public string DisplayName { get; set; }
    }

    public class UpdateUserDto
    {
        public string DisplayName { get; set; }
        public string AvatarUrl { get; set; }
    }
}
