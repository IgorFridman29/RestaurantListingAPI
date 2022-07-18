using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using RestaurantListingAPI.Data;

namespace RestaurantListingAPI.Services
{
    public static class ServiceExtensions
    {
        public static void AddIdentityConfiguration(this IServiceCollection services)
        {
            var builder = services.AddIdentityCore<ApiUser>(
                options => {
                    options.User.RequireUniqueEmail = true;
                    options.Password.RequireLowercase = false;
                    });

            builder = new IdentityBuilder(builder.UserType, typeof(IdentityRole), services).AddRoles<IdentityRole>();

            builder.AddEntityFrameworkStores<DatabaseContext>().AddDefaultTokenProviders();

        }
    }
}
