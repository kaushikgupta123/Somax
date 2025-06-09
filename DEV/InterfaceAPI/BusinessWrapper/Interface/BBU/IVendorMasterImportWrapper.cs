using DataContracts;
using InterfaceAPI.Models.BBU.VendorMasterImport;
using InterfaceAPI.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InterfaceAPI.BusinessWrapper.Interface.BBU
{
    public interface IVendorMasterImportWrapper : ICommonWrapper
    {
        ProcessLogModel CreateVMImport(List<VendorMasterImportJSONModel> vmImportModelList);
        ProcessLogModel ProcessVMImport();
        string DownloadToLocal(string fileName, string apiName);
        string DecryptFile(string fileName, string encryptFilePath, string apiName);
        string ArchiveFileWithDBValue(string SourceFilePath, string sourceFileName, string DestinationPath);
        void EncryptandUploadFile(string fileName, string apiName);
    }
}