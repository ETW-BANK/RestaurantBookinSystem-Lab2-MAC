using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Restaurant.Utility;
using RestaurantBookingApp.Models.ViewModel;
using System.Text;

namespace RestaurantBookingApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MenusController : Controller
    {
        private readonly HttpClient _httpClient;

        public MenusController(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://localhost:7232/api/FoodMenu/");
        }

        [HttpGet]
        public IActionResult Index() => View();

        [HttpGet]
        public async Task<IActionResult> GetAllMenu()
        {
            var response = await _httpClient.GetAsync("GetAllMenues");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var serviceResponse = JsonConvert.DeserializeObject<ServiceResponse<List<MenuVM>>>(data);
                return Json(serviceResponse.Success ? new { data = serviceResponse.Data } : new { data = new List<CustomerVM>(), error = serviceResponse.Message });
            }

            return Json(new { data = new List<MenuVM>(), error = "Unable to retrieve Menu from the server." });
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.PageTitle = "Create Menu";
            ViewBag.ButtonLabel = "Create";

            return View(new MenuVM());
        }


        [HttpPost]
        public async Task<IActionResult> Create(MenuVM menu)
        {
            var content = new StringContent(JsonConvert.SerializeObject(menu), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("CreateNewMenu", content);
            TempData["success"] = "Menu Created successfully";
            return response.IsSuccessStatusCode ? RedirectToAction(nameof(Index)) : View(menu);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var response = await _httpClient.GetAsync($"GetMenu/{id}");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var menuResponse = JsonConvert.DeserializeObject<ServiceResponse<MenuVM>>(data);

                ViewBag.PageTitle = "Edit Menu";
                ViewBag.ButtonLabel = "Update";

                if (menuResponse?.Data != null)
                {
                    return View("Create", menuResponse.Data);
                }
            }

            return View("Error");
        }



        [HttpPost]
        public async Task<IActionResult> Edit(MenuVM menu)
        {
            var content = new StringContent(JsonConvert.SerializeObject(menu), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync("Update", content);

            if (response.IsSuccessStatusCode)
            {

                TempData["success"] = "menu Info Updated successfully";
            }
            return response.IsSuccessStatusCode ? RedirectToAction("Index") : View(menu);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var response = await _httpClient.GetAsync($"GetMenu/{id}");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var serviceResponse = JsonConvert.DeserializeObject<ServiceResponse<MenuVM>>(data);
                if (serviceResponse.Success)
                {

                    return View(serviceResponse.Data);
                }
            }

            return View("Error");
        }


        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var response = await _httpClient.DeleteAsync($"DeleteMenu/{id}");
            if (response.IsSuccessStatusCode)
            {
                TempData["success"] = "Menu Deleted successfully";
                return RedirectToAction("Index");
            }

            return View("Error");
        }

    }
}
