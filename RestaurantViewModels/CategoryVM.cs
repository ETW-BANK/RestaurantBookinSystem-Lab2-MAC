


using Restaurant.Models;
using System.ComponentModel.DataAnnotations;

namespace RestaurantViewModels
{
    public class CategoryVM
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }

        public string? ImageUrl { get; set; }

    }
}
