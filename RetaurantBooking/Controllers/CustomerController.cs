using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Restaurant.Data.Access.Repository.Services.IServices;
using Restaurant.Models.DTOs;
using Restaurant.Utility;

namespace RetaurantBooking.Controllers
{
    [ApiController]
    [Route("api/[controller]/[Action]")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCustomer(int id)
        {
            var result = await _customerService.GetSingleAsync(id);
            return result.Success ? Ok(result) : NotFound(result.Message);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCustomers()
        {
            var result = await _customerService.GetAllAsync();
            return result.Success ? Ok(result) : BadRequest(result.Message);
        }

        [HttpPost]
        public async Task<IActionResult> CreateNewCustomer([FromBody] CustomerDto customer)
        {
            var response = await _customerService.AddItemAsync(customer);
            return response.Success ? CreatedAtAction(nameof(GetCustomer), new { id = response.Message }, response.Data) : BadRequest(response.Message);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] CustomerDto customer)
        {
            var result = await _customerService.UpdateCustomerAsync(customer);
            return result.Success ? Ok(result.Message) : NotFound(result.Message);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            var result = await _customerService.RemoveAsync(id);
            return result.Success ? Ok(result.Message) : BadRequest(result.Message);
        }
    }

}
