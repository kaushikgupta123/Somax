using InterfaceAPI.Models.EPMPOEDIExport;
using InterfaceAPI.Models.Common;

namespace InterfaceAPI.BusinessWrapper.Interface
{
    public interface IEPMPOEDIExportWrapper : ICommonWrapper
    {     
        EPMPOEDIExportModel ConvertDataRowToModel(long PurchaseOrderId);       
        bool CheckLoginSession(string LoginSessionID);       
        string ExportToSFTP(string fileName, FileTypeEnum fileType);
        string ConvertToDataFile(EPMPOEDIExportModel ExportList);

    }
}