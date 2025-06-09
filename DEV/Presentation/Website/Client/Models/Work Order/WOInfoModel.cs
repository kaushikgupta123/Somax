using Client.CustomValidation;
using Common.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Client.Models.WorkOrder
{
    public class WOInfoModel
    {
        #region Workorder

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
        public string Description { get; set; }
        [Display(Name = "spnDownRequired|" + LocalizeResourceSetConstants.Global)]
        public bool DownRequired { get; set; }
        [Display(Name = "spnPriority|" + LocalizeResourceSetConstants.Global)]
        public string Priority { get; set; }
        [Display(Name = "spnAccount|" + LocalizeResourceSetConstants.Global)]
        public long? Labor_AccountId { get; set; }
        [Display(Name = "spnAssigned|" + LocalizeResourceSetConstants.Global)]
        public string Assigned { get; set; }
        [Display(Name = "spnChargeType|" + LocalizeResourceSetConstants.Global)]
        public string ChargeType { get; set; }
        [Display(Name = "spnChargeToName|" + LocalizeResourceSetConstants.Global)]
        public string ChargeTo_Name { get; set; }
        [Display(Name = "spnRequired|" + LocalizeResourceSetConstants.Global)]
        public DateTime? RequiredDate { get; set; }
        [Display(Name = "spnCreatedBy|" + LocalizeResourceSetConstants.Global)]
        public string Createby { get; set; }

        [Display(Name = "globalCreateDate|" + LocalizeResourceSetConstants.WorkOrderDetails)]
        public DateTime? CreateDate { get; set; }

        [Display(Name = "GlobalSourceType|" + LocalizeResourceSetConstants.Global)]
        public string SourceType { get; set; }
        [RequiredIfValueExist("WorkAssigned_PersonnelId", ErrorMessage = "Please select scheduled start date.")]
        [Display(Name = "spnScheduledDate|" + LocalizeResourceSetConstants.Global)]
        public DateTime? ScheduledStartDate { get; set; }
        [Display(Name = "spnScheduledDuration|" + LocalizeResourceSetConstants.Global)]
        [RequiredIfValueExistThenGreaterThanZeroAttribute("WorkAssigned_PersonnelId", ErrorMessage = "Please Fill Proper Schedule Duration.")]
        [RegularExpression(@"^\d*\.?\d{0,2}$", ErrorMessage = "globalTwoDecimalRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Range(0, 999999.99, ErrorMessage = "globalTwoDecimalAfterTotalEightRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        public decimal? ScheduledDuration { get; set; }
        [Display(Name = "spnFailure|" + LocalizeResourceSetConstants.WorkOrderDetails)]
        public string FailureCode { get; set; }

        [Display(Name = "spnActualFinish|" + LocalizeResourceSetConstants.WorkOrderDetails)]
        public DateTime? ActualFinishDate { get; set; }

        [RegularExpression(@"^\d*\.?\d{0,2}$", ErrorMessage = "globalTwoDecimalRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Range(0, 9999999999.99, ErrorMessage = "globalTwoDecimalAfterTotalTwelveRegErrMsg|" + LocalizeResourceSetConstants.Global)]

        [Display(Name = "spnActualDuration|" + LocalizeResourceSetConstants.Global)]
        public decimal? ActualDuration { get; set; }

        [Display(Name = "spnCompletedBy|" + LocalizeResourceSetConstants.WorkOrderDetails)]
        public long CompleteBy_PersonnelId { get; set; }
        [Display(Name = "spnCompleteComments|" + LocalizeResourceSetConstants.Global)]
        public string CompleteComments { get; set; }

        [Display(Name = "spnCompletedBy|" + LocalizeResourceSetConstants.WorkOrderDetails)]
        public IEnumerable<SelectListItem> ScheduleWorkList { get; set; }
        public IEnumerable<SelectListItem> PriorityList { get; set; }
        public IEnumerable<SelectListItem> ShiftList { get; set; }
        public IEnumerable<SelectListItem> TypeList { get; set; }
        public IEnumerable<SelectListItem> ChargeTypeList { get; set; }
        public IEnumerable<SelectListItem> ChargeTypelookUpList { get; set; }
        public IEnumerable<SelectListItem> WorkAssignedLookUpList { get; set; }
        public long? ChargeToId { get; set; }
        [Display(Name = "spnWorkAssigned|" + LocalizeResourceSetConstants.Global)]
        [RequiredIfValueExist("ScheduledStartDate",ErrorMessage = "Please enter Work Assigned name.")]
        public long? WorkAssigned_PersonnelId { get; set; }
        [Display(Name = "GlobalChargeTo|" + LocalizeResourceSetConstants.Global)]
        public string ChargeToClientLookupId { get; set; }
        public string CreateBy_PersonnelName { get; set; }
        public string Creator { get; set; }
        public string CompleteBy { get; set; }

        #endregion
    }
}