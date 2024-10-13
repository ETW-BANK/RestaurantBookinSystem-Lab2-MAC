using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Restaurant.Data.Access.Repository.IRepository;
using Restaurant.Data.Access.Repository.Services;
using Restaurant.Data.Access.Repository.Services.IServices;
using RestaurantViewModels;


namespace RetaurantBooking.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class TableController : ControllerBase
    {
        private readonly ITableService _tableService;
        private readonly IUnitOfWork _unitOfWork;   
        public TableController(ITableService tableService,IUnitOfWork unitOfWork)
        {
            _tableService = tableService;
            _unitOfWork = unitOfWork;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTable(int id)
        {
            var result =  _tableService.GetById(id);

           return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTables()
        {
            IEnumerable<TablesVM> tables = _tableService.GetAllTables();

            return Ok(tables);
        }

        [HttpPost]
        public async Task<IActionResult> CreateNewTable(TablesVM table)
        {
            _tableService.CreateTable(table);
          
            return Ok("Table Created Succesfully");
        }

        [HttpGet]

        public IActionResult Update(int id)
        {
            var table = _tableService.GetById(id);
            if (table == null)
            {
                return NotFound("Table Not Found");
            }

            return Ok(table);

        }

        [HttpPut]
        public async Task<IActionResult> Update(TablesVM table)
        {
            _tableService.UpdateTable(table);
     

            return Ok("Table Updated");    
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTable(int id)
        {
         _tableService.DeleteTable(id);
            if (id == 0)
            {
                return NotFound("Table Not Found");

            }
            return Ok("Table Deleted Successfully");
        }
    }

}
