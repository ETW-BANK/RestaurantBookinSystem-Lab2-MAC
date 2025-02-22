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
    public class CategoriesController : Controller
    {
        private readonly HttpClient _httpClient;

        public CategoriesController(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://localhost:7232/api/Category/");
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            var response = await _httpClient.GetAsync("GetAllCategories");

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var serviceResponse = JsonConvert.DeserializeObject<List<CategoryVM>>(data);
                return Json(new { data = serviceResponse });
            }

            return Json(new { data = new List<CategoryVM>(), error = "Unable to retrieve categories from the server." });
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.PageTitle = "Create Category";
            ViewBag.ButtonLabel = "Create";

            return View(new CategoryVM());
        }


        [HttpPost]
      
        public async Task<IActionResult> Create(CategoryVM category)
        {
            if (category == null)
            {
                TempData["error"] = "Invalid category data!";
                return View(category);
            }

            // Check if Name is null or empty
            if (string.IsNullOrEmpty(category.Name))
            {
                TempData["error"] = "Category Name is required!";
                return View(category);
            }

            var content = new StringContent(JsonConvert.SerializeObject(category), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("CreateCategory", content);

            if (response.IsSuccessStatusCode)
            {
                TempData["success"] = "Category created successfully.";
                return RedirectToAction(nameof(Index));
            }

            TempData["error"] = "Something went wrong!";
            return View(category);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var response = await _httpClient.GetAsync($"GetCategory/{id}");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var CategoryResponse = JsonConvert.DeserializeObject<CategoryVM>(data);

                ViewBag.PageTitle = "Edit Category";
                ViewBag.ButtonLabel = "Update";

                if (CategoryResponse != null)
                {
                    return View("Create", CategoryResponse);
                }
            }

            return View("Error");
        }


        [HttpPost]
        public async Task<IActionResult> Edit(CategoryVM category)
        {
            var content = new StringContent(JsonConvert.SerializeObject(category), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync("UpdateCategory", content);

            if (response.IsSuccessStatusCode)
            {
                TempData["success"] = "Category Updated successfully";
                return RedirectToAction(nameof(Index));
            }

            return View("Create", category);
        }

    }
}
