using MediatR;
using Microsoft.EntityFrameworkCore;
using MovieReviewApi.Application.DTOs;
using MovieReviewApi.Application.Interfaces;
using MovieReviewApi.Application.Queries.Movie;
using MovieReviewApi.Domain.Common.Movies;

namespace MovieReviewApi.Application.Handlers.Movie
{
    public class GetMovieByIdHandler:IRequestHandler<GetMovieByIdQuery,Result<MovieWithActorsDto>>
    {
        private readonly IApplicationDbContext _context;

        public GetMovieByIdHandler(IApplicationDbContext context)
        {
            _context = context;   
        }

        public async Task<Result<MovieWithActorsDto>> Handle(GetMovieByIdQuery request, CancellationToken cancellationToken)
        {
            var movie = await _context.Movies.Include(m => m.Actors).FirstOrDefaultAsync(m => m.Id == request.Id,cancellationToken);
            if (movie == null) return Result<MovieWithActorsDto>.Failure(MovieErrors.NotFound);
            var actors = movie.Actors.Select(a =>  a.Name).ToList();

            var dto = new MovieWithActorsDto
            {
                Id = movie.Id,
                Title = movie.Title,
                Description = movie.Description,
                ReleaseDate = movie.ReleaseDate,
                DurationMinutes = movie.DurationMinutes,
                Rating = movie.Rating,
                ActorNames = actors,
                FileUrl = movie.Url
            };

            return Result<MovieWithActorsDto>.Success(dto);
        }
    }
}
