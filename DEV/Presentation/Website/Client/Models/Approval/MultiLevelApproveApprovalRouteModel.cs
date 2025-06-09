using Common.Constants;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Client.Models.Approval
{
    public class MultiLevelApproveApprovalRouteModel
    {
        [Required(ErrorMessage = "validationApproverId|" + LocalizeResourceSetConstants.Global)]
        public long ApproverId { get; set; }
        public long ObjectId { get; set; }
        public string Comments { get; set; }
        public string ApproverName { get; set; }
        public int ApproverCount { get; set; }
        public IEnumerable<SelectListItem> ApproverList { get; set; }
        public bool IsMaterialRequest { get; set; }
        public bool IsWorkRequest { get; set; }
        public string RequestType { get; set; }
        public long ApprovalGroupId { get; set; }
    }
}