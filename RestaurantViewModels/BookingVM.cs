using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Restaurant.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantViewModels
{
    public class BookingVM
    {
        public int BookingId { get; set; }

        [Required]
        public DateOnly BookingDate { get; set; }

        [Required]
        public string BookingTime { get; set; }

        [Required]
        [Range(1, 20, ErrorMessage = "Number of guests must be between 1 and 20.")]
        public int NumberOfGuests { get; set; }

        [Required]
        public int TableId { get; set; }

        public int TableNumber { get; set; }  
        public string applicationUserId { get; set; }
       
        public string Name { get; set; } 
        public string Phone { get; set; }  
        public string Email { get; set; }  


    }
}


    

