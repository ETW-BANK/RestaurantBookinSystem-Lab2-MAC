using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        public async Task<IActionResult> Create([FromQuery] MenuVM menuVM)
        {
            if (menuVM == null)
            {
                return BadRequest("Invalid menu details.");
            }

            _menuService.CreateMenu(menuVM);

            return Ok(new { message = "Menu created successfully." });
        }

        [HttpGet]

        public async Task<IActionResult> GetMenus()
        {
            try
            {
                var menus =  _menuService.GetAllMenues();
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
            var menu = _menuService.GetById(id);
            if (menu == null)
            {
                return NotFound("Menu Not Found");
            }
            return Ok(menu);
        }

        [HttpPut]

        public async Task<IActionResult> UpdateMenu([FromQuery] MenuVM menuVM)
        {
            if (menuVM == null)
            {
                return BadRequest("Invalid menu details.");
            }

            _menuService.UpdateMenu(menuVM);

            return Ok(new { message = "Menu updated successfully." });
        }

        [HttpDelete]

        public async Task<IActionResult> DeleteMenu(int id)
        {
            var menuToDelete = _menuService.GetById(id);

            if (menuToDelete == null)
            {
                return NotFound("Menu not found.");
            }

            return Ok(new { message = "Menu Deleted successfully." });
        }
    }
}
