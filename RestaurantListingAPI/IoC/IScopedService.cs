using System;

namespace RestaurantListingAPI.IoC
{
    public interface IScopedService
    {
        Guid GetOperation();
    }
}
