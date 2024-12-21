using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Restaurant.Data.Access.Repository.IRepository;
using Restaurant.Data.Access.Repository.Services.IServices;
using Restaurant.Utility;
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

        //[Area("Customer")]
        //[Authorize(Roles = StaticData.Role_Customer)]
        [HttpPost]


        public IActionResult Create([FromBody] BookingVM bookingVm)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid booking details.");
            }

            try
            {
              var userId= "d1abb8f5-68d3-4632-895c-dc09b1489ccb";

                // Proceed with booking creation
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
