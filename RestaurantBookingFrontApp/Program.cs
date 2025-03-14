using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Restaurant.Data.Access.Data;
using Restaurant.Utility;
using Restaurant.Services;
using ServiceRegisterExtension;

namespace RestaurantBookingFrontApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var connectionstring = builder.Configuration.GetConnectionString("RestDb");
            builder.Services.AddDbContext<RestaurantDbContext>(option => option.UseSqlServer(connectionstring));

            builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<RestaurantDbContext>().AddDefaultTokenProviders();

            builder.Services.AddScoped<IEmailSender, EmailSender>();
            builder.Services.AddHttpClient();


            builder.Services.AddScoped<IServicesRegisterExtension, ServiceRegisterExtension.ServiceRegisterExtension>();
            builder.Services.AddScoped<ICategoryService, CategoryService>();

            //Register additional services
            var serviceProvider = builder.Services.BuildServiceProvider();
            var serviceRegisterExtension = serviceProvider.GetRequiredService<IServicesRegisterExtension>();
            serviceRegisterExtension.RegisterServices(builder.Services);

            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages();

            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = $"/Identity/Account/Login";
                options.LogoutPath = $"/Identity/Account/Logout";
                options.AccessDeniedPath = $"/Identity/Account/AccessDenied";

            });
         
        
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

           
            app.UseRouting();

            app.MapRazorPages();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllerRoute(
                name: "default",
                pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}