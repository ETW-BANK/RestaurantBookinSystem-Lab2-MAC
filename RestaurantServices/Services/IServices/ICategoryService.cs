using RestaurantViewModels;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using Restaurant.Models;

namespace Restaurant.Services
{
    public interface ICategoryService
    {


        IEnumerable<Category> GetAll();

       Category GetById(int id);

        void Update(Category category);


         void CreateCategory(CategoryVM category,IFormFile? file);

       Category DeleteCategory(Category category);


    }
}