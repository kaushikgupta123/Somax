using Client.CustomValidation;
using Common.Constants;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Client.Models.MaterialRequest
{
    public class PartInInventoryModel
    {
        public long ClientId { get; set; }
        public long EstimatedCostsId { get; set; }
        [Display(Name = "spnDescription|" + LocalizeResourceSetConstants.Global)]
        public string Description { get; set; }
        [RegularExpression(@"^\d*\.?\d{0,2}$", ErrorMessage = "globalTwoDecimalRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Required(ErrorMessage = "validationQuantity|" + LocalizeResourceSetConstants.Global)]
        [Range(0.01, 99999999.99, ErrorMessage = "spnValidationQuantityNotZero|" + LocalizeResourceSetConstants.MaterialRequest)]
        public decimal? Quantity { get; set; }
        public long ObjectId { get; set; }
        public long CategoryId { get; set; }
        public string PartClientLookupId { get; set; }
        #region V2-1148 added properties
        public bool ShoppingCart { get; set; }
        [Display(Name = "spnUnitCost|" + LocalizeResourceSetConstants.Global)]
        [RequiredIf("ShoppingCart", true, ErrorMessage = "validationUnitCost|" + LocalizeResourceSetConstants.Global)]
        [RegularExpression(@"^\d*\.?\d{0,5}$", ErrorMessage = "globalFiveDecimalRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Range(0.01, 9999999999.99999, ErrorMessage = "globalFiveDecimalAfterTotalFifteenRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        public decimal? UnitCost { get; set; }
        [Display(Name = "spnUnitCost|" + LocalizeResourceSetConstants.Global)]
        [RegularExpression(@"^\d*\.?\d{0,5}$", ErrorMessage = "globalFiveDecimalRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Range(0, 9999999999.99999, ErrorMessage = "globalFiveDecimalAfterTotalFifteenRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        public decimal? UnitCostStockPart { get; set; }
        [RequiredIf("ShoppingCart", true, ErrorMessage = "spnValidUnitofMeasure|" + LocalizeResourceSetConstants.Global)]
        public string Unit { get; set; }
        public long? AccountId { get; set; }
        [Display(Name = "spnAccount|" + LocalizeResourceSetConstants.Global)]
        [RequiredIf("IsAccountClientLookupIdReq", true, ErrorMessage = "validationAccountId|" + LocalizeResourceSetConstants.Global)]
        public string AccountClientLookupId { get; set; }
        public bool IsAccountClientLookupIdReq { get; set; }
        public long? VendorId { get; set; }
        public string VendorClientLookupId { get; set; }
        public bool IsPartCategoryClientLookupIdReq { get; set; }
        public long? PartCategoryMasterId { get; set; }
        [Display(Name = "spnPartCategory|" + LocalizeResourceSetConstants.Global)]
        [RequiredIf("IsPartCategoryClientLookupIdReq", true, ErrorMessage = "validationPartCategoryId|" + LocalizeResourceSetConstants.Global)]
        public string PartCategoryClientLookupId { get; set; }
        #endregion
    }
}