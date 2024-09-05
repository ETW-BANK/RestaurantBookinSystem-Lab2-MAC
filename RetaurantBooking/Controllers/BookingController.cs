using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Restaurant.Data.Access.Repository.Services.IServices;
using Restaurant.Models;
using Restaurant.Models.DTOs;
using Restaurant.Utility;

namespace RetaurantBooking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;
        public BookingController(IBookingService bookingService)
        {
           _bookingService = bookingService;    
        }


        [HttpGet]
        [Route("GetSingleBooking")]
        public async Task<IActionResult> GetBooking(int id)
        {
            var result = await _bookingService.GetSingleAsync(id);
            if (!result.Success)
            {
                return NotFound(result.Message);
            }
            return Ok(result);
        }

        [HttpGet]
        [Route("GetAllBookings")]
        public async Task<IActionResult> GetAllBookings()
        {
            var result = await _bookingService.GetAllAsync();

            if (!result.Success )
            {
                return BadRequest(result.Message);
            }
            return Ok(result.Data);
        }


        [HttpPost]
        [Route("CreateBooking")]

        public async Task<IActionResult> CreateNweBooking([FromQuery] BookingCreateDto booking)
        {
      
            var response = await _bookingService.AddItemAsync(booking);


            if (!response.Success)
            {
                return BadRequest(response.Message);
            }
            return Ok(response.Message);

          
            
            
            
            
        }

        [HttpPut]
        [Route("UpdateBooking")]
        public async Task<IActionResult> Update([FromQuery] BookingCreateDto booking)
        {

            var result = await _bookingService.UpdateBookingAsync(booking);

            if (!result.Success)
            {

                return NotFound(result.Message);
            }
           

            return Ok(result.Message);
        }

        [HttpDelete]
        [Route("DeleteBooking")]

        public async Task<IActionResult> DeleteBooking(int id)

        {

           var result= await _bookingService.RemoveAsync(id);

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            return Ok(result.Message);

        }
    }

}
