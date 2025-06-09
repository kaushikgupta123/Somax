using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Models.Common
{
    public class PersonnelLookUpModel
    {
        public string ClientLookupId { get; set; }
        public string NameFirst { get; set; }
        public string NameLast { get; set; }
        public int TotalCount { get; set; }
        public long? PersonnelId { get; set; }
        public string PClientLookupId { get; set; }
    }
}