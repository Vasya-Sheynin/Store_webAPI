using CommonModules.Domain.Entities;
using System.ComponentModel.DataAnnotations;

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
        [Required] string NewPassword,
        [Required] string Email
    );
}
