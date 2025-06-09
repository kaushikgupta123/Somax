using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Models.Configuration.ApprovalGroups
{
    public class PersonnelApprovalModel
    {
        public long? PersonnelId { get; set; }
        public string ClientLookupId { get; set; }
        public string FullName { get; set; }
        public string AssetGroup1 { get; set; }
        public string AssetGroup2 { get; set; }
        public string AssetGroup3 { get; set; }
        public string AssetGroup1ClientlookUpId { get; set; }
        public string AssetGroup2ClientlookUpId { get; set; }
        public string AssetGroup3ClientlookUpId { get; set; }
        public int TotalCount { get; set; }
    }
}