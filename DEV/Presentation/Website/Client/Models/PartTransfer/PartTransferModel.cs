using Common.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Client.Models.PartTransfer
{
    public class PartTransferModel
    {
        public long ClientId { get; set; }
        public long PartTransferId { get; set; }
        public long RequestSiteId { get; set; }  
        public string RequestSite_Name { get; set; }
        public string Request_Location { get; set; }
        public long RequestPartId { get; set; }
        public string RequestPart_Description { get; set; }
        public string RequestPart_ClientLookupId { get; set; }
        public long IssueSiteId { get; set; } 
        public string IssueSite_Name { get; set; }
        public long IssuePartId { get; set; }
        public string IssuePart_ClientLookupId { get; set; }
        public long Creator_PersonnelId { get; set; }
        [Range(1.0, 999999999.999999, ErrorMessage = "globalSixDecimalAfterNineDecimalBeforeTotalFifteenRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        public decimal? Quantity { get; set; }
        public string Reason { get; set; }
        public DateTime? RequiredDate { get; set; }
        [Required(ErrorMessage = "ValidShippingAccount|" + LocalizeResourceSetConstants.Global)]
        public long ShippingAccountId { get; set; }
        public string Status { get; set; }
        public string IssuePart_Description { get; set; }
        public string Description { get; set; }
        public decimal QuantityIssued { get; set; }
        public decimal QuantityReceived { get; set; }
        public decimal QuantityInTransit { get; set; }
        public string LastEvent { get; set; }
        public DateTime? LastEventDate { get; set; }
        public long LastEventBy_PersonnelId { get; set; }
        public string LastEventBy_PersonnelName { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public decimal IssuePart_QtyOnHand { get; set; }
        public string Issue_Location { get; set; }
        public int UpdateIndex { get; set; }
        public IEnumerable<SelectListItem> AccountList { get; set; }
        public string Comment { get; set; }
        public string ForceCompleteReason { get; set; }
        public IEnumerable<SelectListItem> ForceCompleteReasonList { get; set; }
       
    }
}