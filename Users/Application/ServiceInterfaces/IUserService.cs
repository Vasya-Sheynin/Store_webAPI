namespace Users.Application.ServiceInterfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetUsersAsync();
        Task<UserDto> GetUserByIdAsync(Guid id);
        Task<UserDto> InsertUserAsync(CreateUserDto user);
        Task DeleteUserAsync(Guid id);
        Task UpdateUserAsync(Guid id, UpdateUserDto user);
    }
}
