using Dapper;
using MediatR;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MovieReviewApi.Application.Commands.Actor;
using MovieReviewApi.Application.DTOs;
using MovieReviewApi.Application.Interfaces;
using MovieReviewApi.Domain.Common.Actors;
using MovieReviewApi.Domain.Entities;
using System.Data;

namespace MovieReviewApi.Application.Handlers.Actor
{
    public class CreateActorHandler : IRequestHandler<CreateActorCommand, Result<ActorDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IDbConnectionFactory _connection; 

        public CreateActorHandler(IApplicationDbContext context, IDbConnectionFactory connection)
        {
            _context = context;
            _connection = connection;
        }

        public async Task<Result<ActorDto>> Handle(CreateActorCommand request, CancellationToken cancellationToken)
        {

            var connection = await _connection.CreateConnectionAsync(cancellationToken);
            List<string> movieTitles = new List<string>();
            string? movieIdsCsv = null;

            if (request.dto.MovieIds != null && request.dto.MovieIds.Any())
            {
                var movies = await _context.Movies.Where(m => request.dto.MovieIds.Contains(m.Id)).ToListAsync(cancellationToken);
                if (movies.Count != request.dto.MovieIds.Count)
                {
                    var invalidIds = request.dto.MovieIds.Except(movies.Select(m => m.Id)).ToList();
                    return Result<ActorDto>.Failure(ActorErrors.MoviesNotFound(invalidIds));
                }
                movieIdsCsv = string.Join(",", request.dto.MovieIds);
            }

            // Prepare parameters
            var parameters = new DynamicParameters();
            parameters.Add("@Name", request.dto.Name, DbType.String);
            parameters.Add("@Bio", request.dto.Bio, DbType.String);
            parameters.Add("@DateOfBirth", request.dto.DateOfBirth, DbType.DateTime2);
            parameters.Add("@MovieIds", movieIdsCsv, DbType.String);

            // Execute stored procedure and get the first row
            var actor = await connection.QueryFirstAsync<dynamic>(
                "CreateActor",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            if (request.dto.MovieIds != null && request.dto.MovieIds.Any())
            {
                movieTitles = await _context.Movies
                    .Where(m => request.dto.MovieIds.Contains(m.Id))
                    .Select(m => m.Title)
                    .ToListAsync(cancellationToken);
            }

            // Map to ActorDto
            var actorDto = new ActorDto
            {
                Id = actor.Id,
                Name = actor.Name,
                Bio = actor.Bio,
                DateOfBirth = actor.DateOfBirth,
                Movies = movieTitles
            };

            return Result<ActorDto>.Success(actorDto);
        }
    }
}