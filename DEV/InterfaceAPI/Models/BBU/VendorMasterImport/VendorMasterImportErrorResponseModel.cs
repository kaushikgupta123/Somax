using System.Collections.Generic;

namespace InterfaceAPI.Models.BBU.VendorMasterImport
{
    public class VendorMasterImportErrorResponseModel
    {
        public VendorMasterImportErrorResponseModel()
        {
            VendorMasterImportProcessErrMsgList = new List<string>();
            VendorMasterImportErrMsgList = new List<string>();
        }
        
        public List<string> VendorMasterImportProcessErrMsgList { get; set; }
        public List<string> VendorMasterImportErrMsgList { get; set; }
    }
}