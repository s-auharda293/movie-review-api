using Dapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MovieReviewApi.Application.DTOs;
using MovieReviewApi.Application.Interfaces;
using MovieReviewApi.Application.KeylessEntities;
using MovieReviewApi.Application.Queries.Movie;
using System.Data;

namespace MovieReviewApi.Application.Handlers.Movie
{
    public class SearchMoviesHandler : IRequestHandler<SearchMoviesQuery, Result<MovieResponseDto>>
    {
        private readonly IDbConnectionFactory _connection;

        public SearchMoviesHandler(IDbConnectionFactory connection)
        {
            _connection = connection;
        }

        public async Task<Result<MovieResponseDto>> Handle(SearchMoviesQuery request, CancellationToken cancellationToken)
        {
            var connection = await _connection.CreateConnectionAsync(cancellationToken);

            var parameters = new DynamicParameters();
            parameters.Add("SearchTerm", request.request.SearchTerm, DbType.String);
            parameters.Add("SearchColumn", request.request.SearchColumn, DbType.String);
            parameters.Add("Page", request.request.Page, DbType.Int32);
            parameters.Add("PageSize", request.request.PageSize, DbType.Int32);
            parameters.Add("SortColumn", request.request.SortColumn, DbType.String);
            parameters.Add("SortDirection", request.request.SortDirection, DbType.String);

            using var multi = await connection.QueryMultipleAsync(
                "SearchMovies",
                parameters,
                commandType: CommandType.StoredProcedure);

            var totalCount = multi.ReadSingle<int>();

            
            var movies = multi.Read<GetMoviesResult>().ToList();

            var movieDtos = movies.Select(m =>
            {

                var actorNames = m.ActorNamesCache?
                              .Split(',', StringSplitOptions.RemoveEmptyEntries)
                              .Select(n => n.Trim())
                              .ToList() ?? new List<string>();

                return new MovieWithActorsDto
                {
                    Id = m.Id,
                    Title = m.Title,
                    Description = m.Description ?? "",
                    ReleaseDate = m.ReleaseDate,
                    DurationMinutes = m.DurationMinutes,
                    Rating = m.Rating,
                    FileUrl = m.Url,
                    ActorNames = actorNames
                };
            }).ToList();

            var response = new MovieResponseDto
            {
                Movies = movieDtos,
                TotalCount = totalCount
            };

            return Result<MovieResponseDto>.Success(response);
        }
    }
}
