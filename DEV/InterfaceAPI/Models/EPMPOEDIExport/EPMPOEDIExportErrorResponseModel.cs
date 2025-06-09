using System.Collections.Generic;

namespace InterfaceAPI.Models.EPMPOEDIExport
{
    public class EPMPOEDIExportErrorResponseModel
    {
        public EPMPOEDIExportErrorResponseModel()
        {
            PurchaseOrderEDIEXPProcessErrMsgList = new List<string>();
            PurchaseOrderEDIEXPErrMsgList = new List<string>();
        }
        
        public List<string> PurchaseOrderEDIEXPProcessErrMsgList { get; set; }
        public List<string> PurchaseOrderEDIEXPErrMsgList { get; set; }
    }
}