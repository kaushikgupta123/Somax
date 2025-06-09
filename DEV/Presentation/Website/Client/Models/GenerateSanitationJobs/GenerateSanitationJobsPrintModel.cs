using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Models.GenerateSanitationJobs
{
    public class GenerateSanitationJobsPrintModel
    {
        public string DueDate { get; set; }
        public string EquipmentClientLookupId { get; set; }
        public string EquipmentName { get; set; }
        public string Description { get; set; }
        public string Shift { get; set; }
   
    }
}