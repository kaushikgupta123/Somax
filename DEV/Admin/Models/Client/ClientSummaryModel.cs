using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Admin.Models.Client
{
    public class ClientSummaryModel
    {
        public long ClientId { get; set; }       
        public string Name { get; set; }        
        public string Contact { get; set; }        
        public string Email { get; set; }
        public string BusinessType { get; set; }        
        public string PackageLevel { get; set; }        
        public DateTime CreateDate { get; set; }       
        public string Status { get; set; }
    }
}