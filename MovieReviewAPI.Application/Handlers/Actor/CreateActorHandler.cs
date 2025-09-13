using MediatR;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MovieReviewApi.Application.Commands.Actor;
using MovieReviewApi.Application.DTOs;
using MovieReviewApi.Application.Interfaces;
using MovieReviewApi.Application.KeylessEntities.CreateActorResult;
using MovieReviewApi.Domain.Common.Actors;
using System.Data;

namespace MovieReviewApi.Application.Handlers.Actor
{
    public class CreateActorHandler : IRequestHandler<CreateActorCommand, Result<ActorDto>>
    {
        private readonly IApplicationDbContext _context;

        public CreateActorHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result<ActorDto>> Handle(CreateActorCommand request, CancellationToken cancellationToken)
        {
            var dto = request.dto;

            // Call stored procedure and get the created actor
            var spResult = await _context.Set<CreateActorResult>()
                .FromSqlInterpolated($@"
            EXEC CreateActor 
                @Bio = {dto.Bio ?? (object)DBNull.Value}, 
                @DateOfBirth = {dto.DateOfBirth ?? (object)DBNull.Value}, 
                @Name = {dto.Name}
        ")
                .AsNoTracking()
                .ToListAsync(cancellationToken); // executes the SQL

            var actor = spResult.FirstOrDefault();

            if (actor == null)
                return Result<ActorDto>.Failure(ActorErrors.CreationFailed);

            // Map to DTO
            var actorDto = new ActorDto
            {
                Id = actor.Id,
                Name = actor.Name,
                Bio = actor.Bio,
                DateOfBirth = actor.DateOfBirth,
                Movies = new List<string>() // empty at creation
            };

            return Result<ActorDto>.Success(actorDto);
        }
    }
}