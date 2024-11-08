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
        [Required] Guid Id,
        [Required] string Name,
        [Required] string Email,
        [Required] string PasswordHash,
        [Range(0, 1)] SecurityRoles Role
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
        [Required] string Name,
        [Required] string Password
    );

    public record UserRegisterDto(
        [Required] string Name,
        [Required] string Password,
        [Required] string Email
    );

    public record UserRecoveryDto(
    [Required] string Name,
    [Required] string Email
);
}
