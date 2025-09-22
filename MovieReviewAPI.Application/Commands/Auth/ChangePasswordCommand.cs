using MediatR;
using MovieReviewApi.Application.DTOs;

namespace MovieReviewApi.Application.Commands.Auth
{
    public record ChangePasswordCommand(ChangePasswordRequest Request) : IRequest<Result<ChangePasswordResponse>>;
}
