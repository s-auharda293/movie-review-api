using Dapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using MovieReviewApi.Application.Commands.Review;
using MovieReviewApi.Application.Interfaces;
using MovieReviewApi.Domain.Entities;
using System.Data;
using System.Security.Claims;

namespace MovieReviewApi.Application.Handlers.Review
{
    public class DeleteReviewHandler:IRequestHandler<DeleteReviewCommand,Result<bool>>
    {
        private readonly IDbConnectionFactory _connection;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public DeleteReviewHandler(IDbConnectionFactory connection, IHttpContextAccessor httpContextAccessor) { 
            _connection = connection;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Result<bool>> Handle(DeleteReviewCommand request, CancellationToken cancellationToken) {

            var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userRole = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Role)?.Value;

            if (string.IsNullOrEmpty(userId))
                return Result<bool>.Failure(ReviewErrors.UserNotAuthenticated);

            var connection = await _connection.CreateConnectionAsync(cancellationToken);

            int affectedRows;

            if (userRole == "Admin")
            {
                // Admin can delete any review
                affectedRows = await connection.ExecuteAsync(
                    "DeleteReview",
                    new { Id = request.Id },
                    commandType: CommandType.StoredProcedure
                );
            }
            else
            {
                // User can only delete their own review
                affectedRows = await connection.ExecuteAsync(
                    "DeleteReviewForUser",
                    new { Id = request.Id, UserId = userId },
                    commandType: CommandType.StoredProcedure
                );
            }

            if (affectedRows == 0) return Result<bool>.Failure(ReviewErrors.NotFound);

            return Result<bool>.Success(true);
        }
    }
}
