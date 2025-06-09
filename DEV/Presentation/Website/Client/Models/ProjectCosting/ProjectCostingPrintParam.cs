using Client.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Models.ProjectCosting
{
    [Serializable]
    public class ProjectCostingPrintParam
    {
        public ProjectCostingPrintParam()
        {
            tableHaederProps = new List<TableHaederProp>();
        }
        public List<TableHaederProp> tableHaederProps { get; set; }
        public string colname { get; set; }
        public string coldir { get; set; }
        public int CustomQueryDisplayId { get; set; }
        public string ClientlookupId { get; set; }
        public string Description { get; set; }
        public string WorkOrderClientLookupId { get; set; }
        public string CreateStartDateVw { get; set; }
        public string CreateEndDateVw { get; set; }
        public string CompleteStartDateVw { get; set; }
        public string CompleteEndDateVw { get; set; }
        public string CloseStartDateVw { get; set; }
        public string CloseEndDateVw { get; set; }
        public string Status { get; set; }
        public string txtSearchval { get; set; }
    }
}