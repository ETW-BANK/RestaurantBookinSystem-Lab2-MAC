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

namespace RestaurantServices.Services
{
    public class UserService : IUserService
    {
      
        private IUnitOfWork _unitOfWork;
       private readonly RestaurantDbContext _dbContext; 
      

        public UserService(RestaurantDbContext dbContext,IUnitOfWork unitOfWork,RestaurantDbContext dbContext1 )
        {
            _unitOfWork = unitOfWork;
          _dbContext= dbContext;    
       
          
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



        public UserVm GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void UpdateUser(UserVm user)
        {
            throw new NotImplementedException();
        }

        public async Task<RoleManagmentVM> RoleManagment(string userId)
        {

            string RoleID = _dbContext.UserRoles.FirstOrDefault(u => u.UserId == userId).RoleId;

            RoleManagmentVM RoleVM = new RoleManagmentVM()
            {
                ApplicationUser=_dbContext.ApplicationUsers.FirstOrDefault(u=>u.Id==userId),
                RoleList=_dbContext.Roles.Select(i=>new SelectListItem
                {

                    Text = i.Name,  
                    Value=i.Name
                }),

            };

            RoleVM.ApplicationUser.Role = _dbContext.Roles.FirstOrDefault(u => u.Id == RoleID).Name;

            return RoleVM;  

         
        }


       

           

          
        




        public async Task LockUnlock(string userId)
        {
            var userFromDb = await _dbContext.ApplicationUsers.FirstOrDefaultAsync(u => u.Id == userId);

            if (userFromDb != null)
            {
                if (userFromDb.LockoutEnd != null && userFromDb.LockoutEnd > DateTime.Now)
                {
                
                    userFromDb.LockoutEnd = DateTime.Now;
                }
                else
                {
                    userFromDb.LockoutEnd = DateTime.Now.AddYears(1000);
                }

                await _dbContext.SaveChangesAsync();
            }
        }

    }
}
