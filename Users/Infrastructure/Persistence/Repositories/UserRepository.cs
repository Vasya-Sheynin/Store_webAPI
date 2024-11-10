using CommonModules.Domain.Entities;
using CommonModules.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using CommonModules.Persistence;

namespace Users.Infrastructure.Persistence.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext appDbContext;

        public UserRepository(AppDbContext dbContext) 
        {
            appDbContext = dbContext;
        }

        public async Task DeleteUserAsync(Guid id)
        {
            var userById = await appDbContext.Users.Where(user => user.Id == id).FirstOrDefaultAsync();
            appDbContext.Remove(userById);
            await appDbContext.SaveChangesAsync();
        }

        public async Task<User> GetUserByIdAsync(Guid id)
        {
            var user = await appDbContext.Users.Where(user => user.Id == id).FirstOrDefaultAsync();

            return user;
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            var users = appDbContext.Users;

            List<User> result = await users.Select(user => new User(
                user.Id,
                user.Name,
                user.Email,
                user.PasswordHash,
                user.Role
            )).ToListAsync();

            return result;
        }

        public async Task InsertUserAsync(User user)
        {
            await appDbContext.Users.AddAsync(user);
            await appDbContext.SaveChangesAsync();
        }

        public async Task UpdateUserAsync(User user)
        {
            appDbContext.Users.Update(user);
            await appDbContext.SaveChangesAsync();
        }
    }
}
