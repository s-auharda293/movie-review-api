using MediatR;
using MovieReviewApi.Application.DTOs;

namespace MovieReviewApi.Application.Commands.Auth
{
    public record RegisterUserCommand(UserRegisterRequest Request) : IRequest<Result<UserResponse>>;
}
