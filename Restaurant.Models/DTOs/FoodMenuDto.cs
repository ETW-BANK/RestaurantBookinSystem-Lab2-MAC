using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Models.DTOs
{
    public class FoodMenuDto
    {
      
        public int Id { get; set; }

  
        public string Title { get; set; }

        public double price { get; set; }   
      
        public bool IsAvailable { get; set; }

     
        public string? ImageUrl { get; set; }
    }
}
