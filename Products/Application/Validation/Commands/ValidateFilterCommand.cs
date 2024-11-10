using MediatR;
using Products.Application.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Products.Application.Validation.Commands
{
    public record ValidateFilterCommand(Filter filter) : IRequest;
}
