
using Restaurant.Models;
using RestaurantViewModels;



namespace RestaurantServices.Services.IServices
{
  public interface IUserService
    {

        Task<List<UserVm>> GetAllUsers();

        UserVm GetUser(string id);


    }
}
