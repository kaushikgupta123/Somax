using System.Collections.Generic;

namespace InterfaceAPI.Models.BBU.POImport
{
    public class POImportResponseModel
    {
        public POImportResponseModel()
        {
            errMessageList = new List<POImportErrorResponseModel>();
        }
        public string fileName { get; set; }
        public string status { get; set; }
        public string errMessage { get; set; }
        public List<POImportErrorResponseModel> errMessageList { get; set; }
        public List<POImportResponseModel> poImportResponseModelList { get; set; }
    }
}