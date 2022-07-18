using AutoMapper;
using RestaurantListingAPI.Data;

namespace RestaurantListingAPI.DTO
{
    public class MapperInitializer : Profile
    {
        public MapperInitializer()
        {
            CreateMap<Restaurant, RestaurantDTO>().ReverseMap();
            CreateMap<Restaurant, CreateRestaurantDTO>().ReverseMap();
            CreateMap<Restaurant, UpdateRestaurantDTO>().ReverseMap();

            CreateMap<Location, LocationDTO>().ReverseMap();
            CreateMap<Location, CreateLocationDTO>().ReverseMap();
            CreateMap<Location, UpdateLocationDTO>().ReverseMap();

            CreateMap<Dish, DishDTO>().ReverseMap();
            CreateMap<Dish, CreateDishDTO>().ReverseMap();
            CreateMap<Dish, UpdateDishDTO>().ReverseMap();
        }
    }
}
