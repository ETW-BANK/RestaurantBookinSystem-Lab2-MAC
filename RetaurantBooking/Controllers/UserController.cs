
using Microsoft.AspNetCore.Mvc;
using Restaurant.Data.Access.Repository.IRepository;
using Restaurant.Models;
using RestaurantServices.Services.IServices;

namespace RetaurantBooking.Controllers
{
    [Area("Admin")]
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
      
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserService _userservice;
        public UserController(  IUnitOfWork unitOfWork,IUserService userService)
        {
          
            _unitOfWork = unitOfWork;
            _userservice = userService; 
       
        }

        [HttpGet]

        public async Task<IActionResult> GetSingleUser(string Id)
        {
            var user =  _unitOfWork.ApplicationUserRepository.GetFirstOrDefault(u => u.Id == Id);

            if (user == null)
            {
                return BadRequest("User not found");

            }

            return Ok(user);
        }
      
        [HttpGet]
        public  async Task<IActionResult> GetAllUsers()
        {
           IEnumerable<ApplicationUser> userlist= await _userservice.GetAllUsers();
           

            if (userlist==null )
            {
                return NotFound("No users found.");
            }

            return Ok(userlist);
        } 






        //[HttpPost]
        //public async Task<IActionResult> LockUser([FromBody] string id)
        //{
        //    var usertolock = await _dbContext.ApplicationUsers.FirstOrDefaultAsync(x => x.Id == id);

        //    if (usertolock == null)
        //    {
        //        return BadRequest();
        //    }
        //    if(usertolock.LockoutEnd!=null && usertolock.LockoutEnd > DateTime.Now)
        //    {
        //        usertolock.LockoutEnd = DateTime.Now;   
        //    }

        //    else
        //    {
        //        usertolock.LockoutEnd=DateTime.Now.AddDays(5);
        //    }

        //    return Ok(usertolock);
        //}



        //[HttpGet]
        //public async Task<IActionResult> RoleManagment(string userId)
        //{
        //    try
        //    {
        //        var userRoles = await _userService.RoleManagment(userId); 
        //        if (userRoles == null)
        //        {
        //            return NotFound(new { message = "User not found" });
        //        }

        //        return Ok(new { data = userRoles });
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(new { message = ex.Message });
        //    }
        //}

        //[HttpPost]
        //public async  Task<IActionResult> RoleManagment(RoleManagmentVM roleManagmentVM)
        //{
        //   var newrole=_userService.RoleManagment(roleManagmentVM.ApplicationUser.Id);

        //    if (newrole == null)
        //    {
        //        return BadRequest();
        //    }
        //    else
        //    {
        //      _userService.UpdateUserRole(roleManagmentVM);

        //        return Ok();
        //    }
        //}
    }
}
