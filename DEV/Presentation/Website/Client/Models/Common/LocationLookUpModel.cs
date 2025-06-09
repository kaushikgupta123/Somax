using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Models.Common
{
    public class LocationLookUpModel
    {
        public long LocationId { get; set; }
        public string ClientLookupId { get; set; }
        public string Name { get; set; }
        public string Complex { get; set; }
        public string Type { get; set; }
        public long TotalCount { get; set; }
    }
}