using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Store_Api.Data;
using Store_Api.Controllers.Dto;
using Store_Api.Data.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Store_Api.Controllers
{
    [Route("store-api")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration config;
        private readonly AppDbContext dbContext;

        public LoginController(IConfiguration config, AppDbContext dbContext)
        {
            this.config = config;
            this.dbContext = dbContext;
        }

        [AllowAnonymous]
        [HttpPost("auth/login")]
        public async Task<ActionResult> Login([FromBody] UserLoginDto userLogin)
        {
            var user = await Authenticate(userLogin);

            if (user is not null)
            {
                var token = GenerateToken(user);

                return Ok(token);
            }

            return NotFound("User not found");
        }

        [AllowAnonymous]
        [HttpPost("auth/signup")]
        public async Task<ActionResult> Register([FromBody] UserRegisterDto userLogin)
        {
            if (await Authenticate(new UserLoginDto(userLogin.Name, userLogin.Password)) is not null)
            {
                return Conflict("A user with the same name and password already exists.");
            }

            var user = new User(
                Guid.NewGuid(),
                userLogin.Name,
                userLogin.Email,
                BCrypt.Net.BCrypt.EnhancedHashPassword(userLogin.Password),
                Data.SecurityRoles.Standard
            );

            await dbContext.AddAsync(user);
            await dbContext.SaveChangesAsync();

            var token = GenerateToken(user);

            return Ok(token);
        }

        private string GenerateToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.GetSection("Jwt")["Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Uri, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            var token = new JwtSecurityToken(
                config.GetSection("Jwt")["Issuer"],
                config.GetSection("Jwt")["Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(5),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private async Task<User?> Authenticate(UserLoginDto userLogin)
        {
            var users = await dbContext.Users
                .Where(u => u.Name.ToLower() == userLogin.Name.ToLower())
                .ToListAsync();

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
