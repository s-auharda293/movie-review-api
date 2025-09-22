using MediatR;
using MovieReviewApi.Application.DTOs;

namespace MovieReviewApi.Application.Commands.Auth
{
    public record RevokeRefreshTokenCommand(string RefreshToken) : IRequest<Result<RevokeRefreshTokenResponse>>;
}
