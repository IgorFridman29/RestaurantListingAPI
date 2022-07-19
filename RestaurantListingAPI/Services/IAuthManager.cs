using RestaurantListingAPI.DTO;
using System.Threading.Tasks;

namespace RestaurantListingAPI.Services
{
    public interface IAuthManager
    {
        Task<bool> ValidateUser(LoginUserDTO userDTO);
        Task<string> CreateToken();
    }
}
