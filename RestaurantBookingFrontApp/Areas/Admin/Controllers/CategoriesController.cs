using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Restaurant.Utility;
using RestaurantViewModels;
using System.Net.Http.Headers;
using System.Text;


namespace RestaurantBookingFrontApp.Controllers
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
            _httpClient.Timeout = TimeSpan.FromMinutes(5);
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                var response = await _httpClient.GetAsync("GetCategories");

                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var categories = JsonConvert.DeserializeObject<List<CategoryVM>>(data);

                  
                 

                    return View(categories);
                }
                else
                {
                    TempData["error"] = "Unable to retrieve categories from the server.";
                    return View(new List<CategoryVM>());
                }
            }
            catch (Exception ex)
            {
                TempData["error"] = $"An error occurred: {ex.Message}";
                return View(new List<CategoryVM>());
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new CategoryVM());
        }


        [HttpPost]
        public async Task<IActionResult> Create(CategoryVM categoryVM, IFormFile file)
        {
            if (!ModelState.IsValid)
            {
                return View(categoryVM);
            }

            try
            {
                using (var formData = new MultipartFormDataContent())
                {
                
                    formData.Add(new StringContent(categoryVM.Name), "Name");
                    formData.Add(new StringContent(categoryVM.DisplayOrder.ToString()), "DisplayOrder");

                  
                    if (file != null)
                    {
                        var fileContent = new StreamContent(file.OpenReadStream());
                        fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse(file.ContentType);
                        formData.Add(fileContent, "Image", file.FileName);
                    }

                
                    var response = await _httpClient.PostAsync("Create", formData);

                    if (response.IsSuccessStatusCode)
                    {
                        TempData["success"] = "Category created successfully.";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        var errorResponse = await response.Content.ReadAsStringAsync();
                        TempData["error"] = $"Failed to create category. Error: {errorResponse}";
                        return View(categoryVM);
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["error"] = $"An error occurred while creating the category. Error: {ex.Message}";
                return View(categoryVM);
            }
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var response = await _httpClient.GetAsync($"GetById/{id}");

            if (response.IsSuccessStatusCode)
            {
                var category = await response.Content.ReadFromJsonAsync<CategoryVM>();
                return View(category);
            }

            TempData["error"] = "Category not found.";
            return RedirectToAction(nameof(Index));
        }

       
        [HttpPost]
        public async Task<IActionResult> Edit(int id, CategoryVM category, IFormFile file)
        {
            if (!ModelState.IsValid)
            {
                return View(category);
            }

            using (var formData = new MultipartFormDataContent())
            {
                
                var categoryJson = JsonConvert.SerializeObject(category);
                var categoryContent = new StringContent(categoryJson, Encoding.UTF8, "application/json");
                formData.Add(categoryContent, "categoryVM");

                
                if (file != null)
                {
                    var fileContent = new StreamContent(file.OpenReadStream());
                    fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse(file.ContentType);
                    formData.Add(fileContent, "file", file.FileName);
                }

               
                var response = await _httpClient.PutAsync($"Update/{id}", formData);

                if (response.IsSuccessStatusCode)
                {
                    TempData["success"] = "Category updated successfully.";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["error"] = "Failed to update category. Please try again.";
                    return View(category);
                }
            }
        }

   
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _httpClient.DeleteAsync($"Delete/{id}");

            if (response.IsSuccessStatusCode)
            {
                TempData["success"] = "Category deleted successfully.";
            }
            else
            {
                TempData["error"] = "Failed to delete category. Please try again.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}