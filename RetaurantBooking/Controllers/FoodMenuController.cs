using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Restaurant.Data.Access.Repository.Services.IServices;
using Restaurant.Models.DTOs;
using Restaurant.Utility;

namespace RetaurantBooking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoodMenuController : ControllerBase
    {
        private readonly IFoodMenuService _foodService;
        public FoodMenuController(IFoodMenuService foodService)
        {
         _foodService = foodService;
        }


        [HttpGet]
        [Route("GetSingleMenu")]
        public async Task<IActionResult> GetMenu(int id)
        {
            var result = await _foodService.GetSingleAsync(id);
            if (!result.Success)
            {
                return NotFound(result.Message);
            }
            return Ok(result);
        }

        [HttpGet]
        [Route("GetAllMenues")]
        public async Task<IActionResult> GetAllMenu()
        {
            var result = await _foodService.GetAllAsync();

            if (!result.Success )
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
        }


        [HttpPost]
        [Route("CreateMenue")]

        public async Task<IActionResult> CreateNewMenu([FromQuery] FoodMenuDto foodmenu)
        {
      
            var response = await _foodService.AddItemAsync(foodmenu);


            if (!response.Success)
            {
                return BadRequest(response.Message);
            }
            return Ok(response.Message);

          
            
            
            
            
        }

        [HttpPut]
        [Route("UpdateMenu")]
        public async Task<IActionResult> Update([FromQuery] FoodMenuDto foodmenu)
        {

            var result = await _foodService.UpdateMenueAsync(foodmenu);

            if (!result.Success)
            {

                return NotFound(result.Message);
            }
           

            return Ok(result.Message);
        }

        [HttpDelete]
        [Route("DeleteMenu")]

        public async Task<IActionResult> DeleteMenu(int id)

        {

           var result= await _foodService.RemoveAsync(id);

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            return Ok(result.Message);

        }
    }

}
