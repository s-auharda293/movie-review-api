using Dapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MovieReviewApi.Application.Commands.Actor;
using MovieReviewApi.Application.DTOs;
using MovieReviewApi.Application.Interfaces;
using MovieReviewApi.Domain.Common.Actors;
using System.Data;

namespace MovieReviewApi.Application.Handlers.Actor
{
    public class PatchActorHandler : IRequestHandler<PatchActorCommand,Result<ActorDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IDbConnectionFactory _connection;

        public PatchActorHandler(IApplicationDbContext context, IDbConnectionFactory connection)
        {
            _context = context;
            _connection = connection;
        }


        public async Task<Result<ActorDto>> Handle(PatchActorCommand request, CancellationToken cancellationToken)
        {
            string? movieIdsCsv = null;

            var actor = await _context.Actors.FirstOrDefaultAsync(a=>a.Id == request.Id);
            if (actor == null) return Result<ActorDto>.Failure(ActorErrors.NotFound);

            if (request.dto.MovieIds != null && request.dto.MovieIds.Any())
            {
                var movies = await _context.Movies.Where(m=>request.dto.MovieIds.Contains(m.Id)).ToListAsync(cancellationToken);
                if (movies.Count != request.dto.MovieIds.Count)
                {
                    var invalidIds = request.dto.MovieIds.Except(movies.Select(m => m.Id)).ToList();
                    return Result<ActorDto>.Failure(ActorErrors.MoviesNotFound(invalidIds));
                }
                movieIdsCsv = string.Join(",", request.dto.MovieIds);
            }
            using var connection = await _connection.CreateConnectionAsync(cancellationToken);

            var parameters = new DynamicParameters();
            parameters.Add("@Id", request.Id, DbType.Guid);
            parameters.Add("@Name", request.dto.Name, DbType.String);
            parameters.Add("@Bio", request.dto.Bio, DbType.String);
            parameters.Add("@DateOfBirth", request.dto.DateOfBirth, DbType.DateTime2);
            parameters.Add("@MovieIds", movieIdsCsv, DbType.String);

            // Call stored procedure
            var updatedActor = await connection.QueryFirstOrDefaultAsync<ActorDto>(
                "PatchActor",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            if (updatedActor == null)
                return Result<ActorDto>.Failure(ActorErrors.NotFound);

            updatedActor.Movies = await _context.Movies
             .Where(m => request.dto.MovieIds == null
                      ? m.Actors.Any(a => a.Id == request.Id)
                      : request.dto.MovieIds.Contains(m.Id))
             .Select(m => m.Title)
             .ToListAsync(cancellationToken);

            return Result<ActorDto>.Success(updatedActor);
        }
    }
}