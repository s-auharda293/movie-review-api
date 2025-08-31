
using Microsoft.EntityFrameworkCore;
using MovieReviewApi.Domain.Common;
using MovieReviewApi.Domain.Entities;

namespace MovieReviewApi.Infrastructure.Persistence;

public class MovieReviewDbContext: DbContext
{
    public DbSet<Movie> Movies { get; set; } = null!;

    public DbSet<Actor> Actors { get; set; } = null!;

    //public DbSet<Genre> Genres { get; set; } = null!;

    public MovieReviewDbContext(DbContextOptions<MovieReviewDbContext> options) : base(options) { 
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
       // modelBuilder.Entity<Actor>()
       //.HasIndex(a => new { a.Name, a.DateOfBirth })
       //.IsUnique(); //handle duplicate entries later on

        modelBuilder.Entity<Actor>()
            .HasMany(a => a.Movies)   // actor has many movies
            .WithMany(m => m.Actors); // movie has many actors

        //modelBuilder.Entity<Genre>()
        //    .HasMany(g => g.Movies) // genre can be in many movies
        //    .WithMany(m => m.Genres); // movie can have many genres

        modelBuilder.Entity<Movie>()
            .Property(m => m.Rating)
            .HasPrecision(2, 1);

        modelBuilder.Entity<BaseEntity>()
            .Property(b => b.CreatedAt)
            .HasDefaultValueSql("GETUTCDATE()");

        modelBuilder.Entity<BaseEntity>()
          .Property(b => b.UpdatedAt)
          .HasDefaultValueSql("GETUTCDATE()");
    }


}