using MediatR;
using Microsoft.EntityFrameworkCore;
using MovieReviewApi.Application.DTOs;
using MovieReviewApi.Application.Interfaces;
using MovieReviewApi.Application.Queries.Movie;
using MovieReviewApi.Domain.Common.Movies;

namespace MovieReviewApi.Application.Handlers.Movie
{
    public class GetMovieByIdHandler:IRequestHandler<GetMovieByIdQuery,Result<MovieDto>>
    {
        private readonly IApplicationDbContext _context;

        public GetMovieByIdHandler(IApplicationDbContext context)
        {
            _context = context;   
        }

        public async Task<Result<MovieDto>> Handle(GetMovieByIdQuery request, CancellationToken cancellationToken)
        {
            var movie = await _context.Movies.Include(m => m.Actors).FirstOrDefaultAsync(m => m.Id == request.Id,cancellationToken);
            if (movie == null) return Result<MovieDto>.Failure(MovieErrors.NotFound);
            var dto = new MovieDto
            {
                Id = movie.Id,
                Title = movie.Title,
                Description = movie.Description,
                ReleaseDate = movie.ReleaseDate,
                DurationMinutes = movie.DurationMinutes,
                Rating = movie.Rating,
                Actors = movie.Actors.Select(a => a.Name).ToList(),
            };

            return Result<MovieDto>.Success(dto);
        }
    }
}
