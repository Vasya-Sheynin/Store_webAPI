using MediatR;
using Products.Application.ServiceInterfaces;
using Products.Application.Validation.Commands;

namespace Products.Application.Validation.CommandHandlers
{
    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand>
    {
        private readonly IProductService productService;

        public DeleteProductCommandHandler(IProductService prodSrv)
        {
            productService = prodSrv;
        }

        public async Task Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            await productService.DeleteProductAsync(request.id, request.userId);
        }
    }
}
