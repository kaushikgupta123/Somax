using System.Collections.Generic;

namespace InterfaceAPI.Models.PurchaseOrderReceipt
{
    public class ReceiptLineErrorModel
    {
        public ReceiptLineErrorModel()
        {
            LineErrMsgList = new List<string>();
            LineProcessErrMsgList = new List<string>();
        }
        public int ExPOLineId { get; set; }
        public List<string> LineErrMsgList { get; set; }
        public List<string> LineProcessErrMsgList { get; set; }
    }
}