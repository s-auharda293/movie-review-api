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
    public class UpdateActorHandler : IRequestHandler<UpdateActorCommand, Result<ActorWithMoviesDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IDbConnectionFactory _connection;
        public UpdateActorHandler(IApplicationDbContext context, IDbConnectionFactory connection)
        {
            _context = context;
            _connection = connection;
        }

        public async Task<Result<ActorWithMoviesDto>> Handle(UpdateActorCommand request, CancellationToken cancellationToken)
        {

            var connection = await _connection.CreateConnectionAsync(cancellationToken);
            string? movieIdsCsv = null;
            List<string> movieTitles = new();

            if (request.dto.MovieIds != null && request.dto.MovieIds.Any())
            {
                var movies = await _context.Movies.Where(m => request.dto.MovieIds.Contains(m.Id)).ToListAsync(cancellationToken);
                if (movies.Count != request.dto.MovieIds.Count)
                {
                    var invalidIds = request.dto.MovieIds.Except(movies.Select(m => m.Id)).ToList();
                    return Result<ActorWithMoviesDto>.Failure(ActorErrors.MoviesNotFound(invalidIds));
                }
                movieIdsCsv = string.Join(",", request.dto.MovieIds);

                movieTitles = await _context.Movies
                     .Where(m => request.dto.MovieIds.Contains(m.Id))
                     .Select(m => m.Title)
                     .ToListAsync(cancellationToken);
            }

            var exists = await _context.Actors.FirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken);
            if (exists == null) return Result<ActorWithMoviesDto>.Failure(ActorErrors.NotFound);

            // Prepare parameters
            var parameters = new DynamicParameters();
            parameters.Add("@Id", request.Id, DbType.Guid);
            parameters.Add("@Name", request.dto.Name, DbType.String);
            parameters.Add("@Bio", request.dto.Bio, DbType.String);
            parameters.Add("@DateOfBirth", request.dto.DateOfBirth, DbType.DateTime2);
            parameters.Add("@MovieIds", movieIdsCsv, DbType.String);

         
            var updatedActor = await connection.QueryFirstAsync<dynamic>(
              "UpdateActor",
              parameters,
              commandType: CommandType.StoredProcedure
          );

            var actorDto = new ActorWithMoviesDto
            {
                Id = updatedActor.Id,
                Name = updatedActor.Name,
                Bio = updatedActor.Bio,
                DateOfBirth = updatedActor.DateOfBirth,
                MovieTitles = movieTitles?? new List<String>()
            };

            return Result<ActorWithMoviesDto>.Success(actorDto);

        }
    }
}
