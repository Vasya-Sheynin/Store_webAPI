﻿using Microsoft.AspNetCore.Identity;
using Store_webApi.Data;
using Store_webAPI.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace Store_webAPI.Controllers.Dto
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
