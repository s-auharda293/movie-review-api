using MediatR;
using Microsoft.EntityFrameworkCore;
using MovieReviewApi.Application.DTOs;
using MovieReviewApi.Application.Interfaces;
using MovieReviewApi.Application.Queries.Actor;
using MovieReviewApi.Domain.Common.Actors;

namespace MovieReviewApi.Application.Handlers.Actor
{
    public class GetActorByIdHandler : IRequestHandler<GetActorByIdQuery, Result<ActorDto>> {
        private readonly IApplicationDbContext _context;

        public GetActorByIdHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result<ActorDto>> Handle(GetActorByIdQuery request,CancellationToken cancellationToken) {
            var actor = await _context.Actors.Include(m => m.Movies).FirstOrDefaultAsync(a => a.Id == request.Id);

            if (actor == null) return Result<ActorDto>.Failure(ActorErrors.NotFound);

            var dto =  new ActorDto
            {
                Id = actor.Id,
                Name = actor.Name,
                Bio = actor.Bio,
                DateOfBirth = actor.DateOfBirth,
                Movies = actor.Movies?.Select(m=>m.Title).ToList()??new List<string>()
            };

            return Result<ActorDto>.Success(dto);

        }

    }
}
