using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Restaurant.Data.Access.Data;
using Restaurant.Data.Access.Repository.IRepository;
using Restaurant.Data.Access.Repository.Services;
using Restaurant.Data.Access.Repository.Services.IServices;
using Restaurant.Models;
using RestaurantServices.Services.IServices;
using RestaurantViewModels;
using System.Text.Json.Serialization;
using System.Web.Helpers;

namespace RetaurantBooking.Controllers
{
    [Area("Admin")]
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly RestaurantDbContext _dbContext;    
        private readonly UserManager<IdentityUser> _userManager;
       
      
        public UserController(IUserService userService, UserManager<IdentityUser> userManager, IUnitOfWork unitOfWork,RestaurantDbContext dbcontext)
        {
           _userService = userService;  
            _unitOfWork = unitOfWork;
            _dbContext = dbcontext;
            _userManager = userManager;
          
           
         
        }

        [HttpGet]

        public async Task<IActionResult> GetSingleUser(string Id)
        {
            var user = await _userService.GetById(Id);

            if (user == null)
            {
                return BadRequest("User not found");

            }

            return Ok(user);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            IEnumerable<UserVm> users = await _userService.GetAllUsers();

            return Ok(users);
        }


        


      
        [HttpPost]
        public async Task<IActionResult> LockUser([FromBody] string id)
        {
            var usertolock = await _dbContext.ApplicationUsers.FirstOrDefaultAsync(x => x.Id == id);

            if (usertolock == null)
            {
                return BadRequest();
            }
            if(usertolock.LockoutEnd!=null && usertolock.LockoutEnd > DateTime.Now)
            {
                usertolock.LockoutEnd = DateTime.Now;   
            }

            else
            {
                usertolock.LockoutEnd=DateTime.Now.AddDays(5);
            }

            return Ok(usertolock);
        }



        [HttpGet]
        public async Task<IActionResult> RoleManagment(string userId)
        {
            try
            {
                var userRoles = await _userService.RoleManagment(userId); 
                if (userRoles == null)
                {
                    return NotFound(new { message = "User not found" });
                }

                return Ok(new { data = userRoles });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost]
        public async  Task<IActionResult> RoleManagment(RoleManagmentVM roleManagmentVM)
        {
           var newrole=_userService.RoleManagment(roleManagmentVM.ApplicationUser.Id);

            if (newrole == null)
            {
                return BadRequest();
            }
            else
            {
              _userService.UpdateUserRole(roleManagmentVM);

                return Ok();
            }
        }
    }
}
