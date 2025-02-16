using Microsoft.AspNetCore.Http;
using Restaurant.Models;
using RestaurantViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantServices.Services.IServices
{
    public interface ICategoryService
    {
        IEnumerable<Category> GetAllCategories();

        Category GetById(int id);

        void UpdateCategory(CategoryVM category);


        Task CreateCategory(CategoryVM category);

        Category DeleteCategory(Category menu);
    }
}
