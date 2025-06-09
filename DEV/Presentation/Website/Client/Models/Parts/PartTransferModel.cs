using Common.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Client.Models.Parts
{
    public class PartTransferModel
    {
        //public string ClientLookupId { get; set; }
        //public string LongDescription { get; set; }
        //public string ShortDescription { get; set; }
        //public string Description { get; set; }
        //public long PartId { get; set; }       
        public string SiteName { get; set; }      
        public decimal QtyOnHand { get; set; }
        public string Reason { get; set; }
        [RegularExpression(@"^\d*\.?\d{0,6}$", ErrorMessage = "globalSixDecimalRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Range(1.0, 999999999.999999, ErrorMessage = "globalSixDecimalAfterNineDecimalBeforeTotalFifteenRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Required(ErrorMessage = "spnQuantityVal|" + LocalizeResourceSetConstants.PartDetails)]
        public decimal? Qty { get; set; }
        public DateTime? RequiredDate { get; set; }
        [Required(ErrorMessage = "ValidShippingAccount|" + LocalizeResourceSetConstants.Global)]
        public long? ShippingAccountId { get; set; }
        public IEnumerable<SelectListItem> ShippingAccountList { get; set; }
        #region V2-862
        public long IssuePartId { get; set; }
        public long RequestPartId { get; set; }
        public string IssueClientLookupId { get; set; }
        public string RequestClientLookupId { get; set; }
        public string IssueDescription { get; set; }
        public string RequestDescription { get; set; }
        #endregion
    }
}