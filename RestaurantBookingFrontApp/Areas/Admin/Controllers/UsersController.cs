using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Restaurant.Utility;
using RestaurantViewModels;

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
    }
  }
