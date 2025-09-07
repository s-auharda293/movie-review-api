using MediatR;
using MovieReviewApi.Application.DTOs;

namespace MovieReviewApi.Application.Queries.Actor
{
    public record GetMoviesByIdsQuery(List<Guid> Ids) : IRequest<IEnumerable<MovieDto>>;
}
