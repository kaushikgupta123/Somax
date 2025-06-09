using System;
namespace Client.Models.PreventiveMaintenance
{
    public class PMSchedulingReassignModel
    {
        public long PrevMaintSchedId  { get; set; }
         public string AssignedTo { get; set; }
        public string PMID { get; set; }
        public string Description { get; set; }
        public string ChargeTo { get; set; }
        public string ChargeToName { get; set; }
        public DateTime? NextDue { get; set; }

        public int TotalCount { get; set; }
        #region V2-977
        public long PMSAssignId { get; set; }
        public long IndexId { get; set; }
        #endregion
    }
}