using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.MSIdentity.Shared;
using Newtonsoft.Json;
using Restaurant.Models;
using Restaurant.Models.DTOs;
using Restaurant.Utility;
using RestaurantBookingApp.Models.ViewModel;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace RestaurantBookingApp.Controllers
{
    public class CustomersController : Controller
    {
        private readonly HttpClient _httpClient;

        public CustomersController(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://localhost:7232/api/Customer/");
        }

        [HttpGet]
        public IActionResult Index() => View();

        [HttpGet]
        public async Task<IActionResult> GetAllCustomers()
        {
            var response = await _httpClient.GetAsync("GetAllCustomers");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var serviceResponse = JsonConvert.DeserializeObject<ServiceResponse<List<CustomerVM>>>(data);
                return Json(serviceResponse.Success ? new { data = serviceResponse.Data } : new { data = new List<CustomerVM>(), error = serviceResponse.Message });
            }

            return Json(new { data = new List<CustomerVM>(), error =  "Unable to retrieve customers from the server." });
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.PageTitle = "Create Customer";
            ViewBag.ButtonLabel = "Create";
          
            return View(new CustomerVM()); 
        }


        [HttpPost]
        public async Task<IActionResult> Create(CustomerVM customer)
        {
            var content = new StringContent(JsonConvert.SerializeObject(customer), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("CreateNewCustomer", content);
            TempData["success"] = "Customer Created successfully";
            return response.IsSuccessStatusCode ? RedirectToAction(nameof(Index)) : View(customer);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var response = await _httpClient.GetAsync($"GetCustomer/{id}");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var customerResponse = JsonConvert.DeserializeObject<ServiceResponse<CustomerVM>>(data);

                ViewBag.PageTitle = "Edit Customer";
                ViewBag.ButtonLabel = "Update";

                if (customerResponse?.Data != null)
                {
                    return View("Create", customerResponse.Data); 
                }
            }

            return View("Error");
        }



        [HttpPost]
        public async Task<IActionResult> Edit(CustomerVM customer)
        {
            var content = new StringContent(JsonConvert.SerializeObject(customer), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync("Update", content);

            if(response.IsSuccessStatusCode)
            {

                TempData["success"] = "Customer Info Updated successfully";
            }
            return response.IsSuccessStatusCode ? RedirectToAction("Index") : View(customer);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var response = await _httpClient.GetAsync($"GetCustomer/{id}");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var serviceResponse = JsonConvert.DeserializeObject<ServiceResponse<CustomerVM>>(data);
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
            var response = await _httpClient.DeleteAsync($"DeleteCustomer/{id}");
            if (response.IsSuccessStatusCode)
            {
                TempData["success"] = "Customer Deleted successfully";
                return RedirectToAction("Index"); 
            }

            return View("Error"); 
        }

    }

}











