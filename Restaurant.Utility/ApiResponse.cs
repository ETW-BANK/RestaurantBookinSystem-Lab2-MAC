using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Utility
{
   public class ApiResponse<T>
    {
        public T Data { get; set; }
        public string Error { get; set; }
    }
}
