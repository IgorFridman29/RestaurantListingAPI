using AutoMapper;
using RestaurantListingAPI.Data;

namespace RestaurantListingAPI.DTO
{
    public class MapperInitializer : Profile
    {
        public MapperInitializer()
        {
            // Map Restaurant
            CreateMap<Restaurant, RestaurantDTO>().ReverseMap();
            CreateMap<Restaurant, CreateRestaurantDTO>().ReverseMap();
            CreateMap<Restaurant, UpdateRestaurantDTO>().ReverseMap();

            // Map Location
            CreateMap<Location, LocationDTO>().ReverseMap();
            CreateMap<Location, CreateLocationDTO>().ReverseMap();
            CreateMap<Location, UpdateLocationDTO>().ReverseMap();

            // Map Dish
            CreateMap<Dish, DishDTO>().ReverseMap();
            CreateMap<Dish, CreateDishDTO>().ReverseMap();
            CreateMap<Dish, UpdateDishDTO>().ReverseMap();

            // Map ApiUser
            CreateMap<ApiUser, RegisterUserDTO>().ReverseMap();
        }
    }
}
