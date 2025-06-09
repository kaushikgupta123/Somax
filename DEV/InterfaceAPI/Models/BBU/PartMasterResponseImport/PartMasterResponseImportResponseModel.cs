using System.Collections.Generic;

namespace InterfaceAPI.Models.BBU.PartMasterResponseImport
{
    public class PartMasterResponseImportResponseModel
    {
        public PartMasterResponseImportResponseModel()
        {
            errMessageList = new List<PartMasterResponseImportErrorResponseModel>();
        }
        public string fileName { get; set; }
        public string status { get; set; }
        public string errMessage { get; set; }
        public List<PartMasterResponseImportErrorResponseModel> errMessageList { get; set; }
        public List<PartMasterResponseImportResponseModel> partMasterResponseImportResponseModelList { get; set; }
    }
}