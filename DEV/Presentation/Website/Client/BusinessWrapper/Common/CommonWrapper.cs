using AzureUtil;

using Client.Common;
using Client.Models;
using Client.Models.Common;
using Client.Models.Configuration.CategoryMaster;
using Client.Models.Meters;
using Client.Models.MultiStoreroomPart;
using Client.Models.Parts;
using Client.Models.PartsManagement.PartsManagementRequest;
using Client.Models.Personnel;
using Client.Models.SensorAlert;
using Client.Models.UIConfig;
using Common.Constants;
using Common.Enumerations;
using Data.DataContracts;
using DataContracts;
using DevExpress.Office.Utils;
using INTDataLayer.BAL;
using INTDataLayer.EL;

using Microsoft.WindowsAzure.Storage.Blob;

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

using Utility;

using static DevExpress.Data.Filtering.Helpers.SubExprHelper;

namespace Client.BusinessWrapper.Common
{
    public class CommonWrapper
    {
        public string myNetworkPath = string.Empty;
        private DatabaseKey _dbKey;
        private UserData userData; public CommonWrapper(UserData userData)
        {
            this.userData = userData;
            _dbKey = userData.DatabaseKey;
        }
        public string GetClientLogoUrl()
        {
            string image_URL = string.Empty;
            // Retrieve Logo Attachment Record
            DataContracts.Attachment attach = new DataContracts.Attachment()
            {
                ClientId = userData.DatabaseKey.Client.ClientId
            };
            attach.RetrieveLogo(userData.DatabaseKey, userData.Site.SiteId);
            if (!string.IsNullOrEmpty(attach.AttachmentURL))
            {
                bool ClientOnPremise = userData.DatabaseKey.Client.OnPremise;
                image_URL = attach.AttachmentURL;
                if (ClientOnPremise)
                {
                    image_URL = UtilityFunction.PhotoBase64ImgSrc(attach.AttachmentURL);
                }
                else
                {
                    AzureUtil.AzureSetup azure = new AzureUtil.AzureSetup();
                    image_URL = azure.GetSASUrlClientSite(userData.DatabaseKey.Client.ClientId, userData.DatabaseKey.User.DefaultSiteId, image_URL);
                }

            }
            else
            {
                ClientLogoEL ce = new ClientLogoEL();
                ce.ClientId = userData.DatabaseKey.Client.ClientId;
                ce.SiteId = 0;
                ce.Type = "Forms";
                DataTable dt = ClientLogoBL.GetLogoByType(ce, userData.DatabaseKey.ClientConnectionString);
                if (dt != null && dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["Image"] != null && dt.Rows[0]["Image"].ToString() != "")
                    {
                        byte[] ImageData = (byte[])dt.Rows[0]["Image"];
                        if (ImageData.Length > 0)
                        {
                            const string UploadDirectory = "../Images/DisplayImg/";
                            // RKL - 2024-Aug-23
                            //string imgName = "ClientLogoWO" + "." + "jpg";
                            string imgName = "ClientLogoWO" + ce.ClientId.ToString() + "." + "jpg";
                            string uploadDirectoryServerPath = System.Web.HttpContext.Current.Server.MapPath(UploadDirectory);
                            if (!Directory.Exists(uploadDirectoryServerPath))
                            {
                                Directory.CreateDirectory(uploadDirectoryServerPath);
                            }
                            string filePath = UploadDirectory + Path.GetFileName(imgName);
                            File.WriteAllBytes(System.Web.HttpContext.Current.Server.MapPath(filePath), ImageData);
                            image_URL = filePath;
                        }
                    }
                }
            }
            return image_URL;
        }

        public string GetAzureImageUrl(long objectId, string objectType)
        {
            string imageurl = string.Empty;
            bool lExternal = false;
            string filename = string.Empty;
            Attachment attach = new Attachment()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                ObjectName = objectType,
                ObjectId = objectId,
                Profile = true,
                Image = true
            };
            List<Attachment> AList = attach.RetrieveProfileAttachments(userData.DatabaseKey, userData.Site.TimeZone);
            if (AList.Count > 0) // Check Attachment Table count, If count exists
            {
                AList.OrderByDescending(a => a.AttachmentId);
                lExternal = AList.First().External;
                imageurl = AList.First().AttachmentURL;
                if (userData.DatabaseKey.Client.OnPremise)
                {
                    imageurl = imageurl;
                }
                else if (!lExternal && imageurl != "")// 1.If there is an AttachmentURL returned from an associated Attachment record and the External Flag is false
                {

                    AzureUtil.AzureSetup aset = new AzureUtil.AzureSetup();
                    if (objectType.ToLower() != "partmaster" && objectType.ToLower() != "client")
                    {
                        imageurl = aset.GetSASUrlClientSite(userData.DatabaseKey.Client.ClientId, userData.DatabaseKey.User.DefaultSiteId, imageurl);
                    }
                    else if (objectType.ToLower() == "partmaster" || objectType.ToLower() == "client")
                    {
                        imageurl = aset.GetSASUrlClient(userData.DatabaseKey.Client.ClientId, imageurl);//---SAS appended Url------

                    }
                }
                else if (lExternal && imageurl != "") // 2.If there is an AttachmentURL returned from an associated Attachment record and the External Flag is true
                {
                    imageurl = imageurl;
                }
                else
                {
                    imageurl = string.Empty;
                }
            }


            return imageurl;
        }

        public string GetOnPremiseImageUrl(long objectId, string objectType)
        {
            string imageurl = string.Empty;
            bool lExternal = false;
            string filename = string.Empty;
            Attachment attach = new Attachment()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                ObjectName = objectType,
                ObjectId = objectId,
                Profile = true,
                Image = true
            };
            List<Attachment> AList = attach.RetrieveProfileAttachments(userData.DatabaseKey, userData.Site.TimeZone);
            AList.OrderByDescending(a => a.AttachmentId);
            if (AList.Count > 0) // Check Attachment Table count, If count exists
            {
                lExternal = AList.First().External;
                imageurl = AList.First().AttachmentURL;
            }
            return imageurl;
        }

        public void DeleteAzureImage(long objectId, string objectName, ref string rtrMsg)
        { // Check if there is a profile image attachment record for the object 
            Attachment attach = new Attachment()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                ObjectName = objectName,
                ObjectId = objectId,
                Profile = true,
                Image = true
            };
            attach.ClientId = userData.DatabaseKey.Client.ClientId;
            List<Attachment> AList = attach.RetrieveProfileAttachments(userData.DatabaseKey, userData.Site.TimeZone);
            AList.OrderByDescending(a => a.AttachmentId);
            if (AList.Count > 0)
            {
                // Profile Image Attachment Record Exists
                string image_url = AList.First().AttachmentURL;
                bool external = AList.First().External;
                attach.AttachmentId = AList.First().AttachmentId;
                attach.Delete(userData.DatabaseKey);
                // If the image is NOT external then we delete the image 
                if (!external)
                {
                    AzureUtil.AzureSetup aset = new AzureUtil.AzureSetup();
                    aset.DeleteBlobByURL(image_url);
                    rtrMsg = "Success";
                }
                else
                {
                    rtrMsg = "External";
                }
            }
            else
            {
                rtrMsg = "Not Found";
            }
        }
        public void DeleteOnPremiseImage(long objectId, string objectName, ref string rtrMsg)
        { // Check if there is a profile image attachment record for the object 
            Attachment attach = new Attachment()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                ObjectName = objectName,
                ObjectId = objectId,
                Profile = true,
                Image = true
            };
            attach.ClientId = userData.DatabaseKey.Client.ClientId;
            List<Attachment> AList = attach.RetrieveProfileAttachments(userData.DatabaseKey, userData.Site.TimeZone);
            AList.OrderByDescending(a => a.AttachmentId);
            if (AList.Count > 0)
            {
                // Profile Image Attachment Record Exists
                string image_url = AList.First().AttachmentURL;
                bool external = AList.First().External;
                attach.AttachmentId = AList.First().AttachmentId;
                attach.Delete(userData.DatabaseKey);
                // If the image is NOT external then we delete the image 
                if (!external)
                {
                    string filePath = string.Empty;

                    int ConnectRemoteShareErrorCode = 0;
                    NetworkCredential credentials = UtilityFunction.GetOnPremiseCredential();

                    var OnPremisePath = UtilityFunction.GetOnPremiseDirectory();
                    using (new ConnectToSharedFolder(OnPremisePath.NetworkPath, credentials, out ConnectRemoteShareErrorCode))
                    {
                        if (ConnectRemoteShareErrorCode == 0)
                        {
                            filePath = Path.Combine(OnPremisePath.NetworkPath, OnPremisePath.RemoteDrivePath, image_url);
                            if (File.Exists(filePath))
                            {
                                File.Delete(filePath);
                                rtrMsg = "Success";
                            }
                            else
                            {
                                rtrMsg = "File does not exist.";
                            }
                        }
                        else
                        {
                            rtrMsg = "Please check your network path and credentials.";
                        }
                    }
                }
                else
                {
                    rtrMsg = "External";
                }
            }
            else
            {
                rtrMsg = "Not Found";
            }
        }
        #region Attachment
        public int AttachmentCount(long objectId, string tableName, bool security, string RequestType = "")
        {
            Attachment attInfo = new Attachment()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                ObjectName = tableName,
                ObjectId = objectId
            };
            var attachmentCount = attInfo.RetrieveAttachmentCount_V2(userData.DatabaseKey);
            return attachmentCount;
        }
        public List<Client.Models.AttachmentModel> PopulateAttachments(long objectId, string tableName, bool securityy, string RequestType = "")
        {
            Client.Models.AttachmentModel objAttachmentModel;
            List<DataContracts.Attachment> attchList = new List<Attachment>();
            List<Client.Models.AttachmentModel> AttachmentModelList = new List<Client.Models.AttachmentModel>();
            Attachment attInfo = new Attachment()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                ObjectName = tableName,
                ObjectId = objectId
            };
            if (!string.IsNullOrEmpty(RequestType) && RequestType == "woprint")
            {
                attchList = attInfo.RetrieveAllAttachmentsForObject(userData.DatabaseKey, userData.Site.TimeZone, true, securityy, RequestType);
            }
            else
            {
                attchList = attInfo.RetrieveAllAttachmentsForObject(userData.DatabaseKey, userData.Site.TimeZone, false, securityy);
            }
            if (attchList != null)
            {
                var attachmentAllowed = string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["AttachmentAllowedFileExtensions"]) ? "" : System.Configuration.ConfigurationManager.AppSettings["AttachmentAllowedFileExtensions"];
                string extension = string.Empty;
                foreach (var item in attchList)
                {
                    string extensionInFileName = string.Empty;
                    objAttachmentModel = new Client.Models.AttachmentModel();
                    string fName = item.FileName;
                    string fType = item.FileType.ToLower();
                    SanitizeFileName(ref fName, ref fType);
                    item.FileName = fName;
                    item.FileType = fType;
                    objAttachmentModel.FileAttachmentId = item.AttachmentId;
                    objAttachmentModel.Subject = item.Description;
                    objAttachmentModel.FileName = item.FileName;
                    objAttachmentModel.FullName = item.FullName;
                    objAttachmentModel.FileType = item.FileType;
                    objAttachmentModel.FileSize = item.FileSize;
                    objAttachmentModel.FileSizeWithUnit = Convert.ToString(item.FileSize);
                    objAttachmentModel.OwnerName = item.UploadedBy;
                    objAttachmentModel.CreateDate = item.CreateDate ?? DateTime.MinValue;
                    objAttachmentModel.RequestType = RequestType;
                    objAttachmentModel.PartMasterRequestId = objectId;
                    objAttachmentModel.AttachmentUrl = item.AttachmentURL;
                    objAttachmentModel.Description = item.Description;
                    objAttachmentModel.Image = item.Image;
                    objAttachmentModel.Profile = item.Profile;
                    objAttachmentModel.External = item.External;
                    objAttachmentModel.Reference = item.Reference;
                    objAttachmentModel.ContentType = item.ContentType;
                    objAttachmentModel.PrintwithForm = item.PrintwithForm;
                    string[] arrFileExt = attachmentAllowed.Split(',');
                    if (arrFileExt != null && arrFileExt.Any(x => fType.Equals(x.ToLower())))
                    {
                        AttachmentModelList.Add(objAttachmentModel);
                    }
                }
            }
            return AttachmentModelList;
        }

        public List<Client.Models.AttachmentModel> PopulatePdfPhotoFromPrint(long objectId, string tableName, bool securityy, string RequestType = "")
        {
            Client.Models.AttachmentModel objAttachmentModel;
            List<DataContracts.Attachment> attchList = new List<Attachment>();
            List<Client.Models.AttachmentModel> AttachmentModelList = new List<Client.Models.AttachmentModel>();
            Attachment attInfo = new Attachment()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                ObjectName = tableName,
                ObjectId = objectId
            };
            if (!string.IsNullOrEmpty(RequestType) && RequestType == "woprint")
            {
                attchList = attInfo.RetrieveAllAttachmentsForObject(userData.DatabaseKey, userData.Site.TimeZone, true, securityy, RequestType);
            }
            else
            {
                attchList = attInfo.RetrieveAllAttachmentsForObject(userData.DatabaseKey, userData.Site.TimeZone, false, securityy);
            }

            if (attchList != null)
            {
                foreach (var item in attchList)
                {
                    objAttachmentModel = new Client.Models.AttachmentModel();
                    //objAttachmentModel.FileInfoId = item.FileInfoId;//removed from table
                    objAttachmentModel.FileAttachmentId = item.AttachmentId;
                    objAttachmentModel.Subject = item.Description;
                    objAttachmentModel.FileName = item.FileName;
                    objAttachmentModel.FileSize = item.FileSize;
                    objAttachmentModel.FileSizeWithUnit = Convert.ToString(item.FileSize);
                    objAttachmentModel.FullName = item.FullName;
                    objAttachmentModel.OwnerName = item.UploadedBy;
                    objAttachmentModel.CreateDate = item.CreateDate ?? DateTime.MinValue;

                    objAttachmentModel.RequestType = RequestType;
                    objAttachmentModel.PartMasterRequestId = objectId;
                    objAttachmentModel.AttachmentUrl = item.AttachmentURL;
                    objAttachmentModel.Description = item.Description;
                    objAttachmentModel.Image = item.Image;
                    objAttachmentModel.Profile = item.Profile;
                    objAttachmentModel.External = item.External;
                    objAttachmentModel.Reference = item.Reference;
                    objAttachmentModel.ContentType = item.ContentType;
                    AttachmentModelList.Add(objAttachmentModel);
                }
            }
            return AttachmentModelList;
        }

        #region  Populate Attachment For Print V2-663
        public List<Client.Models.AttachmentModel> PopulateAttachmentForPrint(List<DataContracts.Attachment> attchList, long objectId, string RequestType)
        {
            Client.Models.AttachmentModel objAttachmentModel;
            List<Client.Models.AttachmentModel> AttachmentModelList = new List<Client.Models.AttachmentModel>();
            if (attchList != null)
            {
                foreach (var item in attchList)
                {
                    objAttachmentModel = new Client.Models.AttachmentModel();
                    //objAttachmentModel.FileInfoId = item.FileInfoId;//removed from table
                    objAttachmentModel.FileAttachmentId = item.AttachmentId;
                    objAttachmentModel.Subject = item.Description;
                    objAttachmentModel.FileName = item.FileName;
                    objAttachmentModel.FileSize = item.FileSize;
                    objAttachmentModel.FileSizeWithUnit = Convert.ToString(item.FileSize);
                    objAttachmentModel.FullName = item.FullName;
                    objAttachmentModel.OwnerName = item.UploadedBy;
                    objAttachmentModel.CreateDate = item.CreateDate ?? DateTime.MinValue;
                    objAttachmentModel.RequestType = RequestType;
                    objAttachmentModel.PartMasterRequestId = objectId;
                    objAttachmentModel.Description = item.Description;
                    objAttachmentModel.Image = item.Image;
                    if (userData.DatabaseKey.Client.OnPremise)
                    {
                        objAttachmentModel.AttachmentUrl = item.AttachmentURL;
                    }
                    else if (item.Image)
                    {
                        if (!item.External && item.AttachmentURL != "")// 1.If there is an AttachmentURL returned from an associated Attachment record and the External Flag is false
                        {

                            AzureUtil.AzureSetup aset = new AzureUtil.AzureSetup();
                            if (item.ObjectName.ToLower() != "partmaster")
                            {
                                objAttachmentModel.AttachmentUrl = aset.GetSASUrlClientSite(userData.DatabaseKey.Client.ClientId, userData.DatabaseKey.User.DefaultSiteId, item.AttachmentURL);
                            }
                            else if (item.ObjectName.ToLower() == "partmaster")
                            {
                                objAttachmentModel.AttachmentUrl = aset.GetSASUrlClient(userData.DatabaseKey.Client.ClientId, item.AttachmentURL);//---SAS appended Url------

                            }

                        }

                    }
                    else
                    {
                        objAttachmentModel.AttachmentUrl = item.AttachmentURL;
                    }
                    objAttachmentModel.Profile = item.Profile;
                    objAttachmentModel.External = item.External;
                    objAttachmentModel.Reference = item.Reference;
                    objAttachmentModel.ContentType = item.ContentType;
                    objAttachmentModel.PrintwithForm = item.PrintwithForm;
                    AttachmentModelList.Add(objAttachmentModel);
                }
            }
            return AttachmentModelList;
        }
        #endregion
        public DataContracts.Attachment DownloadAttachment(long _fileinfoId)
        {
            Attachment attach = new Attachment()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                AttachmentId = _fileinfoId
            };
            attach.Retrieve(userData.DatabaseKey);

            AzureSetup asetup = new AzureSetup();
            Uri attach_url = new Uri(attach.AttachmentURL);
            CloudBlockBlob cb = asetup.GetCloudBlockBlobURI(attach_url);
            if (cb.Exists())
            {
                string fName = attach.FileName;
                string fType = attach.FileType;
                SanitizeFileName(ref fName, ref fType);
                attach.FileName = fName;
                attach.FileType = fType;
                SharedAccessBlobPolicy policy = new SharedAccessBlobPolicy()
                {
                    Permissions = SharedAccessBlobPermissions.Read,
                    SharedAccessExpiryTime = DateTime.UtcNow.AddHours(1),
                };
                SharedAccessBlobHeaders headers = new SharedAccessBlobHeaders()
                {
                    ContentDisposition = string.Format("attachment;filename=\"{0}.{1}\"", attach.FileName, attach.FileType),
                    ContentType = attach.ContentType
                };
                var sasToken = cb.GetSharedAccessSignature(policy, headers);
                attach.AttachmentURL = cb.Uri.AbsoluteUri + sasToken;

            }
            return attach;
        }

        public bool IsOnpremiseCredentialValid()
        {
            int ConnectRemoteShareErrorCode = 0;
            NetworkCredential credentials = UtilityFunction.GetOnPremiseCredential();
            var OnPremisePath = UtilityFunction.GetOnPremiseDirectory();
            bool OnPremise = userData.DatabaseKey.Client.OnPremise;
            if (OnPremise)
            {
                using (new ConnectToSharedFolder(OnPremisePath.NetworkPath, credentials, out ConnectRemoteShareErrorCode))
                {
                    if (ConnectRemoteShareErrorCode == 0)
                    {
                        return true;
                    }
                }
            }
            else
            {
                return true;
            }
            return false;
        }
        public DataContracts.Attachment DownloadAttachmentOnPremise(long _fileinfoId)
        {
            int ConnectRemoteShareErrorCode = 0;
            Attachment attachment = new Attachment()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                AttachmentId = _fileinfoId
            };

            attachment.Retrieve(userData.DatabaseKey);
            string image_url = attachment.AttachmentURL;

            NetworkCredential credentials = UtilityFunction.GetOnPremiseCredential();
            var OnPremisePath = UtilityFunction.GetOnPremiseDirectory();

            using (new ConnectToSharedFolder(OnPremisePath.NetworkPath, credentials, out ConnectRemoteShareErrorCode))
            {
                if (ConnectRemoteShareErrorCode == 0)
                {
                    image_url = Path.Combine(OnPremisePath.NetworkPath, OnPremisePath.RemoteDrivePath, image_url);
                    if (File.Exists(image_url))
                    {
                        string fName = attachment.FileName;
                        string fType = attachment.FileType;
                        SanitizeFileName(ref fName, ref fType);
                        attachment.FileName = fName;
                        attachment.FileType = fType;
                        attachment.AttachmentURL = image_url;

                    }
                }
            }

            return attachment;
        }
        public Attachment AddAttachment(Client.Models.AttachmentModel objAttachment, Stream stream1, ref bool attachStatus, bool securityy)
        {
            Attachment attach = new Attachment();
            var attachmentAllowed = System.Configuration.ConfigurationManager.AppSettings["AttachmentAllowedFileExtensions"];
            if (!string.IsNullOrEmpty(attachmentAllowed))
            {
                var fileExtension = objAttachment.FileType.ToLower();
                string[] arrFileExt = attachmentAllowed.Split(',');
                if (arrFileExt != null && arrFileExt.Any(x => fileExtension.Equals(x.ToLower())))
                {
                    attachStatus = true;
                }
                if (attachStatus)
                {
                    MemoryStream ms = new MemoryStream();
                    stream1.CopyTo(ms);
                    byte[] uploadedFile = ms.ToArray();
                    string fileName = objAttachment.FileName;
                    string content_type = objAttachment.ContentType;
                    string fType = objAttachment.FileType;
                    SanitizeFileName(ref fileName, ref fType);
                    objAttachment.FileName = fileName;
                    objAttachment.FileType = fType;
                    long clientid = userData.DatabaseKey.Client.ClientId;
                    long siteid = userData.DatabaseKey.Personnel.SiteId;
                    string uri = string.Empty;
                    if (uploadedFile.Length > 1)
                    {
                        AzureUtil.AzureSetup aset = new AzureUtil.AzureSetup();
                        fileName = fileName.Replace(" ", "");
                        fileName = aset.CreateFileNamebyObject(objAttachment.TableName, objAttachment.ObjectId.ToString(), fileName);
                        uri = aset.UploadToAzureBlob(clientid, siteid, fileName, uploadedFile, content_type);
                        if (!string.IsNullOrEmpty(uri))
                        {
                            attach.ClientId = userData.DatabaseKey.Client.ClientId;
                            attach.ObjectName = objAttachment.TableName;
                            attach.ObjectId = objAttachment.ObjectId;
                            List<Attachment> Alist = attach.RetrieveAllAttachmentsForObject(userData.DatabaseKey, userData.Site.TimeZone, false, securityy);
                            if (Alist.Count > 0 && Alist.Any(att => att.FileName == objAttachment.FileName && att.FileType == objAttachment.FileType))
                            {
                                //after implemnentation need to comment some more code*************V2-725 not required to check duplicate here
                                //Attachment updattach = Alist.First((att => att.FileName == objAttachment.FileName && att.FileType == objAttachment.FileType));
                                //updattach.UploadedBy_PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
                                //updattach.ContentType = objAttachment.ContentType;
                                //updattach.FileSize = objAttachment.FileSize;
                                //updattach.AttachmentURL = uri;
                                //updattach.Update(userData.DatabaseKey);
                            }
                            else
                            {
                                attach.ObjectName = objAttachment.TableName;
                                attach.ObjectId = objAttachment.ObjectId;
                                attach.UploadedBy_PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
                                attach.Description = objAttachment.Subject;
                                attach.FileName = objAttachment.FileName;
                                attach.FileType = objAttachment.FileType;
                                attach.ContentType = objAttachment.ContentType;
                                attach.FileSize = objAttachment.FileSize;
                                attach.PrintwithForm = objAttachment.PrintwithForm;
                                attach.Image = false;
                                attach.Profile = false;
                                attach.External = false;
                                attach.Reference = false;
                                attach.AttachmentURL = uri;
                                attach.Create(userData.DatabaseKey);
                            }
                        }
                    }
                }
                return attach;
            }
            else
            {
                return null;
            }
        }


        public void CopyStream(Stream stream, string destPath)
        {
            using (var fileStream = new FileStream(destPath, FileMode.Create, FileAccess.Write))
            {
                stream.CopyTo(fileStream);
            }
        }
        public Attachment AddAttachmentOnPremise(Client.Models.AttachmentModel objAttachment, Stream stream, ref bool attachStatus, bool securityy)
        {
            Attachment attach = new Attachment();
            string Filepath = string.Empty;
            string UserName = string.Empty;
            string UserPassword = string.Empty;
            int ConnectRemoteShareErrorCode = 0;
            var attachmentAllowed = string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["AttachmentAllowedFileExtensions"]) ? "" : System.Configuration.ConfigurationManager.AppSettings["AttachmentAllowedFileExtensions"];

            NetworkCredential credentials = UtilityFunction.GetOnPremiseCredential();

            var OnPremisePath = UtilityFunction.GetOnPremiseDirectory();

            using (new ConnectToSharedFolder(OnPremisePath.NetworkPath, credentials, out ConnectRemoteShareErrorCode))
            {
                if (ConnectRemoteShareErrorCode == 0)
                {
                    Filepath = Path.Combine(OnPremisePath.NetworkPath, OnPremisePath.RemoteDrivePath);
                    if (!string.IsNullOrEmpty(attachmentAllowed))
                    {
                        var fileExtension = objAttachment.FileType.ToLower();
                        string[] arrFileExt = attachmentAllowed.Split(',');
                        if (arrFileExt != null && arrFileExt.Any(x => fileExtension.Equals(x.ToLower())))
                        {
                            attachStatus = true;
                        }
                        if (attachStatus)
                        {
                            string fileName = objAttachment.FileName;
                            string content_type = objAttachment.ContentType;

                            string fType = objAttachment.FileType;                           
                            SanitizeFileName(ref fileName, ref fType);
                            objAttachment.FileName = fileName;
                            objAttachment.FileType = fType;
                            objAttachment.PrintwithForm = objAttachment.PrintwithForm;
                            long clientid = userData.DatabaseKey.Client.ClientId;
                            long siteid = userData.DatabaseKey.Personnel.SiteId;
                            string uri = string.Empty;
                            string DBFilePath = string.Empty;
                            if (stream.Length > 1)
                            {
                                Filepath = UploadFileOnPremise(Filepath, objAttachment.ObjectId, objAttachment.TableName, out DBFilePath);
                                uri = Path.Combine(Filepath, fileName + "." + fType);
                                DBFilePath = Path.Combine(DBFilePath, fileName + "." + fType);
                                if (!string.IsNullOrEmpty(uri))
                                {

                                    CopyStream(stream, uri);
                                    attach.ClientId = userData.DatabaseKey.Client.ClientId;
                                    attach.ObjectName = objAttachment.TableName;
                                    attach.ObjectId = objAttachment.ObjectId;
                                    List<Attachment> Alist = attach.RetrieveAllAttachmentsForObject(userData.DatabaseKey, userData.Site.TimeZone, false, securityy);
                                    if (Alist.Count > 0 && Alist.Any(att => att.FileName == objAttachment.FileName && att.FileType == objAttachment.FileType))
                                    {
                                        //after implemnentation need to comment some more code*************V2-725
                                        //Attachment updattach = Alist.First((att => att.FileName == objAttachment.FileName && att.FileType == objAttachment.FileType));
                                        //updattach.UploadedBy_PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
                                        //updattach.ContentType = objAttachment.ContentType;
                                        //updattach.FileSize = objAttachment.FileSize;
                                        //updattach.AttachmentURL = DBFilePath;
                                        //updattach.Update(userData.DatabaseKey);
                                    }
                                    else
                                    {
                                        attach.ObjectName = objAttachment.TableName;
                                        attach.ObjectId = objAttachment.ObjectId;
                                        attach.UploadedBy_PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
                                        attach.Description = objAttachment.Subject;
                                        attach.FileName = objAttachment.FileName;
                                        attach.FileType = objAttachment.FileType;
                                        attach.ContentType = objAttachment.ContentType;
                                        attach.FileSize = objAttachment.FileSize;
                                        attach.PrintwithForm = objAttachment.PrintwithForm;
                                        attach.Image = false;
                                        attach.Profile = false;
                                        attach.External = false;
                                        attach.Reference = false;
                                        attach.AttachmentURL = DBFilePath;
                                        attach.Create(userData.DatabaseKey);
                                    }

                                }
                            }
                        }
                        return attach;
                    }
                    else
                    {
                        return null;
                    }

                }
            }
            if (ConnectRemoteShareErrorCode > 0)
            {
                // attachStatus = true;
                attach.ErrorMessages = new List<string>();
                attach.ErrorMessages.Add(UtilityFunction.GetMessageFromResource("globalNotAuthorisedUploadFile", LocalizeResourceSetConstants.Global));
            }
            return attach;
        }

        public bool DeleteAttachmentFromAzure(long _fileAttachmentId, long objectId)
        {
            try
            {
                Attachment attachment = new Attachment()
                {
                    ClientId = userData.DatabaseKey.Client.ClientId,
                    AttachmentId = Convert.ToInt64(_fileAttachmentId),
                    ObjectId = objectId,
                    ObjectName = AttachmentTableConstant.WorkOrder,
                    Profile = false,
                    Image = false
                };

                attachment.ClientId = userData.DatabaseKey.Client.ClientId;
                List<Attachment> AList = attachment.RetrieveProfileAttachments(userData.DatabaseKey, userData.Site.TimeZone);
                AList.OrderByDescending(a => a.AttachmentId);
                if (AList.Count > 0)
                {
                    // Profile Image Attachment Record Exists
                    string image_url = AList.First().AttachmentURL;
                    bool external = AList.First().External;
                    attachment.AttachmentId = AList.First().AttachmentId;
                    attachment.Delete(userData.DatabaseKey);
                    // If the image is NOT external then we delete the image 

                    AzureUtil.AzureSetup aset = new AzureUtil.AzureSetup();
                    aset.DeleteBlobByURL(image_url);
                    //rtrMsg = "Success";
                }
                return true;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool DeleteAttachment(long _fileAttachmentId)
        {
            try
            {
                Attachment attachment = new Attachment()
                {
                    ClientId = userData.DatabaseKey.Client.ClientId,
                    AttachmentId = Convert.ToInt64(_fileAttachmentId)
                };
                attachment.Retrieve(userData.DatabaseKey);
                string image_url = attachment.AttachmentURL;
                attachment.Delete(userData.DatabaseKey);
                AzureUtil.AzureSetup aset = new AzureUtil.AzureSetup();
                aset.DeleteBlobByURL(image_url);
                return true;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool EditAttachment(Client.Models.AttachmentModel objAttachment)
        {
            try
            {
                Attachment attachment = new Attachment()
                {
                    ClientId = userData.DatabaseKey.Client.ClientId,
                    AttachmentId = Convert.ToInt64(objAttachment.AttachmentId)
                };
                attachment.Retrieve(userData.DatabaseKey);
                attachment.Description = objAttachment.Description;
                attachment.PrintwithForm = objAttachment.PrintwithForm;
                attachment.Update(userData.DatabaseKey);              
                return true;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool DeleteAttachmentOnPremise(long _fileAttachmentId, ref string rtrMsg)
        {
            int ConnectRemoteShareErrorCode = 0;
            bool result = false;
            try
            {
                Attachment attachment = new Attachment()
                {
                    ClientId = userData.DatabaseKey.Client.ClientId,
                    AttachmentId = Convert.ToInt64(_fileAttachmentId)
                };
                attachment.Retrieve(userData.DatabaseKey);
                string image_url = attachment.AttachmentURL;

                NetworkCredential credentials = UtilityFunction.GetOnPremiseCredential();
                var OnPremisePath = UtilityFunction.GetOnPremiseDirectory();

                using (new ConnectToSharedFolder(OnPremisePath.NetworkPath, credentials, out ConnectRemoteShareErrorCode))
                {
                    if (ConnectRemoteShareErrorCode == 0)
                    {
                        image_url = Path.Combine(OnPremisePath.NetworkPath, OnPremisePath.RemoteDrivePath, image_url);
                        if (File.Exists(image_url))
                        {
                            File.Delete(image_url);
                        }
                        attachment.Delete(userData.DatabaseKey);
                        result = true;
                    }
                }
                if (ConnectRemoteShareErrorCode > 0)
                {
                    rtrMsg = UtilityFunction.GetMessageFromResource("globalNotAuthorisedDeleteFile", LocalizeResourceSetConstants.Global);
                }
                return result;
            }
            catch
            {
                throw;
            }
        }

        public List<AttachmentModel> PopulatePdfAttachments(long objectId)
        {
            List<AttachmentModel> AttachmentPdfModelList = new List<AttachmentModel>();
            List<AttachmentModel> WoAttachment = PopulateAttachments(objectId, "workorder", userData.Security.WorkOrders.Edit);
            if (WoAttachment != null && WoAttachment.Count > 0)
            {
                WoAttachment = WoAttachment.Where(x => x.ContentType.Contains("pdf")).ToList();
            }
            AttachmentModel objAttachmentPdfModel;
            foreach (var v in WoAttachment)
            {
                objAttachmentPdfModel = new AttachmentModel();
                objAttachmentPdfModel.AttachmentId = v.AttachmentId;
                objAttachmentPdfModel.Subject = v.Subject;
                objAttachmentPdfModel.AttachmentUrl = v.AttachmentUrl;
                objAttachmentPdfModel.ContentType = v.ContentType;
                objAttachmentPdfModel.FileSize = v.FileSize;
                objAttachmentPdfModel.FileName = v.FileName;
                AttachmentPdfModelList.Add(objAttachmentPdfModel);
            }

            return AttachmentPdfModelList;
        }

        #region Sas enabled url
        public List<AttachmentModel> GetSasImageUrlList(ref List<AttachmentModel> attachModel)
        {
            string imageurl = string.Empty;
            string sasToken = string.Empty;
            byte[] ImageData = new byte[0];

            if (attachModel != null && attachModel.Count > 0)
            {
                foreach (AttachmentModel am in attachModel)
                {
                    if (!am.External && am.AttachmentUrl != "")// 1.If there is an AttachmentURL returned from an associated Attachment record and the External Flag is false
                    {
                        AzureUtil.AzureSetup aset = new AzureUtil.AzureSetup();
                        imageurl = aset.GetSASUrlClientSite(userData.DatabaseKey.Client.ClientId, userData.DatabaseKey.User.DefaultSiteId, am.AttachmentUrl);//---SAS appended Url------
                        am.AttachmentUrl = imageurl;
                    }
                    else if (am.External && am.AttachmentUrl != "") // 2.If there is an AttachmentURL returned from an associated Attachment record and the External Flag is true
                    {
                        imageurl = am.AttachmentUrl;
                    }
                }
            }

            return attachModel;
        }
        public string GetSasAttachmentUrl(ref string AttachURL)
        {
            string attachurl = string.Empty;
            AzureUtil.AzureSetup aset = new AzureUtil.AzureSetup();
            attachurl = aset.GetSASUrlClientSite(userData.DatabaseKey.Client.ClientId, userData.DatabaseKey.User.DefaultSiteId, AttachURL);//---SAS appended Url------
            AttachURL = attachurl;
            return AttachURL;
        }
        public string GetSasAttachmentUrlClientForDevexpressPrint(ref string AttachURL)
        {
            string attachurl = string.Empty;
            AzureUtil.AzureSetup aset = new AzureUtil.AzureSetup();
            attachurl = aset.GetSASUrlClient(userData.DatabaseKey.Client.ClientId, AttachURL);//---SAS appended Url------
            AttachURL = attachurl;
            return AttachURL;
        }
        public string GetClientLogoUrlForDevExpressPrint()
        {
            string image_URL = string.Empty;
            // Retrieve Logo Attachment Record
            DataContracts.Attachment attach = new DataContracts.Attachment()
            {
                ClientId = userData.DatabaseKey.Client.ClientId
            };
            attach.RetrieveLogo(userData.DatabaseKey, userData.Site.SiteId);
            if (!string.IsNullOrEmpty(attach.AttachmentURL))
            {
                bool ClientOnPremise = userData.DatabaseKey.Client.OnPremise;
                image_URL = attach.AttachmentURL;
                if (ClientOnPremise)
                {
                    image_URL = UtilityFunction.Base64SrcDevexpress(attach.AttachmentURL);
                }
                else
                {
                    AzureUtil.AzureSetup azure = new AzureUtil.AzureSetup();
                    image_URL = azure.GetSASUrlClientSite(userData.DatabaseKey.Client.ClientId, userData.DatabaseKey.User.DefaultSiteId, image_URL);
                }

            }
            else
            {
                ClientLogoEL ce = new ClientLogoEL();
                ce.ClientId = userData.DatabaseKey.Client.ClientId;
                ce.SiteId = 0;
                ce.Type = "Forms";
                DataTable dt = ClientLogoBL.GetLogoByType(ce, userData.DatabaseKey.ClientConnectionString);
                if (dt != null && dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["Image"] != null && dt.Rows[0]["Image"].ToString() != "")
                    {
                        byte[] ImageData = (byte[])dt.Rows[0]["Image"];
                        if (ImageData.Length > 0)
                        {
                            const string UploadDirectory = "../Images/DisplayImg/";
                            string imgName = "ClientLogoWO" + "." + "jpg";
                            string uploadDirectoryServerPath = System.Web.HttpContext.Current.Server.MapPath(UploadDirectory);
                            if (!Directory.Exists(uploadDirectoryServerPath))
                            {
                                Directory.CreateDirectory(uploadDirectoryServerPath);
                            }
                            string filePath = UploadDirectory + Path.GetFileName(imgName);
                            File.WriteAllBytes(System.Web.HttpContext.Current.Server.MapPath(filePath), ImageData);
                            image_URL = filePath;
                        }
                    }
                }
            }
            return image_URL;
        }
        #endregion

        #endregion

        #region Note
        public List<NotesModel> PopulateNotes(long objectId, string tableName)
        {
            NotesModel objNotesModel;
            List<NotesModel> NotesModelList = new List<NotesModel>();
            Notes note = new Notes()
            {
                ObjectId = objectId,
                TableName = tableName
            };
            List<Notes> NotesList = note.RetrieveByObjectId(userData.DatabaseKey, userData.Site.TimeZone);
            if (NotesList != null)
            {
                foreach (var item in NotesList)
                {
                    objNotesModel = new NotesModel();
                    objNotesModel.NotesId = item.NotesId;
                    objNotesModel.Subject = item.Subject;
                    objNotesModel.OwnerName = item.OwnerName;
                    objNotesModel.ModifiedDate = item.ModifiedDate;
                    objNotesModel.ObjectId = item.ObjectId;
                    NotesModelList.Add(objNotesModel);
                }
            }
            return NotesModelList;
        }

        public List<Notes> PopulateNote(long objectId, string tableName)
        {
            Notes note = new Notes()
            {
                ObjectId = objectId,
                TableName = tableName
            };
            List<Notes> NotesList = note.RetrieveByObjectId(userData.DatabaseKey, userData.Site.TimeZone);
            return NotesList;
        }


        public List<string> AddOrUpdateNote(NotesModel notesModel, ref string Mode, string tableName, List<UserMentionDataModel.UserMentionData> objUserMentionData)
        {
            Notes notes = new Notes()
            {
                OwnerId = userData.DatabaseKey.User.UserInfoId,
                OwnerName = userData.DatabaseKey.User.FullName,
                Subject = notesModel.Subject ?? "No Subject",
                Content = notesModel.Content,
                Type = notesModel.Type,
                TableName = tableName,
                ObjectId = notesModel.ObjectId,//notesModel.WorkOrderId,
                UpdateIndex = notesModel.updatedindex,
                NotesId = notesModel.NotesId
            };
            if (notesModel.NotesId == 0)
            {
                Mode = "add";
                notes.Create(this.userData.DatabaseKey);
                ProcessAlert objAlert = new ProcessAlert(this.userData);

                List<long> nos = new List<long>() { notesModel.ObjectId };
                List<string> userNameList = new List<string>();
                List<long> userIds = new List<long>();
                var UserList = new List<Tuple<long, string, string>>();
                if (objUserMentionData != null && objUserMentionData.Count > 0)
                {
                    foreach (var item in objUserMentionData)
                    {
                        UserList.Add
                       (
                            Tuple.Create(Convert.ToInt64(item.userId), item.userName, item.emailId)
                      );
                    }
                    Task CreateAlertTask;
                    switch (tableName)
                    {
                        case ("WorkOrder"):
                            CreateAlertTask = Task.Factory.StartNew(() => objAlert.CreateAlert<DataContracts.WorkOrder>(AlertTypeEnum.WorkOrderCommentMention, nos, notes, UserList));
                            break;
                        case ("Equipment"):
                            CreateAlertTask = Task.Factory.StartNew(() => objAlert.CreateAlert<DataContracts.Equipment>(AlertTypeEnum.AssetCommentMention, nos, notes, UserList));
                            break;
                    }

                }
            }
            else
            {
                Mode = "update";
                notes.Update(this.userData.DatabaseKey);
            }

            return notes.ErrorMessages;
        }

        public List<string> AddNotes(NotesModel notesModel, string tableName)
        {
            Notes notes = new Notes()
            {
                OwnerId = userData.DatabaseKey.User.UserInfoId,
                OwnerName = userData.DatabaseKey.User.FullName,
                Subject = notesModel.Subject,
                Content = notesModel.Content,
                Type = notesModel.Type,
                TableName = tableName,
                ObjectId = notesModel.ObjectId,
                UpdateIndex = notesModel.updatedindex,
                NotesId = notesModel.NotesId
            };
            if (notesModel.NotesId == 0)
            {
                notes.Create(userData.DatabaseKey);
            }
            else
            {
                notes.ObjectId = notesModel.ObjectId;
                notes.Update(userData.DatabaseKey);
            }
            return notes.ErrorMessages;
        }

        public bool DeleteNotes(long _notesId)
        {
            try
            {
                Notes notes = new Notes()
                {
                    NotesId = _notesId
                };
                notes.Delete(userData.DatabaseKey);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public NotesModel EditNotes(long objectId, long notesId)
        {
            NotesModel objNotesModel = new Models.NotesModel();
            Notes note = new Notes() { NotesId = notesId };
            note.Retrieve(userData.DatabaseKey);
            objNotesModel.NotesId = note.NotesId;
            objNotesModel.updatedindex = note.UpdateIndex;
            objNotesModel.Subject = note.Subject;
            objNotesModel.Content = note.Content;
            objNotesModel.ObjectId = objectId;
            objNotesModel.updatedindex = note.UpdateIndex;
            return objNotesModel;
        }

        //---------for new comment section---------------


        #endregion

        #region LookUpList
        public List<DataContracts.LookupList> GetAllLookUpList()
        {
            List<DataContracts.LookupList> objLookUp = new Models.LookupList().RetrieveAll(userData.DatabaseKey);
            return objLookUp;
        }
        public List<PartXRefGridDataModel> PopulatePartIds(int pageNumber, int ResultsPerPage, string OrderColumn, string OrderDirection, string ClientLookupId, string Description, string UPCcode, string Manufacturer, string ManufacturerId, string StockType)
        {
            PartXRefGridDataModel objPartXRefGridDataModel;
            List<PartXRefGridDataModel> PartXRefGridDataModelList = new List<PartXRefGridDataModel>();

            DataContracts.LookupListResultSet.PartLookupListResultSet PartLookupList
                = new DataContracts.LookupListResultSet.PartLookupListResultSet();

            DataContracts.LookupListResultSet.PartLookupListTransactionParameters settings = new DataContracts.LookupListResultSet.PartLookupListTransactionParameters()
            {
                SiteId = userData.DatabaseKey.User.DefaultSiteId,
                PageNumber = pageNumber,
                ResultsPerPage = ResultsPerPage,
                OrderColumn = OrderColumn,
                OrderDirection = OrderDirection
            };

            settings.ClientLookupId = ClientLookupId;
            settings.Description = Description;
            settings.UPCCode = UPCcode;
            settings.Manufacturer = Manufacturer;
            settings.ManufacturerId = ManufacturerId;
            settings.StockType = StockType;
            PartLookupList.UpdateSettings(userData.DatabaseKey, settings);
            PartLookupList.RetrieveResults();

            foreach (var item in PartLookupList.Items)
            {
                objPartXRefGridDataModel = new PartXRefGridDataModel();
                objPartXRefGridDataModel.PartId = item.PartId;
                objPartXRefGridDataModel.ClientLookUpId = item.ClientLookupId;
                objPartXRefGridDataModel.Description = item.Description;
                objPartXRefGridDataModel.Manufacturer = item.Manufacturer;
                objPartXRefGridDataModel.ManufacturerID = item.ManufacturerId;
                objPartXRefGridDataModel.UPCcode = item.UPCCode;
                objPartXRefGridDataModel.StockType = item.StockType;
                objPartXRefGridDataModel.TotalCount = PartLookupList.Count;
                objPartXRefGridDataModel.IssueUnit = item.IssueUnit;//V2-1119
                PartXRefGridDataModelList.Add(objPartXRefGridDataModel);
            }
            return PartXRefGridDataModelList;
        }

        public List<VendorLookupModel> PopulateVendorIds(int pageNumber, int ResultsPerPage, string OrderColumn, string OrderDirection, string clientLookupId, string name)
        {
            VendorLookupModel objVendorLookupModel;
            List<VendorLookupModel> VendorLookupModelList = new List<VendorLookupModel>();
            DataContracts.LookupListResultSet.VendorLookupListResultSet VendorLookupList
                    = new DataContracts.LookupListResultSet.VendorLookupListResultSet();

            DataContracts.LookupListResultSet.VendorLookupListTransactionParameters settings = new DataContracts.LookupListResultSet.VendorLookupListTransactionParameters()
            {
                SiteId = userData.DatabaseKey.User.DefaultSiteId,//siteId.ToLong(),
                PageNumber = pageNumber,
                ResultsPerPage = ResultsPerPage,
                OrderColumn = OrderColumn,
                OrderDirection = OrderDirection
            };

            settings.ClientLookupId = clientLookupId;
            settings.Name = name;

            VendorLookupList.UpdateSettings(userData.DatabaseKey, settings);
            VendorLookupList.RetrieveResults_V2();

            foreach (var v in VendorLookupList.Items)
            {
                objVendorLookupModel = new VendorLookupModel();
                objVendorLookupModel.ClientLookupId = v.ClientLookupId;
                objVendorLookupModel.Name = v.Name;
                objVendorLookupModel.VendorID = v.VendorId;
                objVendorLookupModel.TotalCount = v.totalCount;
                VendorLookupModelList.Add(objVendorLookupModel);
            }
            return VendorLookupModelList;
        }

        public List<VendorLookupModel> PopulatePunchOutVendorIds(int pageNumber, int ResultsPerPage, string OrderColumn, string OrderDirection, string clientLookupId, string name, string addressCity = "", string addressState = "")
        {
            VendorLookupModel objVendorLookupModel;
            List<VendorLookupModel> VendorLookupModelList = new List<VendorLookupModel>();
            DataContracts.LookupListResultSet.PunchOutVendorInternalLookupListResultSet VendorLookupList
                    = new DataContracts.LookupListResultSet.PunchOutVendorInternalLookupListResultSet();

            DataContracts.LookupListResultSet.VendorLookupListTransactionParameters settings = new DataContracts.LookupListResultSet.VendorLookupListTransactionParameters()
            {
                SiteId = userData.DatabaseKey.User.DefaultSiteId,//siteId.ToLong(),
                PageNumber = pageNumber,
                ResultsPerPage = ResultsPerPage,
                OrderColumn = OrderColumn,
                OrderDirection = OrderDirection
            };

            settings.ClientLookupId = clientLookupId;
            settings.Name = name;
            settings.AddressCity = addressCity;
            settings.AddressState = addressState;

            VendorLookupList.UpdateSettings(userData.DatabaseKey, settings);
            VendorLookupList.RetrieveResults();

            foreach (var v in VendorLookupList.Items)
            {
                objVendorLookupModel = new VendorLookupModel();
                objVendorLookupModel.ClientLookupId = v.ClientLookupId;
                objVendorLookupModel.Name = v.Name;
                objVendorLookupModel.VendorID = v.VendorId;
                objVendorLookupModel.AddressCity = v.AddressCity;
                objVendorLookupModel.AddressState = v.AddressState;
                objVendorLookupModel.TotalCount = VendorLookupList.Count;
                VendorLookupModelList.Add(objVendorLookupModel);
            }
            return VendorLookupModelList;
        }
        public List<VendorLookupModel> PopulateInternalVendorIds(int pageNumber, int ResultsPerPage, string OrderColumn, string OrderDirection, string clientLookupId, string name)
        {
            VendorLookupModel objVendorLookupModel;
            List<VendorLookupModel> VendorLookupModelList = new List<VendorLookupModel>();
            DataContracts.LookupListResultSet.VendorInternalLookupListResultSet VendorLookupList
                    = new DataContracts.LookupListResultSet.VendorInternalLookupListResultSet();

            DataContracts.LookupListResultSet.VendorLookupListTransactionParameters settings = new DataContracts.LookupListResultSet.VendorLookupListTransactionParameters()
            {
                SiteId = userData.DatabaseKey.User.DefaultSiteId,//siteId.ToLong(),
                PageNumber = pageNumber,
                ResultsPerPage = ResultsPerPage,
                OrderColumn = OrderColumn,
                OrderDirection = OrderDirection
            };

            settings.ClientLookupId = clientLookupId;
            settings.Name = name;

            VendorLookupList.UpdateSettings(userData.DatabaseKey, settings);
            VendorLookupList.RetrieveResults();

            foreach (var v in VendorLookupList.Items)
            {
                objVendorLookupModel = new VendorLookupModel();
                objVendorLookupModel.ClientLookupId = v.ClientLookupId;
                objVendorLookupModel.Name = v.Name;
                objVendorLookupModel.VendorID = v.VendorId;
                objVendorLookupModel.TotalCount = VendorLookupList.Count;
                VendorLookupModelList.Add(objVendorLookupModel);
            }
            return VendorLookupModelList;
        }
        public List<AccountLookUpModel> PopulateAccountIds(int pageNumber, int ResultsPerPage, string OrderColumn, string OrderDirection, string clientLookupId, string name)
        {
            AccountLookUpModel objAccountLookupModel;
            List<AccountLookUpModel> AccountLookupModelList = new List<AccountLookUpModel>();
            DataContracts.LookupListResultSet.AccountLookupListResultSet AccountLookupList
                    = new DataContracts.LookupListResultSet.AccountLookupListResultSet();

            DataContracts.LookupListResultSet.AccountLookupListTransactionParameters settings = new DataContracts.LookupListResultSet.AccountLookupListTransactionParameters()
            {
                SiteId = userData.DatabaseKey.User.DefaultSiteId,//siteId.ToLong(),
                PageNumber = pageNumber,
                ResultsPerPage = ResultsPerPage,
                OrderColumn = OrderColumn,
                OrderDirection = OrderDirection
            };

            settings.ClientLookupId = clientLookupId;
            settings.Name = name;

            AccountLookupList.UpdateSettings(userData.DatabaseKey, settings);
            AccountLookupList.RetrieveResults();
            if (AccountLookupList.Items != null)
            {
                AccountLookupList.Items = AccountLookupList.Items.Where(x => x.InactiveFlag == false).ToList();
            }

            foreach (var v in AccountLookupList.Items)
            {

                objAccountLookupModel = new AccountLookUpModel();
                objAccountLookupModel.ClientLookupId = v.ClientLookupId;
                objAccountLookupModel.Name = v.Name;
                objAccountLookupModel.AccountId = v.AccountId;
                objAccountLookupModel.TotalCount = AccountLookupList.Count;
                AccountLookupModelList.Add(objAccountLookupModel);

            }
            return AccountLookupModelList;
        }

        public List<WorkOrderLookUpModel> PopulateWorkOrderIds(int pageNumber, int ResultsPerPage, string OrderColumn, string OrderDirection, string clientLookupId, string Description, string ChargeTo, string WorkAssigned, string Requestor, string Status)
        {
            WorkOrderLookUpModel objWorkOrderLookupModel;
            List<WorkOrderLookUpModel> WorkOrderLookupModelList = new List<WorkOrderLookUpModel>();
            DataContracts.LookupListResultSet.WorkOrderLookupListResultSet_V2 WorkOrderLookupList
                    = new DataContracts.LookupListResultSet.WorkOrderLookupListResultSet_V2();

            DataContracts.LookupListResultSet.WorkOrderLookupListTransactionParameters settings = new DataContracts.LookupListResultSet.WorkOrderLookupListTransactionParameters()
            {
                SiteId = userData.DatabaseKey.User.DefaultSiteId,
                PageNumber = pageNumber,
                ResultsPerPage = ResultsPerPage,
                OrderColumn = OrderColumn,
                OrderDirection = OrderDirection
            };

            settings.ClientLookupId = clientLookupId;
            settings.Description = Description;
            settings.ChargeTo_Name = ChargeTo;
            settings.WorkAssigned_Name = WorkAssigned;
            settings.Requestor_Name = Requestor;
            settings.Status = Status;

            WorkOrderLookupList.UpdateSettings(userData.DatabaseKey, settings);
            WorkOrderLookupList.RetrieveResults();
            // 2022-APR-21 - The Stored procedure limits the status values to Approved, Scheduled, Work Request, Awaiting Approval and Planning (Planning is new)
            // There is no reason to do this next Linq query
            // Since the stored procudure is only sending back a "page" of rows - this could cause strange issues if we further limit the returned result set
            // Therefore - I commented this out 
            //if (WorkOrderLookupList.Items != null)
            //{
            //    WorkOrderLookupList.Items = WorkOrderLookupList.Items.Where(x => x.Status == WorkOrderStatusConstants.Approved || x.Status == WorkOrderStatusConstants.Scheduled || x.Status == WorkOrderStatusConstants.WorkRequest || x.Status == WorkOrderStatusConstants.AwaitingApproval).ToList();
            //}
            foreach (var v in WorkOrderLookupList.Items)
            {

                objWorkOrderLookupModel = new WorkOrderLookUpModel();
                objWorkOrderLookupModel.ClientLookupId = v.ClientLookupId;
                objWorkOrderLookupModel.Description = string.IsNullOrEmpty(v.Description) ? "" : v.Description;
                objWorkOrderLookupModel.ChargeTo = string.IsNullOrEmpty(v.ChargeTo_Name) ? "" : v.ChargeTo_Name;
                objWorkOrderLookupModel.WorkAssigned = string.IsNullOrEmpty(v.WorkAssigned_Name) ? "" : v.WorkAssigned_Name;
                objWorkOrderLookupModel.Requestor = string.IsNullOrEmpty(v.Requestor_Name) ? "" : v.Requestor_Name;
                objWorkOrderLookupModel.Status = string.IsNullOrEmpty(v.Status) ? "" : v.Status;
                objWorkOrderLookupModel.TotalCount = WorkOrderLookupList.Count;
                objWorkOrderLookupModel.WorkOrderId = v.WorkOrderId;
                WorkOrderLookupModelList.Add(objWorkOrderLookupModel);


            }
            return WorkOrderLookupModelList;
        }

        public List<EquipmentLookupModel> PopulateEquipmentList(int pageNumber, int resultsPerPage, string orderColumn, string orderDirection, string clientLookupId, string name, string model, string type, string serialNumber, bool InactiveFlag)
        {
            DataContracts.LookupListResultSet.EquipmentLookupListResultSet EquipmentLookupList
                                = new DataContracts.LookupListResultSet.EquipmentLookupListResultSet();

            DataContracts.LookupListResultSet.EquipmentLookupListTransactionParameters settings = new DataContracts.LookupListResultSet.EquipmentLookupListTransactionParameters()
            {
                SiteId = userData.DatabaseKey.User.DefaultSiteId,//siteId.ToLong(),
                PageNumber = pageNumber,
                ResultsPerPage = resultsPerPage,
                OrderColumn = orderColumn,
                OrderDirection = orderDirection
            };

            settings.ClientLookupId = clientLookupId;
            settings.Name = name;
            settings.Model = model;
            settings.Type = type;
            settings.SerialNumber = serialNumber;

            EquipmentLookupList.UpdateSettings(userData.DatabaseKey, settings);
            EquipmentLookupList.RetrieveResults();

            List<EquipmentLookupModel> EquipmentLookupModelList = new List<EquipmentLookupModel>();
            EquipmentLookupModel objEquipmentLookupModel;
            if (!InactiveFlag && EquipmentLookupList.Items != null)
            {
                EquipmentLookupList.Items = EquipmentLookupList.Items.Where(x => x.InactiveFlag == false).ToList();
            }
            foreach (var v in EquipmentLookupList.Items)
            {

                objEquipmentLookupModel = new EquipmentLookupModel();
                objEquipmentLookupModel.ClientLookupId = v.ClientLookupId;
                objEquipmentLookupModel.Name = v.Name;
                objEquipmentLookupModel.Model = v.Model;
                objEquipmentLookupModel.Type = v.Type;
                objEquipmentLookupModel.SerialNumber = v.SerialNumber;
                objEquipmentLookupModel.TotalCount = EquipmentLookupList.Count;
                objEquipmentLookupModel.EquipmentId = v.EquipmentId;
                EquipmentLookupModelList.Add(objEquipmentLookupModel);
            }
            return EquipmentLookupModelList;
        }
        public List<Account> AccountList()
        {
            Account acct = new Account()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                SiteId = userData.Site.SiteId
            };
            List<Account> laborList = acct.RetrieveClientLookupIdBySearchCriteria(userData.DatabaseKey);
            return laborList;
        }
        public List<Account> AccountList(long siteid)
        {
            Account acct = new Account()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                SiteId = siteid
            };
            List<Account> laborList = acct.RetrieveClientLookupIdBySearchCriteria(userData.DatabaseKey);
            return laborList;
        }

        public List<PartManagementGridDataModel> PopulatePartMasterIds(int pageNumber, int ResultsPerPage, string OrderColumn, string OrderDirection, string ClientLookupId, string LongDescription, string RequestType)
        {
            PartManagementGridDataModel objPartManagementGridDataModel;
            List<PartManagementGridDataModel> PartManagementGridDataModelList = new List<PartManagementGridDataModel>();

            DataContracts.LookupListResultSet.PartMasterNumberResultSet PartMasterNumberLookupList
                = new DataContracts.LookupListResultSet.PartMasterNumberResultSet();

            DataContracts.LookupListResultSet.PartMasterNumberTransactionParameters settings = new DataContracts.LookupListResultSet.PartMasterNumberTransactionParameters()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                SiteId = userData.DatabaseKey.User.DefaultSiteId,//siteId.ToLong(),
                PageNumber = pageNumber,
                ResultsPerPage = ResultsPerPage,
                OrderColumn = OrderColumn,
                OrderDirection = OrderDirection
            };
            settings.ClientLookupId = ClientLookupId;
            settings.LongDescription = LongDescription;
            settings.RequestType = RequestType;


            PartMasterNumberLookupList.UpdateSettings(userData.DatabaseKey, settings);
            PartMasterNumberLookupList.RetrieveResults();

            foreach (PartMaster e in PartMasterNumberLookupList.Items)
            {
                objPartManagementGridDataModel = new PartManagementGridDataModel();
                objPartManagementGridDataModel.PartMasterId = e.PartMasterId;
                objPartManagementGridDataModel.ClientLookupId = e.ClientLookupId;
                objPartManagementGridDataModel.LongDescription = e.LongDescription;
                objPartManagementGridDataModel.TotalCount = PartMasterNumberLookupList.Count;
                PartManagementGridDataModelList.Add(objPartManagementGridDataModel);
            }

            return PartManagementGridDataModelList;
        }

        public List<PartManagementReplaceGridModel> PopulatePartReplacementIds(int pageNumber, int ResultsPerPage, string OrderColumn, string OrderDirection, string ClientLookupId, string Description, string RequestType)
        {
            PartManagementReplaceGridModel objPartManagementReplaceGridModel;
            List<PartManagementReplaceGridModel> PartManagementReplaceGridModelList = new List<PartManagementReplaceGridModel>();
            DataContracts.LookupListResultSet.PartMasterSomaxPartLookupResultSet SomaxPartLookupList
                = new DataContracts.LookupListResultSet.PartMasterSomaxPartLookupResultSet();

            DataContracts.LookupListResultSet.PartMasterSomaxPartLookupTransactionParameters settings = new DataContracts.LookupListResultSet.PartMasterSomaxPartLookupTransactionParameters()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                SiteId = userData.DatabaseKey.User.DefaultSiteId,//siteId.ToLong(),
                PageNumber = pageNumber,
                ResultsPerPage = ResultsPerPage,
                OrderColumn = OrderColumn,
                OrderDirection = OrderDirection
            };
            settings.ClientLookupId = ClientLookupId;
            settings.Description = Description;
            settings.RequestType = RequestType;

            SomaxPartLookupList.UpdateSettings(userData.DatabaseKey, settings);
            SomaxPartLookupList.RetrieveResults();

            foreach (PartMaster e in SomaxPartLookupList.Items)
            {
                objPartManagementReplaceGridModel = new PartManagementReplaceGridModel();
                objPartManagementReplaceGridModel.ClientLookupId = e.ClientLookupId;
                objPartManagementReplaceGridModel.Description = e.Description;
                objPartManagementReplaceGridModel.TotalCount = SomaxPartLookupList.Count;
                PartManagementReplaceGridModelList.Add(objPartManagementReplaceGridModel);
            }
            return PartManagementReplaceGridModelList;
        }

        public List<PartMgmtManufacGridModel> ManufacturerMasterLookupList(int pageNumber, int ResultsPerPage, string OrderColumn, string OrderDirection)
        {
            PartMgmtManufacGridModel objPartMgmtManufacGridModel;
            List<PartMgmtManufacGridModel> PartMgmtManufacGridModelList = new List<PartMgmtManufacGridModel>();

            DataContracts.LookupListResultSet.ManufactureLookupListResultSet ManufactureLookupList
                 = new DataContracts.LookupListResultSet.ManufactureLookupListResultSet();

            DataContracts.LookupListResultSet.ManufactureLookupListResultSetTransactionParameters settings = new DataContracts.LookupListResultSet.ManufactureLookupListResultSetTransactionParameters()
            {
                PageNumber = pageNumber,
                ResultsPerPage = ResultsPerPage,//this.UserData.DatabaseKey.User.ResultsPerPage,
                OrderColumn = OrderColumn,
                OrderDirection = OrderDirection
            };
            ManufactureLookupList.UpdateSettings(userData.DatabaseKey, settings);
            ManufactureLookupList.RetrieveResults();

            foreach (var v in ManufactureLookupList.Items)
            {
                objPartMgmtManufacGridModel = new PartMgmtManufacGridModel();
                objPartMgmtManufacGridModel.ManufacturerMasterId = v.ManufacturerMasterId;
                objPartMgmtManufacGridModel.ClientLookupId = v.ClientLookupId;
                objPartMgmtManufacGridModel.Name = v.Name;
                objPartMgmtManufacGridModel.TotalCount = ManufactureLookupList.Count;
                PartMgmtManufacGridModelList.Add(objPartMgmtManufacGridModel);
            }
            return PartMgmtManufacGridModelList;
        }

        public List<AssetPopupModel> PopulateAssetList()
        {
            Equipment obj = new Equipment()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                SiteId = userData.DatabaseKey.User.DefaultSiteId,
            };
            var dataList = obj.Equipment_GetAllDeptLineSys(this.userData.DatabaseKey);
            List<AssetPopupModel> AssetPopupModelList = new List<AssetPopupModel>();
            AssetPopupModel objAssetPopupModel;
            foreach (var v in dataList)
            {
                objAssetPopupModel = new AssetPopupModel();
                objAssetPopupModel.EquipmentId = v.EquipmentId;
                objAssetPopupModel.ClientLookUpId = v.ClientLookupId;
                objAssetPopupModel.Name = v.Name;
                objAssetPopupModel.AssetType = v.Type;
                objAssetPopupModel.DeptClientLookupId = v.DeptClientLookUpId;
                objAssetPopupModel.LineClientLookupId = v.LineClientLookUpId;
                objAssetPopupModel.SystemClientLookupId = v.SysClientLookUpId;
                AssetPopupModelList.Add(objAssetPopupModel);
            }
            return AssetPopupModelList;
        }



        #endregion

        #region Notification
        public List<NotificationModel> GetNotificationList(string NotificationType, ref int UnreadCount, ref int UnreadSelectedtabCount)
        {
            List<NotificationModel> NotificationModelList = new List<NotificationModel>();
            NotificationModel objNotificationModel;
            UnreadCount = 0;
            UnreadSelectedtabCount = 0;
            List<AlertComposite> CompositeAlertList = new List<AlertComposite>();
            CompositeAlertList = new AlertComposite().RetrieveAlertCompositeByFilterCriteria(this.userData.DatabaseKey, this.userData.Site.TimeZone);
            if (CompositeAlertList != null)
            {
                UnreadCount = CompositeAlertList.Where(x => x.IsRead == false).Count();
            }
            // newNotiCount = UnreadCount;
            if (NotificationType == "Maintenance")
            {
                CompositeAlertList = CompositeAlertList.Where(
                x => x.AlertName == NotificationNameConstants.WorkRequestApprovalNeeded ||
                x.AlertName == NotificationNameConstants.WorkRequestApproved ||
                x.AlertName == NotificationNameConstants.WorkRequestDenied ||
                x.AlertName == NotificationNameConstants.WorkOrderComplete ||
                x.AlertName == NotificationNameConstants.SensorReadingAlertWO ||
                x.AlertName == NotificationNameConstants.WorkOrderCommentMention ||
                x.AlertName == NotificationNameConstants.AssetCommentMention ||
                x.AlertName == NotificationNameConstants.WorkOrderAssign ||
                x.AlertName == NotificationNameConstants.WorkOrderApprovalNeeded ||
                x.AlertName == NotificationNameConstants.WOPlanCommentMention ||
                x.AlertName == NotificationNameConstants.ProjectCommentMention ||
                x.AlertName == NotificationNameConstants.WRApprovalRouting ||
                x.AlertName == NotificationNameConstants.KPIReOpened ||
                x.AlertName == NotificationNameConstants.KPISubmitted ||
                x.AlertName == NotificationNameConstants.WorkOrderPlanner
                ).ToList();
                if (CompositeAlertList != null)
                {
                    UnreadSelectedtabCount = CompositeAlertList.Where(x => x.IsRead == false).Count();
                }
                foreach (var item in CompositeAlertList)
                {
                    objNotificationModel = new NotificationModel();
                    objNotificationModel.AlertUserId = item.AlertUserId;
                    objNotificationModel.AlertsId = item.AlertsId;
                    objNotificationModel.HeadLine = item.HeadLine;
                    objNotificationModel.ActiveDate = item.ActiveDate;
                    objNotificationModel.AlertDefineId = item.AlertDefineId;
                    objNotificationModel.Details = item.Details;
                    objNotificationModel.IsRead = item.IsRead;
                    #region V2-1136 Work Order Hot Link From Notification
                    if (!string.IsNullOrEmpty(item.ObjectClientLookupId))
                    {
                        if (item.AlertName == NotificationNameConstants.WorkRequestApprovalNeeded ||
                            item.AlertName == NotificationNameConstants.WorkOrderAssign ||
                            item.AlertName == NotificationNameConstants.WorkOrderCommentMention ||
                            item.AlertName == NotificationNameConstants.WorkOrderComplete ||
                            item.AlertName == NotificationNameConstants.WorkOrderApprovalNeeded ||
                            item.AlertName == NotificationNameConstants.WorkRequestApproved ||
                            item.AlertName == NotificationNameConstants.WorkRequestDenied ||
                            item.AlertName == NotificationNameConstants.WRApprovalRouting)
                        {

                            string htmlTag = $"<a data-woid={item.ObjectId} data-alertname={item.AlertName} class='lnk_WorkoderdetailsFromNotification' href='javascript:void(0)'>{item.ObjectClientLookupId}</a>";
                            objNotificationModel.HeadLine = item.HeadLine.Replace(item.ObjectClientLookupId, htmlTag);
                        }
                        else if (item.AlertName == NotificationNameConstants.AssetCommentMention)
                        {

                            string htmlTag = $"<a data-equipmentid={item.ObjectId} data-alertname={item.AlertName} class='lnk_EquipmentdetailsFromNotification' href='javascript:void(0)'>{item.ObjectClientLookupId}</a>";
                            objNotificationModel.HeadLine = item.HeadLine.Replace(item.ObjectClientLookupId, htmlTag);
                        }
                    }
                    #endregion
                    objNotificationModel.NotificationType = "Maintenance";
                    NotificationModelList.Add(objNotificationModel);
                }
                if (UnreadSelectedtabCount > 0)
                {
                    AlertUser objAlertUser = new AlertUser();
                    objAlertUser.IsRead = true;
                    objAlertUser.UserId = this.userData.DatabaseKey.Personnel.PersonnelId;
                    objAlertUser.SelectedNotificationTab = "Maintenance";
                    objAlertUser.UpdateAlertByUserIdAndNotificationTab(this.userData.DatabaseKey);
                }
            }
            else if (NotificationType == "Inventory")
            {
                CompositeAlertList = CompositeAlertList.Where(
                    x => x.AlertName == NotificationNameConstants.PartMasterRequestApprovalNeeded ||
                    x.AlertName == NotificationNameConstants.PartMasterRequestApproved ||
                    x.AlertName == NotificationNameConstants.PartMasterRequestDenied ||
                    x.AlertName == NotificationNameConstants.PartMasterRequestReturned ||
                    x.AlertName == NotificationNameConstants.PartMasterRequestProcessed ||
                    x.AlertName == NotificationNameConstants.PartMasterRequestSiteApprovalNeeded ||
                      x.AlertName == NotificationNameConstants.PartMasterRequestSiteApproved ||
                    x.AlertName == NotificationNameConstants.PartMasterRequestSiteApprovalNeededLocal ||
                    x.AlertName == NotificationNameConstants.PartMasterRequestAdditionProcessed ||
                    x.AlertName == NotificationNameConstants.PartMasterRequestReplaceProcessed ||
                    x.AlertName == NotificationNameConstants.PartMasterRequestInactivationProcessed ||
                     x.AlertName == NotificationNameConstants.PartMasterRequestSXReplaceProcessed ||
                    x.AlertName == NotificationNameConstants.PartMasterRequestECO_ReplaceProcessed ||
                      x.AlertName == NotificationNameConstants.PartMasterRequestECO_SX_ReplaceProcessed ||
                    x.AlertName == NotificationNameConstants.PartTransferCreated ||
                    x.AlertName == NotificationNameConstants.PartTransferIssue ||
                    x.AlertName == NotificationNameConstants.PartTransferReceipt ||
                    x.AlertName == NotificationNameConstants.PartTransferDenied ||
                     x.AlertName == NotificationNameConstants.PartTransferCanceled ||
                    x.AlertName == NotificationNameConstants.PartTransferForceCompPend ||
                      x.AlertName == NotificationNameConstants.PartCommentMention ||
                    x.AlertName == NotificationNameConstants.MaterialRequestApprovalNeeded
                    ).OrderByDescending(x => x.ActiveDate).ToList();
                if (CompositeAlertList != null)
                {
                    UnreadSelectedtabCount = CompositeAlertList.Where(x => x.IsRead == false).Count();
                }

                foreach (var item in CompositeAlertList)
                {
                    objNotificationModel = new NotificationModel();
                    objNotificationModel.AlertUserId = item.AlertUserId;
                    objNotificationModel.AlertsId = item.AlertsId;
                    objNotificationModel.HeadLine = item.HeadLine;
                    objNotificationModel.ActiveDate = item.ActiveDate;
                    objNotificationModel.AlertDefineId = item.AlertDefineId;
                    objNotificationModel.Details = item.Details;
                    objNotificationModel.IsRead = item.IsRead;
                    objNotificationModel.NotificationType = "Inventory";
                    #region V2-1147
                    if (!string.IsNullOrEmpty(item.ObjectClientLookupId))
                    {

                        if (item.AlertName == NotificationNameConstants.PartCommentMention)
                        {

                            string htmlTag = $"<a data-partid={item.ObjectId} data-alertname={item.AlertName} class='lnk_PartdetailsFromNotification' href='javascript:void(0)'>{item.ObjectClientLookupId}</a>";
                            objNotificationModel.HeadLine = item.HeadLine.Replace(item.ObjectClientLookupId, htmlTag);
                        }
                    }
                    #endregion
                    NotificationModelList.Add(objNotificationModel);
                }
                if (UnreadSelectedtabCount > 0)
                {
                    AlertUser objAlertUser = new AlertUser();
                    objAlertUser.IsRead = true;
                    objAlertUser.UserId = this.userData.DatabaseKey.Personnel.PersonnelId;
                    objAlertUser.SelectedNotificationTab = "Inventory";
                    objAlertUser.UpdateAlertByUserIdAndNotificationTab(this.userData.DatabaseKey);
                }
            }
            else if (NotificationType == "Procurement")
            {
                CompositeAlertList = CompositeAlertList.Where(
                    x => x.AlertName == NotificationNameConstants.PurchaseRequestApprovalNeeded ||
                    x.AlertName == NotificationNameConstants.PurchaseRequestApproved ||
                    x.AlertName == NotificationNameConstants.PurchaseRequestConverted ||
                    x.AlertName == NotificationNameConstants.PurchaseOrderReceipt ||
                    x.AlertName == NotificationNameConstants.PurchaseRequestDenied ||
                    x.AlertName == NotificationNameConstants.PurchaseRequestReturned ||
                    x.AlertName == NotificationNameConstants.POEmailToVendor ||
                    x.AlertName == NotificationNameConstants.POImportedReviewRequired
                    ).OrderByDescending(x => x.ActiveDate).ToList();
                if (CompositeAlertList != null)
                {
                    UnreadSelectedtabCount = CompositeAlertList.Where(x => x.IsRead == false).Count();
                }
                foreach (var item in CompositeAlertList)
                {
                    objNotificationModel = new NotificationModel();
                    objNotificationModel.AlertUserId = item.AlertUserId;
                    objNotificationModel.AlertsId = item.AlertsId;
                    objNotificationModel.HeadLine = item.HeadLine;
                    objNotificationModel.ActiveDate = item.ActiveDate;
                    objNotificationModel.AlertDefineId = item.AlertDefineId;
                    objNotificationModel.Details = item.Details;
                    objNotificationModel.IsRead = item.IsRead;
                    objNotificationModel.NotificationType = "Procurement";
                    #region V2-1147
                    if (!string.IsNullOrEmpty(item.ObjectClientLookupId))
                    {
                        if (item.AlertName == NotificationNameConstants.PurchaseRequestApprovalNeeded 
                            || item.AlertName == NotificationNameConstants.PurchaseRequestApproved
                            || item.AlertName == NotificationNameConstants.PurchaseRequestConverted
                            || item.AlertName == NotificationNameConstants.PurchaseRequestDenied
                            || item.AlertName == NotificationNameConstants.PurchaseRequestReturned)
                        {

                            string htmlTag = $"<a data-prid={item.ObjectId} data-alertname={item.AlertName} class='lnk_PurchaseRequestdetailsFromNotification' href='javascript:void(0)'>{item.ObjectClientLookupId}</a>";
                            objNotificationModel.HeadLine = item.HeadLine.Replace(item.ObjectClientLookupId, htmlTag);
                        }
                       else if (item.AlertName == NotificationNameConstants.PurchaseOrderReceipt)
                        {

                            string htmlTag = $"<a data-polineitemid={item.ObjectId} data-alertname={item.AlertName} class='lnk_PurchaseOrderdetailsFromNotification' href='javascript:void(0)'>{item.ObjectClientLookupId}</a>";
                            objNotificationModel.HeadLine = item.HeadLine.Replace(item.ObjectClientLookupId, htmlTag);
                        }
                    }
                    #endregion
                    NotificationModelList.Add(objNotificationModel);
                }
                if (UnreadSelectedtabCount > 0)
                {
                    AlertUser objAlertUser = new AlertUser();
                    objAlertUser.IsRead = true;
                    objAlertUser.UserId = this.userData.DatabaseKey.Personnel.PersonnelId;
                    objAlertUser.SelectedNotificationTab = "Procurement";
                    objAlertUser.UpdateAlertByUserIdAndNotificationTab(this.userData.DatabaseKey);
                }
            }
            else if (NotificationType == "System")
            {
                CompositeAlertList = CompositeAlertList.Where(
                    x => x.AlertName == NotificationNameConstants.NewUserAdded ||
                    x.AlertName == NotificationNameConstants.ResetPassword

                    ).OrderByDescending(x => x.ActiveDate).ToList();
                if (CompositeAlertList != null)
                {
                    UnreadSelectedtabCount = CompositeAlertList.Where(x => x.IsRead == false).Count();
                }
                foreach (var item in CompositeAlertList)
                {
                    objNotificationModel = new NotificationModel();
                    objNotificationModel.AlertUserId = item.AlertUserId;
                    objNotificationModel.AlertsId = item.AlertsId;
                    objNotificationModel.HeadLine = item.HeadLine;
                    objNotificationModel.ActiveDate = item.ActiveDate;
                    objNotificationModel.AlertDefineId = item.AlertDefineId;
                    objNotificationModel.Details = item.Details;
                    objNotificationModel.IsRead = item.IsRead;
                    objNotificationModel.NotificationType = "System";
                    NotificationModelList.Add(objNotificationModel);
                }
                if (UnreadSelectedtabCount > 0)
                {
                    AlertUser objAlertUser = new AlertUser();
                    objAlertUser.IsRead = true;
                    objAlertUser.UserId = this.userData.DatabaseKey.Personnel.PersonnelId;
                    objAlertUser.SelectedNotificationTab = "System";
                    objAlertUser.UpdateAlertByUserIdAndNotificationTab(this.userData.DatabaseKey);
                }
            }
            else if (NotificationType == "APM")
            {
                CompositeAlertList = CompositeAlertList.Where(
                    x => x.AlertName == NotificationNameConstants.APMMeterEvent ||
                    x.AlertName == NotificationNameConstants.APMWarningEvent ||
                    x.AlertName == NotificationNameConstants.APMCriticalEvent
                    ).OrderByDescending(x => x.ActiveDate).ToList();
                if (CompositeAlertList != null)
                {
                    UnreadSelectedtabCount = CompositeAlertList.Where(x => x.IsRead == false).Count();
                }
                foreach (var item in CompositeAlertList)
                {
                    objNotificationModel = new NotificationModel();
                    objNotificationModel.AlertUserId = item.AlertUserId;
                    objNotificationModel.AlertsId = item.AlertsId;
                    objNotificationModel.HeadLine = item.HeadLine;
                    objNotificationModel.ActiveDate = item.ActiveDate;
                    objNotificationModel.AlertDefineId = item.AlertDefineId;
                    objNotificationModel.Details = item.Details;
                    objNotificationModel.IsRead = item.IsRead;
                    objNotificationModel.NotificationType = "APM";
                    NotificationModelList.Add(objNotificationModel);
                }
                if (UnreadSelectedtabCount > 0)
                {
                    AlertUser objAlertUser = new AlertUser();
                    objAlertUser.IsRead = true;
                    objAlertUser.UserId = this.userData.DatabaseKey.Personnel.PersonnelId;
                    objAlertUser.SelectedNotificationTab = "APM";
                    objAlertUser.UpdateAlertByUserIdAndNotificationTab(this.userData.DatabaseKey);
                }
            }
            return NotificationModelList;
        }
        public bool DeleteNotification(long AlertUserId)
        {
            try
            {
                AlertUser alertUser = new AlertUser()
                {
                    AlertUserId = AlertUserId
                };
                alertUser.Retrieve(userData.DatabaseKey);
                alertUser.IsDeleted = true;
                alertUser.Update(userData.DatabaseKey);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool ClearNotification(string Type)
        {
            try
            {
                AlertUser objAlertUser = new AlertUser();
                objAlertUser.ClientId = userData.DatabaseKey.Client.ClientId;
                objAlertUser.UserId = this.userData.DatabaseKey.Personnel.PersonnelId;
                objAlertUser.SelectedNotificationTab = Type;
                objAlertUser.UpdateAlertByNotificationType(this.userData.DatabaseKey);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int GetUnreadNotificationCount(out int ResultMaintenanceCount, out int ResultInventoryCount, out int ResultProcurementCount, out int ResultSystemCount)
        {
            ResultInventoryCount = 0;
            ResultInventoryCount = 0;
            ResultProcurementCount = 0;
            ResultSystemCount = 0;
            DataContracts.AlertComposite alertComposite = new DataContracts.AlertComposite();
            return alertComposite.RetrieveAlertCompositeByCount(this.userData.DatabaseKey, out ResultMaintenanceCount, out ResultInventoryCount, out ResultProcurementCount, out ResultSystemCount);
        }
        #endregion
        public List<Menu> GetEventInfoCountByStatus()
        {
            DataTable dt = new DataTable();
            DataColumn dcol1 = new DataColumn("TableName", typeof(String));
            dt.Columns.Add(dcol1);
            DataColumn dcol2 = new DataColumn("Status", typeof(String));
            dt.Columns.Add(dcol2);

            DataRow dr = dt.NewRow();
            dr[0] = "event";
            dr[1] = EventStatusConstants.Open;

            dt.Rows.Add(dr);

            DataRow dr1 = dt.NewRow();
            dr1[0] = "workorder";
            dr1[1] = WorkOrderStatusConstants.WorkRequest;

            dt.Rows.Add(dr1);


            DataContracts.Menu menu = new DataContracts.Menu()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                SiteId = userData.DatabaseKey.User.DefaultSiteId,
                StatusData = dt
            };

            List<Menu> count = menu.RetrieveMenuStatusList(userData.DatabaseKey);

            return count;
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

        public string GetNoImageUrl()
        {
            string noImageUrl = ConfigurationManager.AppSettings[WebConfigConstants.NoImageUrl];
            string hostedUrl = GetHostedUrl();
            return hostedUrl + noImageUrl;
        }

        public IEnumerable<SelectListItem> PopulateCustomQueryDisplay(string TableName, bool IsSelectAll = false)
        {

            List<KeyValuePair<string, string>> customList = new List<KeyValuePair<string, string>>();
            customList = CustomQueryDisplay.RetrieveQueryItemsByTableAndLanguage(userData.DatabaseKey, TableName, userData.Site.LocalizationLanguage, userData.Site.LocalizationCulture);
            if (IsSelectAll && customList.Count > 0)
            {
                customList.Insert(0, new KeyValuePair<string, string>("0", "--Select All--"));
            }
            return customList.Select(x => new SelectListItem { Text = x.Value, Value = x.Key }).OrderBy(x => x.Value);
        }
        public IEnumerable<SelectListItem> PopulateCustomQueryDisplayForDevice(string TableName, bool IsSelectAll = false)
        {

            List<KeyValuePair<string, string>> customList = new List<KeyValuePair<string, string>>();
            customList = CustomQueryDisplay.RetrieveQueryItemsByTableAndLanguage(userData.DatabaseKey, TableName, userData.Site.LocalizationLanguage, userData.Site.LocalizationCulture);
            return customList.Select(x => new SelectListItem { Text = x.Value, Value = x.Key }).OrderBy(x => x.Value);
        }
        private string SanitizeFileName(ref string FName, ref string FType)
        {
            string op = string.Empty;
            int flen = 0;
            int index = 0;
            string[] fNameArray = null;
            string fname = string.Empty;
            fname = FName;
            fname = fname.Replace("..", ".").Replace("..", ".").Replace("..", ".").Replace("..", ".");
            FType = FType.Replace(".", "");
            string[] stringSeparators = new string[] { FType };
            fNameArray = fname.Split(stringSeparators, StringSplitOptions.None);
            if (fNameArray[0].ToString() != "")
            {
                fname = fNameArray[0].ToString();
                flen = fname.Length;
                index = fname.LastIndexOf('.');
                if (index == (flen - 1))
                {
                    fname = fname.Remove((flen - 1), 1);
                }
                FName = fname;
            }
            else if (fname.ToString() != "")
            {
                flen = fname.Length;
                index = FName.LastIndexOf('.');
                if (index == (flen - 1))
                {
                    fname = fname.Remove((flen - 1), 1);
                }
                FName = fname;
            }

            //check if last index is dot

            return FName;
        }

        #region Text Search Implementation

        public List<MemorizeSearch> GetSearchOptionList(string tableName)
        {
            MemorizeSearch memorizeSearch = new MemorizeSearch()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                SiteId = userData.DatabaseKey.User.DefaultSiteId,
                ObjectName = tableName
            };

            var optionList = memorizeSearch.MemorizeSearchRetrieveForSearch(userData.DatabaseKey);
            return optionList;
        }

        public List<MemorizeSearch> ModifySearchOptionList(string tableName, string searchText, bool isClear)
        {
            MemorizeSearch memorizeSearch = new MemorizeSearch()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                SiteId = userData.DatabaseKey.User.DefaultSiteId,
                ObjectName = tableName,
                SearchText = searchText,
                IsClear = isClear
            };

            var optionList = memorizeSearch.MemorizeSearchRetrieveafterCreateAndDelete(userData.DatabaseKey);
            return optionList;
        }
        #endregion

        #region Comment        
        public List<Notes> PopulateComment(long objectId, string tableName)
        {
            Notes note = new Notes()
            {
                ObjectId = objectId,
                TableName = tableName
            };
            List<Notes> NotesList = note.RetrieveByObjectId(userData.DatabaseKey, userData.Site.TimeZone);
            return NotesList;
        }
        public List<string> AddOrUpdateComment(NotesModel notesModel, ref string Mode, string tableName, List<UserMentionDataModel.UserMentionData> objUserMentionData)
        {
            Notes notes = new Notes()
            {
                OwnerId = userData.DatabaseKey.User.UserInfoId,
                OwnerName = userData.DatabaseKey.User.FullName,
                Subject = notesModel.Subject ?? "No Subject",
                Content = notesModel.Content,
                Type = notesModel.Type,
                TableName = tableName,
                ObjectId = notesModel.ObjectId,//notesModel.WorkOrderId,
                UpdateIndex = notesModel.updatedindex,
                NotesId = notesModel.NotesId
            };
            if (notesModel.NotesId == 0)
            {
                Mode = "add";
                notes.Create(this.userData.DatabaseKey);
                ProcessAlert objAlert = new ProcessAlert(this.userData);

                List<long> nos = new List<long>() { notesModel.ObjectId };
                List<string> userNameList = new List<string>();
                List<long> userIds = new List<long>();
                var UserList = new List<Tuple<long, string, string>>();
                if (objUserMentionData != null && objUserMentionData.Count > 0)
                {
                    foreach (var item in objUserMentionData)
                    {
                        UserList.Add
                       (
                            Tuple.Create(Convert.ToInt64(item.userId), item.userName, item.emailId)
                      );
                    }
                    Task CreateAlertTask;
                    switch (tableName)
                    {
                        case ("WorkOrder"):
                            CreateAlertTask = Task.Factory.StartNew(() => objAlert.CreateAlert<DataContracts.WorkOrder>(AlertTypeEnum.WorkOrderCommentMention, nos, notes, UserList));
                            break;
                        case ("Equipment"):
                            CreateAlertTask = Task.Factory.StartNew(() => objAlert.CreateAlert<DataContracts.Equipment>(AlertTypeEnum.AssetCommentMention, nos, notes, UserList));
                            break;
                        case ("Part"):
                            CreateAlertTask = Task.Factory.StartNew(() => objAlert.CreateAlert<DataContracts.Part>(AlertTypeEnum.PartCommentMention, nos, notes, UserList));
                            break;
                        case ("ServiceOrder"):
                            CreateAlertTask = Task.Factory.StartNew(() => objAlert.CreateAlert<DataContracts.ServiceOrder>(AlertTypeEnum.ServiceOrderCommentMention, nos, notes, UserList));
                            break;
                        case ("WorkOrderPlan"):
                            CreateAlertTask = Task.Factory.StartNew(() => objAlert.CreateAlert<DataContracts.WorkOrderPlan>(AlertTypeEnum.WOPlanCommentMention, nos, notes, UserList));
                            break;
                        case ("Project"):
                            CreateAlertTask = Task.Factory.StartNew(() => objAlert.CreateAlert<DataContracts.Project>(AlertTypeEnum.ProjectCommentMention, nos, notes, UserList));
                            break;
                    }

                }
            }
            else
            {
                Mode = "update";
                notes.Update(this.userData.DatabaseKey);
            }

            return notes.ErrorMessages;
        }

        public List<string> AddComment(NotesModel notesModel, string tableName)
        {
            Notes notes = new Notes()
            {
                OwnerId = userData.DatabaseKey.User.UserInfoId,
                OwnerName = userData.DatabaseKey.User.FullName,
                Subject = notesModel.Subject ?? "No Subject",
                Content = notesModel.Content,
                Type = notesModel.Type,
                TableName = tableName,
                ObjectId = notesModel.ObjectId,//notesModel.WorkOrderId,
                UpdateIndex = notesModel.updatedindex,
                NotesId = notesModel.NotesId
            };
            notes.Create(this.userData.DatabaseKey);
            return notes.ErrorMessages;
        }

        public bool DeleteComment(long _notesId)
        {
            try
            {
                Notes notes = new Notes()
                {
                    NotesId = Convert.ToInt64(_notesId)
                };
                notes.Delete(userData.DatabaseKey);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<Personnel> MentionList(string searchtext)
        {
            Personnel p = new Personnel();
            List<Personnel> Mlist = new List<Personnel>();
            p.ClientId = userData.DatabaseKey.Client.ClientId;
            p.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            p.Searchtext = searchtext;
            Mlist = p.PersonnelRetrieveForMention(userData.DatabaseKey);
            return Mlist;
        }
        #endregion

        #region UiConfig Column Hide
        public List<UIConfigModel> GetHiddenList(string viewName, string isExternal = "")
        {
            LoginCacheSet loginCacheSet = new LoginCacheSet();
            loginCacheSet.userData = userData;
            var uIConfigList = loginCacheSet.GetUIConfigAllCommon(ConfigurationManager.ConnectionStrings[WebConfigConstants.ADMIN_CONNECTION_STRING].ToString(), "", viewName, UiConfigConstants.IsHideTrue, UiConfigConstants.IsRequiredNone, isExternal);

            UIConfigModel objUIConfigModel;
            List<UIConfigModel> uiConfigModelList = new List<UIConfigModel>();

            foreach (var item in uIConfigList)
            {
                objUIConfigModel = new UIConfigModel();
                objUIConfigModel.ViewName = item.ViewName;
                objUIConfigModel.ColumnName = item.ColumnName;
                objUIConfigModel.Required = item.Required;
                objUIConfigModel.Hide = item.Hide;
                objUIConfigModel.IsExternal = item.IsExternal;
                objUIConfigModel.Disable = item.Disable;
                uiConfigModelList.Add(objUIConfigModel);
            }
            return uiConfigModelList;
        }

        public List<UIConfigModel> GetAllUiConfigList(string viewName, string isExternal = "")
        {
            LoginCacheSet loginCacheSet = new LoginCacheSet();
            loginCacheSet.userData = userData;
            var uIConfigList = loginCacheSet.GetUIConfigAllCommon(ConfigurationManager.ConnectionStrings[WebConfigConstants.ADMIN_CONNECTION_STRING].ToString(), "", viewName, UiConfigConstants.IsHideNone, UiConfigConstants.IsRequiredNone, isExternal);

            UIConfigModel objUIConfigModel;
            List<UIConfigModel> uiConfigModelList = new List<UIConfigModel>();

            foreach (var item in uIConfigList)
            {
                objUIConfigModel = new UIConfigModel();
                objUIConfigModel.ViewName = item.ViewName;
                objUIConfigModel.ColumnName = item.ColumnName;
                objUIConfigModel.Required = item.Required;
                objUIConfigModel.Hide = item.Hide;
                objUIConfigModel.IsExternal = item.IsExternal;
                objUIConfigModel.Disable = item.Disable;
                uiConfigModelList.Add(objUIConfigModel);
            }
            return uiConfigModelList;
        }

        public List<UIConfigModel> GetAllUiConfigListForClientSideValidator(string viewName, string isExternal = "")
        {
            LoginCacheSet loginCacheSet = new LoginCacheSet();
            loginCacheSet.userData = userData;
            var uIConfigList = loginCacheSet.GetUIConfigAllCommon(ConfigurationManager.ConnectionStrings[WebConfigConstants.ADMIN_CONNECTION_STRING].ToString(), "", viewName, UiConfigConstants.IsHideNone, UiConfigConstants.IsRequiredTrue, isExternal);

            UIConfigModel objUIConfigModel;
            List<UIConfigModel> uiConfigModelList = new List<UIConfigModel>();

            foreach (var item in uIConfigList)
            {
                objUIConfigModel = new UIConfigModel();
                objUIConfigModel.ViewName = item.ViewName;
                objUIConfigModel.ColumnName = item.ColumnName;
                objUIConfigModel.Required = item.Required;
                objUIConfigModel.Hide = item.Hide;
                objUIConfigModel.IsExternal = item.IsExternal;
                objUIConfigModel.Disable = item.Disable;
                uiConfigModelList.Add(objUIConfigModel);
            }
            return uiConfigModelList;
        }

        #endregion


        #region User Management Modifications-V2-332  

        public List<UserAccessLookupModel> PopulateUserAccessList(int pageNumber, int ResultsPerPage, string OrderColumn, string OrderDirection, string SecurityProfileName, string SecurityProfileDescription)
        {
            List<UserAccessLookupModel> UserAccessLookupModelList = new List<UserAccessLookupModel>();

            DataContracts.SecurityProfile secprof = new DataContracts.SecurityProfile()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                Page = pageNumber,
                ResultsPerPage = ResultsPerPage,
                OrderColumn = OrderColumn,
                OrderDirection = OrderDirection,
                Name = SecurityProfileName,
                Description = SecurityProfileDescription
            };
            List<DataContracts.SecurityProfile> splist = new List<DataContracts.SecurityProfile>();
            secprof.PackageLevel = userData.DatabaseKey.Client.PackageLevel;
            secprof.ProductGrouping = GetProductGrouping();

            splist = secprof.RetrieveByPackageLevel(userData.DatabaseKey);
            foreach (var v in splist)
            {
                UserAccessLookupModel objUserAccessLookupModel = new UserAccessLookupModel();
                objUserAccessLookupModel.SecurityProfileId = v.SecurityProfileId;
                objUserAccessLookupModel.SecurityProfileName = v.Name;
                objUserAccessLookupModel.SecurityProfileDescription = v.Description;
                objUserAccessLookupModel.UserType = v.UserType;
                objUserAccessLookupModel.CMMSUser = v.CMMSUser;
                objUserAccessLookupModel.SanitationUser = v.SanitationUser;
                objUserAccessLookupModel.TotalCount = secprof.ResultCount;  /* splist.Count;*/
                objUserAccessLookupModel.PackageLevel = secprof.PackageLevel;
                objUserAccessLookupModel.ProductGrouping = secprof.ProductGrouping;
                UserAccessLookupModelList.Add(objUserAccessLookupModel);
            }
            return UserAccessLookupModelList;
        }
        public List<UserAccessLookupModel> PopulateUserAccessChangeList(int pageNumber, int ResultsPerPage, string OrderColumn, string OrderDirection, string SecurityProfileName, string SecurityProfileDescription, bool APM, bool CMMS, bool Sanitation, bool Fleet, bool Production)
        {
            List<UserAccessLookupModel> UserAccessLookupModelList = new List<UserAccessLookupModel>();

            DataContracts.SecurityProfile secprof = new DataContracts.SecurityProfile()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                Page = pageNumber,
                ResultsPerPage = ResultsPerPage,
                OrderColumn = OrderColumn,
                OrderDirection = OrderDirection,
                Name = SecurityProfileName,
                Description = SecurityProfileDescription
            };
            List<DataContracts.SecurityProfile> splist = new List<DataContracts.SecurityProfile>();
            secprof.PackageLevel = userData.DatabaseKey.Client.PackageLevel;
            secprof.ProductGrouping = GetUserAccessChangeProductGrouping(APM, CMMS, Sanitation, Fleet, Production);
            splist = secprof.RetrieveByPackageLevel(userData.DatabaseKey);
            foreach (var v in splist)
            {
                UserAccessLookupModel objUserAccessLookupModel = new UserAccessLookupModel();
                objUserAccessLookupModel.SecurityProfileId = v.SecurityProfileId;
                objUserAccessLookupModel.SecurityProfileName = v.Name;
                objUserAccessLookupModel.SecurityProfileDescription = v.Description;
                objUserAccessLookupModel.UserType = v.UserType;
                objUserAccessLookupModel.CMMSUser = v.CMMSUser;
                objUserAccessLookupModel.SanitationUser = v.SanitationUser;
                objUserAccessLookupModel.TotalCount = secprof.ResultCount;  /* splist.Count;*/
                objUserAccessLookupModel.PackageLevel = secprof.PackageLevel;
                objUserAccessLookupModel.ProductGrouping = secprof.ProductGrouping;
                UserAccessLookupModelList.Add(objUserAccessLookupModel);
            }
            return UserAccessLookupModelList;
        }
        private int GetProductGrouping()
        {
            // RKL - 2020-11-17 - Included the Fleet settings 
            // New product groupings
            int productGrouping = 0;
            var siteData = userData.Site;
            //-------Product Grouping Values----//
            // CMMS Only 
            if (siteData.CMMS == true && siteData.APM == false && siteData.Sanitation == false && siteData.Fleet == false && siteData.Production == false)      //---These 5 conditions denote product grouping=1
            {
                productGrouping = 1;
            }
            // Sanitation Only
            else if (siteData.CMMS == false && siteData.APM == false && siteData.Sanitation == true && siteData.Fleet == false && siteData.Production == false) //---These 5 conditions denote product grouping=2
            {
                productGrouping = 2;
            }
            // APM Only
            else if (siteData.CMMS == false && siteData.APM == true && siteData.Sanitation == false && siteData.Fleet == false && siteData.Production == false) //---These 5 conditions denote product grouping=3
            {
                productGrouping = 3;
            }
            // CMMS and Sanitation
            else if (siteData.CMMS == true && siteData.APM == false && siteData.Sanitation == true && siteData.Fleet == false && siteData.Production == false)  //---These 5 conditions denote product grouping=4
            {
                productGrouping = 4;
            }
            // CMMS and APM
            else if (siteData.CMMS == true && siteData.APM == true && siteData.Sanitation == false && siteData.Fleet == false && siteData.Production == false)  //---These 5 conditions denote product grouping=5
            {
                productGrouping = 5;
            }
            // CMMS and APM and Sanitation
            else if (siteData.CMMS == true && siteData.APM == true && siteData.Sanitation == true && siteData.Fleet == false && siteData.Production == false)   //---These 5 conditions denote product grouping=6
            {
                productGrouping = 6;
            }
            // Fleet Only 
            else if (siteData.CMMS == false && siteData.APM == false && siteData.Sanitation == false && userData.Site.Fleet == true && siteData.Production == false)
            {
                productGrouping = 7;
            }
            // CMMS and Fleet
            else if (siteData.CMMS == true && siteData.APM == false && siteData.Sanitation == false && userData.Site.Fleet == true && siteData.Production == false)
            {
                productGrouping = 8;
            }
            // Fleet and APM
            else if (siteData.CMMS == false && siteData.APM == true && siteData.Sanitation == false && userData.Site.Fleet == true && siteData.Production == false)
            {
                productGrouping = 9;
            }
            // CMMS and APM and FLEET
            else if (siteData.CMMS == true && siteData.APM == true && siteData.Sanitation == false && userData.Site.Fleet == true && siteData.Production == false)
            {
                productGrouping = 10;
            }

            // CMMS Only and Production 
            else if (siteData.CMMS == true && siteData.APM == false && siteData.Sanitation == false && siteData.Fleet == false && siteData.Production == true)      //---These 5 conditions denote product grouping=11
            {
                productGrouping = 11;
            }
            // CMMS  and Production ,Sanitation
            else if (siteData.CMMS == true && siteData.APM == false && siteData.Sanitation == true && siteData.Fleet == false && siteData.Production == true)      //---These 5 conditions denote product grouping=12
            {
                productGrouping = 12;
            }
            // CMMS  and Production and APM
            else if (siteData.CMMS == true && siteData.APM == true && siteData.Sanitation == false && siteData.Fleet == false && siteData.Production == true)      //---These 5 conditions denote product grouping=13
            {
                productGrouping = 13;
            }
            // CMMS  and Production and APM and Sanitation
            else if (siteData.CMMS == true && siteData.APM == true && siteData.Sanitation == true && siteData.Fleet == false && siteData.Production == true)      //---These 5 conditions denote product grouping=14
            {
                productGrouping = 14;
            }
            return productGrouping;
        }
        private int GetUserAccessChangeProductGrouping(bool APM, bool CMMS, bool Sanitation, bool Fleet, bool Production)
        {
            // RKL - 2020-11-17 - Included the Fleet settings 
            // New product groupings
            int productGrouping = 0;
            // var siteData = userData.Site;
            //-------Product Grouping Values----//
            // CMMS Only 
            if (CMMS == true && APM == false && Sanitation == false && Fleet == false && Production == false)      //---These 5 conditions denote product grouping=1
            {
                productGrouping = 1;
            }
            // Sanitation Only
            else if (CMMS == false && APM == false && Sanitation == true && Fleet == false && Production == false) //---These 5 conditions denote product grouping=2
            {
                productGrouping = 2;
            }
            // APM Only
            else if (CMMS == false && APM == true && Sanitation == false && Fleet == false && Production == false) //---These 5 conditions denote product grouping=3
            {
                productGrouping = 3;
            }
            // CMMS and Sanitation
            else if (CMMS == true && APM == false && Sanitation == true && Fleet == false && Production == false)  //---These 5 conditions denote product grouping=4
            {
                productGrouping = 4;
            }
            // CMMS and APM
            else if (CMMS == true && APM == true && Sanitation == false && Fleet == false && Production == false)  //---These 5 conditions denote product grouping=5
            {
                productGrouping = 5;
            }
            // CMMS and APM and Sanitation
            else if (CMMS == true && APM == true && Sanitation == true && Fleet == false && Production == false)   //---These 5 conditions denote product grouping=6
            {
                productGrouping = 6;
            }
            // Fleet Only 
            else if (CMMS == false && APM == false && Sanitation == false && Fleet == true && Production == false)
            {
                productGrouping = 7;
            }
            // CMMS and Fleet
            else if (CMMS == true && APM == false && Sanitation == false && Fleet == true && Production == false)
            {
                productGrouping = 8;
            }
            // Fleet and APM
            else if (CMMS == false && APM == true && Sanitation == false && Fleet == true && Production == false)
            {
                productGrouping = 9;
            }
            // CMMS and APM and FLEET
            else if (CMMS == true && APM == true && Sanitation == false && Fleet == true && Production == false)
            {
                productGrouping = 10;
            }
            // CMMS Only and Production 
            else if (CMMS == true && APM == false && Sanitation == false && Fleet == false && Production == true)      //---These 5 conditions denote product grouping=11
            {
                productGrouping = 11;
            }
            // CMMS  and Production ,Sanitation
            else if (CMMS == true && APM == false && Sanitation == true && Fleet == false && Production == true)      //---These 5 conditions denote product grouping=12
            {
                productGrouping = 12;
            }
            // CMMS  and Production and APM
            else if (CMMS == true && APM == true && Sanitation == false && Fleet == false && Production == true)        //---These 5 conditions denote product grouping=13
            {
                productGrouping = 13;
            }
            // CMMS  and Production and APM and Sanitation
            else if (CMMS == true && APM == true && Sanitation == true && Fleet == false && Production == true)      //---These 5 conditions denote product grouping=14
            {
                productGrouping = 14;
            }
            return productGrouping;
        }
        #endregion

        #region V2-389
        public List<DropDownModel> GetListFromConstVals(string ConstantType)
        {
            List<DropDownModel> values = new List<DropDownModel>();
            DropDownModel multiSelectModel;

            DataConstant dataConstant = new DataConstant();
            dataConstant.ConstantType = ConstantType;
            dataConstant.LocaleId = _dbKey.Localization;

            var dtValues = dataConstant.RetrieveLocaleForConstantType_V2(_dbKey);
            foreach (var item in dtValues)
            {
                multiSelectModel = new DropDownModel();
                multiSelectModel.value = item.Value;
                multiSelectModel.text = item.LocalName;
                values.Add(multiSelectModel);
            }
            return values;
        }
        #endregion

        #region Personnel
        public List<Personnel> PersonnelList()
        {
            Personnel p = new Personnel();
            List<Personnel> Mlist = new List<Personnel>();
            p.ClientId = userData.DatabaseKey.Client.ClientId;
            p.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            Mlist = p.RetrieveAll(userData.DatabaseKey);
            return Mlist;
        }
        public Personnel GetPersonnelByPersonnelId(long PersonnelId)
        {
            Personnel personnel = new Personnel();
            personnel.ClientId = userData.DatabaseKey.Client.ClientId;
            personnel.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            personnel.PersonnelId = PersonnelId;

            personnel.Retrieve(userData.DatabaseKey); ;
            return personnel;
        }
        #endregion

        #region Fleet

        public List<FleetAssetLookupModel> PopulateFleetEquipmentList(int pageNumber, int resultsPerPage, string orderColumn, string orderDirection, string clientLookupId, string name, string model, string make, string vin)
        {
            DataContracts.LookupListResultSet.FleetAssetLookupListResultSet EquipmentLookupList
                                = new DataContracts.LookupListResultSet.FleetAssetLookupListResultSet();

            DataContracts.LookupListResultSet.FleetAssetLookupListTransactionParameters settings = new DataContracts.LookupListResultSet.FleetAssetLookupListTransactionParameters()
            {
                SiteId = userData.DatabaseKey.User.DefaultSiteId,
                PageNumber = pageNumber,
                ResultsPerPage = resultsPerPage,
                OrderColumn = orderColumn,
                OrderDirection = orderDirection
            };

            settings.ClientLookupId = clientLookupId;
            settings.Name = name;
            settings.Model = model;
            settings.Make = make;
            settings.VIN = vin;

            EquipmentLookupList.UpdateSettings(userData.DatabaseKey, settings);
            EquipmentLookupList.RetrieveResults();

            List<FleetAssetLookupModel> FleetAssetLookupModelList = new List<FleetAssetLookupModel>();
            FleetAssetLookupModel objFleetAssetLookupModel;
            foreach (var v in EquipmentLookupList.Items)
            {
                objFleetAssetLookupModel = new FleetAssetLookupModel();
                objFleetAssetLookupModel.ClientLookupId = v.ClientLookupId;
                objFleetAssetLookupModel.Name = v.Name;
                objFleetAssetLookupModel.Model = v.Model;
                objFleetAssetLookupModel.Make = v.Make;
                objFleetAssetLookupModel.VIN = v.VIN;
                objFleetAssetLookupModel.Meter1Type = v.Meter1Type;
                objFleetAssetLookupModel.Meter2Type = v.Meter2Type;
                objFleetAssetLookupModel.TotalCount = EquipmentLookupList.Count;
                objFleetAssetLookupModel.EquipmentId = v.EquipmentId;
                objFleetAssetLookupModel.Meter1CurrentReading = v.Meter1CurrentReading;
                objFleetAssetLookupModel.Meter2CurrentReading = v.Meter2CurrentReading;
                objFleetAssetLookupModel.Meter1CurrentReadingDate = v.Meter1CurrentReadingDate;
                objFleetAssetLookupModel.Meter2CurrentReadingDate = v.Meter2CurrentReadingDate;
                objFleetAssetLookupModel.Meter1Unit = v.Meter1Units;
                objFleetAssetLookupModel.Meter2Unit = v.Meter2Units;
                objFleetAssetLookupModel.FuelUnits = v.FuelUnits;

                if (v.Meter1CurrentReadingDate != null && v.Meter1CurrentReadingDate != default(DateTime))
                {
                    objFleetAssetLookupModel.Meter1DayDiff = Math.Abs((DateTime.Today - Convert.ToDateTime(v.Meter1CurrentReadingDate)).Days);
                }
                if (v.Meter2CurrentReadingDate != null && v.Meter2CurrentReadingDate != default(DateTime))
                {
                    objFleetAssetLookupModel.Meter2DayDiff = Math.Abs((DateTime.Today - Convert.ToDateTime(v.Meter2CurrentReadingDate)).Days);
                }

                FleetAssetLookupModelList.Add(objFleetAssetLookupModel);
            }
            return FleetAssetLookupModelList;
        }


        public List<FleetIssuesLookupModel> PopulateFleetIssuesList(int pageNumber, int resultsPerPage, string orderColumn, string orderDirection, long Equipmentid, string Date, string Description, string Status, string Defects)
        {

            DataContracts.LookupListResultSet.FleetIssuesLookupListResultSet FleetIssuesLookupList
                                = new DataContracts.LookupListResultSet.FleetIssuesLookupListResultSet();

            DataContracts.LookupListResultSet.FleetIssuesLookupListTransactionParameters settings = new DataContracts.LookupListResultSet.FleetIssuesLookupListTransactionParameters()
            {
                SiteId = userData.DatabaseKey.User.DefaultSiteId,
                PageNumber = pageNumber,
                ResultsPerPage = resultsPerPage,
                OrderColumn = orderColumn,
                OrderDirection = orderDirection

            };
            settings.EquipmentId = Equipmentid;
            settings.Date = Date;
            settings.Description = Description;
            settings.Status = Status;
            settings.Defects = Defects;

            FleetIssuesLookupList.UpdateSettings(userData.DatabaseKey, settings);
            FleetIssuesLookupList.RetrieveResults();

            List<FleetIssuesLookupModel> FleetIssuesLookupModelList = new List<FleetIssuesLookupModel>();
            FleetIssuesLookupModel objFleetIssuesLookupModel;

            foreach (var v in FleetIssuesLookupList.Items)
            {
                objFleetIssuesLookupModel = new FleetIssuesLookupModel();
                objFleetIssuesLookupModel.FleetIssuesId = v.FleetIssuesId;
                objFleetIssuesLookupModel.Readingdate = v.RecordDate;
                objFleetIssuesLookupModel.Description = v.Description;
                objFleetIssuesLookupModel.Status = v.Status;
                objFleetIssuesLookupModel.Defects = v.Defects;
                objFleetIssuesLookupModel.TotalCount = FleetIssuesLookupList.Count;

                FleetIssuesLookupModelList.Add(objFleetIssuesLookupModel);
            }
            return FleetIssuesLookupModelList;
        }


        public List<AssetAvailabilityLogLookUpModel> PopulateAssetAvailabilityLogList(int pageNumber, int resultsPerPage, string orderColumn, string orderDirection, long objectId, string TransactionDate, string Event, string ReturnToService, string Reason, string ReasonCode,string PersonnelName)
        {
            DataContracts.LookupListResultSet.AssetAvailabilityLogLookupListResultSet AssetAvailabilityLogLookupList
                                = new DataContracts.LookupListResultSet.AssetAvailabilityLogLookupListResultSet();

            DataContracts.LookupListResultSet.AssetAvailibilityLogLookupListTransactionParameters settings = new DataContracts.LookupListResultSet.AssetAvailibilityLogLookupListTransactionParameters()
            {
                SiteId = userData.DatabaseKey.User.DefaultSiteId,
                PageNumber = pageNumber,
                ResultsPerPage = resultsPerPage,
                OrderColumn = orderColumn,
                OrderDirection = orderDirection
            };

            settings.ObjectId = objectId;
            settings.TransactionDate = TransactionDate;
            settings.Event = Event;
            settings.ReturnToService = ReturnToService;
            settings.Reason = Reason;
            settings.ReasonCode = ReasonCode;
            settings.PersonnelName = PersonnelName;

            AssetAvailabilityLogLookupList.UpdateSettings(userData.DatabaseKey, settings);
            AssetAvailabilityLogLookupList.RetrieveResults();

            List<AssetAvailabilityLogLookUpModel> AssetAvailibilityLookupModelList = new List<AssetAvailabilityLogLookUpModel>();
            AssetAvailabilityLogLookUpModel objAssetAvailibilityLookupModel;
            foreach (var v in AssetAvailabilityLogLookupList.Items)
            {
                objAssetAvailibilityLookupModel = new AssetAvailabilityLogLookUpModel();
                objAssetAvailibilityLookupModel.ObjectId = v.ObjectId;
                objAssetAvailibilityLookupModel.TransactionDate = v.TransactionDate.HasValue ? v.TransactionDate.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : string.Empty;
                objAssetAvailibilityLookupModel.Event = v.Event;
                if (v.ReturnToService != null && v.ReturnToService == default(DateTime))
                {
                    objAssetAvailibilityLookupModel.ReturnToService = string.Empty;
                }
                else
                {
                    objAssetAvailibilityLookupModel.ReturnToService = v.ReturnToService.HasValue ? v.ReturnToService.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : string.Empty;
                }
                objAssetAvailibilityLookupModel.Reason = v.Reason;
                objAssetAvailibilityLookupModel.ReasonCode = v.ReasonCode;
                objAssetAvailibilityLookupModel.PersonnelName = v.PersonnelName;
                objAssetAvailibilityLookupModel.TotalCount = AssetAvailabilityLogLookupList.Count;

                AssetAvailibilityLookupModelList.Add(objAssetAvailibilityLookupModel);
            }
            return AssetAvailibilityLookupModelList;
        }
        #endregion

        #region Hierarchical List
        public List<HierarchicalList> GetHierarchicalListByName(string ListName)
        {
            HierarchicalList HierarchicalList = new HierarchicalList()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                SiteId = userData.DatabaseKey.User.DefaultSiteId,
                ListName = ListName,
                Level1Value = "",
                Level2Value = "",
                Level3Value = ""
            };
            List<HierarchicalList> objLookUp = HierarchicalList.RetrieveActiveListByName(userData.DatabaseKey);
            return objLookUp;
        }
        #endregion

        #region Equipment Lookuplist chunk
        public List<EquipmentLookupModel> GetEquipmentLookupListGridData(string orderbycol = "", string orderDir = "", int skip = 0, int length = 0, string clientLookupId = "", string Name = "", string Model = "", string SerialNumber = "", string Type = "", string AssetGroup1ClientLookupId = "", string AssetGroup2ClientLookupId = "", string AssetGroup3ClientLookupId = "")
        {

            EquipmentLookupModel newEquipmentLookupModel;
            List<EquipmentLookupModel> newEquipmentLookupSearchModelList = new List<EquipmentLookupModel>();
            List<Equipment> EquipmentList = new List<Equipment>();
            Equipment equipment = new Equipment();
            equipment.ClientId = this.userData.DatabaseKey.Client.ClientId;
            equipment.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            equipment.OrderbyColumn = orderbycol;
            equipment.OrderBy = orderDir;
            equipment.OffSetVal = skip;
            equipment.NextRow = length;
            equipment.ClientLookupId = clientLookupId;
            equipment.Name = Name;
            equipment.Type = Type;
            equipment.Model = Model;
            equipment.SerialNumber = SerialNumber;
            equipment.AssetGroup1ClientLookupId = AssetGroup1ClientLookupId;
            equipment.AssetGroup2ClientLookupId = AssetGroup2ClientLookupId;
            equipment.AssetGroup3ClientLookupId = AssetGroup3ClientLookupId;


            EquipmentList = equipment.GetAllEquipmentLookupListV2(userData.DatabaseKey, userData.Site.TimeZone);

            foreach (var item in EquipmentList)
            {
                newEquipmentLookupModel = new EquipmentLookupModel();


                newEquipmentLookupModel.EquipmentId = item.EquipmentId;
                newEquipmentLookupModel.ClientLookupId = item.ClientLookupId;
                newEquipmentLookupModel.Name = item.Name;
                newEquipmentLookupModel.Model = item.Model;
                newEquipmentLookupModel.SerialNumber = item.SerialNumber;
                newEquipmentLookupModel.Type = item.Type;
                newEquipmentLookupModel.AssetGroup1ClientLookupId = item.AssetGroup1ClientLookupId;
                newEquipmentLookupModel.AssetGroup2ClientLookupId = item.AssetGroup2ClientLookupId;
                newEquipmentLookupModel.AssetGroup3ClientLookupId = item.AssetGroup3ClientLookupId;
                newEquipmentLookupModel.TotalCount = item.TotalCount;

                newEquipmentLookupSearchModelList.Add(newEquipmentLookupModel);
            }

            return newEquipmentLookupSearchModelList;
        }

        public List<EquipmentLookupModel> GetEquipmentLookupListGridDataMobile(string orderbycol = "", string orderDir = "", int skip = 0, int length = 0, string clientLookupId = "", string Name = "")
        {

            EquipmentLookupModel newEquipmentLookupModel;
            List<EquipmentLookupModel> newEquipmentLookupSearchModelList = new List<EquipmentLookupModel>();
            List<Equipment> EquipmentList = new List<Equipment>();
            Equipment equipment = new Equipment();
            equipment.ClientId = this.userData.DatabaseKey.Client.ClientId;
            equipment.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            equipment.OrderbyColumn = orderbycol;
            equipment.OrderBy = orderDir;
            equipment.OffSetVal = skip;
            equipment.NextRow = length;
            equipment.ClientLookupId = clientLookupId;
            equipment.Name = Name;
            EquipmentList = equipment.GetAllEquipmentLookupListMobileV2(userData.DatabaseKey, userData.Site.TimeZone);

            foreach (var item in EquipmentList)
            {
                newEquipmentLookupModel = new EquipmentLookupModel();


                newEquipmentLookupModel.EquipmentId = item.EquipmentId;
                newEquipmentLookupModel.ClientLookupId = item.ClientLookupId;
                newEquipmentLookupModel.Name = item.Name;
                newEquipmentLookupModel.TotalCount = item.TotalCount;

                newEquipmentLookupSearchModelList.Add(newEquipmentLookupModel);
            }

            return newEquipmentLookupSearchModelList;
        }
        #endregion

        #region Vendor Lookuplist chunk
        public List<VendorLookupModel> GetVendorLookupListGridData(string orderbycol = "", string orderDir = "",
            int skip = 0, int length = 0, string clientLookupId = "", string Name = "", string addressCity = "", string addressState = "")
        {

            VendorLookupModel newVendorLookupModel;
            List<VendorLookupModel> newVendorLookupSearchModelList = new List<VendorLookupModel>();
            List<Vendor> VendortList = new List<Vendor>();
            Vendor vendor = new Vendor();
            vendor.ClientId = this.userData.DatabaseKey.Client.ClientId;
            vendor.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            vendor.orderbyColumn = orderbycol;
            vendor.orderBy = orderDir;
            vendor.offset1 = skip;
            vendor.nextrow = length;
            vendor.ClientLookupId = clientLookupId;
            vendor.Name = Name;
            vendor.AddressCity = addressCity;
            vendor.AddressState = addressState;

            VendortList = vendor.GetAllVendorLookupListV2(userData.DatabaseKey, userData.Site.TimeZone);

            foreach (var item in VendortList)
            {
                newVendorLookupModel = new VendorLookupModel();
                newVendorLookupModel.VendorID = item.VendorId;
                newVendorLookupModel.ClientLookupId = item.ClientLookupId;
                newVendorLookupModel.Name = item.Name;
                newVendorLookupModel.AddressCity = item.AddressCity;
                newVendorLookupModel.AddressState = item.AddressState;
                newVendorLookupModel.TotalCount = item.totalCount;

                newVendorLookupSearchModelList.Add(newVendorLookupModel);
            }

            return newVendorLookupSearchModelList;
        }
        #endregion

        #region Storeroom Lookuplist chunk
        public List<StoreroomLookupModel> GetStoreroomLookupListGridData(string orderbycol = "", string orderDir = "",
            int skip = 0, int length = 0, long StoreroomId = 0, string Name = "", string Description = "")
        {

            StoreroomLookupModel newStoreroomLookupModel;
            List<StoreroomLookupModel> newStoreroomLookupSearchModelList = new List<StoreroomLookupModel>();
            List<Storeroom> StoreroomList = new List<Storeroom>();
            Storeroom storeroom = new Storeroom();
            storeroom.ClientId = this.userData.DatabaseKey.Client.ClientId;
            storeroom.PersonnelId = this.userData.DatabaseKey.Personnel.PersonnelId;
            storeroom.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            storeroom.orderbyColumn = orderbycol;
            storeroom.orderBy = orderDir;
            storeroom.offset1 = skip;
            storeroom.nextrow = length;
            storeroom.StoreroomId = StoreroomId;
            storeroom.Name = Name;
            storeroom.Description = Description;

            StoreroomList = storeroom.GetAllStoreroomLookupListV2(userData.DatabaseKey, userData.Site.TimeZone);
            foreach (var item in StoreroomList)
            {
                newStoreroomLookupModel = new StoreroomLookupModel();
                newStoreroomLookupModel.StoreroomId = item.StoreroomId;
                newStoreroomLookupModel.Name = item.Name;
                newStoreroomLookupModel.Description = item.Description;
                newStoreroomLookupModel.TotalCount = item.totalCount;

                newStoreroomLookupSearchModelList.Add(newStoreroomLookupModel);
            }

            return newStoreroomLookupSearchModelList;
        }
        #endregion

        #region WorkOrder Lookuplist chunk
        public List<WorkOrderLookUpModel> GetWorkOrderLookupListGridData(string OrderColumn, string OrderDirection, int pageNumber, int ResultsPerPage, string clientLookupId, string Description, string ChargeTo, string WorkAssigned, string Requestor, string Status)
        {
            WorkOrderLookUpModel newWorkOrderLookupModel;
            List<WorkOrderLookUpModel> newWorkOrderLookupSearchModelList = new List<WorkOrderLookUpModel>();
            List<WorkOrder> workorderlist = new List<WorkOrder>();

            WorkOrder workOrder = new WorkOrder();
            workOrder.ClientLookupId = clientLookupId;
            workOrder.Description = Description;
            workOrder.ChargeTo_Name = ChargeTo;
            workOrder.WorkAssigned_Name = WorkAssigned;
            workOrder.Requestor_Name = Requestor;
            workOrder.Status = Status;
            workOrder.OrderbyColumn = OrderColumn;
            workOrder.OrderBy = OrderDirection;
            workOrder.OffSetVal = pageNumber;
            workOrder.NextRow = ResultsPerPage;
            workOrder.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            workOrder.ClientId = this.userData.DatabaseKey.Client.ClientId;

            workorderlist = workOrder.GetAllWorkOrderLookupListV2(userData.DatabaseKey);

            foreach (var workorder in workorderlist)
            {
                newWorkOrderLookupModel = new WorkOrderLookUpModel();
                newWorkOrderLookupModel.ClientLookupId = workorder.ClientLookupId;
                newWorkOrderLookupModel.Description = string.IsNullOrEmpty(workorder.Description) ? "" : workorder.Description;
                newWorkOrderLookupModel.ChargeTo = string.IsNullOrEmpty(workorder.ChargeTo_Name) ? "" : workorder.ChargeTo_Name;
                newWorkOrderLookupModel.WorkAssigned = string.IsNullOrEmpty(workorder.WorkAssigned_Name) ? "" : workorder.WorkAssigned_Name;
                newWorkOrderLookupModel.Requestor = string.IsNullOrEmpty(workorder.Requestor_Name) ? "" : workorder.Requestor_Name;
                newWorkOrderLookupModel.Status = string.IsNullOrEmpty(workorder.Status) ? "" : workorder.Status;
                newWorkOrderLookupModel.TotalCount = workorder.TotalCount;
                newWorkOrderLookupModel.WorkOrderId = workorder.WorkOrderId;
                newWorkOrderLookupSearchModelList.Add(newWorkOrderLookupModel);

            }
            return newWorkOrderLookupSearchModelList;
        }


        public List<WorkOrderLookUpModel> GetWorkOrderIssueToLookupListGridData(string OrderColumn, string OrderDirection, int pageNumber, int ResultsPerPage, string clientLookupId, string Description, string ChargeTo, string WorkAssigned, string Requestor, string Status)
        {
            var workorderlist = new List<WorkOrder>();

            WorkOrder workOrder = new WorkOrder();
            workOrder.ClientLookupId = clientLookupId;
            workOrder.Description = Description;
            workOrder.ChargeTo_Name = ChargeTo;
            workOrder.WorkAssigned_Name = WorkAssigned;
            workOrder.Requestor_Name = Requestor;
            workOrder.Status = Status;
            workOrder.OrderbyColumn = OrderColumn;
            workOrder.OrderBy = OrderDirection;
            workOrder.OffSetVal = pageNumber;
            workOrder.NextRow = ResultsPerPage;
            workOrder.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            workOrder.ClientId = this.userData.DatabaseKey.Client.ClientId;

            workorderlist = workOrder.GetAllWorkOrderLookupListV2(userData.DatabaseKey)
                .Where(x => x.Status == "Scheduled" || x.Status == "Approved")
                .ToList();

            var newWorkOrderLookupSearchModelList = workorderlist.Select(workorder => new WorkOrderLookUpModel
            {
                ClientLookupId = workorder.ClientLookupId,
                Description = string.IsNullOrEmpty(workorder.Description) ? "" : workorder.Description,
                ChargeTo = string.IsNullOrEmpty(workorder.ChargeTo_Name) ? "" : workorder.ChargeTo_Name,
                WorkAssigned = string.IsNullOrEmpty(workorder.WorkAssigned_Name) ? "" : workorder.WorkAssigned_Name,
                Requestor = string.IsNullOrEmpty(workorder.Requestor_Name) ? "" : workorder.Requestor_Name,
                Status = string.IsNullOrEmpty(workorder.Status) ? "" : workorder.Status,
                TotalCount = workorder.TotalCount,
                WorkOrderId = workorder.WorkOrderId
            }).ToList();

            return newWorkOrderLookupSearchModelList;
        }
        #endregion

        #region Part LookupList chunk
        public List<PartXRefGridDataModel> GetPartLookupListGridData(string orderbycol = "", string OrderDirection = "", int skip = 0, int length = 0, string ClientLookupId = "", string Description = "", string UPCcode = "", string Manufacturer = "", string ManufacturerId = "", string StockType = "", string Storeroomid = "")
        {
            PartXRefGridDataModel objPartXRefGridDataModel;
            List<PartXRefGridDataModel> PartXRefGridDataModelList = new List<PartXRefGridDataModel>();

            Part part = new Part();
            part.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            part.ClientLookupId = ClientLookupId;
            part.Description = Description;
            part.UPCCode = UPCcode;
            part.Manufacturer = Manufacturer;
            part.ManufacturerId = ManufacturerId;
            part.StockType = StockType;
            part.Page = skip;
            part.ResultsPerPage = length;
            part.OrderbyColumn = orderbycol;
            part.OrderBy = OrderDirection;

            if (userData.DatabaseKey.Client.UseMultiStoreroom && !string.IsNullOrEmpty(Storeroomid))
            {
                part.StoreroomId = !string.IsNullOrEmpty(Storeroomid) ? Convert.ToInt64(Storeroomid) : 0;
                if (part.StoreroomId > 0)
                {
                    part.PartChunkSearchLookupListForMultiStoreroom_V2(userData.DatabaseKey, userData.Site.TimeZone);
                }

            }
            else
            {
                part.PartChunkSearchLookupListV2(userData.DatabaseKey, userData.Site.TimeZone);
            }
            foreach (var item in part.listOfPart)
            {
                objPartXRefGridDataModel = new PartXRefGridDataModel();
                objPartXRefGridDataModel.PartId = item.PartId;
                objPartXRefGridDataModel.ClientLookUpId = item.ClientLookupId;
                objPartXRefGridDataModel.Description = item.Description;
                objPartXRefGridDataModel.Manufacturer = item.Manufacturer;
                objPartXRefGridDataModel.ManufacturerID = item.ManufacturerId;
                objPartXRefGridDataModel.UPCcode = item.UPCCode;
                objPartXRefGridDataModel.StockType = item.StockType;
                #region V2-1124
                objPartXRefGridDataModel.IssueUnit = item.IssueUnit;
                objPartXRefGridDataModel.AppliedCost = item.AppliedCost;
                #endregion
                objPartXRefGridDataModel.TotalCount = item.TotalCount;
                objPartXRefGridDataModel.PartStoreroomId = item.PartStoreroomId;//RKL-MAIL-Label Printing from Receipts
                PartXRefGridDataModelList.Add(objPartXRefGridDataModel);
            }
            return PartXRefGridDataModelList;
        }
        #endregion

        #region Retrieve Part Id By ClientLookUp
        public PartModel RetrievePartIdByClientLookUp(string ClientLookupId = "")
        {
            Part part = new Part();
            part.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            part.ClientId = userData.DatabaseKey.Client.ClientId;
            part.ClientLookupId = ClientLookupId;
            var partResult = part.RetrievePartIdByClientLookupId(userData.DatabaseKey);

            var partModel = new PartModel()
            {
                PartId = partResult.PartId,

            };
            return partModel;
        }
        #endregion

        #region Retrieve Personnel Id By ClientLookUpId
        public PersonnelModel RetrievePersonnelIdByClientLookUpId(string ClientLookupId = "")
        {
            Personnel personnel = new Personnel();
            personnel.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            personnel.ClientId = userData.DatabaseKey.Client.ClientId;
            personnel.ClientLookupId = ClientLookupId;
            var personnelResult = personnel.RetrievePersonnelIdByClientLookupId(userData.DatabaseKey);

            var personnelModel = new PersonnelModel()
            {
                PersonnelId = personnelResult.PersonnelId,

            };
            return personnelModel;
        }
        #endregion
        #region Retrieve Equipment Id By ClientLookUpId
        public Equipment RetrieveEquipmentIdByClientLookUpId(string clientLookUpId)
        {
            Equipment equipment = new Equipment { ClientId = _dbKey.Client.ClientId, SiteId = _dbKey.User.DefaultSiteId, ClientLookupId = clientLookUpId };
            var result = equipment.RetrieveEquipmentIdByClientLookupIdV2(_dbKey);
            return result;
        }
        #endregion
        #region Retrieve Workorder Id By ClientLookUpId
        public WorkOrder RetrieveWorkOrderIdByClientLookUpId(string clientLookUpId)
        {
            WorkOrder workorder = new WorkOrder { ClientId = _dbKey.Client.ClientId, SiteId = _dbKey.User.DefaultSiteId, ClientLookupId = clientLookUpId };
            var result = workorder.RetrieveWorkOrderIdByClientLookupIdV2(_dbKey);
            return result;
        }
        #endregion

        #region Instruction
        public List<string> AddOrUpdateInstruction(InstructionModel instructModel, ref string Mode, string tableName)
        {
            Instructions instruct = new Instructions();

            instruct.Contents = instructModel.Content ?? "";
            instruct.TableName = tableName;
            instruct.ObjectId = instructModel.ObjectId;
            instruct.InstructionsId = instructModel.InstructionId;

            if (instructModel.InstructionId == 0)
            {
                Mode = "add";
                instruct.Create(this.userData.DatabaseKey);
            }
            else
            {
                Mode = "update";
                instruct.Update(this.userData.DatabaseKey);
            }

            return instruct.ErrorMessages;
        }

        public List<InstructionModel> PopulateInstructions(long objectId, string tableName)
        {
            InstructionModel objInstructionModel;
            List<InstructionModel> InstructModelList = new List<InstructionModel>();
            Instructions instruct = new Instructions()
            {
                ObjectId = objectId,
                TableName = tableName
            };
            List<Instructions> InstructionsList = instruct.RetrieveInstructionsByObjectId_V2(userData.DatabaseKey);
            if (InstructionsList != null)
            {
                foreach (var item in InstructionsList)
                {
                    objInstructionModel = new InstructionModel();
                    objInstructionModel.InstructionId = item.InstructionsId;
                    objInstructionModel.Content = item.Contents;
                    objInstructionModel.ObjectId = item.ObjectId;
                    InstructModelList.Add(objInstructionModel);
                }
            }
            return InstructModelList;
        }
        #endregion

        #region v2
        public List<LocationLookUpModel> PopulateLocationList_V2(int pageNumber, int resultsPerPage, string orderColumn, string orderDirection, string clientLookupId, string name)
        {
            List<LocationLookUpModel> LocationLookUpModelList = new List<LocationLookUpModel>();
            LocationLookUpModel objLocationLookUpModel;

            DataContracts.LookupListResultSet.LocationLookupListResultSet LocationLookupList
                    = new DataContracts.LookupListResultSet.LocationLookupListResultSet();

            DataContracts.LookupListResultSet.LocationLookupListTransactionParameters settings = new DataContracts.LookupListResultSet.LocationLookupListTransactionParameters()
            {
                SiteId = userData.DatabaseKey.User.DefaultSiteId,
                PageNumber = pageNumber,
                ResultsPerPage = resultsPerPage,
                OrderColumn = orderColumn,
                OrderDirection = orderDirection
            };

            settings.ClientLookupId = clientLookupId;
            settings.Name = name;

            LocationLookupList.UpdateSettings(userData.DatabaseKey, settings);
            LocationLookupList.RetrieveResults_V2();

            foreach (var item in LocationLookupList.Items)
            {
                objLocationLookUpModel = new LocationLookUpModel();
                objLocationLookUpModel.LocationId = item.LocationId;
                objLocationLookUpModel.ClientLookupId = item.ClientLookupId;
                objLocationLookUpModel.Name = item.Name;
                objLocationLookUpModel.Complex = item.Complex;
                objLocationLookUpModel.Type = item.Type;
                objLocationLookUpModel.TotalCount = LocationLookupList.Count;
                LocationLookUpModelList.Add(objLocationLookUpModel);
            }
            return LocationLookUpModelList;
        }
        #endregion
        #region PersonnelDetails
        public PersonnelModel GetPersonnelDetailsByPersonnelId(long PersonnelId)
        {
            PersonnelModel objPersonnelModel;

            DataContracts.Personnel personnel = new DataContracts.Personnel()
            {
                PersonnelId = PersonnelId
            };
            personnel.RetrievePersonnelByPersonnelId_V2(userData.DatabaseKey, personnel);
            objPersonnelModel = initializePersonnelDetailsControls(personnel);
            return objPersonnelModel;
        }
        public PersonnelModel initializePersonnelDetailsControls(Personnel obj)
        {
            PersonnelModel objPersonnelModel = new PersonnelModel();
            objPersonnelModel.PersonnelId = obj.PersonnelId;
            objPersonnelModel.Email = obj.Email;
            objPersonnelModel.UserName = obj.UserName;

            return objPersonnelModel;
        }
        #endregion


        #region Update parts On Order in workOrder
        public void UpdatePartsonOrder(long workorderId, string Mode)//--V2-576--//
        {
            WorkOrder w = new WorkOrder();
            w.ClientId = userData.DatabaseKey.Client.ClientId;
            w.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            w.WorkOrderId = workorderId;
            w.Mode = Mode;
            w.WorkOrderUpdatePartsonOrder(this.userData.DatabaseKey, userData.Site.TimeZone);
        }
        public void UpdateListPartsonOrder(long ObjectId, string Mode, string TableName)//--V2-576--//
        {
            WorkOrder w = new WorkOrder();
            w.ClientId = userData.DatabaseKey.Client.ClientId;
            w.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            w.ObjectId = ObjectId;
            w.Mode = Mode;
            w.TableName = TableName;
            w.WorkOrderUpdateListPartsonOrder(this.userData.DatabaseKey, userData.Site.TimeZone);
        }
        #endregion

        #region Retrieve all Active Full users Personnel
        public List<Personnel> PersonnelListForActiveFullUser()
        {
            Personnel p = new Personnel();
            List<Personnel> Mlist = new List<Personnel>();
            p.ClientId = userData.DatabaseKey.Client.ClientId;
            p.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            Mlist = p.RetrieveAllPersonnelforActiveandFullUser(userData.DatabaseKey);
            return Mlist;
        }
        #endregion

        #region UI Configuration
        public List<UIConfigurationDetailsModel> RetrieveUIConfigurationDetailsByUIViewId(long UIViewId)
        {
            UIConfiguration configuration = new UIConfiguration()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                UIViewId = UIViewId
            };
            UIConfigurationDetailsModel ConfigurationDetailsModel;
            List<UIConfigurationDetailsModel> UIConfigurationDetailsList = new List<UIConfigurationDetailsModel>();
            List<UIConfiguration> uIConfigurationList = new List<UIConfiguration>();
            uIConfigurationList = configuration.RetrieveDetailsByUIViewId(userData.DatabaseKey);
            foreach (var data in uIConfigurationList)
            {
                ConfigurationDetailsModel = new UIConfigurationDetailsModel();
                ConfigurationDetailsModel.UIConfigurationId = data.UIConfigurationId;
                ConfigurationDetailsModel.TableName = data.TableName;
                ConfigurationDetailsModel.ColumnName = data.ColumnName;
                ConfigurationDetailsModel.ColumnLabel = data.ColumnLabel;
                ConfigurationDetailsModel.ColumnType = data.ColumnType;
                ConfigurationDetailsModel.Required = data.Required;
                ConfigurationDetailsModel.LookupType = data.LookupType;
                ConfigurationDetailsModel.LookupName = data.LookupName;
                ConfigurationDetailsModel.UDF = data.UDF;
                ConfigurationDetailsModel.Enabled = data.Enabled;
                ConfigurationDetailsModel.SystemRequired = data.SystemRequired;
                ConfigurationDetailsModel.Order = data.Order;
                ConfigurationDetailsModel.Display = data.Display;
                ConfigurationDetailsModel.ViewOnly = data.ViewOnly;
                ConfigurationDetailsModel.Section = data.Section;
                ConfigurationDetailsModel.SectionName = data.SectionName;

                UIConfigurationDetailsList.Add(ConfigurationDetailsModel);
            }
            return UIConfigurationDetailsList;
        }
        #endregion

        #region Project Lookuplist chunk
        public List<ProjectLookupModel> GetProjectLookupListGridData(string orderbycol = "", string orderDir = "",
            int skip = 0, int length = 0, string clientLookupId = "", string Description = "")
        {

            ProjectLookupModel newProjectLookupModel;
            List<ProjectLookupModel> newProjectLookupSearchModelList = new List<ProjectLookupModel>();
            List<Project> ProjectList = new List<Project>();
            Project project = new Project();
            project.ClientId = this.userData.DatabaseKey.Client.ClientId;
            project.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            project.orderbyColumn = orderbycol;
            project.orderBy = orderDir;
            project.offset1 = skip;
            project.nextrow = length;
            project.ClientLookupId = clientLookupId;
            project.Description = Description;

            ProjectList = project.GetAllProjectLookupListV2(userData.DatabaseKey, userData.Site.TimeZone);

            foreach (var item in ProjectList)
            {
                newProjectLookupModel = new ProjectLookupModel();
                newProjectLookupModel.ProjectID = item.ProjectId;
                newProjectLookupModel.ClientLookupId = item.ClientLookupId;
                newProjectLookupModel.Description = item.Description;

                newProjectLookupModel.TotalCount = item.totalCount;

                newProjectLookupSearchModelList.Add(newProjectLookupModel);
            }

            return newProjectLookupSearchModelList;
        }
        #endregion

        #region V2- 635
        public string GetFolderNumberbyClientSite(Int64 ClientId, Int64 SiteId)
        {
            //--- Client-Site folder format : 0004-00025 //
            string ReturnFileNo = string.Empty;
            string ClientMask = string.Empty;
            string SiteMask = string.Empty;
            string mask4 = "0000";
            string mask3 = "000";
            string mask2 = "00";
            string mask1 = "0";

            //---Adding mask to ClientId--//
            if (ClientId <= 9)
            {
                ClientMask = mask3 + ClientId.ToString();
            }
            else if (ClientId > 9 && ClientId <= 99)
            {
                ClientMask = mask2 + ClientId.ToString();
            }
            else if (ClientId > 99 && ClientId <= 999)
            {
                ClientMask = mask1 + ClientId.ToString();
            }
            else
            {
                ClientMask = ClientId.ToString();
            }

            //---Adding mask to SiteId--//
            if (SiteId <= 9)
            {
                SiteMask = mask4 + SiteId.ToString();
            }
            else if (SiteId > 9 && SiteId <= 99)
            {
                SiteMask = mask3 + SiteId.ToString();
            }
            else if (SiteId > 99 && SiteId <= 999)
            {
                SiteMask = mask2 + SiteId.ToString();
            }
            else if (SiteId > 999 && SiteId <= 9999)
            {
                SiteMask = mask1 + SiteId.ToString();
            }
            else
            {
                SiteMask = SiteId.ToString();
            }

            return ReturnFileNo = ClientMask + "-" + SiteMask;
        }

        public string GetFolderNumberbyClient(Int64 ClientId)
        {
            string ReturnFileNo = string.Empty;
            string ClientMask = string.Empty;
            string SiteMask = string.Empty;
            string mask3 = "000";
            string mask2 = "00";
            string mask1 = "0";

            //---Adding mask to ClientId--//
            if (ClientId <= 9)
            {
                ClientMask = mask3 + ClientId.ToString();
            }
            else if (ClientId > 9 && ClientId <= 99)
            {
                ClientMask = mask2 + ClientId.ToString();
            }
            else if (ClientId > 99 && ClientId <= 999)
            {
                ClientMask = mask1 + ClientId.ToString();
            }
            else
            {
                ClientMask = ClientId.ToString();
            }



            return ReturnFileNo = ClientMask;
        }

        public string CreateFileNamebyObject(string tablename, string objectId, string imageName)
        {
            string retName = string.Empty;
            StringBuilder sb = new StringBuilder();
            string delimitter = "/";
            sb.Append(tablename);
            sb.Append(delimitter);
            sb.Append(objectId);
            sb.Append(delimitter);
            sb.Append(imageName);
            retName = sb.ToString();

            return retName;
        }

        public string UploadFileOnPremise(string FilePath, long objectId, string TableName, out string DBFilepath)
        {
            CommonWrapper commonWrapper = new CommonWrapper(userData);

            if (FilePath != "")
            {
                long ClientId = userData.DatabaseKey.Client.ClientId;
                long SiteId = userData.DatabaseKey.User.DefaultSiteId;
                string ObjectIdPath = string.Empty;
                string FolderNumberbyClientAndSite = string.Empty;
                if (TableName.ToLower() == "partmaster" || TableName.ToLower() == "formsettings" || TableName.ToLower() == "client")
                {
                    FolderNumberbyClientAndSite = commonWrapper.GetFolderNumberbyClient(ClientId);
                }
                else
                {
                    FolderNumberbyClientAndSite = commonWrapper.GetFolderNumberbyClientSite(ClientId, SiteId);
                }
                bool isRootPathExists = Directory.Exists(FilePath);
                if (!isRootPathExists)
                    Directory.CreateDirectory(FilePath);
                string ClientSitePathString = Path.Combine(FilePath, FolderNumberbyClientAndSite);
                bool isClientAndSitePathExists = Directory.Exists(ClientSitePathString);

                if (!isClientAndSitePathExists)
                    Directory.CreateDirectory(ClientSitePathString);
                #region ModulePath
                string ModulePathString = Path.Combine(FilePath, FolderNumberbyClientAndSite, TableName);
                bool isModulePathExists = Directory.Exists(ModulePathString);

                if (!isModulePathExists)
                    Directory.CreateDirectory(ModulePathString);
                #endregion ModulePath
                #region ObjectPath
                string ObjectPathString = Path.Combine(FilePath, FolderNumberbyClientAndSite, TableName, Convert.ToString(objectId));
                bool isObjectPathExists = Directory.Exists(ObjectPathString);

                if (!isObjectPathExists)
                    Directory.CreateDirectory(ObjectPathString);
                #endregion ObjectPath
                DBFilepath = Path.Combine(FolderNumberbyClientAndSite, TableName, Convert.ToString(objectId));
                return ObjectPathString;
            }
            else
            {
                DBFilepath = "";
                return "";
            }

        }

        #endregion


        #region V2-637 Asset Lookup for Repairable Spare Asset
        public List<RepariableSpareAssetLookupModel> GetEquipmentRepSpareAssetLookupListGridData(string orderbycol = "", string orderDir = "", int skip = 0, int length = 0, string clientLookupId = "", string Name = "", string Make = "", string Model = "", string Type = "", bool IsAssigned = true)
        {

            RepariableSpareAssetLookupModel newRepSpareLookupModel;
            List<RepariableSpareAssetLookupModel> newRepSpareLookupSearchModelList = new List<RepariableSpareAssetLookupModel>();
            List<Equipment> EquipmentList = new List<Equipment>();
            Equipment equipment = new Equipment();
            equipment.ClientId = this.userData.DatabaseKey.Client.ClientId;
            equipment.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            equipment.OrderbyColumn = orderbycol;
            equipment.OrderBy = orderDir;
            equipment.OffSetVal = skip;
            equipment.NextRow = length;
            equipment.ClientLookupId = clientLookupId;
            equipment.Name = Name;
            equipment.Make = Make;
            equipment.Model = Model;
            equipment.Type = Type;
            equipment.IsAssigned = IsAssigned;

            EquipmentList = equipment.GetEquipmentForRepSpareAssetLookupListV2(userData.DatabaseKey, userData.Site.TimeZone);

            foreach (var item in EquipmentList)
            {
                newRepSpareLookupModel = new RepariableSpareAssetLookupModel();


                newRepSpareLookupModel.EquipmentId = item.EquipmentId;
                newRepSpareLookupModel.ClientLookupId = item.ClientLookupId;
                newRepSpareLookupModel.Name = item.Name;
                newRepSpareLookupModel.Model = item.Model;
                newRepSpareLookupModel.Make = item.Make;
                newRepSpareLookupModel.Type = item.Type;

                newRepSpareLookupModel.TotalCount = item.TotalCount;

                newRepSpareLookupSearchModelList.Add(newRepSpareLookupModel);
            }

            return newRepSpareLookupSearchModelList;
        }
        #endregion

        #region V2-687 Multi Storeroom Support 
        public IEnumerable<SelectListItem> GetStoreroomList(string StoreroomAuthType = "")
        {
            Storeroom storeroom = new Storeroom();
            storeroom.ClientId = userData.DatabaseKey.Client.ClientId;
            storeroom.StoreroomAuthType = StoreroomAuthType;  // StoreroomAuthType to fetch records  are
                                                              //Maintain ,Issue ,IssueTransfer, ReceiveTransfer,PhysicalInventory,Purchase,ReceivePurchase
            storeroom.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            storeroom.PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
            var storeroomList = storeroom.RetrieveStoreroomList(userData.DatabaseKey);
            // V2-1091 - RKL - Storeroom List - the name should include the description 
            return storeroomList.Select(x => new SelectListItem { Text = x.Name + '-' + x.Description, Value = Convert.ToString(x.StoreroomId) });
        }
        #region Retrieve Part Id By StoreroomId And ClientLookUp
        public PartModel RetrievePartIdByStoreroomIdAndClientLookUp(string ClientLookupId = "", string StoreroomId = "")
        {
            Part part = new Part();
            part.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            part.ClientId = userData.DatabaseKey.Client.ClientId;
            part.ClientLookupId = ClientLookupId;
            part.StoreroomId = !string.IsNullOrEmpty(StoreroomId) ? Convert.ToInt64(StoreroomId) : 0;
            Part partResult = new Part();
            if (userData.DatabaseKey.Client.UseMultiStoreroom && part.StoreroomId > 0)
            {
                partResult = part.RetrievePartIdByStoreroomIdAndClientLookupId(userData.DatabaseKey);
            }
            var partModel = new PartModel()
            {
                PartId = partResult.PartId,

            };
            return partModel;
        }
        #endregion
        #endregion

        #region V2-716

        public List<Attachment> GetAzureMultipleImageUrl(int? start, int? length, long objectId, string objectType)
        {

            string filename = string.Empty;
            Attachment attach = new Attachment()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                ObjectName = objectType,
                ObjectId = objectId,
                offset1 = Convert.ToString(start),
                nextrow = Convert.ToString(length)
                //Profile = true,
                //Image = true
            };
            List<Attachment> attachments = attach.RetrieveMultipleProfileAttachments(userData.DatabaseKey, userData.Site.TimeZone);
            attachments.OrderByDescending(a => a.AttachmentId);
            if (attachments.Count > 0) // Check Attachment Table count, If count exists
            {
                foreach (var AList in attachments)
                {
                    bool lExternal = false;
                    string imageurl = string.Empty;
                    lExternal = AList.External;
                    imageurl = AList.AttachmentURL;

                    if (!lExternal && imageurl != "")// 1.If there is an AttachmentURL returned from an associated Attachment record and the External Flag is false
                    {

                        AzureUtil.AzureSetup aset = new AzureUtil.AzureSetup();
                        if (objectType.ToLower() != "partmaster")
                        {
                            imageurl = aset.GetSASUrlClientSite(userData.DatabaseKey.Client.ClientId, userData.DatabaseKey.User.DefaultSiteId, imageurl);
                        }
                        else if (objectType.ToLower() == "partmaster")
                        {
                            imageurl = aset.GetSASUrlClient(userData.DatabaseKey.Client.ClientId, imageurl);//---SAS appended Url------

                        }


                    }
                    else if (lExternal && imageurl != "") // 2.If there is an AttachmentURL returned from an associated Attachment record and the External Flag is true
                    {
                        imageurl = imageurl;
                    }
                    else
                    {
                        imageurl = string.Empty;
                    }
                    AList.AttachmentURL = imageurl;
                }
            }

            return attachments;
        }

        public List<Attachment> GetOnPremiseMultipleImageUrl(int? start, int? length, long objectId, string objectType)
        {
            string imageurl = string.Empty;
            bool lExternal = false;
            string filename = string.Empty;
            Attachment attach = new Attachment()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                ObjectName = objectType,
                ObjectId = objectId,
                offset1 = Convert.ToString(start),    // RKL - 2023-Jan-30
                nextrow = Convert.ToString(length)    // RKL - 2023-Jan-30
                //Profile = true,
                //Image = true
            };
            List<Attachment> AList = attach.RetrieveMultipleProfileAttachments(userData.DatabaseKey, userData.Site.TimeZone);
            //AList.OrderByDescending(a => a.AttachmentId);
            //if (AList.Count > 0) // Check Attachment Table count, If count exists
            //{
            //    lExternal = AList.First().External;
            //    imageurl = AList.First().AttachmentURL;
            //}
            return AList;
        }
        public void SetAsDefaultImage(long AttachmentId, long objectId, string objectName, ref string rtrMsg, ref string imageurl)
        { // Check if there is a profile image attachment record for the object 
            Attachment attach = new Attachment()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                ObjectName = objectName,
                ObjectId = objectId,
                Profile = true,
                Image = true
            };
            attach.ClientId = userData.DatabaseKey.Client.ClientId;
            List<Attachment> AList = attach.RetrieveProfileAttachments(userData.DatabaseKey, userData.Site.TimeZone);
            AList.Where(a => a.Profile == true);
            if (AList.Count > 0)
            {
                // Profile Image Attachment Record Exists
                string image_url = AList.First().AttachmentURL;
                bool external = AList.First().External;
                attach.AttachmentId = AList.First().AttachmentId;
                attach.Retrieve(userData.DatabaseKey);
                attach.Profile = false;
                attach.Description = "";
                attach.Update(userData.DatabaseKey);
                //attach.Delete(userData.DatabaseKey);
                //if (!external)
                //{
                //    AzureUtil.AzureSetup aset = new AzureUtil.AzureSetup();
                //    aset.DeleteBlobByURL(image_url);
                //    rtrMsg = "Success";
                //}
                //else
                //{
                //    rtrMsg = "External";
                //}
            }
            attach.AttachmentId = AttachmentId;
            attach.Retrieve(userData.DatabaseKey);
            imageurl = attach.AttachmentURL;
            attach.Profile = true;
            attach.Description = "Profile Image";
            attach.Update(userData.DatabaseKey);
            if (attach.ErrorMessages != null && attach.ErrorMessages.Count > 0)
            {
                rtrMsg = "Not Success";
                return;
            }
            else
            {
                rtrMsg = "Success";
            }
        }
        #endregion
        #region V2-716 DeleteImage
        public void DeleteAzureMultipleImage(long AttachmentId, long objectId, string objectName, ref string rtrMsg)
        { // Check if there is a profile image attachment record for the object 
            Attachment attach = new Attachment()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                AttachmentId = AttachmentId,
                ObjectName = objectName,
                ObjectId = objectId,
                //Profile = true,
                Image = true
            };
            attach.ClientId = userData.DatabaseKey.Client.ClientId;
            
            attach.Retrieve(userData.DatabaseKey);
            string image_url = attach.AttachmentURL;
            bool external = attach.External;
            attach.Delete(userData.DatabaseKey);
            if (!external)
            {
                AzureUtil.AzureSetup aset = new AzureUtil.AzureSetup();
                aset.DeleteBlobByURL(image_url);
                rtrMsg = "Success";
            }
            else
            {
                rtrMsg = "External";
            }
        }
        public void DeleteOnPremiseMultipleImage(long AttachmentId, long objectId, string objectName, ref string rtrMsg)
        { // Check if there is a profile image attachment record for the object 
            Attachment attach = new Attachment()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                AttachmentId = AttachmentId,
                ObjectName = objectName,
                ObjectId = objectId,
                Image = true
            };
            attach.ClientId = userData.DatabaseKey.Client.ClientId;
            
            attach.Retrieve(userData.DatabaseKey);
            string image_url = attach.AttachmentURL;
            bool external = attach.External;
            attach.Delete(userData.DatabaseKey);
            if (!external)
            {
                string filePath = string.Empty;

                int ConnectRemoteShareErrorCode = 0;
                NetworkCredential credentials = UtilityFunction.GetOnPremiseCredential();

                var OnPremisePath = UtilityFunction.GetOnPremiseDirectory();
                using (new ConnectToSharedFolder(OnPremisePath.NetworkPath, credentials, out ConnectRemoteShareErrorCode))
                {
                    if (ConnectRemoteShareErrorCode == 0)
                    {
                        filePath = Path.Combine(OnPremisePath.NetworkPath, OnPremisePath.RemoteDrivePath, image_url);
                        if (File.Exists(filePath))
                        {
                            File.Delete(filePath);
                            rtrMsg = "Success";
                        }
                        else
                        {
                            rtrMsg = "File does not exist.";
                        }
                    }
                    else
                    {
                        rtrMsg = "Please check your network path and credentials.";
                    }
                }
            }
            else
            {
                rtrMsg = "External";
            }
        }
        public int AttachmentCount_ByObjectAndFileName(long objectId, string tableName, string fileName)
        {
            Attachment attInfo = new Attachment()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                ObjectName = tableName,
                ObjectId = objectId,
                FileName = fileName,
            };
            var attachmentCount = attInfo.RetrieveAttachmentCount_ByObjectAndFileName(userData.DatabaseKey);
            return attachmentCount;
        }
        #endregion
        #region V2-725
        public bool IsduplicateFileExist(long ObjectId, string TableName, bool Security, string FileName, string FileType)
        {
            bool isduplicate = false;
            Attachment objfileAtt = new Attachment();
            objfileAtt.ClientId = userData.DatabaseKey.Client.ClientId;
            objfileAtt.ObjectName = TableName;
            objfileAtt.ObjectId = ObjectId;
            List<Attachment> Alist = objfileAtt.RetrieveAllAttachmentsForObject(userData.DatabaseKey, userData.Site.TimeZone, false, Security);
            if (Alist.Count > 0 && Alist.Any(att => att.FileName == FileName && att.FileType == FileType))
            {
                isduplicate = true;
            }
            return isduplicate;

        }
        #endregion

        public bool CheckIsActiveInterface(string InterfaceType)
        {
            bool IsActive = false;
            InterfaceProp iprop = new InterfaceProp();
            iprop.InterfaceType = InterfaceType;// ApiConstants.PurchaseRequestExport;
            iprop.ClientId = userData.DatabaseKey.Client.ClientId;
            // V2-399 - 2020-Aug-11 - Interfaces are active at the site level
            iprop.SiteId = userData.Site.SiteId;
            iprop.CheckIsActive(userData.DatabaseKey);
            if (iprop.InterfacePropId > 0)
            {
                IsActive = true;
            }
            return IsActive;
        }
        public InterfaceProp RetrieveInterfaceProperties(string InterfaceType)
        {
            InterfaceProp iprop = new InterfaceProp();
            iprop.ClientId = userData.DatabaseKey.Client.ClientId;
            iprop.SiteId = userData.Site.SiteId;
            iprop.InterfaceType = InterfaceType;
            iprop.RetrieveInterfaceProperties(userData.DatabaseKey);
            return iprop;
        }

        #region V2-736
        public List<PartXRefGridDataModel> GetPartLookupListGridData_Mobile(string orderbycol = "", string OrderDirection = "", int skip = 0, int length = 0, string ClientLookupId = "", string Description = "", string Storeroomid = "")
        {
            PartXRefGridDataModel objPartXRefGridDataModel;
            List<PartXRefGridDataModel> PartXRefGridDataModelList = new List<PartXRefGridDataModel>();

            Part part = new Part();
            part.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            part.ClientLookupId = ClientLookupId;
            part.Description = Description;
            part.Page = skip;
            part.ResultsPerPage = length;
            part.OrderbyColumn = orderbycol;
            part.OrderBy = OrderDirection;

            if (userData.DatabaseKey.Client.UseMultiStoreroom && !string.IsNullOrEmpty(Storeroomid))
            {
                part.StoreroomId = !string.IsNullOrEmpty(Storeroomid) ? Convert.ToInt64(Storeroomid) : 0;
                if (part.StoreroomId > 0)
                {
                    part.PartChunkSearchLookupListForMultiStoreroomMobile_V2(userData.DatabaseKey, userData.Site.TimeZone);
                }

            }
            else
            {
                part.PartChunkSearchLookupListMobileV2(userData.DatabaseKey, userData.Site.TimeZone);
            }
            foreach (var item in part.listOfPart)
            {
                objPartXRefGridDataModel = new PartXRefGridDataModel();
                objPartXRefGridDataModel.PartId = item.PartId;
                objPartXRefGridDataModel.ClientLookUpId = item.ClientLookupId;
                objPartXRefGridDataModel.Description = item.Description;
                objPartXRefGridDataModel.TotalCount = item.TotalCount;
                PartXRefGridDataModelList.Add(objPartXRefGridDataModel);
            }
            return PartXRefGridDataModelList;
        }
        #endregion

        public List<AccountLookUpModel> GetAccountLookupListGridDataMobile(string orderbycol = "", string orderDir = "", int skip = 0, int length = 0, string clientLookupId = "", string Name = "")
        {

            AccountLookUpModel newAccountLookUpModel;
            List<AccountLookUpModel> newAccountLookUpSearchModelList = new List<AccountLookUpModel>();
            List<Account> AccountList = new List<Account>();
            Account account = new Account();
            account.ClientId = this.userData.DatabaseKey.Client.ClientId;
            account.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            account.OrderbyColumn = orderbycol;
            account.OrderBy = orderDir;
            account.OffSetVal = skip;
            account.NextRow = length;
            account.ClientLookupId = clientLookupId;
            account.Name = Name;

            AccountList = account.GetAllAccountLookupListMobileV2(userData.DatabaseKey, userData.Site.TimeZone);

            foreach (var item in AccountList)
            {
                newAccountLookUpModel = new AccountLookUpModel();


                newAccountLookUpModel.AccountId = item.AccountId;
                newAccountLookUpModel.ClientLookupId = item.ClientLookupId;
                newAccountLookUpModel.Name = item.Name;
                newAccountLookUpModel.TotalCount = item.TotalCount;

                newAccountLookUpSearchModelList.Add(newAccountLookUpModel);
            }

            return newAccountLookUpSearchModelList;
        }

        #region V2-823
        public List<string> AddOrUpdateCommentWithoutUserMention(NotesModel notesModel, ref string Mode, string tableName)
        {
            Notes notes = new Notes()
            {
                OwnerId = userData.DatabaseKey.User.UserInfoId,
                OwnerName = userData.DatabaseKey.User.FullName,
                Subject = notesModel.Subject ?? "No Subject",
                Content = notesModel.Content,
                Type = notesModel.Type,
                TableName = tableName,
                ObjectId = notesModel.ObjectId,
                UpdateIndex = notesModel.updatedindex,
                NotesId = notesModel.NotesId
            };
            if (notesModel.NotesId == 0)
            {
                Mode = "add";
                notes.Create(this.userData.DatabaseKey);
            }
            else
            {
                Mode = "update";
                notes.Update(this.userData.DatabaseKey);
            }

            return notes.ErrorMessages;
        }
        #endregion

        #region V2-929
        public List<PersonnelLookUpModel> GetActiveAdminOrFullPersonnelLookupListGridData(string orderbycol = "", string orderDir = "", int skip = 0, int length = 0, string clientLookupId = "", string NameFirst = "", string NameLast = "")
        {

            PersonnelLookUpModel personnelLookUpModel;
            List<PersonnelLookUpModel> personnelLookUpModelList = new List<PersonnelLookUpModel>();
            List<Personnel> PersonnelList = new List<Personnel>();
            Personnel personnel = new Personnel();
            personnel.ClientId = this.userData.DatabaseKey.Client.ClientId;
            personnel.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            personnel.OrderbyColumn = orderbycol;
            personnel.OrderBy = orderDir;
            personnel.OffSetVal = skip;
            personnel.NextRow = length;
            personnel.ClientLookupId = clientLookupId;
            personnel.NameFirst = NameFirst;
            personnel.NameLast = NameLast;

            PersonnelList = personnel.RetrieveLookupListForActiveAdminOrFullUser(userData.DatabaseKey);

            foreach (var item in PersonnelList)
            {
                personnelLookUpModel = new PersonnelLookUpModel();


                personnelLookUpModel.PersonnelId = item.PersonnelId;
                personnelLookUpModel.ClientLookupId = item.ClientLookupId;
                personnelLookUpModel.NameFirst = item.NameFirst;
                personnelLookUpModel.NameLast = item.NameLast;
                personnelLookUpModel.TotalCount = item.TotalCount;

                personnelLookUpModelList.Add(personnelLookUpModel);
            }

            return personnelLookUpModelList;
        }
        #endregion

        #region V2-948
        public Equipment GetAccountByEquipmentId(long equipmentId)
        {
            Equipment equipment = new Equipment { ClientId = _dbKey.Client.ClientId, SiteId = _dbKey.User.DefaultSiteId, EquipmentId = equipmentId };
            var result = equipment.RetrieveLaborAccountByEquipmentIdV2(_dbKey);
            return result;
        }
        #endregion

        #region V2-950
        public List<PersonnelLookUpModel> GetActiveAdminOrFullPlannerPersonnelLookupListGridData(string orderbycol = "", string orderDir = "", int skip = 0, int length = 0, string clientLookupId = "", string NameFirst = "", string NameLast = "")
        {

            PersonnelLookUpModel personnelLookUpModel;
            List<PersonnelLookUpModel> personnelLookUpModelList = new List<PersonnelLookUpModel>();
            List<Personnel> PersonnelList = new List<Personnel>();
            Personnel personnel = new Personnel();
            personnel.ClientId = this.userData.DatabaseKey.Client.ClientId;
            personnel.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            personnel.OrderbyColumn = orderbycol;
            personnel.OrderBy = orderDir;
            personnel.OffSetVal = skip;
            personnel.NextRow = length;
            personnel.ClientLookupId = clientLookupId;
            personnel.NameFirst = NameFirst;
            personnel.NameLast = NameLast;

            PersonnelList = personnel.RetrieveLookupListForActiveAdminOrFullUserPlanner(userData.DatabaseKey);

            foreach (var item in PersonnelList)
            {
                personnelLookUpModel = new PersonnelLookUpModel();


                personnelLookUpModel.PersonnelId = item.PersonnelId;
                personnelLookUpModel.ClientLookupId = item.ClientLookupId;
                personnelLookUpModel.NameFirst = item.NameFirst;
                personnelLookUpModel.NameLast = item.NameLast;
                personnelLookUpModel.TotalCount = item.TotalCount;

                personnelLookUpModelList.Add(personnelLookUpModel);
            }

            return personnelLookUpModelList;
        }

        public List<MetersModel> GetMeterLookupListGridData(string orderbycol = "", string orderDir = "", int skip = 0, int length = 0, string clientLookupId = "", string Name = "")
        {
            MetersModel objMetersModel;
            List<MetersModel> meterList = new List<MetersModel>();
            Meter meter = new Meter();
            meter.ClientId = this.userData.DatabaseKey.Client.ClientId;
            meter.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            meter.OrderbyColumn = orderbycol;
            meter.OrderBy = orderDir;
            meter.OffSetVal = skip;
            meter.NextRow = length;
            meter.ClientLookupId = clientLookupId;
            meter.Name = Name;

            List<Meter> meters = meter.RetrieveForTableLookupList_V2(this.userData.DatabaseKey);
            foreach (var item in meters)
            {
                objMetersModel = new MetersModel();
                objMetersModel.MeterClientLookUpId = item.ClientLookupId;
                objMetersModel.MeterId = item.MeterId;
                objMetersModel.MeterName = item.Name;
                objMetersModel.ReadingCurrent = item.ReadingCurrent;
                objMetersModel.TotalCount = item.TotalCount;
     meterList.Add(objMetersModel);
            }
            return meterList;
        }
        #endregion

        #region V2-981
        #region Purchase Order Lookuplist chunk
        public List<PurchaseOrderLookupModel> GetPurchaseOrderLookupListGridData(string orderbycol = "", string orderDir = "",
            int skip = 0, int length = 0, string poclientLookupId = "", string status = "", string vendorClientLookupId = "", string vendorName = "")
        {

            PurchaseOrderLookupModel purchaseOrderLookupModel;
            List<PurchaseOrderLookupModel> purchaseOrderLookupModelList = new List<PurchaseOrderLookupModel>();
            List<PurchaseOrder> PurchaseOrderList = new List<PurchaseOrder>();
            PurchaseOrder purchaseOrder = new PurchaseOrder();
            purchaseOrder.ClientId = this.userData.DatabaseKey.Client.ClientId;
            purchaseOrder.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            purchaseOrder.orderbyColumn = orderbycol;
            purchaseOrder.orderBy = orderDir;
            purchaseOrder.offset1 = Convert.ToString(skip);
            purchaseOrder.nextrow = Convert.ToString(length);
            purchaseOrder.ClientLookupId = poclientLookupId;
            purchaseOrder.Status = status;
            purchaseOrder.VendorClientLookupId = vendorClientLookupId;
            purchaseOrder.VendorName = vendorName;




            PurchaseOrderList = purchaseOrder.GetAllPurchaseOrderLookupListV2(userData.DatabaseKey, userData.Site.TimeZone);

            foreach (var item in PurchaseOrderList)
            {
                purchaseOrderLookupModel = new PurchaseOrderLookupModel();
                purchaseOrderLookupModel.PurchaseOrderId = item.PurchaseOrderId;
                purchaseOrderLookupModel.POClientLookupId = item.ClientLookupId;
                purchaseOrderLookupModel.Status = item.Status;
                purchaseOrderLookupModel.VendorClientLookupId = item.VendorClientLookupId;
                purchaseOrderLookupModel.VendorName = item.VendorName;
                purchaseOrderLookupModel.TotalCount = item.TotalCount;
                purchaseOrderLookupModelList.Add(purchaseOrderLookupModel);
            }

            return purchaseOrderLookupModelList;
        }
        #endregion

        #region Personnel chunk lookup list
        public List<PersonnelLookUpModel> GetPersonnelLookupListGridData(string orderbycol = "", string orderDir = "", int skip = 0, int length = 0, string PersonnelClientLookupId = "", string PersonnelNameFirst = "", string PersonnelNameLast = "")
        {

            PersonnelLookUpModel personnelLookUpModel;
            List<PersonnelLookUpModel> personnelLookUpModelList = new List<PersonnelLookUpModel>();
            List<Personnel> PersonnelList = new List<Personnel>();
            Personnel personnel = new Personnel();
            personnel.ClientId = this.userData.DatabaseKey.Client.ClientId;
            personnel.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            personnel.OrderbyColumn = orderbycol;
            personnel.OrderBy = orderDir;
            personnel.OffSetVal = skip;
            personnel.NextRow = length;
            personnel.ClientLookupId = PersonnelClientLookupId;
            personnel.NameFirst = PersonnelNameFirst;
            personnel.NameLast = PersonnelNameLast;

            PersonnelList = personnel.GetAllPersonnelLookupListV2(userData.DatabaseKey, userData.Site.TimeZone);

            foreach (var item in PersonnelList)
            {
                personnelLookUpModel = new PersonnelLookUpModel();
                personnelLookUpModel.PersonnelId = item.PersonnelId;
                personnelLookUpModel.PClientLookupId = item.ClientLookupId;
                personnelLookUpModel.NameFirst = item.NameFirst;
                personnelLookUpModel.NameLast = item.NameLast;
                personnelLookUpModel.TotalCount = item.TotalCount;
                personnelLookUpModelList.Add(personnelLookUpModel);
            }

            return personnelLookUpModelList;
        }
        #endregion

        #endregion

        #region V2-536
        public List<SensorAlertModel> GetActiveSensorAlertProcedureLookupListGridData(string orderbycol = "", string orderDir = "", int skip = 0, int length = 0, string clientLookupId = "", string Description = "", string Type = "")
        {
            SensorAlertModel objSensorAlertModel;
        List<SensorAlertModel> sensorAlertProcList = new List<SensorAlertModel>();
            SensorAlertProcedure sensorAlertProcedure = new SensorAlertProcedure();
            sensorAlertProcedure.ClientId = this.userData.DatabaseKey.Client.ClientId;
            sensorAlertProcedure.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            sensorAlertProcedure.OrderbyColumn = orderbycol;
            sensorAlertProcedure.OrderBy = orderDir;
            sensorAlertProcedure.OffSetVal = skip;
            sensorAlertProcedure.NextRow = length;
            sensorAlertProcedure.ClientLookUpId = clientLookupId;
            sensorAlertProcedure.Description = Description;
            sensorAlertProcedure.Type = Type;

            List<SensorAlertProcedure> meters = sensorAlertProcedure.RetrieveForActiveTableLookupList_V2(this.userData.DatabaseKey);
            foreach (var item in meters)
            {
                objSensorAlertModel = new SensorAlertModel();
                objSensorAlertModel.ClientLookUpId = item.ClientLookUpId;
                objSensorAlertModel.SensorAlertProcedureId = item.SensorAlertProcedureId;
                objSensorAlertModel.Description = item.Description;
                objSensorAlertModel.Type = item.Type;
                objSensorAlertModel.TotalCount = item.TotalCount;
                sensorAlertProcList.Add(objSensorAlertModel);
            }
            return sensorAlertProcList;
        }
        #endregion

        #region V2-846
        public List<Equipment> GetAllParentChunkList(int skip = 0, int length = 0, string SearchText = "")
        {
            Equipment equipment = new Equipment();
            equipment.ClientId = userData.DatabaseKey.Client.ClientId;
            equipment.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            equipment.OffSetVal = skip;
            equipment.NextRow = length;
            equipment.SearchText = SearchText;

            List<Equipment> equipmentList = equipment.GetAllEquipmentParent(this.userData.DatabaseKey);
            return equipmentList;
        }
        public List<Equipment> GetAllChildrenForEquipmentTree(long EquipmentId)
        {
            Equipment equipment = new Equipment();
            equipment.ClientId = userData.DatabaseKey.Client.ClientId;
            equipment.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            equipment.EquipmentId = EquipmentId;

            List<Equipment> equipmentList = equipment.GetAllEquipmentChildrenForParent(this.userData.DatabaseKey);
            return equipmentList;
        }
        #endregion
        #region V2-1068 PartCategorylookuplist
        public List<CategoryMasterModel> GetPartCategoryMasterLookupListGridData
            (string orderbycol = "", string orderDir = "",
    int skip = 0, int length = 0, string _clientLookupId = "", string _description = "")
        {
            List<CategoryMasterModel> CategoryMasterModelList = new List<CategoryMasterModel>();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            CategoryMasterModel categoryMasterModel;
            PartCategoryMaster category = new PartCategoryMaster();
            category.ClientId = userData.DatabaseKey.Client.ClientId;
            category.orderbyColumn = orderbycol;
            category.orderBy = orderDir;
            category.offset1 = skip;
            category.nextrow = length;
            category.ClientLookupId = _clientLookupId;
            category.Description = _description;

            var cList = category.RetrievelookuplistChunkSearch(userData.DatabaseKey);

            foreach (var item in cList)
            {
                categoryMasterModel = new CategoryMasterModel();
                categoryMasterModel.PartCategoryMasterId = item.PartCategoryMasterId;
                categoryMasterModel.ClientLookupId = item.ClientLookupId;
                categoryMasterModel.Description = item.Description;
                categoryMasterModel.TotalCount = item.totalCount;
                CategoryMasterModelList.Add(categoryMasterModel);
            }

            return CategoryMasterModelList;
        }

        #endregion
        #region Vendor Lookuplist chunk search for Mobile
        public List<VendorLookupModel> GetVendorLookupListGridData_ForMobile(string orderbycol = "", string orderDir = "",
            int skip = 0, int length = 0, string TextSearch = "")
        {
            VendorLookupModel newVendorLookupModel;
            List<VendorLookupModel> newVendorLookupSearchModelList = new List<VendorLookupModel>();
            List<Vendor> VendortList = new List<Vendor>();
            Vendor vendor = new Vendor();
            vendor.ClientId = this.userData.DatabaseKey.Client.ClientId;
            vendor.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            vendor.orderbyColumn = orderbycol;
            vendor.orderBy = orderDir;
            vendor.offset1 = skip;
            vendor.nextrow = length;
            vendor.SearchText = TextSearch;

            VendortList = vendor.GetAllVendorLookupListV2(userData.DatabaseKey, userData.Site.TimeZone);

            foreach (var item in VendortList)
            {
                newVendorLookupModel = new VendorLookupModel();
                newVendorLookupModel.VendorID = item.VendorId;
                newVendorLookupModel.ClientLookupId = item.ClientLookupId;
                newVendorLookupModel.Name = item.Name;
                newVendorLookupModel.TotalCount = item.totalCount;
                newVendorLookupSearchModelList.Add(newVendorLookupModel);
            }
            return newVendorLookupSearchModelList;
        }
        #endregion

        #region V2-1068 PartCategorylookuplist For Mobile
        public List<CategoryMasterModel> GetPartCategoryMasterLookupListGridData_ForMobile
            (string orderbycol = "", string orderDir = "",
    int skip = 0, int length = 0, string TextSearch = "")
        {
            List<CategoryMasterModel> CategoryMasterModelList = new List<CategoryMasterModel>();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            CategoryMasterModel categoryMasterModel;
            PartCategoryMaster category = new PartCategoryMaster();
            category.ClientId = userData.DatabaseKey.Client.ClientId;
            category.orderbyColumn = orderbycol;
            category.orderBy = orderDir;
            category.offset1 = skip;
            category.nextrow = length;
            category.SearchText = TextSearch;

            var cList = category.RetrievelookuplistChunkSearch(userData.DatabaseKey);

            foreach (var item in cList)
            {
                categoryMasterModel = new CategoryMasterModel();
                categoryMasterModel.PartCategoryMasterId = item.PartCategoryMasterId;
                categoryMasterModel.ClientLookupId = item.ClientLookupId;
                categoryMasterModel.Description = item.Description;
                categoryMasterModel.TotalCount = item.totalCount;
                CategoryMasterModelList.Add(categoryMasterModel);
            }
            return CategoryMasterModelList;
        }
        #endregion
        #region V2-1076  lookuplist for personnel planner 

        public List<PersonnelLookUpModel> GetActiveAdminOrFullPlannerPersonnelLookupListGridData_Mobile(
    string orderbycol = "", string orderDir = "", int skip = 0, int length = 0,
    string clientLookupId = "", string NameFirst = "", string NameLast = "")
        {
            var personnel = new Personnel
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                SiteId = userData.DatabaseKey.User.DefaultSiteId,
                OrderbyColumn = orderbycol,
                OrderBy = orderDir,
                OffSetVal = skip,
                NextRow = length,
                ClientLookupId = clientLookupId
            };

            var personnelList = personnel.RetrieveLookupListForActiveAdminOrFullUserPlanner(userData.DatabaseKey);

            return personnelList.Select(item => new PersonnelLookUpModel
            {
                PersonnelId = item.PersonnelId,
                ClientLookupId = item.ClientLookupId,
                NameFirst = item.NameFirst,
                NameLast = item.NameLast,
                TotalCount = item.TotalCount
            }).ToList();
        }

        #endregion
        #region V2-1167 Part LookupList chunk for Single Page 
        public List<PartXRefGridDataModel> GetPartLookupListGridDataForSingleStockLineItem(string orderbycol = "", string OrderDirection = "", int skip = 0, int length = 0, string ClientLookupId = "", string Description = "", string UPCcode = "", string Manufacturer = "", string ManufacturerId = "", string StockType = "", string Storeroomid = "", string VendorId = "")
        {
            PartXRefGridDataModel objPartXRefGridDataModel;
            List<PartXRefGridDataModel> PartXRefGridDataModelList = new List<PartXRefGridDataModel>();

            Part part = new Part();
            part.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            part.ClientLookupId = ClientLookupId;
            part.Description = Description;
            part.UPCCode = UPCcode;
            part.Manufacturer = Manufacturer;
            part.ManufacturerId = ManufacturerId;
            part.StockType = StockType;
            part.Page = skip;
            part.ResultsPerPage = length;
            part.OrderbyColumn = orderbycol;
            part.OrderBy = OrderDirection;
            part.VendorId = !string.IsNullOrEmpty(VendorId) ? Convert.ToInt64(VendorId) : 0;
            if (userData.DatabaseKey.Client.UseMultiStoreroom && !string.IsNullOrEmpty(Storeroomid))
            {
                part.StoreroomId = !string.IsNullOrEmpty(Storeroomid) ? Convert.ToInt64(Storeroomid) : 0;
                if (part.StoreroomId > 0)
                {
                    part.PartChunkSearchLookupListForSingleStockLineItemMultiStoreroom_V2(userData.DatabaseKey, userData.Site.TimeZone);
                }

            }
            else
            {
                part.PartChunkSearchLookupListForSingleStockLineItemV2(userData.DatabaseKey, userData.Site.TimeZone);
            }
            foreach (var item in part.listOfPart)
            {
                objPartXRefGridDataModel = new PartXRefGridDataModel();
                objPartXRefGridDataModel.PartId = item.PartId;
                objPartXRefGridDataModel.ClientLookUpId = item.ClientLookupId;
                objPartXRefGridDataModel.Description = item.Description;
                objPartXRefGridDataModel.Manufacturer = item.Manufacturer;
                objPartXRefGridDataModel.ManufacturerID = item.ManufacturerId;
                objPartXRefGridDataModel.UPCcode = item.UPCCode;
                objPartXRefGridDataModel.StockType = item.StockType;
                #region V2-1124
                objPartXRefGridDataModel.IssueUnit = item.IssueUnit;
                objPartXRefGridDataModel.AppliedCost = item.AppliedCost;
                #endregion
                objPartXRefGridDataModel.TotalCount = item.TotalCount;
                objPartXRefGridDataModel.PartStoreroomId = item.PartStoreroomId;
                PartXRefGridDataModelList.Add(objPartXRefGridDataModel);
            }
            return PartXRefGridDataModelList;
        }
        #endregion

        #region V2-1178
        public List<Storeroom> GetStoreroomListByClientIdSiteId()
        {
            LookUpListModel model = new LookUpListModel();
            Storeroom storeroom = new Storeroom()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                SiteId = this.userData.DatabaseKey.User.DefaultSiteId
            };

            List<Storeroom> StoreroomList = storeroom.RetrieveAllStoreroomForLookupList(this.userData.DatabaseKey);
            return StoreroomList;
        }

        public List<PersonnelLookUpModel> GetActiveAdminOrFullChunkSearchPersonnelLookupListGridData(string orderbycol = "", string orderDir = "", int skip = 0, int length = 0, string clientLookupId = "", string NameFirst = "", string NameLast = "", string SearchText = "")
        {

            PersonnelLookUpModel personnelLookUpModel;
            List<PersonnelLookUpModel> personnelLookUpModelList = new List<PersonnelLookUpModel>();
            List<Personnel> PersonnelList = new List<Personnel>();
            Personnel personnel = new Personnel();
            personnel.ClientId = this.userData.DatabaseKey.Client.ClientId;
            personnel.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            personnel.OrderbyColumn = orderbycol;
            personnel.OrderBy = orderDir;
            personnel.OffSetVal = skip;
            personnel.NextRow = length;
            personnel.ClientLookupId = clientLookupId;
            personnel.NameFirst = NameFirst;
            personnel.NameLast = NameLast;
            personnel.SearchText = SearchText;

            PersonnelList = personnel.Personnel_RetrieveChunkSearchForActiveAdminOrFullUser_V2(userData.DatabaseKey);

            foreach (var item in PersonnelList)
            {
                personnelLookUpModel = new PersonnelLookUpModel();


                personnelLookUpModel.PersonnelId = item.PersonnelId;
                personnelLookUpModel.ClientLookupId = item.ClientLookupId;
                personnelLookUpModel.NameFirst = item.NameFirst;
                personnelLookUpModel.NameLast = item.NameLast;
                personnelLookUpModel.TotalCount = item.TotalCount;

                personnelLookUpModelList.Add(personnelLookUpModel);
            }

            return personnelLookUpModelList;
        }
        #endregion
    }

}
