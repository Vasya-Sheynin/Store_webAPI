using Users.Application.ServiceInterfaces;
using CommonModules.Domain.Entities;
using CommonModules.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            await userRepository.DeleteUserAsync(id);
        }

        public async Task<UserDto> GetUserByIdAsync(Guid id)
        {
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

            user.Id = id;
            user.Name = userDto.Name;
            user.Email = userDto.Email;
            user.PasswordHash = BCrypt.Net.BCrypt.EnhancedHashPassword(userDto.Password);
            user.Role = userDto.Role;

            await userRepository.UpdateUserAsync(user);
        }
    }
}
