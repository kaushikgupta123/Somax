using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Admin.Models
{
    public class ChangeClientIDModel
    {
        public long ClientId { get; set; }        
        public int UpdateIndex { get; set; }
    }
}