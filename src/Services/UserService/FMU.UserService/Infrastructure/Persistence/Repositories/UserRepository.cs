using FMU.UserService.Application.Interfaces;
using FMU.UserService.Domain.Entities;
using FMU.UserService.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace FMU.UserService.Infrastructure.Persistence.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserDbContext _context;

        public UserRepository(UserDbContext context)
        {
            _context = context;
        }

        public async Task<User> GetByIdAsync(Guid id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<User> GetByUsernameAsync(string username)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<IEnumerable<User>> SearchUsersAsync(string searchTerm, int limit = 10)
        {
            return await _context.Users
                .Where(u => u.Username.Contains(searchTerm) || u.DisplayName.Contains(searchTerm))
                .Take(limit)
                .ToListAsync();
        }

        public async Task<bool> UsernameExistsAsync(string username)
        {
            return await _context.Users.AnyAsync(u => u.Username == username);
        }

        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
        }

        public async Task UpdateAsync(User user)
        {
            _context.Users.Update(user);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
