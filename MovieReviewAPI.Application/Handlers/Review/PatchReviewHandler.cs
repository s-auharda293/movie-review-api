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
    public class PatchReviewHandler:IRequestHandler<PatchReviewCommand,Result<ReviewDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IDbConnectionFactory _connection;
        public PatchReviewHandler(IApplicationDbContext context, IDbConnectionFactory connection)
        {
         _context = context;   
         _connection = connection;
        }

        public async Task<Result<ReviewDto>> Handle(PatchReviewCommand request, CancellationToken cancellationToken) {
            var review = await _context.Reviews.FirstOrDefaultAsync(r => r.Id == request.Id);
            if (review == null) return Result<ReviewDto>.Failure(ReviewErrors.NotFound);

            var connection = await _connection.CreateConnectionAsync(cancellationToken);

            // Prepare parameters (nullable for patch)
            var parameters = new DynamicParameters();
            parameters.Add("@Id", request.Id, DbType.Guid);
            parameters.Add("@UserName", string.IsNullOrEmpty(request.dto.UserName) ? null : request.dto.UserName, DbType.String);
            parameters.Add("@Comment", string.IsNullOrEmpty(request.dto.Comment) ? null : request.dto.Comment, DbType.String);
            parameters.Add("@Rating", request.dto.Rating, DbType.Decimal);

            // Execute the stored procedure and get the updated review
            var updatedReview = await connection.QueryFirstAsync<ReviewDto>(
                "PatchReview",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return Result<ReviewDto>.Success(updatedReview);


        }
    }
}
