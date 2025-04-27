using FMU.UserService.Domain.Entities;

namespace FMU.UserService.Application.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetByIdAsync(Guid id);
        Task<User> GetByUsernameAsync(string username);
        Task<IEnumerable<User>> SearchUsersAsync(string searchTerm, int limit = 10);
        Task<bool> UsernameExistsAsync(string username);
        Task AddAsync(User user);
        Task UpdateAsync(User user);
        Task SaveChangesAsync();
    }
}
