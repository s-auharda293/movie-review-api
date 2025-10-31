using Dapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MovieReviewApi.Application.Commands.Movie;
using MovieReviewApi.Application.DTOs;
using MovieReviewApi.Application.Interfaces;
using MovieReviewApi.Domain.Common.Movies;
using System;
using System.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace MovieReviewApi.Application.Handlers.Movie
{
    public class PatchMovieHandler:IRequestHandler<PatchMovieCommand,Result<MovieWithActorsDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IDbConnectionFactory _connection;
        private readonly IFileStorageService _fileStorageService;

        public PatchMovieHandler(IApplicationDbContext context, IDbConnectionFactory connection, IFileStorageService fileStorageService)
        {
            _context = context;
            _connection = connection;
            _fileStorageService = fileStorageService;
        }

        public async Task<Result<MovieWithActorsDto>> Handle(PatchMovieCommand request, CancellationToken cancellationToken) {
            string? actorIdsCsv = null;
            List<string> actorNames = new();
            string? newUrl = null;

           var movie = await _context.Movies.Include(m=>m.Actors).FirstOrDefaultAsync(m=>m.Id==request.Id);
            if (movie == null) return Result<MovieWithActorsDto>.Failure(MovieErrors.NotFound);

            if (request.dto.ActorIds != null && request.dto.ActorIds.Any())
            {
                var actors = await _context.Actors.Where(a=>request.dto.ActorIds.Contains(a.Id)).ToListAsync(cancellationToken);
                if (actors.ToList().Count != request.dto.ActorIds.Count)
                {
                    var invalidIds = request.dto.ActorIds.Except(actors.Select(a => a.Id).ToList());
                    return Result<MovieWithActorsDto>.Failure(MovieErrors.ActorsNotFound(invalidIds));
                }
                actorIdsCsv = string.Join(",", request.dto.ActorIds);
                actorNames = actors.Select(a =>a.Name).ToList();

            }


            if (actorNames.Count == 0)
            {
                actorNames = movie.Actors.Select(a => a.Name ).ToList();
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

            if (request.dto.File != null && request.dto.File.Length != 0)
            {
                using var stream = request.dto.File.OpenReadStream();
                newUrl = await _fileStorageService.UpdateFileAsync(stream, request.Id, request.dto.File.FileName, "local");
                //newUrl = await _fileStorageService.UpdateFileAsync(stream, request.Id, request.dto.File.FileName, "minio");
            }

                parameters.Add("@Url", newUrl, DbType.String);

            var patchMovie = await connection.QueryFirstAsync<dynamic>(
                "PatchMovie",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            if (request.dto.File == null || request.dto.File.Length != 0)
            {
                newUrl = movie.Url;
            }


            if (patchMovie == null)
            {
                return Result<MovieWithActorsDto>.Failure(MovieErrors.NotFound);
            }

            var dto = new MovieWithActorsDto
            {
                Id = patchMovie.Id,
                Title = patchMovie.Title,
                Description = patchMovie.Description,
                ReleaseDate = patchMovie.ReleaseDate,
                DurationMinutes = patchMovie.DurationMinutes,
                Rating = patchMovie.Rating,
                ActorNames = actorNames,
                FileUrl = newUrl,
            };

            return Result<MovieWithActorsDto>.Success(dto);

        }
    }
}
