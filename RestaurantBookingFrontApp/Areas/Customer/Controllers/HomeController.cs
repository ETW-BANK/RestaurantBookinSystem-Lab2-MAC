
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Restaurant.Services;
using RestaurantBookingFrontApp.Models;
using RestaurantViewModels;
using System.Diagnostics;



namespace RestaurantBookingFrontApp.Areas.Customer.Controllers
{

    [Area("Customer")]
    public class HomeController : Controller
    {

        private readonly ILogger<HomeController> _logger;
    
        private readonly ICategoryService _categoryService;
        public HomeController(ILogger<HomeController> logger, ICategoryService categoryService)
        {
            _logger = logger;
      
       
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {


            var categorylist = await _categoryService.GetAll();


            if (categorylist == null)
            {

                TempData["error"] = "An error occurred while retrieving categories.";
            }


            return View(categorylist);

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
