using Common.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Client.Models
{
   
    public class DownTimeModel
    {
        public DownTimeModel()
        {
            _EquipmentSummaryModel = new EquipmentSummaryModel();
        }
        public EquipmentSummaryModel _EquipmentSummaryModel { get; set; }
        public string PersonalId { get; set; }
        public long EquipmentId { get; set; }
        [Display(Name = "GlobalWorkOrder|" + LocalizeResourceSetConstants.Global)]
        public string WorkOrderClientLookupId { get; set; }
        [Required(ErrorMessage = "DateDownErrorMessage|" + LocalizeResourceSetConstants.EquipmentDetails)]
        [Display(Name = "GlobalDate|" + LocalizeResourceSetConstants.Global)]
        public DateTime? DateDown { get; set; }
        [Required(ErrorMessage = "MinutesDownErrorMessage|" + LocalizeResourceSetConstants.EquipmentDetails)]
        [Display(Name = "globalMinutes|" + LocalizeResourceSetConstants.Global)]
        [RegularExpression(@"^\d*\.?\d{0,4}$", ErrorMessage = "globalFourDecimalRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Range(0.0001, 99999999999.9999, ErrorMessage = "globalFourDecimalAfterTotalFifteenRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        public decimal? MinutesDown { get; set; }
        public long DowntimeId { get; set; }
        public bool DowntimeCreateSecurity { get; set; }
        public bool DowntimeEditSecurity { get; set; }
        public bool DowntimeDeleteSecurity { get; set; }
        [Display(Name = "spnPersonnelId|" + LocalizeResourceSetConstants.EquipmentDetails)]
        public string PersonnelClientLookupId { get; set; }
        public string PersonnelNameFull { get; set; }

        public List<string> ErrorMessage { get; set; }

        public IEnumerable<SelectListItem> PersonnelList { get; set; }
        public IEnumerable<SelectListItem> WorkOrderList { get; set; }
        [Required(ErrorMessage = "ReasonErrMsg|" + LocalizeResourceSetConstants.EquipmentDetails)]
        public string ReasonForDown { get; set; }  //V2-695
        public string ReasonForDownDescription { get; set; }  //V2-695
        public int TotalCount { get; set; }//V2-695 wo

        public decimal TotalMinutesDown { get; set; }//V2-695 wo

    }
}