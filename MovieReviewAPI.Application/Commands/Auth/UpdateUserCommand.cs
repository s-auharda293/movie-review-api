using MediatR;
using MovieReviewApi.Application.DTOs;

namespace MovieReviewApi.Application.Commands.Auth
{
    public record UpdateUserCommand(Guid Id, UpdateUserRequest Request) : IRequest<Result<UserResponse>>;

}
