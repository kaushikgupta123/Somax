using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Models.PreventiveMaintenance
{
    public class PMGeneratedWorkOrderPrintModel
    {

        public string DueDate { get; set; }
        public string EquipmentClientLookupId { get; set; }
        public string EquipmentName { get; set; }
        public string PrevMaintMasterClientLookupId { get; set; }
        public string PrevMaintMasterDescription { get; set; }
        public string RequiredDate { get; set; }
        public string AssignedToName { get; set; }
        public bool? DownRequired { get; set; }
        public string Shift { get; set; }



    }
}