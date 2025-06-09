using System;
using System.Collections.Generic;

namespace Client.Models.Reports
{
    public class POByAccoutntReportDevExpressPrintModel : LocalisationBaseVM
    {
        public POByAccoutntReportDevExpressPrintModel()
        {
            POItemsDevExpressPrintModel = new List<POItemsDevExpressPrintModel>();
        }
        public List<POItemsDevExpressPrintModel> POItemsDevExpressPrintModel { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}