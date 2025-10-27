using MediatR;
using Microsoft.EntityFrameworkCore;
using MovieReviewApi.Application.DTOs;
using MovieReviewApi.Application.Interfaces;
using MovieReviewApi.Application.Queries.Actor;
using MovieReviewApi.Domain.Common.Actors;

namespace MovieReviewApi.Application.Handlers.Actor
{
    public class GetActorByIdHandler : IRequestHandler<GetActorByIdQuery, Result<ActorWithMoviesDto>> {
        private readonly IApplicationDbContext _context;

        public GetActorByIdHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result<ActorWithMoviesDto>> Handle(GetActorByIdQuery request,CancellationToken cancellationToken) {
            var actor = await _context.Actors.AsNoTracking().Include(m => m.Movies).FirstOrDefaultAsync(a => a.Id == request.Id);

            if (actor == null) return Result<ActorWithMoviesDto>.Failure(ActorErrors.NotFound);


            var dto = new ActorWithMoviesDto
            {
                Id = actor.Id,
                Name = actor.Name,
                Bio = actor.Bio!,
                DateOfBirth = actor.DateOfBirth,
                MovieTitles = string.IsNullOrWhiteSpace(actor.MovieTitlesCache)
                ? new List<string>()
                : actor.MovieTitlesCache.Split(',', StringSplitOptions.RemoveEmptyEntries)
                                        .Select(t => t.Trim())
                                        .ToList()
            };

            return Result<ActorWithMoviesDto>.Success(dto);

        }

    }
}
