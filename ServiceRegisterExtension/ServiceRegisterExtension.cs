using Microsoft.Extensions.DependencyInjection;
using RestaurantServices.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ServiceRegisterExtension
{
    public class ServiceRegisterExtension : IServicesRegisterExtension
    {

        public ServiceRegisterExtension()
        {
            
        }
        public void RegisterServices(IServiceCollection services)
        {
            //services.AddScoped<IUserService, UserService>();
            //services.AddScoped<ITableService, TableService>();
            //services.AddScoped<IBookingService, BookingService>();
        }
    }
}