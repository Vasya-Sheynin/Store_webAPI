using MediatR;
using Users.Application.ServiceInterfaces;
using Users.Application.Validation.Queries;

namespace Users.Application.Validation.CommandHandlers
{
    public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, IEnumerable<UserDto>>
    {
        private readonly IUserService userService;

        public GetUsersQueryHandler(IUserService service)
        {
            userService = service;
        }

        public async Task<IEnumerable<UserDto>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            var users = await userService.GetUsersAsync();

            return users;
        }
    }
}
