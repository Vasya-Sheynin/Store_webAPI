using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users.Application.Validation.Commands;

namespace Users.Application.Validation.CommandHandlers
{
    public class ValidateUserRecoveryCommandHandler : IRequestHandler<ValidateUserRecoveryCommand>
    {
        public async Task Handle(ValidateUserRecoveryCommand request, CancellationToken cancellationToken)
        {
            // Used for invoking Validate() in ValidationBehavior pipeline; 
        }
    }
}
