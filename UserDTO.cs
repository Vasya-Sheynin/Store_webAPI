namespace Store_webAPI.UserDTOs
{
    public record UserDTO(int Id, string Name, string Email, string PasswordHash, string CreatedDate);

    public record CreateUserDTO(string Name, string Email, string Password);

    public record UpdateUserDTO(string Name, string Email, string Password);
}
