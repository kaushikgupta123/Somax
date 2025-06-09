using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Models.GenerateSanitationJobs
{
    
    public class SanitationJobBatchEntryModel
    {
        public SanitationJobBatchEntryModel()
        {
            list = new List<SanBatchEntryListModel>();
        }
        public List<string> AssetGroup1Ids { get; set; }
        public List<string> AssetGroup2Ids { get; set; }
        public List<string> AssetGroup3Ids { get; set; }
        public bool chkPrintSan { get; set; }
        public string ScheduleType { get; set; }
        public string OnDemand { get; set; }
        public DateTime? GenerateThrough { get; set; }
        public List<SanBatchEntryListModel> list { get; set; }
    }

    public class SanBatchEntryListModel
    {
        public long SanMasterBatchEntryId { get; set; }
        public long SanMasterBatchHeaderId { get; set; }

    }
}