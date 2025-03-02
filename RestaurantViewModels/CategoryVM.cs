


using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Restaurant.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantViewModels
{
    public class CategoryVM
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int? DisplayOrder { get; set; }

        [NotMapped] // Prevents EF from trying to map this property to the database
        public IFormFile? Image { get; set; }


        [ValidateNever] // Prevents validation issues during model binding
        public string? ImageUrl { get; set; }
    }
}
