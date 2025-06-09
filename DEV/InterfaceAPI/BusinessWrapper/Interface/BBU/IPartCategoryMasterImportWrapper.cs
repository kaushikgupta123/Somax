using InterfaceAPI.Models.BBU.PartCategoryMasterImport;
using InterfaceAPI.Models.Common;

using System.Collections.Generic;

namespace InterfaceAPI.BusinessWrapper.Interface.BBU
{
    public interface IPartCategoryMasterImportWrapper : ICommonWrapper
    {
        ProcessLogModel CreatePartCategoryMasterImport(List<PartCategoryMasterImportJSONModel> partMasterImportModelList);
        ProcessLogModel ProcessPartCategoryMasterImport();
        string DownloadToLocal(string fileName, string apiName);
        string DecryptFile(string fileName, string encryptFilePath, string apiName);
        string ArchiveFileWithDBValue(string SourceFilePath, string sourceFileName, string DestinationPath);
        void EncryptandUploadFile(string fileName, string apiName);
    }
}