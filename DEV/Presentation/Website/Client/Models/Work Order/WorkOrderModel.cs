using Client.Models.Common;
using Common.Constants;
using DataContracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Client.Models.WorkOrder
{
    public class WorkOrderModel
    {
        #region Workorder
        public WorkOrderModel()
        {
            WorkAssignedList = new List<DataTableDropdownModel>();
        }

        public long WorkOrderId { get; set; }

        [Display(Name = "GlobalWorkOrder|" + LocalizeResourceSetConstants.Global)]
        public string ClientLookupId { get; set; }

        [Display(Name = "spnStatus|" + LocalizeResourceSetConstants.Global)]
        public string Status { get; set; }

        [Display(Name = "spnShift|" + LocalizeResourceSetConstants.Global)]
        public string Shift { get; set; }
        [Display(Name = "GlobalType|" + LocalizeResourceSetConstants.Global)]
       
        public string Type { get; set; }

        [Display(Name = "spnDescription|" + LocalizeResourceSetConstants.Global)]
        [Required(ErrorMessage = "validationDescription|" + LocalizeResourceSetConstants.Global)]
        public string Description { get; set; }
        [Display(Name = "spnDownRequired|" + LocalizeResourceSetConstants.Global)]
        public bool DownRequired { get; set; }

        [Display(Name = "spnPriority|" + LocalizeResourceSetConstants.Global)]
        public string Priority { get; set; }
        [Display(Name = "spnAccount|" + LocalizeResourceSetConstants.Global)]
        public long? Labor_AccountId { get; set; }

        [Display(Name = "spnAssigned|" + LocalizeResourceSetConstants.Global)]
        public string Assigned { get; set; }
        public string AssignedFullName { get; set; }

        [Display(Name = "spnChargeType|" + LocalizeResourceSetConstants.Global)]
        
        public string ChargeType { get; set; }

        [Display(Name = "spnChargeToName|" + LocalizeResourceSetConstants.Global)]
        public string ChargeTo_Name { get; set; }

        public string AssetLocation { get; set; }

        [Display(Name = "spnRequired|" + LocalizeResourceSetConstants.Global)]
        public DateTime? RequiredDate { get; set; }
        [Display(Name = "spnCreatedBy|" + LocalizeResourceSetConstants.Global)]
        public string Createby { get; set; }

        [Display(Name = "globalCreateDate|" + LocalizeResourceSetConstants.Global)]
        public DateTime? CreateDate { get; set; }

        [Display(Name = "GlobalSourceType|" + LocalizeResourceSetConstants.Global)]
        public string SourceType { get; set; }

        [Display(Name = "spnScheduledDate|" + LocalizeResourceSetConstants.Global)]
        public DateTime? ScheduledStartDate { get; set; }

        [Display(Name = "spnScheduledDuration|" + LocalizeResourceSetConstants.Global)]
        [RegularExpression(@"^\d*\.?\d{0,2}$", ErrorMessage = "globalTwoDecimalRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Range(0, 999999.99, ErrorMessage = "globalTwoDecimalAfterTotalEightRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        public decimal? ScheduledDuration { get; set; }

        [Display(Name = "spnFailure|" + LocalizeResourceSetConstants.WorkOrderDetails)]
        public string FailureCode { get; set; }

        [Display(Name = "spnActualFinish|" + LocalizeResourceSetConstants.WorkOrderDetails)]
        public DateTime? ActualFinishDate { get; set; }

        [RegularExpression(@"^\d*\.?\d{0,2}$", ErrorMessage = "globalTwoDecimalRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Range(0, 999999.99, ErrorMessage = "globalTwoDecimalAfterTotalEightRegErrMsg|" + LocalizeResourceSetConstants.Global)]

        [Display(Name = "spnActualDuration|" + LocalizeResourceSetConstants.Global)]
        public decimal? ActualDuration { get; set; }

        [Display(Name = "spnCompletedBy|" + LocalizeResourceSetConstants.WorkOrderDetails)]
        public long CompleteBy_PersonnelId { get; set; }
        [Display(Name = "spnCompleteComments|" + LocalizeResourceSetConstants.Global)]
        public string CompleteComments { get; set; }

        [Display(Name = "spnCompletedBy|" + LocalizeResourceSetConstants.WorkOrderDetails)]
        public string CompleteBy { get; set; }
        public string WorkAssigned { get; set; }
        public IEnumerable<SelectListItem> ScheduleWorkList { get; set; }
        public IEnumerable<SelectListItem> PriorityList { get; set; }
        public IEnumerable<SelectListItem> ShiftList { get; set; }
        public IEnumerable<SelectListItem> TypeList { get; set; }
        public IEnumerable<SelectListItem> AccountLookUpList { get; set; }
        public IEnumerable<SelectListItem> ChargeTypeList { get; set; }
        public IEnumerable<SelectListItem> ChargeTypelookUpList { get; set; }
        public IEnumerable<SelectListItem> FailureList { get; set; }
        public IEnumerable<SelectListItem> WorkAssignedLookUpList { get; set; }
        public IEnumerable<SelectListItem> CreatebyLookUpList { get; set; }
        public IEnumerable<SelectListItem> StatusList { get; set; }
        public IEnumerable<SelectListItem> DenyReasonList { get; set; }

        public IEnumerable<SelectListItem> WbStatusList { get; set; }
        public IEnumerable<SelectListItem> CreateDatesList { get; set; }
        public string CreateBy_PersonnelName { get; set; }

        #endregion
        public string ModifyBy { get; set; }
        public DateTime? ModifyDate { get; set; }
        public string ActionCode { get; set; }
        public decimal ActualLaborCosts { get; set; }
        public decimal ActualLaborHours { get; set; }
        public decimal ActualMaterialCosts { get; set; }
        public decimal ActualOutsideServiceCosts { get; set; }
        public DateTime? ActualStartDate { get; set; }
        public decimal ActualTotalCosts { get; set; }
        public bool ApprovalRequired { get; set; }
        public long ApproveBy_PersonnelId { get; set; }
        public DateTime? ApproveDate { get; set; }
        public string CancelReason { get; set; }
        [Display(Name = "GlobalChargeTo|" + LocalizeResourceSetConstants.Global)]
        public long? ChargeToId { get; set; }
        public long CloseBy_PersonnelId { get; set; }
        public DateTime? CloseDate { get; set; }
        public bool CompleteAllTasks { get; set; }
        public DateTime? CompleteDate { get; set; }
        public long Creator_PersonnelId { get; set; }
        public string Crew { get; set; }
        
        [Display(Name = "spnChargeToName|" + LocalizeResourceSetConstants.Global)]
        public string ChargeToClientLookupId { get; set; }
        public int TotalCount { get; set; }

        public bool EquipDown { get; set; }
        public DateTime? EquipDownDate { get; set; }
        public decimal EquipDownHours { get; set; }
        public DateTime? EquipUpDate { get; set; }
        public decimal EstimatedLaborCosts { get; set; }
        public decimal EstimatedLaborHours { get; set; }
        public decimal EstimatedMaterialCosts { get; set; }
        public decimal EstimatedOutsideServiceCosts { get; set; }
        public decimal EstimatedPurchaseMaterialCosts { get; set; }

        public decimal EstimatedTotalCosts { get; set; }

        public string JobPlan { get; set; }

        public string strLabor_AccountId { get; set; }

        public string Location { get; set; }

        public long Material_AccountId { get; set; }


        public long MeterId { get; set; }

        public decimal MeterReadingDone { get; set; }

        public decimal MeterReadingDue { get; set; }

        public long Other_AccountId { get; set; }

        public long Planner_PersonnelId { get; set; }

        public long PrevMaintBatchId { get; set; }

        public int PrimaveraProjectNumber { get; set; }

        public int Printed { get; set; }



        public long ProjectId { get; set; }
        public string ReasonforDown { get; set; }
        public string ReasonNotDone { get; set; }
        public long ReleaseBy_PersonnelId { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public DateTime? RequestDate { get; set; }
        public long Requestor_PersonnelId { get; set; }
        public string RequestorLocation { get; set; }

        [Display(Name = "spnRequestorPhone|" + LocalizeResourceSetConstants.WorkOrderDetails)]
        public string RequestorPhone { get; set; }

        public int RIMEAssetCriticality { get; set; }
        public int RIMEPriority { get; set; }
        public int RIMEWorkClass { get; set; }
        public DateTime? ScheduledFinishDate { get; set; }
        public long Scheduler_PersonnelId { get; set; }
        public string Section { get; set; }

        public DateTime? SignOffDate { get; set; }
        public long SignoffBy_PersonnelId { get; set; }
        public long SourceId { get; set; }

        public decimal SuspendDuration { get; set; }

        public long WorkAssigned_PersonnelId { get; set; }

        public int PartsOnOrder { get; set; }
        public byte[] WorkImage { get; set; }
        public string DeniedReason { get; set; }
        public DateTime? DeniedDate { get; set; }
        public long DeniedBy_PersonnelId { get; set; }
        public string DeniedComment { get; set; }
        public bool EmergencyWorkOrder { get; set; }
        public DateTime? CancelDate { get; set; }
        public string Category { get; set; }

        [Display(Name = "spnRequestorName|" + LocalizeResourceSetConstants.WorkOrderDetails)]
        public string RequestorName { get; set; }
        public string RequestorPhoneNumber { get; set; }


        [Display(Name = "spnRequestorEmail|" + LocalizeResourceSetConstants.WorkOrderDetails)]
        public string RequestorEmail { get; set; }
        public string AzureImageURL { get; set; }


        public string PersonnelClientLookupId { get; set; }
        public string Requestor_PersonnelClientLookupId { get; set; }

        public string DateRange { get; set; }
        public string DateColumn { get; set; }
        public string Creator { get; set; }

        public string DepartmentName { get; set; }
        public int UpdateIndex { get; set; }

        public string WorkAssigned_PersonnelClientLookupId { get; set; }
        public string CompleteBy_PersonnelClientLookupId { get; set; }
        public string SignoffBy_PersonnelClientLookupId { get; set; }
        public string SignoffBy_PersonnelClientLookupIdSecond { get; set; }
        public bool IsFoodSafetyShow { get; set; }


        public string SiteInformation { get; set; }
        public bool PlantLocation { get; set; }
        public List<DataTableDropdownModel> WorkAssignedList { get; set; }
        public List<DataTableDropdownModel> ShiftListdropDown { get; set; }
        public string BusinessType { get; set; }
        public Security security { get; set; }
        public string PackageLevel { get; set; }

        public string Personnels { get; set; }
        public string PersonnelList { get; set; }

        public string AssetGroup1ClientlookupId { get; set; }
        public string AssetGroup2ClientlookupId { get; set; }
        public string AssetGroup3ClientlookupId { get; set; }
        public string AssetGroup1Name { get; set; }
        public string AssetGroup2Name { get; set; }
        public string AssetGroup3Name { get; set; }
        //<!--(Added on 25/06/2020)-->
        public long AssetGroup1Id { get; set; }
        public long AssetGroup2Id { get; set; }
        public long AssetGroup3Id { get; set; }

        public string AssetGroup1AdvSearchId { get; set; }
        public string AssetGroup2AdvSearchId { get; set; }
        public string AssetGroup3AdvSearchId { get; set; }
        //<!--(Added on 25/06/2020)-->
        public IEnumerable<SelectListItem> DateRangeDropList { get; set; }
        public IEnumerable<SelectListItem> DateRangeDropListForWO { get; set; }        
        //V2-347
        //v2-364
        public IEnumerable<SelectListItem> DateRangeDropListForWOCreatedate { get; set; }
        public IEnumerable<SelectListItem> SourceTypeList { get; set; }
        public IEnumerable<SelectListItem> AssetGroup1List { get; set; }
        public IEnumerable<SelectListItem> AssetGroup2List { get; set; }
        public IEnumerable<SelectListItem> AssetGroup3List { get; set; }
        public string Intructions { get; set; }
        public string ProjectClientLookupId { get; set; }

        public bool ClientOnPremise { get; set; }
        //V2-732
        public bool IsUseMultiStoreroom { get; set; }
        public string PlannerFullName { get; set; } //V2-1078

        [Display(Name = "spnProjectId|" + LocalizeResourceSetConstants.Global)]
        public string ProjectIds { get; set; } //V2-1135
    }
}