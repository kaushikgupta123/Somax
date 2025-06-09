using Client.CustomValidation;
using Common.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Client.Models
{
    public class ScheduleRecords
    {
        public long PrevMaintScheId { get; set; }
        public string PrevmaintClientlookUp { get; set; }

        [Display(Name = "GlobalChargeTo|" + LocalizeResourceSetConstants.Global)]
        [Required(ErrorMessage = "validationChargeTo|" + LocalizeResourceSetConstants.Global)]
        public string ChargeToClientLookupId { get; set; }

        [Display(Name = "spnChargeToName|" + LocalizeResourceSetConstants.Global)]
        public string ChargeToName { get; set; }
        public string AssignedTo_PersonnelClientLookupId { get; set; }
        public string Meter_ClientLookupId { get; set; }
        public bool Security { get; set; }
        public long PrevMaintMasterId { get; set; }
        public long PrevMaintLibraryID { get; set; }

        [Display(Name = "spnAssignedTo|" + LocalizeResourceSetConstants.Global)]
        public long? AssignedTo_PersonnelId { get; set; }
        public string AssociationGroup { get; set; }

        [Display(Name = "spnCalendarSlack|" + LocalizeResourceSetConstants.PrevMaintDetails)]
        [Range(0, int.MaxValue, ErrorMessage = "globalIntMaxValueRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        public int? CalendarSlack { get; set; }
        public long ChargeToId { get; set; }

        [Display(Name = "spnChargeType|" + LocalizeResourceSetConstants.Global)]
        [Required(ErrorMessage = "validationChargeType|" + LocalizeResourceSetConstants.Global)]
        public string ChargeType { get; set; }
        public string Crew { get; set; }
        [Display(Name = "GlobalComplete|" + LocalizeResourceSetConstants.Global)]
        public DateTime? CurrentWOComplete { get; set; }

        [Display(Name = "spnDownRequired|" + LocalizeResourceSetConstants.Global)]
        public bool DownRequired { get; set; }
        public string ExcludeDOW { get; set; }

        [RequiredIf("ScheduleType", "Calendar", ErrorMessage = "validFequencyMsg|" + LocalizeResourceSetConstants.PrevMaintDetails)]
        [Range(0, int.MaxValue, ErrorMessage = "globalIntMaxValueRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Display(Name = "spnPerformEvery|" + LocalizeResourceSetConstants.PrevMaintDetails)]
        public int? Frequency { get; set; }

        [RequiredIf("ScheduleType", "Calendar", ErrorMessage = "validFequencyTypeMsg|" + LocalizeResourceSetConstants.PrevMaintDetails)]
        [Display(Name = "spnFrequencyType|" + LocalizeResourceSetConstants.Global)]
        public string FrequencyType { get; set; }

        [Display(Name = "globalInActive|" + LocalizeResourceSetConstants.Global)]
        public bool InactiveFlag { get; set; }
        public string JobPlan { get; set; }
        public long Last_WorkOrderId { get; set; }

        [Display(Name = "spnLastPerformed|" + LocalizeResourceSetConstants.Global)]
        public DateTime? LastPerformed { get; set; }

        [Display(Name = "spnLastScheduled|" + LocalizeResourceSetConstants.Global)]
        public DateTime? LastScheduled { get; set; }
        public decimal MeterHighLevel { get; set; }

        [Display(Name = "Meter|" + LocalizeResourceSetConstants.Global)]
        [RequiredIf("ScheduleType", "Meter", ErrorMessage = "validMeterMsg|" + LocalizeResourceSetConstants.PrevMaintDetails)]
        public long MeterId { get; set; }

        [Display(Name = "spnMeterInterval|" + LocalizeResourceSetConstants.PrevMaintDetails)]
        [RequiredIf("ScheduleType", "Meter", ErrorMessage = "validmeterinterval|" + LocalizeResourceSetConstants.PrevMaintDetails)]
        [RegularExpression(@"^\d*\.?\d{0,3}$", ErrorMessage = "globalThreeDecimalRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Range(0, 99999999999999.999, ErrorMessage = "globalThreeDecimalAfterTotalSeventeenRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        public decimal? MeterInterval { get; set; }

        [Display(Name = "spnMeterLastDone|" + LocalizeResourceSetConstants.PrevMaintDetails)]
        [RegularExpression(@"^\d*\.?\d{0,3}$", ErrorMessage = "globalThreeDecimalRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Range(0, 99999999999999.999, ErrorMessage = "globalThreeDecimalAfterTotalSeventeenRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        public decimal? MeterLastDone { get; set; }

        [Display(Name = "spnMeterLastDue|" + LocalizeResourceSetConstants.PrevMaintDetails)]
        [RequiredIf("ScheduleType", "Meter", ErrorMessage = "validMeterLastDue|" + LocalizeResourceSetConstants.PrevMaintDetails)]
        [RegularExpression(@"^\d*\.?\d{0,3}$", ErrorMessage = "globalThreeDecimalRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Range(0, 99999999999999.999, ErrorMessage = "globalThreeDecimalAfterTotalSeventeenRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        public decimal? MeterLastDue { get; set; }

        [RegularExpression(@"^\d*\.?\d{0,3}$", ErrorMessage = "globalThreeDecimalRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Range(0, 99999999999999.999, ErrorMessage = "globalThreeDecimalAfterTotalSeventeenRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        public decimal MeterLastReading { get; set; }

        [RegularExpression(@"^\d*\.?\d{0,3}$", ErrorMessage = "globalThreeDecimalRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Range(0, 99999999999999.999, ErrorMessage = "globalThreeDecimalAfterTotalSeventeenRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        public decimal? MeterLowLevel { get; set; }

        [Display(Name = "spnMeterMethod|" + LocalizeResourceSetConstants.PrevMaintDetails)]
        public string MeterMethod { get; set; }
        public bool MeterOn { get; set; }

        [Display(Name = "spnMeterSlack|" + LocalizeResourceSetConstants.PrevMaintDetails)]
        [RegularExpression(@"^\d*\.?\d{0,3}$",  ErrorMessage = "globalThreeDecimalRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Range(0, 99999999999999.999, ErrorMessage = "globalThreeDecimalAfterTotalSeventeenRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        public decimal? MeterSlack { get; set; }

        [Display(Name = "spnNextDue|" + LocalizeResourceSetConstants.Global)]
        [RequiredIf("ScheduleType", "Calendar",ErrorMessage = "validNextDueMsg|" + LocalizeResourceSetConstants.PrevMaintDetails)]
        public DateTime? NextDueDate { get; set; }
        [RequiredIf("ScheduleType", "OnDemand", ErrorMessage = "validOnDemandMsg|" + LocalizeResourceSetConstants.PrevMaintDetails)]
        public string OnDemandGroup { get; set; }

        [Display(Name = "spnPriority|" + LocalizeResourceSetConstants.Global)]
        public string Priority { get; set; }

        [Display(Name = "spnScheduled|" + LocalizeResourceSetConstants.Global)]
        public bool Scheduled { get; set; }        
        [RequiredIf("ScheduleType", "Calendar", ErrorMessage = "spnSelectSchedulemethod|" + LocalizeResourceSetConstants.PrevMaintDetails)]
        [Display(Name = "spnScheduleMethod|" + LocalizeResourceSetConstants.Global)]
        public string ScheduleMethod { get; set; }
        public string ScheduleType { get; set; }
        public string ScheduleType_ { get; set; }
        public string ScheduleWeeks { get; set; }
        public string Section { get; set; }

        [Display(Name = "spnShift|" + LocalizeResourceSetConstants.Global)]
        public string Shift { get; set; }

        [Display(Name = "GlobalType|" + LocalizeResourceSetConstants.Global)]
        public string Type { get; set; }
        public int RIMEWorkClass { get; set; }
        public string GaugeEventType { get; set; }
        public string Category { get; set; }
        public int UpdateIndex { get; set; }

        [Display(Name = "GlobalWorkOrder|" + LocalizeResourceSetConstants.Global)]
        public string WorkOrder_ClientLookupId { get; set; }
        public string BusinessType { get; set; }
        public IEnumerable<SelectListItem> OnDemandList { get; set; }
        public IEnumerable<SelectListItem> MeterLookUpList { get; set; }
        public IEnumerable<SelectListItem> WorkAssignedLookUpList { get; set; }
        public IEnumerable<SelectListItem> ScheduledTypeList { get; set; }
        public IEnumerable<SelectListItem> ScheduledSectionList { get; set; }
        public IEnumerable<SelectListItem> ScheduledPriorityList { get; set; }
        public IEnumerable<SelectListItem> ScheduledCategoryList { get; set; }
        public IEnumerable<SelectListItem> ScheduledShiftList { get; set; }
        public IEnumerable<SelectListItem> ChargeTypeList { get; set; }
        public IEnumerable<SelectListItem> ScheduleMethodList { get; set; }
        public IEnumerable<SelectListItem> FrequencyTypeList { get; set; }
        public IEnumerable<SelectListItem> MeterMethodList { get; set; }
        public IEnumerable<SelectListItem> ChargeTypelookUpList { get; set; }
        public bool ChargeToIdStatus { get; set; }
        //For PDF Print V2-355
        public string NextDueDateString { get; set; }
        public long? Planner_PersonnelId { get; set; }
        public IEnumerable<SelectListItem> PlannerPersonnelList { get; set; }
        public string FailureCode { get; set; }
        public IEnumerable<SelectListItem> FailureCodeList { get; set; }
        public string ActionCode { get; set; }
        public IEnumerable<SelectListItem> ActionCodeList { get; set; }
        public string RootCauseCode { get; set; }
        public IEnumerable<SelectListItem> RootCauseCodeList { get; set; }
        public string[] ExclusionDaysString { get; set; }
        public string ExclusionDaysStringHdn { get; set; }
        public IEnumerable<SelectListItem> DaysOfWeekList { get; set; }

        public bool Sunday { get; set; }
        public bool Monday { get; set; }
        public bool Tuesday { get; set; }
        public bool Wednesday { get; set; }
        public bool Thursday { get; set; }
        public bool Friday { get; set; }
        public bool Saturday { get; set; }
        public int ChildCount { get; set; } //V2-712
        public bool PlanningRequired { get; set; } //V2-1161
        public int TotalCount { get; set; } //V2-1212
    }
}