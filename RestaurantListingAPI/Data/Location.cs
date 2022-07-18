namespace RestaurantListingAPI.Data
{
    public class Location
    {
        public int Id { get; set; }
        public string Country { get; set; }
        public string Address { get; set; }

        // Foreign key
        public int RestaurantId { get; set; }
        public virtual Restaurant Restaurant { get; set; }
    }
}
