using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurant.Data.Access.Repository.IRepository;
using Restaurant.Models;
using RestaurantServices.Services.IServices;
using RestaurantViewModels;

namespace RetaurantBooking.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBookingService _bookingService;


        public BookingController(IUnitOfWork unitOfWork, IBookingService bookingService)
        {
            _unitOfWork = unitOfWork;
            _bookingService = bookingService;

        }

        [HttpPost]
        //[Authorize]
        public async Task<IActionResult> Create([FromBody] BookingVM bookingVM)
        {
            if (bookingVM == null)
            {
                return BadRequest("Invalid booking details.");
            }

            _bookingService.CreateBooking(bookingVM);

            return Ok(new { message = "Booking created successfully." });
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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSingleBooking(int id)
        {
            var booking= _unitOfWork.BookingRepository.GetFirstOrDefault(x => x.Id == id);
            if (booking == null)
            {
                return NotFound("Booking Not Found");
            }
            return Ok(booking);
        }

        [HttpDelete("{bookingId}")]
        public async Task<IActionResult> DeleteBooking(int bookingId)
        {
            var bookingToDelete = _unitOfWork.BookingRepository.GetFirstOrDefault(x => x.Id == bookingId);

            if (bookingToDelete == null)
            {
                return NotFound("Booking not found.");
            }


            _bookingService.DeleteBooking(bookingToDelete);

            return Ok("Booking deleted successfully.");
        }

    }
}
