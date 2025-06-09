using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Admin.Models
{
    public class DropDownModel
    {
        public string text { get; set; }
        public string value { get; set; }
    }

    public class DropDownWithIdModel
    {
        public string text { get; set; }
        public long value { get; set; }
    }
}