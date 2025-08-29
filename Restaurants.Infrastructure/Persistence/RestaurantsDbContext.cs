using Microsoft.EntityFrameworkCore;
using Restaurants.Domain.Entities;


namespace Restaurants.Infrastructure.Persistence;

internal class RestaurantsDbContext: DbContext 
{
    internal DbSet<Restaurant> Restaurants { get; set; }
    internal DbSet<Dish> Dishes { get; set; }

    //to establish connection with database
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=localhost\\MSSQLSERVER02;Database=RestaurantsDB;Trusted_Connection=True;TrustServerCertificate=True;");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        //address is not a separate table it's properties will be stored as extra columns in 
        //the Restaurants table
        modelBuilder.Entity<Restaurant>().OwnsOne(r => r.Address);

        //one restaurant has many dishes, WithOne means each Dish belongs to one Restaurant
        modelBuilder.Entity<Restaurant>()
            .HasMany(r => r.Dishes)
            .WithOne()
            .HasForeignKey(d => d.RestaurantId);
    }
}
