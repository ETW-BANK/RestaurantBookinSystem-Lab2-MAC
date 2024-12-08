using Microsoft.AspNetCore.Identity;
using Restaurant.Data.Access.Data;
using Restaurant.Data.Access.Repository;
using Restaurant.Data.Access.Repository.IRepository;
using Restaurant.Models;
using RestaurantServices.Services.IServices;


namespace RestaurantServices.Services
{
    public class UserService : IUserService
    {


        private IUnitOfWork _unitOfWork;
        private RestaurantDbContext _dbContext; 
        public UserService( IUnitOfWork unitOfWork,RestaurantDbContext dbContext)
            {
               
                _unitOfWork = unitOfWork;   
                _dbContext = dbContext;
            }

        public async Task<IEnumerable<ApplicationUser>> GetAllUsers()
        {
           
           IEnumerable<ApplicationUser> usersList = _unitOfWork.ApplicationUserRepository.GetAll().ToList();

            
            var userRoles = _dbContext.UserRoles.ToList(); 
            var roles = _dbContext.Roles.ToList();         

            
            foreach (var user in usersList)
            {
                var userRole = userRoles.FirstOrDefault(ur => ur.UserId == user.Id);
                if (userRole != null)
                {
                    var role = roles.FirstOrDefault(r => r.Id == userRole.RoleId);
                    if (role != null)
                    {
                        user.Role = role.Name; 
                    }
                }
            }

            return usersList;
        }

    }

}
