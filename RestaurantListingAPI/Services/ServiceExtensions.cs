using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using RestaurantListingAPI.Data;
using RestaurantListingAPI.Models;
using Serilog;
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

        public static void AddAutorizationPolicies(this IServiceCollection services)
        {
            services.AddAuthorization(op =>
            {
                op.AddPolicy("PayingOnly", policy => policy.RequireClaim("IsPaid", "True"));
                op.AddPolicy("HasMobile", policy => policy.RequireClaim("mobilephone"));
            });
        }

        public static void UseCustomeExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(error =>
            {
                error.Run(async context =>
                {
                    //context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    context.Response.ContentType = "application/json";

                    //grab the feature of the actual error
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {
                        //write to serilog
                        Log.Error($"Error {contextFeature.Error} occured!!!");

                        //create an error message to display in browser
                        Error error = new Error
                        {
                            StatusCode = context.Response.StatusCode,
                            Message = "(Global)Internal Server Error. Please Try Again Later."
                        };
                        await context.Response.WriteAsync(error.ToString());
                    }
                });
            });
        }

        public static void AddVersioning(this IServiceCollection services)
        {
            services.AddApiVersioning(opt =>
            {
                // add a header in the http response that specified the used version
                opt.ReportApiVersions = true;
                // if nothing is specified used the default version
                opt.AssumeDefaultVersionWhenUnspecified = true;
                // this is the default version
                opt.DefaultApiVersion = new ApiVersion(1, 0);


                //add this line later when showing how to pass version in header
                // opt.ApiVersionReader = new HeaderApiVersionReader("api-version");
            });
        }
    }
}
