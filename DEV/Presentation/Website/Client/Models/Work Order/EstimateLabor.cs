using Common.Constants;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Client.Models.Work_Order
{
    public class EstimateLabor
    {
        public string ClientLookupId { get; set; }
        public long EstimatedCostsId { get; set; }
        public long workOrderId { get; set; }
        public string PrevmaintClientlookUp { get; set; }

        [Display(Name = "spnDuration|" + LocalizeResourceSetConstants.Global)]
        [Required(ErrorMessage = "validationDuration|" + LocalizeResourceSetConstants.WorkOrderDetails)]
        [RegularExpression(@"^\d*\.?\d{0,2}$", ErrorMessage = "globalTwoDecimalRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Range(0, 99999999.99, ErrorMessage = "globalTwoDecimalAfterTotalTenRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        public decimal? Duration { get; set; }
        public string Description { get; set; }      

        [Display(Name = "spnCraft|" + LocalizeResourceSetConstants.Global)]
        [Required(ErrorMessage = "validationSelectCraft|" + LocalizeResourceSetConstants.Global)]      
        public long CraftId { get; set; }
        [Display(Name = "spnQuantity|" + LocalizeResourceSetConstants.Global)]
        [Required(ErrorMessage = "validationQuantity|" + LocalizeResourceSetConstants.Global)]
        [RegularExpression(@"^\d*\.?\d{0,2}$", ErrorMessage = "globalTwoDecimalRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Range(0, 99999999.99, ErrorMessage = "globalTwoDecimalAfterTotalTenRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        public decimal? Quantity { get; set; }
        public decimal? UnitCost { get; set; }
        public decimal? TotalCost { get; set; }
        public long CategoryId { get; set; }
        public IEnumerable<SelectListItem> CraftList { get; set; }
    }
}