using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Common.Constants;
namespace Client.Models.PreventiveMaintenance
{
    public class EstimateOtherModel
    {
        public long EstimatedCostsId { get; set; }
        public string ObjectType { get; set; }
        public long PrevMaintMasterId { get; set; }
        public string PrevmaintClientlookUp { get; set; }
        public string Category { get; set; }
        [Display(Name = "spnDescription|" + LocalizeResourceSetConstants.Global)]
        [Required(ErrorMessage = "validationDescription|" + LocalizeResourceSetConstants.Global)]
        public string Description { get; set; }        
        [Display(Name = "spnUnitCost|" + LocalizeResourceSetConstants.Global)]
        [Required(ErrorMessage = "validationUnitCost|" + LocalizeResourceSetConstants.Global)]
        [RegularExpression(@"^\d*\.?\d{0,5}$", ErrorMessage = "globalFiveDecimalRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Range(0, 9999999999.99999, ErrorMessage = "globalFiveDecimalAfterTotalFifteenRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        public decimal? UnitCost { get; set; }        
        [Display(Name = "spnQuantity|" + LocalizeResourceSetConstants.Global)]
        [RegularExpression(@"^\d*\.?\d{0,2}$", ErrorMessage = "globalTwoDecimalRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Range(0, 99999999.99, ErrorMessage = "globalTwoDecimalAfterEightDecimalBeforeErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Required(ErrorMessage = "validationQuantity|" + LocalizeResourceSetConstants.Global)]
        public decimal? Quantity { get; set; }
        [Display(Name = "spnSource|" + LocalizeResourceSetConstants.Global)]
        [Required(ErrorMessage = "validationSource|" + LocalizeResourceSetConstants.Global)]      
        public string Source { get; set; }
        [Display(Name = "spnVendor|" + LocalizeResourceSetConstants.Global)]
        public long? VendorId { get; set; }
        public long UpdateIndex { get; set; }
        public string VendorClientLookupId { get; set; }
        public IEnumerable<SelectListItem> SourceList { get; set; }
        public IEnumerable<SelectListItem> VendorLookUpList { get; set; }
    }
}