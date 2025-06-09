using Admin.ActionFilters;
using Admin.BusinessWrapper;
using Admin.BusinessWrapper.Client;
using Admin.BusinessWrapper.Site;
using Admin.Common;
using Admin.Controllers.Common;
using Admin.Models;
using Admin.Models.UserManagement;

using Common.Constants;

using DataContracts;

using Newtonsoft.Json;

using Rotativa;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Admin.Controllers.UserManagement
{
    public class UserManagementController : SomaxBaseController
    {
        // GET: UserManagement
        #region Search Grid
        public ActionResult Index()
        {
            //ActiveSiteWrapper objActiveSiteWrapper = new ActiveSiteWrapper(userData);
            UserManagementVM objUserManagementVM = new UserManagementVM();
            UserModel objUserModel = new UserModel();
            UsermanagmentWrapper objUserWrapper = new UsermanagmentWrapper(userData);
            var ClientList = objUserWrapper.GetAllActiveClientForAdmin();
            var SiteList = objUserWrapper.GetSiteForAdmin_V2();
            if (ClientList != null)
            {
                objUserManagementVM.ClientList = ClientList.Select(x => new SelectListItem { Text = x.Name, Value = x.ClientId.ToString() });
            }
            if (SiteList != null)
            {
                objUserManagementVM.SiteList = SiteList.Select(x => new SelectListItem { Text = x.ClientName + "(" + x.Name + ")", Value = x.SiteId.ToString() });
            }
            LocalizeControls(objUserManagementVM, LocalizeResourceSetConstants.Configuration);
            return View("~/Views/UserManagement/Index.cshtml", objUserManagementVM);

        }

        [HttpPost]
        public string PopulateuserManagment(int? draw, int? start, int? length, string UserName = "", string LastName = "", string FirstName = "", string Email = "", string order = "1", string orderDir = "asc", int CaseNo = 1, string SearchText = "", long SiteId = 0, long ClientId = 0)
        {

            UsermanagmentWrapper objUserWrapper = new UsermanagmentWrapper(userData);
            List<UserManagementModel> retUserManagmentlist = null;

            retUserManagmentlist = objUserWrapper.PopulateuserManagmentForAdmin(start ?? 0, length ?? 10, CaseNo, UserName, LastName, FirstName, Email, ClientId, SearchText, order, orderDir, SiteId);

            long TotalCount = 0;
            if (retUserManagmentlist != null && retUserManagmentlist.Count > 0)
                TotalCount = retUserManagmentlist[0].TotalCount;
            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = TotalCount, recordsFiltered = TotalCount, data = retUserManagmentlist }, JsonSerializer12HoursDateAndTimeSettings);
        }
        public PartialViewResult UserManagementDetails(long UserId = 0, long ClientId = 0)
        {
            UsermanagmentWrapper _userWraperObj = new UsermanagmentWrapper(userData);
            UserManagementVM userdetVM = new UserManagementVM();
            UserModel _userModel = new UserModel();
            userdetVM.userModels = _userWraperObj.GetdUserdetailsByUserInfoID(UserId, ClientId);
            _userModel.PackageLevel = this.userData.DatabaseKey.Client.PackageLevel;
            UserDetails userDetail = new UserDetails();
            userDetail.ClientId = ClientId;
            userdetVM.UserDetail = userDetail;
            LocalizeControls(userdetVM, LocalizeResourceSetConstants.UserDetails);
            return PartialView("~/Views/UserManagement/UserManagementDetails.cshtml", userdetVM);
        }
        [HttpGet]
        public ActionResult PopulateuserManagmentInnerGridDetails(long UserInfoId, long ClientId)
        {
            UserManagementVM objUserManagementVM = new UserManagementVM();
            UsermanagmentWrapper objUserWrapper = new UsermanagmentWrapper(userData);
            objUserManagementVM.InnerGridDataList = objUserWrapper.PopulateuserManagmentInnerGridDetails(UserInfoId, ClientId);
            LocalizeControls(objUserManagementVM, LocalizeResourceSetConstants.UserDetails);
            return PartialView("~/Views/UserManagement/_InnerGridUserManagement.cshtml", objUserManagementVM);
        }
        #endregion
        #region Print 
        [HttpGet]
        public string UserDetailsPrintData(string UserName = "", string LastName = "", string FirstName = "", string Email = "", long ClientId = 0,
            string order = "1", string orderDir = "asc", int CaseNo = 1, string SearchText = "", long SiteId = 0)
        {

            UsermanagmentWrapper objUserWrapper = new UsermanagmentWrapper(userData);
            List<UserManagementModel> retUserManagmentlist = null;
            List<PrintUserModel> PrintUserModelList = null;
            retUserManagmentlist = objUserWrapper.PopulateuserManagmentForAdmin(0, 100000, CaseNo, UserName, LastName, FirstName, Email, ClientId, SearchText, order, orderDir, SiteId);

            PrintUserModelList = new List<PrintUserModel>();
            PrintUserModel objPrintModel = null;
            foreach (var item in retUserManagmentlist)
            {
                objPrintModel = new PrintUserModel();
                objPrintModel.UserName = item.UserName;
                objPrintModel.LastName = item.LastName;
                objPrintModel.FirstName = item.FirstName;
                objPrintModel.SecurityProfile = item.SecurityProfile;
                objPrintModel.Email = item.Email;
                objPrintModel.Client = item.CompanyName;
                objPrintModel.Active = item.IsActive;
                PrintUserModelList.Add(objPrintModel);
            }


            return JsonConvert.SerializeObject(new { data = PrintUserModelList }, JsonSerializerDateSettings);
        }
        [HttpPost]
        public JsonResult SetPrintData(UMPrintParams objPrintParams)
        {
            Session["PRINTPARAMS"] = objPrintParams;
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        [EncryptedActionParameter]
        public ActionResult ExportASPDF(string d = "")
        {
            UsermanagmentWrapper uWrapper = new UsermanagmentWrapper(userData);
            UMPdfPrintModel objPrintModel;
            UserManagementVM objUserManagementVM = new UserManagementVM();
            List<UMPdfPrintModel> objPrintModelList = new List<UMPdfPrintModel>();
            UMPrintParams objPrintParams = (UMPrintParams)Session["PRINTPARAMS"];
            List<UserManagementModel> objSearchList = null;
            var locker = new object();
            objSearchList = uWrapper.PopulateuserManagmentForAdmin(0, 100000, objPrintParams.CaseNo, objPrintParams.UserName, objPrintParams.LastName, objPrintParams.FirstName, objPrintParams.Email, objPrintParams.ClientId, objPrintParams.SearchText, objPrintParams.order, objPrintParams.orderDir, objPrintParams.SiteId);

            foreach (var data in objSearchList)
            {
                objPrintModel = new UMPdfPrintModel();
                objPrintModel.UserName = data.UserName;
                objPrintModel.LastName = data.LastName;
                objPrintModel.FirstName = data.FirstName;
                objPrintModel.Client = data.CompanyName;
                objPrintModel.SecurityProfile = data.SecurityProfile;
                objPrintModel.Email = data.Email;
                objPrintModel.Active = data.IsActive;
                if (data.SiteCount > 0)
                {
                    objPrintModel.innerGridUserManagement = uWrapper.PopulateuserManagmentInnerGridDetails(data.UserInfoId, data.ClientId);
                }
                lock (locker)
                {
                    objPrintModelList.Add(objPrintModel);
                }
            }
            objUserManagementVM.PrintModelList = objPrintModelList;
            objUserManagementVM.tableHaederProps = objPrintParams.tableHaederProps;
            LocalizeControls(objUserManagementVM, LocalizeResourceSetConstants.UserDetails);
            if (d == "d")
            {
                return new PartialViewAsPdf("~/Views/UserManagement/UMGridPdfPrintTemplate.cshtml", objUserManagementVM)
                {
                    PageSize = Rotativa.Options.Size.A4,
                    FileName = "User Management.pdf",
                    PageMargins = { Left = 3, Right = 3, Top = 3, Bottom = 3 },
                };
            }
            else
            {
                try
                {


                    return new ViewAsPdf("~/Views/UserManagement/UMGridPdfPrintTemplate.cshtml", objUserManagementVM)
                    {
                        PageSize = Rotativa.Options.Size.A4,
                        PageMargins = { Left = 3, Right = 3, Top = 3, Bottom = 3 },
                    };
                }
                catch (Exception ex)
                {
                    // Log the exception details
                    var er = ex.Message;
                    throw;
                }
            }

        }
        #endregion
        #region Populate Activity Grid
        [HttpGet]
        public string PopulateActivityTableGrid(int? draw, int? start, int? length, long ClientId, string order = "1", string orderDir = "asc")
        {

            UsermanagmentWrapper uWrapper = new UsermanagmentWrapper(userData);
            start = start.HasValue
                ? start / length
                : 0;
            int skip = start * length ?? 0;
            var orderCol = Convert.ToInt16(order);
            order = orderCol > 0 ? (orderCol + 1).ToString() : order;
            List <LoginAuditingInfo> LoginAuditingInfoList = uWrapper.PopulateActivity(ClientId, order, orderDir, skip, length ?? 0);
            var totalRecords = 0;
            var recordsFiltered = 0;
            recordsFiltered = LoginAuditingInfoList.Select(o => o.TotalCount).FirstOrDefault();
            totalRecords = LoginAuditingInfoList.Select(o => o.TotalCount).FirstOrDefault();
            var filteredResult = LoginAuditingInfoList
                .ToList();
            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult }, JsonSerializerDateSettings);
        }
        #endregion
        #region User Management - Add/Remove Sites
        [HttpPost]
        public string PopulatePersonnelSites(int? draw, int? start, int? length, long userInfoId, long ClientId)
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            UsermanagmentWrapper uWrapper = new UsermanagmentWrapper(userData);
            var PersonnelSiteList = uWrapper.PopulatePersonnelSites(userInfoId, ClientId);
            PersonnelSiteList = this.GetAllUserSiteByUserInfoIdSortByColumnWithOrder(order, orderDir, PersonnelSiteList);
            var totalRecords = 0;
            var recordsFiltered = 0;
            start = start.HasValue
                ? start / length
                : 0;
            recordsFiltered = PersonnelSiteList.Count();
            totalRecords = PersonnelSiteList.Count();
            int initialPage = start.Value;
            var filteredResult = PersonnelSiteList
               .Skip(initialPage * length ?? 0)
               .Take(length ?? 0)
               .ToList();

            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult }, JsonSerializer12HoursDateAndTimeSettings);
        }

        private List<UserManagementPersonnelModel> GetAllUserSiteByUserInfoIdSortByColumnWithOrder(string order, string orderDir, List<UserManagementPersonnelModel> data)
        {
            List<UserManagementPersonnelModel> lst = new List<UserManagementPersonnelModel>();
            if (data != null)
            {
                switch (order)
                {
                    case "0":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.SiteId).ToList() : data.OrderBy(p => p.SiteId).ToList();
                        break;
                    case "1":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.UserSiteName).ToList() : data.OrderBy(p => p.UserSiteName).ToList();
                        break;
                    case "2":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.CraftDescription).ToList() : data.OrderBy(p => p.CraftDescription).ToList();
                        break;

                }
            }
            return lst;
        }

        [HttpPost]
        public JsonResult ValidateAddSite(long _userInfoid, long _siteid, bool _siteControlled, string _userType, long ClientId)
        {
            UsermanagmentWrapper uWrapper = new UsermanagmentWrapper(userData);
            UserDetails userdtls = new UserDetails();
            userdtls = uWrapper.ValidateAddSite(_userInfoid, _siteid, _siteControlled, _userType, ClientId);
            if (userdtls.ErrorMessages != null && userdtls.ErrorMessages.Count > 0)
            {
                return Json(userdtls.ErrorMessages, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { validationStatus = true, userInfoId = userdtls.UserInfoId }, JsonRequestBehavior.AllowGet);
            }
        }

        public PartialViewResult AddUmSite(long userInfoId, string userName, string userFirstName, string userMiddleName, string userLastName, string userType, bool isSuperUser, bool siteControlled, long ClientId, long DefaultSiteId)
        {
            UserManagementVM userdetVM = new UserManagementVM();
            UserSiteModel usrpersonnel = new UserSiteModel();
            usrpersonnel.UserInfoId = userInfoId;
            usrpersonnel.ClientId = ClientId;
            usrpersonnel.ClientLookupId = userName;
            usrpersonnel.FirstName = userFirstName;
            usrpersonnel.MiddleName = userMiddleName;
            usrpersonnel.LastName = userLastName;
            usrpersonnel.UserType = userType;
            usrpersonnel.IsSuperUser = isSuperUser;
            usrpersonnel.SiteControlled = siteControlled;
            UsermanagmentWrapper uWrapper = new UsermanagmentWrapper(userData);
            var SiteList = uWrapper.GetAllAssignedSites(userInfoId, ClientId);
            if (SiteList != null)
            {
                usrpersonnel.SiteList = SiteList;
            }

            var craftDetails = uWrapper.GetLookUpList_Craft(ClientId, DefaultSiteId);
            if (craftDetails != null)
            {
                usrpersonnel.LookupCraftList = craftDetails.Select(x => new SelectListItem { Text = x.Description, Value = x.CraftId.ToString() });
            }

            userdetVM.userSiteModel = usrpersonnel;
            LocalizeControls(userdetVM, LocalizeResourceSetConstants.UserDetails);
            return PartialView("~/Views/UserManagement/_AddEditUMSite.cshtml", userdetVM);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveUmSites(UserManagementVM objPersonnel)
        {
            string Mode = string.Empty;
            if (ModelState.IsValid)
            {
                var userInfoId = objPersonnel.userSiteModel.UserInfoId;
                var userClientId = objPersonnel.userSiteModel.ClientId;
                UsermanagmentWrapper uWrapper = new UsermanagmentWrapper(userData);
                List<string> errorMessages = new List<string>();
                Mode = "add";

                errorMessages = uWrapper.AddUmSites(objPersonnel.userSiteModel, userInfoId);

                if (errorMessages != null && errorMessages.Count > 0)
                {
                    return Json(errorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), userInfoId = userInfoId, userClientId = userClientId, mode = Mode }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                string ModelValidationFailedMessage = string.Empty;
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }


        public JsonResult ValidateRemoveSite(long _userInfoid, long _siteId, bool _siteControlled, string _userType, long ClientId)
        {
            UsermanagmentWrapper uWrapper = new UsermanagmentWrapper(userData);
            UserDetails userdtls = new UserDetails();
            userdtls = uWrapper.ValidateRemoveSite(_userInfoid, _siteId, _siteControlled, _userType, ClientId);
            if (userdtls.ErrorMessages != null && userdtls.ErrorMessages.Count > 0)
            {
                return Json(userdtls.ErrorMessages, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { validationStatus = true, userInfoId = userdtls.UserInfoId }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult DeleteUmSites(long personnelId, long userInfoId, long siteId, string userType, bool isSuperUser, long defaultSiteId, long ClientId)
        {
            UsermanagmentWrapper uWrapper = new UsermanagmentWrapper(userData);
            var deleteResult = uWrapper.UmPersonnelDelete(personnelId, userInfoId, siteId, userType, isSuperUser, defaultSiteId, ClientId);
            if (deleteResult)
            {
                return Json(new { Result = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Result = JsonReturnEnum.failed.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult GetDefaultBuyerCounts(long userInfoId, long siteId, long ClientId)
        {
            UsermanagmentWrapper aWrapper = new UsermanagmentWrapper(userData);
            var openJobsCount = aWrapper.GetCountForAdmin(userInfoId, siteId, ClientId);
            return Json(openJobsCount, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }


}