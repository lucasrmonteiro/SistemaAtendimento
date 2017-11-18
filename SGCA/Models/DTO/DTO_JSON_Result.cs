using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SGCA.Models.DTO
{
    public class DTO_JSON_Result
    {
        public int status { get; set; }
        public object result { get; set; }
        public string msg { get; set; }
    }
}