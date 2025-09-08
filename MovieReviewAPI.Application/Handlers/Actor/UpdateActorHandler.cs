using MediatR;
using Microsoft.EntityFrameworkCore;
using MovieReviewApi.Application.Commands.Actor;
using MovieReviewApi.Application.Interfaces;

namespace MovieReviewApi.Application.Handlers.Actor
{
    public class UpdateActorHandler : IRequestHandler<UpdateActorCommand, bool>
    {
        private readonly IApplicationDbContext _context;
        public UpdateActorHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(UpdateActorCommand request, CancellationToken cancellationToken)
        {
            var actor = await _context.Actors.FirstOrDefaultAsync(a=>a.Id==request.Id,cancellationToken);
        if (actor == null) return false;

            actor.Name = request.dto.Name;
            actor.Bio = request.dto.Bio;
            actor.DateOfBirth = request.dto.DateOfBirth;

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
