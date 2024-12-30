
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
            var user = _userService.GetUser(id);
               

            if (user == null)
            {
                return NotFound(new { message = "User not found." });
            }

           

            return Ok(user);
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userService.GetAllUsers();
            if (users == null || !users.Any())
            {
                return NotFound(new { message = "No users found." });
            }

            return Ok(new { data = users });
        }

       
    }
}
