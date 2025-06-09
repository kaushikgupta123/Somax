using InterfaceAPI.Models;
using InterfaceAPI.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace InterfaceAPI.BusinessWrapper.Interface
{
    public interface ICommonWrapper
    {

        /// <summary>
        /// Check the user's authenticity
        /// </summary>
        /// <param name="CurrentClaimsPrincipal">ClaimsPrincipal of caller user</param>
        /// <returns>true = if authentication is successfull. false = if failed to authenticate.</returns>
        bool IsUserAuthentiCate(ClaimsPrincipal CurrentClaimsPrincipal);

        /// <summary>
        /// Check if the Api is undermaintenance
        /// </summary>
        /// <returns>true = if in maintenace state. false = if api is available.</returns>
        bool IsMaintenanceGoingOn();

        /// <summary>
        /// Check if a particular Interface is enabled for the client 
        /// </summary>
        /// <param name="InterfaceType">The respective interface type.</param>
        /// <returns>true = if enable. false = if not enable.</returns>
        bool CheckIsActiveInterface(string InterfaceType);

        /// <summary>
        /// Creating log 
        /// </summary>
        /// <returns>LogId </returns>
        long CreateLog(string processName);

        /// <summary>
        /// Updating existing log 
        /// </summary>
        /// <returns>void </returns>
        void UpdateLog(long logID, int totalTransaction, int sucessfulTransaction, int failTransaction, string logMessage, string processName);

        [Obsolete]
        /// <summary>
        /// Send file to archive 
        /// </summary>
        /// <returns>void </returns>
        string SendToArchive(string fileName, string ftpUid, string ftpPwd, string type);

        /// <summary>
        /// Delete json files from local directory 
        /// </summary>
        /// <returns>void </returns>
        void DeleteJsonFiles(string type);

        /// <summary>
        /// Check Authentication by User Name and password
        /// </summary>
        /// <param name="Username">User Name</param>
        /// <param name="Password">Password</param>
        /// <returns>boolean value -- true if authenticated else false</returns>
        bool CheckAuthentication(string Username, string Password);

        /// <summary>
        /// Get File Names in a SFTP directory
        /// </summary>
        /// <param name="DirectoryPath">DirectoryPath</param>
        /// <returns>List of Files.</returns>
        List<string> GetFileNames(string DirectoryPath);// Get web.config credentials to retrieve files from SFTP

        /// <summary>
        /// Return Json String after reading file contains
        /// </summary>
        /// <param name="DirectoryPath">SFTP source path where file exists.</param>
        /// <returns>Returns file contains as Json String.</returns>
        // string GetJsonFromSFTPFile(string DirectoryPath, string fileName, string fileType);
        string GetJsonFromSFTPFile(string DirectoryPath, string fileName, FileTypeEnum fileType);

        /// <summary>
        /// Delete file from SFTP server.
        /// </summary>
        /// <param name="DirectoryPath">From where to delete</param>
        /// <param name="fileName">File to delete</param>
        void DeleteFilesFromSFTP(string DirectoryPath, string fileName);

        /// <summary>
        /// Archive files in SFTP.
        /// </summary>
        /// <param name="SourceFilePath">From Where to Archive.</param>
        /// <param name="sourceFileName">File to Archive</param>
        /// <param name="DestinationPath">Where to Archive.</param>
        /// <returns></returns>
        string ArchiveFile(string SourceFilePath, string sourceFileName, string DestinationPath);

        Int64 CreateUpdateExportLog(string processName, Int64 logId, string msg, int rowsExtracted);


        string EncryptFileRSA(string inputFilePath, string outputFilePath, string publicKey);
        string EncryptFilePGP(string inputFilePath, string outputFilePath, string publicKey);
        string DecryptFileRSA(string inputFilePath, string outputFilePath, string privateKey);
        string DecryptFilePGP(string inputFilePath, string outputFilePath, string privateKey);
        void DeleteLocalFiles(string filePath);

        List<string> GetFileList(string DirectoryPath);// Get db credentials to retrieve files from SFTP
        string DownloadFileToLocalDirectory(string fileName, string apiName);
        string ArchiveFileWithDBvalue(string SourceFilePath, string sourceFileName, string DestinationPath);
        void EncryptandUploadFileToSFTP(string fileName, string apiName);//---Testing upload
        void RetrieveInterfacePropValues(string interfaceType);
        void SendEMail(string userName, string body);
        //string GetJsonFromDownloadedSFTPFiles(string DirectoryPath, string fileName, FileTypeEnum fileType, string[] PropertyNames);
        string GetJsonFromDownloadedSFTPFiles(string DirectoryPath, string fileName, FileTypeEnum fileType);
    }
}