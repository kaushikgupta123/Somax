using Client.Models.Common;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Client.Models.Sanitation
{
    public class SanitationVerificationModel
    {
        public long ClientId { get; set; }       
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
        public string Assigned_PersonnelClientLookupId { get; set; }
        public string CancelReason { get; set; }
        public long ChargeToId { get; set; }
        public string ChargeType { get; set; }
        public string ChargeTo_Name { get; set; }
        public string ChargeTo_ClientLookupId { get; set; }
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
        public string CreateBy_PersonnelId { get; set; }
        public IEnumerable<SelectListItem> scheduleStatusList { get; set; }
        public IEnumerable<SelectListItem> scheduleCreateDateList { get; set; }

        public string schStatusId { get; set; }
        public string schCreateDateId { get; set; }
        public List<DataTableDropdownModel> FailReasonList { get; set; }
    }
}