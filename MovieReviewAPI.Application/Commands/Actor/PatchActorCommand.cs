using MediatR;
using MovieReviewApi.Application.DTOs;

namespace MovieReviewApi.Application.Commands.Actor
{
    public record PatchActorCommand(Guid Id, PatchActorDto dto) : IRequest<bool>;
}
