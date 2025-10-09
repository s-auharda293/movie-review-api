using MediatR;
using MovieReviewApi.Application.DTOs;

namespace MovieReviewApi.Application.Queries.Movie
{
    public record SearchMoviesQuery(MovieRequestDto request) : IRequest<Result<MovieResponseDto>>;
}
