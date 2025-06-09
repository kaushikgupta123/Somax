using Client.Common;
using Client.CustomValidation;

using Common.Constants;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Client.Models.MaterialRequest
{
    public class PartNotInInventoryModel
    {
        public long ClientId { get; set; }
        public long EstimatedCostsId { get; set; }
        [Display(Name = "spnDescription|" + LocalizeResourceSetConstants.Global)]
        [Required(ErrorMessage = "validationDescription|" + LocalizeResourceSetConstants.Global)]
        [MaxLength(127, ErrorMessage = "ValidationMaxLength127Description|" + LocalizeResourceSetConstants.MaterialRequest)]
        public string Description { get; set; }
        [RegularExpression(@"^\d*\.?\d{0,2}$", ErrorMessage = "globalTwoDecimalRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Required(ErrorMessage = "validationQuantity|" + LocalizeResourceSetConstants.Global)]
        [Range(0.01, 99999999.99, ErrorMessage = "spnValidationQuantityNotZero|" + LocalizeResourceSetConstants.MaterialRequest)]
        public decimal? Quantity { get; set; }
        public long ObjectId { get; set; }
        public long CategoryId { get; set; }
        public string PartClientLookupId { get; set; }

        #region V2-1148 added properties
        [Display(Name = "spnUnitCost|" + LocalizeResourceSetConstants.Global)]
        [RequiredIf("IsAddPartNotInInventoryForm", true, ErrorMessage = "validationUnitCost|" + LocalizeResourceSetConstants.Global)]
        [RegularExpression(@"^\d*\.?\d{0,5}$", ErrorMessage = "globalFiveDecimalRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Range(0.01, 9999999999.99999, ErrorMessage = "globalFiveDecimalAfterTotalFifteenRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        public decimal? UnitCost { get; set; }//1148
        public bool ShoppingCart { get; set; }

        [RequiredIf("IsAddPartNotInInventoryForm", true, ErrorMessage = "spnValidUnitofMeasure|" + LocalizeResourceSetConstants.Global)]
        public string Unit { get; set; }//1148
        public long? AccountId { get; set; }//V2-1148
        [Display(Name = "spnAccount|" + LocalizeResourceSetConstants.Global)]
        [RequiredIf("IsAccountClientLookupIdReq", true, ErrorMessage = "validationAccountId|" + LocalizeResourceSetConstants.Global)]
        public string AccountClientLookupId { get; set; }//1148
        public bool IsAccountClientLookupIdReq { get; set; }//1148
        public long? VendorId { get; set; }//1148
        public string VendorClientLookupId { get; set; }//1148
        public bool IsPartCategoryClientLookupIdReq { get; set; }//1148
        public long? PartCategoryMasterId { get; set; }//1148
        [Display(Name = "spnPartCategory|" + LocalizeResourceSetConstants.Global)]
        [RequiredIf("IsPartCategoryClientLookupIdReq", true, ErrorMessage = "validationPartCategoryId|" + LocalizeResourceSetConstants.Global)]
        public string PartCategoryClientLookupId { get; set; }//1148
        public bool IsAddPartNotInInventoryForm { get; set; }
        #endregion
    }
}