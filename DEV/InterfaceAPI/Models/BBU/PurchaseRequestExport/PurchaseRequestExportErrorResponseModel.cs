using System.Collections.Generic;

namespace InterfaceAPI.Models.BBU.PurchaseRequestExport
{
    public class PurchaseRequestExportErrorResponseModel
    {
        public PurchaseRequestExportErrorResponseModel()
        {
            PurchaseRequestEXPProcessErrMsgList = new List<string>();
            PurchaseRequestEXPErrMsgList = new List<string>();
        }
        
        public List<string> PurchaseRequestEXPProcessErrMsgList { get; set; }
        public List<string> PurchaseRequestEXPErrMsgList { get; set; }
    }
}