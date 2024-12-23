
using RestaurantViewModels;



namespace RestaurantServices.Services.IServices
{
  public interface IUserService
    {

        Task<List<UserVm>> GetAllUsers();

       


    }
}
