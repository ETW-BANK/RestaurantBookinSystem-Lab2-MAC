using RestaurantViewModels;
using Microsoft.AspNetCore.Http;
using Restaurant.Models;

namespace Restaurant.Services
{
    public interface ICategoryService
    {


      Task  <IEnumerable<CategoryVM>> GetAll();

       Category GetById(int? id);

        void Update(CategoryVM category,IFormFile? file);


        Task CreateCategory(CategoryVM category, IFormFile? file);

        Task<Category> DeleteCategory(Category category);


    }
}