using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MovieReviewApi.Application.Interfaces;
using MovieReviewApi.Application.Interfaces.Identity;
using MovieReviewApi.Infrastructure.Data;    
using MovieReviewApi.Infrastructure.Services.Identity;

namespace MovieReviewApi.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration) {
        var connectionString = configuration.GetConnectionString("MovieReviewDb");
        services.AddDbContext<MovieReviewDbContext>(options => options.UseSqlServer(connectionString));
        services.AddScoped<IApplicationDbContext, MovieReviewDbContext>();
        services.AddScoped<IRoleService, RoleService>();

    }
}
