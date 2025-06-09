using DataContracts;
using InterfaceAPI.Models.BBU.PartMasterImport;
using InterfaceAPI.Models.BBU.PurchaseRequestExport;
using InterfaceAPI.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InterfaceAPI.BusinessWrapper.Interface.BBU
{
    public interface IPurchaseRequestExportWrapper : ICommonWrapper
    { 
        List<PurchaseRequestExportModel> ConvertDataRowToModel(long logId);       
        string ExportToSFTP(string fileName, FileTypeEnum fileType);
        Int64 ExportLog(string Type, Int64 LogId, string msg, int rowsProcessed);
        string ConvertToDataFile(List<PurchaseRequestExportModel> ExportList, out int rowsExtracted);

    }
}