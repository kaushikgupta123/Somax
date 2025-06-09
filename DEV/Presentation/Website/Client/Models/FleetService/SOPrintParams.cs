using Client.Models.Common;
using System;
using System.Collections.Generic;

namespace Client.Models.FleetService
{
    [Serializable]
    public class SOPrintParams
    {
        public SOPrintParams()
        {
            tableHaederProps = new List<TableHaederProp>();
        }
        public List<TableHaederProp> tableHaederProps { get; set; }
        public string colname { get; set; }
        public string coldir { get; set; }
        public int CustomQueryDisplayId { get; set; }
        public long ServiceOrderId { get; set; }
        public string ClientLookupId { get; set; }
        public string AssetID { get; set; }
        public string AssetName { get; set; }
        public string Description { get; set; }
        public string Shift { get; set; }
        public string Type { get; set; }
        public string VIN { get; set; }
        public string searchText { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? ScheduleDate { get; set; }
        public DateTime? CompleteDate { get; set; }
        public string Assigned { get; set; }
        public int ChildCount { get; set; }
        public int TotalCount { get; set; }
        public DateTime? CreateStartDateVw { get; set; }
        public DateTime? CreateEndDateVw { get; set; }
        public DateTime? CompleteStartDateVw { get; set; }
        public DateTime? CompleteEndDateVw { get; set; }
        public string personnelList { get; set; }
    }
}