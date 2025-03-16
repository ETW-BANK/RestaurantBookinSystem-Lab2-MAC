using Microsoft.AspNetCore.Http;
using Restaurant.Models;
using RestaurantViewModels;


namespace RestaurantServices.Services.IServices
{
    public interface IMenuService
    {
        Task<List<Menue>> GetAll();

        Task<MenuVM?> GetbyId(int? id);

        Task Update(MenuVM menu, IFormFile? file);


        Task CreateMenue(MenuVM menueVM, IFormFile? file);

       void DeleteMenue(int? id);    
    }
}
