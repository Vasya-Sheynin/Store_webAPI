using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Users.Application.Validation.Commands
{
    public record ValidateUserRegisterCommand(UserRegisterDto userRegisterDto) : IRequest;
}
