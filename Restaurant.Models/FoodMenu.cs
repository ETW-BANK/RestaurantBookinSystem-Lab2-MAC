using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Models
{
    public class FoodMenu
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public double Price { get; set; }   

        [Required]  
        public bool IsAvailable { get; set; }  

        [ValidateNever]
        public string? ImageUrl { get; set; }

        public ICollection<Booking>? MenuBooking { get; set; }
    }
}
