using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Models
{
    public class MyBookings
    {
        public int Id { get; set; } 

        public int BookingId { get; set; }   
        [ForeignKey("BookingId")]
        [ValidateNever] 
        public ICollection<Booking> Bookings { get; set; }

        [ForeignKey("ApplicationUserId")]
        [ValidateNever]
        public string ApplicationUserId { get; set; }   
    }
}
