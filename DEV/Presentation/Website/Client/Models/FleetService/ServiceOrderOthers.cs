using Common.Constants;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Client.Models.FleetService
{
    public class ServiceOrderOthers
    {        
        public long OtherCostsId { get; set; }
        [Display(Name = "spnSource|" + LocalizeResourceSetConstants.Global)]
        [Required(ErrorMessage = "validSource|" + LocalizeResourceSetConstants.FleetServiceOrder)]
        public string Source { get; set; }
        [Display(Name = "spnVendorID|" + LocalizeResourceSetConstants.Global)]
        public long? VendorId { get; set; }
        public string VendorClientLookupId { get; set; }       
        public string Description { get; set; }
        [Display(Name = "spnUnitCost|" + LocalizeResourceSetConstants.Global)]
        [Required(ErrorMessage = "validUnitCost|" + LocalizeResourceSetConstants.FleetServiceOrder)]
        [RegularExpression(@"^\d*\.?\d{0,5}$", ErrorMessage = "globalFiveDecimalRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Range(0, 9999999999.99999, ErrorMessage = "globalFiveDecimalAfterTotalFifteenRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        public decimal? UnitCost { get; set; }
        [Display(Name = "spnQuantity|" + LocalizeResourceSetConstants.Global)]
        [Required(ErrorMessage = "validQuantity|" + LocalizeResourceSetConstants.FleetServiceOrder)]
        [RegularExpression(@"^\d*\.?\d{0,2}$", ErrorMessage = "globalTwoDecimalRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Range(0, 99999999.99, ErrorMessage = "globalTwoDecimalAfterTotalTenRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        public decimal? Quantity { get; set; }
        public decimal TotalCost { get; set; }
        public long ServiceOrderLineItemId { get; set; }
        public long ServiceOrderId { get; set; }       
        public IEnumerable<SelectListItem> SourceList { get; set; }
        
    }
}