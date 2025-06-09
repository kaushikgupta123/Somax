using Client.ActionFilters;
using Client.BusinessWrapper.ActiveSiteWrapper;
using Client.BusinessWrapper.Common;
using Client.BusinessWrapper.Configuration.AccountConfig;
using Client.Common;
using Client.Controllers.Common;
using Client.Models.Configuration.Account;
using Common.Constants;
using DataContracts;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Client.Controllers.Configuration.AccountConfig
{
    public class AccountController : ConfigBaseController
    {
        #region Search
        [CheckUserSecurity(securityType = SecurityConstants.Accounts)]
        public ActionResult Index()
        {
            AccountConfigVM objVM = new AccountConfigVM();
            ActiveSiteWrapper objActiveSiteWrapper = new ActiveSiteWrapper(userData);
            CommonWrapper commonWrapper = new CommonWrapper(userData);

            objVM.security = this.userData.Security;
            objVM._userdata = this.userData;

            if (this.userData.DatabaseKey.Client.PackageLevel.ToUpper() == PackageLevelConstant.Enterprise.ToUpper() && this.userData.DatabaseKey.User.IsSuperUser == true)
            {
                var siteList = objActiveSiteWrapper.GetSites();
                objVM.SiteList = siteList;
            }
            var StatusList = UtilityFunction.InactiveStatusTypesWithBoolValue();
            objVM.accountDetails = new AccountModel();
            if (StatusList != null)
            {
                objVM.accountDetails.AccountTypeList = StatusList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
            }
            var ExternalList = UtilityFunction.IsExternalTypesWithBoolValue();
            if (ExternalList != null)
            {
                objVM.accountDetails.ExternalTypeList = ExternalList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
            }

            LocalizeControls(objVM, LocalizeResourceSetConstants.AccountDetails);
            return View("~/Views/Configuration/Account/Index.cshtml", objVM);
        }
        [HttpPost]
        public string GetAccountGridData(int? draw, int? start, int? length, string ClientLookupId, string Name, long Siteid, string InactiveFlag = "", string IsExternal = "")
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            var colname = Request.Form.GetValues("columns[" + order + "][name]");
            AccountConfigVM objacc = new AccountConfigVM();
            bool allowsitename = false;

            if (this.userData.DatabaseKey.Client.PackageLevel.ToUpper() == PackageLevelConstant.Enterprise.ToUpper() && this.userData.DatabaseKey.User.IsSuperUser == true)
            { allowsitename = true; }

            AccountWrapper accWrapper = new AccountWrapper(userData);

            var accountList = accWrapper.PopulateAccountList();

            //objacc._userdata = accountList.;
            accountList = this.GetAllAccountSortByColumnWithOrder(colname[0], orderDir, accountList);

            accountList = GetAccountSearchResult(accountList, InactiveFlag, ClientLookupId, Siteid, Name, IsExternal);
            var totalRecords = 0;
            var recordsFiltered = 0;
            start = start.HasValue
                ? start / length
                : 0;
            recordsFiltered = accountList.Count();
            totalRecords = accountList.Count();
            int initialPage = start.Value;

            var filteredResult = accountList
                .Skip(initialPage * length ?? 0)
                .Take(length ?? 0)
                .ToList();

            #region uiconfig
            CommonWrapper cWrapper = new CommonWrapper(userData);
            //var hiddenList = cWrapper.UiConfigAllColumnsCustom(UiConfigConstants.AccountSearch, UiConfigConstants.IsHideTrue, UiConfigConstants.IsRequiredNone, UiConfigConstants.IsExternalNone, UiConfigConstants.TargetView).Select(x => x.ColumnName).ToList();
            var hiddenList = cWrapper.GetHiddenList(UiConfigConstants.AccountSearch).Select(x => x.ColumnName).ToList();
            #endregion

            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult, hiddenColumnList = hiddenList, AllowSiteNameColumn = allowsitename }, JsonSerializerDateSettings);

        }
        private List<AccountModel> GetAllAccountSortByColumnWithOrder(string order, string orderDir, List<AccountModel> data)
        {
            List<AccountModel> lst = new List<AccountModel>();
            switch (order)
            {
                case "0":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ClientLookupId).ToList() : data.OrderBy(p => p.ClientLookupId).ToList();
                    break;
                case "1":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Name).ToList() : data.OrderBy(p => p.Name).ToList();
                    break;
                case "2":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.SiteName).ToList() : data.OrderBy(p => p.SiteName).ToList();
                    break;
                case "3":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.InactiveFlag).ToList() : data.OrderBy(p => p.InactiveFlag).ToList();
                    break;
                case "4":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.IsExternal).ToList() : data.OrderBy(p => p.IsExternal).ToList();
                    break;
                default:
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ClientLookupId).ToList() : data.OrderBy(p => p.ClientLookupId).ToList();
                    break;
            }
            return lst;
        }
        private List<AccountModel> GetAccountSearchResult(List<AccountModel> retAccList, string InactiveFlag, string ClientLookupId = "", long Siteid = 0, string Name = "", string IsExternal = "")
        {
            if (retAccList != null)
            {
                if (!string.IsNullOrEmpty(ClientLookupId))
                {
                    ClientLookupId = ClientLookupId.ToUpper();
                    retAccList = retAccList.Where(x => (!string.IsNullOrWhiteSpace(x.ClientLookupId) && x.ClientLookupId.ToUpper().Contains(ClientLookupId))).ToList();
                }
                if (!string.IsNullOrEmpty(Name))
                {
                    Name = Name.ToUpper();
                    retAccList = retAccList.Where(x => (!string.IsNullOrWhiteSpace(x.Name) && x.Name.ToUpper().Contains(Name))).ToList();
                }
                if (Siteid > 0)
                {
                    var SiteId = Siteid.ToString();
                    retAccList = retAccList.Where(x => x.SiteId == Siteid).ToList();
                }

                if (!string.IsNullOrEmpty(InactiveFlag))
                {
                    bool flagVal = false;
                    if (InactiveFlag.Equals("true"))
                    {
                        flagVal = true;
                    }
                    else if (InactiveFlag.Equals("false"))
                    {
                        flagVal = false;
                    }
                    retAccList = retAccList.Where(x => x.InactiveFlag == flagVal).ToList();
                }
                if (!string.IsNullOrEmpty(IsExternal))
                {
                    bool flagVal = false;
                    if (IsExternal.Equals("true"))
                    {
                        flagVal = true;
                    }
                    else if (IsExternal.Equals("false"))
                    {
                        flagVal = false;
                    }
                    retAccList = retAccList.Where(x => x.IsExternal == flagVal).ToList();
                }
            }
            return retAccList;
        }
        public string GetAccountPrintData(string colname, string coldir, string ClientLookupId, long Siteid, string InactiveFlag, string Name, string IsExternal = "")
        {
            List<AccountPrintModel> AccountPrintModelList = new List<AccountPrintModel>();
            AccountPrintModel objAccountPrintModel;
            AccountWrapper accWrapper = new AccountWrapper(userData);
            var accountList = accWrapper.PopulateAccountList();

            if (accountList != null && accountList.Count > 0)
            {
                accountList = this.GetAllAccountSortByColumnWithOrder(colname, coldir, accountList);
                accountList = GetAccountSearchResult(accountList, InactiveFlag, ClientLookupId, Siteid, Name, IsExternal);
            }
            foreach (var acc in accountList)
            {
                objAccountPrintModel = new AccountPrintModel();
                objAccountPrintModel.AccountNumber = acc.ClientLookupId;
                objAccountPrintModel.Name = acc.Name;
                objAccountPrintModel.SiteName = acc.SiteName;
                objAccountPrintModel.InactiveFlag = acc.InactiveFlag;
                objAccountPrintModel.IsExternal = acc.IsExternal;
                AccountPrintModelList.Add(objAccountPrintModel);
            }
            return JsonConvert.SerializeObject(new { data = AccountPrintModelList }, JsonSerializerDateSettings);
        }

        //#region GetActiveSiteList
        //public JsonResult GetActiveSites()
        //{
        //    List<ChangeActiveSiteModel> ChangeActiveSiteModelList = new List<ChangeActiveSiteModel>();
        //    ChangeActiveSiteModel objChangeActiveSiteModel = new ChangeActiveSiteModel();
        //    ActiveSiteWrapper objActiveSiteWrapper = new ActiveSiteWrapper(userData);

        //    objChangeActiveSiteModel.ChangeSiteSiteId = userData.DatabaseKey.User.DefaultSiteId;
        //    var siteList = objActiveSiteWrapper.GetSites();
        //    objChangeActiveSiteModel.SiteList = siteList;
        //    return Json(objChangeActiveSiteModel, JsonRequestBehavior.AllowGet);
        //}
        //#endregion
        #endregion

        #region Details Account
        public PartialViewResult AccountDetail(long AccountId = 0)
        {
            AccountConfigVM objVM = new AccountConfigVM();
            AccountModel obj = new AccountModel();
            AccountWrapper accWrapper = new AccountWrapper(userData);
            obj = accWrapper.AccountDetails(AccountId);
            ChangeAccountId ChngAcc = new ChangeAccountId()
            {
                AccountId = obj.AccountId,
                UpdateIndex = obj.UpdateIndex,
                ClientLookupId = obj.ClientLookupId
            };
            objVM._userdata = this.userData;
            objVM.accountDetails = obj;
            objVM.changeAccountIdModel = ChngAcc;
            objVM.security = this.userData.Security;
            LocalizeControls(objVM, LocalizeResourceSetConstants.AccountDetails);

            #region uiconfig
            CommonWrapper cWrapper = new CommonWrapper(userData);
            string isExternal = "";
            if (obj.IsExternal)
            {
                isExternal = UiConfigConstants.IsExternalTrue;
            }
            else
            {
                isExternal = UiConfigConstants.IsExternalFalse;
            }
            //objVM.hiddenColumnList = cWrapper.UiConfigAllColumnsCustom(UiConfigConstants.AccountDetail, UiConfigConstants.IsHideTrue, UiConfigConstants.IsRequiredNone, isExternal, UiConfigConstants.TargetView).Select(x => x.ColumnName).ToList();
            objVM.hiddenColumnList = cWrapper.GetHiddenList(UiConfigConstants.AccountDetail, isExternal).Select(x => x.ColumnName).ToList();

            #endregion

            return PartialView("~/Views/Configuration/Account/_AccountDetail.cshtml", objVM);
        }



        #endregion
        #region Notes
        public string PopulateNotes(int? draw, int? start, int? length, long accountId)
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            AccountWrapper accWrapper = new AccountWrapper(userData);
            var NotesList = accWrapper.PopulateNoteList(accountId);
            NotesList = this.GetAllNotesSortByColumnWithOrder(order, orderDir, NotesList);
            var totalRecords = 0;
            var recordsFiltered = 0;
            start = start.HasValue
                ? start / length
                : 0;
            recordsFiltered = NotesList.Count();
            totalRecords = NotesList.Count();
            int initialPage = start.Value;
            var filteredResult = NotesList
                .Skip(initialPage * length ?? 0)
                .Take(length ?? 0)
                .ToList();

            bool showAddBtn = userData.Security.OnDemandLibrary.Create;
            bool showEditBtn = userData.Security.OnDemandLibrary.Edit;
            bool showDeleteBtn = userData.Security.OnDemandLibrary.Delete;
            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult }, JsonSerializer12HoursDateAndTimeSettings);
        }
        private List<NoteModel> GetAllNotesSortByColumnWithOrder(string order, string orderDir, List<NoteModel> data)
        {
            List<NoteModel> lst = new List<NoteModel>();
            switch (order)
            {
                case "0":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Subject).ToList() : data.OrderBy(p => p.Subject).ToList();
                    break;
                case "1":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.OwnerName).ToList() : data.OrderBy(p => p.OwnerName).ToList();
                    break;
                case "2":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ModifiedDate).ToList() : data.OrderBy(p => p.ModifiedDate).ToList();
                    break;
            }
            return lst;
        }
        public ActionResult AddOrEditNotes(long? NoteId, long AccountId, string ClientLookupId)
        {
            AccountWrapper accWrapper = new AccountWrapper(userData);
            AccountConfigVM objVM = new AccountConfigVM();
            NoteModel model = new NoteModel();
            if (NoteId != null)
            {
                model = accWrapper.RetriveNoteById(NoteId, AccountId);
            }
            else
            {
                model.AccountID = AccountId;
                model.NotesId = 0;
            }
            model.ClientLookupId = ClientLookupId;
            objVM.noteModel = model;
            LocalizeControls(objVM, LocalizeResourceSetConstants.AccountDetails);
            return PartialView("~/Views/Configuration/Account/_NotesAddEdit.cshtml", objVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult AddOrEditNotes(AccountConfigVM obj)
        {
            if (ModelState.IsValid)
            {
                AccountWrapper accWrapper = new AccountWrapper(userData);
                string Mode = string.Empty;
                List<String> errorList = new List<string>();
                if (obj.noteModel.NotesId > 0)
                {
                    Mode = "update";
                    errorList = accWrapper.UpdateNote(obj.noteModel);
                }
                else
                {
                    Mode = "add";
                    errorList = accWrapper.AddNote(obj.noteModel);
                }
                if (errorList != null && errorList.Count > 0)
                {
                    return Json(errorList, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), mode = Mode, accountID = obj.noteModel.AccountID }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                string ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult DeleteNote(long noteId)
        {
            AccountWrapper accWrapper = new AccountWrapper(userData);
            var deleteResult = accWrapper.DeleteNote(noteId);
            if (deleteResult)
            {
                return Json(new { Result = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Result = JsonReturnEnum.failed.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        #region Attachments
        public string PopulateAttachment(int? draw, int? start, int? length, long accountId)
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            CommonWrapper objCommonWrapper = new CommonWrapper(userData);
            var AttachmentList = objCommonWrapper.PopulateAttachments(accountId, "Account", userData.Security.Accounts.Edit);
            if (AttachmentList != null)
            {
                AttachmentList = GetAllAttachmentsSortByColumnWithOrder(order, orderDir, AttachmentList);
            }
            var totalRecords = 0;
            var recordsFiltered = 0;
            start = start.HasValue
                ? start / length
                : 0;
            recordsFiltered = AttachmentList.Count();
            totalRecords = AttachmentList.Count();
            int initialPage = start.Value;
            var filteredResult = AttachmentList
                .Skip(initialPage * length ?? 0)
                .Take(length ?? 0)
                .ToList();
            bool showAddBtn = userData.Security.OnDemandLibrary.Create;
            bool showEditBtn = userData.Security.OnDemandLibrary.Edit;
            bool showDeleteBtn = userData.Security.OnDemandLibrary.Delete;
            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult });
        }
        private List<AttachmentModel> GetAllAttachmentSortByColumnWithOrder(string order, string orderDir, List<AttachmentModel> data)
        {
            List<AttachmentModel> lst = new List<AttachmentModel>();
            switch (order)
            {
                case "0":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Subject).ToList() : data.OrderBy(p => p.Subject).ToList();
                    break;
                case "1":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.FullName).ToList() : data.OrderBy(p => p.FullName).ToList();
                    break;
                case "2":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.FileSizeWithUnit).ToList() : data.OrderBy(p => p.FileSizeWithUnit).ToList();
                    break;
                case "3":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.OwnerName).ToList() : data.OrderBy(p => p.OwnerName).ToList();
                    break;
                case "4":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.CreateDate).ToList() : data.OrderBy(p => p.CreateDate).ToList();
                    break;
            }
            return lst;
        }
        [HttpGet]
        public PartialViewResult AddAttachments(long AccountId, string ClientLookupId)
        {
            AccountWrapper accWrapper = new AccountWrapper(userData);
            AccountConfigVM objVM = new AccountConfigVM();
            AttachmentModel model = new AttachmentModel();
            model.AccountID = AccountId;
            model.ClientLookupId = ClientLookupId;
            objVM.attachmentModel = model;
            LocalizeControls(objVM, LocalizeResourceSetConstants.AccountDetails);
            return PartialView("~/Views/Configuration/Account/_AttachmentAdd.cshtml", objVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveAttachments()
        {
            string ModelValidationFailedMessage = string.Empty;
            if (ModelState.IsValid)
            {
                Stream stream = Request.Files[0].InputStream;
                Client.Models.AttachmentModel attachmentModel = new Client.Models.AttachmentModel();
                CommonWrapper objCommonWrapper = new CommonWrapper(userData);
                Attachment fileAtt = new Attachment();
                attachmentModel.FileName = System.IO.Path.GetFileNameWithoutExtension(Request.Files[0].FileName);
                string fileExt = System.IO.Path.GetExtension(Request.Files[0].FileName);
                attachmentModel.FileType = fileExt.Substring(1);

                attachmentModel.FileSize = Request.Files[0].ContentLength;
                attachmentModel.ContentType = Request.Files[0].ContentType;
                attachmentModel.ObjectId = Convert.ToInt64(Request.Form["attachmentModel.AccountID"]);
                attachmentModel.Subject = String.IsNullOrEmpty(Request.Form["attachmentModel.Subject"]) ? "No Subject" : Request.Form["attachmentModel.Subject"];
                attachmentModel.TableName = "Account";
                bool attachStatus = false;
                bool OnPremise = userData.DatabaseKey.Client.OnPremise;
                if (OnPremise)
                {
                    fileAtt = objCommonWrapper.AddAttachmentOnPremise(attachmentModel, stream, ref attachStatus, userData.Security.Accounts.Edit);
                }
                else
                {
                    fileAtt = objCommonWrapper.AddAttachment(attachmentModel, stream, ref attachStatus, userData.Security.Accounts.Edit);
                }
                if (attachStatus)
                {
                    if (fileAtt.ErrorMessages != null && fileAtt.ErrorMessages.Count > 0)
                    {
                        return Json(fileAtt.ErrorMessages, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { Result = JsonReturnEnum.success.ToString(), accountID = Convert.ToInt64(Request.Form["attachmentModel.AccountID"]) }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    var fileTypeMessage = UtilityFunction.GetMessageFromResource("spnInvalidFileType", LocalizeResourceSetConstants.Global);
                    return Json(fileTypeMessage, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult DownloadAttachment(long _fileinfoId)
        {
            CommonWrapper objCommonWrapper = new CommonWrapper(userData);
            DataContracts.Attachment fileInfo = new DataContracts.Attachment();

            bool OnPremise = userData.DatabaseKey.Client.OnPremise;
            if (OnPremise)
            {
                fileInfo = objCommonWrapper.DownloadAttachmentOnPremise(_fileinfoId);
                string contentType = MimeMapping.GetMimeMapping(fileInfo.AttachmentURL);
                return File(fileInfo.AttachmentURL, contentType, fileInfo.FileName + '.' + fileInfo.FileType);
            }
            else
            {
                fileInfo = objCommonWrapper.DownloadAttachment(_fileinfoId);
                return Redirect(fileInfo.AttachmentURL);
            }

        }
        [HttpPost]
        public ActionResult DeleteAttachments(long fileAttachmentId)
        {
            CommonWrapper objCommonWrapper = new CommonWrapper(userData);
            bool deleteResult = false;
            string Message = string.Empty;
            bool OnPremise = userData.DatabaseKey.Client.OnPremise;
            if (OnPremise)
            {
                deleteResult = objCommonWrapper.DeleteAttachmentOnPremise(fileAttachmentId, ref Message);
            }
            else
            {
                deleteResult = objCommonWrapper.DeleteAttachment(fileAttachmentId);

            }
            if (deleteResult)
            {
                return Json(new { Result = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Result = JsonReturnEnum.failed.ToString(), Message = Message }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        #region Account
        public ActionResult AddOrEditAccount(long? AccountId, bool IsAddFromDetails)
        {
            AccountWrapper accWrapper = new AccountWrapper(userData);
            AccountConfigVM objVM = new AccountConfigVM();
            AccountModel obj = new AccountModel();
            if (!IsAddFromDetails)
            {
                obj = accWrapper.AccountDetails(AccountId ?? 0);
                obj.ViewName = UiConfigConstants.AccountEdit;//--V2-375//
                obj.IsExternal = obj.IsExternal;//--V2-375//
            }
            else
            {
                obj.ViewName = UiConfigConstants.AccountAdd;//--V2-375//
                obj.IsExternal = obj.IsExternal;//--V2-375//
                obj.AccountId = AccountId ?? 0;
                obj.IsAddFromDetails = true;
                obj.SiteName = userData.Site.Name;
            }

            objVM._userdata = this.userData;
            objVM.accountDetails = obj;
            LocalizeControls(objVM, LocalizeResourceSetConstants.AccountDetails);
            return PartialView("~/Views/Configuration/Account/_AccountAddEdit.cshtml", objVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveAccount(AccountConfigVM objVM, string Command)
        {
            if (ModelState.IsValid)
            {
                List<String> errorList = new List<string>();
                AccountWrapper accWrapper = new AccountWrapper(userData);
                string Mode = string.Empty;
                long AccountID = 0;
                if (!objVM.accountDetails.IsAddFromDetails && !objVM.accountDetails.IsAddFromIndex)
                {
                    Mode = "update";
                    errorList = accWrapper.UpdateAccount(objVM.accountDetails, ref AccountID);
                }
                else
                {
                    Mode = "add";
                    errorList = accWrapper.InsertAccount(objVM.accountDetails, ref AccountID);

                }
                if (errorList != null && errorList.Count > 0)
                {
                    return Json(errorList, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), AccountID = AccountID, mode = Mode, Command = Command }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                string ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult ValidateForActiveInactive(bool InActiveFlag, long AccountId, string ClientLookupId)
        {
            string flag = string.Empty;
            AccountWrapper accWrapper = new AccountWrapper(userData);
            if (InActiveFlag)
            {
                return Json(new { validationStatus = true }, JsonRequestBehavior.AllowGet);
            }
            var account = accWrapper.ValidateActiveInactiveAcount(AccountId, InActiveFlag);
            if (account.ErrorMessages != null && account.ErrorMessages.Count > 0)
            {
                return Json(account.ErrorMessages, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { validationStatus = true }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult MakeActiveInactive(bool InActiveFlag, long AccountId, int UpdateIndex)
        {
            AccountWrapper accWrapper = new AccountWrapper(userData);
            var ErrorMessages = accWrapper.MakeActiveInactive(AccountId, InActiveFlag, UpdateIndex);
            if (ErrorMessages != null && ErrorMessages.Count > 0)
            {
                return Json(ErrorMessages, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var createEventStatus = accWrapper.CreateAccountEvent(AccountId, InActiveFlag);
                if (createEventStatus != null && createEventStatus.Count > 0)
                {
                    return Json(createEventStatus, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
                }

            }
        }
        #endregion

        #region AccountChange Pop-Up
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult ValiDateChangeAccoutPage(AccountConfigVM objVM)
        {
            if (ModelState.IsValid)
            {
                string IsAddOrUpdate = string.Empty;
                AccountWrapper accWrapper = new AccountWrapper(userData);
                long UpdateIndex = 0;
                List<string> errorList = accWrapper.ChangeAccountID(objVM, ref UpdateIndex);
                if (errorList != null && errorList.Count > 0)
                {
                    return Json(errorList, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Issuccess = true, AccountId = objVM.changeAccountIdModel.AccountId, ClientLookUpId = objVM.changeAccountIdModel.ClientLookupId, UpdateIndex = UpdateIndex }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                string ModelValidationFailedMessage = string.Empty;
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
    }
}