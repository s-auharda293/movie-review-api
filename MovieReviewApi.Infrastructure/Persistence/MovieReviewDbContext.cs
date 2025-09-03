
using Microsoft.EntityFrameworkCore;
using MovieReviewApi.Domain.Common;
using MovieReviewApi.Domain.Entities;

namespace MovieReviewApi.Infrastructure.Persistence;

public class MovieReviewDbContext: DbContext
{
    public DbSet<Movie> Movies { get; set; } = null!;

    public DbSet<Actor> Actors { get; set; } = null!;

    public DbSet<Review> Reviews { get; set; } = null!;

    //public DbSet<Genre> Genres { get; set; } = null!;

    public MovieReviewDbContext(DbContextOptions<MovieReviewDbContext> options) : base(options) { 
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // modelBuilder.Entity<Actor>()
        //.HasIndex(a => new { a.Name, a.DateOfBirth })
        //.IsUnique(); //handle duplicate entries later on


        //modelBuilder.Entity<Genre>()
        //    .HasMany(g => g.Movies) // genre can be in many movies
        //    .WithMany(m => m.Genres); // movie can have many genres

        modelBuilder.Entity<Actor>().ToTable("Actors");
        modelBuilder.Entity<Movie>().ToTable("Movies");
        modelBuilder.Entity<Review>().ToTable("Reviews");

        modelBuilder.Entity<Actor>()
        .HasMany(a => a.Movies)
        .WithMany(m => m.Actors)
        .UsingEntity<Dictionary<string, object>>(
            "ActorMovie",
            j => j.HasOne<Movie>()
                  .WithMany()
                  .HasForeignKey("MovieId")
                  .OnDelete(DeleteBehavior.Restrict),  
            j => j.HasOne<Actor>()
                  .WithMany()
                  .HasForeignKey("ActorId")
                  .OnDelete(DeleteBehavior.Restrict)  
        );

        modelBuilder.Entity<Review>()
        .HasOne(r => r.Movie)
        .WithMany(m => m.Reviews)
        .HasForeignKey(r => r.MovieId)
        .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Movie>()
            .Property(m => m.Rating)
            .HasPrecision(3, 1);

        modelBuilder.Entity<Review>()
            .Property(r => r.Rating)
            .HasPrecision(3, 1);

        modelBuilder.Entity<BaseEntity>()
            .Property(b => b.CreatedAt)
            .HasDefaultValueSql("GETUTCDATE()");

        modelBuilder.Entity<BaseEntity>()
            .Property(b => b.UpdatedAt)
            .HasDefaultValueSql("GETUTCDATE()");
    }


}