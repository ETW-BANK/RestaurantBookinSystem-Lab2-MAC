using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        public IActionResult Index() => View();


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

            return Json(new { data = new List<UserVm>(), error = "Unable to retrieve Tables from the server." });
        }
        //[HttpGet]
        //public async Task<IActionResult> RoleManagment(string userId)
        //{
        //    try
        //    {
        //        var response = await _httpClient.GetAsync($"RoleManagement/{userId}");

        //        if (response.IsSuccessStatusCode)
        //        {

        //            var data = await response.Content.ReadAsStringAsync();
        //            var roleVm = JsonConvert.DeserializeObject<RoleManagmentVM>(data);


        //            return View(roleVm);
        //        }
        //        else
        //        {
        //            TempData["Error"] = "Unable to retrieve role management data.";
        //            return View(new RoleManagmentVM());
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        TempData["Error"] = "An error occurred: " + ex.Message;
        //        return View(new RoleManagmentVM());
        //    }
        //}
        //GET Method to retrieve user role information
       [HttpGet]
        public async Task<IActionResult> RoleManagment(string userId)
        {
            try
            {
                // Make an HTTP GET request to retrieve the user role data
                var response = await _httpClient.GetAsync($"RoleManagement/{userId}");

                if (response.IsSuccessStatusCode)
                {
                    // Deserialize the response to RoleManagmentVM
                    var data = await response.Content.ReadAsStringAsync();
                    var roleVm = JsonConvert.DeserializeObject<RoleManagmentVM>(data);

                    // Return the view with the role data
                    return View(roleVm);
                }
                else
                {
                    // Handle the case when the response is not successful
                    TempData["Error"] = "Unable to retrieve role management data.";
                    return View(new RoleManagmentVM());
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during the process
                TempData["Error"] = "An error occurred: " + ex.Message;
                return View(new RoleManagmentVM());
            }
        }


        [HttpPost]
        public async Task<IActionResult> UpdateRole(RoleManagmentVM roleVm)
        {
            try
            {
              
                var content = new StringContent(JsonConvert.SerializeObject(roleVm), Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("RoleManagement/UpdateRole", content);

                if (response.IsSuccessStatusCode)
                {
                    
                    TempData["Success"] = "User role updated successfully.";

              
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                  
                    TempData["Error"] = "Failed to update the role. Please try again.";
                    return View("RoleManagment", roleVm);
                }
            }
            catch (Exception ex)
            {
               
                TempData["Error"] = "An error occurred: " + ex.Message;
                return View("RoleManagment", roleVm);
            }
        }



        [HttpPost]
        public async Task<IActionResult> LockUnlock([FromBody]string id)
        {
            var content = new StringContent(JsonConvert.SerializeObject(id), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"LockUser?id={id}", content);

            if (response.IsSuccessStatusCode)
            {

                TempData["success"] = "User has been successfully locked/unlocked.";
                return Ok();
              
            }
            else
            {
                TempData["Error"] = "User has NOT been successfully locked/unlocked.";

                return BadRequest();
            }
        }



    }
  }
