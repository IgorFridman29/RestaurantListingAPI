using System;

namespace RestaurantListingAPI.IoC
{
    public interface ITransientService
    {
        Guid GetOperation();
    }
}
