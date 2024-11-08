using CommonModules.Domain.Entities;
using Infrastructure.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using Users.Application;
using Users.Application.ServiceInterfaces;
using Users.Infrastructure.Email;

namespace Users.UsersApi.Controllers
{
    [Route("store-api")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService userService;
        private readonly IAuthentication authentication;
        private readonly Infrastructure.Email.IEmailSender emailSender;
        private readonly EmailConfig emailConfig;

        public UsersController(IUserService service, IAuthentication auth, Infrastructure.Email.IEmailSender sender, EmailConfig config)
        {
            userService = service;
            authentication = auth;
            emailSender = sender;
            emailConfig = config;
        }

        [AllowAnonymous]
        [HttpPost("auth/login")]
        public async Task<ActionResult> Login([FromBody] UserLoginDto userLogin)
        {
            var token = await authentication.LoginAsync(userLogin);

            return Ok(token);
        }

        [AllowAnonymous]
        [HttpPost("auth/signup")]
        public async Task<ActionResult> Register([FromBody] UserRegisterDto userRegister)
        {
            var token = await authentication.RegisterAsync(userRegister);

            return Ok(token);
        }

        [AllowAnonymous]
        [HttpPost("auth/recovery")]
        public async Task<ActionResult> RecoverPasswordForAccount([FromBody] UserRecoveryDto userRecovery)
        {
            var newPassword = await userService.SetDefaultPasswordAsync(userRecovery);

            var message = new Message(emailConfig.From, "Password recovery", $"Password for account {userRecovery.Name} has been set to {newPassword}");
            emailSender.SendEmail(message);

            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("users")]
        public async Task<ActionResult<IEnumerable<UserDto>>> Get()
        {
            var users = await userService.GetUsersAsync();

            return Ok(users);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("users/{id}")]
        public async Task<ActionResult<UserDto>> GetById([FromRoute] Guid id)
        {
            var user = await userService.GetUserByIdAsync(id);

            return Ok(user);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("users")]
        public async Task<ActionResult<UserDto>> Create([FromBody] CreateUserDto newUser)
        {
            var user = await userService.InsertUserAsync(newUser);

            return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("users/{id}")]
        public async Task<ActionResult> Update([FromRoute] Guid id, [FromBody] UpdateUserDto newUser)
        {
            await userService.UpdateUserAsync(id, newUser);

            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("users/{id}")]
        public async Task<ActionResult> Delete([FromRoute] Guid id)
        {
            await userService.DeleteUserAsync(id);

            return NoContent();
        }
    }
}
