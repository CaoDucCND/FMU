namespace FMU.UserService.Domain.Entities
{
    public class User
    {
        public Guid Id { get; private set; }
        public string Username { get; private set; }
        public string DisplayName { get; private set; }
        public string AvatarUrl { get; private set; }
        public DateTime LastActive { get; private set; }


        public User(string username, string displayName)
        {
            Id = Guid.NewGuid();
            Username = username;
            DisplayName = displayName ?? username;
            LastActive = DateTime.UtcNow;
        }

        public void UpdateProfile(string displayName, string avatarUrl)
        {
            if (!string.IsNullOrWhiteSpace(displayName))
                DisplayName = displayName;

            AvatarUrl = avatarUrl;
        }
    }
}
