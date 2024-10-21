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

        [HttpGet("users")]
        public ActionResult<IEnumerable<UserDto>> Get()
        {
            var users = dbContext.Users;
            
            List<UserDto> result = users.Select(user => new UserDto(
                user.Id, 
                user.Name,
                user.Email,
                user.Password,
                user.Role
            )).ToList();

            return result;
        }

        [HttpGet("users/{id}")]
        public async Task<ActionResult<UserDto>> GetById(Guid id)
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
                userToGet.Password,
                userToGet.Role
            );

            return result;
        }

        [HttpPost("users")]
        public async Task<ActionResult<UserDto>> Create(CreateUserDto newUser)
        {
            var user = new User(
                Guid.NewGuid(), 
                newUser.Name, 
                newUser.Email, 
                newUser.Password, 
                newUser.Role
            ); 

            await dbContext.AddAsync(user);
            await dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
        }

        [HttpPut("users/{id}")]
        public async Task<ActionResult> Update(Guid id, UpdateUserDto newUser)
        {
            var userToUpdate = await dbContext.Users.Where(user => user.Id == id).SingleOrDefaultAsync();

            if (userToUpdate is null)
            {
                return NotFound();
            }


            userToUpdate.Id = id;
            userToUpdate.Name = newUser.Name;
            userToUpdate.Email = newUser.Email;
            userToUpdate.Password = newUser.Password;
            userToUpdate.Role = newUser.Role;

            dbContext.Update(userToUpdate);
            await dbContext.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("users/{id}")]
        public async Task<ActionResult> Delete(Guid id)
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
