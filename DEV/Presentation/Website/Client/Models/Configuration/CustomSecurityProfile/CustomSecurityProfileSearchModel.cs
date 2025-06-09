using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Models.Configuration.CustomSecurityProfile
{
    public class CustomSecurityProfileSearchModel
    {
        public long SecurityProfileId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int TotalCount { get; set; }
    }
}