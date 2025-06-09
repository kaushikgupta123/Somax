using System.Collections.Generic;

namespace InterfaceAPI.Models.PurchaseOrderReceipt
{
    public class ReceiptErrorResponseModel
    {
        public ReceiptErrorResponseModel()
        {
            HdrErrMsgList = new List<string>();
            ReceiptLineErrorList = new List<ReceiptLineErrorModel>();
            HdrProcessErrMsgList = new List<string>();
        }
        public int EXPOID { get; set; }
        public List<string> HdrProcessErrMsgList { get; set; }
        public List<string> HdrErrMsgList { get; set; }
        public List<ReceiptLineErrorModel> ReceiptLineErrorList { get; set; }
    }
    public class ReceiptResponseModel
  {
      public string response_message { get; set; }
      public List<ReceiptImportModel> receipts { get; set; }
  }
}