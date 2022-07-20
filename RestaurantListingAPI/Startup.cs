using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using RestaurantListingAPI.Data;
using RestaurantListingAPI.DTO;
using RestaurantListingAPI.IoC;
using RestaurantListingAPI.Repositories;
using RestaurantListingAPI.Services;

namespace RestaurantListingAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<DatabaseContext>(options =>
            {
                options.UseSqlServer(
                    Configuration.GetConnectionString("sqlConnection"));
            },
            ServiceLifetime.Singleton);

            services.AddMemoryCache();
            services.AddResponseCaching();

            services.AddAuthentication();

            services.AddIdentityConfiguration();

            // Authorization policies
            services.AddAutorizationPolicies();

            services.AddJWTAuthentication(Configuration);

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", builder =>
                {
                    builder.AllowAnyOrigin();
                    builder.AllowAnyMethod();
                    builder.AllowAnyHeader();
                });
            });


            services.AddAutoMapper(typeof(MapperInitializer));

            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IAuthManager, AuthManager>();

            services.AddSingleton<CachingProperties>();

            services.AddControllers().AddNewtonsoftJson(
                op => op.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
            
            services.AddVersioning();
            
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "RestaurantListingAPI", Version = "v1" });
            });

            services.AddVersioning();

            services.AddTransient<ITransientService, OperationService>();
            services.AddScoped<IScopedService, OperationService>();
            services.AddSingleton<ISingletonService, OperationService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseSeedMiddleware();
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "RestaurantListingAPI v1"));
            }

            app.UseCustomeExceptionHandler();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors();

            app.UseResponseCaching();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
