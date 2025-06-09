using Client.Models.Common;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Client.Models.MaterialRequest
{
    [Serializable]
    public class MRPrintParams
    {
        public MRPrintParams()
        {
            tableHaederProps = new List<TableHaederProp>();
        }
        public List<TableHaederProp> tableHaederProps { get; set; }
        public string colname { get; set; }
        public string coldir { get; set; }
        public int CustomQueryDisplayId { get; set; }
        public string txtSearchval { get; set; }
        public long MaterialRequestId { get; set; }
        public string Description { get; set; }
        public string Status { get; internal set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? RequiredDate { get; set; }
        public DateTime? CompleteDate { get; set; }
        public string Account_ClientLookupId { get; set; }
    }
}