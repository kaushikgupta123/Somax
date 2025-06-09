using System;
using System.Collections.Generic;

namespace Client.Models.PreventiveMaintenance
{
    public class PreventiveMaintenanceSearchCriteriaModel
    {
        #region Property
        public string CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string ModifyBy { get; set; }
        public DateTime ModifyDate { get; set; }
        public string Inactive { get; set; }
        public Dictionary<string, List<KeyValuePair<string, string>>> SearchCriteria { get; set; }
        public Int32 CaseNo { get; set; }
        public string orderbyColumn { get; set; }
        public string orderBy { get; set; }
        public Int32 offset1 { get; set; }
        public Int32 nextrow { get; set; }
        public string Chargeto { get; set; }
        public string ChargetoName { get; set; }
        public Int32 FilterType { get; set; }
        public Int64 FilterValue { get; set; }
        public string SearchText { get; set; }
        public Int64 EquipmentId { get; set; }
        public Int64 LocationId { get; set; }
        public Int64 AssignedId { get; set; }
        public string ClientLookupId { get; set; }
        public string Description { get; set; }
        public string ScheduleType { get; set; }
        public string Type { get; set; }

        public bool InactiveFlag { get; set; }
        #endregion
    }
}