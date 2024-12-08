using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Restaurant.Data.Access.Repository.IRepository;
using Restaurant.Data.Access.Repository.Services.IServices;
using RestaurantServices.Services.IServices;
using RestaurantViewModels;
using System.Security.Claims;

namespace RetaurantBooking.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBookingService _bookingService;

        public BookingController(IUnitOfWork unitOfWork,IBookingService bookingService)
        {
            _unitOfWork = unitOfWork;
            _bookingService = bookingService;
        }

        [HttpPost]
        
        public IActionResult Create([FromBody] BookingVM bookingVm)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid booking details.");
            }

            try
            {
                var userId = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                userId = "d50472d8-50e2-4382-a2fe-d1287fddac6c";
                if (userId == null)
                {
                    return Unauthorized("User is not authenticated.");
                }

                _bookingService.CreateBooking(bookingVm, userId);
                return Ok("Booking created successfully.");
            }
            catch (Exception ex)
            {
               
                return StatusCode(500, "Internal server error: " + ex.Message);
            }

        }

        [HttpGet]
        public async Task<IActionResult> GetBookings()
        {
            try
            {
                var bookings = await _bookingService.GetBookingsAsync();
                return Ok(bookings);    
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
