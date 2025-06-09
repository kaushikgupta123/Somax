using DataContracts;
using InterfaceAPI.Models.BBU.PartMasterImport;
using InterfaceAPI.Models.BBU.PartMasterRequestExport;
using InterfaceAPI.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InterfaceAPI.BusinessWrapper.Interface.BBU
{
    public interface IPartMasterRequestExportWrapper : ICommonWrapper
    { 
        List<PartMasterRequestExportModel> ConvertDataRowToModel(long logId);       
        string ExportToSFTP(string fileName, FileTypeEnum fileType);
        Int64 ExportLog(string Type, Int64 LogId, string msg, int rowsProcessed);
        string ConvertToDataFile(List<PartMasterRequestExportModel> ExportList, out int rowsExtracted);

    }
}