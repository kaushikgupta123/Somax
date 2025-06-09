using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Models.Configuration.ApprovalGroups
{
    public class ApprovalGroupsPDFPrintModel : ApprovalGroupsPrintModel
    {
        public ApprovalGroupsPDFPrintModel()
        {
            LineItemModelList = new List<LineItemModel>();
        }
        public List<LineItemModel> LineItemModelList { get; set; }
        public decimal Total { get; set; }
    }
}