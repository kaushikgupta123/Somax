using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Models.Configuration.ApprovalGroups
{
    public class ApprovalGroupsPrintModel
    {
        public long ApprovalGroupId { get; set; }
        public string RequestType { get; set; }
        public string Description { get; set; }
        public string AssetGroup1ClientLookupId { get; set; }
        public string AssetGroup2ClientLookupId { get; set; }
        public string AssetGroup3ClientLookupId { get; set; }
    }
}