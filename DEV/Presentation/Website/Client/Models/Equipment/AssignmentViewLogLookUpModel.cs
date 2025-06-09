using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Models.Equipment
{
    public class AssignmentViewLogLookUpModel
    {

        public long ObjectId { get; set; }
        public string TransactionDate { get; set; }
        public string Status { get; set; }
        public long PersonnelId { get; set; }
        public long Assigned { get; set; }
        public string Location { get; set; }
        public long ParentId { get; set; }
        public long AssetGroup1 { get; set; }
        public long AssetGroup2 { get; set; }
        public long AssetGroup3 { get; set; }
        public long TotalCount { get; set; }
        public string PersonnelName { get; set; }
        public string ParentClientLookupId { get; set; }
        public string AssetGroup1Name { get; set; }
        public string AssetGroup2Name { get; set; }
        public string AssetGroup3Name { get; set; }
        public string AssignedClientLookupId { get; set; }
    }
}