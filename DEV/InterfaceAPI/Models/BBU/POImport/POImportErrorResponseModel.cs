using System.Collections.Generic;

namespace InterfaceAPI.Models.BBU.POImport
{
    public class POImportErrorResponseModel
    {
        public POImportErrorResponseModel()
        {
            POImportProcessErrMsgList = new List<string>();
            POImportErrMsgList = new List<string>();
        }       
        public List<string>POImportProcessErrMsgList { get; set; }
        public List<string> POImportErrMsgList { get; set; }
    }
}