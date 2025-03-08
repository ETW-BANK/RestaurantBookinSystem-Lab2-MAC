using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Restaurant.Models;
using Restaurant.Utility;
using RestaurantViewModels;
using System.Text;

namespace RestaurantBookingFrontApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = StaticData.Role_Admin)]
    public class CategoriesController : Controller
    {
        private readonly HttpClient _httpClient;

        public CategoriesController(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://localhost:44307/api/Category/");
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var response = await _httpClient.GetAsync("GetCategories");

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var categories = JsonConvert.DeserializeObject<List<CategoryVM>>(data);

                // Ensure full URL for each category's ImageUrl
                foreach (var category in categories)
                {
                    if (!string.IsNullOrEmpty(category.ImageUrl))
                    {
                        category.ImageUrl = $"https://localhost:44307{category.ImageUrl}";
                    }
                }

                return View(categories);
            }

            TempData["error"] = "Unable to retrieve categories from the server.";
            return View(new List<CategoryVM>());
        }





        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            var response = await _httpClient.GetAsync("GetCategories");

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var serviceResponse = JsonConvert.DeserializeObject<List<CategoryVM>>(data);

                return Json(new { data = serviceResponse });
            }

            return Json(new { data = new List<CategoryVM>(), error = "Unable to retrieve Tables from the server." });
        }




        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.PageTitle = "Create Table";
            ViewBag.ButtonLabel = "Create";

            return View(new CategoryVM());
        }

        [HttpPost]
        public async Task<IActionResult> Create(CategoryVM category)
        {
            var content = new StringContent(JsonConvert.SerializeObject(category), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("CreateCategory", content);

            if (response.IsSuccessStatusCode)
            {
                TempData["success"] = "Category Created successfully";
                return RedirectToAction(nameof(Index));
            }

            return View(category);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var response = await _httpClient.GetAsync($"GetCategory/{id}");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var categoryResponse = JsonConvert.DeserializeObject<CategoryVM>(data);

                ViewBag.PageTitle = "Edit Category";
                ViewBag.ButtonLabel = "Update";

                if (categoryResponse != null)
                {
                    return View("Create", categoryResponse);
                }
            }

            TempData["error"] = "Category not found.";
            return RedirectToAction(nameof(Index));
        }



        [HttpPost]
        public async Task<IActionResult> Edit(CategoryVM categoryvm)
        {
            var content = new StringContent(JsonConvert.SerializeObject(categoryvm), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync("UpdateCategory", content);

            if (response.IsSuccessStatusCode)
            {
                TempData["success"] = "Category Info Updated successfully";
                return RedirectToAction(nameof(Index));
            }

            return View("Create", categoryvm);

        }
    }
}