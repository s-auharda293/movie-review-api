using MediatR;
using MovieReviewApi.Application.Commands.Actor;
using MovieReviewApi.Application.DTOs;
using System.Threading;

namespace MovieReviewApi.Application.Commands.Actor
{
    public record PatchActorCommand(Guid Id, PatchActorDto dto) : IRequest<bool>;
}
