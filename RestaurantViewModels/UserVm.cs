﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantViewModels
{
   public class UserVm
    {
        [Required]
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
        public string? Email { get; set; }   
        public string? StreetAddress { get; set; }

        public string? City { get; set; }
        public string? State { get; set; }

        public string? PostalCode { get; set; }

        public string? PhoneNumber { get; set; }    

       


    }
}
