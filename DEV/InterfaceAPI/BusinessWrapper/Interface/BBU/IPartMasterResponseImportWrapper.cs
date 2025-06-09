using DataContracts;
using InterfaceAPI.Models.BBU.PartMasterResponseImport;
using InterfaceAPI.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InterfaceAPI.BusinessWrapper.Interface.BBU
{
    public interface IPartMasterResponseImportWrapper : ICommonWrapper
    {
        ProcessLogModel CreatePartMasterResponseImport(List<PartMasterResponseImportJSONModel> partMasterResponseImportModelList, long LogId);
        ProcessLogModel ProcessPartMasterResponseImport();
        string DownloadToLocal(string fileName, string apiName);
        string DecryptFile(string fileName, string encryptFilePath, string apiName);
        string ArchiveFileWithDBValue(string SourceFilePath, string sourceFileName, string DestinationPath);
        void EncryptandUploadFile(string fileName, string apiName);
    }
}