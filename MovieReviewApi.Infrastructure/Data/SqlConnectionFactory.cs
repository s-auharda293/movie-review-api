using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using MovieReviewApi.Application.Interfaces;
using System.Data;

namespace MovieReviewApi.Infrastructure.Data
{
    public class SqlConnectionFactory : IDbConnectionFactory
    {

        private readonly string? _connectionString;
        public SqlConnectionFactory(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("MovieReviewDb");
        }
        public async Task<IDbConnection> CreateConnectionAsync(CancellationToken cancellationToken)
        {
            var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);
            return connection;
        }
    }
}
