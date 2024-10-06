using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantViewModels
{
    public class TablesVM
    {

        [Required]
        [Key]
        public int Id { get; set; }

        [Required]
        public int TableNumber { get; set; }
        [Required]
        public int NumberOfSeats { get; set; }
        public bool IsAvailable { get; set; }
    }
}
