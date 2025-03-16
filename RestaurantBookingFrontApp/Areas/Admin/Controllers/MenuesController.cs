using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Restaurant.Models;
using Restaurant.Services;
using Restaurant.Utility;
using RestaurantServices.Services.IServices;
using RestaurantViewModels;

namespace RestaurantBookingFrontApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = StaticData.Role_Admin)]
    public class MenuesController : Controller
    {

        private readonly IMenuService _menuService;
      


        public MenuesController(IMenuService menuService)
        {
            _menuService = menuService;
        


        }
        public async Task<IActionResult> Index()
        {
            var menuelist=await _menuService.GetAll();

            if (menuelist == null )
            {
                TempData["error"] = "No menus found.";
                return View(menuelist);
            }

            return View(menuelist);
        }

        [HttpGet]
        public async Task<IActionResult> Upsert(int? id)
        {
            var menuVM = await _menuService.GetbyId(id);
         

         

            if (menuVM == null)
            {
                TempData["error"] = "Menu not found!";
                return NotFound();
            }

            return View(menuVM);
        }


        [HttpPost]
        public async Task<IActionResult> Upsert(MenuVM menuVM, IFormFile? file)
        {

            try
            {
                if (menuVM.Menue.menueId == 0)
                {
                    // Create mode
                    await _menuService.CreateMenue(menuVM,file);
                    TempData["success"] = "Menu created successfully.";
                }
                else
                {

                  await _menuService.Update(menuVM, file);

                    TempData["success"] = "Menu updated successfully.";
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["error"] = $"An error occurred: {ex.Message}";
                return View(menuVM);
            }
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0) return NotFound();

            var menu = _menuService.GetbyId(id);
            if (menu == null) return NotFound();

            return View(menu);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(Menue menue)
        {
            var menu = _menuService.GetbyId(menue.menueId);
            if (menu.Id == null)
            {
                TempData["error"] = "Invalid menu ID.";
                return RedirectToAction(nameof(Index));
            }


            try
            {
                _menuService.DeleteMenue(menu.Id);
                TempData["success"] = "Menu deleted successfully!";
            }
            catch (Exception ex)
            {
                TempData["error"] = $"An error occurred: {ex.Message}";
            }

            return RedirectToAction(nameof(Index));
        }

    }
}
