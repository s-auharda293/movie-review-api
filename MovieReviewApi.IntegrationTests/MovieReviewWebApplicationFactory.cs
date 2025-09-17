using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MovieReviewApi.Infrastructure.Data;

namespace MovieReviewApi.IntegrationTests
{
    public class MovieReviewWebApplicationFactory : WebApplicationFactory<Program>
    {
        //private readonly string _connectionString = @"Server=localhost\MSSQLSERVER02;Database=MovieReviewTestDb;User Id=sauharda;Password=M3w<c9]238#;TrustServerCertificate=True";
       //customize DI and services of the app
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {
                // Remove existing DbContext registration
                services.RemoveAll(typeof(DbContextOptions<MovieReviewDbContext>));

                // Register test database
                services.AddDbContext<MovieReviewDbContext>(options =>
                {
                    options.UseSqlServer("Server=localhost\\MSSQLSERVER02;Database=MovieReviewTestDb;User Id=sauharda;Password=M3w<c9]238#;TrustServerCertificate=True");
                });

                // Build service provider and reset test database
                var sp = services.BuildServiceProvider();
                using var scope = sp.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<MovieReviewDbContext>();
                Console.WriteLine("Resetting test database...");
                Console.WriteLine($"Using DB: {db.Database.GetDbConnection().Database}");
                db.Database.EnsureDeleted();
                db.Database.Migrate();
                Console.WriteLine("Database ready.");
            });
        }
    }
}
