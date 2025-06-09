using Common.Constants;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
//namespace Client.Models.Work_Order
namespace Client.Models
{
    public class WorkOrderDowntimeModel
    {
        [Required(ErrorMessage = "ValidationDowndate|" + LocalizeResourceSetConstants.WorkOrderDetails)]
        [Display(Name = "spnDownDate|" + LocalizeResourceSetConstants.WorkOrderDetails)]
        public DateTime? Downdate { get; set; }
        [Required(ErrorMessage = "MinutesDownErrorMessage|" + LocalizeResourceSetConstants.EquipmentDetails)]
        [RegularExpression(@"^\d*\.?\d{0,4}$", ErrorMessage = "globalFourDecimalRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Range(0.0001, 99999999999.9999, ErrorMessage = "globalFourDecimalAfterTotalFifteenRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        public decimal? MinutesDown { get; set; }
        public long WorkOrderId { get; set; }
        public long? ChargeToId { get; set; }
        public long DowntimeId { get; set; }
        [Required(ErrorMessage = "ReasonErrMsg|" + LocalizeResourceSetConstants.EquipmentDetails)]
        public string ReasonForDown { get; set; }
        public string ReasonForDownDescription { get; set; }
        public bool DowntimeCreateSecurity { get; set; }
        public bool DowntimeEditSecurity { get; set; }
        public bool DowntimeDeleteSecurity { get; set; }
        public string WorkOrderClientLookupId { get; set; }
        public IEnumerable<SelectListItem> ReasonForDownList { get; set; }
        public int TotalCount { get; set; }   
        public decimal TotalMinutesDown { get; set; }  

    }
}