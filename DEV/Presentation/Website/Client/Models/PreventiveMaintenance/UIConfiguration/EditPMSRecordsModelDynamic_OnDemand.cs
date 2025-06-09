using Common.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Client.Models.PreventiveMaintenance.UIConfiguration
{
    public class EditPMSRecordsModelDynamic_OnDemand
    {
        public EditPMSRecordsModelDynamic_OnDemand()
        {

        }

        #region UDF columns
        public long PMSchedUDFId { get; set; }
        public string Text1 { get; set; }
        public string Text2 { get; set; }
        public string Text3 { get; set; }
        public string Text4 { get; set; }
        public DateTime? Date1 { get; set; }
        public DateTime? Date2 { get; set; }
        public DateTime? Date3 { get; set; }
        public DateTime? Date4 { get; set; }
        public bool Bit1 { get; set; }
        public bool Bit2 { get; set; }
        public bool Bit3 { get; set; }
        public bool Bit4 { get; set; }
        public decimal? Numeric1 { get; set; }
        public decimal? Numeric2 { get; set; }
        public decimal? Numeric3 { get; set; }
        public decimal? Numeric4 { get; set; }
        public string Select1 { get; set; }
        public string Select2 { get; set; }
        public string Select3 { get; set; }
        public string Select4 { get; set; }
        #endregion

        #region Preventive Maintenance table columns
        public long PrevMaintSchedId { get; set; }
        public long? PrevMaintMasterId { get; set; }
        public long? AssignedTo_PersonnelId { get; set; }
        public string AssociationGroup { get; set; }
        public int? CalendarSlack { get; set; }
        public long? ChargeToId { get; set; }
        public string ChargeType { get; set; }
        public string Crew { get; set; }
        public DateTime? CurrentWOComplete { get; set; }
        public bool DownRequired { get; set; }
        public string ExcludeDOW { get; set; }
        public int? Frequency { get; set; }
        public string FrequencyType { get; set; }
        public bool InactiveFlag { get; set; }
        public string JobPlan { get; set; }
        public long? Last_WorkOrderId { get; set; }
        public DateTime? LastPerformed { get; set; }
        public DateTime? LastScheduled { get; set; }
        public decimal? MeterHighLevel { get; set; }
        public long? MeterId { get; set; }
        public decimal? MeterInterval { get; set; }
        public decimal? MeterLastDone { get; set; }
        public decimal? MeterLastDue { get; set; }
        public decimal? MeterLastReading { get; set; }
        public decimal? MeterLowLevel { get; set; }
        public string MeterMethod { get; set; }
        public bool MeterOn { get; set; }
        public decimal? MeterSlack { get; set; }
        public DateTime? NextDueDate { get; set; }
        public string OnDemandGroup { get; set; }
        public string Priority { get; set; }
        public bool Scheduled { get; set; }
        public string ScheduleMethod { get; set; }
        public string ScheduleType { get; set; }
        public string ScheduleType_ { get; set; }
        public string ScheduleWeeks { get; set; }
        public string Section { get; set; }
        public string Shift { get; set; }
        public string Type { get; set; }
        public int? RIMEWorkClass { get; set; }
        public string Category { get; set; }
        public string GaugeEventType { get; set; }
        public string RootCauseCode { get; set; }
        public string ActionCode { get; set; }
        public string FailureCode { get; set; }
        public long? Planner_PersonnelId { get; set; }
        public bool PlanningRequired { get; set; } //V2-1161
        public int? NumbersOfPMSchedAssignRecords { get; set; } //V2-1161
        public int? UpdateIndex { get; set; }
        #endregion
        #region Others
        public string ChargeToName { get; set; }
        public string ChargeToClientLookupId { get; set; }
        public string Planner_ClientLookupId { get; set; }
        public string PrevmaintClientlookUp { get; set; }
        public long? PrevMaintLibraryID { get; set; }
        #endregion
    }
}