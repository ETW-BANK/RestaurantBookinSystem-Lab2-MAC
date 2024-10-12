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
              
                var listOfUsers = await _dbContext.ApplicationUsers
                    .Select(x => new UserVm
                    {
                        Id = x.Id,
                        Name = x.Name,  
                        Email = x.Email,
                        StreetAddress = x.StreetAddress,
                        City = x.City,
                        State = x.State,
                        PostalCode = x.PostalCode,
                        PhoneNumber = x.PhoneNumber
                    })
                    .ToListAsync();  

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
    }
}
