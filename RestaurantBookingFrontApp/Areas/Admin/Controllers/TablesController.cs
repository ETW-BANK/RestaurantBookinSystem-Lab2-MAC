using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestaurantViewModels;
using System.Text;

namespace RestaurantBookingApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TablesController : Controller
    {
        private readonly HttpClient _httpClient;

        public TablesController(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://localhost:7232/api/Table/");
        }

        [HttpGet]
        public IActionResult Index() => View();

        [HttpGet]
        public async Task<IActionResult> GetAllTables()
        {
            var response = await _httpClient.GetAsync("GetAllTable");

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var serviceResponse = JsonConvert.DeserializeObject<List<TablesVM>>(data);

                return Json(new { data = serviceResponse });
            }

            return Json(new { data = new List<TablesVM>(), error = "Unable to retrieve Tables from the server." });
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.PageTitle = "Create Table";
            ViewBag.ButtonLabel = "Create";

            return View(new TablesVM());
        }

        [HttpPost]
        public async Task<IActionResult> Create(TablesVM table)
        {
            var content = new StringContent(JsonConvert.SerializeObject(table), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("CreateNewTable", content);

            if (response.IsSuccessStatusCode)
            {
                TempData["success"] = "Table Created successfully";
                return RedirectToAction(nameof(Index));
            }

            return View(table);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var response = await _httpClient.GetAsync($"GetTable/{id}");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var tableResponse = JsonConvert.DeserializeObject<TablesVM>(data);

                ViewBag.PageTitle = "Edit Table";
                ViewBag.ButtonLabel = "Update";

                if (tableResponse != null)
                {
                    return View("Create", tableResponse);
                }
            }

            return View("Error");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(TablesVM table)
        {
            var content = new StringContent(JsonConvert.SerializeObject(table), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync("Update", content);

            if (response.IsSuccessStatusCode)
            {
                TempData["success"] = "Table Info Updated successfully";
                return RedirectToAction(nameof(Index));
            }

            return View("Create", table);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _httpClient.GetAsync($"GetTable/{id}");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var serviceResponse = JsonConvert.DeserializeObject<TablesVM>(data);
                if (serviceResponse != null)
                {
                    return View(serviceResponse);
                }
            }

            return View("Error");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var response = await _httpClient.DeleteAsync($"DeleteTable/{id}");
            if (response.IsSuccessStatusCode)
            {
                TempData["success"] = "Table Deleted successfully";
                return RedirectToAction(nameof(Index));
            }

            return View("Error");
        }
    }
}
