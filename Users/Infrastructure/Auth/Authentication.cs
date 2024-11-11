using CommonModules.Domain.Entities;
using CommonModules.Domain.Interfaces;
using Infrastructure.Auth;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Users.Application;
using Users.Application.Exceptions;

namespace Users.Infrastructure.Auth
{
    public class Authentication : IAuthentication
    {
        private readonly IUserRepository userRepository;
        private readonly IConfiguration configManager;

        public Authentication(IUserRepository repository, IConfiguration config)
        {
            userRepository = repository;
            configManager = config;
        }

        public async Task<string> LoginAsync(UserLoginDto userLogin)
        {
            var user = await Authenticate(userLogin);
            string token = string.Empty;

            if (user is not null)
            {
                token = GenerateToken(user, 60);
            }
            else
            {
                throw new UserNotFoundException("Authentication");
            }

            return token;
        }

        public async Task<string> RegisterAsync(UserRegisterDto userRegister)
        {
            var u = (await userRepository.GetUsersAsync()).FirstOrDefault(i => i.Name == userRegister.Name);
            if (u is not null)
            {
                throw new UserAlreadyExistsException("Authentication");
            }

            var user = new User(
                Guid.NewGuid(),
                userRegister.Name,
                userRegister.Email,
                BCrypt.Net.BCrypt.EnhancedHashPassword(userRegister.Password),
                SecurityRoles.Standard
            );

            await userRepository.InsertUserAsync(user);

            var token = GenerateToken(user, 60);

            return token;
        }

        private string GenerateToken(User user, int duration)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configManager.GetSection("Jwt")["Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Uri, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            var token = new JwtSecurityToken(
                configManager.GetSection("Jwt")["Issuer"],
                configManager.GetSection("Jwt")["Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(duration),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private async Task<User?> Authenticate(UserLoginDto userLogin)
        {
            var users = (await userRepository.GetUsersAsync())
                .Where(u => u.Name.ToLower() == userLogin.Name.ToLower())
                .ToList();

            foreach (var user in users)
            {
                if (BCrypt.Net.BCrypt.EnhancedVerify(userLogin.Password, user.PasswordHash))
                {
                    return user;
                }
            }

            return null;
        }

        public async Task<string> GenerateRecoveryTokenAsync(UserRecoveryDto user)
        {
            var userToRecover = (await userRepository.GetUsersAsync()).FirstOrDefault(u => u.Name == user.Name);

            if (userToRecover == null)
            {
                throw new UserNotFoundException("Authentication");
            }

            string recoveryToken = GenerateToken(userToRecover, 5);

            return recoveryToken;
        }

        public async Task<string> RecoverAsync(Guid userId, string newPassword)
        {
            var user = await userRepository.GetUserByIdAsync(userId);
            user.PasswordHash = BCrypt.Net.BCrypt.EnhancedHashPassword(newPassword);
            await userRepository.UpdateUserAsync(user);

            return GenerateToken(user, 60);
        }

        public Claim[] ParseToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadJwtToken(token);

            var claims = jsonToken.Claims.ToArray();

            return claims;
        }
    }
}
