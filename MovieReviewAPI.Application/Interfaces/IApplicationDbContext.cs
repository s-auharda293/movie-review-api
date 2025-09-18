using Microsoft.EntityFrameworkCore;
using MovieReviewApi.Application.KeylessEntities;
using MovieReviewApi.Domain.Entities;

namespace MovieReviewApi.Application.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<Movie> Movies { get; }
        DbSet<Actor> Actors { get; }
        DbSet<Review> Reviews { get; }

        DbSet<GetMoviesResult> GetMoviesResult{ get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);

    }
}
