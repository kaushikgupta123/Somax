using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Models.Configuration.FleetServiceTask
{
    public class FleetServiceTaskSearchModel
    {
        public string orderbyColumn { get; set; }
        public string orderBy { get; set; }
        public Int32 offset1 { get; set; }
        public Int32 nextrow { get; set; }

        public long ServiceTaskId { get; set; }
        public string ClientLookupId { get; set; }
        public string Description { get; set; }
        public bool InactiveFlag { get; set; }
        public int TotalCount { get; set; }

        //public FleetServiceTaskSearchModel(string ClientLookupId, string Description)
        //{
        //    this.ClientLookupId = ClientLookupId;
        //    this.Description = Description;
        //}
    }
}