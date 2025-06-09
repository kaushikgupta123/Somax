using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Models.Configuration.Craft
{
    public class CraftPrintModel
    {
        public string Craft { get; set; }
        public string Description { get; set; }
        public string Rate { get; set; }
        public bool Inactive { get; set; }
    }
}