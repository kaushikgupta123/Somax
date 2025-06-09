using Client.CustomValidation;
using Common.Constants;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Client.Models.Work_Order
{
    public class EstimatePart
    {

        [Display(Name = "spnPart|" + LocalizeResourceSetConstants.Global)]
        public string ClientLookupId { get; set; }
        public string MainClientLookupId { get; set; }
        public long WorkOrderId { get; set; }
        public long EstimatedCostsId { get; set; }
        [MaxLength(127, ErrorMessage = "ValidationMaxLength127Description|" + LocalizeResourceSetConstants.MaterialRequest)]
        [Required(ErrorMessage = "validationDescription|" + LocalizeResourceSetConstants.Global)]
        public string Description { get; set; }

        [Display(Name = "spnUnitCost|" + LocalizeResourceSetConstants.Global)]
        [Required(ErrorMessage = "validationUnitCost|" + LocalizeResourceSetConstants.Global)]
        [RegularExpression(@"^\d*\.?\d{0,5}$", ErrorMessage = "globalFiveDecimalRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Range(0.01, 9999999999.99999, ErrorMessage = "globalFiveDecimalAfterTotalFifteenRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        public decimal? UnitCost { get; set; }

        [Display(Name = "spnQuantity|" + LocalizeResourceSetConstants.Global)]
        [RegularExpression(@"^\d*\.?\d{0,2}$", ErrorMessage = "globalTwoDecimalRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Range(0.01, 99999999.99, ErrorMessage = "spnValidationQuantityNotZero|" + LocalizeResourceSetConstants.MaterialRequest)]
        [Required(ErrorMessage = "validationQuantity|" + LocalizeResourceSetConstants.Global)]
        public decimal? Quantity { get; set; }

        [Required(ErrorMessage = "spnValidUnitofMeasure|" + LocalizeResourceSetConstants.Global)]
        public string Unit { get; set; }//V2-1068

        public bool ShoppingCart { get; set; }

        public bool IsAccountClientLookupIdReq { get; set; }
        public bool IsPartCategoryClientLookupIdReq { get; set; }

        public long? AccountId { get; set; }//V2-1068
        [Display(Name = "spnAccount|" + LocalizeResourceSetConstants.Global)]
        [RequiredIf("IsAccountClientLookupIdReq", true, ErrorMessage = "validationAccountId|" + LocalizeResourceSetConstants.Global)]
        public string AccountClientLookupId { get; set; }//V2-1068
        public long? VendorId { get; set; }//V2-1068
        public string VendorClientLookupId { get; set; }//V2-1068
        public long? PartCategoryMasterId { get; set; }//V2-1068

        [Display(Name = "spnPartCategory|" + LocalizeResourceSetConstants.Global)]
        [RequiredIf("IsPartCategoryClientLookupIdReq", true, ErrorMessage = "validationPartCategoryId|" + LocalizeResourceSetConstants.Global)]
        public string PartCategoryClientLookupId { get; set; }//V2-1068
        public decimal? TotalCost { get; set; }
        public long CategoryId { get; set; }//V2-690
        public IEnumerable<SelectListItem> PartsList { get; set; }
        public string Status { get; set; }//V2-726  
        public long PurchaseRequestId { get; set; } //V2-1078
    }
}