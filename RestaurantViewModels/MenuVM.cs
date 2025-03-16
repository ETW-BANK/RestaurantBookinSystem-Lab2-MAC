
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using Restaurant.Models;
using System.ComponentModel.DataAnnotations;


namespace RestaurantViewModels
{
    public class MenuVM
    {
      public Menue Menue { get; set; }  
        public IEnumerable<SelectListItem>? CategoryList { get; set; }
 
        public Available? Available { get; set; }

    }

   
}

