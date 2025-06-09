using Common.Constants;
using System;
using System.ComponentModel.DataAnnotations;

namespace Client.Models.Work_Order
{
    public class DowntimeModel
    {
       
        [Required(ErrorMessage = "ValidationDowndate|" + LocalizeResourceSetConstants.WorkOrderDetails)]       
        [Display(Name = "spnDownDate|" + LocalizeResourceSetConstants.WorkOrderDetails)]
        public DateTime? Downdate { get; set; }
        [Required(ErrorMessage = "MinutesDownErrorMessage|" + LocalizeResourceSetConstants.EquipmentDetails)]
        [RegularExpression(@"^\d*\.?\d{0,4}$", ErrorMessage = "globalFourDecimalRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Range(0.0001, 99999999999.9999, ErrorMessage = "globalFourDecimalAfterTotalFifteenRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        public decimal? Minutes { get; set; }
        public long WorkOrderId { get; set; }
    }
}