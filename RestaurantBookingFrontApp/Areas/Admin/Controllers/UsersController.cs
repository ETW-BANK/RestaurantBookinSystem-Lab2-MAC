using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using Restaurant.Models;
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
        public async Task<IActionResult> GetAllUser()
        {
            try
            {
                var response = await _httpClient.GetAsync("GetUsers");

                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();


                    var serviceResponse = JsonConvert.DeserializeObject<ApiResponse<List<UserVm>>>(data);


                    return Json(new { data = serviceResponse.Data });
                }

                return StatusCode((int)response.StatusCode, "Failed to fetch users from the API.");
            }
            catch (Exception ex)
            {

                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
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
            // Validate the input model
            if (roleVm == null || roleVm.ApplicationUser == null || roleVm.RoleList == null || !roleVm.RoleList.Any())
            {
                TempData["error"] = "Invalid role management data.";
                return Json(new { success = false, userId = roleVm?.ApplicationUser?.Id });
            }

            try
            {
                // Log the incoming payload for debugging
                var jsonPayload = JsonConvert.SerializeObject(roleVm);
                Console.WriteLine(jsonPayload); // Debug log

                // Create the request content
                var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                // Make the POST request to update the role
                var response = await _httpClient.PostAsync("RoleManagement", content); // Ensure the URL matches the API route

                // Check the response for success
                if (response.IsSuccessStatusCode)
                {
                    TempData["success"] = "User role updated successfully.";
                    return Json(new { success = true, message = "User role updated successfully." });
                }

                // Log and handle the error response
                var errorMessage = await response.Content.ReadAsStringAsync();
                TempData["error"] = $"Error updating role: {errorMessage}";
                return Json(new { success = false, userId = roleVm.ApplicationUser.Id, message = errorMessage });
            }
            catch (Exception ex)
            {
                // Capture and log exceptions
                TempData["error"] = $"Exception occurred: {ex.Message}";
                return Json(new { success = false, userId = roleVm.ApplicationUser.Id, message = ex.Message });
            }
        }




    }
}