using System.Collections.Generic;

namespace InterfaceAPI.Models.BBU.VendorCatalogImport
{
    public class VendorCatalogImportResponseModel
    {
        public VendorCatalogImportResponseModel()
        {
            errMessageList = new List<VendorCatalogImportErrorResponseModel>();
        }
        public string fileName { get; set; }
        public string status { get; set; }
        public string errMessage { get; set; }
        public List<VendorCatalogImportErrorResponseModel> errMessageList { get; set; }
        public List<VendorCatalogImportResponseModel> VendorCatalogImportResponseModelList { get; set; }
    }
}