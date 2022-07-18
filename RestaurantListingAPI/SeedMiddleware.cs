using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using RestaurantListingAPI.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantListingAPI
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class SeedMiddleware
    {
        private readonly RequestDelegate _next;

        public SeedMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext, DatabaseContext dbContext)
        {
            if (dbContext.Database.CanConnect() && !dbContext.Restaurants.Any())
            {
                await InsertDataAsync(dbContext);
            }
            await _next(httpContext);
        }

        private async Task InsertDataAsync(DatabaseContext dbContext)
        {

            var restaurants = new List<Restaurant> {

        new Restaurant
        {
            Name = "Thai Place",
            VisitedOn = DateTime.Now.AddMonths(-18),
            Location = new Location{
                Country = "Israel",
                Address = "Tel Aviv"
            },
            Dishes = new List<Dish>{
                new Dish{
                     Name = "Pad Thai",
                     Review = "Very Spicy and very yummy",
                     Stars = 4,
                }
           }
        },

         new Restaurant
        {
            Name = "Busy Street",
            VisitedOn = DateTime.Now.AddMonths(-1),
            Location = new Location{
                Country = "Israel",
                Address = "Netanya"
            },
            Dishes = new List<Dish>{
                new Dish{
                    Name = "Shakshuka",
                    Review = "The best one and the original one",
                    Stars = 4,
                },
                new Dish{
                    Name = "Falafel",
                    Review = "Too oily lacks salt",
                    Stars = 1,
                }
           }
        },
          new Restaurant
        {
            Name = "Middle Eastern Foody",
            VisitedOn = DateTime.Now.AddHours(-2),
            Location = new Location{
                Country = "Israel",
                Address = "Hayfa"
            },
            Dishes = new List<Dish>{
                new Dish{
                    Name = "Malabi",
                    Review = "Excellent not too sweet",
                    Stars = 5,
                },
                new Dish{
                    Name = "Greek Salad",
                    Review = "Fresh and Tasty Feta cheese",
                    Stars = 4,
                }
           }
        }
    };
            await dbContext.AddRangeAsync(restaurants);
            await dbContext.SaveChangesAsync();
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseSeedMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<SeedMiddleware>();
        }
    }
}
