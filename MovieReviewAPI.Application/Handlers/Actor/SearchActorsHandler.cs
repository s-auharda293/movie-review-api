using Dapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MovieReviewApi.Application.DTOs;
using MovieReviewApi.Application.Interfaces;
using MovieReviewApi.Application.Queries.Actor;
using System.Data;
using System.Text.Json;

namespace MovieReviewApi.Application.Handlers.Movie
{
    public class SearchActorsHandler : IRequestHandler<SearchActorsQuery, Result<ActorResponseDto>>
    {
        private readonly IDbConnectionFactory _connection;

        public SearchActorsHandler(IDbConnectionFactory connection)
        {
            _connection = connection;
        }

        public async Task<Result<ActorResponseDto>> Handle(SearchActorsQuery request, CancellationToken cancellationToken)
        {
            var connection = await _connection.CreateConnectionAsync(cancellationToken);

            var parameters = new DynamicParameters();

            parameters.Add("SearchTerm", request.request.SearchTerm, DbType.String);
            parameters.Add("SearchColumn", request.request.SearchColumn, DbType.String);
            parameters.Add("Page", request.request.Page, DbType.Int32);
            parameters.Add("PageSize", request.request.PageSize, DbType.Int32);
            parameters.Add("SortColumn", request.request.SortColumn, DbType.String);
            parameters.Add("SortDirection", request.request.SortDirection, DbType.String);

            using (var multi = await connection.QueryMultipleAsync(
                "SearchActors",
                parameters,
                commandType: CommandType.StoredProcedure))
            {
                var totalCount = multi.ReadSingle<int>();

                var actors = multi.Read<ActorDto>().ToList();

                var actorListWithMovies = actors.Select(a => new ActorWithMoviesDto
                {
                    Id = a.Id,
                    Name = a.Name,
                    Bio = a.Bio!,
                    DateOfBirth = a.DateOfBirth,
                    MovieTitles = a.MovieTitlesCache?
                      .Split(',', StringSplitOptions.RemoveEmptyEntries)
                      .Select(title => title.Trim())
                      .ToList() ?? new List<string>()
                }).ToList();


                var response = new ActorResponseDto
                {
                    TotalCount = totalCount,
                    Actors = actorListWithMovies
                };

                return Result<ActorResponseDto>.Success(response);
            }
        }
    }
}
