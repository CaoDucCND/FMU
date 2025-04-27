using FMU.UserService.Application.DTOs;

namespace FMU.UserService.Application.Interfaces
{
    public interface IUserService
    {
        Task<UserDto> GetUserByIdAsync(Guid id);
        Task<UserDto> GetUserByUsernameAsync(string username);
        Task<IEnumerable<UserDto>> SearchUsersAsync(string searchTerm, int limit = 10);
        Task<Guid> CreateUserAsync(CreateUserDto createUserDto);
        Task UpdateUserAsync(Guid id, UpdateUserDto updateUserDto);
    }
}
