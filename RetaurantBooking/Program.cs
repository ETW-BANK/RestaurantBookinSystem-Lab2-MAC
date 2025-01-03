using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
          

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IServicesRegisterExtension, ServiceRegisterExtension.ServiceRegisterExtension>();
            builder.Services.AddScoped<IDbInitilizer, DbInitializer>();
            var serviceProvider = builder.Services.BuildServiceProvider();
            var serviceRegisterExtension = serviceProvider.GetRequiredService<IServicesRegisterExtension>();
            serviceRegisterExtension.RegisterServices(builder.Services);

           


            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication(); // Make sure authentication middleware is added
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
