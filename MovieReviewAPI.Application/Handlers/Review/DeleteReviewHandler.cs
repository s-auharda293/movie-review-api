using Dapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MovieReviewApi.Application.Commands.Actor;
using MovieReviewApi.Application.Commands.Review;
using MovieReviewApi.Application.Interfaces;
using MovieReviewApi.Domain.Entities;
using System.Data;

namespace MovieReviewApi.Application.Handlers.Review
{
    public class DeleteReviewHandler:IRequestHandler<DeleteReviewCommand,Result<bool>>
    {
        private readonly IDbConnectionFactory _connection;
        public DeleteReviewHandler(IDbConnectionFactory connection) { 
            _connection = connection;
        }

        public async Task<Result<bool>> Handle(DeleteReviewCommand request, CancellationToken cancellationToken) {

            var connection = await _connection.CreateConnectionAsync(cancellationToken);
            var affectedRows = await connection.ExecuteAsync(
                "DeleteReview",
                 new { Id = request.Id },
                 commandType: CommandType.StoredProcedure
                );

            if (affectedRows == 0) return Result<bool>.Failure(ReviewErrors.NotFound);

            return Result<bool>.Success(true);
        }
    }
}
