using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Restaurant.Utility;
using RestaurantBookingFrontApp.Models;
using RestaurantViewModels;
using System.Diagnostics;
using System.Text;

namespace RestaurantBookingFrontApp.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {

        private readonly HttpClient _httpClient;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, HttpClient httpClient)
        {
            _logger = logger;
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://localhost:7232/api/Booking/");
        }

        public IActionResult Index()
        {
            return View();
        }



        //[Authorize(Roles = StaticData.Role_Customer)]
        [HttpPost]
        public async Task<IActionResult> CreateBookings([FromForm] BookingVM bookingVm)
        {
            try
            {
                var jsonData = JsonConvert.SerializeObject(bookingVm);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("Create", content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("BookingConfirmation", "Home");
                }

                TempData["Error"] = "Unable to create booking.";
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An error occurred: " + ex.Message;
                return RedirectToAction("Index", "Home");
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
