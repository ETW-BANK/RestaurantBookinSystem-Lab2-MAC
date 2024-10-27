using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using Restaurant.Utility;
using RestaurantViewModels;
using System.Text;

namespace RestaurantBookingFrontApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = StaticData.Role_Admin)]
    public class UsersController : Controller
    {
        private readonly HttpClient _httpClient;

        public UsersController(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://localhost:7232/api/User/");
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var response = await _httpClient.GetAsync("GetAllUsers");

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var serviceResponse = JsonConvert.DeserializeObject<List<UserVm>>(data);

                return Json(new { data = serviceResponse });
            }

            return Json(new { data = new List<UserVm>(), error = "Unable to retrieve users from the server." });
        }


        [HttpGet]
        public async Task<IActionResult> RoleManagement(string userId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"RoleManagment?userId={userId}");

                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();

                   
                    var result = JsonConvert.DeserializeObject<dynamic>(data);

                   
                    var roleVm = result?.data.ToObject<RoleManagmentVM>();

                    if (roleVm != null)
                    {
                        
                        ViewBag.PageTitle = "Role Management";
                        return View(roleVm); 
                    }
                }

               
                TempData["Error"] = "Unable to retrieve role management data.";
                return RedirectToAction("Index"); 
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("Index"); 
            }
        }




        [HttpPost]
        public async Task<IActionResult> RoleManagement(RoleManagmentVM roleVm)
        {
            var content = new StringContent(JsonConvert.SerializeObject(roleVm), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("UpdateRole", content);

            if (response.IsSuccessStatusCode)
            {
                TempData["success"] = "Role updated successfully.";
                return RedirectToAction("Index");
            }

            TempData["error"] = "Error updating role.";
            return BadRequest(new { success = false, message = "Error updating role." });
        }

        [HttpPost]
        public async Task<IActionResult> LockUnlock(string userId)
        {
            var content = new StringContent(JsonConvert.SerializeObject(userId), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("lock-unlock", content);

            if (response.IsSuccessStatusCode)
            {
                TempData["success"] = "User has been successfully locked/unlocked.";
                return Ok(new { success = true, message = "User has been locked/unlocked." });
            }

            TempData["error"] = "User could not be locked/unlocked.";
            return BadRequest(new { success = false, message = "Failed to lock/unlock user." });
        }
    }
}
