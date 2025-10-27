using MediatR;
using Microsoft.EntityFrameworkCore;
using MovieReviewApi.Application.DTOs;
using MovieReviewApi.Application.Interfaces;
using MovieReviewApi.Application.KeylessEntities;
using MovieReviewApi.Application.Queries.Movie;

namespace MovieReviewApi.Application.Handlers.Actor
{
    public class GetMoviesHandler : IRequestHandler<GetMoviesQuery, Result<IEnumerable<MovieWithActorsDto>>>
    {
        private readonly IApplicationDbContext _context;
        public GetMoviesHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result<IEnumerable<MovieWithActorsDto>>> Handle(GetMoviesQuery request, CancellationToken cancellationToken)
        {
            var movies = await _context.GetMoviesResult
                .FromSqlRaw("EXEC GetMovies")
                .ToListAsync(cancellationToken);

            var movieDtos = new List<MovieWithActorsDto>();

            foreach (var movie in movies)
            {
                var actorIds = movie.ActorIds
                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(id => Guid.Parse(id.Trim()))
                    .ToList();

                var actors = await _context.Actors
                    .Where(a => actorIds.Contains(a.Id))
                    .Select(a =>  a.Name)
                    .ToListAsync(cancellationToken);

                movieDtos.Add(new MovieWithActorsDto
                {
                    Id = movie.Id,
                    Title = movie.Title,
                    Description = movie.Description ?? "",
                    ReleaseDate = movie.ReleaseDate,
                    DurationMinutes = movie.DurationMinutes,
                    Rating = movie.Rating,
                    ActorNames = actors,
                    FileUrl = movie.Url
                });
            }

            return Result<IEnumerable<MovieWithActorsDto>>.Success(movieDtos);
        }
    }
}
