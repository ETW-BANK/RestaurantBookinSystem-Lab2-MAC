using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Restaurant.Models.DTOs;
using Restaurant.Utility;
using RestaurantBookingApp.Models.ViewModel;
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
            var response = await _httpClient.GetAsync("GetAllTables");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var serviceResponse = JsonConvert.DeserializeObject<ServiceResponse<List<TablesVM>>>(data);
                return Json(serviceResponse.Success ? new { data = serviceResponse.Data } : new { data = new List<TablesVM>(), error = serviceResponse.Message });
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
            TempData["success"] = "Table Created successfully";
            return response.IsSuccessStatusCode ? RedirectToAction(nameof(Index)) : View(table);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var response = await _httpClient.GetAsync($"GetTable/{id}");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var tableResponse = JsonConvert.DeserializeObject<ServiceResponse<TablesVM>>(data);

                ViewBag.PageTitle = "Edit Table";
                ViewBag.ButtonLabel = "Update";

                if (tableResponse?.Data != null)
                {
                    return View("Create", tableResponse.Data);
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
            }
            return response.IsSuccessStatusCode ? RedirectToAction("Index") : View(table);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var response = await _httpClient.GetAsync($"GetTable/{id}");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var serviceResponse = JsonConvert.DeserializeObject<ServiceResponse<TablesVM>>(data);
                if (serviceResponse.Success)
                {

                    return View(serviceResponse.Data);
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
                return RedirectToAction("Index");
            }

            return View("Error");
        }

    }
}
