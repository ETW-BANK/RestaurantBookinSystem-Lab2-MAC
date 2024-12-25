using RestaurantViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantServices.Services.IServices
{
    public interface IMybookingService
    {
        IEnumerable<MyBookingsVM> GetAll(); 
    }
}
