using MediatR;
using Microsoft.EntityFrameworkCore;
using MovieReviewApi.Application.Commands.Actor;
using MovieReviewApi.Application.DTOs;
using MovieReviewApi.Application.Interfaces;
using MovieReviewApi.Domain.Common.Actors;

namespace MovieReviewApi.Application.Handlers.Actor
{
    public class DeleteActorHandler : IRequestHandler<DeleteActorCommand, Result<bool>>
    {
        private readonly IApplicationDbContext _context;
        public DeleteActorHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Result<bool>> Handle(DeleteActorCommand request, CancellationToken cancellationToken)
        {
                var actor = await _context.Actors.Include(a => a.Movies).FirstOrDefaultAsync(a=>a.Id == request.Id);
                if (actor == null) return Result<bool>.Failure(ActorErrors.NotFound);

                _context.Actors.Remove(actor);
                await _context.SaveChangesAsync(cancellationToken); //when we do this EF Core deletes the rows in the join table not the movies
                return Result<bool>.Success(true);
            
        }
    }
}
