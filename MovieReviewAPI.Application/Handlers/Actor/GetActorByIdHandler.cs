using MediatR;
using Microsoft.EntityFrameworkCore;
using MovieReviewApi.Application.DTOs;
using MovieReviewApi.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public record GetActorByIdQuery(Guid Id) : IRequest<ActorDto>;

namespace MovieReviewApi.Application.Handlers.Actor
{
    public class GetActorByIdHandler : IRequestHandler<GetActorByIdQuery, ActorDto> {
        private readonly IApplicationDbContext _context;

        public GetActorByIdHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ActorDto> Handle(GetActorByIdQuery request,CancellationToken cancellationToken) {
            var actor = await _context.Actors.FirstOrDefaultAsync(a => a.Id == request.Id);

            if (actor == null) return null;

            return new ActorDto
            {
                Id = actor.Id,
                Name = actor.Name,
                Bio = actor.Bio,
                DateOfBirth = actor.DateOfBirth,
                Movies = actor.Movies?.Select(m=>m.Title).ToList()??new List<string>()
            };

        }

    }
}
