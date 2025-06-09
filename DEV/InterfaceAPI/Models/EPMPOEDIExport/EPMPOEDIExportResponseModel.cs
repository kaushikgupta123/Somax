using System.Collections.Generic;

namespace InterfaceAPI.Models.EPMPOEDIExport
{
    public class EPMPOEDIExportResponseModel
    {
        public EPMPOEDIExportResponseModel()
        {
            errMessageList = new List<EPMPOEDIExportResponseModel>();
        }
        public string fileName { get; set; }
        public string status { get; set; }
        public string errMessage { get; set; }
        public List<EPMPOEDIExportResponseModel> errMessageList { get; set; }
    }
}