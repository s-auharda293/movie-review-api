using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using MovieReviewApi.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using MovieReviewApi.Application.Interfaces;

namespace MovieReviewApi.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration) {
        var connectionString = configuration.GetConnectionString("MovieReviewDb");
        services.AddDbContext<MovieReviewDbContext>(options => options.UseSqlServer(connectionString));
        services.AddScoped<IApplicationDbContext, MovieReviewDbContext>();

    }
}
