using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantViewModels
{
    public class MenuVM
    {
        public int MenueId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public double Price { get; set; }
        public int Qty { get; set; }
        public IFormFile ImageFile { get; set; }
        public string? CategoryName { get; set; }  // Instead of full Category object, show only Name
        public string AvailableStatus { get; set; }

        public string? Image { get; set; }
    }
}
