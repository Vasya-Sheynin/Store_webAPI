using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Store_webAPI.Data;
using Store_webAPI.Data.Entities;

namespace Store_webAPI.Controllers
{
    [Route("storeApi/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext dbContext;

        public UserController(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
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

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetById(Guid id)
        {
            var user = await dbContext.Users.Where(user => user.Id == id).SingleOrDefaultAsync();

            if (user is null)
            {
                return NotFound();
            }

            var result = new UserDto(
                user.Id,
                user.Name,
                user.Email,
                user.Password,
                user.Role
            );

            return result;
        }

        [HttpPost]
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

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(Guid id, UpdateUserDto newUser)
        {
            var userToUpdate = await dbContext.Users.Where(user => user.Id == id).SingleOrDefaultAsync();

            if (userToUpdate is null)
            {
                return NotFound();
            }

            dbContext.Entry(userToUpdate).State = EntityState.Detached;

            User user = new User(
                id, 
                newUser.Name, 
                newUser.Email, 
                newUser.Password, 
                newUser.Role
            );
            dbContext.Users.Update(user);

            await dbContext.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
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
