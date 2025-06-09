using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Models.Common
{
    public class AssetAvailabilityLogLookUpModel
    {
        public long ObjectId { get; set; }
        public string TransactionDate { get; set; }
        public string Event { get; set; }
        public string ReturnToService { get; set; }
        public string Reason { get; set; }
        public string ReasonCode { get; set; }
        public string PersonnelName { get; set; }
        public long TotalCount { get; set; }
    }
}