using CommonModules.Domain.Entities;
using CommonModules.Domain.Interfaces;
using Users.Application.Exceptions;
using Users.Application.ServiceInterfaces;

namespace Users.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;

        public UserService(IUserRepository repository)
        {
            userRepository = repository;
        }

        public async Task DeleteUserAsync(Guid id)
        {
            var userById = await userRepository.GetUserByIdAsync(id);
            if (userById is null)
            {
                throw new UserNotFoundException("UserService");
            }

            await userRepository.DeleteUserAsync(id);
        }

        public async Task<UserDto> GetUserByIdAsync(Guid id)
        {
            var userById = await userRepository.GetUserByIdAsync(id);
            if (userById is null)
            {
                throw new UserNotFoundException("UserService");
            }

            var user = await userRepository.GetUserByIdAsync(id);
            var userDto = new UserDto(user.Id, user.Name, user.Email, user.PasswordHash, user.Role);

            return userDto;
        }

        public async Task<IEnumerable<UserDto>> GetUsersAsync()
        {
            var users = (await userRepository.GetUsersAsync()).Select(user => new UserDto(
                user.Id,
                user.Name,
                user.Email,
                user.PasswordHash,
                user.Role
                ));

            return users;
        }

        public async Task<UserDto> InsertUserAsync(CreateUserDto userDto)
        {
            var newUser = new User(
                Guid.NewGuid(),
                userDto.Name,
                userDto.Email,
                BCrypt.Net.BCrypt.EnhancedHashPassword(userDto.Password),
                userDto.Role
                );

            await userRepository.InsertUserAsync(newUser);

            var user = new UserDto(newUser.Id, newUser.Name, newUser.Email, newUser.PasswordHash, newUser.Role);

            return user;
        }

        public async Task UpdateUserAsync(Guid id, UpdateUserDto userDto)
        {
            var user = await userRepository.GetUserByIdAsync(id);
            if (user is null)
            {
                throw new UserNotFoundException("UserService");
            }

            var users = (await userRepository.GetUsersAsync()).FirstOrDefault(u => u.Name == userDto.Name);
            if (users is not null)
            {
                throw new UserAlreadyExistsException("UserService");
            }

            user.Id = id;
            user.Name = userDto.Name;
            user.Email = userDto.Email;
            user.PasswordHash = BCrypt.Net.BCrypt.EnhancedHashPassword(userDto.Password);
            user.Role = userDto.Role;

            await userRepository.UpdateUserAsync(user);
        }
    }
}
