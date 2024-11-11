using MediatR;

namespace Products.Application.Validation.Commands
{
    public record UpdateProductCommand(Guid id, UpdateProductDto updateProductDto, Guid userId) : IRequest;
}
