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
            List<ActorMovieDto> movieEntities = new List<ActorMovieDto>();

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

                movieEntities = await _context.Movies
                    .Where(m => request.dto.MovieIds.Contains(m.Id))
                    .Select(m => new ActorMovieDto
                    {
                        Id = m.Id,
                        Title = m.Title,
                    })
                    .ToListAsync(cancellationToken);
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

            updatedActor.Movies = movieEntities;

            return Result<ActorDto>.Success(updatedActor);
        }
    }
}


// Alternative PATCH handler for adding movies instead of replacing all MovieIds.
// Includes duplicate checks to prevent associating movies already linked to the actor.
//using Dapper;
//using MediatR;
//using Microsoft.EntityFrameworkCore;
//using MovieReviewApi.Application.Commands.Actor;
//using MovieReviewApi.Application.DTOs;
//using MovieReviewApi.Application.Interfaces;
//using MovieReviewApi.Domain.Common.Actors;
//using System.Data;

//namespace MovieReviewApi.Application.Handlers.Actor
//{
//    public class PatchActorHandler : IRequestHandler<PatchActorCommand, Result<ActorDto>>
//    {
//        private readonly IApplicationDbContext _context;
//        private readonly IDbConnectionFactory _connection;

//        public PatchActorHandler(IApplicationDbContext context, IDbConnectionFactory connection)
//        {
//            _context = context;
//            _connection = connection;
//        }

//        public async Task<Result<ActorDto>> Handle(PatchActorCommand request, CancellationToken cancellationToken)
//        {
//            string? movieIdsCsv = null;
//            List<string> movieTitles = new();

//            // Fetch actor including movies
//            var actor = await _context.Actors
//                .Include(a => a.Movies)
//                .FirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken);

//            if (actor == null)
//                return Result<ActorDto>.Failure(ActorErrors.NotFound);

//            if (request.dto.MovieIds != null && request.dto.MovieIds.Any())
//            {
//                // Remove duplicates from request
//                request.dto.MovieIds = request.dto.MovieIds.Distinct().ToList();

//                var associatedMovieIds = actor?.Movies?.Select(m => m.Id).ToList();

//                // Fetch all requested movies from DB
//                var movies = await _context.Movies
//                    .Where(m => request.dto.MovieIds.Contains(m.Id))
//                    .ToListAsync(cancellationToken);

//                // Check for invalid movie IDs
//                if (movies.Count != request.dto.MovieIds.Count)
//                {
//                    var invalidIds = request.dto.MovieIds.Except(movies.Select(m => m.Id)).ToList();
//                    return Result<ActorDto>.Failure(ActorErrors.MoviesNotFound(invalidIds));
//                }

//                // Check for movies already associated with actor
//                var duplicateIds = request.dto.MovieIds.Intersect(associatedMovieIds!).ToList();
//                if (duplicateIds.Any())
//                    return Result<ActorDto>.Failure(ActorErrors.AlreadyExistsForMovie(duplicateIds));

//                // Add movie titles for response
//                movieTitles = movies.Select(m => m.Title).ToList();
//                movieIdsCsv = request.dto.MovieIds != null ? string.Join(",", request.dto.MovieIds) : null;
//            }

//            // Prepare parameters for stored procedure
//            using var connection = await _connection.CreateConnectionAsync(cancellationToken);
//            var parameters = new DynamicParameters();
//            parameters.Add("@Id", request.Id, DbType.Guid);
//            parameters.Add("@Name", request.dto.Name, DbType.String);
//            parameters.Add("@Bio", request.dto.Bio, DbType.String);
//            parameters.Add("@DateOfBirth", request.dto.DateOfBirth, DbType.DateTime2);
//            parameters.Add("@MovieIds", movieIdsCsv, DbType.String);

//            var updatedActor = await connection.QueryFirstOrDefaultAsync<ActorDto>(
//                "PatchActor",
//                parameters,
//                commandType: CommandType.StoredProcedure
//            );

//            if (updatedActor == null)
//                return Result<ActorDto>.Failure(ActorErrors.NotFound);

//            // Set movies in response
//            updatedActor.Movies = movieTitles;

//            return Result<ActorDto>.Success(updatedActor);
//        }
//    }
//}
