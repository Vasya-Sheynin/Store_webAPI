using MediatR;
using Users.Application.ServiceInterfaces;
using Users.Application.Validation.Queries;

namespace Users.Application.Validation.Handlers
{
    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserDto>
    {
        private readonly IUserService userService;

        public GetUserByIdQueryHandler(IUserService service)
        {
            userService = service;
        }

        public async Task<UserDto> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var user = await userService.GetUserByIdAsync(request.id);

            return user;
        }
    }
}
