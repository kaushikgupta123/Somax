using Common.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Client.Models.PreventiveMaintenance
{
    public class PreventiveMaintenanceModel
    {
        #region Identification
        public long ClientId { get; set; }
        public long PrevMaintMasterId { get; set; }
        [Required(ErrorMessage = "validPrevMaintMasterId|" + LocalizeResourceSetConstants.PrevMaintDetails)]
        [RegularExpression("^[A-Z0-9\\%\\-\\:\\/\\$\\*\\+\\.]+$", ErrorMessage = "PrevMaintMasterIdRegErrMsg|" + LocalizeResourceSetConstants.PrevMaintDetails)]
        [Display(Name = "spnPrevMstrId|" + LocalizeResourceSetConstants.PrevMaintDetails)]
        public string ClientLookupId { get; set; }
        [Display(Name = "spnDescription|" + LocalizeResourceSetConstants.Global)]
        [Required(ErrorMessage = "validationDescription|" + LocalizeResourceSetConstants.Global)]
        public string Description { get; set; }
        [Display(Name = "GlobalType|" + LocalizeResourceSetConstants.Global)]
        public string Type { get; set; }
        [Display(Name = "spnSchdType|" + LocalizeResourceSetConstants.PrevMaintDetails)]
        [Required(ErrorMessage = "validscheduletype|" + LocalizeResourceSetConstants.PrevMaintDetails)]
        public string ScheduleType { get; set; }
        [Display(Name = "spnJobDuration|" + LocalizeResourceSetConstants.PrevMaintDetails)]
        [Required(ErrorMessage = "validationJobDuration|" + LocalizeResourceSetConstants.PrevMaintDetails)]
        [RegularExpression(@"^\d*\.?\d{0,2}$", ErrorMessage = "globalTwoDecimalRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Range(0, 999999.99, ErrorMessage = "globalTwoDecimalAfterTotalEightRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        public decimal? JobDuration { get; set; }
        [Display(Name = "globalInActive|" + LocalizeResourceSetConstants.Global)]
        public bool InactiveFlag { get; set; }
        public string Status { get; set; }
        public string TaskNumber { get; set; }
        public long EstimatedCostsId { get; set; }
       
        #endregion
        public IEnumerable<SelectListItem> ScheduleTypeList { get; set; }
        public IEnumerable<SelectListItem> TypeList { get; set; }
        
        [Display(Name = "spnChargeType|" + LocalizeResourceSetConstants.Global)]
        public string ChargeType { get; set; }
        [Display(Name = "GlobalChargeTo|" + LocalizeResourceSetConstants.Global)]
        public string ChargeTo { get; set; }
        [Display(Name = "spnChargeToName|" + LocalizeResourceSetConstants.Global)]
        public string ChargeToName { get; set; }
        [Display(Name = "spnLastPerformed|" + LocalizeResourceSetConstants.Global)]
        public string LastPerformed { get; set; }
        [Display(Name = "spnLastScheduled|" + LocalizeResourceSetConstants.Global)]
        public string LastScheduled { get; set; }
        [Display(Name = "GlobalWorkOrder|" + LocalizeResourceSetConstants.Global)]
        public string WorkOrder { get; set; }
        [Display(Name = "spnScheduleMethod|" + LocalizeResourceSetConstants.Global)]
        public string ScheduleMethod { get; set; }
        [Display(Name = "spnFrequencyType|" + LocalizeResourceSetConstants.Global)]
        public string FrequencyType { get; set; }
        [Display(Name = "spnPerformEvery|" + LocalizeResourceSetConstants.PrevMaintDetails)]
        public string PerformEvery { get; set; }
        [Display(Name = "GlobalComplete|" + LocalizeResourceSetConstants.Global)]
        public string Complete { get; set; }
        [Display(Name = "spnNextDue|" + LocalizeResourceSetConstants.Global)]
        public string NextDue { get; set; }
        [Display(Name = "spnCalendarSlack|" + LocalizeResourceSetConstants.PrevMaintDetails)]
        public string CalendarSlack { get; set; }
        [Display(Name = "Meter|" + LocalizeResourceSetConstants.Global)]
        public string Meter { get; set; }
        [Display(Name = "spnMeterInterval|" + LocalizeResourceSetConstants.PrevMaintDetails)]
        public string MeterInterval { get; set; }
        [Display(Name = "spnMeterLastDue|" + LocalizeResourceSetConstants.PrevMaintDetails)]
        public string MeterLastDue { get; set; }
        [Display(Name = "spnMeterSlack|" + LocalizeResourceSetConstants.PrevMaintDetails)]
        public string MeterSlack { get; set; }
        [Display(Name = "spnMeterMethod|" + LocalizeResourceSetConstants.PrevMaintDetails)]
        public string MeterMethod { get; set; }
        [Display(Name = "spnMeterLastDone|" + LocalizeResourceSetConstants.PrevMaintDetails)]
        public string MeterLastDone { get; set; }
        [Display(Name = "spnDownRequired|" + LocalizeResourceSetConstants.Global)]
        public string DownRequired { get; set; }
        [Display(Name = "spnScheduled|" + LocalizeResourceSetConstants.Global)]
        public string Scheduled { get; set; }
        [Display(Name = "spnAssignedTo|" + LocalizeResourceSetConstants.Global)]
        public string AssignedTo { get; set; }
        [Display(Name = "spnPriority|" + LocalizeResourceSetConstants.Global)]
        public string Priority { get; set; }
        [Display(Name = "GlobalType|" + LocalizeResourceSetConstants.Global)]
        public string SType { get; set; }
        [Display(Name = "spnShift|" + LocalizeResourceSetConstants.Global)]
        public string Shift { get; set; }
        public long PrevMaintLibraryId { get; set; }
        public int UpdateIndex { get; set; }
        public int ChildCount { get; set; }
        public int TotalCount { get; set; }

        //V2-1204
        public bool Copy_EstimatedCost { get; set; }
        public bool Copy_Tasks { get; set; }
        public bool Copy_Notes { get; set; }
    }
}