using System.Collections.Generic;

namespace InterfaceAPI.Models.BBU.PartCategoryMasterImport
{
    public class PartCategoryMasterImportErrorResponseModel
    {
        public PartCategoryMasterImportErrorResponseModel()
        {
            PartCategoryMasterImportProcessErrMsgList = new List<string>();
            PartCategoryMasterImportErrMsgList = new List<string>();
        }
        public int ExAccountId { get; set; }
        public List<string> PartCategoryMasterImportProcessErrMsgList { get; set; }
        public List<string> PartCategoryMasterImportErrMsgList { get; set; }
    }
}