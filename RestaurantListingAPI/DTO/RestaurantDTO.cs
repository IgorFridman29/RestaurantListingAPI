using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RestaurantListingAPI.DTO
{
    public class BaseRestaurantDTO
    {
        [Required]
        [StringLength(maximumLength: 50, ErrorMessage = "Restaurant Name Is Too Long")]
        public string Name { get; set; }

        public DateTime VisitedOn { get; set; }
    }

    public class CreateRestaurantDTO : BaseRestaurantDTO
    {
        [Required]
        public CreateLocationDTO Location { get; set; }

        public List<CreateDishDTO> Dishes { get; set; }
    }

    public class UpdateRestaurantDTO : BaseRestaurantDTO
    {
        [Required]
        public UpdateLocationDTO Location { get; set; }

        [Required]
        public List<UpdateDishDTO> Dishes { get; set; }
    }

    public class RestaurantDTO : BaseRestaurantDTO
    {
        public int Id { get; set; }

        //8 a Restaurant has a location
        public LocationDTO Location { get; set; }

        //9 A restaurant has many dishes
        public List<DishDTO> Dishes { get; set; }
    }
}
