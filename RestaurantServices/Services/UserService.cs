using Microsoft.AspNetCore.Identity;
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
           
            List<ApplicationUser> list=_dbContext.ApplicationUsers.ToList();

            var userroles=await _dbContext.UserRoles.ToListAsync();
            var roles=await _dbContext.Roles.ToListAsync(); 

            foreach (var user in list)
            {
                var roleid=await _dbContext.UserRoles.FirstOrDefaultAsync(r => r.UserId==user.Id);
                var role = await _dbContext.Roles.FirstOrDefaultAsync(u=>u.Id ==roleid.RoleId);



                var listOfUsers = await _dbContext.ApplicationUsers
                    .Select(user => new UserVm
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


                    })
                    .ToListAsync();

                return listOfUsers;
            }

            return null;

          
        }

        public UserVm GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void UpdateUser(UserVm user)
        {
            throw new NotImplementedException();
        }
    }
}
