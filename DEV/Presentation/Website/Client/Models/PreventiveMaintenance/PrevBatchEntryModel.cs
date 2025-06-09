using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Models.PreventiveMaintenance
{
    public class PrevBatchEntryModel
    {
        public PrevBatchEntryModel()
        {
            list = new List<PrevBatchEntryListModel>();
        }
        public List<string> AssetGroup1Ids { get; set; }
        public List<string> AssetGroup2Ids { get; set; }
        public List<string> AssetGroup3Ids { get; set; }
        public List<string> PrevMaintSchedType { get; set; }
        public List<string> PrevMaintMasterType { get; set; }

        public bool chkPrintWorkOrder { get; set; }

        public List<PrevBatchEntryListModel> list { get; set; }
    }
   
    public class PrevBatchEntryListModel
    {
        public long PrevMaintBatchEntryId { get; set; }
        public long PrevMaintBatchHeaderId { get; set; }
        //V2-1161
        public long PrevMaintSchedId { get; set; } 
        public bool? PlanningRequired { get; set; } 

    }
}