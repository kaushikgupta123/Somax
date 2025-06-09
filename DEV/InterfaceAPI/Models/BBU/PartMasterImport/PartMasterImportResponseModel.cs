using System.Collections.Generic;

namespace InterfaceAPI.Models.BBU.PartMasterImport
{
    public class PartMasterImportResponseModel
    {
        public PartMasterImportResponseModel()
        {
            errMessageList = new List<PartMasterImportErrorResponseModel>();
        }
        public string fileName { get; set; }
        public string status { get; set; }
        public string errMessage { get; set; }
        public List<PartMasterImportErrorResponseModel> errMessageList { get; set; }
        public List<PartMasterImportResponseModel> PartMasterImportResponseModelList { get; set; }
    }
}