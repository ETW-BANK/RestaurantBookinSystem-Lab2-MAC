using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Restaurant.Data.Access.Data;
using Restaurant.Data.Access.Repository.IRepository;
using Restaurant.Models;
using RestaurantServices.Services.IServices;
using RestaurantViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using Microsoft.Identity.Client;
using System.Data;

namespace RestaurantServices.Services
{
    public class UserService : IUserService
    {
      
        private IUnitOfWork _unitOfWork;
       private readonly RestaurantDbContext _dbContext;
        private readonly UserManager<IdentityUser> _userManager;


        public UserService(RestaurantDbContext dbContext,IUnitOfWork unitOfWork,UserManager<IdentityUser> userManager)
        {
            _unitOfWork = unitOfWork;
          _dbContext= dbContext;  
            _userManager= userManager;
         
       
          
        }
        public void CreateUser(UserVm user)
        {
            throw new NotImplementedException();
        }

        public void DeleteUser(int id)
        {
            throw new NotImplementedException();
        }



        public async Task<IEnumerable<UserVm>> GetAllUsers()
        {

            List<ApplicationUser> users = await _dbContext.ApplicationUsers.ToListAsync();
            List<IdentityUserRole<string>> userRoles = await _dbContext.UserRoles.ToListAsync();
            List<IdentityRole> roles = await _dbContext.Roles.ToListAsync();
             

        List<UserVm> listOfUsers = new List<UserVm>();

            foreach (var user in users)
            {

                var userRole = userRoles.FirstOrDefault(r => r.UserId == user.Id);
                if (userRole != null)
                {
                    var role = roles.FirstOrDefault(r => r.Id == userRole.RoleId);
                    if (role != null)
                    {

                        listOfUsers.Add(new UserVm
                        {
                            Id = user.Id,
                            Name = user.Name,
                            Email = user.Email,
                            StreetAddress = user.StreetAddress,
                            City = user.City,
                            State = user.State,
                            PostalCode = user.PostalCode,
                            PhoneNumber = user.PhoneNumber,
                            Role = role.Name
                        });
                    }
                }
            }


            return listOfUsers;
        }



        public async Task<UserVm> GetById(string id)
        {
            ApplicationUser user = await _dbContext.ApplicationUsers.FirstOrDefaultAsync(x => x.Id == id);

            if (user == null)
            {
                return null;
            }
            IdentityUserRole<string> userRole = await _dbContext.UserRoles.FirstOrDefaultAsync(u => u.UserId == id);

            IdentityRole role = null;

            if (userRole != null)
            {
                role = await _dbContext.Roles.FirstOrDefaultAsync(r => r.Id == userRole.RoleId);
            }


            if (user != null)
            {
                var userVm = new UserVm
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    City = user.City,
                    State = user.State,
                    StreetAddress = user.StreetAddress,
                    PhoneNumber = user.PhoneNumber,
                    PostalCode = user.PostalCode,
                    Role = role?.Name,


                };


                return userVm;


            }

            return null;
        }

        public void UpdateRole(RoleManagmentVM roles)
        {
            string currentRoleId = _dbContext.UserRoles.FirstOrDefault(u => u.UserId == roles.ApplicationUser.Id)?.RoleId;
            string oldRole = _dbContext.Roles.FirstOrDefault(x => x.Id == currentRoleId)?.Name;

            if (roles.ApplicationUser.Role != oldRole)
            {
                var applicationUser = _dbContext.ApplicationUsers.FirstOrDefault(u => u.Id == roles.ApplicationUser.Id);

                if (applicationUser == null) throw new Exception("User not found.");

                _userManager.RemoveFromRoleAsync(applicationUser, oldRole).GetAwaiter().GetResult();
                _userManager.AddToRoleAsync(applicationUser, roles.ApplicationUser.Role).GetAwaiter().GetResult();

                // Save changes to make sure role assignment is persisted
                _dbContext.SaveChanges();
            }
        }














        public async Task LockUnlock(string userId)
        {
            var userFromDb = await _dbContext.ApplicationUsers.FirstOrDefaultAsync(u=>u.Id == userId);

           
            if(userFromDb.LockoutEnd!=null && userFromDb.LockoutEnd > DateTime.Now)
            {
                userFromDb.LockoutEnd = DateTime.Now;   
            }
            else
            {
                userFromDb.LockoutEnd= DateTime.Now.AddDays(5);
            }
           
                

            await _dbContext.SaveChangesAsync();
        }


    }
}
