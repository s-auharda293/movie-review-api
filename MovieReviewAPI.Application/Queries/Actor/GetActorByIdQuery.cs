using MediatR;
using MovieReviewApi.Application.DTOs;


namespace MovieReviewApi.Application.Queries.Actor
{
    public record GetActorByIdQuery(Guid Id): IRequest<Result<ActorDto>>;
}
