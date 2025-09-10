using MediatR;
using Microsoft.EntityFrameworkCore;
using MovieReviewApi.Application.Commands.Actor;
using MovieReviewApi.Application.DTOs;
using MovieReviewApi.Application.Interfaces;
using MovieReviewApi.Domain.Common.Actors;

namespace MovieReviewApi.Application.Handlers.Actor
{
    public class UpdateActorHandler : IRequestHandler<UpdateActorCommand, Result<ActorDto>>
    {
        private readonly IApplicationDbContext _context;
        public UpdateActorHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result<ActorDto>> Handle(UpdateActorCommand request, CancellationToken cancellationToken)
        {
            var actor = await _context.Actors.FirstOrDefaultAsync(a=>a.Id==request.Id,cancellationToken);
        if (actor == null) return Result<ActorDto>.Failure(ActorErrors.NotFound);

            actor.Name = request.dto.Name;
            actor.Bio = request.dto.Bio;
            actor.DateOfBirth = request.dto.DateOfBirth;

            if (request.dto.MovieIds != null && request.dto.MovieIds.Any())
            {
                var movies = await _context.Movies.Where(m=>request.dto.MovieIds.Contains(m.Id)).ToListAsync(cancellationToken);
                if (movies.Count != request.dto.MovieIds.Count)
                {
                    var invalidIds = request.dto.MovieIds.Except(movies.Select(m => m.Id)).ToList();
                    return Result<ActorDto>.Failure(ActorErrors.MoviesNotFound(invalidIds));
                }
                actor.Movies = movies.ToList();
            }


            _context.Actors.Update(actor);
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
