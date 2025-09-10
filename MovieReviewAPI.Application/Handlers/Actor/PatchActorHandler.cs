using MediatR;
using Microsoft.EntityFrameworkCore;
using MovieReviewApi.Application.Commands.Actor;
using MovieReviewApi.Application.DTOs;
using MovieReviewApi.Application.Interfaces;
using MovieReviewApi.Domain.Common.Actors;

namespace MovieReviewApi.Application.Handlers.Actor
{
    public class PatchActorHandler : IRequestHandler<PatchActorCommand,Result<ActorDto>>
    {
        private readonly IApplicationDbContext _context;

        public PatchActorHandler(IApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<Result<ActorDto>> Handle(PatchActorCommand request, CancellationToken cancellationToken)
        {
            var actor = await _context.Actors.FirstOrDefaultAsync(a=>a.Id == request.Id);
            if (actor == null) return Result<ActorDto>.Failure(ActorErrors.NotFound);

            if (!string.IsNullOrEmpty(request.dto.Name)) actor.Name = request.dto.Name;
            if (request.dto.DateOfBirth.HasValue) actor.DateOfBirth = request.dto.DateOfBirth;
            if (!string.IsNullOrEmpty(request.dto.Bio)) actor.Bio = request.dto.Bio;

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

            var dto = new ActorDto
            {
                Id = actor.Id,
                Name = actor.Name,
                Bio = actor.Bio,
                DateOfBirth = actor.DateOfBirth,
                Movies = actor.Movies?.Select(m => m.Title).ToList() ?? new List<string>()
            };

            return Result<ActorDto>.Success(dto);
        }
    }
}
