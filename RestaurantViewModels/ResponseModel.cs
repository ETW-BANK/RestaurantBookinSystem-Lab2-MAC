using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantViewModels
{
    public class ResponseModel
    {
        public bool success { get; set; } // Assuming you might have this in your response
        public string message { get; set; } // Assuming you might have this in your response
        public Data data { get; set; }

        public class Data
        {
            public ApplicationUser applicationUser { get; set; }
            public List<RoleList> roleList { get; set; }
        }

        public class ApplicationUser
        {
            public string name { get; set; }
            public string streetAddress { get; set; }
            public string city { get; set; }
            public string state { get; set; }
            public string postalCode { get; set; }
            public string role { get; set; }
            public string id { get; set; }
            // Include other properties as needed
        }

        public class RoleList
        {
            public bool disabled { get; set; }
            public object group { get; set; }
            public bool selected { get; set; }
            public string text { get; set; }
            public string value { get; set; }
        }
    }
}
