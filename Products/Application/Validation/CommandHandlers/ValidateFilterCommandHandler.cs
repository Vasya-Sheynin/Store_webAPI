using MediatR;
using Products.Application.Validation.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Products.Application.Validation.CommandHandlers
{
    public class ValidateFilterCommandHandler : IRequestHandler<ValidateFilterCommand>
    {
        public async Task Handle(ValidateFilterCommand request, CancellationToken cancellationToken)
        {
            // Used for invoking Validate() in ValidationBehavior pipeline;
        }
    }
}
