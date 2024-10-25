using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Store_webAPI.Controllers.Dto;
using Store_webAPI.Data;
using Store_webAPI.Data.Entities;

namespace Store_webAPI.Controllers
{
    [Route("store-api")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext dbContext;

        public UserController(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("users")]
        public ActionResult<IEnumerable<UserDto>> Get()
        {
            var users = dbContext.Users;
            
            List<UserDto> result = users.Select(user => new UserDto(
                user.Id, 
                user.Name,
                user.Email,
                user.PasswordHash,
                user.Role
            )).ToList();

            return result;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("users/{id}")]
        public async Task<ActionResult<UserDto>> GetById([FromRoute] Guid id)
        {
            var userToGet = await dbContext.Users.Where(user => user.Id == id).SingleOrDefaultAsync();

            if (userToGet is null)
            {
                return NotFound();
            }

            var result = new UserDto(
                userToGet.Id,
                userToGet.Name,
                userToGet.Email,
                userToGet.PasswordHash,
                userToGet.Role
            );

            return result;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("users")]
        public async Task<ActionResult<UserDto>> Create([FromBody] CreateUserDto newUser)
        {
            var user = new User(
                Guid.NewGuid(), 
                newUser.Name, 
                newUser.Email, 
                BCrypt.Net.BCrypt.EnhancedHashPassword(newUser.Password), 
                newUser.Role
            ); 

            await dbContext.AddAsync(user);
            await dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = user.Id }, 
                new UserDto(user.Id, user.Name, user.Email, user.PasswordHash, user.Role));
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("users/{id}")]
        public async Task<ActionResult> Update([FromRoute] Guid id,[FromBody] UpdateUserDto newUser)
        {
            var userToUpdate = await dbContext.Users.Where(user => user.Id == id).SingleOrDefaultAsync();

            if (userToUpdate is null)
            {
                return NotFound();
            }


            userToUpdate.Id = id;
            userToUpdate.Name = newUser.Name;
            userToUpdate.Email = newUser.Email;
            userToUpdate.PasswordHash = BCrypt.Net.BCrypt.EnhancedHashPassword(newUser.Password);
            userToUpdate.Role = newUser.Role;

            dbContext.Update(userToUpdate);
            await dbContext.SaveChangesAsync();

            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("users/{id}")]
        public async Task<ActionResult> Delete([FromRoute] Guid id)
        {
            var userById = await dbContext.Users.Where(user => user.Id == id).SingleOrDefaultAsync();
            if (userById is null)
            {
                return NotFound();
            }

            dbContext.Remove(userById);
            await dbContext.SaveChangesAsync();

            return NoContent();
        }
    }
}
