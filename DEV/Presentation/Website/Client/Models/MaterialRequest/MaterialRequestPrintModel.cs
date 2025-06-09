using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Client.Models.MaterialRequest
{
    public class MaterialRequestPrintModel
    {
        public long MaterialRequestId { get; set; }
        public string Description { get; set; }
        public DateTime? RequiredDate { get; set; }
        public string Account_ClientLookupId { get; set; }
        public string Status { get; internal set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? CompleteDate { get; set; }
    }
}