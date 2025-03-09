
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Restaurant.Models;
using Restaurant.Utility;
using RestaurantBookingFrontApp.Models;
using RestaurantViewModels;
using System.Diagnostics;
using System.Net.Http;


namespace RestaurantBookingFrontApp.Areas.Customer.Controllers
{

    [Area("Customer")]
    public class HomeController : Controller
    {

        private readonly ILogger<HomeController> _logger;
        private readonly HttpClient _httpClient = new HttpClient();
        public HomeController(ILogger<HomeController> logger, HttpClient httpClient)
        {
            _logger = logger;
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://localhost:44307/api/Category/");
            _httpClient = httpClient;
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

                    // Ensure full URL for each category's ImageUrl
                    foreach (var category in categories)
                    {
                        if (category?.Category != null && !string.IsNullOrEmpty(category.Category.ImageUrl))
                        {
                            category.Category.ImageUrl = $"https://localhost:44307{category.Category.ImageUrl}";
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


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
