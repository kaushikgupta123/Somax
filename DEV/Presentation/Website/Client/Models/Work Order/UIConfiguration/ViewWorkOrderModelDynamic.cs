using System;

namespace Client.Models.Work_Order.UIConfiguration
{
    public class ViewWorkOrderModelDynamic
    {
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
        public string ActionCode { get; set; }
        public decimal ActualDuration { get; set; }
        public DateTime? ActualFinishDate { get; set; }
        //public decimal ActualLaborCosts { get; set; }
        //public decimal ActualLaborHours { get; set; }
        //public decimal ActualMaterialCosts { get; set; }
        //public decimal ActualOutsideServiceCosts { get; set; }
        //public DateTime? ActualStartDate { get; set; }
        //public decimal ActualTotalCosts { get; set; }
        //public bool ApprovalRequired { get; set; }
        //public long? ApproveBy_PersonnelId { get; set; }
        //public DateTime? ApproveDate { get; set; }
        //public string CancelReason { get; set; }
        public long? ChargeToId { get; set; }
        //public string ChargeType { get; set; }
        public string ChargeTo_Name { get; set; }
        //public long? CloseBy_PersonnelId { get; set; }
        //public DateTime? CloseDate { get; set; }
        public bool CompleteAllTasks { get; set; }
        //public long? CompleteBy_PersonnelId { get; set; }
        //public string CompleteComments { get; set; }
        //public DateTime? CompleteDate { get; set; }
        //public long? Creator_PersonnelId { get; set; }
        //public string Crew { get; set; }
        public string Description { get; set; }
        public bool DownRequired { get; set; }
        //public bool EquipDown { get; set; }
        //public DateTime? EquipDownDate { get; set; }
        //public decimal EquipDownHours { get; set; }
        //public DateTime? EquipUpDate { get; set; }
        //public decimal EstimatedLaborCosts { get; set; }
        //public decimal EstimatedLaborHours { get; set; }
        //public decimal EstimatedMaterialCosts { get; set; }
        //public decimal EstimatedOutsideServiceCosts { get; set; }
        //public decimal EstimatedPurchaseMaterialCosts { get; set; }
        //public decimal EstimatedTotalCosts { get; set; }
        public string FailureCode { get; set; }
        //public string JobPlan { get; set; }
        public long? Labor_AccountId { get; set; }
        public string Location { get; set; }
        //public long? Material_AccountId { get; set; }
        //public long? MeterId { get; set; }
        //public decimal MeterReadingDone { get; set; }
        //public decimal MeterReadingDue { get; set; }
        //public long? Other_AccountId { get; set; }
        //public long? Planner_PersonnelId { get; set; }
        //public long? PrevMaintBatchId { get; set; }
        //public int? PrimaveraProjectNumber { get; set; }
        //public int? Printed { get; set; }
        public string Priority { get; set; }
        public long? ProjectId { get; set; }
        //public string ReasonforDown { get; set; }
        public string ReasonNotDone { get; set; }
        //public long? ReleaseBy_PersonnelId { get; set; }
        //public DateTime? ReleaseDate { get; set; }
        //public DateTime? RequestDate { get; set; }
        //public long? Requestor_PersonnelId { get; set; }
        //public string RequestorLocation { get; set; }
        //public string RequestorPhone { get; set; }
        public DateTime? RequiredDate { get; set; }
        //public int? RIMEAssetCriticality { get; set; }
        //public int? RIMEPriority { get; set; }
        //public int? RIMEWorkClass { get; set; }
        public decimal ScheduledDuration { get; set; }
        //public DateTime? ScheduledFinishDate { get; set; }
        public DateTime? ScheduledStartDate { get; set; }
        //public long? Scheduler_PersonnelId { get; set; }
        //public string Section { get; set; }
        public string Shift { get; set; }
        //public DateTime? SignOffDate { get; set; }
        //public long? SignoffBy_PersonnelId { get; set; }
        public long? SourceId { get; set; }
        public string SourceType { get; set; }
        public string Status { get; set; }
        //public decimal SuspendDuration { get; set; }
        public string Type { get; set; }
        //public long? WorkAssigned_PersonnelId { get; set; }
        //public string DeniedReason { get; set; }
        //public DateTime? DeniedDate { get; set; }
        //public long? DeniedBy_PersonnelId { get; set; }
        //public string DeniedComment { get; set; }
        //public bool EmergencyWorkOrder { get; set; }
        //public DateTime? CancelDate { get; set; }
        //public string Category { get; set; }
        public string RequestorName { get; set; }
        public string RequestorPhoneNumber { get; set; }
        public string RequestorEmail { get; set; }
        public DateTime? SchedInitDate { get; set; }
        //public int? PartsOnOrder { get; set; }
        //public int? UpdateIndex { get; set; }
        public string CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        //public string ModifyBy { get; set; }
        //public DateTime? ModifyDate { get; set; }
        public long? CompleteBy_PersonnelId { get; set; }
        public string ReasonforDown { get; set; }
        public string RootCauseCode { get; set; }
        public long? Planner_PersonnelId { get; set; }
        #endregion
        public string PlannerClientLookupId { get; set; }
        public string ChargeToClientLookupId { get; set; }
        public string Labor_AccountClientLookupId { get; set; }
        //public string Material_AccountClientLookupId { get; set; }
        //public string Other_AccountClientLookupId { get; set; }
        public string ProjectClientLookupId { get; set; }

        public string Select1ClientLookupId { get; set; }
        public string Select2ClientLookupId { get; set; }
        public string Select3ClientLookupId { get; set; }
        public string Select4ClientLookupId { get; set; }
        public string CreateBy_PersonnelName { get; set; }
        public string CompleteBy_PersonnelName { get; set; }
        public string SourceIdClientLookupId { get; set; }
    }

}

