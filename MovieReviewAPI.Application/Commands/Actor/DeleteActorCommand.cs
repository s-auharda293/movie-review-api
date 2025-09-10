using MediatR;
using MovieReviewApi.Application.DTOs;

namespace MovieReviewApi.Application.Commands.Actor
{
    public record DeleteActorCommand(Guid Id) : IRequest<Result<bool>>;
}
