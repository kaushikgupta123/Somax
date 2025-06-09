using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Models.Configuration.ApprovalGroups
{
    public class LineItemModel
    {
        public long AppGroupApproversId { get; set; }
        public string Approver { get; set; }
        public int Level { get; set; }
        public long ApproverId { get; set; }
    }
}