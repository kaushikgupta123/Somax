using System.Collections.Generic;

namespace InterfaceAPI.Models.BBU.PartMasterRequestExport
{
    public class PartMasterRequestExportErrorResponseModel
    {
        public PartMasterRequestExportErrorResponseModel()
        {
            PMREXPErrMsgList = new List<string>();
            PMREXPProcessErrMsgList = new List<string>();
        }
        public int ExAccountId { get; set; }
        public List<string> PMREXPProcessErrMsgList { get; set; }
        public List<string> PMREXPErrMsgList { get; set; }
    }
}