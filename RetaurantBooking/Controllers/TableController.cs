
using Microsoft.AspNetCore.Mvc;
using Restaurant.Data.Access.Repository.IRepository;
using Restaurant.Data.Access.Repository.Services.IServices;
using Restaurant.Models;
using RestaurantViewModels;



namespace RetaurantBooking.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class TableController : ControllerBase
    {
        
        private readonly ITableService _tableservice;
        public TableController(ITableService tableService)
        {
          
            _tableservice = tableService;   
           
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetTable(int id)
        {
            var table = _tableservice.GetById(id);  
            if (table == null)
            {
                return NotFound("Table Not Found");
            }
            return Ok(table);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllTables()
        {
           var tables = _tableservice.GetAllTables();
            return Ok(tables);
        }

        [HttpPost]
        public async Task<IActionResult> CreateNewTable([FromBody] TablesVM table)
        {
            if (table == null)
            {
                return BadRequest("Invalid table data");
            }

           _tableservice.CreateTable(table);
          
            return Ok("Table Created Successfully");
        }


        [HttpPut]
        public async Task<IActionResult> Update([FromBody] TablesVM tableVM)
        {
            if (tableVM == null)
            {
                return BadRequest("Invalid table data");
            }

            if (tableVM== null)
            {
                return NotFound("Table not found");
            }
            _tableservice.UpdateTable(tableVM);
            return Ok("Table Updated Successfully");
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTable(int id)
        {
            var tableToDelete = _tableservice.GetById(id);  
            if (tableToDelete == null)
            {
                return NotFound("Table Not Found");
            }

           _tableservice.DeleteTable(tableToDelete);
          
            return Ok("Table Deleted Successfully");
        }
    }

}
