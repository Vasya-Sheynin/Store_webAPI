using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Users.Application.Exceptions
{
    public class UserAlreadyExistsException : UserBaseException
    {
        public UserAlreadyExistsException(string instance)
        {
            Type = "user-already-exists-exception";
            Title = "User already exists exception.";
            Detail = "Cannot add new user since another with the same credentials already exists.";
            Instance = instance;
        }
    }
}
