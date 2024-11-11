using MediatR;
using Products.Application;
using Products.Application.ServiceInterfaces;
using Products.Application.Validation.Queries;

namespace Application.Validation.Handlers
{
    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductDto>
    {
        private readonly IProductService productService;

        public GetProductByIdQueryHandler(IProductService prodSrv)
        {
            productService = prodSrv;
        }

        public async Task<ProductDto> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var product = await productService.GetProductByIdAsync(request.id);

            return product;
        }
    }
}
