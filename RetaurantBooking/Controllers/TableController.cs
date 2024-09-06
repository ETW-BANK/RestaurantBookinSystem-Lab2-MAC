using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Restaurant.Data.Access.Repository.Services;
using Restaurant.Data.Access.Repository.Services.IServices;
using Restaurant.Models.DTOs;

namespace RetaurantBooking.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class TableController : ControllerBase
    {
        private readonly ITableService _tableService;
        public TableController(ITableService tableService)
        {
            _tableService = tableService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTable(int id)
        {
            var result = await _tableService.GetSingleAsync(id);
            return result.Success ? Ok(result) : NotFound(result.Message);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTables()
        {
            var result = await _tableService.GetAllAsync();
            return result.Success ? Ok(result) : BadRequest(result.Message);
        }

        [HttpPost]
        public async Task<IActionResult> CreateNewTable([FromBody] TablesDto table)
        {
            var response = await _tableService.AddItemAsync(table);
            return response.Success ? CreatedAtAction(nameof(GetTable), new { id = response.Message }, response.Data) : BadRequest(response.Message);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] TablesDto table)
        {
            var result = await _tableService.UpdateTableAsync(table);
            return result.Success ? Ok(result.Message) : NotFound(result.Message);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTable(int id)
        {
            var result = await _tableService.RemoveAsync(id);
            return result.Success ? Ok(result.Message) : BadRequest(result.Message);
        }
    }

}
