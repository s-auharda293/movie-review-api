using MediatR;
using Microsoft.EntityFrameworkCore;
using MovieReviewApi.Application.DTOs;
using MovieReviewApi.Application.Interfaces;
using MovieReviewApi.Application.Queries.Actor;

namespace MovieReviewApi.Application.Handlers.Actor
{
    public class GetMoviesByIdsHandler:IRequestHandler<GetActorsByIdsQuery,IEnumerable<ActorDto>>
    {
        private readonly IApplicationDbContext _context;
        public GetMoviesByIdsHandler(IApplicationDbContext context)
        {
            _context = context;   
        }

        public async Task<IEnumerable<ActorDto>> Handle(GetActorsByIdsQuery request, CancellationToken cancellationToken) {
           var actors = await _context.Actors.Where(a => request.Ids.Contains(a.Id)).ToListAsync(cancellationToken);

           var actorDtos = actors.Select(a => new ActorDto
            {
                Id = a.Id,
                Name = a.Name,
                Bio = a.Bio,
                DateOfBirth = a.DateOfBirth,
                Movies = a.Movies?.Select(m => m.Title).ToList() ?? new List<string>(),
            });
            return actorDtos;

        }
    }
}
