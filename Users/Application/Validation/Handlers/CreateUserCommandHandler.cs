using MediatR;
using Users.Application.ServiceInterfaces;
using Users.Application.Validation.Commands;

namespace Users.Application.Validation.CommandHandlers
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, UserDto>
    {
        private readonly IUserService userService;

        public CreateUserCommandHandler(IUserService service)
        {
            userService = service;
        }

        public async Task<UserDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await userService.InsertUserAsync(request.createUserDto);

            return user;
        }
    }
}
