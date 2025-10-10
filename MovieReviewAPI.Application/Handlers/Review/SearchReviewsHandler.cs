using Dapper;
using MediatR;
using MovieReviewApi.Application.DTOs;
using MovieReviewApi.Application.Interfaces;
using MovieReviewApi.Application.Queries.Review;
using System.Data;

namespace MovieReviewApi.Application.Handlers.Review
{
    public class SearchReviewsHandler : IRequestHandler<SearchReviewsQuery, Result<ReviewResponseDto>>
    {

        private readonly IDbConnectionFactory _connection;

        public SearchReviewsHandler(IDbConnectionFactory connection)
        {
            _connection = connection;
        }
        public async Task<Result<ReviewResponseDto>> Handle(SearchReviewsQuery request, CancellationToken cancellationToken)
        {

            var connection = await _connection.CreateConnectionAsync(cancellationToken);

            var parameters = new DynamicParameters();
            parameters.Add("@Page", request.request.Page, DbType.Int32);
            parameters.Add("@PageSize", request.request.PageSize, DbType.Int32);
            parameters.Add("@SortColumn", request.request.Sort?.Field, DbType.String);
            parameters.Add("@SortDir", request.request.Sort?.Dir ?? "asc", DbType.String);
            parameters.Add("@SearchColumn", request.request.SearchColumn, DbType.String);
            parameters.Add("@SearchTerm", request.request.SearchTerm, DbType.String);
            parameters.Add("@TotalCount", dbType: DbType.Int32, direction: ParameterDirection.Output);

            var reviews = await connection.QueryAsync<dynamic>(
                "SearchReviews",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            int totalCount = parameters.Get<int>("@TotalCount");

            var reviewDtos = reviews.Select(r => new ReviewDto
            {
                Id = r.Id is Guid g ? g : Guid.Parse(r.Id.ToString()),
                MovieId = r.MovieId,
                UserId = r.UserId,
                UserName = r.UserName,
                Comment = r.Comment,
                Rating = r.Rating
            }).ToList();

            //int totalCount = parameters.Get<int>("@TotalCount");

            var response = new ReviewResponseDto
            {
                Reviews = reviewDtos,
                TotalCount = totalCount
            };

            return Result<ReviewResponseDto>.Success(response);
        }
    }
}