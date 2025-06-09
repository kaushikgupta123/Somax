using System.Collections.Generic;

namespace InterfaceAPI.Models.BBU.PartMasterImport
{
    public class PartMasterImportErrorResponseModel
    {
        public PartMasterImportErrorResponseModel()
        {
            PMIErrMsgList = new List<string>();
            PMIProcessErrMsgList = new List<string>();
        }
        public int ExAccountId { get; set; }
        public List<string> PMIProcessErrMsgList { get; set; }
        public List<string> PMIErrMsgList { get; set; }
    }
}