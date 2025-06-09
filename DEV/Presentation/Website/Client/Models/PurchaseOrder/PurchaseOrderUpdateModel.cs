using Client.CustomValidation;
using Common.Constants;
using DataContracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
namespace Client.Models.PurchaseOrder
{
    public class PurchaseOrderUpdateModel
    {
        public long PurchaseOrderId { get; set; }
        public long? SiteId { get; set; }
        public long? DepartmentId { get; set; }
        public long? AreaId { get; set; }
        public long? StoreroomId { get; set; }
        public string ClientLookupId { get; set; }
        public string Attention { get; set; }
        public long? Buyer_PersonnelId { get; set; }
        public string Carrier { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        [DataType(DataType.Date)]

        [Display(Name = "spnPoRequired|" + LocalizeResourceSetConstants.PurchaseOrder)]
        public DateTime? Required { get; set; }
        public string FOB { get; set; }
        public string Terms { get; set; }
        public string Reason { get; set; }
        public string MessageToVendor { get; set; }
        public IEnumerable<SelectListItem> BuyerList { get; set; }
        public IEnumerable<SelectListItem> FOBList { get; set; }
        public IEnumerable<SelectListItem> TermsList { get; set; }
    }
}