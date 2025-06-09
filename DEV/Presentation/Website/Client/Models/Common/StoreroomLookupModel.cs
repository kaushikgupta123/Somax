using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Models.Common
{
    public class StoreroomLookupModel
    {
        public long StoreroomId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public long TotalCount { get; set; }
    }
}