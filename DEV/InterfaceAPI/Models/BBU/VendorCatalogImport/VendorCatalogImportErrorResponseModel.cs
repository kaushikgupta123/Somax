using System.Collections.Generic;

namespace InterfaceAPI.Models.BBU.VendorCatalogImport
{
    public class VendorCatalogImportErrorResponseModel
    {
        public VendorCatalogImportErrorResponseModel()
        {
            VendorCatalogImportProcessErrMsgList = new List<string>();
            VendorCatalogImportErrMsgList = new List<string>();
        }
        
        public List<string> VendorCatalogImportProcessErrMsgList { get; set; }
        public List<string> VendorCatalogImportErrMsgList { get; set; }
    }
}