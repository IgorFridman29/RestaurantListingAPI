using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using RestaurantListingAPI.Data;
using System.Text;

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
        public static void AddJWTAuthentication(this IServiceCollection services,
            IConfiguration Configuration)
        {
            var jwtSettings = Configuration.GetSection("Jwt");
            var key = jwtSettings.GetSection("KEY").Value;

            services.AddAuthentication(o =>
            {
                /*
                 *  we specify the default authentication scheme JwtBearerDefaults.AuthenticationScheme as well as DefaultChallengeScheme.
                 */
                o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            /*By calling the AddJwtBearer method, we enable the JWT authenticating using the default scheme, 
             * and we pass a parameter, which we use to set up JWT bearer options:
             */
            .AddJwtBearer(o =>
            {

                o.TokenValidationParameters = new TokenValidationParameters
                {

                    //The issuer is the actual server that created the token
                    ValidateIssuer = true,
                    //The receiver of the token is a valid recipient 
                    ValidateAudience = true,
                    //The token has not expired
                    ValidateLifetime = true,
                    //The signing key is valid and is trusted by the server
                    ValidateIssuerSigningKey = true,

                    //Additionally, we are providing values for the issuer, audience, and the secret key that the server uses to generate the signature for JWT.
                    ValidIssuer = jwtSettings.GetSection("Issuer").Value,
                    ValidAudience = jwtSettings.GetSection("Audience").Value,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                };
            });
        }
    }
}
