using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Models.Common
{
    public class UserAccessLookupModel
    {
        public long? SecurityProfileId { get; set; }
        public string SecurityProfileName { get; set; }
        public string SecurityProfileDescription { get; set; }
        public string UserType { get; set; }
        public long TotalCount { get; set; }
        public bool CMMSUser { get; set; }
        public bool SanitationUser { get; set; }
        public int ProductGrouping { get; set; }
        public string PackageLevel { get; set; }
    }
}