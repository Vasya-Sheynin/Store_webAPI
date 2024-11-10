using CommonModules.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonModules.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetUsersAsync();
        Task<User> GetUserByIdAsync(Guid id);
        Task InsertUserAsync(User user);
        Task DeleteUserAsync(Guid id);
        Task UpdateUserAsync(User user);
    }
}
