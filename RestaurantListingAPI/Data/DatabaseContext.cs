using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace RestaurantListingAPI.Data
{
    public class DatabaseContext : IdentityDbContext<ApiUser>
    {
        // list all tables = DbSets
        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Dish> Dishes { get; set; }

        public DatabaseContext(DbContextOptions options) : base(options)
        {

        }
    }
}
