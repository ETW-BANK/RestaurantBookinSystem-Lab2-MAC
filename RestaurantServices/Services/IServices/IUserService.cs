using Restaurant.Models;
using RestaurantViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantServices.Services.IServices
{
  public interface IUserService
    {

        Task<IEnumerable<UserVm>> GetAllUsers();

      UserVm GetById(int id);

        void UpdateUser(UserVm user);


        void CreateUser(UserVm user);

        void DeleteUser(int id);

        Task<RoleManagmentVM> RoleManagment(string userId);

        Task LockUnlock(string userId); 
    }
}
