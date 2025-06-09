using InterfaceAPI.Models.Account;
using InterfaceAPI.Models.Common;
using InterfaceAPI.Models.EPMInvoiceImport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterfaceAPI.BusinessWrapper.Interface
{
    public interface IEPMInvoiceImportWrapper : ICommonWrapper
    {
        ProcessLogModel CreateEPMInvoiceImport(EPMInvoiceImportModel ePMInvoiceImportModel);
        List<EPMInvoiceImportErrorResponseModel> EPMInvoiceImportValidate(long logId);
    }
}
