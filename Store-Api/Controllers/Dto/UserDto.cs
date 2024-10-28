using Microsoft.AspNetCore.Identity;
using Store_Api.Data;
using Store_Api.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace Store_Api.Controllers.Dto
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
}
