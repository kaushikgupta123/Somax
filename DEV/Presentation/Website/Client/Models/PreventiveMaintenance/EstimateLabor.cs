using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Common.Constants;
namespace Client.Models.PreventiveMaintenance
{
    public class EstimateLabor
    {
        public long EstimatedCostsId { get; set; }
        public long PrevMaintMasterId { get; set; }
        public string PrevmaintClientlookUp { get; set; }        
        [Display(Name = "spnJobDuration|" + LocalizeResourceSetConstants.PrevMaintDetails)]
        [RegularExpression(@"^\d*\.?\d{0,5}$", ErrorMessage = "globalTwoDecimalRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Range(0, 999999.99, ErrorMessage = "globalTwoDecimalAfterTotalEightRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        public decimal? Duration { get; set; }
        [Display(Name = "spnCraft|" + LocalizeResourceSetConstants.Global)]
        [Required(ErrorMessage = "validationSelectCraft|" + LocalizeResourceSetConstants.Global)]       
        public long CraftId { get; set; }
        [Display(Name = "spnQuantity|" + LocalizeResourceSetConstants.Global)]
        [RegularExpression(@"^\d*\.?\d{0,2}$", ErrorMessage = "globalTwoDecimalRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Range(0, 99999999.99, ErrorMessage = "globalTwoDecimalAfterEightDecimalBeforeErrMsg|" + LocalizeResourceSetConstants.Global)]
        public decimal? Quantity { get; set; }
        public IEnumerable<SelectListItem> CraftList { get; set; }
    }
}