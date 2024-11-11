using MediatR;

namespace Products.Application.Validation.Commands
{
    public record CreateProductCommand(CreateProductDto createProductDto, Guid userId) : IRequest<ProductDto>;
}
