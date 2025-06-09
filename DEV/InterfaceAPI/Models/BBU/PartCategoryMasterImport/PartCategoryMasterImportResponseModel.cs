using System.Collections.Generic;

namespace InterfaceAPI.Models.BBU.PartCategoryMasterImport
{
    public class PartCategoryMasterImportResponseModel
    {
        public PartCategoryMasterImportResponseModel()
        {
            errMessageList = new List<PartCategoryMasterImportErrorResponseModel>();
        }
        public string fileName { get; set; }
        public string status { get; set; }
        public string errMessage { get; set; }
        public List<PartCategoryMasterImportErrorResponseModel> errMessageList { get; set; }
        public List<PartCategoryMasterImportResponseModel> PartCategoryMasterImportResponseModelList { get; set; }
    }
}