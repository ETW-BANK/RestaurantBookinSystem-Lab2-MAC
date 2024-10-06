using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Models
{
    public class Tables
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int TableNumber { get; set; }
        [Required]
        public int NumberOfSeats { get; set; }
        [Required]
        public bool IsAvailable { get; set; }



    }
}
