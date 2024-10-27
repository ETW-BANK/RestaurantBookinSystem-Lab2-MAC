using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Restaurant.Data.Access.Repository.IRepository;
using Restaurant.Data.Access.Repository.Services;
using Restaurant.Data.Access.Repository.Services.IServices;
using Restaurant.Models;
using RestaurantViewModels;


namespace RetaurantBooking.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class TableController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;   
       
        public TableController(IUnitOfWork unitOfWork)
        {
           _unitOfWork = unitOfWork;
           
        }

      
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTable(int id)
        {
            var table = _unitOfWork.TableRepository.GetFirstOrDefault(x => x.Id == id);
            if (table == null)
            {
                return NotFound("Table Not Found");
            }
            return Ok(table);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTables()
        {
            IEnumerable<Tables> tables = _unitOfWork.TableRepository.GetAll().ToList();
            return Ok(tables);
        }

        [HttpPost]
        public async Task<IActionResult> CreateNewTable([FromBody] Tables table)
        {
            if (table == null)
            {
                return BadRequest("Invalid table data");
            }

            _unitOfWork.TableRepository.Add(table);
            _unitOfWork.Save();
            return Ok("Table Created Successfully");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Update(int id)
        {
            var table = _unitOfWork.TableRepository.GetFirstOrDefault(x => x.Id == id);
            if (table == null)
            {
                return NotFound("Table Not Found");
            }
            return Ok(table);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] Tables table)
        {
            if (table == null)
            {
                return BadRequest("Invalid table data");
            }

            _unitOfWork.TableRepository.UpdateTable(table);
            _unitOfWork.Save();

            return Ok("Table Updated Successfully");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTable(int id)
        {
            var tableToDelete = _unitOfWork.TableRepository.GetFirstOrDefault(x => x.Id == id);
            if (tableToDelete == null)
            {
                return NotFound("Table Not Found");
            }

            _unitOfWork.TableRepository.Remove(tableToDelete);
            _unitOfWork.Save();

            return Ok("Table Deleted Successfully");
        }
    }

}
