using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Restaurant.Data.Access.Repository.IRepository;
using Restaurant.Data.Access.Repository.Services;
using Restaurant.Data.Access.Repository.Services.IServices;
using Restaurant.Models;
using RestaurantServices.Services.IServices;
using RestaurantViewModels;

namespace RetaurantBooking.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IUnitOfWork _unitOfWork;
       
      
        public UserController(IUserService userService, IUnitOfWork unitOfWork)
        {
           _userService = userService;  
            _unitOfWork = unitOfWork;
          
           
         
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            IEnumerable<UserVm> users = await _userService.GetAllUsers();

            return Ok(users);
        }
        [HttpPost]
        public async Task<IActionResult> LockUser([FromBody]string id)
        {
          
            await _userService.LockUnlock(id);
       
           
            return Ok();
        }

        [HttpGet("{userId}")] // 'userId' corresponds to the route parameter
        public async Task<IActionResult> RoleManagement(string userId)
        {
            try
            {
                
                RoleManagmentVM roleVm = await _userService.RoleManagment(userId);

              
                return Ok(roleVm);
            }
            catch (Exception ex)
            {
                
                return BadRequest(new { error = ex.Message });
            }
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
