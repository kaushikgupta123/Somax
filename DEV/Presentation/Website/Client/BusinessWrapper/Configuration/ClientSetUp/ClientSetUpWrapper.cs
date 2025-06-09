using Client.BusinessWrapper.Common;
using Client.Common;
using Client.Models.Configuration.ClientSetUp;
using Common.Constants;
using DataContracts;

using INTDataLayer.BAL;
using INTDataLayer.EL;

using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;

namespace Client.BusinessWrapper.Configuration.ClientSetUp
{
    public class ClientSetUpWrapper
    {
        private DatabaseKey _dbKey;
        private UserData userData;
        List<string> errorMessage = new List<string>();
        public ClientSetUpWrapper(UserData userData)
        {
            this.userData = userData;
            _dbKey = userData.DatabaseKey;
        }
        internal DataContracts.Client ClientDetails()
        {
            DataContracts.Client CL = new DataContracts.Client();
            CL.ClientId = userData.DatabaseKey.Client.ClientId;
            CL.Retrieve(this.userData.DatabaseKey);
            return CL;
        }
        //internal DataContracts.PasswordSettings PasswordSettingsDetails()
        //{
        //    DataContracts.PasswordSettings PS = new DataContracts.PasswordSettings();
        //    PS.ClientId = userData.DatabaseKey.Client.ClientId;
        //    PS.RetrieveByClientId(this.userData.DatabaseKey);
        //    return PS;
        //}

        internal PasswordSettingsModel PasswordSettingsDetails()
        {
            PasswordSettingsModel PasswordSettingsModel = new PasswordSettingsModel();
            DataContracts.PasswordSettings PS = new DataContracts.PasswordSettings();
            PS.ClientId = userData.DatabaseKey.Client.ClientId;
            PS.RetrieveByClientId(this.userData.DatabaseKey);
            PasswordSettingsModel.MaxAttempts = PS.MaxAttempts;
            PasswordSettingsModel.PWReqMinLength = PS.PWReqMinLength;
            PasswordSettingsModel.PWMinLength = PS.PWMinLength;
            PasswordSettingsModel.PWReqExpiration = PS.PWReqExpiration;
            PasswordSettingsModel.PWExpiresDays = PS.PWExpiresDays;
            PasswordSettingsModel.PWRequireNumber = PS.PWRequireNumber;
            PasswordSettingsModel.PWRequireAlpha = PS.PWRequireAlpha;
            PasswordSettingsModel.PWRequireMixedCase = PS.PWRequireMixedCase;
            PasswordSettingsModel.PWRequireSpecialChar = PS.PWRequireSpecialChar;
            PasswordSettingsModel.PWNoRepeatChar = PS.PWNoRepeatChar;
            PasswordSettingsModel.PWNotEqualUserName = PS.PWNotEqualUserName;
            PasswordSettingsModel.AllowAdminReset = PS.AllowAdminReset;

            return PasswordSettingsModel;
        }
        //   RetrieveByClientId
        internal List<Attachment> GetListAttachmentL()
        {
            Attachment attach = new Attachment();
            attach.ClientId = this.userData.DatabaseKey.Client.ClientId;
            List<Attachment> AList = attach.RetrieveAllAttachment(this.userData.DatabaseKey, this.userData.Site.TimeZone).OrderByDescending(a => a.CreateDate)
                                             .Where(x => x.ClientId == this.userData.DatabaseKey.Client.ClientId
                                             && x.ObjectName == "Client"
                                             && x.ObjectId == this.userData.DatabaseKey.Client.ClientId
                                             && x.Profile == true
                                             && x.Image == true).ToList();
            return AList;
        }
        internal DataTable GetClientLog()
        {
            INTDataLayer.EL.ClientLogoEL ce = new INTDataLayer.EL.ClientLogoEL();
            ce.ClientId = this.userData.DatabaseKey.Client.ClientId;
            ce.SiteId = 0;
            ce.CallerUserName = this.userData.DatabaseKey.UserName;
            DataTable dt = INTDataLayer.BAL.ClientLogoBL.GetLogoByClientSite(ce, this.userData.DatabaseKey.ClientConnectionString);
            return dt;
        }
        internal List<String> UpdateClientSetup(ClientSetUpModel objM, ref string ErrorMsg, ref long UpdateIndex)
        {
            DataContracts.Client CL = new DataContracts.Client();
            CL.ClientId = userData.DatabaseKey.Client.ClientId;
            CL.Retrieve(this.userData.DatabaseKey);
            if (CL.IsValid)
            {
                CL.CompanyName = objM.CompanyName;
                CL.PrimaryContact = objM.PrimaryContact;
                CL.OfficerPhone = objM.OfficerPhone ?? string.Empty;
                CL.Email = objM.Email;
                CL.WOPrintMessage = objM.WOPrintMessage ?? string.Empty;
                CL.AssetTree = objM.AssetTree;
                CL.PMWOGenerateMethod = objM.PMWOGenerateMethod ?? string.Empty;
                CL.MasterSanGenerateMethod = objM.MasterSanGenerateMethod ?? string.Empty;
                CL.UpdateIndex = objM.UpdateIndex;
                CL.Update(this.userData.DatabaseKey);
                UpdateIndex = CL.UpdateIndex;
            }
            return CL.ErrorMessages;
        }


        public List<string> UpdatePasswordSettings(PasswordSettingsModel objM, ref string ErrorMsg)
        {
            DataContracts.PasswordSettings PS = new DataContracts.PasswordSettings();
            PS.ClientId = userData.DatabaseKey.Client.ClientId;
            PS.MaxAttempts = objM.MaxAttempts;
            PS.PWReqMinLength = objM.PWReqMinLength;
            PS.PWMinLength = objM.PWMinLength ?? 0;
            PS.PWReqExpiration = objM.PWReqExpiration;
            PS.PWExpiresDays = objM.PWExpiresDays ?? 0;
            PS.PWRequireNumber = objM.PWRequireNumber;
            PS.PWRequireAlpha = objM.PWRequireAlpha;
            PS.PWRequireMixedCase = objM.PWRequireMixedCase;
            PS.PWRequireSpecialChar = objM.PWRequireSpecialChar;
            PS.PWNoRepeatChar = objM.PWNoRepeatChar;
            PS.PWNotEqualUserName = objM.PWNotEqualUserName;
            PS.AllowAdminReset = objM.AllowAdminReset;
            PS.UpdatePasswordSettingsByClientId(userData.DatabaseKey);
            return PS.ErrorMessages;

        }

        internal Int64 UploadPhoto(byte[] objM, ref string ErrorMsg)
        {
            ClientLogoBL cb = new ClientLogoBL();
            ClientLogoEL ce = new ClientLogoEL();
            ce.ClientId = this.userData.DatabaseKey.Client.ClientId;
            ce.SiteId = 0;
            ce.Type = "Forms";
            ce.Image = objM;
            ce.CallerUserName = this.userData.DatabaseKey.UserName;
            Int64 Id = ClientLogoBL.InsertUpdateLogo(ce, this.userData.DatabaseKey.ClientConnectionString);
            return Id;
        }
        internal long DeletePhoto()
        {
            long retval = 0;
            ClientLogoEL ce = new ClientLogoEL();
            ce.CallerUserInfoId = this.userData.DatabaseKey.User.UserInfoId;
            ce.ClientId = this.userData.DatabaseKey.Client.ClientId;
            ce.SiteId = 0;  // Logo is Client not Site Speficic this.UserData.Site.SiteId;
            ce.CallerUserName = this.userData.DatabaseKey.UserName;
            ce.Image = new byte[0];
            DataTable dt = ClientLogoBL.GetLogoByClientSite(ce, this.userData.DatabaseKey.ClientConnectionString);
            if (dt != null && dt.Rows.Count > 0)
            {
                ce.LogoId = Convert.ToInt64(dt.Rows[0]["LogoId"]);
                if (dt.Rows[0]["LogoId"] != null && dt.Rows[0]["LogoId"].ToString() != "")
                {
                    byte[] b = new byte[0];
                    retval = ClientLogoBL.UpdateLogo(ce, this.userData.DatabaseKey.ClientConnectionString);
                }
            }
            return retval;
        }
        #region v2-634

        internal WoCompletionSettingsModel CompletionSettingsDetails()
        {
            WoCompletionSettingsModel CompletionSettingsModel = new WoCompletionSettingsModel();
            DataContracts.WOCompletionSettings WOCS = new DataContracts.WOCompletionSettings();
            WOCS.ClientId = userData.DatabaseKey.Client.ClientId;
            WOCS.RetrieveByClientId(this.userData.DatabaseKey);
            CompletionSettingsModel.WOCompletionSettingsId = WOCS.WOCompletionSettingsId;
            CompletionSettingsModel.UseWOCompletionWizard = WOCS.UseWOCompletionWizard;
            CompletionSettingsModel.WOCommentTab = WOCS.WOCommentTab;
            CompletionSettingsModel.TimecardTab = WOCS.TimecardTab;
            CompletionSettingsModel.AutoAddTimecard = WOCS.AutoAddTimecard;
            //V2-728
            CompletionSettingsModel.WOCompCriteriaTab = WOCS.WOCompCriteriaTab;
            CompletionSettingsModel.WOCompCriteriaTitle = WOCS.WOCompCriteriaTitle;
            CompletionSettingsModel.WOCompCriteria = WOCS.WOCompCriteria;

            return CompletionSettingsModel;
        }
        public List<string> UpdateCompletionSettings(WoCompletionSettingsModel objM, ref string ErrorMsg)
        {
            DataContracts.WOCompletionSettings PS = new DataContracts.WOCompletionSettings();
            PS.ClientId = userData.DatabaseKey.Client.ClientId;
            PS.RetrieveByClientId(this.userData.DatabaseKey);
            PS.UseWOCompletionWizard = objM.UseWOCompletionWizard;
            PS.WOCommentTab = objM.WOCommentTab;
            PS.TimecardTab = objM.TimecardTab;
            PS.AutoAddTimecard = objM.AutoAddTimecard;
            PS.WOCompCriteriaTab = objM.WOCompCriteriaTab;//V2-728   
            if (PS.WOCompletionSettingsId == 0)
            {
                PS.Create(userData.DatabaseKey);
            }
            else
            {
                PS.Update(userData.DatabaseKey);
            }

            return PS.ErrorMessages;

        }

        #endregion

        #region V2-728 WO Completion Criteria
        public List<string> UpdateWOCompletionCriteria(WoCompletionCriteriaSetupModel objM, ref string ErrorMsg)
        {
            DataContracts.WOCompletionSettings PS = new DataContracts.WOCompletionSettings();
            PS.ClientId = userData.DatabaseKey.Client.ClientId;
            PS.RetrieveByClientId(this.userData.DatabaseKey);
            PS.WOCompCriteriaTitle = objM.WOCompCriteriaTitle;
            PS.WOCompCriteria = objM.WOCompCriteria;
            if (objM.WOCompCriteriaTab == true) {
                if (PS.WOCompletionSettingsId == 0)
                {
                    PS.WOCompCriteriaTab = objM.WOCompCriteriaTab;
                    PS.Create(userData.DatabaseKey);
                }
                else
                {
                    PS.Update(userData.DatabaseKey);
                }
            }

            return PS.ErrorMessages;

        }
        #endregion

        #region V2-944 Form Settings
        internal WoFormSettingsSetupModel FormSettingsDetails()
        {
            WoFormSettingsSetupModel woFormSettingsSetupModel = new WoFormSettingsSetupModel();
            DataContracts.FormSettings formSettings = new DataContracts.FormSettings();
            formSettings.ClientId = userData.DatabaseKey.Client.ClientId;
            formSettings.RetrieveByClientId(this.userData.DatabaseKey);
            woFormSettingsSetupModel.FormSettingsId = formSettings.FormSettingsId;
            woFormSettingsSetupModel.WOLaborRecording = formSettings.WOLaborRecording;
            woFormSettingsSetupModel.WOUIC = formSettings.WOUIC;
            woFormSettingsSetupModel.WOScheduling = formSettings.WOScheduling;
            woFormSettingsSetupModel.WOSummary = formSettings.WOSummary;
            woFormSettingsSetupModel.WOPhotos = formSettings.WOPhotos;
            woFormSettingsSetupModel.WOComments = formSettings.WOComments;
            //V2-945
            woFormSettingsSetupModel.PRUIC = formSettings.PRUIC;
            woFormSettingsSetupModel.PRLine2 = formSettings.PRLine2;
            woFormSettingsSetupModel.PRLIUIC = formSettings.PRLIUIC;
            woFormSettingsSetupModel.PRComments = formSettings.PRComments;
            //V2-946
            woFormSettingsSetupModel.POUIC = formSettings.POUIC;
            woFormSettingsSetupModel.POLine2 = formSettings.POLine2;
            woFormSettingsSetupModel.POLIUIC = formSettings.POLIUIC;
            woFormSettingsSetupModel.POComments = formSettings.POComments;
            woFormSettingsSetupModel.POTandC = formSettings.POTandC;
            woFormSettingsSetupModel.POTandCURL = formSettings.POTandCURL;
            //V2-947
            woFormSettingsSetupModel.PORHeader = formSettings.PORHeader;
            woFormSettingsSetupModel.PORLine2 = formSettings.PORLine2;
            woFormSettingsSetupModel.PORPrint = formSettings.PORPrint;
            //V2-1011
            woFormSettingsSetupModel.PORUIC = formSettings.PORUIC;
            woFormSettingsSetupModel.PORComments = formSettings.PORComments;
            return woFormSettingsSetupModel;
        }

        public List<string> UpdateFormSettings(WoFormSettingsSetupModel objM, ref string ErrorMsg)
        {
            DataContracts.FormSettings formSettings = new DataContracts.FormSettings();
            formSettings.ClientId = userData.DatabaseKey.Client.ClientId;
            formSettings.RetrieveByClientId(this.userData.DatabaseKey);
            formSettings.WOLaborRecording = objM.WOLaborRecording;
            formSettings.WOUIC = objM.WOUIC;
            formSettings.WOScheduling = objM.WOScheduling;
            formSettings.WOSummary = objM.WOSummary;
            formSettings.WOPhotos = objM.WOPhotos;
            formSettings.WOComments = objM.WOComments;
            if (formSettings.FormSettingsId == 0)
            {
                formSettings.Create(userData.DatabaseKey);
            }
            else
            {
                formSettings.Update(userData.DatabaseKey);
            }

            return formSettings.ErrorMessages;

        }
        #endregion
        #region V2-945
        public List<string> UpdatePRFormSettings(WoFormSettingsSetupModel objM, ref string ErrorMsg)
        {
            DataContracts.FormSettings formSettings = new DataContracts.FormSettings();
            formSettings.ClientId = userData.DatabaseKey.Client.ClientId;
            formSettings.RetrieveByClientId(this.userData.DatabaseKey);
            formSettings.PRUIC = objM.PRUIC;
            formSettings.PRLine2 = objM.PRLine2;
            formSettings.PRLIUIC = objM.PRLIUIC;
            formSettings.PRComments = objM.PRComments;
            if (formSettings.FormSettingsId == 0)
            {
                formSettings.Create(userData.DatabaseKey);
            }
            else
            {
                formSettings.Update(userData.DatabaseKey);
            }

            return formSettings.ErrorMessages;

        }
        #endregion

        #region V2-946
        public List<string> UpdatePOFormSettings(WoFormSettingsSetupModel objM, ref string ErrorMsg, ref string attachurl)
        {
            var rtrMsg = "";
            bool deleteResult = false;
            FormSettings formSettings = new FormSettings();
            formSettings.ClientId = userData.DatabaseKey.Client.ClientId;
            formSettings.RetrieveByClientId(this.userData.DatabaseKey);
            formSettings.POUIC = objM.POUIC;
            formSettings.POLine2 = objM.POLine2;
            formSettings.POLIUIC = objM.POLIUIC;
            formSettings.POComments = objM.POComments;
            formSettings.POTandC = objM.POTandC;
            attachurl = formSettings.POTandCURL;
            if (formSettings.FormSettingsId == 0)
            {
                formSettings.POTandCURL = string.Empty;
                formSettings.Create(userData.DatabaseKey);
            }
            else
            {
                if (!formSettings.POTandC && formSettings.POTandCURL != "")
                {
                    if (userData.DatabaseKey.Client.OnPremise)
                    {
                        deleteResult = DeleteAttachmentOnPremise(formSettings.POTandCURL, ref rtrMsg);
                        if (rtrMsg != "")
                        {
                            formSettings.ErrorMessages.Add(rtrMsg);
                            return formSettings.ErrorMessages;
                        }
                    }
                    else
                    {
                        deleteResult = DeleteAttachment(formSettings.POTandCURL);
                    }
                    if(deleteResult)
                    {
                        formSettings.POTandCURL = string.Empty;
                        attachurl = formSettings.POTandCURL;
                    }
                }
                formSettings.Update(userData.DatabaseKey);
            }

            return formSettings.ErrorMessages;

        }
        public FormSettings SaveWithTandCURL(WoFormSettingsSetupModel objAttachment, Stream stream1, ref bool attachStatus, ref string attachurl)
        {
            FormSettings formSettings = new FormSettings();
            formSettings.ClientId = userData.DatabaseKey.Client.ClientId;
            formSettings.RetrieveByClientId(this.userData.DatabaseKey);
            formSettings.POUIC = objAttachment.POUIC;
            formSettings.POLine2 = objAttachment.POLine2;
            formSettings.POLIUIC = objAttachment.POLIUIC;
            formSettings.POComments = objAttachment.POComments;
            formSettings.POTandC = objAttachment.POTandC;
            var attachmentAllowed = "pdf";
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
                        fileName = aset.CreateFileNamebyObject("FormSettings", objAttachment.FormSettingsId.ToString(), fileName);
                        uri = aset.UploadToAzureBlob(clientid, fileName, uploadedFile, "");//applying for all sites for login in client
                        attachurl = uri;
                        if (!string.IsNullOrEmpty(uri))
                        {
                            formSettings.POTandCURL = uri;
                            if (formSettings.FormSettingsId == 0)
                            {
                                formSettings.Create(userData.DatabaseKey);
                            }
                            else
                            {
                                formSettings.Update(userData.DatabaseKey);
                            }
                        }
                    }
                }
                return formSettings;
            }
            else
            {
                return null;
            }
        }
        public FormSettings SaveWithTandCURLOnPremise(WoFormSettingsSetupModel objAttachment, Stream stream, ref bool attachStatus, ref string attachurl)
        {
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            FormSettings formSettings = new FormSettings();
            formSettings.ClientId = userData.DatabaseKey.Client.ClientId;
            formSettings.RetrieveByClientId(this.userData.DatabaseKey);
            formSettings.POUIC = objAttachment.POUIC;
            formSettings.POLine2 = objAttachment.POLine2;
            formSettings.POLIUIC = objAttachment.POLIUIC;
            formSettings.POComments = objAttachment.POComments;
            formSettings.POTandC = objAttachment.POTandC;
            string Filepath = string.Empty;
            string UserName = string.Empty;
            string UserPassword = string.Empty;
            int ConnectRemoteShareErrorCode = 0;
            var attachmentAllowed = "pdf";

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

                            string fType = objAttachment.FileType;
                            SanitizeFileName(ref fileName, ref fType);
                            objAttachment.FileName = fileName;
                            objAttachment.FileType = fType;
                            long clientid = userData.DatabaseKey.Client.ClientId;
                            long siteid = userData.DatabaseKey.Personnel.SiteId;
                            string uri = string.Empty;
                            string DBFilePath = string.Empty;
                            if (stream.Length > 1)
                            {
                                Filepath = commonWrapper.UploadFileOnPremise(Filepath, objAttachment.FormSettingsId, "FormSettings", out DBFilePath);
                                uri = Path.Combine(Filepath, fileName + "." + fType);
                                DBFilePath = Path.Combine(DBFilePath, fileName + "." + fType);
                                if (!string.IsNullOrEmpty(uri))
                                {

                                    CopyStream(stream, uri);
                                    formSettings.POTandCURL = DBFilePath;
                                    attachurl = DBFilePath;
                                    if (formSettings.FormSettingsId == 0)
                                    {
                                        formSettings.Create(userData.DatabaseKey);
                                    }
                                    else
                                    {
                                        formSettings.Update(userData.DatabaseKey);
                                    }
                                }
                            }
                        }
                        return formSettings;
                    }
                    else
                    {
                        return null;
                    }

                }
            }
            if (ConnectRemoteShareErrorCode > 0)
            {
                formSettings.ErrorMessages = new List<string>();
                formSettings.ErrorMessages.Add(UtilityFunction.GetMessageFromResource("globalNotAuthorisedUploadFile", LocalizeResourceSetConstants.Global));
            }
            return formSettings;
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
        public void CopyStream(Stream stream, string destPath)
        {
            using (var fileStream = new FileStream(destPath, FileMode.Create, FileAccess.Write))
            {
                stream.CopyTo(fileStream);
            }
        }
        public bool DeleteAttachment(string AttachmentURL)
        {
            try
            {
                string image_url = AttachmentURL;
                AzureUtil.AzureSetup aset = new AzureUtil.AzureSetup();
                aset.DeleteBlobByURL(image_url);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool DeleteAttachmentOnPremise(string AttachmentURL, ref string rtrMsg)
        {
            int ConnectRemoteShareErrorCode = 0;
            bool result = false;
            try
            {
                string image_url = AttachmentURL;

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
        #endregion

        #region V2-947
        public List<string> UpdatePORFormSettings(WoFormSettingsSetupModel objM, ref string ErrorMsg)
        {
            DataContracts.FormSettings formSettings = new DataContracts.FormSettings();
            formSettings.ClientId = userData.DatabaseKey.Client.ClientId;
            formSettings.RetrieveByClientId(this.userData.DatabaseKey);
            formSettings.PORHeader = objM.PORHeader;
            formSettings.PORUIC = objM.PORUIC;   //V2-1011
            formSettings.PORLine2 = objM.PORLine2;
            formSettings.PORComments = objM.PORComments;  //V2-1011
            formSettings.PORPrint = objM.PORPrint;
            if (formSettings.FormSettingsId == 0)
            {
                formSettings.Create(userData.DatabaseKey);
            }
            else
            {
                formSettings.Update(userData.DatabaseKey);
            }

            return formSettings.ErrorMessages;

        }
        #endregion
    }
}