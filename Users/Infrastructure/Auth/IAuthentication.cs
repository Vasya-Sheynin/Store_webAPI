using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users.Application;

namespace Infrastructure.Auth
{
    public interface IAuthentication
    {
        Task<string> LoginAsync(UserLoginDto userLogin);

        Task<string> RegisterAsync(UserRegisterDto userRegister);
    }
}
