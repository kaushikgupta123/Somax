using System.Collections.Generic;

namespace InterfaceAPI.Models.BBU.PartMasterRequestExport
{
    public class PartMasterRequestExportResponseModel
    {
        public PartMasterRequestExportResponseModel()
        {
            errMessageList = new List<PartMasterRequestExportResponseModel>();
        }
        public string fileName { get; set; }
        public string status { get; set; }
        public string errMessage { get; set; }
        public List<PartMasterRequestExportResponseModel> errMessageList { get; set; }
    }
}