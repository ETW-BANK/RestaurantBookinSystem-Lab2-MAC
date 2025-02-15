using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Models
{
    public class Menue
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public double Price { get; set; }
        public int Qty { get; set; }    

        public string? Image { get; set; }

        public int CategoryId { get; set; }

        [ForeignKey(nameof(CategoryId))]

        public Category? Category { get; set; }  
        public Available Available { get; set; }    


    }

    public enum Available
    {
         NO,
         Yes
    }
}
