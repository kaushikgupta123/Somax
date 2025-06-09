using System.Collections.Generic;

namespace InterfaceAPI.Models.PurchaseOrder
{
    public class ImportLineErrorModel
    {
        public ImportLineErrorModel()
        {
            LineErrMsgList = new List<string>();
            LineProcessErrMsgList = new List<string>();
        }
        public int ExPOLineId { get; set; }
        public List<string> LineErrMsgList { get; set; }
        public List<string> LineProcessErrMsgList { get; set; }
    }
}