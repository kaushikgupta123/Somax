using Client.BusinessWrapper.Configuration.ClientSetUp;
using Client.Controllers.Common;
using Client.Models.Configuration.ClientSetUp;
using Common.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using AzureUtil;
using System.Data;
using Client.Common;
using System.Drawing;
using System.Drawing.Drawing2D;
using Client.BusinessWrapper.Common;
using System.Text;
using DataContracts;

namespace Client.Controllers.Configuration.ClientSetUp
{
    public class ClientSetUpController : ConfigBaseController
    {
        public ActionResult Index()
        {
            ClientSetUpWrapper cWrapper = new ClientSetUpWrapper(userData);
            ClientSetUpVM objClientSetUpVM = new ClientSetUpVM();
            var client = cWrapper.ClientDetails();
            PasswordSettingsModel passwordSettingsModel = new PasswordSettingsModel();
            WoCompletionSettingsModel woCompletionSettingsModel = new WoCompletionSettingsModel();
            WoCompletionCriteriaSetupModel woCompletionCriteriaSetupModel = new WoCompletionCriteriaSetupModel();//v2-728
            CommonWrapper comWrapper = new CommonWrapper(userData);
            ClientSetUpModel ImgModel = new ClientSetUpModel();
            objClientSetUpVM.security = this.userData.Security;
            string ImageURL = string.Empty;
            bool ClientOnPremise = userData.DatabaseKey.Client.OnPremise;
            if (ClientOnPremise)
            {
                ImageURL = comWrapper.GetOnPremiseImageUrl(userData.DatabaseKey.Client.ClientId, AttachmentTableConstant.Client);
            }
            else
            {
                ImageURL = comWrapper.GetAzureImageUrl(userData.DatabaseKey.Client.ClientId, AttachmentTableConstant.Client);
            }
            CommonWrapper objCommonWrapper = new CommonWrapper(userData);
            var PMWOGenerationMethod = objCommonWrapper.GetListFromConstVals(LookupListConstants.PMWOGenerationMethod);
            objClientSetUpVM.PMWOGenerationMethodList = PMWOGenerationMethod.Select(x => new SelectListItem { Text = x.text, Value = x.value }).ToList();
            #region V2-992
            var MasterSanGenerateMethod = objCommonWrapper.GetListFromConstVals(LookupListConstants.MasterSanGenerateMethod);
            objClientSetUpVM.MasterSanGenerateMethodList = MasterSanGenerateMethod.Select(x => new SelectListItem { Text = x.text, Value = x.value }).ToList();

            #endregion

            ClientSetUpModel clientModel = new ClientSetUpModel()
            {
                CompanyName = client.CompanyName,
                PrimaryContact = client.PrimaryContact,
                OfficerPhone = client.OfficerPhone,
                Email = client.Email,
                WOPrintMessage = client.WOPrintMessage,
                AssetTree = client.AssetTree,
                ImageUrl = ImageURL,
                IsImageDelete = ImgModel.IsImageDelete,
                PMWOGenerateMethod = client.PMWOGenerateMethod,
                UpdateIndex = client.UpdateIndex,
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                _userdata = this.userData,
                ClientOnPremise = userData.DatabaseKey.Client.OnPremise,
                MasterSanGenerateMethod = client.MasterSanGenerateMethod,//V2-922
            };
            passwordSettingsModel = cWrapper.PasswordSettingsDetails();
            woCompletionSettingsModel = cWrapper.CompletionSettingsDetails();
            objClientSetUpVM.clientSetUpModel = clientModel;
            objClientSetUpVM.passwordSettingsModel = passwordSettingsModel;
            objClientSetUpVM.woCompletionSettingsModel = woCompletionSettingsModel;
            //v2-728
            woCompletionCriteriaSetupModel.WOCompCriteriaTab = woCompletionSettingsModel.WOCompCriteriaTab;
            woCompletionCriteriaSetupModel.WOCompCriteriaTitle = woCompletionSettingsModel.WOCompCriteriaTitle;
            woCompletionCriteriaSetupModel.WOCompCriteria = woCompletionSettingsModel.WOCompCriteria;
            objClientSetUpVM.woCompletionCriteriaSetupModel = woCompletionCriteriaSetupModel;
            //v2-728
            //V2-944
            WoFormSettingsSetupModel woFormSettingsSetupModel = new WoFormSettingsSetupModel();//V2-944
            woFormSettingsSetupModel=cWrapper.FormSettingsDetails();
            objClientSetUpVM.woFormSettingsSetupModel= woFormSettingsSetupModel;
            //V2-944
            LocalizeControls(objClientSetUpVM, LocalizeResourceSetConstants.SetUpDetails);
            return View("~/Views/Configuration/ClientSetUp/Index.cshtml", objClientSetUpVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateClientSetup(ClientSetUpVM objM)
        {
            if (ModelState.IsValid)
            {
                ClientSetUpWrapper cWrapper = new ClientSetUpWrapper(userData);
                string Mode = string.Empty;
                long UpdateIndex = 0;
                List<String> errorList = cWrapper.UpdateClientSetup(objM.clientSetUpModel, ref Mode, ref UpdateIndex);
                if (errorList != null && errorList.Count > 0)
                {
                    return Json(errorList, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), UpdateIndex = UpdateIndex }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                string ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdatePasswordSettings(ClientSetUpVM objM)
        {
            if (ModelState.IsValid)
            {
                ClientSetUpWrapper cWrapper = new ClientSetUpWrapper(userData);
                string Mode = string.Empty;
                //   long UpdateIndex = 0;
                List<String> errorList = cWrapper.UpdatePasswordSettings(objM.passwordSettingsModel, ref Mode);
                if (errorList != null && errorList.Count > 0)
                {
                    return Json(errorList, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                string ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }


        #region Upload/Delete Photo        
        [HttpPost]
        public ActionResult UploadNewImageToAzure(string fileName)
        {
            var originalDirectory = new System.IO.DirectoryInfo(string.Format("{0}UploadedImages\\images\\{1}", Server.MapPath(@"\write\"), userData.SessionId.ToString()));
            string route = originalDirectory.FullName.ToString();
            byte[] bytes = System.IO.File.ReadAllBytes(route + "/" + fileName);
            MemoryStream ms = new MemoryStream(bytes);
            System.Drawing.Image original = System.Drawing.Image.FromStream(ms);
            System.Drawing.Image Thumb = CreateThumbnail(Image.FromStream(ms), new Size(100, 100));
            MemoryStream ms1 = new MemoryStream();
            Thumb.Save(ms1, System.Drawing.Imaging.ImageFormat.Jpeg);
            ClientSetUpWrapper cWrapper = new ClientSetUpWrapper(userData);
            string Mode = string.Empty;
            Int64 id = cWrapper.UploadPhoto(bytes, ref Mode);
            return Json(new { imageurl = Convert.ToBase64String(ms.ToArray()) });
        }
        [HttpPost]
        public ActionResult DeleteImageFromAzure()
        {
            CommonWrapper comWrapper = new CommonWrapper(userData);
            string isSuccess = string.Empty;
            comWrapper.DeleteAzureImage(userData.DatabaseKey.Client.ClientId, AttachmentTableConstant.Client, ref isSuccess);
            return Json(isSuccess.ToLower(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult DeleteImageFromOnPremise()
        {
            CommonWrapper comWrapper = new CommonWrapper(userData);
            string isSuccess = string.Empty;
            comWrapper.DeleteOnPremiseImage(userData.DatabaseKey.Client.ClientId, AttachmentTableConstant.Client, ref isSuccess);
            return Json(isSuccess.ToLower(), JsonRequestBehavior.AllowGet);
        }
        public Image CreateThumbnail(Image image, Size thumbnailSize)
        {
            float scalingRatio = CalculateScalingRatio(image.Size, thumbnailSize);
            int scaledWidth = (int)Math.Round((float)image.Size.Width / scalingRatio);
            int scaledHeight = (int)Math.Round((float)image.Size.Height / scalingRatio);
            int scaledLeft = 0;
            int scaledTop = 0;
            Rectangle cropArea = new Rectangle(scaledLeft, scaledTop, scaledWidth, scaledHeight);
            System.Drawing.Image thumbnail = new Bitmap(scaledWidth, scaledHeight);
            using (Graphics thumbnailGraphics = Graphics.FromImage(thumbnail))
            {
                thumbnailGraphics.CompositingQuality = CompositingQuality.HighQuality;
                thumbnailGraphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                thumbnailGraphics.SmoothingMode = SmoothingMode.HighQuality;
                thumbnailGraphics.DrawImage(image, cropArea, new Rectangle(0, 0, image.Width, image.Height), GraphicsUnit.Pixel);
            }
            return thumbnail;
        }
        private float CalculateScalingRatio(Size originalSize, Size targetSize)
        {
            float scalingRatio = 1;
            if (originalSize.Width > targetSize.Width && originalSize.Width >= originalSize.Height)
            {
                scalingRatio = (float)originalSize.Width / (float)targetSize.Width;
            }
            else if (originalSize.Width < targetSize.Width) //resize to large
            {
                scalingRatio = (float)targetSize.Width / (float)originalSize.Width;
            }
            else if (originalSize.Height > targetSize.Height && originalSize.Height >= originalSize.Width)
            {
                scalingRatio = (float)originalSize.Height / (float)targetSize.Height;
            }
            else if (originalSize.Height < targetSize.Height)//resize to large
            {
                scalingRatio = (float)targetSize.Height / (float)originalSize.Height;
            }
            return scalingRatio;
        }
        #endregion

        #region V2-634 Update WoCompletionSettings
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateWOCompletionSettings(ClientSetUpVM objM)
        {

            if (ModelState.IsValid)
            {
                ClientSetUpWrapper cWrapper = new ClientSetUpWrapper(userData);
                string Mode = string.Empty;
                List<String> errorList;
                errorList = cWrapper.UpdateCompletionSettings(objM.woCompletionSettingsModel, ref Mode);

                if (errorList != null && errorList.Count > 0)
                {
                    return Json(errorList, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                string ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region V2-728 Completion Criteria
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateWOCompletionCriteria(ClientSetUpVM objM)
        {

            if (ModelState.IsValid)
            {
                ClientSetUpWrapper cWrapper = new ClientSetUpWrapper(userData);
                string Mode = string.Empty;
                List<String> errorList;
                errorList = cWrapper.UpdateWOCompletionCriteria(objM.woCompletionCriteriaSetupModel, ref Mode);

                if (errorList != null && errorList.Count > 0)
                {
                    return Json(errorList, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                string ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region V2-944 Form Settings
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateWOFormConfiguration(ClientSetUpVM objM)
        {

            if (ModelState.IsValid)
            {
                ClientSetUpWrapper cWrapper = new ClientSetUpWrapper(userData);
                string Mode = string.Empty;
                List<String> errorList;
                errorList = cWrapper.UpdateFormSettings(objM.woFormSettingsSetupModel, ref Mode);

                if (errorList != null && errorList.Count > 0)
                {
                    return Json(errorList, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                string ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion
        #region V2-945 PRForm Settings
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdatePRFormConfiguration(ClientSetUpVM objM)
        {

            if (ModelState.IsValid)
            {
                ClientSetUpWrapper cWrapper = new ClientSetUpWrapper(userData);
                string Mode = string.Empty;
                List<String> errorList;
                errorList = cWrapper.UpdatePRFormSettings(objM.woFormSettingsSetupModel, ref Mode);

                if (errorList != null && errorList.Count > 0)
                {
                    return Json(errorList, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                string ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion
        #region V2-946 POForm Settings
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdatePOFormConfiguration()
        {
            if (ModelState.IsValid)
            {
                Stream stream = Request.Files[0].InputStream;
                ClientSetUpWrapper cWrapper = new ClientSetUpWrapper(userData);
                string Mode = string.Empty;
                string attachurl = "";
                List<String> errorList;
                WoFormSettingsSetupModel woFormSettingsSetupModel = new WoFormSettingsSetupModel();
                woFormSettingsSetupModel.FormSettingsId = Convert.ToInt64(Request.Form["woFormSettingsSetupModel.FormSettingsId"]);
                woFormSettingsSetupModel.POUIC = Convert.ToBoolean(Request.Form["woFormSettingsSetupModel.POUIC"].Split(',').FirstOrDefault());
                woFormSettingsSetupModel.POLine2 = Convert.ToBoolean(Request.Form["woFormSettingsSetupModel.POLine2"].Split(',').FirstOrDefault());
                woFormSettingsSetupModel.POLIUIC = Convert.ToBoolean(Request.Form["woFormSettingsSetupModel.POLIUIC"].Split(',').FirstOrDefault());
                woFormSettingsSetupModel.POComments = Convert.ToBoolean(Request.Form["woFormSettingsSetupModel.POComments"].Split(',').FirstOrDefault());
                woFormSettingsSetupModel.POTandC = Convert.ToBoolean(Request.Form["woFormSettingsSetupModel.POTandC"].Split(',').FirstOrDefault());
                if (Request.Files[0].FileName == "" || !woFormSettingsSetupModel.POTandC)
                {
                    errorList = cWrapper.UpdatePOFormSettings(woFormSettingsSetupModel, ref Mode, ref attachurl);
                    if (errorList != null && errorList.Count > 0)
                    {
                        return Json(errorList, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { Result = JsonReturnEnum.success.ToString(),AttachURL= attachurl }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    woFormSettingsSetupModel.FileName = System.IO.Path.GetFileNameWithoutExtension(Request.Files[0].FileName);
                    string fileExt = System.IO.Path.GetExtension(Request.Files[0].FileName);
                    woFormSettingsSetupModel.FileType = fileExt.Substring(1);
                    bool attachStatus = false;
                    bool OnPremise = userData.DatabaseKey.Client.OnPremise;
                    FormSettings formSettings = new FormSettings();
                    if (OnPremise)
                    {
                        formSettings = cWrapper.SaveWithTandCURLOnPremise(woFormSettingsSetupModel, stream, ref attachStatus, ref attachurl);
                    }
                    else
                    {
                        formSettings = cWrapper.SaveWithTandCURL(woFormSettingsSetupModel, stream, ref attachStatus,ref attachurl);
                    }
                    if (attachStatus)
                    {
                        if (formSettings.ErrorMessages != null && formSettings.ErrorMessages.Count > 0)
                        {
                            return Json(formSettings.ErrorMessages, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            return Json(new { Result = JsonReturnEnum.success.ToString(), AttachURL = attachurl }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        var fileTypeMessage = UtilityFunction.GetMessageFromResource("spnInvalidFileType", LocalizeResourceSetConstants.Global);
                        return Json(fileTypeMessage, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            else
            {
                string ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region V2-947 PORForm Settings
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdatePORFormConfiguration(ClientSetUpVM objM)
        {

            if (ModelState.IsValid)
            {
                ClientSetUpWrapper cWrapper = new ClientSetUpWrapper(userData);
                string Mode = string.Empty;
                List<String> errorList;
                errorList = cWrapper.UpdatePORFormSettings(objM.woFormSettingsSetupModel, ref Mode);

                if (errorList != null && errorList.Count > 0)
                {
                    return Json(errorList, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                string ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion
    }
}
