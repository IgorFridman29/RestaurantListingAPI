using System.ComponentModel.DataAnnotations;

namespace RestaurantListingAPI.DTO
{
    public class CreateLocationDTO
    {
        [Required]
        [StringLength(maximumLength: 30, ErrorMessage = "Location Country is too Long")]
        public string Country { get; set; }

        [Required]
        [StringLength(maximumLength: 50, ErrorMessage = "Location Address is too Long")]
        public string Address { get; set; }
        
        [Required]
        public int RestaurantId { get; set; }
    }

    public class LocationDTO : CreateLocationDTO
    {
        public int Id { get; set; }
        public RestaurantDTO Restaurant { get; set; }
    }

    public class UpdateLocationDTO : CreateLocationDTO
    {

    }
}
