using Dapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MovieReviewApi.Application.Commands.Movie;
using MovieReviewApi.Application.Interfaces;
using MovieReviewApi.Domain.Common.Movies;
using System.Data;

namespace MovieReviewApi.Application.Handlers.Movie
{
    public class DeleteMovieHandler : IRequestHandler<DeleteMovieCommand, Result<bool>>
    {
        private readonly IDbConnectionFactory _connection;
        public DeleteMovieHandler(IDbConnectionFactory connection)
        {
            _connection = connection;
        }
        public async Task<Result<bool>> Handle(DeleteMovieCommand request, CancellationToken cancellationToken)
        {
            var connection = await _connection.CreateConnectionAsync(cancellationToken);

            var affectedRows = await connection.ExecuteAsync(
                 "DeleteMovie",                   
                 new { Id = request.Id },
                 commandType: CommandType.StoredProcedure         
             );

            if (affectedRows == 0)
                return Result<bool>.Failure(MovieErrors.NotFound);

            return Result<bool>.Success(true);

        }
    }
}
