using MediatR;
using MovieReviewApi.Application.DTOs;

namespace MovieReviewApi.Application.Commands.Auth
{
        public record GenerateTokenCommand() : IRequest<Result<CurrentUserResponse>>;
}
