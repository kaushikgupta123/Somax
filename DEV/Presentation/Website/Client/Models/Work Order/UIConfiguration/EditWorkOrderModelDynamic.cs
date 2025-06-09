using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Models.Work_Order.UIConfiguration
{
    public class EditWorkOrderModelDynamic
    {
        public EditWorkOrderModelDynamic()
        {

        }
        #region UDF columns
        public long WorkOrderUDFId { get; set; }
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
        public decimal Numeric1 { get; set; }
        public decimal Numeric2 { get; set; }
        public decimal Numeric3 { get; set; }
        public decimal Numeric4 { get; set; }
        public string Select1 { get; set; }
        public string Select2 { get; set; }
        public string Select3 { get; set; }
        public string Select4 { get; set; }
        #endregion
        #region Work Order table columns
        public long WorkOrderId { get; set; }
        public string ClientLookupId { get; set; }
        public DateTime? CompleteDate { get; set; }
        public string ActionCode { get; set; }
        public decimal ActualDuration { get; set; }
        public DateTime? ActualFinishDate { get; set; }
        public long? ChargeToId { get; set; }
        public string ChargeTo_Name { get; set; }
        public bool CompleteAllTasks { get; set; }
        public string Description { get; set; }
        public bool DownRequired { get; set; }
        public string FailureCode { get; set; }
        public long? Labor_AccountId { get; set; }
        public string Location { get; set; }
        public string Priority { get; set; }
        public long? ProjectId { get; set; }
        public string ReasonNotDone { get; set; }
        public DateTime? RequiredDate { get; set; }
        public decimal ScheduledDuration { get; set; }
        public DateTime? ScheduledStartDate { get; set; }
        public string Shift { get; set; }
        public long? SourceId { get; set; }
        public string SourceType { get; set; }
        public string Status { get; set; }
        public string Type { get; set; }
        public int? PartsOnOrder { get; set; }
        public string RequestorName { get; set; }
        public string RequestorPhoneNumber { get; set; }
        public string RequestorEmail { get; set; }
        public DateTime? CreateDate { get; set; }
        public string ReasonforDown { get; set; }
        public string RootCauseCode { get; set; }
        public long? Planner_PersonnelId { get; set; }
        #endregion
        public string PlannerClientLookupId { get; set; }
        public string ChargeToClientLookupId { get; set; }
        public string AccountClientLookupId { get; set; }
        public string SourceIdClientLookupId { get; set; }
        public string CreateBy_PersonnelName { get; set; }
        public string CompleteBy_PersonnelName { get; set; }
        public string ProjectClientLookupId { get; set; } //V2-782
    }

}