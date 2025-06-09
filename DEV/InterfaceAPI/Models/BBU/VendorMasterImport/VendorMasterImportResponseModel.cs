using System.Collections.Generic;

namespace InterfaceAPI.Models.BBU.VendorMasterImport
{
    public class VendorMasterImportResponseModel
    {
        public VendorMasterImportResponseModel()
        {
            errMessageList = new List<VendorMasterImportErrorResponseModel>();
        }
        public string fileName { get; set; }
        public string status { get; set; }
        public string errMessage { get; set; }
        public List<VendorMasterImportErrorResponseModel> errMessageList { get; set; }
        public List<VendorMasterImportResponseModel> VendorMasterImportResponseModelList { get; set; }
    }
}