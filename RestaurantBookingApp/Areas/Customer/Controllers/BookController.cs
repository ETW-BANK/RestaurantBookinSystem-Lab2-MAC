using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestaurantBookingApp.Models.ViewModel;
using System.Text;

namespace RestaurantBookingApp.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class BookController : Controller
    {
        private readonly HttpClient _httpClient;

        public BookController(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://localhost:7232/api/Booking/");
        }
        [HttpGet]
        public IActionResult Index() => View();

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.PageTitle = "Make A Reservation";
            ViewBag.ButtonLabel = "Create";
            return View(new BookingVM());
        }

        [HttpPost]
        public async Task<IActionResult> Create(BookingVM booking)
        {
            ViewBag.PageTitle = "Create Booking";
            ViewBag.ButtonLabel = "Create";

            var content = new StringContent(JsonConvert.SerializeObject(booking), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("CreateNewBooking", content);

            if (response.IsSuccessStatusCode)
            {
                TempData["success"] = "Booking created successfully";
                return RedirectToAction(nameof(Index));
            }

            // Optionally, capture the error message
            var errorData = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError(string.Empty, $"Error: {errorData}");

            return View(booking);
        }
    }
}
