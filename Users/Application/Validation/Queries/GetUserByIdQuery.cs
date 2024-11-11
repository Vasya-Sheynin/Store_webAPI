using MediatR;

namespace Users.Application.Validation.Queries
{
    public record GetUserByIdQuery(Guid id) : IRequest<UserDto>;
}
