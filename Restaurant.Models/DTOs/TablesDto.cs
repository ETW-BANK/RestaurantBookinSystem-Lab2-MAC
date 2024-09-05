using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Models.DTOs
{
    public class TablesDto
    {
     
        public int Id { get; set; }

     
        public int TableNumber { get; set; }

      
        public int NumberOfSeats { get; set; }

       

        public bool isAvialable { get; set; }
    }
}
