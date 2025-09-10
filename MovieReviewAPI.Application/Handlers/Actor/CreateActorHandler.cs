using MediatR;
using Microsoft.EntityFrameworkCore;
using MovieReviewApi.Application.Commands.Actor;
using MovieReviewApi.Application.DTOs;
using MovieReviewApi.Application.Interfaces;
using MovieReviewApi.Domain.Common.Actors;
using MovieReviewApi.Domain.Entities;

namespace MovieReviewApi.Application.Handlers.Actor
{
    public class CreateActorHandler : IRequestHandler<CreateActorCommand, Result<ActorDto>>
    {
        private readonly IApplicationDbContext _context;

        public CreateActorHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result<ActorDto>> Handle(CreateActorCommand request, CancellationToken cancellationToken)
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
                    return Result<ActorDto>.Failure(ActorErrors.MoviesNotFound(invalidIds));
                }

                actor.Movies = movies;
            }

            await _context.Actors.AddAsync(actor, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            var actorDto = new ActorDto
            {
                Id = actor.Id,
                Name = actor.Name,
                Bio = actor.Bio,
                DateOfBirth = actor.DateOfBirth,
                Movies = actor.Movies?.Select(m => m.Title).ToList() ?? new List<string>()
            };

            return Result<ActorDto>.Success(actorDto);
        }
    }
}
