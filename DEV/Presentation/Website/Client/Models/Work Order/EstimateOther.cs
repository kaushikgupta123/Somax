using Common.Constants;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Client.Models.Work_Order
{
    public class EstimateOther
    {
        public long EstimatedCostsId { get; set; }
        public long workOrderId { get; set; }
        [Display(Name = "spnPart|" + LocalizeResourceSetConstants.Global)]
        public string ClientLookupId { get; set; }
        public string ObjectType { get; set; }
        public string Category { get; set; }


        [Display(Name = "spnDescription|" + LocalizeResourceSetConstants.Global)]
        public string Description { get; set; }
        [Required(ErrorMessage = "validationunitcost|" + LocalizeResourceSetConstants.Global)]
        [RegularExpression(@"^\d*\.?\d{0,5}$", ErrorMessage = "globalFiveDecimalRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Range(0, 9999999999.99999, ErrorMessage = "globalFiveDecimalAfterTotalFifteenRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        public decimal UnitCost { get; set; }
        [Required(ErrorMessage = "validationquantity|" + LocalizeResourceSetConstants.Global)]
        [RegularExpression(@"^\d*\.?\d{0,2}$", ErrorMessage = "globalTwoDecimalRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Range(0, 99999999.99, ErrorMessage = "globalTwoDecimalAfterTotalTenRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Display(Name = "spnQuantity|" + LocalizeResourceSetConstants.Global)]
        public decimal Quantity { get; set; }

        [Display(Name = "spnSource|" + LocalizeResourceSetConstants.Global)]
        [Required(ErrorMessage = "validationSource|" + LocalizeResourceSetConstants.WorkOrderDetails)]
        public string Source { get; set; }

        [Display(Name = "spnVendor|" + LocalizeResourceSetConstants.Global)]
        public long? VendorId { get; set; }
        public long UpdateIndex { get; set; }
        public string VendorClientLookupId { get; set; }
        public decimal? TotalCost { get; set; }
        public IEnumerable<SelectListItem> SourceList { get; set; }
        public IEnumerable<SelectListItem> VendorLookUpList { get; set; }
    }
}