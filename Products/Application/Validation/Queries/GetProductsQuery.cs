using MediatR;
using Products.Application.Filters;

namespace Products.Application.Validation.Queries
{
    public record GetProductsQuery(Filter filter) : IRequest<IEnumerable<ProductDto>>;
}
