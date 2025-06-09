using Client.Common;
using Client.CustomValidation;
using Common.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
 
namespace Client.Models.PurchaseRequest
{
    public class PartNotInInventoryModel
    {
        [Display(Name = "spnDescription|" + LocalizeResourceSetConstants.Global)]
        [Required(ErrorMessage = "validationDescription|" + LocalizeResourceSetConstants.Global)]
        public string Description { get; set; }
        //[Display(Name = "spnUnitofMeasure|" + LocalizeResourceSetConstants.Global)]
        //[Required(ErrorMessage = "validationUOM|" + LocalizeResourceSetConstants.Global)]
        //public string UnitofMeasure { get; set; }
        [RequiredAsPerUI(UiConfigConstants.PurchaseRequestLineItemAdd, UiConfigConstants.PurchaseRequestLineItemEdit, "PurchaseRequestLineItemId", "0", null, null, ErrorMessage = "validationAccountId|" + LocalizeResourceSetConstants.Global)]
        [Display(Name = "spnAccountId|" + LocalizeResourceSetConstants.Global)]
        // [Required(ErrorMessage = "validationAccountId|" + LocalizeResourceSetConstants.PurchaseRequest)]
        public long? AccountId { get; set; }
        [Display(Name = "spnChargeType|" + LocalizeResourceSetConstants.Global)]
        [Required(ErrorMessage = "validationChargeType|" + LocalizeResourceSetConstants.Global)]
        public string ChargeType { get; set; }
        [Display(Name = "GlobalChargeTo|" + LocalizeResourceSetConstants.Global)]
        [Required(ErrorMessage = "validationChargeToId|" + LocalizeResourceSetConstants.PurchaseRequest)]
        public long ChargeToID { get; set; }
        public long PurchaseRequestId { get; set; }

        [Required(ErrorMessage = "ValidationChargeToID|" + LocalizeResourceSetConstants.EventInfo)]
        public string ChargeToClientLookupIdToShow { get; set; }

        public Int64 PurchaseRequestLineItemId { get; set; }
        public int UpdateIndex { get; set; }
        public string ClientLookupId { get; set; }
        public IEnumerable<SelectListItem> AccountList { get; set; }
        public IEnumerable<SelectListItem> ChargeTypeList { get; set; }
        public IEnumerable<SelectListItem> UOMList { get; set; }
        public IEnumerable<SelectListItem> ChargeTypelookUpList { get; set; }
        [RequiredAsPerUI(UiConfigConstants.PurchaseRequestLineItemAdd, UiConfigConstants.PurchaseRequestLineItemEdit, "PurchaseRequestLineItemId", "0", null, null, ErrorMessage = "validationQuantity|" + LocalizeResourceSetConstants.Global)]
        [RegularExpression(@"^\d*\.?\d{0,6}$", ErrorMessage = "globalSixDecimalRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Range(0, 999999999.999999, ErrorMessage = "globalSixDecimalAfterNineDecimalBeforeTotalFifteenRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        public decimal? OrderQuantity { get; set; }
        [RequiredAsPerUI(UiConfigConstants.PurchaseRequestLineItemAdd, UiConfigConstants.PurchaseRequestLineItemEdit, "PurchaseRequestLineItemId", "0", null, null, ErrorMessage = "validationUnitCost|" + LocalizeResourceSetConstants.Global)]
        [RegularExpression(@"^\d*\.?\d{0,5}$", ErrorMessage = "globalFiveDecimalRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Range(0, 9999999999.99999, ErrorMessage = "globalFiveDecimalAfterTotalFifteenRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        public decimal? UnitCost { get; set; }
        public string ChargeTo_Name { get; set; }
        public string ViewName { get; set; }
        
         [Display(Name = "spnUnitofMeasure|" + LocalizeResourceSetConstants.Global)]
        [Required(ErrorMessage = "validationUOM|" + LocalizeResourceSetConstants.Global)]
        public string PurchaseUOM { get; set; }
        [Display(Name = "Required Date" + LocalizeResourceSetConstants.Global)]
        [FutureDateValidation(ErrorMessage = "Required date should not be a past date")]
        [RequiredIf("IsShopingCart", "true", ErrorMessage = "Please enter required date")]
        public DateTime? RequiredDate { get; set; }
        public bool IsShopingCart { get; set; }

        public string Status { get; set; }
    }
}