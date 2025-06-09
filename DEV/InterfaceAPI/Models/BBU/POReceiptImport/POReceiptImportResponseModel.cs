using System.Collections.Generic;

namespace InterfaceAPI.Models.BBU.POReceiptImport
{
    public class POReceiptImportResponseModel
    {
        public POReceiptImportResponseModel()
        {
            errMessageList = new List<POReceiptImportErrorResponseModel>();
        }
        public string fileName { get; set; }
        public string status { get; set; }
        public string errMessage { get; set; }
        public List<POReceiptImportErrorResponseModel> errMessageList { get; set; }
        public List<POReceiptImportResponseModel> poReceiptImportResponseModelList { get; set; }
    }
}