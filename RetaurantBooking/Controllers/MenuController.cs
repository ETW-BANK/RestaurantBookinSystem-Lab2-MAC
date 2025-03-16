using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Restaurant.Models;
using RestaurantServices.Services.IServices;
using RestaurantViewModels;

namespace RetaurantBooking.Controllers
{

    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class MenuController : ControllerBase
    {
        private readonly IMenuService _menuService;

        public MenuController(IMenuService menuService)
        {
            _menuService = menuService;
        }



        [HttpPost]

        public async Task<IActionResult> CreateMenu([FromForm] MenuVM menu, IFormFile? file)
        {
            try
            {
                await _menuService.CreateMenue(menu, file);
                return Ok(menu);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "erroroccerd" + ex });
            }
        }


        [HttpGet]

        public async Task<IActionResult> GetMenus()
        {
            try
            {
                var menus = _menuService.GetAll();
                return Ok(menus);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]

        public async Task<IActionResult> GetMenu(int id)
        {
            var menu = _menuService.GetbyId(id);
            if (menu == null)
            {
                return NotFound("Menu Not Found");
            }
            return Ok(menu);
        }

        [HttpPut]

        public async Task<IActionResult> UpdateMenu([FromQuery] MenuVM menuVM, IFormFile? file)
        {
            if (menuVM == null)
            {
                return BadRequest("Invalid menu details.");
            }

            _menuService.Update(menuVM, file);

            return Ok(new { message = "Menu updated successfully." });
        }

        //[HttpDelete]
        //public async Task<IActionResult> DeleteMenu([FromQuery] int? id)
        //{
        //    if (id == null || id == 0)
        //    {
        //        return BadRequest("Invalid menu ID.");
        //    }

        //    try
        //    {
        //        // Log or inspect the ID value here
        //        Console.WriteLine($"Deleting menu with ID: {id}");

        //        var menuToDelete = await _menuService.GetbyId(id.Value);

        //        if (menuToDelete == null)
        //        {
        //            return NotFound("Menu not found.");
        //        }

        //        // Call the Delete method in the service
        //         _menuService.DeleteMenu(menuToDelete);

        //        return Ok(new { message = "Menu deleted successfully." });
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, new { message = $"An error occurred: {ex.Message}" });
        //    }
        //}

    }
}
