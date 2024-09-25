using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Restaurant.Utility;
using RestaurantBookingApp.Models.ViewModel;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantBookingApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BookingsController : Controller
    {
        private readonly HttpClient _httpClient;

        public BookingsController(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://localhost:7232/api/Booking/");
        }

        [HttpGet]
        public IActionResult Index() => View();

        [HttpGet]
        public async Task<IActionResult> GetAllBooking()
        {
            var response = await _httpClient.GetAsync("GetAllBookings");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var serviceResponse = JsonConvert.DeserializeObject<ServiceResponse<List<BookingVM>>>(data);
                return Json(serviceResponse.Success ? new { data = serviceResponse.Data } : new { data = new List<BookingVM>(), error = serviceResponse.Message });
            }

            return Json(new { data = new List<BookingVM>(), error = "Unable to retrieve bookings from the server." });
        }

    
       
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.PageTitle = "Create Booking";
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

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var response = await _httpClient.GetAsync($"GetBooking/{id}");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var bookingResponse = JsonConvert.DeserializeObject<ServiceResponse<BookingVM>>(data);

                ViewBag.PageTitle = "Edit Booking";
                ViewBag.ButtonLabel = "Update";

                if (bookingResponse?.Data != null)
                {
                    return View("Create", bookingResponse.Data);
                }
            }

            TempData["error"] = "Error fetching booking details.";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(BookingVM booking)
        {
            ViewBag.PageTitle = "Edit Booking";
            ViewBag.ButtonLabel = "Update";

            var content = new StringContent(JsonConvert.SerializeObject(booking), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync("Update", content);

            if (response.IsSuccessStatusCode)
            {
                TempData["success"] = "Booking information updated successfully";
                return RedirectToAction("Index");
            }

           
            var errorData = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError(string.Empty, $"Error: {errorData}");

            return View("Create", booking);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _httpClient.GetAsync($"GetBooking/{id}");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var serviceResponse = JsonConvert.DeserializeObject<ServiceResponse<BookingVM>>(data);
                if (serviceResponse.Success)
                {
                    return View(serviceResponse.Data);
                }
            }

            TempData["error"] = "Error fetching booking details for deletion.";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var response = await _httpClient.DeleteAsync($"DeleteBooking/{id}");
            if (response.IsSuccessStatusCode)
            {
                TempData["success"] = "Booking deleted successfully";
                return RedirectToAction("Index");
            }

            TempData["error"] = "Error deleting booking.";
            return RedirectToAction("Index");
        }
    }
}
