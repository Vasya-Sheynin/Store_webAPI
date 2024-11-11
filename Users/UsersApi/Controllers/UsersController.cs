using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Users.Application;
using Users.Application.Validation.Commands;
using Users.Application.Validation.Queries;
using Users.Infrastructure.Validation.Commands;

namespace Users.UsersApi.Controllers
{
    [Route("store-api")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ISender sender;

        public UsersController(ISender s)
        {
            sender = s;
        }

        [AllowAnonymous]
        [HttpPost("auth/login")]
        public async Task<ActionResult> Login([FromBody] UserLoginDto userLogin)
        {
            var token = await sender.Send(new LoginUserCommand(userLogin));

            return Ok(token);
        }

        [AllowAnonymous]
        [HttpPost("auth/signup")]
        public async Task<ActionResult> Register([FromBody] UserRegisterDto userRegister)
        {
            var token = await sender.Send(new RegisterUserCommand(userRegister));

            return Ok(token);
        }

        [AllowAnonymous]
        [HttpPost("auth/recovery/email")]
        public async Task<ActionResult> GetRecoveryEmail([FromBody] UserRecoveryDto userRecovery)
        {
            await sender.Send(new SendRecoveryEmailCommand(userRecovery));

            return NoContent();
        }

        [AllowAnonymous]
        [HttpGet("auth/recovery")]
        public async Task<ActionResult> RecoverUserPassword([FromQuery] string token, [FromQuery] string password)
        {
            var loginToken = await sender.Send(new RecoverUserCommand(token, password));

            return Ok(loginToken);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("users")]
        public async Task<ActionResult<IEnumerable<UserDto>>> Get()
        {
            var users = await sender.Send(new GetUsersQuery());

            return Ok(users);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("users/{id}")]
        public async Task<ActionResult<UserDto>> GetById([FromRoute] Guid id)
        {
            var user = await sender.Send(new GetUserByIdQuery(id));

            return Ok(user);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("users")]
        public async Task<ActionResult<UserDto>> Create([FromBody] CreateUserDto newUser)
        {
            var user = await sender.Send(new CreateUserCommand(newUser));

            return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("users/{id}")]
        public async Task<ActionResult> Update([FromRoute] Guid id, [FromBody] UpdateUserDto newUser)
        {
            await sender.Send(new UpdateUserCommand(id, newUser));

            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("users/{id}")]
        public async Task<ActionResult> Delete([FromRoute] Guid id)
        {
            await sender.Send(new DeleteUserCommand(id));

            return NoContent();
        }
    }
}
