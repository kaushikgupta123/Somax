using System.Collections.Generic;

namespace InterfaceAPI.Models.BBU.POReceiptImport
{
    public class POReceiptImportErrorResponseModel
    {
        public POReceiptImportErrorResponseModel()
        {
            POReceiptImportProcessErrMsgList = new List<string>();
            POReceiptImportErrMsgList = new List<string>();
        }       
        public List<string>POReceiptImportProcessErrMsgList { get; set; }
        public List<string> POReceiptImportErrMsgList { get; set; }
    }
}