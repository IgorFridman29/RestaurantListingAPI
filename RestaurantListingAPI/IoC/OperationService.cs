using System;

namespace RestaurantListingAPI.IoC
{
    public class OperationService : ITransientService, IScopedService, ISingletonService
    {
        private readonly Guid _id;

        public OperationService()
        {
            _id = Guid.NewGuid();
        }

        public Guid GetOperation()
        {
            return _id;
        }
    }
}
