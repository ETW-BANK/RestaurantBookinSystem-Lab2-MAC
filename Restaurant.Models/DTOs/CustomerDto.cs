using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Models.DTOs
{
    public class CustomerDto
    {


        public int Id { get; set; }


        public string FirstName { get; set; }


        public string LasttName { get; set; }

        [EmailAddress]
        public string Email { get; set; }



        public string Phone { get; set; }


    }
}
