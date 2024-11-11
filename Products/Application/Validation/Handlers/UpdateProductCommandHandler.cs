using MediatR;
using Products.Application.ServiceInterfaces;
using Products.Application.Validation.Commands;

namespace Products.Application.Validation.CommandHandlers
{
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand>
    {
        private readonly IProductService productService;

        public UpdateProductCommandHandler(IProductService prodSrv)
        {
            productService = prodSrv;
        }

        public async Task Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            await productService.UpdateProductAsync(request.id, request.updateProductDto, request.userId);
        }
    }
}
