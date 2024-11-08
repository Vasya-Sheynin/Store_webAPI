using Users.Application.ServiceInterfaces;
using CommonModules.Domain.Entities;
using CommonModules.Domain.Interfaces;
using MediatR;
using Users.Application.Validation.Commands;
using Users.Application.Exceptions;

namespace Users.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;
        private readonly ISender sender;

        public UserService(IUserRepository repository, ISender s)
        {
            userRepository = repository;
            sender = s;
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
            await sender.Send(new ValidateUserInsertCommand(userDto));

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
            await sender.Send(new ValidateUserUpdateCommand(userDto));

            var userById = await userRepository.GetUserByIdAsync(id);
            if (userById is null)
            {
                throw new UserNotFoundException("UserService");
            }

            var user = await userRepository.GetUserByIdAsync(id);

            user.Id = id;
            user.Name = userDto.Name;
            user.Email = userDto.Email;
            user.PasswordHash = BCrypt.Net.BCrypt.EnhancedHashPassword(userDto.Password);
            user.Role = userDto.Role;

            await userRepository.UpdateUserAsync(user);
        }
    }
}
