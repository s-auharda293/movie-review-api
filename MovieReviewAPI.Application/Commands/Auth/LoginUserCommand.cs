using MediatR;
using MovieReviewApi.Application.DTOs;

namespace MovieReviewApi.Application.Commands.Auth
{
    public record LoginUserCommand(UserLoginRequest Request) : IRequest<Result<UserResponse>>;
}
