using MediatR;
using MovieReviewApi.Application.DTOs;
using MovieReviewApi.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using MovieReviewApi.Application.KeylessEntities;
using MovieReviewApi.Application.Queries.Movie;

namespace MovieReviewApi.Application.Handlers.Actor
{
    public class GetMoviesHandler : IRequestHandler<GetMoviesQuery, Result<IEnumerable<MovieDto>>>
    {
        private readonly IApplicationDbContext _context;
        public GetMoviesHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result<IEnumerable<MovieDto>>> Handle(GetMoviesQuery request, CancellationToken cancellationToken)
        {
            // Call the stored procedure into your keyless entity
            var movies = await _context.GetMoviesResult
                .FromSqlRaw("EXEC GetMovies")
                .ToListAsync(cancellationToken);



            // Map the results to DTOs
            var movieDtos = movies.Select(m => new MovieDto
            {
                Id = m.Id,
                Title = m.Title,
                Description = m.Description ?? "",
                ReleaseDate = m.ReleaseDate,
                DurationMinutes = m.DurationMinutes,
                Rating = m.Rating,
                Actors = string.IsNullOrWhiteSpace(m.ActorNames)
                ? new List<string>()
                : m.ActorNames.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries).ToList()
            }).ToList();

            return Result<IEnumerable<MovieDto>>.Success(movieDtos);
        }
    }
}
