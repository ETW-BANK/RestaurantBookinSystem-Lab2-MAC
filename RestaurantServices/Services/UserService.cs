using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Restaurant.Data.Access.Data;
using Restaurant.Data.Access.Repository;
using Restaurant.Data.Access.Repository.IRepository;
using Restaurant.Models;
using RestaurantServices.Services.IServices;
using RestaurantViewModels;
using System.Security.Claims;


namespace RestaurantServices.Services
{
    public class UserService : IUserService
    {


        private IUnitOfWork _unitOfWork;
        private RestaurantDbContext _dbContext;

        public UserService(IUnitOfWork unitOfWork, RestaurantDbContext dbContext)
        {

            _unitOfWork = unitOfWork;
            _dbContext = dbContext;

        }

        public async Task<List<UserVm>> GetAllUsers()
        {
            // Fetch all users
            var usersList = _unitOfWork.ApplicationUserRepository.GetAll().ToList();

            // Fetch roles and user-role relationships
            var userRoles = _dbContext.UserRoles.ToList();
            var roles = _dbContext.Roles.ToList();

            // Map ApplicationUser to UserVm and assign roles
            var userVmList = usersList.Select(user =>
            {
                var userRole = userRoles.FirstOrDefault(ur => ur.UserId == user.Id);
                var role = userRole != null ? roles.FirstOrDefault(r => r.Id == userRole.RoleId)?.Name : null;

                return new UserVm
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    StreetAddress = user.StreetAddress,
                    City = user.City,
                    State = user.State,
                    PostalCode = user.PostalCode,
                    PhoneNumber = user.PhoneNumber,
                    Role = role
                };
            }).ToList();

            return userVmList;
        }

        public async Task<UserDetailsVM> GetUserDetails(string userId)
        {
            var user = await _dbContext.ApplicationUsers
                                       .Where(u => u.Id == userId)
                                       .FirstOrDefaultAsync();

            if (user == null)
            {
                return null; // Return null if the user is not found
            }

            // Mapping user data to UserDetailsVM
            return new UserDetailsVM
            {
                Id = user.Id,
                Name = user.Name,
                Phone = user.PhoneNumber,
                Email = user.Email
            };
        }
    }
}