using MediatR;

namespace Users.Application.Validation.Queries
{
    public record GetUsersQuery() : IRequest<IEnumerable<UserDto>>;
}
