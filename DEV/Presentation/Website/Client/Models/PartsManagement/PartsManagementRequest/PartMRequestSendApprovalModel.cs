using Common.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace Client.Models.PartsManagement.PartsManagementRequest
{
    public class PartMRequestSendApprovalModel
    {
        public long? PartMasterRequestId { get; set; }
        public IEnumerable<SelectListItem> SendToIdList { get; set; }       
        public long? SendToId { get; set; }
        [Required(ErrorMessage = "ValidcommentstosendthistoanotheruserReviewComments|" + LocalizeResourceSetConstants.PartsManagementDetail)]
        public string Comment { get; set; }
        public string RequestType { get; set; }
    }
}