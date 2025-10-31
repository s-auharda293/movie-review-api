
using Microsoft.EntityFrameworkCore;
using MovieReviewApi.Application.Interfaces;
using MovieReviewApi.Domain.Entities;
using MovieReviewApi.Application.KeylessEntities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace MovieReviewApi.Infrastructure.Data;

public class MovieReviewDbContext: IdentityDbContext<ApplicationUser>, IApplicationDbContext
{
    public DbSet<Movie> Movies { get; set; } = null!;

    public DbSet<Actor> Actors { get; set; } = null!;

    public DbSet<Review> Reviews { get; set; } = null!;

    //public DbSet<Genre> Genres { get; set; } = null!;

    //Keyless entities
    public DbSet<GetMoviesResult> GetMoviesResult { get; set; }


    public MovieReviewDbContext(DbContextOptions<MovieReviewDbContext> options) : base(options) { 
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) {
       return base.SaveChangesAsync(cancellationToken);
    }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        // modelBuilder.Entity<Actor>()
        //.HasIndex(a => new { a.Name, a.DateOfBirth })
        //.IsUnique(); //handle duplicate entries later on


        //modelBuilder.Entity<Genre>()
        //    .HasMany(g => g.Movies) // genre can be in many movies
        //    .WithMany(m => m.Genres); // movie can have many genres

        modelBuilder.Entity<Actor>().ToTable("Actors");
        modelBuilder.Entity<Movie>().ToTable("Movies");
        modelBuilder.Entity<Review>().ToTable("Reviews");

        modelBuilder.Entity<GetMoviesResult>().HasNoKey();
        modelBuilder.Entity<GetMoviesResult>().ToView(null);

        modelBuilder.Entity<Actor>(entity =>
        {
            entity.Property(a => a.Name)
                  .HasMaxLength(100)
                  .IsRequired(); //not null

            entity.HasIndex(a => a.Name).HasDatabaseName("IX_Actors_Name");
            entity.HasIndex(a => a.DateOfBirth).HasDatabaseName("IX_Actors_DateOfBirth");
            entity.HasIndex(a => a.CreatedAt).HasDatabaseName("IX_Actors_CreatedAt");

            // Many-to-many configuration with Movies
            entity.HasMany(a => a.Movies)
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
                     .OnDelete(DeleteBehavior.Restrict),
               j =>
               {
                   j.HasKey("ActorId", "MovieId");
                   j.HasIndex("ActorId").HasDatabaseName("IX_ActorMovie_ActorId");
                   j.HasIndex("MovieId").HasDatabaseName("IX_ActorMovie_MovieId");
               }
               );

            modelBuilder.Entity<Movie>(entity =>
            {
                entity.HasIndex(m => m.ReleaseDate).HasDatabaseName("IX_Movies_ReleaseDate");
                entity.HasIndex(m => m.DurationMinutes).HasDatabaseName("IX_Movies_DurationMinutes");
                entity.HasIndex(m => m.Rating).HasDatabaseName("IX_Movies_Rating");
                //entity.HasIndex(m => m.Status).HasDatabaseName("IX_Movies_Status");
           });

            modelBuilder.Entity<Review>()
           .HasOne(r => r.Movie)
           .WithMany(m => m.Reviews)
           .HasForeignKey(r => r.MovieId)
           //.OnDelete(DeleteBehavior.Restrict);
           .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Movie>()
                .Property(m => m.Rating)
                .HasPrecision(4, 1);

            modelBuilder.Entity<Review>()
                .Property(r => r.Rating)
                .HasPrecision(4, 1);

            modelBuilder.Entity<GetMoviesResult>()
            .Property(g => g.Rating)
            .HasPrecision(4, 1); 


            modelBuilder.Entity<Actor>()
                .Property(a => a.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");
            modelBuilder.Entity<Actor>()
                .Property(a => a.UpdatedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            modelBuilder.Entity<Movie>()
                .Property(m => m.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");
            modelBuilder.Entity<Movie>()
                .Property(m => m.UpdatedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            modelBuilder.Entity<Review>()
                .Property(r => r.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");
            modelBuilder.Entity<Review>()
                .Property(r => r.UpdatedAt)
                .HasDefaultValueSql("GETUTCDATE()");

        });
    }
}