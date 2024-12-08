using Microsoft.Extensions.DependencyInjection;
using Restaurant.Data.Access.Repository.Services.IServices;
using RestaurantServices.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceRegisterExtension
{
  public interface IServicesRegisterExtension
    {
       
            void RegisterServices(IServiceCollection services);
        
    }
}
