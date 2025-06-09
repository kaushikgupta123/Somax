using System.Collections.Generic;

namespace InterfaceAPI.Models.BBU.PartMasterResponseImport
{
    public class PartMasterResponseImportErrorResponseModel
    {
        public PartMasterResponseImportErrorResponseModel()
        {
            PMRIErrMsgList = new List<string>();
            PMRIProcessErrMsgList = new List<string>();
        }       
        public List<string> PMRIProcessErrMsgList { get; set; }
        public List<string> PMRIErrMsgList { get; set; }
    }
}