using System.ComponentModel.DataAnnotations;

namespace RestaurantListingAPI.DTO
{
    public class RegisterUserDTO
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string MobileNumber { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool IsPaid { get; set; }

    }
}
