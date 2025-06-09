using System.Collections.Generic;

namespace InterfaceAPI.Models.PurchaseOrder
{
    public class ImportErrorResponseModel
    {
        public ImportErrorResponseModel()
        {
            HdrErrMsgList = new List<string>();
            ImportLineErrorList = new List<ImportLineErrorModel>();
            HdrProcessErrMsgList = new List<string>();
        }
        public int EXPOID { get; set; }
        public List<string> HdrProcessErrMsgList { get; set; }
        public List<string> HdrErrMsgList { get; set; }
        public List<ImportLineErrorModel> ImportLineErrorList { get; set; }
    }
}