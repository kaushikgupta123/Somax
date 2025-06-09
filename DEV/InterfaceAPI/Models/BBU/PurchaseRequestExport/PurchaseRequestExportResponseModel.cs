using System.Collections.Generic;

namespace InterfaceAPI.Models.BBU.PurchaseRequestExport
{
    public class PurchaseRequestExportResponseModel
    {
        public PurchaseRequestExportResponseModel()
        {
            errMessageList = new List<PurchaseRequestExportResponseModel>();
        }
        public string fileName { get; set; }
        public string status { get; set; }
        public string errMessage { get; set; }
        public List<PurchaseRequestExportResponseModel> errMessageList { get; set; }
    }
}