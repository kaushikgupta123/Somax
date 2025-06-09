using System.Collections.Generic;

namespace InterfaceAPI.Models.Vendor
{
    public class VendorCsvResponseModel
    {
        public VendorCsvResponseModel()
        {
            VendorImportResponseModelList = new List<VendorImportResponseModel>();
        }
        public string fileName { get; set; }
        public string status { get; set; }
        public string errMessage { get; set; }
        public List<VendorImportResponseModel> VendorImportResponseModelList { get; set; }
    }
}