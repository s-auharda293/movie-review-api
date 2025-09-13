using Microsoft.EntityFrameworkCore;
using MovieReviewApi.Domain.Entities;

namespace MovieReviewApi.Application.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<Movie> Movies { get; }
        DbSet<Actor> Actors { get; }
        DbSet<Review> Reviews { get; }
        DbSet<TEntity> Set<TEntity>() where TEntity : class;
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);

    }
}
