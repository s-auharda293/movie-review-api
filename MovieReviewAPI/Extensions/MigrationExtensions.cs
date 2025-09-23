using MovieReviewApi.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace MovieReviewApi.Api.Extensions
{
    public static class MigrationExtensions
    {
        public static void ApplyMigrations(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

            try
            {
                // Migrate main app database
                var dbContext = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();
                ((DbContext)dbContext).Database.Migrate();
                logger.LogInformation("MovieReviewDb migration applied successfully.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while applying migrations in Docker database.");
            }
        }
    }
}
