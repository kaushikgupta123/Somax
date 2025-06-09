using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Models.GenerateSanitationJobs
{
    public class GenerateSanitationJobsSearchModel
    {
        public string orderbyColumn { get; set; }
        public string orderBy { get; set; }
        public Int32 offset1 { get; set; }
        public Int32 nextrow { get; set; }
        public long SanMasterBatchEntryId { get; set; }
        public long SanMasterBatchHeaderId { get; set; }
        public DateTime? DueDate { get; set; }
        public string EquipmentClientLookupId { get; set; }
        public string EquipmentName { get; set; }
        public string Shift { get; set; }
        public string Description { get; set; }
        public int TotalCount { get; set; }

    }
}