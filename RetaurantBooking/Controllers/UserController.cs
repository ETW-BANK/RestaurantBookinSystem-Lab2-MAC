
using Microsoft.AspNetCore.Mvc;
using RestaurantServices.Services.IServices;

namespace RetaurantBooking.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("GetSingleUser/{id}")]
        public IActionResult GetSingleUser(string id)
        {
            var user = _userService.GetAllUsers()
                .Result
                .FirstOrDefault(u => u.Id == id);

            if (user == null)
            {
                return NotFound(new { message = "User not found." });
            }

            // Serialize and return complete user details
            var userDetails = new
            {
                user.Id,
                user.Name,
                user.Email,
              
            };

            return Ok(userDetails);
        }
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
