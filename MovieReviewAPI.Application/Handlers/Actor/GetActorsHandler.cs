using MediatR;
using MovieReviewApi.Application.DTOs;
using MovieReviewApi.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using MovieReviewApi.Application.Queries.Actor;

namespace MovieReviewApi.Application.Handlers.Actor
{
    public class GetActorsHandler: IRequestHandler<GetActorsQuery, Result<IEnumerable<ActorDto>>>
    {
        private readonly IApplicationDbContext _context;
        public GetActorsHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result<IEnumerable<ActorDto>>> Handle(GetActorsQuery request, CancellationToken cancellationToken) {
            var actors = await _context.Actors.Include(a => a.Movies).ToListAsync(cancellationToken);
            var actorDtos = actors.Select(a => new ActorDto
            {
                Id = a.Id,
                Name = a.Name,
                Bio = a.Bio,
                DateOfBirth = a.DateOfBirth,
                Movies = a.Movies?
                .Select(m => new ActorMovieDto { Id = m.Id, Title = m.Title })
                .ToList() ?? new List<ActorMovieDto>()
            });

            return Result<IEnumerable<ActorDto>>.Success(actorDtos);
        }
    }
}