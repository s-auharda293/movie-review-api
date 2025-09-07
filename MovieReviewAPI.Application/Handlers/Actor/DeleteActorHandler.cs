using MediatR;
using Microsoft.EntityFrameworkCore;
using MovieReviewApi.Application.Commands.Actor;
using MovieReviewApi.Application.Interfaces;

namespace MovieReviewApi.Application.Handlers.Actor
{
    public class DeleteActorHandler : IRequestHandler<DeleteActorCommand, bool>
    {
        private readonly IApplicationDbContext _context;
        public DeleteActorHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<bool> Handle(DeleteActorCommand request, CancellationToken cancellationToken)
        {
                var actor = await _context.Actors.FirstOrDefaultAsync(a=>a.Id == request.Id);
                if (actor == null) return false;

                if (actor.Movies != null && actor.Movies.Any())
                {
                    actor.Movies.Clear(); //remove tracked list actor.Movies
                }

                _context.Actors.Remove(actor);
                await _context.SaveChangesAsync(cancellationToken); //when we do this EF Core deletes the rows in the join table not the movies
                return true;
            
        }
    }
}
