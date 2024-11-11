using MediatR;

namespace Products.Application.Validation.Queries
{
    public record GetProductByIdQuery(Guid id) : IRequest<ProductDto>;
}
