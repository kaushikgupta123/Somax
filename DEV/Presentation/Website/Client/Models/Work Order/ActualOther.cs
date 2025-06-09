using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Common.Constants;
namespace Client.Models.Work_Order
{
    public class ActualOther
    {
        public long workOrderId { get; set; }
        public string ClientLookupId { get; set; }
        public long OtherCostsId { get; set; }
        [Display(Name = "spnSource|" + LocalizeResourceSetConstants.Global)]
        [Required(ErrorMessage = "validSource|" + LocalizeResourceSetConstants.WorkOrderDetails)]
        public string Source { get; set; }
        [Display(Name = "spnVendorID|" + LocalizeResourceSetConstants.Global)]
        public long? VendorId { get; set; }
        public string VendorClientLookupId { get; set; }
        [Display(Name = "spnDescription|" + LocalizeResourceSetConstants.Global)]
        public string Description { get; set; }
        [Display(Name = "spnUnitCost|" + LocalizeResourceSetConstants.Global)]  
        [RegularExpression(@"^\d*\.?\d{0,5}$", ErrorMessage = "globalFiveDecimalRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Range(0, 9999999999.99999, ErrorMessage = "globalFiveDecimalAfterTotalFifteenRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        public decimal? UnitCost { get; set; }
        [Display(Name = "spnQuantity|" + LocalizeResourceSetConstants.Global)]
        [RegularExpression(@"^\d*\.?\d{0,2}$", ErrorMessage = "globalTwoDecimalRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Range(0, 99999999.99, ErrorMessage = "globalTwoDecimalAfterTotalTenRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        public decimal? Quantity { get; set; }
        public decimal TotalCost { get; set; }
        public IEnumerable<SelectListItem> SourceList { get; set; }
        public IEnumerable<SelectListItem> VendorLookUpList { get; set; }
    }

}