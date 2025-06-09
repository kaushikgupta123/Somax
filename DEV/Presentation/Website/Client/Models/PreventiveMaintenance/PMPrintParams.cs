using Client.Models.Common;
using System;
using System.Collections.Generic;

namespace Client.Models.PreventiveMaintenance
{
    [Serializable]
    public class PMPrintParams
    {
        public PMPrintParams()
        {
            tableHaederProps = new List<TableHaederProp>();
        }
        public List<TableHaederProp> tableHaederProps { get; set; }
        public string colname { get; set; }
        public string coldir { get; set; }
        public long EquipmentId { get; set; }
        public long LocationId { get; set; }
        public long AssignedId { get; set; }
        public string SearchText { get; set; }
        public bool InactiveFlag { get; set; }
        public string MasterjobId { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public string ScheduleType { get; set; }
        public string ChargeTo { get; set; }
        public string ChargeToName { get; set; }
    }
}