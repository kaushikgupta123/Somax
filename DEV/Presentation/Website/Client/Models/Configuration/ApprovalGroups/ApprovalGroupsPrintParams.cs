using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Client.Models.Common; 

namespace Client.Models.Configuration.ApprovalGroups
{
    [Serializable]
    public class ApprovalGroupsPrintParams
    {
        public ApprovalGroupsPrintParams()
        {
            tableHaederProps = new List<TableHaederProp>();
        }
        public List<TableHaederProp> tableHaederProps { get; set; }
        public string colname { get; set; }
        public string coldir { get; set; }
        public string ApprovalGroupId { get; set; }
        public string RequestType { get; set; }
        public string Description { get; set; }
        //public string AssetGroup1ClientLookupId { get; set; }
        //public string AssetGroup2ClientLookupId { get; set; }
        //public string AssetGroup3ClientLookupId { get; set; }
        public long AssetGroup1Id { get; set; }
        public long AssetGroup2Id { get; set; }
        public long AssetGroup3Id { get; set; }
        public string SearchText { get; set; }
    }
}