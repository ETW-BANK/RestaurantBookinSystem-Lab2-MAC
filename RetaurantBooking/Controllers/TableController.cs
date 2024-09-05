using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Restaurant.Data.Access.Repository.Services.IServices;
using Restaurant.Models.DTOs;

namespace RetaurantBooking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TableController : ControllerBase
    {
        private readonly ITableService _tableService;
        public TableController(ITableService tableService)
        {
            _tableService = tableService;
        }


        [HttpGet]
        [Route("GetSingleTable")]
        public async Task<IActionResult> GetTable(int id)
        {
            var result = await _tableService.GetSingleAsync(id);

            if (!result.Success)
            {
                return NotFound(result.Message);
            }
            return Ok(result);
        }

        [HttpGet]
        [Route("GetAllTables")]
        public async Task<IActionResult> GetAllTables()
        {
            var result = await _tableService.GetAllAsync();

            if (!result.Success)
            {
                return NotFound(result.Message);
            }
            return Ok(result);
        }


        [HttpPost]
        [Route("CreateTable")]

        public async Task<IActionResult> CreateTable([FromQuery] TablesDto table)
        {
            var result=await _tableService.AddItemAsync(table);

            if(!result.Success)
            {
                return BadRequest(result.Message);
            }
            
            return Ok(result.Message);
        }

        [HttpPut]
        [Route("UpdateTable")]

        public async Task<IActionResult> Update([FromQuery] TablesDto table)

        {

            var result = await _tableService.UpdateTableAsync(table);

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            return Ok(result.Message);

        }

        [HttpDelete]
        [Route("DeleteTable")]

        public async Task<IActionResult> DeleteTable(int id)

        {

           var result= await _tableService.RemoveAsync(id); 

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }


            return Ok(result.Message);

        }
    }

}
