using System.Collections.Generic;

namespace InterfaceAPI.Models.EPMInvoiceImport
{
    public class EPMInvoiceImportErrorResponseModel
    {
        public EPMInvoiceImportErrorResponseModel()
        {
            EPMInvoiceImportProcessErrMsgList = new List<string>();
            EPMInvoiceImportErrMsgList = new List<string>();
        }
        public string PurchaseOrderId { get; set; }
        public List<string> EPMInvoiceImportProcessErrMsgList { get; set; }
        public List<string> EPMInvoiceImportErrMsgList { get; set; }
    }
}