using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Client.Models.Configuration.ApprovalGroups
{
    public class ApprovalGroupsModel
    {
        public long ApprovalGroupId { get; set; }
        public string RequestType { get; set; }
        public string Description { get; set; }
        public string AssetGroup1ClientLookupId { get; set; }
        public string AssetGroup2ClientLookupId { get; set; }
        public string AssetGroup3ClientLookupId { get; set; }
        public long? AssetGroup1Id { get; set; }
        public long? AssetGroup2Id { get; set; }
        public long? AssetGroup3Id { get; set; }
        public int TotalCount { get; set; }
        public int ChildCount { get; set; }
        public IEnumerable<SelectListItem> AssetGroup1List { get; set; }
        public IEnumerable<SelectListItem> AssetGroup2List { get; set; }
        public IEnumerable<SelectListItem> AssetGroup3List { get; set; }
        public IEnumerable<SelectListItem> RequestTypeList { get; set; }
    }
}