namespace Store_Api.Controllers.Dto
{
    public record UserLoginDto(
        string Name,
        string Password
    );

    public record UserRegisterDto(
        string Name,
        string Password,
        string Email
    );
}
