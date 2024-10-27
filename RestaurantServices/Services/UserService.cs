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
      
   
       private readonly RestaurantDbContext _dbContext;
        private readonly UserManager<IdentityUser> _userManager;


        public UserService(RestaurantDbContext dbContext,UserManager<IdentityUser> userManager)
        {
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

        public async  Task<RoleManagmentVM> RoleManagment(string userId)
        {
           string RoleID =  _dbContext.UserRoles.FirstOrDefault(x => x.UserId == userId).RoleId;

            RoleManagmentVM RoleVM = new RoleManagmentVM()
            {

                ApplicationUser = _dbContext.ApplicationUsers.FirstOrDefault(u => u.Id == userId),

                RoleList = _dbContext.Roles.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Name
                }),
            };

            RoleVM.ApplicationUser.Role = _dbContext.Roles.FirstOrDefault(u => u.Id == RoleID).Name;
          
            return RoleVM;
        }

        public async Task UpdateUserRole(RoleManagmentVM roleManagmentVM)
        {
            // Get the current role
            string RoleID = _dbContext.UserRoles.FirstOrDefault(u => u.UserId == roleManagmentVM.ApplicationUser.Id)?.RoleId;
            string oldRole = _dbContext.Roles.FirstOrDefault(u => u.Id == RoleID)?.Name;

            if (oldRole != roleManagmentVM.ApplicationUser.Role)
            {
                ApplicationUser applicationUser = _dbContext.ApplicationUsers.FirstOrDefault(u => u.Id == roleManagmentVM.ApplicationUser.Id);

                if (applicationUser != null)
                {
                    _userManager.RemoveFromRoleAsync(applicationUser, oldRole).GetAwaiter().GetResult();
                   _userManager.AddToRoleAsync(applicationUser, roleManagmentVM.ApplicationUser.Role).GetAwaiter().GetResult();
                }
            }
        }




    }
}
