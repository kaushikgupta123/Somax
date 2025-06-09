using Client.Models.Common;
using System;
using System.Collections.Generic;


namespace Client.Models.WorkOrderPlanning
{
    [Serializable]
    public class RSPrintParams
    {
        public RSPrintParams()
        {
            tableHaederProps = new List<TableHaederProp>();
        }
        public List<TableHaederProp> tableHaederProps { get; set; }
        public string colname { get; set; }
        public string coldir { get; set; }
        public long WorkOrderPlanId { get; set; }
        public string ClientLookupId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string RequiredDate { get; set; }       
        public string Type { get; set; }
        public List<string> PersonnelList { get; set; }
        public string SearchText { get; set; }

    }
}