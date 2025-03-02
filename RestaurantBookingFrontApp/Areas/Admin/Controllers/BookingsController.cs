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
            _httpClient.BaseAddress = new Uri("https://localhost:4430/api/Booking/");  
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

            return Json(new { data = new List<BookingVM>(), error = "Unable to retrieve bookings from the server." });
        }

      
        [HttpGet]
        public IActionResult Create()
        {
            return View(new BookingVM());
        }

       
        [HttpPost]
        public async Task<IActionResult> Create(BookingVM booking)
        {
            var content = new StringContent(JsonConvert.SerializeObject(booking), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("Create", content);

            if (response.IsSuccessStatusCode)
            {
                TempData["success"] = "Booking created successfully.";
                return RedirectToAction(nameof(Index));
            }

            TempData["error"] = "No table available at this time.";
            return View(booking);
        }


        [HttpPost]
        public async Task<IActionResult> CancelBooking(int bookingId)
        {

            var content = new StringContent("{}", Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"UpdateBooking/{bookingId}", content);

            if (response.IsSuccessStatusCode)
            {
                TempData["success"] = "Booking cancelled successfully.";
                return RedirectToAction("Index");
            }
            else
            {

                var errorDetails = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Error: {response.StatusCode} - {errorDetails}");


                TempData["error"] = $"Failed to cancel booking. Server responded with: {response.StatusCode}";
                return RedirectToAction(nameof(Index));
            }
        }




        [HttpGet]
        public async Task<IActionResult> ConfirmCancelBooking(int bookingId)
        {
            var response = await _httpClient.GetAsync($"GetSingleBooking/{bookingId}");

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var booking = JsonConvert.DeserializeObject<BookingVM>(data);

                return View("CancelBookingConfirmation", booking);
            }

            TempData["error"] = "Booking not found.";
            return RedirectToAction(nameof(Index));
        }

    }
}
