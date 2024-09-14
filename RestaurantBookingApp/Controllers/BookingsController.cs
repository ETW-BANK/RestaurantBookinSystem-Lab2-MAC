using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Restaurant.Utility;
using RestaurantBookingApp.Models.ViewModel;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantBookingApp.Controllers
{
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
        public async Task<IActionResult> GetAllBookings()
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
        public IActionResult Create(int? id)
        {
            if (id.HasValue)
            {
                ViewBag.PageTitle = "Edit Booking";
                ViewBag.ButtonLabel = "Update";

                // Fetch the booking details for editing
                return RedirectToAction("Edit", new { id = id.Value });
            }
            else
            {
                ViewBag.PageTitle = "Create Booking";
                ViewBag.ButtonLabel = "Create";

                return View(new BookingVM());
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var response = await _httpClient.GetAsync($"GetBooking/{id}");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var bookingResponse = JsonConvert.DeserializeObject<ServiceResponse<BookingVM>>(data);

                if (bookingResponse?.Data != null)
                {
                    // Pass the fetched booking data to the view
                    return View("Create", bookingResponse.Data);
                }
            }

            return View("Error");
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrEdit(BookingVM booking)
        {
            if (booking.Id == 0)
            {
                // Handle Create
                var content = new StringContent(JsonConvert.SerializeObject(booking), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("CreateNewBooking", content);
                if (response.IsSuccessStatusCode)
                {
                    TempData["success"] = "Booking Created successfully";
                    return RedirectToAction(nameof(Index));
                }
            }
            else
            {
                // Handle Edit
                var content = new StringContent(JsonConvert.SerializeObject(booking), Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync("Update", content);
                if (response.IsSuccessStatusCode)
                {
                    TempData["success"] = "Booking Info Updated successfully";
                    return RedirectToAction(nameof(Index));
                }
            }

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

            return View("Error");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var response = await _httpClient.DeleteAsync($"DeleteBooking/{id}");
            if (response.IsSuccessStatusCode)
            {
                TempData["success"] = "Booking Deleted successfully";
                return RedirectToAction("Index");
            }

            return View("Error");
        }
    }
}
