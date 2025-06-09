using Client.CustomValidation;

using Common.Constants;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Client.Models.PartIssue
{

    public class PartIssueReturnModel
    {
        public long? PersonnelId { get; set; }
        public long? selectedPersonnelId { get; set; }
        public string IssueToClentLookupId { get; set; }
        [Required(ErrorMessage = "spnPleaseSelectChargeTo|" + LocalizeResourceSetConstants.PartDetails)]
        [Display(Name = "Charge To")]
        public long? ChargeToId { get; set; }
        [Required(ErrorMessage = "spnPleaseSelectChargeTo|" + LocalizeResourceSetConstants.PartDetails)]
        [Display(Name = "Charge To")]
        public string ChargeToClientLookupId { get; set; }
        [Required(ErrorMessage = "spnPleaseSelectChargeType|" + LocalizeResourceSetConstants.PartDetails)]
        [Display(Name = "Charge Type")]
        public string ChargeType { get; set; }
        [Display(Name = "Part ID")]
        [Required(ErrorMessage = "Please select Return From")]
        public string ClientLookupId { get; set; }
        [Required(ErrorMessage = "spnQuantityVal|" + LocalizeResourceSetConstants.PartDetails)]
        [RegularExpression(@"^\d*\.?\d{0,2}$", ErrorMessage = "globalTwoDecimalRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Range(1, 99999999.99, ErrorMessage = "Quantity should be greater than 0 and Valid decimal number having maximum 8 digits before decimal and 2 digits after decimal")]
        [Display(Name = "Quantity")]
        public decimal? Quantity { get; set; }
        [Required(ErrorMessage = "spnPleaseSelectPartId|" + LocalizeResourceSetConstants.Global)]
        public long? PartId { get; set; }
        public string PartDescription { get; set; }
        public string UPCCode { get; set; }
        public string ErrorMessagerow { get; set; }
        [Required(ErrorMessage = "spnPleaseSelectPartId|" + LocalizeResourceSetConstants.Global)]
        public string PartClientLookupId { get; set; }
        public int? IssueToListIndex { get; set; }
        public long? PartStoreroomId { get; set; }
        public bool IsPerformAdjustment { get; set; }     
        public string Comments { get; set; }
        public IEnumerable<SelectListItem> ChargeTypeList { get; set; }
       
        public List<PartIssueReturnModel> list { get; set; }
        public bool MultiStoreroom { get; set; }

        [RequiredIf("MultiStoreroom", true, ErrorMessage = "GlobalStoreroomSelect|" + LocalizeResourceSetConstants.Global)]
        public long? StoreroomId { get; set; }
        public IEnumerable<SelectListItem> StoreroomList { get; set; }
    }
}