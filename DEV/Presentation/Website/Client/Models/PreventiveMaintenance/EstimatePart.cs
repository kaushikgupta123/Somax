using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Common.Constants;
namespace Client.Models.PreventiveMaintenance
{
    public class EstimatePart
    {
        [Display(Name = "spnPart|" + LocalizeResourceSetConstants.Global)]
        [Required(ErrorMessage = "validationselectpart|" + LocalizeResourceSetConstants.PrevMaintDetails)]       
        public string ClientLookupId { get; set; }
        public string PrevmaintClientlookUp { get; set; }
        public long EstimatedCostsId { get; set; }
        public long PrevMaintMasterId { get; set; }
        [MaxLength(127, ErrorMessage = "ValidationMaxLength127Description|" + LocalizeResourceSetConstants.MaterialRequest)]
        [Required(ErrorMessage = "validationDescription|" + LocalizeResourceSetConstants.Global)]
        public string Description { get; set; }

        [RegularExpression(@"^\d*\.?\d{0,5}$", ErrorMessage = "globalFiveDecimalAfterTotalFifteenRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Range(0, 9999999999.99999, ErrorMessage = "globalFiveDecimalAfterTotalFifteenRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        public decimal? UnitCost { get; set; }

        [Range(0, 9999999999.99999, ErrorMessage = "globalFiveDecimalAfterTotalFifteenRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [RegularExpression(@"^\d*\.?\d{0,2}$", ErrorMessage = "globalTwoDecimalRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Required(ErrorMessage = "validationQuantity|" + LocalizeResourceSetConstants.Global)]
        public decimal? Quantity { get; set; }
        public decimal? TotalCost { get; set; }

        public IEnumerable<SelectListItem> PartsList { get; set; }
        public bool ShoppingCart { get; set; }
        public string Unit { get; set; }

        public long? AccountId { get; set; }
        [Display(Name = "spnAccount|" + LocalizeResourceSetConstants.Global)]
        public string AccountClientLookupId
        {
            get; set;
        }
        public long? VendorId { get; set; }
        public string VendorClientLookupId { get; set; }
    }
}