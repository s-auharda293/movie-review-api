using Dapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MovieReviewApi.Application.Commands.Actor;
using MovieReviewApi.Application.Interfaces;
using MovieReviewApi.Domain.Common.Actors;
using System.Data;

namespace MovieReviewApi.Application.Handlers.Actor
{
    public class DeleteActorHandler : IRequestHandler<DeleteActorCommand, Result<bool>>
    {
        private readonly IDbConnectionFactory _connection;
        public DeleteActorHandler(IDbConnectionFactory connection)
        {
            _connection = connection;
        }
        public async Task<Result<bool>> Handle(DeleteActorCommand request, CancellationToken cancellationToken)
        {
            var connection = await _connection.CreateConnectionAsync(cancellationToken);

            var affectedRows = await connection.ExecuteAsync(
                 "DeleteActor",                      
                 new { Id = request.Id },
                 commandType: CommandType.StoredProcedure
             );


            if (affectedRows == 0)
                return Result<bool>.Failure(ActorErrors.NotFound);

            return Result<bool>.Success(true);

        }
    }
}
