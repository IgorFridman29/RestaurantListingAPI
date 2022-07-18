using RestaurantListingAPI.Data;
using System;
using System.Threading.Tasks;

namespace RestaurantListingAPI.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DatabaseContext _databaseContext;
        private IGenericRepository<Restaurant> _restaurants;
        private IGenericRepository<Location> _locations;
        private IGenericRepository<Dish> _dishes;

        public IGenericRepository<Restaurant> Restaurants => _restaurants ??= new GenericRepository<Restaurant>(_databaseContext);

        public IGenericRepository<Location> Locations => _locations ??= new GenericRepository<Location>(_databaseContext);

        public IGenericRepository<Dish> Dishes => _dishes ??= new GenericRepository<Dish>(_databaseContext);

        public UnitOfWork(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public void Dispose()
        {
            _databaseContext.Dispose();
            GC.SuppressFinalize(this);
        }

        public async Task Save()
        {
            await _databaseContext.SaveChangesAsync();
        }
    }
}
