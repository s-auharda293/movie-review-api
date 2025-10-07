using Dapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MovieReviewApi.Application.Commands.Movie;
using MovieReviewApi.Application.DTOs;
using MovieReviewApi.Application.Interfaces;
using MovieReviewApi.Domain.Common.Movies;
using System.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace MovieReviewApi.Application.Handlers.Movie
{
    public class PatchMovieHandler:IRequestHandler<PatchMovieCommand,Result<MovieDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IDbConnectionFactory _connection;

        public PatchMovieHandler(IApplicationDbContext context, IDbConnectionFactory connection)
        {
            _context = context;
            _connection = connection;
        }

        public async Task<Result<MovieDto>> Handle(PatchMovieCommand request, CancellationToken cancellationToken) {
            string? actorIdsCsv = null;
            List<MovieActorDto> actorEntities = new();
            var movie = await _context.Movies.Include(m=>m.Actors).FirstOrDefaultAsync(m=>m.Id==request.Id);
            if (movie == null) return Result<MovieDto>.Failure(MovieErrors.NotFound);

            if (request.dto.ActorIds != null && request.dto.ActorIds.Any())
            {
                var actors = await _context.Actors.Where(a=>request.dto.ActorIds.Contains(a.Id)).ToListAsync(cancellationToken);
                if (actors.ToList().Count != request.dto.ActorIds.Count)
                {
                    var invalidIds = request.dto.ActorIds.Except(actors.Select(a => a.Id).ToList());
                    return Result<MovieDto>.Failure(MovieErrors.ActorsNotFound(invalidIds));
                }
                actorIdsCsv = string.Join(",", request.dto.ActorIds);
                actorEntities = actors.Select(a => new MovieActorDto
                {
                    Id = a.Id,
                    Name = a.Name
                }).ToList();
            }


            var connection = await _connection.CreateConnectionAsync(cancellationToken);

            var parameters = new DynamicParameters();
            parameters.Add("@Id", request.Id, DbType.Guid);
            parameters.Add("@Title", request.dto.Title, DbType.String);
            parameters.Add("@Description", request.dto.Description, DbType.String);
            parameters.Add("@ReleaseDate", request.dto.ReleaseDate, DbType.DateTime2);
            parameters.Add("@DurationMinutes", request.dto.DurationMinutes, DbType.Int32);
            parameters.Add("@Rating", request.dto.Rating, DbType.Decimal);
            parameters.Add("@ActorIds", actorIdsCsv, DbType.String);


            var patchMovie = await connection.QueryFirstAsync<dynamic>(
                "PatchMovie",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            var dto = new MovieDto
            {
                Id = patchMovie.Id,
                Title = patchMovie.Title,
                Description = patchMovie.Description,
                ReleaseDate = patchMovie.ReleaseDate,
                DurationMinutes = patchMovie.DurationMinutes,
                Rating = patchMovie.Rating,
                Actors = actorEntities,
            };

            return Result<MovieDto>.Success(dto);

        }
    }
}
