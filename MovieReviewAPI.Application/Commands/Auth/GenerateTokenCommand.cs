using MediatR;
using MovieReviewApi.Application.DTOs;

namespace MovieReviewApi.Application.Commands.Auth
{
        public record GenerateTokenCommand(string RefreshToken) : IRequest<Result<CurrentUserResponse>>;
}
