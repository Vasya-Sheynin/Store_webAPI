using MediatR;
using Users.Application.ServiceInterfaces;
using Users.Application.Validation.Commands;


namespace Users.Application.Validation.CommandHandlers
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand>
    {
        private readonly IUserService userService;

        public UpdateUserCommandHandler(IUserService service)
        {
            userService = service;
        }

        public async Task Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            await userService.UpdateUserAsync(request.id, request.updateUserDto);
        }
    }
}
