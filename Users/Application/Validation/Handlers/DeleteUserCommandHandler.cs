using MediatR;
using Users.Application.ServiceInterfaces;
using Users.Application.Validation.Commands;

namespace Users.Application.Validation.Handlers
{
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand>
    {
        private readonly IUserService userService;

        public DeleteUserCommandHandler(IUserService service)
        {
            userService = service;
        }
        public async Task Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            await userService.DeleteUserAsync(request.id);
        }
    }
}
