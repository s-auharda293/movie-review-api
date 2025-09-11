using MediatR;
using MovieReviewApi.Application.DTOs;

namespace MovieReviewApi.Application.Commands.Actor
{
    public record CreateActorCommand(CreateActorDto dto) : IRequest<Result<ActorDto>>;

}
