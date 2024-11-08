﻿using CommonModules.Domain.Entities;
using CommonModules.Domain.Interfaces;
using Infrastructure.Auth;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Users.Application;
using Users.Application.Exceptions;
using Users.Application.Validation.Commands;

namespace Users.Infrastructure.Auth
{
    public class Authentication : IAuthentication
    {
        private readonly IUserRepository userRepository;
        private readonly IConfiguration configManager;
        private readonly ISender sender;

        public Authentication(IUserRepository repository, IConfiguration config, ISender s)
        {
            userRepository = repository;
            configManager = config;
            sender = s;
        }

        public async Task<string> LoginAsync(UserLoginDto userLogin)
        {
            await sender.Send(new ValidateUserLoginCommand(userLogin));

            var user = await Authenticate(userLogin);
            string token = string.Empty;

            if (user is not null)
            {
                token = GenerateToken(user);
            }
            else
            {
                throw new UserNotFoundException("Authentication");
            }

            return token;
        }

        public async Task<string> RegisterAsync(UserRegisterDto userRegister)
        {
            await sender.Send(new ValidateUserRegisterCommand(userRegister));

            if (await Authenticate(new UserLoginDto(userRegister.Name, userRegister.Password)) is not null)
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

            var token = GenerateToken(user);

            return token;
        }

        private string GenerateToken(User user)
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
                expires: DateTime.Now.AddMinutes(60),
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
    }
}
