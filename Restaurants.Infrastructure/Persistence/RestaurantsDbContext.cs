using Microsoft.EntityFrameworkCore;
using Restaurants.Domain.Entities;


namespace Restaurants.Infrastructure.Persistence;

public class RestaurantsDbContext: DbContext 
{
    public DbSet<Restaurant> Restaurants { get; set; } = null!;
    public DbSet<Dish> Dishes { get; set; } = null!;


    public RestaurantsDbContext(DbContextOptions<RestaurantsDbContext> options) : base(options) { 
    
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
