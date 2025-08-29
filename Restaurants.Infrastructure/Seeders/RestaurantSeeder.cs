
//using Restaurants.Domain.Entities;
//using Restaurants.Infrastructure.Persistence;

//namespace Restaurants.Infrastructure.Seeders;

//internal class RestaurantSeeder(RestaurantsDbContext dbContext) {
//    public async Task Seed() {
//        if (await dbContext.Database.CanConnectAsync()) {
//            if (!dbContext.Restaurants.Any()) {
//                var restaurants = GetRestaurants();
//                dbContext.Restaurants.AddRange(restaurants);
//                await dbContext.SaveChangesAsync(); 
//            }
//        }
//    }

//    private IEnumerable<Restaurant> GetRestaurants()
//    {
//        List<Restaurant> restaurants = new List<Restaurant>();
//    }
//}

