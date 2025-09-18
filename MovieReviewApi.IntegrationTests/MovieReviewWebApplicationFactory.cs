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

            // Load appsettings.Test.json and capture configuration
            builder.ConfigureAppConfiguration((context, config) =>
            {
                config.AddJsonFile("appsettings.Test.json", optional: false);
                configuration = config.Build();
            });

            builder.ConfigureTestServices(services =>
            {
                // Get connection string from configuration we just built
                var connectionString = configuration!.GetConnectionString("MovieReviewDb");

                // Remove existing DbContext registrations
                services.RemoveAll(typeof(DbContextOptions<MovieReviewDbContext>));
                services.RemoveAll<IApplicationDbContext>();

                // Add test DbContext done for ef core and stored procedure
                services.AddDbContext<MovieReviewDbContext>(options =>
                {
                    options.UseSqlServer(connectionString);
                });

                services.AddScoped<IApplicationDbContext, MovieReviewDbContext>();

                // Replace IDbConnection with one using the same connection string this is done for dapper
                services.RemoveAll<IDbConnection>();
                services.AddScoped<IDbConnection>(_ => new SqlConnection(connectionString));
            });
        }

        public void EnsureDatabase()
        {
            using var scope = Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<MovieReviewDbContext>();
            db.Database.EnsureDeleted();
            db.Database.Migrate();
        }
    }
}
