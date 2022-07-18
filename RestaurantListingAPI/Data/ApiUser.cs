using Microsoft.AspNetCore.Identity;

namespace RestaurantListingAPI.Data
{
    public class ApiUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsPaid { get; set; }
    }
}
