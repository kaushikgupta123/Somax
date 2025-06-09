using InterfaceAPI.Models.Account;
using InterfaceAPI.Models.Common;
using InterfaceAPI.Models.IoTReading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterfaceAPI.BusinessWrapper.Interface
{
    public interface IIoTReadingImportWrapper : ICommonWrapper
    {
        ProcessLogModel CreateIoTReadingImport(List<IoTReadingImportJsonModel> IoTReadingImportModelList);
        List<IoTReadingErrorResponseModel> IoTReadingImportValidate(long logId);
    }
}
