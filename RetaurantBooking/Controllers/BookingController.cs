using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Restaurant.Data.Access.Repository.Services;
using Restaurant.Data.Access.Repository.Services.IServices;
using Restaurant.Models;
using Restaurant.Models.DTOs;
using Restaurant.Utility;

namespace RetaurantBooking.Controllers
{
   
    [ApiController]
    [Route("api/[controller]/[Action]")]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;
        public BookingController(IBookingService bookingService)
        {
           _bookingService = bookingService;    
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetBooking(int id)
        {
            var result = await _bookingService.GetSingleAsync(id);
            return result.Success ? Ok(result) : NotFound(result.Message);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBookings()
        {
            var result = await _bookingService.GetAllAsync();

            return result.Success ? Ok(result) : BadRequest(result.Message);
        }

      
        [HttpPost]
        public async Task<IActionResult> CreateNewBooking([FromBody] BookingDto booking)
        {
            var response = await _bookingService.AddItemAsync(booking);
            return response.Success ? CreatedAtAction(nameof(GetBooking), new { id = response.Message }, response.Data) : BadRequest(response.Message);
        }

        [HttpPut]
        public async Task<IActionResult> Update( BookingDto booking)
        {
            var result = await _bookingService.UpdateBookingAsync(booking);
            return result.Success ? Ok(result.Data) : NotFound(result.Message);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBooking(int id)
        {
            var result = await _bookingService.RemoveAsync(id);
            return result.Success ? Ok(result.Message) : BadRequest(result.Message);
        }
    }

}
