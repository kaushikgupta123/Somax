using System.Collections.Generic;

namespace InterfaceAPI.Models.EPMInvoiceImport
{
    public class EPMInvoiceImportResponseModel
    {
        public EPMInvoiceImportResponseModel()
        {
            errMessageList = new List<EPMInvoiceImportErrorResponseModel>();
        }
        public string fileName { get; set; }
        public string status { get; set; }
        public string errMessage { get; set; }
        public List<EPMInvoiceImportErrorResponseModel> errMessageList { get; set; }
        public List<EPMInvoiceImportResponseModel> EPMInvoiceImportResponseModelList { get; set; }
    }
}