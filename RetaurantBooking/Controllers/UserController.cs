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
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly RestaurantDbContext _dbContext;    
       
      
        public UserController(IUserService userService, IUnitOfWork unitOfWork,RestaurantDbContext dbcontext)
        {
           _userService = userService;  
            _unitOfWork = unitOfWork;
            _dbContext = dbcontext;
          
           
         
        }

        [HttpGet]

        public async Task<IActionResult> GetSingleUser(string Id)
        {
            var user= await _userService.GetById(Id); 

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


      

        [HttpPut("UpdateRole")]
        public IActionResult UpdateRole([FromBody] RoleManagmentVM roleVm)
        {
            try
            {
                _userService.UpdateRole(roleVm);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

    }
}
