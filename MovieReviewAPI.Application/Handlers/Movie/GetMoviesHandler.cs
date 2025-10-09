using MediatR;
using Microsoft.EntityFrameworkCore;
using MovieReviewApi.Application.DTOs;
using MovieReviewApi.Application.Interfaces;
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
            var movies = await _context.GetMoviesResult
                .FromSqlRaw("EXEC GetMovies")
                .ToListAsync(cancellationToken);

            var movieDtos = new List<MovieDto>();

            foreach (var movie in movies)
            {
                var actorIds = movie.ActorIds
                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(id => Guid.Parse(id.Trim()))
                    .ToList();

                var actors = await _context.Actors
                    .Where(a => actorIds.Contains(a.Id))
                    .Select(a => new MovieActorDto
                    {
                        Id = a.Id,
                        Name = a.Name
                    })
                    .ToListAsync(cancellationToken);

                movieDtos.Add(new MovieDto
                {
                    Id = movie.Id,
                    Title = movie.Title,
                    Description = movie.Description ?? "",
                    ReleaseDate = movie.ReleaseDate,
                    DurationMinutes = movie.DurationMinutes,
                    Rating = movie.Rating,
                    Actors = actors
                });
            }

            return Result<IEnumerable<MovieDto>>.Success(movieDtos);
        }
    }
}
