using MediatR;

namespace MovieReviewApi.Application.Commands.Movie
{
    public record DeleteMovieCommand(Guid Id):IRequest<Result<bool>>;
}
