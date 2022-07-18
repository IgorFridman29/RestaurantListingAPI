using System;
using System.Collections.Generic;

namespace RestaurantListingAPI.Data
{
    public class Restaurant
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime VisitedOn { get; set; }
        public virtual Location Location { get; set; }
        public virtual List<Dish> Dishes { get; set; }
    }
}
