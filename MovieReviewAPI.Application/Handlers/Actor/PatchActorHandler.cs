using MediatR;
using Microsoft.EntityFrameworkCore;
using MovieReviewApi.Application.Commands.Actor;
using MovieReviewApi.Application.DTOs;
using MovieReviewApi.Application.Interfaces;

namespace MovieReviewApi.Application.Handlers.Actor
{
    public class PatchActorHandler : IRequestHandler<UpdateActorCommand, bool>
    {
        private readonly IApplicationDbContext _context;

        public PatchActorHandler(IApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<bool> Handle(UpdateActorCommand request, CancellationToken cancellationToken)
        {
            var actor = await _context.Actors.FirstOrDefaultAsync(a=>a.Id == request.Id);
            if (actor == null) return false;

            if (!string.IsNullOrEmpty(request.dto.Name)) actor.Name = request.dto.Name;
            if (request.dto.DateOfBirth.HasValue) actor.DateOfBirth = request.dto.DateOfBirth;
            if (!string.IsNullOrEmpty(request.dto.Bio)) actor.Bio = request.dto.Bio;

            if (request.dto.MovieIds != null && request.dto.MovieIds.Any())
            {
                var movies = await _context.Movies.Where(m=>request.dto.MovieIds.Contains(m.Id)).ToListAsync(cancellationToken);
                if (movies.ToList().Count != request.dto.MovieIds.Count)
                {
                    var invalidIds = request.dto.MovieIds.Except((movies.Select(m => m.Id).ToList())).ToList();
                    throw new ArgumentException($"One or more movies with Ids {string.Join(", ", invalidIds)} do not exist.");
                }
                actor.Movies = movies.ToList();
            }
            _context.Actors.Update(actor);
            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
