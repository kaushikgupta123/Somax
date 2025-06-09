using Business.Authentication;

using Common.Constants;
using Common.Enumerations;

using DataContracts;

using InterfaceAPI.BusinessWrapper.Interface;
using InterfaceAPI.Models.Common;

using Newtonsoft.Json;

using Presentation.Common;

using Renci.SshNet;
using Renci.SshNet.Sftp;

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Web;

namespace InterfaceAPI.BusinessWrapper.Implementation
{
    public class CommonWrapper : ICommonWrapper
    {
        public DatabaseKey _dbKey;
        public UserData _userData;
        string Host;
        string ftpUserId;
        string ftpPassWord;

        string userEmail
        {
            get; set;
        }
        public CommonWrapper(UserData userData)
        {
            _userData = userData;
            _dbKey = userData.DatabaseKey;
            Host = ConfigurationManager.AppSettings["SFTPHost"];
            ftpUserId = ConfigurationManager.AppSettings["SFTPuserid"];
            ftpPassWord = ConfigurationManager.AppSettings["SFTPpassword"];
        }
        public CommonWrapper(UserData userData, string interfaceType)
        {
            _userData = userData;
            _dbKey = userData.DatabaseKey;
            InterfaceProp iprop = new InterfaceProp();
            if (!string.IsNullOrEmpty(interfaceType))
            {
                iprop.InterfaceType = interfaceType;
                iprop.ClientId = _userData.DatabaseKey.Client.ClientId;
                iprop.SiteId = _userData.Site.SiteId;
                iprop.RetrieveInterfaceProperties(_userData.DatabaseKey);
            }

            if (string.IsNullOrEmpty(iprop.PublicKey) || string.IsNullOrEmpty(iprop.PrivateKey) || string.IsNullOrEmpty(iprop.KeyPass))
            {                
                InitiateKeysPGP(iprop);
            }

            sftpCreds(iprop);

        }
        public bool CheckIsActiveInterface(string InterfaceType)
        {
            bool IsActive = false;
            InterfaceProp iprop = new InterfaceProp();
            iprop.InterfaceType = InterfaceType;
            iprop.ClientId = _userData.DatabaseKey.Client.ClientId;
            iprop.SiteId = _userData.Site.SiteId;                     // V2-399
            iprop.CheckIsActive(_userData.DatabaseKey);
            if (iprop != null && iprop.InterfacePropId > 0)
            {
                IsActive = true;
            }
            return IsActive;
        }

        public void RetrieveInterfacePropValues(string interfaceType)
        {
            CommonWrapper cw = new CommonWrapper(_userData, interfaceType);
        }

        public void sftpCreds(InterfaceProp ip)
        {
            SFTPCred.ftpAddress = ip.FTPAddress;
            SFTPCred.ftpFileDirectory = ip.FTPFileDirectory;
            SFTPCred.ftpArcDirectory = ip.FTPArcDirectory;
            SFTPCred.ftpUserName = ip.FTPUserName;
            SFTPCred.ftpPassword = ip.FTPPassword;
            SFTPCred.isMailSend = ip.Switch1;
            SFTPCred.PublicKey = ip.PublicKey;
            SFTPCred.PrivateKey = ip.PrivateKey;
            SFTPCred.delimiter = ip.Delimiter;
            SFTPCred.filesEncrypted = ip.FilesEncrypted;
            SFTPCred.jSONSchemaDir = ip.JSONSchemaDir;
            SFTPCred.keyPass = ip.KeyPass;
            //RKL TEMPORARY - We Should store the port in the table 
            if (ip.FTPAddress == "40.77.48.114")
            {
                SFTPCred.port = 9822;
            }
            else
            {
                // assigned port number 22 as it is the default port for sftp
                SFTPCred.port = ip.FTPPort == 0 ? 22 : ip.FTPPort;
            }
        }
        public bool IsUserAuthentiCate(ClaimsPrincipal CurrentClaimsPrincipal)
        {
            Guid LogsessionId = new Guid(CurrentClaimsPrincipal.Claims.Where(c => c.Type == "LogInSessionId").Select(c => c.Value).SingleOrDefault()); //Flow 2 : Is SecurityToken Valid? NB : It is Checked Under [Authorised]
            DatabaseKey dbKey = Authentication.GetAdminOnlyKey();
            this._userData = new UserData() { SessionId = LogsessionId, WebSite = WebSiteEnum.Client };
            this._userData.Retrieve(dbKey);
            Authentication auth = new Authentication() { UserData = this._userData };
            auth.UserData.LoginAuditing.CreateDate = DateTime.Now;
            auth.VerifyCurrentUser();
            return auth.IsAuthenticated;
        }
        public bool IsMaintenanceGoingOn()
        {
            bool IsMaintenance;
            IsMaintenance = false;
            SiteMaintenance sm = new SiteMaintenance();
            sm.RetrieveOutage(_userData.DatabaseKey);

            if (sm != null && sm.SiteMaintenanceId > 0)
            {
                IsMaintenance = true;
            }
            return IsMaintenance;
        }
        public long CreateLog(string processName)
        {
            int newTransactions = 0;
            string logmessage = string.Empty;
            DateTime rundate = DateTime.UtcNow;
            long logID;

            ImportLog importLog = new ImportLog();
            importLog.ClientId = _userData.DatabaseKey.Client.ClientId;
            switch (processName)
            {
                case "poReceipt":
                    importLog.ProcessName = Common.Constants.ApiConstants.DeanFoodsPOReceiptImport;
                    break;
                case "poImport":
                    importLog.ProcessName = Common.Constants.ApiConstants.DeanFoodsPOImport;
                    break;
                case "vendorImport":
                    importLog.ProcessName = Common.Constants.ApiConstants.DeanFoodsVendorMasterImport;
                    break;
                case "accountImport":
                    importLog.ProcessName = Common.Constants.ApiConstants.DeanFoodsAccountImport;
                    break;
                default:
                    importLog.ProcessName = processName;
                    break;
            }
            //importLog.ProcessName = Common.Constants.ApiConstants.DeanFoodsVendorMasterImport;
            importLog.Transactions = 0;
            importLog.NewTransactions = newTransactions;
            importLog.SuccessfulTransactions = 0;
            importLog.Message = string.Empty;
            importLog.RunDate = rundate;
            importLog.RunBy = _userData.DatabaseKey.UserName;
            //importLog.FileName = de.filename;
            importLog.Create(_userData.DatabaseKey);
            logID = importLog.ImportLogId;
            return logID;
        }

        #region Create Update Export Log
        public long CreateUpdateExportLog(string processName, Int64 logId, string msg, int rowsExtracted = 0)
        {
            string logmessage = string.Empty;
            DateTime rundate = DateTime.UtcNow;
            long logID;
            ExportLog exportLog = new ExportLog();
            exportLog.ClientId = _userData.DatabaseKey.Client.ClientId;
            exportLog.ProcessName = processName;
            exportLog.RowsExtracted = rowsExtracted;
            exportLog.RunDate = System.DateTime.UtcNow;
            exportLog.RunBy = _userData.DatabaseKey.UserName;
            if (logId == 0)
            {
                exportLog.Create(_userData.DatabaseKey);
            }
            else
            {
                exportLog.ExportLogId = logId;
                exportLog.Message = msg;
                exportLog.Update(_userData.DatabaseKey);
            }
            logID = exportLog.ExportLogId;
            return logID;
        }
        #endregion


        public void UpdateLog(long logID, int totalTransaction, int sucessfulTransaction, int failTransaction, string logMessage, string processName)
        {
            ImportLog importLog = new ImportLog();
            importLog.ImportLogId = logID;
            importLog.Retrieve(_userData.DatabaseKey);
            importLog.Transactions = totalTransaction;
            importLog.SuccessfulTransactions = sucessfulTransaction;
            importLog.NewTransactions = sucessfulTransaction;
            importLog.FailedTransactions = failTransaction;
            importLog.CompleteDate = System.DateTime.UtcNow;
            importLog.RunBy = _userData.DatabaseKey.UserName;
            importLog.Message = logMessage;
            importLog.ProcessName = processName;
            importLog.Update(_userData.DatabaseKey);
        }

        #region FTTP
        public string SendToArchive(string fileName, string ftpUid, string ftpPwd, string type)
        {
            string ftpAddr = string.Empty;
            string ftpArchAddr = string.Empty;
            string errMsg = string.Empty;
            if (type.Equals("vendor"))
            {
                ftpAddr = System.Configuration.ConfigurationManager.AppSettings["VendorFTPforCSV"];
                ftpArchAddr = System.Configuration.ConfigurationManager.AppSettings["VendorFTPforArchive"];
            }
            else if (type.Equals("account"))
            {
                ftpAddr = System.Configuration.ConfigurationManager.AppSettings["AccountFTPforCSV"];
                ftpArchAddr = System.Configuration.ConfigurationManager.AppSettings["AccountFTPforArchive"];
            }
            else if (type.Equals(ApiConstants.PartMasterImport))
            {
                ftpAddr = SFTPCred.ftpAddress;
                ftpArchAddr = SFTPCred.ftpArcDirectory;
            }
            else if (type.Equals(ApiConstants.PartMasterRequestExport))
            {
                ftpAddr = SFTPCred.ftpAddress;
                ftpArchAddr = SFTPCred.ftpArcDirectory;
            }
            else
            {
                errMsg = "Unknown type.";
            }

            //--- move file to archive
            try
            {
                FtpWebRequest ftpReq = (FtpWebRequest)WebRequest.Create(ftpAddr + fileName);
                ftpReq.Credentials = new NetworkCredential(ftpUid, ftpPwd);
                ftpReq.Method = WebRequestMethods.Ftp.Rename;
                ftpReq.RenameTo = "../Archive/" + fileName;
                FtpWebResponse ftpResp = (FtpWebResponse)ftpReq.GetResponse();
                errMsg = RenameArchiveFile(fileName, ftpUid, ftpPwd, type);
            }
            catch (WebException e)
            {
                errMsg = ((FtpWebResponse)e.Response).StatusDescription;
            }
            return errMsg;
        }
        private string RenameArchiveFile(string fileName, string ftpUid, string ftpPwd, string type)
        {
            string ftpAddr = string.Empty;
            string ftpArchAddr = string.Empty;
            string errMsg = string.Empty;
            if (type.Equals("vendor"))
            {
                ftpAddr = System.Configuration.ConfigurationManager.AppSettings["VendorFTPforCSV"];
                ftpArchAddr = System.Configuration.ConfigurationManager.AppSettings["VendorFTPforArchive"];
            }
            else if (type.Equals("account"))
            {
                ftpAddr = System.Configuration.ConfigurationManager.AppSettings["AccountFTPforCSV"];
                ftpArchAddr = System.Configuration.ConfigurationManager.AppSettings["AccountFTPforArchive"];
            }
            else if (type.Equals(ApiConstants.PartMasterImport))
            {
                ftpAddr = SFTPCred.ftpAddress;
                ftpArchAddr = SFTPCred.ftpArcDirectory;
            }
            else if (type.Equals(ApiConstants.PartMasterRequestExport))
            {
                ftpAddr = SFTPCred.ftpAddress;
                ftpArchAddr = SFTPCred.ftpArcDirectory;
            }
            else
            {
                errMsg = "Unknown type.";
            }
            try
            {
                ftpArchAddr = ftpArchAddr + "/Archive/";
                FtpWebRequest ftpReq = (FtpWebRequest)WebRequest.Create(ftpArchAddr + fileName);
                ftpReq.Credentials = new NetworkCredential(ftpUid, ftpPwd);
                ftpReq.Method = WebRequestMethods.Ftp.Rename;
                //--- rename file in archive
                string[] fileFullName = fileName.Split('.');
                fileName = fileFullName[0];
                string currDateTime = DateTime.UtcNow.Ticks.ToString();
                fileName = fileFullName[0] + "_" + currDateTime + "." + fileFullName[1];
                ftpReq.RenameTo = fileName;
                FtpWebResponse ftpResp = (FtpWebResponse)ftpReq.GetResponse();
            }
            catch (WebException e)
            {
                errMsg = ((FtpWebResponse)e.Response).StatusDescription;
            }
            return errMsg;
        }
        public List<string> GetFileNames(string DirectoryPath)
        {
            List<string> files = new List<string>();
            using (SftpClient sftp = new SftpClient(Host, ftpUserId, ftpPassWord))
            {
                sftp.Connect();
                var filesInpath = sftp.ListDirectory(DirectoryPath);
                foreach (var item in filesInpath)
                {
                    if (item.IsRegularFile)
                    {
                        files.Add(item.Name);
                    }
                }
                sftp.Disconnect();
            }
            return files;
        }
        public List<string> GetFileList(string DirectoryPath)
        {
            List<string> files = new List<string>();
            using (SftpClient sftp = new SftpClient(SFTPCred.ftpAddress, SFTPCred.port, SFTPCred.ftpUserName, SFTPCred.ftpPassword))
            {
                sftp.Connect();
                var filesInpath = sftp.ListDirectory(SFTPCred.ftpFileDirectory);
                foreach (var item in filesInpath)
                {
                    if (item.IsRegularFile)
                    {
                        files.Add(item.Name);
                    }
                }
                sftp.Disconnect();
            }
            return files;
        }


        public string GetJsonFromSFTPFile(string DirectoryPath, string fileName, FileTypeEnum fileType)
        {
            string JString = string.Empty;
            string fileExtension = Path.GetExtension(fileName);
            string filetype = fileType.ToString();
            try
            {
                using (SftpClient sftp = new SftpClient(Host, ftpUserId, ftpPassWord))
                {
                    sftp.Connect();
                    Stream fileStream = sftp.OpenRead(Path.Combine(DirectoryPath, fileName));
                    using (var sr = new StreamReader(fileStream, true))
                    {
                        string headerline = sr.ReadLine();
                        string[] headers = null;
                        if (fileExtension.Contains(FileExtensionEnum.csv.ToString()))
                        {
                            headers = headerline.Split(',');
                        }
                        else if (fileExtension.Contains(FileExtensionEnum.txt.ToString()))
                        {
                            headers = headerline.Split('|');
                        }
                        DataTable dt = new DataTable();
                        foreach (string header in headers)
                        {
                            dt.Columns.Add(header);
                        }
                        while (!sr.EndOfStream)
                        {
                            string line = sr.ReadLine();
                            string[] rows = null;
                            if (fileExtension.Contains(FileExtensionEnum.csv.ToString()))
                            {
                                rows = line.Split(',');
                            }
                            else if (fileExtension.Contains(FileExtensionEnum.txt.ToString()))
                            {
                                rows = line.Split('|');
                            }
                            DataRow dr = dt.NewRow();
                            for (int i = 0; i < headers.Length; i++)
                            {
                                dr[i] = rows[i];
                            }
                            dt.Rows.Add(dr);
                        }
                        //----convert DataTable to Json
                        JString = JsonConvert.SerializeObject(dt);
                    }
                    sftp.Disconnect();
                }

            }
            catch (Exception e)
            {
                JString = StatusEnum.ErrorConvertingJson.ToString();// "Source file not found.";
            }
            return JString;
        }

        #region Part Master Import

        public string DownloadFileToLocalDirectory(string fileName, string apiName)
        {
            string JString = string.Empty;
            string fileExtension = Path.GetExtension(fileName);
            string DirectoryPath = SFTPCred.ftpFileDirectory;
            string outPathLocal = System.Configuration.ConfigurationManager.AppSettings["LocalImportDirectory"].ToString().Trim()
                     + System.Configuration.ConfigurationManager.AppSettings[apiName].ToString().Trim() + "/Encrypted";
            if (!Directory.Exists(outPathLocal))
            {
                DirectoryInfo di = Directory.CreateDirectory(HttpContext.Current.Server.MapPath(outPathLocal));
            }
            string cOutFile = Path.Combine(outPathLocal, fileName);
            string encryptPath = HttpContext.Current.Server.MapPath(cOutFile);// local path
            try
            {
                using (SftpClient sftp = new SftpClient(SFTPCred.ftpAddress, SFTPCred.port, SFTPCred.ftpUserName, SFTPCred.ftpPassword))
                {
                    sftp.Connect();
                    if (sftp.IsConnected)
                    {
                        using (var file = System.IO.File.OpenWrite(encryptPath))
                        {
                            sftp.DownloadFile(DirectoryPath + fileName, file);
                        }
                        sftp.Disconnect();
                        sftp.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return encryptPath;
        }

        public string DecryptLocalFile(string fileName, string encryptPath, string apiName)
        {
            string outPathLocal = System.Configuration.ConfigurationManager.AppSettings["LocalImportDirectory"].ToString().Trim()
                     + System.Configuration.ConfigurationManager.AppSettings[apiName].ToString().Trim() + "/Decrypted";
            if (!Directory.Exists(outPathLocal))
            {
                DirectoryInfo di = Directory.CreateDirectory(HttpContext.Current.Server.MapPath(outPathLocal));
            }
            // 2021-Aug-14
            // BBU/Oracle is sending .txt.pgp - convert the output to .txt
            // 2021-Aug-29 - sometimes we get .pgp, other times .pgp.txt 
            string cOutFile;
            if (fileName.Contains(".txt.pgp"))
            {
                cOutFile = Path.Combine(outPathLocal, fileName.Replace(".txt.pgp", ".txt"));
            }
            else
            {
                cOutFile = Path.Combine(outPathLocal, fileName.Replace(".pgp", ".txt"));
            }
            string decryptPath = HttpContext.Current.Server.MapPath(cOutFile);// local path
            if (SFTPCred.filesEncrypted == true)
            {
                DecryptFilePGP(encryptPath, decryptPath, SFTPCred.PrivateKey);
                return decryptPath;
            }
            else
            {
                return encryptPath;// downloaded path (will be un-encrypted)
            }


        }
        public string GetJsonFromDownloadedSFTPFiles(string DirectoryPath, string fileName, FileTypeEnum fileType)
    {
      string JString = string.Empty;
            string fileExtension = Path.GetExtension(DirectoryPath);
            string filetype = fileType.ToString();
            try
            {
                FileStream fsIn = new FileStream(DirectoryPath, FileMode.Open);
                using (var sr = new StreamReader(fsIn, true))
                {
                    string headerline = sr.ReadLine();
                    headerline = headerline.Replace("\t", "");
                    string[] headers = null;
                    if (fileExtension.Contains(FileExtensionEnum.csv.ToString()))
                    {
                        headers = headerline.Split(',');
                    }
                    else if (fileExtension.Contains(FileExtensionEnum.txt.ToString()))
                    {
                        string delimiter = SFTPCred.delimiter;
                        string[] splitter = { delimiter };
                        headers = headerline.Split(splitter, StringSplitOptions.None);
                        headers = headers.Select(a => a.Trim()).ToArray();
                    }
                    // RKL - 2022-12-03 
                    // I asked for this check to be put in
                    // However - some files from BBU are not matching up
                    // Will review and maked mods later
                    // Today I need to get this working
                    /*
                    if (PropertyNames != null && PropertyNames.Length > 0)
                    {
                        int cnt = PropertyNames.Where(x => headers.Any(y => y.Trim().Equals(x.Trim()))).ToList().Count();
                        //int cnt = PropertyNames.Where(x => headers.Contains(x.Trim())).ToList().Count();
                        if (PropertyNames.Length != cnt)
                        {
                            return StatusEnum.ColumnMismatch.ToString();
                        }
                    }
                    */
                    DataTable dt = new DataTable();
                    foreach (string header in headers)
                    {
                        dt.Columns.Add(header);
                    }
                    while (!sr.EndOfStream)
                    {
                        string line = sr.ReadLine();

                        string[] rows = null;
                        if (fileExtension.Contains(FileExtensionEnum.csv.ToString()))
                        {
                            rows = line.Split(',');
                        }
                        else if (fileExtension.Contains(FileExtensionEnum.txt.ToString()))
                        {
                            string delimiter = SFTPCred.delimiter;
                            string[] splitter = { delimiter };
                            rows = line.Split(splitter, StringSplitOptions.None);
                            rows = rows.Select(a => a.Trim()).ToArray();
                        }
                        DataRow dr = dt.NewRow();
                        for (int i = 0; i < headers.Length; i++)
                        {
                            dr[i] = rows[i];
                        }
                        dt.Rows.Add(dr);
                    }
                    //----convert DataTable to Json
                    JString = JsonConvert.SerializeObject(dt);
                }


            }
            catch (Exception e)
            {
                JString = StatusEnum.ErrorConvertingJson.ToString();// "Source file not found.";
            }

            return JString;
        }
        #endregion

        #region PartMasterRequest Export
        public string UploadPartMasterRequestExportToSFTP(string inputFile, string fileName, FileTypeEnum fileType)
        {
            string JString = string.Empty;
            string DirectoryPath = SFTPCred.ftpFileDirectory;
            string filePath = string.Empty;
            string outPathLocal = System.Configuration.ConfigurationManager.AppSettings["LocalExportDirectory"].ToString().Trim()
                     + System.Configuration.ConfigurationManager.AppSettings[ApiConstants.PartMasterRequestExport].ToString().Trim() + "/Encrypted";
            if (!Directory.Exists(outPathLocal))
            {
                DirectoryInfo di = Directory.CreateDirectory(HttpContext.Current.Server.MapPath(outPathLocal));
            }
            string cOutFile = Path.Combine(outPathLocal, fileName);
            string encryptPath = HttpContext.Current.Server.MapPath(cOutFile);// local path
            string inputPath = HttpContext.Current.Server.MapPath(inputFile);
            try
            {
                using (SftpClient sftp = new SftpClient(SFTPCred.ftpAddress, SFTPCred.port, SFTPCred.ftpUserName, SFTPCred.ftpPassword))
                {
                    sftp.Connect();
                    if (SFTPCred.filesEncrypted == true)
                    {
                        this.EncryptFilePGP(inputPath, encryptPath, SFTPCred.PublicKey);// encrypt local store
                    }
                    else
                    {
                        encryptPath = inputPath;
                    }

                    string SFTPUploadPath = Path.Combine(DirectoryPath, fileName);//sftp path
                    if (sftp.IsConnected)
                    {
                        using (FileStream fs = new FileStream(encryptPath, FileMode.Open))
                        {
                            sftp.UploadFile(fs, SFTPUploadPath);
                        }
                        sftp.Disconnect();
                        sftp.Dispose();
                        JString = SFTPUploadPath;
                    }
                }
                DeleteLocalFiles(Path.GetDirectoryName(inputPath));
                DeleteLocalFiles(Path.GetDirectoryName(encryptPath));
            }
            catch (Exception e)
            {
                JString = StatusEnum.ErrorConvertingJson.ToString();// "Source file not found.";
            }
            return JString;
        }
        #endregion

        #region PurchaseRequestRequest Export
        public string UploadPurchaseRequestExportToSFTP(string inputFile, string fileName, FileTypeEnum fileType)
        {
            string JString = string.Empty;
            string fileExtension = Path.GetExtension(fileName);
            string filetype = fileType.ToString();
            string DirectoryPath = SFTPCred.ftpFileDirectory;
            string filePath = string.Empty;
            string outPathLocal = System.Configuration.ConfigurationManager.AppSettings["LocalExportDirectory"].ToString().Trim()
                     + System.Configuration.ConfigurationManager.AppSettings[ApiConstants.OraclePurchaseRequestExport].ToString().Trim() + "/Encrypted";
            if (!Directory.Exists(outPathLocal))
            {
                DirectoryInfo di = Directory.CreateDirectory(HttpContext.Current.Server.MapPath(outPathLocal));
            }
            string cOutFile = Path.Combine(outPathLocal, fileName);
            string encryptPath = HttpContext.Current.Server.MapPath(cOutFile);// local path
            string inputPath = HttpContext.Current.Server.MapPath(inputFile);
            try
            {
                using (SftpClient sftp = new SftpClient(SFTPCred.ftpAddress, SFTPCred.port, SFTPCred.ftpUserName, SFTPCred.ftpPassword))
                {
                    sftp.Connect();
                    if (SFTPCred.filesEncrypted == true)
                    {
                        this.EncryptFilePGP(inputPath, encryptPath, SFTPCred.PublicKey);// encrypt local store
                    }
                    else
                    {
                        encryptPath = inputPath;
                    }

                    string SFTPUploadPath = Path.Combine(DirectoryPath, fileName);//sftp path
                    if (sftp.IsConnected)
                    {
                        using (FileStream fs = new FileStream(encryptPath, FileMode.Open))
                        {
                            sftp.UploadFile(fs, SFTPUploadPath);
                        }
                        sftp.Disconnect();
                        sftp.Dispose();
                        JString = SFTPUploadPath;
                    }
                }
                DeleteLocalFiles(Path.GetDirectoryName(inputPath));
                DeleteLocalFiles(Path.GetDirectoryName(encryptPath));
            }
            catch (Exception e)
            {
                JString = StatusEnum.ErrorConvertingJson.ToString();// "Source file not found.";
            }
            return JString;
        }
        #endregion
        /// <summary>
        /// RKL - Archive files sent to the POImport API
        /// </summary>
        /// <param name="DirectoryPath"></param>
        /// <param name="fileName"></param>
        public void ArchivePOImportFiles(string poimport)
        {
            using (SftpClient sftp = new SftpClient(Host, ftpUserId, ftpPassWord))
            {
                // Convert the string to a stream
                MemoryStream stream = new MemoryStream();
                var writer = new StreamWriter(stream);
                writer.Write(poimport);
                writer.Flush();
                stream.Position = 0;
                string file_name = "/var/somax/Dev/DEANFOODS_TEST/PurchOrd/" + "PO_Import_" + DateTime.UtcNow.ToString("yyyy-MM-dd-HH-mm-ss") + ".json";
                // Connect to the SFTP Server and upload the file
                sftp.Connect();
                sftp.UploadFile(stream, file_name);
            }
        }
        public void DeleteFilesFromSFTP(string DirectoryPath, string fileName)
        {
            using (SftpClient sftp = new SftpClient(Host, ftpUserId, ftpPassWord))
            {
                sftp.Connect();
                sftp.DeleteFile(Path.Combine(DirectoryPath, fileName));
                sftp.Disconnect();
            }
        }
        public string ArchiveFile(string SourceFilePath, string sourceFileName, string DestinationPath)
        {
            string archErr = string.Empty;
            try
            {
                using (SftpClient sftp = new SftpClient(Host, ftpUserId, ftpPassWord))
                {
                    sftp.Connect();
                    SftpFile file = sftp.Get(Path.Combine(SourceFilePath, sourceFileName));
                    string destinationFileName = DateTime.UtcNow.ToString("yyyy-MM-dd-HH-mm-ss") + "_" + file.Name;
                    string archivePath = DestinationPath + "/" + destinationFileName;
                    file.MoveTo(archivePath);
                    sftp.Disconnect();
                }
            }
            catch (Exception e)
            {
                archErr = e.Message;
            }
            return archErr;
        }
        public string ArchiveFileWithDBvalue(string SourceFilePath, string sourceFileName, string DestinationPath)
        {
            string archErr = string.Empty;
            try
            {
                using (SftpClient sftp = new SftpClient(SFTPCred.ftpAddress, SFTPCred.port, SFTPCred.ftpUserName, SFTPCred.ftpPassword))
                {
                    sftp.Connect();
                    SftpFile file = sftp.Get(Path.Combine(SourceFilePath, sourceFileName));
                    string destinationFileName = DateTime.UtcNow.ToString("yyyy-MM-dd-HH-mm-ss") + "_" + file.Name;
                    string archivePath = DestinationPath + "/" + destinationFileName;
                    file.MoveTo(archivePath);
                    sftp.Disconnect();
                }
            }
            catch (Exception e)
            {
                archErr = e.Message;
            }
            return archErr;
        }
        #endregion
        public void DeleteJsonFiles(string type)
        {
            string jsonPath = string.Empty;
            if (type.ToLower().Equals("vendor"))
            {
                jsonPath = HttpContext.Current.Server.MapPath("~/JsonSchema/Vendor/");
            }
            else if (type.ToLower().Equals("account"))
            {
                jsonPath = HttpContext.Current.Server.MapPath("~/JsonSchema/Account/");
            }
            else if (type.ToLower().Equals("partmasterimport"))
            {
                jsonPath = HttpContext.Current.Server.MapPath("~/JsonSchema/PartMasterImport/");
            }
            else if (type.ToLower().Equals("iotreading"))
            {
                jsonPath = HttpContext.Current.Server.MapPath("~/JsonSchema/IoTReading/");
            }
            else if (type.ToLower().Equals("monnitiotreading"))
            {
                jsonPath = HttpContext.Current.Server.MapPath("~/JsonSchema/MonnitIoTReading/");
            }
            else if (type.ToLower().Equals("epminvoiceimport"))
            {
                jsonPath = HttpContext.Current.Server.MapPath("~/JsonSchema/EPMInvoiceImport/");
            }
            System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(jsonPath);
            foreach (System.IO.FileInfo file in di.GetFiles())
            {
                file.Delete();
            }
        }


        #region Security

        #region Initiate Public/Private key
        public void InitiateKeys(InterfaceProp iprop)
        {
            WebSecurityAsymmetric.KeyPair kp = new WebSecurityAsymmetric.KeyPair();
            kp = WebSecurityAsymmetric.GenerateNewKeyPair(4096);
            iprop.PublicKey = kp.PublicKey;
            iprop.PrivateKey = kp.PrivateKey;
            iprop.Update(_userData.DatabaseKey);
        }
        public void InitiateKeysPGP(InterfaceProp iprop)
        {
            if (String.IsNullOrEmpty(iprop.KeyPass))
            {
                iprop.KeyPass = System.Configuration.ConfigurationManager.AppSettings["PGPKeyPass"].ToString();
            }
            if (iprop.InterfaceType == ApiConstants.PartMasterRequestExport || iprop.InterfaceType == ApiConstants.OraclePurchaseRequestExport)
            {
                LoadClientPublicKey(iprop);
            }
            else
            {
                WebSecurityPGP.GenerateKey(iprop.KeyPass);
                string fileContentPublic = "";
                string fileContentPrivate = "";
                using (var reader = new StreamReader(WebSecurityPGP.GetPublicFile()))
                {
                    fileContentPublic = reader.ReadToEnd();
                    iprop.PublicKey = fileContentPublic;
                }
                using (var reader = new StreamReader(WebSecurityPGP.GetPrivateFile()))
                {
                    fileContentPrivate = reader.ReadToEnd();
                    iprop.PrivateKey = fileContentPrivate;
                }
            }
            iprop.Update(_userData.DatabaseKey);
        }

        #endregion
        #region Load Public Key
        public void LoadClientPublicKey(InterfaceProp iprop)
        {
            string clientkey;
            /// Load the public key provided by a client 
            using (var reader = new StreamReader(WebSecurityPGP.GetClientFile()))
            {
                clientkey = reader.ReadToEnd();
                iprop.PublicKey = clientkey;
                iprop.PrivateKey = "NO PRIVATE KEY NEEDED";
            }
        }
        #endregion
        #region Encrypt File
        public string EncryptFileRSA(string inputFilePath, string outputFilePath, string publicKey)
        {
            WebSecurityAsymmetric.EncryptFile(inputFilePath, outputFilePath, publicKey);
            return outputFilePath;
        }
        public string EncryptFilePGP(string inputFilePath, string outputFilePath, string publicKey)
        {
            WebSecurityPGP.EncryptStreamFile(inputFilePath, outputFilePath, publicKey);
            return outputFilePath;
        }
        #endregion

        #region Decrypt File
        public string DecryptFileRSA(string inputFilePath, string outputFilePath, string privateKey)
        {
            WebSecurityAsymmetric.DecryptFile(inputFilePath, outputFilePath, privateKey);
            return outputFilePath;
        }
        public string DecryptFilePGP(string inputFilePath, string outputFilePath, string privateKey)
        {
            WebSecurityPGP.DecryptStreamFile(inputFilePath, outputFilePath, privateKey, SFTPCred.keyPass);
            return outputFilePath;
        }
        #endregion

        #endregion

        #region Delete local files
        public void DeleteLocalFiles(string filePath)
        {
            System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(filePath);

            foreach (System.IO.FileInfo file in di.GetFiles())
            {
                file.Delete();
                // RKL - 2021-Sep-07
                /*
                string cMoveDest = @"D:\clients\BBU\Oracle Cloud Project\Testing\Decrypted";
                if (Directory.Exists(cMoveDest) && file.Extension == ".txt")
                {
                  string cFileName = @"\" + file.Name;
                  if (file.Exists)
                  {
                    file.MoveTo(cMoveDest + cFileName);
                  }
                }
                else
                { 
                  file.Delete();
                }
                */
            }
        }
        #endregion

        #region Authentication
        public bool CheckAuthentication(string Username, string Password)
        {
            Authentication auth = new Authentication()
            {
                UserName = Username,
                Password = Password,
                website = WebSiteEnum.Client,
                BrowserInfo = HttpContext.Current.Request.Browser.Type + " " + HttpContext.Current.Request.Browser.Version,
                IpAddress = HttpContext.Current.Request.UserHostAddress
            };

            auth.VerifyLogin();
            if (auth.IsAuthenticated)
            {
                DatabaseKey dbKey = Authentication.GetAdminOnlyKey();
                _userData = new UserData() { SessionId = auth.SessionId, WebSite = WebSiteEnum.Client };
                _userData.Retrieve(dbKey);
                auth.UserData = this._userData;
                auth.UserData.LoginAuditing.CreateDate = DateTime.Now;
                auth.VerifyCurrentUser();
            }
            return auth.IsAuthenticated;
        }
        #endregion

        #region Send Mail
        public void SendEMail(string userName, string body)
        {
            string userEmail = _userData.DatabaseKey.User.Email;
            SendMailByUserId(userName, body, userEmail);
        }
        public void SendMailByUserId(string username, string Response, string email)
        {
            if (string.IsNullOrEmpty(email)) { return; }

            DatabaseKey dbKey = Authentication.GetAdminOnlyKey();

            DataContracts.LoginInfo LoginInfo = new LoginInfo() { UserName = username, Email = email };
            LoginInfo.RetrieveBySearchCriteria(dbKey);

            if (LoginInfo.LoginInfoId > 0)
            {
                try
                {
                    SendEmail(LoginInfo.UserName, LoginInfo.Email, dbKey, Response);

                }
                catch (Exception ex)
                {

                }
            }
        }
        private void SendEmail(string UserName, string EmailAddress, DatabaseKey dbKey, string Response, string Subject = "SOMAX Interface Notification")
        {
            string strScheme = HttpContext.Current.Request.Url.Scheme;//return http or https
            string strAuthority = HttpContext.Current.Request.Url.Authority;// host name localhost:80   

            string LoginUrl = string.Format("{0}://{1}", strScheme, strAuthority);
            StringBuilder body = new StringBuilder();
            body.Append(Response);
            EmailModule email = new Presentation.Common.EmailModule() { MailBody = body.ToString(), Subject = Subject };
            email.Recipients.Add(EmailAddress);
            try
            {
                email.SendEmail();
            }
            catch (Exception ex)
            {

            }
        }
        #endregion

        #region Testing for uploading files in SFTP
        public void EncryptandUploadFileToSFTP(string fileName, string apiName)
        {
            string JString = string.Empty;
            string DirectoryPath = SFTPCred.ftpFileDirectory;
            string inPathLocal = System.Configuration.ConfigurationManager.AppSettings["LocalImportDirectory"].ToString().Trim()
                     + System.Configuration.ConfigurationManager.AppSettings[apiName].ToString().Trim() + "/Decrypted";
            string outPathLocal = System.Configuration.ConfigurationManager.AppSettings["LocalImportDirectory"].ToString().Trim()
                    + System.Configuration.ConfigurationManager.AppSettings[apiName].ToString().Trim() + "/Encrypted";
            if (!Directory.Exists(outPathLocal))
            {
                DirectoryInfo di = Directory.CreateDirectory(HttpContext.Current.Server.MapPath(outPathLocal));
            }
            if (!Directory.Exists(inPathLocal))
            {
                DirectoryInfo di = Directory.CreateDirectory(HttpContext.Current.Server.MapPath(inPathLocal));
            }
            string filename = fileName;//---used for testing upload from previous V1 example--//
            //string filename = "SOMAXITEM-DAT-BBU0329_New.txt";
            //string filename = "01_Sample Data_Parts Master Interface.txt";//---used for testing upload from previous V1 example--//
            string inputFilePath = Path.Combine(inPathLocal, filename);
            string outputFilePath = Path.Combine(outPathLocal, filename);
            inputFilePath = HttpContext.Current.Server.MapPath(inputFilePath);
            outputFilePath = HttpContext.Current.Server.MapPath(outputFilePath.Replace(".txt", ".pgp"));
            if (File.Exists(inputFilePath))
            {
                try
                {
                    if (SFTPCred.filesEncrypted == true)
                    {
                        WebSecurityPGP.EncryptStreamFile(inputFilePath, outputFilePath, SFTPCred.PublicKey);
                    }
                    else
                    {
                        outputFilePath = inputFilePath;
                    }

                    using (SftpClient sftp = new SftpClient(SFTPCred.ftpAddress, SFTPCred.port, SFTPCred.ftpUserName, SFTPCred.ftpPassword))
                    {
                        sftp.Connect();

                        string SFTPUploadPath = Path.Combine(SFTPCred.ftpFileDirectory, filename.Replace(".txt", ".pgp"));//sftp path
                        if (sftp.IsConnected)
                        {
                            using (FileStream fs = new FileStream(outputFilePath, FileMode.Open))
                            {
                                sftp.UploadFile(fs, SFTPUploadPath);
                            }
                            sftp.Disconnect();
                            sftp.Dispose();
                        }
                    }
                    DeleteLocalFiles(Path.GetDirectoryName(inputFilePath));
                    DeleteLocalFiles(Path.GetDirectoryName(outputFilePath));

                }
                catch (Exception e)
                {
                    JString = StatusEnum.ErrorConvertingJson.ToString();// "Source file not found.";
                }
            }
            else
            {
                JString = StatusEnum.ErrorConvertingJson.ToString();// "Source file not found.";
            }

        }
        public string GetHostedUrl()
        {
            string resetUrl;
            int iPort = HttpContext.Current.Request.Url.Port;
            if (iPort != 443 && iPort != 80)
            {
                string[] url = new string[3];
                url[0] = HttpContext.Current.Request.Url.Host;
                url[1] = HttpContext.Current.Request.Url.Port.ToString();
                resetUrl = string.Format(HttpContext.Current.Request.Url.Scheme + "://{0}:{1}", url);
            }
            else
            {
                string[] url = new string[2];
                url[0] = HttpContext.Current.Request.Url.Host;
                resetUrl = string.Format(HttpContext.Current.Request.Url.Scheme + "://{0}", url);
            }
            return resetUrl;
        }
        #endregion


        #region V2-1079 EDIPurchaseOrderExport Export
        public string UploadToSFTP(string inputFile, string fileName, FileTypeEnum fileType, string ExportType)
        {
            string JString = string.Empty;
            string fileExtension = Path.GetExtension(fileName);
            string filetype = fileType.ToString();
            string DirectoryPath = SFTPCred.ftpFileDirectory;
            string filePath = string.Empty;
            string outPathLocal = ConfigurationManager.AppSettings["LocalExportDirectory"].ToString().Trim()
                     + ConfigurationManager.AppSettings[ExportType].ToString().Trim() + "/Encrypted";
            if (!Directory.Exists(outPathLocal))
            {
                Directory.CreateDirectory(HttpContext.Current.Server.MapPath(outPathLocal));
            }
            string cOutFile = Path.Combine(outPathLocal, fileName);
            string encryptPath = HttpContext.Current.Server.MapPath(cOutFile);// local path
            string inputPath = HttpContext.Current.Server.MapPath(inputFile);
            try
            {
                using (SftpClient sftp = new SftpClient(SFTPCred.ftpAddress, SFTPCred.port, SFTPCred.ftpUserName, SFTPCred.ftpPassword))
                {
                    sftp.Connect();
                    if (SFTPCred.filesEncrypted == true)
                    {
                        this.EncryptFilePGP(inputPath, encryptPath, SFTPCred.PublicKey);// encrypt local store
                    }
                    else
                    {
                        encryptPath = inputPath;
                    }

                    string SFTPUploadPath = Path.Combine(DirectoryPath, fileName);//sftp path
                    if (sftp.IsConnected)
                    {
                        using (FileStream fs = new FileStream(encryptPath, FileMode.Open))
                        {
                            sftp.UploadFile(fs, SFTPUploadPath);
                        }
                        sftp.Disconnect();
                        sftp.Dispose();
                        JString = SFTPUploadPath;
                    }
                }
                DeleteLocalFiles(Path.GetDirectoryName(inputPath));
                DeleteLocalFiles(Path.GetDirectoryName(encryptPath));
            }
            catch (Exception e)
            {
                JString = StatusEnum.ErrorConvertingJson.ToString();// "Source file not found.";
            }
            return JString;
        }
        #endregion
    }
    public static class SFTPCred
    {
        public static string ftpAddress { get; set; }
        public static string ftpUserName { get; set; }
        public static string ftpPassword { get; set; }
        public static string ftpFileDirectory { get; set; }
        public static string ftpArcDirectory { get; set; }
        public static bool filesEncrypted { get; set; }
        public static string delimiter { get; set; }
        public static string jSONSchemaDir { get; set; }
        public static string PublicKey { get; set; }
        public static string PrivateKey { get; set; }
        public static bool isMailSend { get; set; }
        public static string keyPass { get; set; }
        // RKL - Need port 
        public static int port { get; set; }
    }
}