using Client.Models.Common;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Client.Models.Sanitation
{
    public class SanitationJobModel
    {
        public long SanitationJobId { get; set; }
        public long SanitationMasterId { get; set; }
        public long AreaId { get; set; }
        public long SiteId { get; set; }
        public long DepartmentId { get; set; }
        public long StoreroomId { get; set; }
        public string ClientLookupId { get; set; }
        public string SourceType { get; set; }
        public long SourceId { get; set; }
        public decimal ActualDuration { get; set; }
        public long AssignedTo_PersonnelId { get; set; }
        public string CancelReason { get; set; }
        public long ChargeToId { get; set; }
        public string ChargeType { get; set; }
        public string ChargeTo_Name { get; set; }
        public long CompleteBy_PersonnelId { get; set; }
        public string CompleteComments { get; set; }
        public DateTime? CompleteDate { get; set; }
        public string Description { get; set; }
        public string Shift { get; set; }
        public bool DownRequired { get; set; }
        public DateTime? ScheduledDate { get; set; }
        public decimal ScheduledDuration { get; set; }
        public string Status { get; set; }
        public long Creator_PersonnelId { get; set; }
        public long ApproveBy_PersonnelId { get; set; }
        public DateTime? ApproveDate { get; set; }
        public long DeniedBy_PersonnelId { get; set; }
        public DateTime? DeniedDate { get; set; }
        public string DeniedReason { get; set; }
        public string DeniedComment { get; set; }
        public long PassBy_PersonnelId { get; set; }
        public DateTime? PassDate { get; set; }
        public long FailBy_PersonnelId { get; set; }
        public DateTime? FailDate { get; set; }
        public string FailReason { get; set; }
        public string FailComment { get; set; }
        public long SanOnDemandMasterId { get; set; }
        public bool Extracted { get; set; }
        public long ExportLogId { get; set; }
        public int UpdateIndex { get; set; }
        public string ValidateType { get; set; }
        public string Assigned { get; set; }
        public string OnDemandGroup { get; set; }
        public string CompleteBy { get; set; }
        public string ShiftDescription { get; set; }
        public string ChargeTo_ClientLookupId { get; set; }
        public long PlantLocationId { get; set; }
        public string DisplayTextChecked { get; set; }
        public string Status_Display { get; set; }
        public string CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public string Prefix { get; set; }
        public long PersonnelId { get; set; }
        public int SanitationMasterCount { get; set; }
        public int SanitationJobCount { get; set; }
        public string SanitationJobList { get; set; }
        public string PersonnelIdList { get; set; }
        public DateTime? ScheduledDateFrom { get; set; }
        public DateTime? ScheduledDateTo { get; set; }
        public string Creator_PersonnelClientLookupId { get; set; }
        public int CustomQueryDisplayId { get; set; }
        public string VerificationCompleteDate { get; set; }
        public string VerifiedBy { get; set; }
        public DateTime? VerifiedDate { get; set; }
        public string CheckStatus { get; set; }
        public string CreateByName { get; set; }
        public string PassBy { get; set; }
        public string FailBy { get; set; }
        public string ImageURI { get; set; }
        public string Creator { get; set; }
        public string CreateBy_Name { get; set; }
        public string Assigned_PersonnelClientLookupId { get; set; }
        public bool CompleteAllTasks { get; set; }
        public string ApproveStatusDrop { get; set; }
        public string ApproveCreatedDate { get; set; }
        public string CreateBy_PersonnelId { get; set; }
        public string ModifyBy { get; set; }
        public string ScheduleFlag { get; set; }
        public string ApproveFlag { get; set; }
        public string DeniedFlag { get; set; }
        public string AssetGroup1Name { get; set; }
        public string AssetGroup2Name { get; set; }
        public string AssetGroup3Name { get; set; }
        public IEnumerable<SelectListItem> WbStatusList { get; set; }
        public IEnumerable<SelectListItem> CreateDatesList { get; set; }
        public IEnumerable<SelectListItem> WorkAssignedLookUpList { get; set; }
        public IEnumerable<SelectListItem> CreatebyLookUpList { get; set; }
        public IEnumerable<SelectListItem> ShiftList { get; set; }
        public IEnumerable<SelectListItem> DenyReasonList { get; set; }
        public List<DataTableDropdownModel> WorkAssignedList { get; set; }
        public List<DataTableDropdownModel> ShiftListdropDown { get; set; }
        public IEnumerable<SelectListItem> DateRangeDropListForSJCreatedate { get; set; }
        public IEnumerable<SelectListItem> DateRangeDropListForSJ { get; set; }
        public IEnumerable<SelectListItem> DateRangeDropListForFailedSJ { get; set; }
        public IEnumerable<SelectListItem> DateRangeDropListForPassedSJ { get; set; }
       public IEnumerable<SelectListItem> SanitationJobViewSearchList { get; set; } //V2-910

    }
}