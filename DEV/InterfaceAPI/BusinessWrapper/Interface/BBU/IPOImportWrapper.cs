using DataContracts;
using InterfaceAPI.Models.BBU.POImport;
using InterfaceAPI.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Http.Controllers;

namespace InterfaceAPI.BusinessWrapper.Interface.BBU
{
    public interface IPOImportWrapper : ICommonWrapper
    {
        ProcessLogModel CreatePOImport(List<POImportJSONModel> poImportModelList);
        ProcessLogModel ProcessPOImport();
        string DownloadToLocal(string fileName, string apiName);
        string DecryptFile(string fileName, string encryptFilePath, string apiName);
        string ArchiveFileWithDBValue(string SourceFilePath, string sourceFileName, string DestinationPath);
        void EncryptandUploadFile(string fileName, string apiName);
        void SendAlert();
        void SendBBUEmails();
        void GenerateEvents();    // V2-612
        void SetEmailArray(long poit, string ponumber, string email);     // Testing Purposes
    }
}