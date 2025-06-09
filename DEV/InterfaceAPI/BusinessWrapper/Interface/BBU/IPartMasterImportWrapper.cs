using DataContracts;
using InterfaceAPI.Models.BBU.PartMasterImport;
using InterfaceAPI.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InterfaceAPI.BusinessWrapper.Interface.BBU
{
    public interface IPartMasterImportWrapper : ICommonWrapper
    {
        ProcessLogModel CreatePartMasterImport(List<PartMasterImportJSONModel> partMasterImportModelList);
        ProcessLogModel ProcessPartMasterImport();
        string DownloadToLocal(string fileName, string apiName);
        string DecryptFile(string fileName, string encryptFilePath, string apiName);
        string ArchiveFileWithDBValue(string SourceFilePath, string sourceFileName, string DestinationPath);
        void EncryptandUploadFile(string fileName, string apiName);
    }
}