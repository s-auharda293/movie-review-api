using MediatR;
using MovieReviewApi.Application.DTOs;

namespace MovieReviewApi.Application.Queries.Actor
{
    public record GetActorsQuery(): IRequest<Result<IEnumerable<ActorDto>>>;
}
