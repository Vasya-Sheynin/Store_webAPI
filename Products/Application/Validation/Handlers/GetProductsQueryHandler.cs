using MediatR;
using Products.Application.ServiceInterfaces;
using Products.Application.Validation.Queries;

namespace Products.Application.Validation.Handlers
{
    public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, IEnumerable<ProductDto>>
    {
        private readonly IProductService productService;

        public GetProductsQueryHandler(IProductService prodSrv)
        {
            productService = prodSrv;
        }

        public async Task<IEnumerable<ProductDto>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
        {
            var products = await productService.GetFilteredProductsAsync(request.filter);

            return products;
        }
    }
}
