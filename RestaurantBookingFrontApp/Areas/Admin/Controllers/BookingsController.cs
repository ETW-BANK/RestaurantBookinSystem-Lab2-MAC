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
    public class BookingsController : Controller
    {
        private readonly HttpClient _httpClient;

        public BookingsController(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://localhost:7232/api/Booking/");
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBookings()
        {
            var response = await _httpClient.GetAsync("GetBookings");

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var serviceResponse = JsonConvert.DeserializeObject<List<BookingVM>>(data);

                return Json(new { data = serviceResponse });
            }

            return Json(new { data = new List<BookingVM>(), error = "Unable to retrieve users from the server." });
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(); // Render the Create view
        }

        [HttpPost]
        public async Task<IActionResult> Create(BookingVM booking)
        {
            // Validate the model
            if (!ModelState.IsValid)
            {
                TempData["error"] = "Invalid booking details.";
                return View(booking);
            }

            // Send the booking data to the API
            var content = new StringContent(JsonConvert.SerializeObject(booking), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("Create", content);

            if (response.IsSuccessStatusCode)
            {
                TempData["success"] = "Booking created successfully.";
                return RedirectToAction(nameof(Index)); // Redirect to Index after success
            }

            TempData["error"] = "Failed to create booking. Please try again.";
            return View(booking); // Stay on the same page if there's an error
        }
    }
}

    

