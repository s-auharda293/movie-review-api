using MediatR;
using MovieReviewApi.Application.DTOs;

namespace MovieReviewApi.Application.Queries.Actor
{
    public record SearchActorsQuery(ActorRequestDto request) : IRequest<Result<ActorResponseDto>>;
}
