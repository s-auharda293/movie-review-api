using MediatR;
using MovieReviewApi.Application.DTOs;

namespace MovieReviewApi.Application.Queries.Movie
{
    //public record GetMoviesQuery(MovieRequestDto request) : IRequest<Result<IEnumerable<MovieDto>>>;

    public record GetMoviesQuery() : IRequest<Result<IEnumerable<MovieDto>>>;
}
