using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
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

        public DateOnly BookingDate { get; set; }   

        public TimeOnly BookingTime { get; set; }   

        public int NumberOfGuests { get; set; }
        public string ApplicationUserId { get; set; }   
        [ForeignKey("ApplictionUserId")]
        [ValidateNever]
        public ApplicationUser ApplicationUser { get; set; }

        public int TableId { get; set; }
        [ForeignKey("TableId")]

        [ValidateNever]
        public Tables Tables { get; set; }
    }
}
