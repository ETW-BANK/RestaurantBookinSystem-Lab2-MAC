


using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Restaurant.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantViewModels
{
    public class CategoryVM
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(30)]
        public string? Name { get; set; }

        public int DisplayOrder { get; set; }

       ///* public IFormFile? Image { get; set; } // */Nullable to prevent null reference errors

        public string? ImageUrl { get; set; }

    }
}
