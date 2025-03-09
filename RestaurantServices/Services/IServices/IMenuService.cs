using Restaurant.Models;
using RestaurantViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantServices.Services.IServices
{
    public interface IMenuService
    {
        IEnumerable<MenuVM> GetAllMenues();

        category GetById(int id);

        void UpdateMenu(MenuVM menu);


        Task CreateMenu(MenuVM menu);

        category DeleteMenu(category menu);
    }
}
