using MediatR;
using Products.Application.ServiceInterfaces;
using Products.Application.Validation.Commands;

namespace Products.Application.Validation.CommandHandlers
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, ProductDto>
    {
        private readonly IProductService productService;

        public CreateProductCommandHandler(IProductService prodSrv)
        {
            productService = prodSrv;
        }

        public async Task<ProductDto> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var product = await productService.InsertProductAsync(request.createProductDto, request.userId);

            return product;
        }
    }
}
