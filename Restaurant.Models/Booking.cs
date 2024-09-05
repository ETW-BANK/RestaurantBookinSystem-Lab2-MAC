using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Models
{
   public class Booking
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime BookingDate { get; set; }= DateTime.Now;

        public int NumberOfGuests {  get; set; }     

        [ForeignKey("CustomerId")]
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }

        [ForeignKey("TablesId")]
        public int TablesId { get; set; }
        public Tables Tables { get; set; }

        [ForeignKey("FoodMenuId")]

        public int? FoodMenuId { get; set; }
        public FoodMenu? FoodMenu { get; set; }  



    }
}
