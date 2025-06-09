using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace SOMAX.G4.Data.INTDataLayer.EL
{
    public class WorkOrderEL
    {
        

        public long ClientId{ set; get; }
        public long WorkOrderId{ set; get; }
        public long SiteId { set; get; }
        public long AreaId{ set; get; }
        public long DepartmentId{ set; get; }
        public long StoreroomId{ set; get; }
        public string ClientLookupId{ set; get; }
        public string ActionCode{ set; get; }
        public decimal ActualDuration{ set; get; }
        public DateTime? ActualFinishDate{ set; get; }
        public decimal ActualLaborCosts{ set; get; }
        public decimal ActualLaborHours{ set; get; }
        public decimal ActualMaterialCosts{ set; get; }
        public decimal ActualOutsideServiceCosts{ set; get; }
        public DateTime? ActualStartDate{ set; get; }
        public decimal ActualTotalCosts{ set; get; }
        public bool ApprovalRequired{ set; get; }
        public long ApproveByPersonnelId{ set; get; }
        public DateTime? ApproveDate{ set; get; }
        public long ChargeToId{ set; get; }
        public string ChargeType{ set; get; }
        public string ChargeTo_Name{ set; get; }
        public long CloseBy_PersonnelId{ set; get; }
        public DateTime? CloseDate{ set; get; }
        public bool CompleteAllTasks{ set; get; }
        public long CompleteBy_PersonnelId{ set; get; }
        public DateTime? CompleteDate{ set; get; }
        public long Creator_PersonnelId{ set; get; }
        public string Crew{ set; get; }
        public string Description{ set; get; }
        public bool DownRequired{ set; get; }
        public bool EquipDown{ set; get; }
        public DateTime? EquipDownDate{ set; get; }
        public DateTime? EquipUpDate{ set; get; }
        public decimal EstimatedLaborCosts{ set; get; }
        public decimal EstimatedLaborHours{ set; get; }
        public decimal EstimatedMaterialCosts{ set; get; }
        public decimal EstimatedOutsideServiceCosts{ set; get; }
        public decimal EstimatedPurchaseMaterialCosts{ set; get; }
        public decimal EstimatedTotalCosts{ set; get; }
        public string FailureCode{ set; get; }
        public string JobPlan{ set; get; }
        public long Labor_AccountId{ set; get; }
        public string Location{ set; get; }
        public long Material_AccountId{ set; get; }
        public long MeterId{ set; get; }
        public decimal MeterReadingDone{ set; get; }
        public decimal MeterReadingDue{ set; get; }
        public long Planner_PersonnelId{ set; get; }
        public long PrevMaintBatchId{ set; get; }
        public int PrimaveraProjectNumber{ set; get; }
        public int Printed{ set; get; }
        public string Priority{ set; get; }
        public long ProjectId{ set; get; }
        public string ReasonforDown{ set; get; }
        public string ReasonNotDone{ set; get; }
        public long ReleaseBy_PersonnelId{ set; get; }
        public DateTime? ReleaseDate{ set; get; }
        public DateTime? RequestDate{ set; get; }
        public long Requestor_PersonnelId{ set; get; }
        public string RequestorLocation{ set; get; }
        public string RequestorPhone{ set; get; }
        public DateTime? RequiredDate{ set; get; }
        public int RIMEAssetCriticality{ set; get; }
        public int RIMEPriority{ set; get; }
        public int RIMEWorkClass{ set; get; }
        public decimal ScheduledDuration{ set; get; }
        public DateTime? ScheduledFinishDate{ set; get; }
        public DateTime? ScheduledStartDate{ set; get; }
        public long Scheduler_PersonnelId{ set; get; }
        public string Section{ set; get; }
        public string Shift{ set; get; }
        public DateTime? SignOffDate{ set; get; }
        public long SignoffBy_PersonnelId{ set; get; }
        public long SourceId{ set; get; }
        public string SourceType{ set; get; }
        public int Status{ set; get; }
        public decimal SuspendDuration{ set; get; }
        public string Type{ set; get; }
        public long WorkAssigned_PersonnelId{ set; get; }
        public int UpdateIndex{ set; get; }
       

        
       
    }
}
