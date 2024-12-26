
using Restaurant.Data.Access.Data;
using Restaurant.Data.Access.Repository.IRepository;
using Restaurant.Models;
using RestaurantServices.Services.IServices;
using RestaurantViewModels;


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
           
            var usersList = _unitOfWork.ApplicationUserRepository.GetAll().ToList();
            var userRoles = _dbContext.UserRoles.ToList();
            var roles = _dbContext.Roles.ToList();
            
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

        public UserVm GetUser(string id)
        {
            var user = _unitOfWork.ApplicationUserRepository.GetFirstOrDefault(x=>x.Id== id);   

            if(user == null)
            {
                throw new Exception("User Not Found");
            }
            
            var userinfo = new UserVm
            {
                Id = user.Id,
                Name = user.UserName,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email,
                StreetAddress= user.StreetAddress,  
                City = user.City,
                State = user.State,
                PostalCode = user.PostalCode,

            };

            return userinfo;    
        }
    }
    }
