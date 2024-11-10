using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users.Application.Validation.Commands;


namespace Users.Application.Validation.CommandHandlers
{
    public class ValidateUserUpdateCommandHandler : IRequestHandler<ValidateUserUpdateCommand>
    {
        public async Task Handle(ValidateUserUpdateCommand request, CancellationToken cancellationToken)
        {
            // Used for invoking Validate() in ValidationBehavior pipeline; 
        }
    }
}
