using Common.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Client.Models.FleetService
{
    public class FleetServiceModel
    {
        [Display(Name = "spnServiceOrderID|" + LocalizeResourceSetConstants.FleetServiceOrder)]
        public long ServiceOrderId { get; set; }
        public string ServiceOrderClientLookupId { get; set; }

        [Display(Name = "spnGlobalEquipmentId|" + LocalizeResourceSetConstants.Global)]
        public string ClientLookupId { get; set; }
        [Display(Name = "spnGlobalEquipmentId|" + LocalizeResourceSetConstants.Global)]
        [Required(ErrorMessage = "SelectEquipmentIDErrorMessage|" + LocalizeResourceSetConstants.Global)]
        [StringLength(31, ErrorMessage = "EquipmentStrLenErrorMessage|" + LocalizeResourceSetConstants.EquipmentDetails)]
        [RegularExpression("^[A-Z0-9\\%\\-\\:\\/\\$\\*\\+\\.]+$", ErrorMessage = "EquipmentIDRegErrMsg|" + LocalizeResourceSetConstants.EquipmentDetails)]
        public string EquipmentClientLookupId { get; set; }
        [Display(Name = "spnName|" + LocalizeResourceSetConstants.Global)]
        public string AssetName { get; set; }
        [Display(Name = "GlobalStatus|" + LocalizeResourceSetConstants.Global)]
        public string Status { get; set; }
        [Display(Name = "GlobalType|" + LocalizeResourceSetConstants.Global)]
        public string Type { get; set; }
        public DateTime? CreateDate { get; set; }
        [Display(Name = "spnGlobalSchedule|" + LocalizeResourceSetConstants.Global)]
        public DateTime? ScheduleDate { get; set; }
        [Display(Name = "GlobalComplete|" + LocalizeResourceSetConstants.Global)]
        public DateTime? CompleteDate { get; set; }
        public string Assigned { get; set; }
        public int ChildCount { get; set; }
        public int TotalCount { get; set; }

        public IEnumerable<SelectListItem> ScheduleWorkList { get; set; }
        [DefaultValue("add")]
        public string Pagetype { get; set; }
        [Display(Name = "spnDescription|" + LocalizeResourceSetConstants.Global)]
        public string Description { get; set; }
        public List<string> ShiftIds { get; set; }
        public IEnumerable<SelectListItem> DateRangeDropListForSOCreatedate { get; set; }
        public IEnumerable<SelectListItem> DateRangeDropListForSO { get; set; }
        public List<string> Typelist { get; set; }        
     

        
        public long Assign_PersonnelId { get; set; }
        public string Meter1Type { get; set; }
        public decimal Meter1CurrentReading { get; set; }
        public string Meter2Type { get; set; }
        public decimal Meter2CurrentReading { get; set; }
        public long CompleteBy_PersonnelId { get; set; }
        [Display(Name = "spnCompleteBy|" + LocalizeResourceSetConstants.FleetServiceOrder)]
        public string CompletedByPersonnels { get; set; }
        [Display(Name = "globalCancel|" + LocalizeResourceSetConstants.Global)]
        public DateTime? CancelDate { get; set; }
        public long CancelBy_PersonnelId { get; set; }
        [Display(Name = "spnCancelBy|" + LocalizeResourceSetConstants.FleetServiceOrder)]
        public string CancelledByPersonnels { get; set; }
        [Display(Name = "spnCancelReason|" + LocalizeResourceSetConstants.Global)]
        public string CancelReason { get; set; }
        [Display(Name = "spnShift|" + LocalizeResourceSetConstants.Global)]
        public string Shift { get; set; }
        
        public long EquipmentId { get; set; }
        public DateTime? Meter1CurrentReadingDate { get; set; }
        public DateTime? Meter2CurrentReadingDate { get; set; }
        public string Meter1Units { get; set; }
        public string Meter2Units { get; set; }
        public decimal LaborTotal { get; set; }
        public decimal PartTotal { get; set; }
        public decimal OtherTotal { get; set; }
        public decimal GrandTotal { get; set; }
        public string ShiftDesc { get; set; }
        public string TypeDesc { get; set; }

        public bool ClientOnPremise { get; set; }
    }
}