using Dapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MovieReviewApi.Application.Commands.Review;
using MovieReviewApi.Application.DTOs;
using MovieReviewApi.Application.Interfaces;
using MovieReviewApi.Domain.Entities;
using System.Data;

namespace MovieReviewApi.Application.Handlers.Review
{
    public class UpdateReviewHandler:IRequestHandler<UpdateReviewCommand,Result<ReviewDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IDbConnectionFactory _connection;
        public UpdateReviewHandler(IApplicationDbContext context, IDbConnectionFactory connection)
        {
            _context = context;   
            _connection = connection;
        }

        public async Task<Result<ReviewDto>> Handle(UpdateReviewCommand request, CancellationToken cancellationToken) {

            var review = await _context.Reviews.FirstOrDefaultAsync(r => r.Id == request.Id);
            if (review == null) return Result<ReviewDto>.Failure(ReviewErrors.NotFound);

            var connection = await _connection.CreateConnectionAsync(cancellationToken);

            var parameters = new DynamicParameters();
            parameters.Add("@Id", request.Id, DbType.Guid);
            parameters.Add("@UserName", request.dto.UserName, DbType.String);
            parameters.Add("@Comment", request.dto.Comment, DbType.String);
            parameters.Add("@Rating", request.dto.Rating, DbType.Decimal);

            var updatedReview = await connection.QueryFirstAsync<ReviewDto>(
                "UpdateReview",
                parameters,
                commandType: CommandType.StoredProcedure
                );

            var reviewDto = new ReviewDto
            {
                Id = updatedReview.Id,
                MovieId = updatedReview.MovieId,
                UserName = updatedReview.UserName,
                Comment = updatedReview.Comment,
                Rating = updatedReview.Rating
            };

            return Result<ReviewDto>.Success(reviewDto);

        }
    }
}
