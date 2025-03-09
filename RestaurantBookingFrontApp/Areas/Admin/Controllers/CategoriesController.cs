using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Restaurant.Models;
using Restaurant.Utility;
using RestaurantViewModels;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

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
                    var categories = JsonConvert.DeserializeObject<List<Category>>(data);

                    // Ensure full URL for each category's ImageUrl
                    foreach (var category in categories)
                    {
                        if (category?.Name != null && !string.IsNullOrEmpty(category.ImageUrl))
                        {
                            category.ImageUrl = $"https://localhost:44307{category.ImageUrl}";
                        }
                    }

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
        public async Task<IActionResult> Create(CategoryVM categoryVM, IFormFile? file)
        {
           
            if (!ModelState.IsValid)
            {
                return View(categoryVM);
            }

            try
            {
           
                using (var formData = new MultipartFormDataContent())
                {
                  
                    if (categoryVM.Category.Id != 0)
                    {
                        formData.Add(new StringContent(categoryVM.Category.Id.ToString()), "Category.Id");
                    }

               
                    formData.Add(new StringContent(categoryVM.Category.Name), "Category.Name");

                   
                    if (categoryVM.Category.DisplayOrder != 0)
                    {
                        formData.Add(new StringContent(categoryVM.Category.DisplayOrder.ToString()), "Category.DisplayOrder");
                    }

                    if (!string.IsNullOrEmpty(categoryVM.Category.ImageUrl))
                    {
                        formData.Add(new StringContent(categoryVM.Category.ImageUrl), "Category.ImageUrl");
                    }

                    if (file != null)
                    {
                        var fileContent = new StreamContent(file.OpenReadStream());
                        fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse(file.ContentType);
                        formData.Add(fileContent, "file", file.FileName); 
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

                // Add the file to the form data (if provided)
                if (file != null)
                {
                    var fileContent = new StreamContent(file.OpenReadStream());
                    fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse(file.ContentType);
                    formData.Add(fileContent, "file", file.FileName);
                }

                // Send the request to the backend API
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

        /// <summary>
        /// Handles the deletion of a category.
        /// </summary>
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