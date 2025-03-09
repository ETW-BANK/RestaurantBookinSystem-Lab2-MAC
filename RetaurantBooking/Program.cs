using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Restaurant.Data.Access.Data;
using Restaurant.Data.Access.DbInisializer;
using Restaurant.Services;
using ServiceRegisterExtension;

namespace RetaurantBooking
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("RestDb");
            builder.Services.AddDbContext<RestaurantDbContext>(options =>
                options.UseSqlServer(connectionString));

            // Add ASP.NET Identity services
            builder.Services.AddIdentity<IdentityUser, IdentityRole>() 
                .AddEntityFrameworkStores<RestaurantDbContext>()
                .AddDefaultTokenProviders();


            {

                builder.Services.AddControllers();

                // Register custom services and repositories

                builder.Services.AddScoped<IServicesRegisterExtension, ServiceRegisterExtension.ServiceRegisterExtension>();
                builder.Services.AddScoped<ICategoryService, CategoryService>();    

                //Register additional services
                var serviceProvider = builder.Services.BuildServiceProvider();
                var serviceRegisterExtension = serviceProvider.GetRequiredService<IServicesRegisterExtension>();
                serviceRegisterExtension.RegisterServices(builder.Services);
                
                // Swagger configuration for API documentation
                builder.Services.AddEndpointsApiExplorer();
                builder.Services.AddSwaggerGen();

                var app = builder.Build();

                // Configure the HTTP request pipeline.
                if (app.Environment.IsDevelopment())
                {
                    app.UseDeveloperExceptionPage();
                    app.UseSwagger();
                    app.UseSwaggerUI();
                }

                app.UseStaticFiles();

                app.UseAuthentication();
                app.UseAuthorization();

                SeedDatabase();

                app.MapControllers();

                app.Run();

               
                void SeedDatabase()
                {
                    using (var scope = app.Services.CreateScope())
                    {
                        var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitilizer>();
                        dbInitializer.Initialize();
                    }
                }
            }
        }
    }
}