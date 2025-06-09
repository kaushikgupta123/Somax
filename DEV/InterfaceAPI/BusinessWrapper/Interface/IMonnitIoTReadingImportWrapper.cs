using InterfaceAPI.Models.Account;
using InterfaceAPI.Models.Common;
using InterfaceAPI.Models.MonnitIoTReading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterfaceAPI.BusinessWrapper.Interface
{
    public interface IMonnitIoTReadingImportWrapper : ICommonWrapper
    {
        ProcessLogModel CreateIoTReadingImport(List<MonnitIoTReadingImportJsonModel> IoTReadingImportModelList);
        List<MonnitIoTReadingErrorResponseModel> IoTReadingImportValidate(long logId);
    }
}
