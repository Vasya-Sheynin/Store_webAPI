using System.Security.Claims;
using Users.Application;

namespace Infrastructure.Auth
{
    public interface IAuthentication
    {
        Task<string> LoginAsync(UserLoginDto userLogin);

        Task<string> RegisterAsync(UserRegisterDto userRegister);

        Task<string> RecoverAsync(Guid userId, string newPassword);

        Task<string> GenerateRecoveryTokenAsync(UserRecoveryDto user);

        Claim[] ParseToken(string token);
    }
}
