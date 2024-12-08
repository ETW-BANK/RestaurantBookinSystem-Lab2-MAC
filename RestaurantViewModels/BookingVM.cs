using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Restaurant.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        public TimeOnly BookingTime { get; set; }

        [Required]
        [Range(1, 20, ErrorMessage = "Number of guests must be between 1 and 20.")]
        public int NumberOfGuests { get; set; }

        [Required]
        public int TableId { get; set; }

        public Tables Table { get; set; }

        public string Userid { get; set; }  
        public string Name { get; set; }   
        public string UserName { get; set; }    
        public string Email { get; set; }   
      
        public ApplicationUser User { get; set; }   


    }
}
