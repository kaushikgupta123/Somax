
using Client.CustomValidation;

using Common.Constants;
using System.ComponentModel.DataAnnotations;


namespace Client.Models.PreventiveMaintenance
{
    public class EditMaterialRequestModel
    {
        public long ClientId { get; set; }
        public long EstimatedCostsId { get; set; }
        [Required(ErrorMessage = "validationDescription|" + LocalizeResourceSetConstants.Global)]
        [MaxLength(127, ErrorMessage = "ValidationMaxLength127Description|" + LocalizeResourceSetConstants.MaterialRequest)]
        public string Description { get; set; }

        [Range(0.01, 99999999.99, ErrorMessage = "globalQuantityNotZeroTwoAfterEightBeforeDecimalErrMsg|" + LocalizeResourceSetConstants.Global)]
        [RegularExpression(@"^\d*\.?\d{0,2}$", ErrorMessage = "globalTwoDecimalRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Required(ErrorMessage = "validationQuantity|" + LocalizeResourceSetConstants.Global)]
        public decimal? Quantity { get; set; }
        public long ObjectId { get; set; }
        public long CategoryId { get; set; }
        public string PartClientLookupId { get; set; }

        [Display(Name = "spnUnitCost|" + LocalizeResourceSetConstants.Global)]
        [RegularExpression(@"^\d*\.?\d{0,5}$", ErrorMessage = "globalFiveDecimalAfterTotalFifteenRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Range(0, 9999999999.99999, ErrorMessage = "globalFiveDecimalAfterTotalFifteenRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        public decimal? UnitCost { get; set; }
        public string Unit { get; set; }

        public bool ShoppingCart { get; set; }
        public bool IsAccountClientLookupIdReq { get; set; }

        public bool IsPartCategoryClientLookupIdReq { get; set; }

        public long? AccountId { get; set; }
        [Display(Name = "spnAccount|" + LocalizeResourceSetConstants.Global)]
        public string AccountClientLookupId { get; set; }
        public long? VendorId { get; set; }
        public string VendorClientLookupId { get; set; }

        public long? PartCategoryMasterId { get; set; }

        [Display(Name = "spnPartCategory|" + LocalizeResourceSetConstants.Global)]
        public string PartCategoryClientLookupId { get; set; }
        public long PrevMaintMasterId { get; set; }
    }
}