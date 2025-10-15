using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MovieReviewApi.Application.Interfaces;
using MovieReviewApi.Infrastructure.Data;
using System.Data;

namespace MovieReviewApi.IntegrationTests
{
    public class MovieReviewWebApplicationFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            IConfiguration? configuration = null;

            // Load test configuration
            builder.ConfigureAppConfiguration((context, config) =>
            {
                config.AddJsonFile("appsettings.Test.json", optional: false);
                configuration = config.Build();
            });

            builder.ConfigureTestServices(services =>
            {
                // Remove existing DbContext registrations
                services.RemoveAll(typeof(DbContextOptions<MovieReviewDbContext>));
                services.RemoveAll<IApplicationDbContext>();

                // Build temporary provider to get configuration
                var sp = services.BuildServiceProvider();
                var config = sp.GetRequiredService<IConfiguration>();
                var connectionString = config.GetConnectionString("MovieReviewDb");

                // Register DbContext for tests
                services.AddDbContext<MovieReviewDbContext>(options =>
                    options.UseSqlServer(connectionString));

                services.AddScoped<IApplicationDbContext, MovieReviewDbContext>();

                // Dapper replacement for IDbConnection
                services.RemoveAll<IDbConnection>();
                services.AddScoped<IDbConnection>(_ => new SqlConnection(connectionString));

                // Build temporary scope to run migrations
                using var scope = services.BuildServiceProvider().CreateScope();
                var scopedProvider = scope.ServiceProvider;
                var dbContext = scopedProvider.GetRequiredService<MovieReviewDbContext>();

                // Ensure fresh database
                dbContext.Database.EnsureDeleted();
                dbContext.Database.Migrate();
            });
        }
    }
}
