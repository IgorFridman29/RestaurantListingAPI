namespace RestaurantListingAPI.Data
{
    public class Dish
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Stars { get; set; }
        public string Review { get; set; }

        // Foreign key
        public int RestaurantId { get; set; }
        public virtual Restaurant Restaurant { get; set; }
    }
}
