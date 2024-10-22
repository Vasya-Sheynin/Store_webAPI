using Microsoft.AspNetCore.Identity;
using Store_webAPI.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace Store_webAPI.Controllers.Dto
{
    public record UserDto(
        Guid Id,
        string Name,
        string Email,
        string PasswordHash,
        User.UserRole Role
    );

    public record CreateUserDto(
        [Required] string Name,
        [Required] string Email,
        [Required] string Password,
        [Range(1, 2)] User.UserRole Role
    );

    public record UpdateUserDto(
        [Required] string Name,
        [Required] string Email,
        [Required] string Password,
        [Range(1, 2)] User.UserRole Role
    );
}
