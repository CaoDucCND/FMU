using AutoMapper;
using FMU.UserService.Application.DTOs;
using FMU.UserService.Application.Interfaces;
using FMU.UserService.Domain.Entities;

namespace FMU.UserService.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<UserDto> GetUserByIdAsync(Guid id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            return _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto> GetUserByUsernameAsync(string username)
        {
            var user = await _userRepository.GetByUsernameAsync(username);
            return _mapper.Map<UserDto>(user);
        }

        public async Task<IEnumerable<UserDto>> SearchUsersAsync(string searchTerm, int limit = 10)
        {
            var users = await _userRepository.SearchUsersAsync(searchTerm, limit);
            return _mapper.Map<IEnumerable<UserDto>>(users);
        }

        public async Task<Guid> CreateUserAsync(CreateUserDto createUserDto)
        {
            var usernameExists = await _userRepository.UsernameExistsAsync(createUserDto.Username);
            if (usernameExists)
                throw new ApplicationException("Username already exists");

            var user = new User(createUserDto.Username, createUserDto.DisplayName);
            await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();

            return user.Id;
        }

        public async Task UpdateUserAsync(Guid id, UpdateUserDto updateUserDto)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                throw new ApplicationException("User not found");

            user.UpdateProfile(updateUserDto.DisplayName, updateUserDto.AvatarUrl);
            await _userRepository.UpdateAsync(user);
            await _userRepository.SaveChangesAsync();
        }
    }
}
