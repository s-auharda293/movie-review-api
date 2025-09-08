using MediatR;
using MovieReviewApi.Application.DTOs;

namespace MovieReviewApi.Application.Queries.Actor
{
   public record GetActorsByIdsQuery(List<Guid> Ids):IRequest<IEnumerable<ActorDto>>;
}
