using Microsoft.EntityFrameworkCore;
using MovieReviewApi.Domain.Entities;

namespace MovieReviewApi.Application.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<Movie> Movies { get; }
        DbSet<Actor> Actors { get; }
        DbSet<Review> Reviews { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);

    }
}
