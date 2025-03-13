
using Restaurant.Models;
using RestaurantViewModels;



namespace RestaurantServices.Services.IServices
{
  public interface IUserService
    {

        Task<IEnumerable<UserVm>> GetAllUsers();

        UserVm GetUser(string id);


    }
}
