using RestaurantViewModels;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using Restaurant.Models;

namespace Restaurant.Services
{
    public interface ICategoryService
    {


      Task  <IEnumerable<CategoryVM>> GetAll();

       Category GetById(int id);

        void Update(Category category);


        Task CreateCategory(CategoryVM category);

       Category DeleteCategory(Category category);


    }
}