using System.Collections.Generic;

namespace InterfaceAPI.Models.Vendor
{
    public class VendorImportResponseModel
    {
        public VendorImportResponseModel()
        {
            errMessageList = new List<VendorMasterErrorModel>();
        }
        public string fileName { get; set; }
        public string status { get; set; }
        public string errMessage { get; set; }
        public List<VendorMasterErrorModel> errMessageList { get; set; }
    }
}