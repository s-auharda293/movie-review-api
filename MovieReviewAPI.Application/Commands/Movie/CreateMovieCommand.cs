using MediatR;
using MovieReviewApi.Application.DTOs;

namespace MovieReviewApi.Application.Commands.Movie
{
    public record CreateMovieCommand(CreateMovieDto dto) : IRequest<MovieDto>;
}
