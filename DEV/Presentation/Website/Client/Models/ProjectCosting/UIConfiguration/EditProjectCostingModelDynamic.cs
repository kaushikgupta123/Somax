using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Models.ProjectCosting.UIConfiguration
{
    public class EditProjectCostingModelDynamic
    {
        #region UDF columns
        public long ProjectUDFId { get; set; }
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
        public decimal? Numeric1 { get; set; }
        public decimal? Numeric2 { get; set; }
        public decimal? Numeric3 { get; set; }
        public decimal? Numeric4 { get; set; }
        public string Select1 { get; set; }
        public string Select2 { get; set; }
        public string Select3 { get; set; }
        public string Select4 { get; set; }
        #endregion


        #region Project table coulmn
        public long ProjectId { get; set; }
        public string ClientLookupId { get; set; }
        public DateTime? ActualFinish { get; set; }
        public DateTime? ActualStart { get; set; }
        public decimal? Budget { get; set; }
        public DateTime? CancelDate { get; set; }
        public long CancelBy_PersonnelId { get; set; }
        public string CancelReason { get; set; }
        public DateTime? CloseDate { get; set; }
        public long? CloseBy_PersonnelId { get; set; }
        public DateTime? CompleteDate { get; set; }
        public long? CompleteBy_PersonnelId { get; set; }
        public long? Coordinator_PersonnelId { get; set; }
        public string Description { get; set; }
        public int FiscalYear { get; set; }
        public DateTime? HoldDate { get; set; }
        public long? HoldBy_PersonnelId { get; set; }
        public long? Owner_PersonnelId { get; set; }
        public decimal ReturnFunds { get; set; }
        public DateTime? ScheduleFinish { get; set; }
        public DateTime? ScheduleStart { get; set; }
        public string Status { get; set; }
        public string Type { get; set; }
        public string Category { get; set; }
        public long? AssignedAssetGroup1 { get; set; }
        public long? AssignedAssetGroup2 { get; set; }
        public long? AssignedAssetGroup3 { get; set; }

        #endregion
    }
}