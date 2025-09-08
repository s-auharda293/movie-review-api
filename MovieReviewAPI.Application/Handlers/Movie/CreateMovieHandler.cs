using MediatR;
using Microsoft.EntityFrameworkCore;
using MovieReviewApi.Application.DTOs;
using MovieReviewApi.Application.Interfaces;
using MovieReviewApi.Domain.Entities;
using MovieReviewApi.Application.Commands.Movie;

namespace MovieReviewApi.Application.Handlers.Movie
{
    public class CreateMovieHandler : IRequestHandler<CreateMovieCommand, MovieDto>
    {
        private readonly IApplicationDbContext _context;

        public CreateMovieHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<MovieDto> Handle(CreateMovieCommand request, CancellationToken cancellationToken)
        {
            var movie = new MovieReviewApi.Domain.Entities.Movie
            {
                Title = request.dto.Title,
                Description = request.dto.Description,
                ReleaseDate = request.dto.ReleaseDate ?? DateTime.UtcNow,
                DurationMinutes = request.dto.DurationMinutes,
                Rating = request.dto.Rating,
            };

            // Add actors if provided
            if (request.dto.ActorIds != null && request.dto.ActorIds.Any())
            {
                var actors = await _context.Actors
                    .Where(a => request.dto.ActorIds.Contains(a.Id))
                    .ToListAsync(cancellationToken);

                if (actors.Count != request.dto.ActorIds.Count)
                {
                    var invalidIds = request.dto.ActorIds.Except(actors.Select(a => a.Id)).ToList();
                    throw new ArgumentException($"One or more actors with Ids {string.Join(", ", invalidIds)} do not exist.");
                }

                movie.Actors = actors;
            }

            await _context.Movies.AddAsync(movie, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return new MovieDto
            {
                Id = movie.Id,
                Title = movie.Title,
                Description = movie.Description,
                ReleaseDate = movie.ReleaseDate,
                DurationMinutes = movie.DurationMinutes,
                Rating = movie.Rating,
                Actors = movie.Actors?.Select(a => a.Name).ToList() ?? new List<string>()
            };
        }
    }
}
