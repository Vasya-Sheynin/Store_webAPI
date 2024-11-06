using CommonModules.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Users.Application
{
    public record UserDto(
        Guid Id,
        string Name,
        string Email,
        string PasswordHash,
        SecurityRoles Role
    );

    public record CreateUserDto(
        [Required] string Name,
        [Required] string Email,
        [Required] string Password,
        [Range(0, 1)] SecurityRoles Role
    );

    public record UpdateUserDto(
        [Required] string Name,
        [Required] string Email,
        [Required] string Password,
        [Range(0, 1)] SecurityRoles Role
    );

    public record UserLoginDto(
        string Name,
        string Password
    );

    public record UserRegisterDto(
        string Name,
        string Password,
        string Email
    );
}
