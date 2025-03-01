using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Restaurant.Data.Access.Data;
using Restaurant.Data.Access.DbInisializer;
using Restaurant.Data.Access.Repository;
using Restaurant.Data.Access.Repository.IRepository;
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
            builder.Services.AddIdentity<IdentityUser, IdentityRole>() // Use your custom ApplicationUser class here
                .AddEntityFrameworkStores<RestaurantDbContext>()
                .AddDefaultTokenProviders();

            builder.Services.AddControllers();

            // Register custom services and repositories
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IServicesRegisterExtension, ServiceRegisterExtension.ServiceRegisterExtension>();
           

            // Register additional services
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

            

            // Add authentication and authorization middleware
            app.UseAuthentication();
            app.UseAuthorization();

            // Seed the database on application startup
            SeedDatabase();

            // Map the controllers for routing
            app.MapControllers();

            // Start the application
            app.Run();

            // Seed the database with initial data
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