using MediatR;

namespace Products.Application.Validation.Commands
{
    public record DeleteProductCommand(Guid id, Guid userId) : IRequest;
}
