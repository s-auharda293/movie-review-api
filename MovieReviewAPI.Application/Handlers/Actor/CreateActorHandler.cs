using MediatR;
using Microsoft.EntityFrameworkCore;
using MovieReviewApi.Application.Commands.Actor;
using MovieReviewApi.Application.DTOs;
using MovieReviewApi.Application.Interfaces;
using MovieReviewApi.Domain.Entities;

namespace MovieReviewApi.Application.Handlers.Actor
{
    public class CreateActorHandler : IRequestHandler<CreateActorCommand, ActorDto>
    {
        private readonly IApplicationDbContext _context;

        public CreateActorHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ActorDto> Handle(CreateActorCommand request, CancellationToken cancellationToken)
        {
            var actor = new MovieReviewApi.Domain.Entities.Actor
            {
                Name = request.dto.Name,
                Bio = request.dto.Bio,
                DateOfBirth = request.dto.DateOfBirth,
            };

            // Add movies if provided
            if (request.dto.MovieIds != null && request.dto.MovieIds.Any())
            {
                var movies = await _context.Movies
                    .Where(m => request.dto.MovieIds.Contains(m.Id))
                    .ToListAsync(cancellationToken);

                if (movies.Count != request.dto.MovieIds.Count)
                {
                    var invalidIds = request.dto.MovieIds.Except(movies.Select(m => m.Id)).ToList();
                    throw new ArgumentException($"One or more movies with Ids {string.Join(", ", invalidIds)} do not exist.");
                }

                actor.Movies = movies;
            }

            await _context.Actors.AddAsync(actor, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return new ActorDto
            {
                Id = actor.Id,
                Name = actor.Name,
                Bio = actor.Bio,
                DateOfBirth = actor.DateOfBirth,
                Movies = actor.Movies?.Select(m => m.Title).ToList() ?? new List<string>()
            };
        }
    }
}
