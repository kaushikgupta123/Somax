using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Models.PreventiveMaintenance
{
    public class PMGenerateWorkOrdersSearchModel
    {
        public string orderbyColumn { get; set; }
        public string orderBy { get; set; }
        public Int32 offset1 { get; set; }
        public Int32 nextrow { get; set; }
        public long PrevMaintBatchEntryId { get; set; }
        public long PrevMaintBatchHeaderId { get; set; }
        public DateTime? DueDate { get; set; }
        public string EquipmentClientLookupId { get; set; }
        public string EquipmentName { get; set; }
        public string PrevMaintMasterClientLookupId { get; set; }
        public string PrevMaintMasterDescription { get; set; }
        public int TotalCount { get; set; }
        public string AssignedTo_PersonnelIds { get; set; }
        public DateTime? PMRequiredDate { get; set; }
        public int ChildCount { get; set; }
        public string AssignedTo_Name { get; set; }
        public string AssignedMultiple { get; set; }
        #region V2-1082
        public string Shift { get; set; }
        public bool? DownRequired { get; set; }
        #endregion
        //V2-1161
        public long PrevMaintSchedId { get; set; }
        public bool? PlanningRequired { get; set; } 

    }
}