using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Client.Models.MaterialRequest
{
    public class MaterialRequestSummaryModel
    {
        public long ClientId { get; set; }
        public long SiteId { get; set; }
        public long MaterialRequestId { get; set; }
        public string Description { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? RequiredDate { get; set; }
        public DateTime? CompleteDate { get; set; }
        public string Status { get; internal set; }
        public string Account_ClientLookupId { get; set; }
        public string Personnel_NameFirst { get; set; }
        public string Personnel_NameLast { get; set; }
    }
    
}