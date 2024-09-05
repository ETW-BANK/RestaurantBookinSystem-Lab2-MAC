using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Models.DTOs
{
    public class BookingDto
    {
        public int Id { get; set; }
        public DateTime BookingDate { get; set; }
        public int NumberOfGuests { get; set; }
        public int CustomerId { get; set; }
        public int TablesId { get; set; }
        public int FoodMenuId { get; set; }

        public CustomerDto? Customer { get; set; }
        public FoodMenuDto? FoodMenu { get; set; }

        public TablesDto? Tables { get; set; }

      

    }
}
