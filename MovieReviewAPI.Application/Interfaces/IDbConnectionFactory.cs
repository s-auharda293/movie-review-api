using System.Data;

namespace MovieReviewApi.Application.Interfaces
{
    public interface IDbConnectionFactory {
        Task<IDbConnection> CreateConnectionAsync(CancellationToken cancellationToken);

    }
}
