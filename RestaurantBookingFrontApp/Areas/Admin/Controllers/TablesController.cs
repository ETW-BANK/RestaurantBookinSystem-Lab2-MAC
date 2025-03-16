
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurant.Data.Access.Repository.Services.IServices;
using Restaurant.Utility;
using RestaurantViewModels;


namespace RestaurantBookingApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = StaticData.Role_Admin)]
    public class TablesController : Controller
    {
        private readonly ITableService _tableservice;

        public TablesController(ITableService tableService)
        {
            _tableservice = tableService;
        }
        public IActionResult Index()
        {


            var tablesList = _tableservice.GetAllTables();  


            if (tablesList == null)
            {

                TempData["error"] = "An error occurred while retrieving categories.";
            }


            return View(tablesList);
        }


        public IActionResult Upsert(int id)
        {
           TablesVM tableVm = new();

            if (id == 0)
            {

                return View(tableVm);
            }
            else
            {
                
                var tables = _tableservice.GetById(id);
                if (tables== null)
                {
                    return NotFound();
                }

               tableVm.Id = tables.Id;
              tableVm.TableNumber = tables.TableNumber;
                tableVm.NumberOfSeats= tables.NumberOfSeats;
                tableVm.IsAvailable = tables.IsAvailable; 
               

                return View(tableVm);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Upsert(TablesVM tableVM)
        {

            try
            {
                if (tableVM.Id == 0)
                {
                    // Create mode
                     _tableservice.CreateTable(tableVM);
                    TempData["success"] = "Table created successfully.";
                }
                else
                {

                   _tableservice.UpdateTable(tableVM);

                    TempData["success"] = "Table updated successfully.";
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["error"] = $"An error occurred: {ex.Message}";
                return View(tableVM);
            }
        }


        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
        if ( id == 0) return NotFound();

        var table = _tableservice.GetById(id);
            if (table == null) return NotFound();

        return View(table);
          }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int id)
        {
            var table= _tableservice.GetById(id);

            if (table == null)

            {

                TempData["error"] = "Table not found.";
                return RedirectToAction(nameof(Index));
            }

            _tableservice.DeleteTable(table);
            TempData["success"] = "Table deleted successfully!";
            return RedirectToAction(nameof(Index));
        }
    }
}