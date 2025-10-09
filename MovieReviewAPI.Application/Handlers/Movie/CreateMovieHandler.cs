using Dapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MovieReviewApi.Application.Commands.Movie;
using MovieReviewApi.Application.DTOs;
using MovieReviewApi.Application.Interfaces;
using MovieReviewApi.Domain.Common.Movies;
using MovieReviewApi.Domain.Entities;
using System.Data;

namespace MovieReviewApi.Application.Handlers.Movie
{
    public class CreateMovieHandler : IRequestHandler<CreateMovieCommand, Result<MovieDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IDbConnectionFactory _connection;

        public CreateMovieHandler(IApplicationDbContext context, IDbConnectionFactory connection)
        {
            _context = context;
            _connection = connection;
        }

        public async Task<Result<MovieDto>> Handle(CreateMovieCommand request, CancellationToken cancellationToken)
        {
            string? actorIdsCsv = null;
            List<MovieActorDto> actorEntities = new();

            if (request.dto.ActorIds != null && request.dto.ActorIds.Any())
            {
                var actors = await _context.Actors
                    .Where(a => request.dto.ActorIds.Contains(a.Id))
                    .ToListAsync(cancellationToken);

                if (actors.Count != request.dto.ActorIds.Count)
                {
                    var invalidIds = request.dto.ActorIds.Except(actors.Select(a => a.Id)).ToList();
                    return Result<MovieDto>.Failure(MovieErrors.ActorsNotFound(invalidIds));
                }

                actorIdsCsv = string.Join(",", request.dto.ActorIds);
                actorEntities = await _context.Actors
                    .Where(a => request.dto.ActorIds.Contains(a.Id))
                    .Select(a => new MovieActorDto
                    {
                        Id = a.Id,
                        Name = a.Name
                    })
                    .ToListAsync(cancellationToken);
            }

           var connection = await _connection.CreateConnectionAsync(cancellationToken);

            var parameters = new DynamicParameters();
            parameters.Add("@Title", request.dto.Title, DbType.String);
            parameters.Add("@Description", request.dto.Description, DbType.String);
            parameters.Add("@ReleaseDate", request.dto.ReleaseDate ?? DateTime.UtcNow, DbType.DateTime2);
            parameters.Add("@DurationMinutes", request.dto.DurationMinutes, DbType.Int32);
            parameters.Add("@Rating", request.dto.Rating, DbType.Decimal);
            parameters.Add("@ActorIds", actorIdsCsv, DbType.String);

            var movie = await connection.QueryFirstAsync<dynamic>(
                "CreateMovie",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            // 3️⃣ Map result to DTO
            var movieDto = new MovieDto
            {
                Id = movie.Id,
                Title = movie.Title,
                Description = movie.Description,
                ReleaseDate = movie.ReleaseDate,
                DurationMinutes = movie.DurationMinutes,
                Rating = movie.Rating,
                Actors = actorEntities ?? new List<MovieActorDto>()
            };

            return Result<MovieDto>.Success(movieDto);
        }
    }
}