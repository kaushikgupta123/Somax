using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Models.AutoTRGeneration
{
    public class PartTranProcessTableModel
    {
        public long RowId { get; set; }
        public long RequestPTStoreroomId { get; set; }
        public long RequestStoreroomId { get; set; }
        public long RequestPartId { get; set; }
        public long IssuePTStoreroomId { get; set; }
        public long IssueStoreroomId { get; set; }
        public long IssuePartId { get; set; }
        public long Creator_PersonnelId { get; set; }
        public decimal? TransferQuantity { get; set; }
    }
}