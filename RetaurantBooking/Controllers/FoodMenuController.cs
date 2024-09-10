using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Restaurant.Data.Access.Repository.Services;
using Restaurant.Data.Access.Repository.Services.IServices;
using Restaurant.Models.DTOs;
using Restaurant.Utility;

namespace RetaurantBooking.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class FoodMenuController : ControllerBase
    {
        private readonly IFoodMenuService _foodService;
        public FoodMenuController(IFoodMenuService foodService)
        {
         _foodService = foodService;
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetMenu(int id)
        {
            var result = await _foodService.GetSingleAsync(id);
            return result.Success ? Ok(result) : NotFound(result.Message);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllMenues()
        {
            var result = await _foodService.GetAllAsync();
            return result.Success ? Ok(result) : BadRequest(result.Message);
        }

        [HttpPost]
        public async Task<IActionResult> CreateNewMenu([FromBody] FoodMenuDto menu)
        {
            var response = await _foodService.AddItemAsync(menu);
            return response.Success ? CreatedAtAction(nameof(GetMenu), new { id = response.Message }, response.Data) : BadRequest(response.Message);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] FoodMenuDto meue)
        {
            var result = await _foodService.UpdateMenueAsync(meue);
            return result.Success ? Ok(result.Message) : NotFound(result.Message);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMenu(int id)
        {
            var result = await _foodService.RemoveAsync(id);
            return result.Success ? Ok(result.Message) : BadRequest(result.Message);
        }
    }

}
