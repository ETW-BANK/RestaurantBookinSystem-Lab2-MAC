using Restaurant.Data.Access.Data;
using Restaurant.Data.Access.Repository.IRepository;
using RestaurantServices.Services.IServices;
using RestaurantViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantServices.Services
{
    public class MybookingsService : IMybookingService
    {
        private IUnitOfWork _unitOfWork;
        private IUserService _userService;
        private RestaurantDbContext _context;
        public MybookingsService(IUnitOfWork unitOfWork,IUserService userService,RestaurantDbContext context)
        {
            _unitOfWork = unitOfWork;
            _userService = userService; 
            _context = context;
        }
        public IEnumerable<MyBookingsVM> GetAll()
        {
            
          
            throw new NotImplementedException();
        }
    }
}
