using MediatR;
using MovieReviewApi.Application.DTOs;

namespace MovieReviewApi.Application.Commands.Actor
{
    public record UpdateActorCommand(Guid Id, UpdateActorDto dto):IRequest<bool>;
}
