using Client.BusinessWrapper.Configuration.UserManagment;
using Client.BusinessWrapper.Common;
using Client.Common;
using Client.Controllers.Common;
using Client.Models;
using Client.Models.Configuration.UserManagement;
using Common.Constants;
using DataContracts;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.IO;
using Client.ActionFilters;
using Rotativa;
using Client.BusinessWrapper.ActiveSiteWrapper;
using System.Web;

namespace Client.Controllers.Configuration
{
    public class UserManagementController : ConfigBaseController
    {
        #region Populate UserManagment
        public ActionResult Index()
        {
            ActiveSiteWrapper objActiveSiteWrapper = new ActiveSiteWrapper(userData);
            UserManagementVM objUserManagementVM = new UserManagementVM();
            UserModel objUserModel = new UserModel();
            UsermanagmentWrapper objUserWrapper = new UsermanagmentWrapper(userData);
            string PackageLevel = userData.DatabaseKey.Client.PackageLevel;
            bool IsSuperUser = userData.DatabaseKey.User.IsSuperUser;

            var craftDetails = GetLookUpList_Craft();
            if (craftDetails != null)
            {
                objUserModel.LookupCraftList = craftDetails.Select(x => new SelectListItem { Text = x.Description.ToString(), Value = x.CraftId.ToString() }).ToList();
            }

            //Only for enterprise user            
            if (PackageLevel.ToUpper() == PackageLevelConstant.Enterprise && IsSuperUser)
            {
                var SiteList = objActiveSiteWrapper.GetSites();
                if (SiteList != null)
                {
                    objUserManagementVM.SiteList = SiteList;
                }
            }
            //V2-802 For Security Profile Ids  
            var SecurityProfileIds = objUserWrapper.GetSecurityProfileList();
            if (SecurityProfileIds != null)
            {
                objUserModel.SecurityProfileIdList = SecurityProfileIds;
            }
            #region V2-905
            var IsActiveList = UtilityFunction.IsActiveStatusWithBoolValue();
            if (IsActiveList != null)
            {
                objUserModel.IsActiveList = IsActiveList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
            }
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            var AllLookUps = commonWrapper.GetAllLookUpList();
            List<DataContracts.LookupList> ShiftLookUpList = new List<DataContracts.LookupList>();
            if (AllLookUps != null)
            {
                ShiftLookUpList = AllLookUps.Where(x => x.ListName == LookupListConstants.Shift).ToList();
                if (ShiftLookUpList != null)
                {
                    objUserModel.ShiftList = ShiftLookUpList.Select(x => new SelectListItem { Text = x.Description, Value = x.ListValue });
                }
            }
            var UserTypeList = UtilityFunction.UserTypesListValue();
            if (UserTypeList != null)
            {
                objUserModel.UserTypeList = UserTypeList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
            }
            #endregion
            objUserManagementVM.userModels = objUserModel;
            objUserManagementVM.PackageLevel = PackageLevel;
            objUserManagementVM.IsSuperUser = IsSuperUser;           

            LocalizeControls(objUserManagementVM, LocalizeResourceSetConstants.Configuration);
            return View("~/Views/Configuration/UserManagement/Index.cshtml", objUserManagementVM);

        }

        [HttpPost]
        public string PopulateuserManagment(int? draw, int? start, int? length, string UserName = "", string LastName = "", string FirstName = "", string Email = "", long CraftId = 0, string order = "1", string orderDir = "asc", int CaseNo = 1, string Sites = "", string SearchText = "", string SecurityProfileIds = "",string UserTypes="",string Shifts="", bool? IsActive=null, string EmployeeId = "")
        {
            string PackageLevel = userData.DatabaseKey.Client.PackageLevel.ToUpper();
            UsermanagmentWrapper objUserWrapper = new UsermanagmentWrapper(userData);
            List<UserManagementModel> retUserManagmentlist = null;             
            if (PackageLevel == PackageLevelConstant.Enterprise && userData.DatabaseKey.User.IsSuperUser)
                retUserManagmentlist = objUserWrapper.PopulateuserManagmentForEnterprise(start ?? 0, length ?? 10, CaseNo, UserName, LastName, FirstName, Email, CraftId, SearchText, order, orderDir, Sites, SecurityProfileIds, UserTypes, Shifts, IsActive, EmployeeId);
            else
                retUserManagmentlist = objUserWrapper.PopulateuserManagmentForBasicProfessional(start ?? 0, length ?? 10, CaseNo, UserName, LastName, FirstName, Email, CraftId, SearchText, order, orderDir, Sites, SecurityProfileIds, UserTypes, Shifts, IsActive, EmployeeId);

            long TotalCount = 0;
            if (retUserManagmentlist != null && retUserManagmentlist.Count > 0)
                TotalCount = retUserManagmentlist[0].TotalCount;
            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = TotalCount, recordsFiltered = TotalCount, data = retUserManagmentlist }, JsonSerializer12HoursDateAndTimeSettings);
        }
        [HttpGet]
        public ActionResult PopulateuserManagmentCraftDetails(long UserInfoId)
        {
            UserManagementVM objUserManagementVM = new UserManagementVM();
            UsermanagmentWrapper objUserWrapper = new UsermanagmentWrapper(userData);
            objUserManagementVM.CraftList = objUserWrapper.PopulateuserManagmentCraftDetails(UserInfoId);
            LocalizeControls(objUserManagementVM, LocalizeResourceSetConstants.UserDetails);
            return PartialView("~/Views/Configuration/UserManagement/_InnerGridCraft.cshtml", objUserManagementVM);
        }
        [HttpGet]
        public string UserDetailsPrintData(string UserName = "", string LastName = "", string FirstName = "", string Email = "", long CraftId = 0,
            string order = "1", string orderDir = "asc", int CaseNo = 1, string SelectedSites = "", string SearchText = "", string SecurityProfileIds = "", string UserTypes = "", string Shifts = "", bool? IsActive = null, string EmployeeId = "")
        {
            string PackageLevel = userData.DatabaseKey.Client.PackageLevel.ToUpper();
            UsermanagmentWrapper objUserWrapper = new UsermanagmentWrapper(userData);
            List<UserManagementModel> retUserManagmentlist = null;
            List<PrintUserModel> PrintUserModelList = null;
            List<PrintUserModel_Enterprise> PrintUserModel_EnterpriseList = null;
            bool IsSuperUser = userData.DatabaseKey.User.IsSuperUser;

            //if (PackageLevel == PackageLevelConstant.Enterprise && IsSuperUser)
            //    retUserManagmentlist = objUserWrapper.PopulateuserManagmentForEnterprise(0, 100000, CaseNo, UserName, LastName, FirstName, Email, CraftId, SearchText, order, orderDir, SelectedSites, SecurityProfileIds);
            //else
            //    retUserManagmentlist = objUserWrapper.PopulateuserManagmentForBasicProfessional(0, 100000, CaseNo, UserName, LastName, FirstName, Email, CraftId, SearchText, order, orderDir, SelectedSites, SecurityProfileIds);


            if (PackageLevel == PackageLevelConstant.Enterprise && userData.DatabaseKey.User.IsSuperUser)
                retUserManagmentlist = objUserWrapper.PopulateuserManagmentForEnterprise(0, 100000, CaseNo, UserName, LastName, FirstName, Email, CraftId, SearchText, order, orderDir, SelectedSites, SecurityProfileIds, UserTypes, Shifts, IsActive, EmployeeId);
            else
                retUserManagmentlist = objUserWrapper.PopulateuserManagmentForBasicProfessional(0, 100000, CaseNo, UserName, LastName, FirstName, Email, CraftId, SearchText, order, orderDir, SelectedSites, SecurityProfileIds, UserTypes, Shifts, IsActive, EmployeeId);

            if (retUserManagmentlist != null && PackageLevel == PackageLevelConstant.Enterprise && IsSuperUser)
            {
                PrintUserModel_EnterpriseList = new List<PrintUserModel_Enterprise>();
                PrintUserModel_Enterprise objPrintModel = null;
                foreach (var item in retUserManagmentlist)
                {
                    objPrintModel = new PrintUserModel_Enterprise();
                    objPrintModel.UserName = item.UserName;
                    objPrintModel.LastName = item.LastName;
                    objPrintModel.FirstName = item.FirstName;
                    objPrintModel.SecurityProfile = item.SecurityProfileDescription;
                    objPrintModel.Email = item.Email;
                    objPrintModel.EmployeeId = item.EmployeeId; //V2-1160
                    PrintUserModel_EnterpriseList.Add(objPrintModel);
                }
            }
            else if (retUserManagmentlist != null)
            {
                PrintUserModelList = new List<PrintUserModel>();
                PrintUserModel objPrintModel = null;
                foreach (var item in retUserManagmentlist)
                {
                    objPrintModel = new PrintUserModel();
                    objPrintModel.UserName = item.UserName;
                    objPrintModel.LastName = item.LastName;
                    objPrintModel.FirstName = item.FirstName;
                    objPrintModel.SecurityProfile = item.SecurityProfileDescription;
                    objPrintModel.Email = item.Email;
                    objPrintModel.Craft = item.CraftDescription;
                    objPrintModel.EmployeeId = item.EmployeeId; //V2-1160
                    PrintUserModelList.Add(objPrintModel);
                }
            }
            if (PackageLevel == PackageLevelConstant.Enterprise && IsSuperUser)
                return JsonConvert.SerializeObject(new { data = PrintUserModel_EnterpriseList }, JsonSerializerDateSettings);
            else
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
            string PackageLevel = userData.DatabaseKey.Client.PackageLevel.ToUpper();

            if (PackageLevel == PackageLevelConstant.Enterprise && userData.DatabaseKey.User.IsSuperUser)
            {
                objSearchList = uWrapper.PopulateuserManagmentForEnterprise(0, 100000, objPrintParams.CaseNo, objPrintParams.UserName,
            objPrintParams.LastName, objPrintParams.FirstName, objPrintParams.Email, objPrintParams.CraftId, objPrintParams.SearchText,
            objPrintParams.order, objPrintParams.orderDir, objPrintParams.SelectedSites, objPrintParams.SecurityProfileIds, objPrintParams.UserTypes, objPrintParams.Shifts, objPrintParams.IsActive, objPrintParams.EmployeeId);
            }

            foreach (var data in objSearchList)
            {
                objPrintModel = new UMPdfPrintModel();
                objPrintModel.UserName = data.UserName;
                objPrintModel.LastName = data.LastName;
                objPrintModel.FirstName = data.FirstName;
                objPrintModel.SecurityProfile = data.SecurityProfileDescription;
                objPrintModel.Email = data.Email;
                objPrintModel.EmployeeId = data.EmployeeId; //V2-1160
                if (data.SiteCount > 0)
                {
                    objPrintModel.innerGridCrafts = uWrapper.PopulateuserManagmentCraftDetails(data.UserInfoId);
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
                return new PartialViewAsPdf("~/Views/Configuration/UserManagement/UMGridPdfPrintTemplate.cshtml", objUserManagementVM)
                {
                    PageSize = Rotativa.Options.Size.A4,
                    FileName = "User Management.pdf",
                    PageMargins = { Left = 3, Right = 3, Top = 3, Bottom = 3 },
                };
            }
            else
            {
                return new PartialViewAsPdf("~/Views/Configuration/UserManagement/UMGridPdfPrintTemplate.cshtml", objUserManagementVM)
                {
                    PageSize = Rotativa.Options.Size.A4,
                    PageMargins = { Left = 3, Right = 3, Top = 3, Bottom = 3 },
                };
            }

        }

        private List<PrintUserModel> GetUserSearchPrintResult(List<PrintUserModel> userList, string UserName = "", string LastName = "", string FirstName = "",
                                        string Email = "", long CraftId = 0)
        {
            if (userList != null)
            {
                if (!string.IsNullOrEmpty(UserName))
                {
                    UserName = UserName.ToUpper();
                    userList = userList.Where(x => (!string.IsNullOrWhiteSpace(x.UserName) && x.UserName.ToUpper().Contains(UserName))).ToList();
                }
                if (!string.IsNullOrEmpty(LastName))
                {
                    LastName = LastName.ToUpper();
                    userList = userList.Where(x => (!string.IsNullOrWhiteSpace(x.LastName) && x.LastName.ToUpper().Contains(LastName))).ToList();
                }
                if (!string.IsNullOrEmpty(FirstName))
                {
                    FirstName = FirstName.ToUpper();
                    userList = userList.Where(x => (!string.IsNullOrWhiteSpace(x.FirstName) && x.FirstName.ToUpper().Contains(FirstName))).ToList();
                }
                if (!string.IsNullOrEmpty(Email))
                {
                    Email = Email.ToUpper();
                    userList = userList.Where(x => (!string.IsNullOrWhiteSpace(x.Email) && x.Email.ToUpper().Contains(Email))).ToList();
                }

                if (CraftId > 0)
                {
                    userList = userList.Where(x => x.PersonnelCraftId == CraftId).ToList();
                }
            }
            return userList;
        }
        private List<UserManagementModel> GetUserSearchResult(List<UserManagementModel> userList, string UserName = "", string LastName = "", string FirstName = "",
                                            string Email = "", long CraftId = 0)
        {
            if (userList != null)
            {
                if (!string.IsNullOrEmpty(UserName))
                {
                    UserName = UserName.ToUpper();
                    userList = userList.Where(x => (!string.IsNullOrWhiteSpace(x.UserName) && x.UserName.ToUpper().Contains(UserName))).ToList();
                }
                if (!string.IsNullOrEmpty(LastName))
                {
                    LastName = LastName.ToUpper();
                    userList = userList.Where(x => (!string.IsNullOrWhiteSpace(x.LastName) && x.LastName.ToUpper().Contains(LastName))).ToList();
                }
                if (!string.IsNullOrEmpty(FirstName))
                {
                    FirstName = FirstName.ToUpper();
                    userList = userList.Where(x => (!string.IsNullOrWhiteSpace(x.FirstName) && x.FirstName.ToUpper().Contains(FirstName))).ToList();
                }
                if (!string.IsNullOrEmpty(Email))
                {
                    Email = Email.ToUpper();
                    userList = userList.Where(x => (!string.IsNullOrWhiteSpace(x.Email) && x.Email.ToUpper().Contains(Email))).ToList();
                }
                if (CraftId > 0)
                {
                    userList = userList.Where(x => x.PersonnelCraftId == CraftId).ToList();
                }
            }
            return userList;
        }
        private List<UserManagementModel> GetAllUserManagmentSortByColumnWithOrder(string order, string orderDir, List<UserManagementModel> data)
        {
            List<UserManagementModel> lst = new List<UserManagementModel>();
            if (data != null)
            {
                switch (order)
                {
                    case "0":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.UserName).ToList() : data.OrderBy(p => p.UserName).ToList();
                        break;
                    case "1":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.LastName).ToList() : data.OrderBy(p => p.LastName).ToList();
                        break;
                    case "2":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.FirstName).ToList() : data.OrderBy(p => p.FirstName).ToList();
                        break;
                    case "3":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Email).ToList() : data.OrderBy(p => p.Email).ToList();
                        break;
                    case "4":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.CraftDescription).ToList() : data.OrderBy(p => p.CraftDescription).ToList();
                        break;
                    default:
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.UserName).ToList() : data.OrderBy(p => p.UserName).ToList();
                        break;
                }
            }
            return lst;
        }


        private List<PrintUserModel> GetAllUserManagmentSortByColumnWithOrder(string order, string orderDir, List<PrintUserModel> data)
        {
            List<PrintUserModel> lst = new List<PrintUserModel>();
            if (data != null)
            {
                switch (order)
                {
                    case "0":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.UserName).ToList() : data.OrderBy(p => p.UserName).ToList();
                        break;
                    case "1":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.LastName).ToList() : data.OrderBy(p => p.LastName).ToList();
                        break;
                    case "2":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.FirstName).ToList() : data.OrderBy(p => p.FirstName).ToList();
                        break;
                    case "3":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Email).ToList() : data.OrderBy(p => p.Email).ToList();
                        break;
                    case "4":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Craft).ToList() : data.OrderBy(p => p.Craft).ToList();
                        break;

                    default:
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.UserName).ToList() : data.OrderBy(p => p.UserName).ToList();
                        break;
                }
            }
            return lst;
        }


        //public PartialViewResult UserManagementDetails(long UserId = 0)
        //{
        //    UsermanagmentWrapper _userWraperObj = new UsermanagmentWrapper(userData);
        //    UserManagementVM userdetVM = new UserManagementVM();
        //    DataContracts.UserDetails userdet = new DataContracts.UserDetails();
        //    UserModel _userModel = new UserModel();
        //    userdet.UserInfoId = UserId;
        //    userdet.ClientId = userData.DatabaseKey.Client.ClientId;
        //    CommonWrapper commonWrapper = new CommonWrapper(userData);           
        //    userdet.RetrieveUserDetailsByUserInfoID(this.userData.DatabaseKey);
        //    userdet.RetrievePersonnelByUserInfoId(this.userData.DatabaseKey, string.Empty);
        //    if (userdet != null)
        //    {
        //        _userModel.UserInfoId = userdet.UserInfoId;
        //        _userModel.UserName = userdet.UserName;
        //        _userModel.FirstName = userdet.FirstName;
        //        _userModel.MiddleName = userdet.MiddleName;
        //        _userModel.LastName = userdet.LastName;
        //        _userModel.CraftId = userdet.Personnel.CraftId;
        //        _userModel.Shift = userdet.Personnel.Shift;
        //        _userModel.SecurityQuestion = userdet.SecurityQuestion;
        //        _userModel.SecurityResponse = userdet.SecurityResponse;
        //        _userModel.Email = userdet.Email;
        //        _userModel.SecurityProfileId = userdet.SecurityProfileId;
        //        _userModel.IsActive = userdet.IsActive;
        //        _userModel.UserType = userdet.UserType;
        //        _userModel.IsSuperUser = userdet.IsSuperUser;
        //        _userModel.Buyer = userdet.Personnel.Buyer;
        //        _userModel.Phone1 = userdet.Personnel.Phone1;
        //        _userModel.Phone2 = userdet.Personnel.Phone2;
        //        _userModel.Address1 = userdet.Personnel.Address1;
        //        _userModel.Address2 = userdet.Personnel.Address2;
        //        _userModel.Address3 = userdet.Personnel.Address3;
        //        _userModel.AddressCity = userdet.Personnel.AddressCity;
        //        _userModel.AddressState = userdet.Personnel.AddressState;
        //        _userModel.AddressPostCode = userdet.Personnel.AddressPostCode;
        //        _userModel.AddressCountry = userdet.Personnel.AddressCountry;
        //        _userModel.PersonnelId = userdet.Personnel.PersonnelId;
        //        _userModel.ClientLookupId = userdet.Personnel.ClientLookupId;
        //        _userModel._userdata=  this.userData;
        //        var AllLookUps = commonWrapper.GetAllLookUpList();
        //        if (AllLookUps != null)
        //        {
        //            var Shift = AllLookUps.Where(x => x.ListName == LookupListConstants.Shift).ToList();
        //            if (Shift != null && Shift.Any(cus => cus.ListValue == userdet.Personnel.Shift))
        //            {
        //                _userModel.Shift = Shift.Where(x => x.ListValue == userdet.Personnel.Shift).Select(x => x.Description).First();
        //            }
        //        }
        //        var craftDetails = GetLookUpList_Craft();
        //        if (craftDetails != null)
        //        {
        //            var tempCraft = craftDetails.Where(x => x.CraftId == userdet.Personnel.CraftId).ToList();
        //            if(tempCraft.Count > 0)
        //            {
        //                _userModel.CraftName = craftDetails.Where(x => x.CraftId == userdet.Personnel.CraftId).ToList().Select(x => x.Description).First();
        //            }
        //        }
        //        LocalizeControls(userdetVM, LocalizeResourceSetConstants.UserDetails);
        //        //var SecurityProfilesList = _userWraperObj.LoadSecurityProfilesList(userdetVM,_userModel);
        //        //if (SecurityProfilesList != null)
        //        //{
        //        //    var tempSecurityProfilesList = SecurityProfilesList.Where(x => x.Value == userdet.SecurityProfileId.ToString()).ToList();
        //        //    if(tempSecurityProfilesList.Count > 0)
        //        //    {
        //        //        _userModel.SecurityProfile = SecurityProfilesList.Where(x => x.Value == userdet.SecurityProfileId.ToString()).Select(x => x.Text).First();
        //        //    }
        //        //}
        //    }
        //    userdetVM.userModels = _userModel;
        //    //LocalizeControls(userdetVM, LocalizeResourceSetConstants.UserDetails);
        //    return PartialView("~/Views/Configuration/UserManagement/UserManagementDetails.cshtml", userdetVM);
        //}
        #endregion

        #region Edit-User
        //[HttpGet]
        //public ActionResult UserEdit(long UserInfoId)
        //{
        //    UserManagementVM userdetVM = new UserManagementVM();
        //    DataContracts.UserDetails userdet = new DataContracts.UserDetails();
        //    UserModel _userModel = new UserModel();
        //    userdet.UserInfoId = UserInfoId;
        //    userdet.ClientId = userData.DatabaseKey.Client.ClientId;
        //    UsermanagmentWrapper _userObj = new UsermanagmentWrapper(userData);
        //    userdet.RetrieveUserDetailsByUserInfoID(this.userData.DatabaseKey);
        //    userdet.RetrievePersonnelByUserInfoId(this.userData.DatabaseKey, string.Empty);
        //    if (userdet != null)
        //    {
        //        _userModel.UserInfoId = userdet.UserInfoId;
        //        _userModel.UserName = userdet.UserName;
        //        _userModel.FirstName = userdet.FirstName;
        //        _userModel.MiddleName = userdet.MiddleName;
        //        _userModel.LastName = userdet.LastName;
        //        _userModel.CraftId = userdet.Personnel.CraftId;
        //        _userModel.Shift = userdet.Personnel.Shift;
        //        _userModel.SecurityQuestion = userdet.SecurityQuestion;
        //        _userModel.SecurityResponse = userdet.SecurityResponse;
        //        _userModel.Email = userdet.Email;
        //        _userModel.SecurityProfileId = userdet.SecurityProfileId;
        //        _userModel.IsActive = userdet.IsActive;
        //        _userModel.UserType = userdet.UserType;
        //        _userModel.IsSuperUser = userdet.IsSuperUser;
        //        _userModel.Password = userdet.Password;
        //        _userModel.Buyer = userdet.Personnel.Buyer;
        //        _userModel.Phone1 = userdet.Personnel.Phone1;
        //        _userModel.Phone2 = userdet.Personnel.Phone2;
        //        _userModel.Address1 = userdet.Personnel.Address1;
        //        _userModel.Address2 = userdet.Personnel.Address2;
        //        _userModel.Address3 = userdet.Personnel.Address3;
        //        _userModel.AddressCity = userdet.Personnel.AddressCity;
        //        _userModel.AddressState = userdet.Personnel.AddressState;
        //        _userModel.AddressPostCode = userdet.Personnel.AddressPostCode;
        //        _userModel.AddressCountry = userdet.Personnel.AddressCountry;
        //        _userModel.PersonnelId = userdet.Personnel.PersonnelId;
        //        _userModel.ClientLookupId = userdet.Personnel.ClientLookupId;
        //        _userModel._userdata = this.userData;
        //    }
        //    _userModel.SecurityBBUUser = _userObj.IsbbuUsers(userdet.UserName);
        //    LocalizeControls(userdetVM, LocalizeResourceSetConstants.UserDetails);
        //    _userModel = _userObj.PopulateDropdownControls(userdetVM,_userModel);
        //    userdetVM.userModels = _userModel;
        //    return PartialView("~/Views/Configuration/UserManagement/_UserEditNew.cshtml", userdetVM);
        //}
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult UserEdit(UserManagementVM userdetVM)
        //{
        //    List<string> errorMessages = new List<string>();
        //    if (ModelState.IsValid)
        //    {
        //        UsermanagmentWrapper _userObj = new UsermanagmentWrapper(userData);
        //        if (userdetVM.userModels.UserType  == UserTypeConstants.Full)
        //        {
        //            if (userdetVM.userModels.SecurityProfileId == 0 || userdetVM.userModels.SecurityProfileId == 9)
        //            {
        //                string ModelValidationFailedMessage = string.Empty;
        //                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalSecurityProfileValidationMsg", LocalizeResourceSetConstants.Global);
        //                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
        //            }
        //        }
        //       else if (userdetVM.userModels.UserType.ToLower() == UserTypeConstants.WorkRequest.ToLower())
        //        {
        //            userdetVM.userModels.SecurityProfileId = 9;
        //        }
        //        else if(userdetVM.userModels.UserType.ToLower() == UserTypeConstants.Reference.ToLower() && userdetVM.userModels.SecurityProfileId != 0)
        //        {
        //            userdetVM.userModels.SecurityProfileId = 0;
        //        }
        //        UserModel _userdetailsObj = _userObj.EditUser(userdetVM.userModels, userdetVM.userModels.UserInfoId);
        //        if (_userdetailsObj.ErrorMessages != null && _userdetailsObj.ErrorMessages.Count > 0)
        //        {
        //            return Json(_userdetailsObj.ErrorMessages, JsonRequestBehavior.AllowGet);
        //        }
        //        else
        //        {
        //            return Json(new { Result = JsonReturnEnum.success.ToString(), UserInfoId = _userdetailsObj.UserInfoId }, JsonRequestBehavior.AllowGet);
        //        }
        //    }
        //    else
        //    {
        //        string ModelValidationFailedMessage = string.Empty;
        //        ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
        //        return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
        //    }

        //}
        #endregion

        #region Notes

        [HttpPost]
        public string PopulateNotes(int? draw, int? start, int? length, long personnelId)
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];

            CommonWrapper cWrapper = new CommonWrapper(userData);

            var Notes = cWrapper.PopulateNotes(personnelId, "Personnel");
            Notes = this.GetAllNotesSortByColumnWithOrder(order, orderDir, Notes);
            var totalRecords = 0;
            var recordsFiltered = 0;
            start = start.HasValue
                ? start / length
                : 0;
            recordsFiltered = Notes.Count();
            totalRecords = Notes.Count();
            int initialPage = start.Value;
            var filteredResult = Notes
                .Skip(initialPage * length ?? 0)
                .Take(length ?? 0)
                .ToList();

            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult }, JsonSerializer12HoursDateAndTimeSettings);
        }
        private List<NotesModel> GetAllNotesSortByColumnWithOrder(string order, string orderDir, List<NotesModel> data)
        {
            List<NotesModel> lst = new List<NotesModel>();
            if (data != null)
            {
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
                    default:
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Subject).ToList() : data.OrderBy(p => p.Subject).ToList();
                        break;
                }
            }
            return lst;
        }

        [HttpPost]
        public PartialViewResult AddUMNotes(long userInfoId, long personnelId, string clientLookupId, string ownerName)
        {
            UserManagementVM userdetVM = new UserManagementVM();
            NotesModel Notes = new NotesModel();
            Notes.UserInfoId = userInfoId;
            Notes.ClientLookupId = clientLookupId;
            Notes.ObjectId = personnelId;
            Notes.OwnerName = ownerName;
            userdetVM.notesModel = Notes;
            LocalizeControls(userdetVM, LocalizeResourceSetConstants.UserDetails);
            return PartialView("~/Views/Configuration/UserManagement/_AddEditUMNote.cshtml", userdetVM);
        }
        [HttpPost]
        public PartialViewResult EditUMNotes(long userInfoId, long personnelId, long notesId, string clientLookupId)
        {
            UserManagementVM userdetVM = new UserManagementVM();
            NotesModel notesModel = new NotesModel();
            UsermanagmentWrapper uWrapper = new UsermanagmentWrapper(userData);
            notesModel = uWrapper.RetrieveNotesForEdit(personnelId, notesId);
            notesModel.ClientLookupId = clientLookupId;
            notesModel.UserInfoId = userInfoId;
            //notesModel.ClientLookupId = clientLookupId;
            userdetVM.notesModel = notesModel;
            LocalizeControls(userdetVM, LocalizeResourceSetConstants.UserDetails);
            return PartialView("~/Views/Configuration/UserManagement/_AddEditUMNote.cshtml", userdetVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddNotes(UserManagementVM userdetVM)
        {
            UsermanagmentWrapper uWrapper = new UsermanagmentWrapper(userData);
            List<string> errorMessages = new List<string>();
            if (ModelState.IsValid)
            {
                string Mode = string.Empty;
                if (userdetVM.notesModel.NotesId == 0)
                {
                    Mode = "add";
                    errorMessages = uWrapper.AddNotes(userdetVM.notesModel, "Personnel");
                }
                else
                {
                    Mode = "edit";
                    errorMessages = uWrapper.EditNotes(userdetVM.notesModel, "Personnel");
                }
                if (errorMessages != null && errorMessages.Count > 0)
                {
                    return Json(errorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), UserInfoId = userdetVM.notesModel.UserInfoId, mode = Mode }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                string ModelValidationFailedMessage = string.Empty;
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult DeleteUMNotes(long notesId)
        {
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            //UsermanagmentWrapper uWrapper = new UsermanagmentWrapper(userData);
            if (commonWrapper.DeleteNotes(notesId))
            {
                return Json(new { Result = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Result = JsonReturnEnum.failed.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion Notes

        #region Attachments
        [HttpPost]
        public string PopulateUmAttachments(int? draw, int? start, int? length, long personnelId)
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            CommonWrapper objCommonWrapper = new CommonWrapper(userData);
            var Attachments = objCommonWrapper.PopulateAttachments(personnelId, "Personnel", false);
            if (Attachments != null)
            {
                Attachments = GetAllAttachmentsSortByColumnWithOrder(order, orderDir, Attachments);
            }
            var totalRecords = 0;
            var recordsFiltered = 0;
            start = start.HasValue
                ? start / length
                : 0;
            recordsFiltered = Attachments.Count();
            totalRecords = Attachments.Count();
            int initialPage = start.Value;
            var filteredResult = Attachments
                .Skip(initialPage * length ?? 0)
                .Take(length ?? 0)
                .ToList();

            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult }, JsonSerializer12HoursDateAndTimeSettings);
        }
        [HttpGet]
        public PartialViewResult AddUmAttachments(long userInfoId, long personnelId, string userName)
        {
            UserManagementVM userdetVM = new UserManagementVM();
            AttachmentModel objAttachment = new AttachmentModel();
            UserManagementModel userDetails = new UserManagementModel();
            userDetails.UserInfoId = userInfoId;
            userDetails.PersonnelID = personnelId;
            userDetails.UserName = userName;
            objAttachment.PersonnelId = personnelId;
            objAttachment.UserInfoId = userInfoId;
            objAttachment.ClientLookupId = userName;
            userdetVM.attachmentModel = new AttachmentModel();
            userdetVM.attachmentModel = objAttachment;
            userdetVM.UserManagementModel = userDetails;
            LocalizeControls(userdetVM, LocalizeResourceSetConstants.UserDetails);
            return PartialView("~/Views/Configuration/UserManagement/_AddUMAttachment.cshtml", userdetVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveUmAttachments()
        {
            string ModelValidationFailedMessage = string.Empty;
            if (ModelState.IsValid)
            {
                var subject = Request.Form["attachmentModel.Subject"];
                subject = subject.Replace(",", "");
                if (subject.Equals(",") || subject == null || subject == "")
                {
                    subject = "No Subject";
                }
                Stream stream = Request.Files[0].InputStream;
                Client.Models.AttachmentModel attachmentModel = new Client.Models.AttachmentModel();
                CommonWrapper objCommonWrapper = new CommonWrapper(userData);
                Attachment fileAtt = new Attachment();
                attachmentModel.FileName = Request.Files[0].FileName.ToString().Split('.')[0].ToString();
                attachmentModel.FileSize = Request.Files[0].ContentLength;
                attachmentModel.FileType = System.IO.Path.GetExtension(Request.Files[0].FileName).Replace(".", "");
                attachmentModel.ContentType = Request.Files[0].ContentType;
                attachmentModel.ObjectId = Convert.ToInt64(Request.Form["attachmentModel.PersonnelId"]);
                attachmentModel.Subject = subject;
                attachmentModel.TableName = "Personnel";
                bool attachStatus = false;
                bool OnPremise = userData.DatabaseKey.Client.OnPremise;
                if (OnPremise)
                {
                    fileAtt = objCommonWrapper.AddAttachmentOnPremise(attachmentModel, stream, ref attachStatus, false);
                }
                else
                {
                    fileAtt = objCommonWrapper.AddAttachment(attachmentModel, stream, ref attachStatus, false);
                }
                if (attachStatus)
                {
                    if (fileAtt.ErrorMessages != null && fileAtt.ErrorMessages.Count > 0)
                    {
                        return Json(fileAtt.ErrorMessages, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { Result = JsonReturnEnum.success.ToString(), umid = Convert.ToInt64(Request.Form["UserManagementModel.UserInfoId"]) }, JsonRequestBehavior.AllowGet);
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
        public ActionResult DeleteUmAttachments(long fileAttachmentId)
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

        #region Contacts
        [HttpPost]
        public string PopulateUmContacts(int? draw, int? start, int? length, long personnelId)
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            UsermanagmentWrapper uWrapper = new UsermanagmentWrapper(userData);
            var ContactList = uWrapper.PopulateUmContacts(personnelId);
            ContactList = this.GetAllContactsSortByColumnWithOrder(order, orderDir, ContactList);
            var totalRecords = 0;
            var recordsFiltered = 0;
            start = start.HasValue
                ? start / length
                : 0;
            recordsFiltered = ContactList.Count();
            totalRecords = ContactList.Count();
            int initialPage = start.Value;
            var filteredResult = ContactList
               .Skip(initialPage * length ?? 0)
               .Take(length ?? 0)
               .ToList();

            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult }, JsonSerializer12HoursDateAndTimeSettings);
        }
        private List<UserManagementContactModel> GetAllContactsSortByColumnWithOrder(string order, string orderDir, List<UserManagementContactModel> data)
        {
            List<UserManagementContactModel> lst = new List<UserManagementContactModel>();
            if (data != null)
            {
                switch (order)
                {
                    case "0":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Name).ToList() : data.OrderBy(p => p.Name).ToList();
                        break;
                    case "1":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Phone1).ToList() : data.OrderBy(p => p.Phone1).ToList();
                        break;
                    case "2":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Email1).ToList() : data.OrderBy(p => p.Email1).ToList();
                        break;
                    case "3":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.OwnerName).ToList() : data.OrderBy(p => p.OwnerName).ToList();
                        break;
                    default:
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Name).ToList() : data.OrderBy(p => p.Name).ToList();
                        break;
                }
            }
            return lst;
        }
        public PartialViewResult AddUmContact(long userInfoId, string userName, string ownerName, long personnelId)
        {
            UserManagementVM userdetVM = new UserManagementVM();
            UserManagementContactModel contact = new UserManagementContactModel();

            contact.UserInfoId = userInfoId;
            contact.ClientLookupId = userName;
            contact.OwnerName = ownerName;
            contact.ObjectId = personnelId;
            contact.OwnerId = userInfoId;

            var countryList = UtilityFunction.CountryList();
            if (countryList != null)
            {
                contact.CountryList = countryList.Select(x => new SelectListItem { Text = x.text, Value = x.value });
            }
            var stateList = UtilityFunction.StateList();
            if (stateList != null)
            {
                contact.StateList = stateList.Select(x => new SelectListItem { Text = x.value, Value = x.value });
            }
            userdetVM.userManagementContactModel = contact;
            LocalizeControls(userdetVM, LocalizeResourceSetConstants.UserDetails);
            return PartialView("~/Views/Configuration/UserManagement/_AddEditUMContact.cshtml", userdetVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveUmContacts(UserManagementVM objContact)
        {
            string Mode = string.Empty;
            if (ModelState.IsValid)
            {
                if (objContact.userManagementContactModel.AddressCountry != null)
                {
                    if (objContact.userManagementContactModel.AddressCountry.Equals("USA"))
                    {
                        objContact.userManagementContactModel.AddressState = objContact.userManagementContactModel.AddressStateForUSA;
                    }
                    else
                    {
                        objContact.userManagementContactModel.AddressState = objContact.userManagementContactModel.AddressStateForOther;
                    }
                }
                var userInfoId = objContact.userManagementContactModel.UserInfoId;
                UsermanagmentWrapper uWrapper = new UsermanagmentWrapper(userData);
                List<string> errorMessages = new List<string>();
                if (objContact.userManagementContactModel.ContactId != 0)
                {
                    Mode = "update";
                    errorMessages = uWrapper.EditUmContacts(objContact.userManagementContactModel);
                }
                else
                {
                    Mode = "add";
                    errorMessages = uWrapper.AddUmContacts(objContact.userManagementContactModel);
                }
                if (errorMessages != null && errorMessages.Count > 0)
                {
                    return Json(errorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), userInfoId = userInfoId, mode = Mode }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                string ModelValidationFailedMessage = string.Empty;
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public PartialViewResult EditUmContact(long userInfoId, long contactId, int updatedindex, string userName, long personelId)
        {
            UserManagementVM userdetVM = new UserManagementVM();
            UserManagementContactModel contact = new UserManagementContactModel();
            UsermanagmentWrapper uWrapper = new UsermanagmentWrapper(userData);
            contact = uWrapper.RetrieveContactsForEdit(contactId);
            var countryList = UtilityFunction.CountryList();
            var valCountry = countryList.Where(x => x.value == contact.AddressCountry).FirstOrDefault();
            if (valCountry != null)
            {
                contact.AddressCountry = valCountry.value;
            }
            if (countryList != null)
            {
                contact.CountryList = countryList.Select(x => new SelectListItem { Text = x.text, Value = x.value });
            }
            var stateList = UtilityFunction.StateList();
            if (valCountry != null)
            {
                if (valCountry.value.Equals("USA"))
                {
                    var valState = stateList.Where(x => x.value == contact.AddressState).FirstOrDefault();
                    if (valState != null)
                    {
                        contact.AddressStateForUSA = valState.value;
                    }
                }
                else
                {
                    contact.AddressStateForOther = contact.AddressState;
                }
            }
            if (stateList != null)
            {
                contact.StateList = stateList.Select(x => new SelectListItem { Text = x.value, Value = x.value });
            }
            contact.UserInfoId = userInfoId;
            contact.ClientLookupId = userName;
            contact.ContactId = contactId;
            contact.PersonnelId = personelId;
            userdetVM.userManagementContactModel = contact;
            LocalizeControls(userdetVM, LocalizeResourceSetConstants.UserDetails);
            return PartialView("~/Views/Configuration/UserManagement/_AddEditUMContact.cshtml", userdetVM);
        }
        [HttpPost]
        public ActionResult DeleteUmContacts(long contactId)
        {
            UsermanagmentWrapper uWrapper = new UsermanagmentWrapper(userData);
            var deleteResult = uWrapper.UmContactDelete(contactId);
            if (deleteResult)
            {
                return Json(new { Result = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Result = JsonReturnEnum.failed.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion Contacts

        #region AddNew User
        [HttpGet]
        public ActionResult AddUser()
        {
            ActiveSiteWrapper objActiveSiteWrapper = new ActiveSiteWrapper(userData);
            UserModel objuser = new UserModel();
            UserManagementVM objuserVM = new UserManagementVM();

            var craftDetails = GetLookUpList_Craft();
            if (craftDetails != null)
            {
                objuser.LookupCraftList = craftDetails.Select(x => new SelectListItem { Text = x.ClientLookUpId + " - " + x.Description + " - " + x.ChargeRate, Value = x.CraftId.ToString() });
            }
            objuser._userdata = this.userData;

            var siteList = objActiveSiteWrapper.GetSites();
            objuser.SiteList = siteList;
            objuser.PackageLevel = this.userData.DatabaseKey.Client.PackageLevel;

            objuserVM.userModels = objuser;
            LocalizeControls(objuserVM, LocalizeResourceSetConstants.UserDetails);
            return PartialView("~/Views/Configuration/UserManagement/_UserAddNew.cshtml", objuserVM);
        }


        [HttpPost]
        public ActionResult CheckIfUserExistForUserAdd(UserManagementVM userManagementVM)
        {
            return Json(!IfUserExist(userManagementVM.userModels.UserName), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        
        #endregion

        #region Reset-Password

        public JsonResult GetUserDetailForResetPassWord(long UserInfoId)
        {
            UsermanagmentWrapper uWrapper = new UsermanagmentWrapper(userData);
            var userDetails = uWrapper.GetUserDetails(UserInfoId);
            return Json(userDetails, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(UserManagementVM _user)
        {
            string AlertMessage = string.Empty;
            if (ModelState.IsValid)
            {
                UsermanagmentWrapper uWrapper = new UsermanagmentWrapper(userData);
                var message = uWrapper.ResetPassword(_user.resetPasswordModel);
                //localization
                if (message == "SuccessAlert")
                {
                    AlertMessage = "The password for {0} (User Name = {1}) has been successfully reset";
                    AlertMessage = string.Format(AlertMessage, _user.resetPasswordModel.FirstName + ' ' + _user.resetPasswordModel.LastName, _user.resetPasswordModel.UserName);
                }
                else if (message == "SuccessEmailFail")
                {
                    AlertMessage = "The password for {0} (User Name = {1}) has been successfully reset.  The system was unable to send an email to {2}.";
                    AlertMessage = string.Format(AlertMessage, _user.resetPasswordModel.FirstName + ' ' + _user.resetPasswordModel.LastName, _user.resetPasswordModel.UserName, _user.resetPasswordModel.EmailAddress);
                }
                else if (message == "TargetUserAlert")
                {
                    AlertMessage = "The password for user {0} has been successfully reset where {1} is the target user’s login id";
                    AlertMessage = string.Format(AlertMessage, _user.resetPasswordModel.FirstName + ' ' + _user.resetPasswordModel.LastName, _user.resetPasswordModel.UserName);
                }
                return Json(new { Result = JsonReturnEnum.success.ToString(), Alert = AlertMessage }, JsonRequestBehavior.AllowGet);

            }
            else
            {
                string ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }

        }

        #endregion

        #region V2-332
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddUser(UserManagementVM _user, string Command)
        {
            string ModelValidationFailedMessage = string.Empty;
            if (ModelState.IsValid)
            {
                UsermanagmentWrapper _userObj = new UsermanagmentWrapper(userData);
                UserModel _userdetailsObj = _userObj.AddNewUser(_user.userModels);
                if (_userdetailsObj != null && _userdetailsObj.ErrorMessages != null && _userdetailsObj.ErrorMessages.Count > 0)
                {
                    return Json(_userdetailsObj.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    objectId = _userdetailsObj.UserInfoId;
                    return Json(new { Result = JsonReturnEnum.success.ToString(), Command = Command, UserInfoId = _userdetailsObj.UserInfoId, SecurityProfileName = _userdetailsObj.SecurityProfileName }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }

        public PartialViewResult UserManagementDetails(long UserId = 0)
        {
            UsermanagmentWrapper _userWraperObj = new UsermanagmentWrapper(userData);
            UserManagementVM userdetVM = new UserManagementVM();
            UserModel _userModel = new UserModel();
            PasswordSettingsModel passwordSettingsModel = new PasswordSettingsModel();
            UserChangeAccessModel _userChangeAccessModel = new UserChangeAccessModel();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            var userdet = _userWraperObj.GetPersonnelAndUserdetailsByUserInfoID(UserId);
            if (userdet != null)
            {
                _userModel.UserInfoId = userdet.UserInfoId;
                _userModel.SiteName = userdet.SiteName;
                _userModel.UserName = userdet.UserName;
                _userModel.FirstName = userdet.FirstName;
                _userModel.MiddleName = userdet.MiddleName;
                _userModel.LastName = userdet.LastName;
                _userModel.CraftId = userdet.Personnel.CraftId;
                _userModel.Shift = userdet.Personnel.Shift;
                _userModel.SecurityQuestion = userdet.SecurityQuestion;
                _userModel.SecurityResponse = userdet.SecurityResponse;
                _userModel.Email = userdet.Email;
                _userModel.SecurityProfileId = userdet.SecurityProfileId;
                _userModel.SecurityProfileName = userdet.SecurityProfileName;
                _userModel.IsActive = userdet.IsActive;
                _userModel.UserType = userdet.UserType;

                if (userdet.UserType == UserTypeConstants.Admin)
                {
                    _userModel.IsSuperUser = true;//userdet.IsSuperUser;                  
                }
                else
                {
                    _userModel.IsSuperUser = false;
                }
                _userModel.Buyer = userdet.Personnel.Buyer;
                _userModel.Phone1 = userdet.Personnel.Phone1;
                _userModel.Phone2 = userdet.Personnel.Phone2;
                _userModel.Address1 = userdet.Personnel.Address1;
                _userModel.Address2 = userdet.Personnel.Address2;
                _userModel.Address3 = userdet.Personnel.Address3;
                _userModel.AddressCity = userdet.Personnel.AddressCity;
                _userModel.AddressState = userdet.Personnel.AddressState;
                _userModel.AddressPostCode = userdet.Personnel.AddressPostCode;
                _userModel.AddressCountry = userdet.Personnel.AddressCountry;
                _userModel.PersonnelId = userdet.Personnel.PersonnelId;
                _userModel.ClientLookupId = userdet.Personnel.ClientLookupId;
                _userModel.FailedAttempts = userdet.FailedAttempts;               
                _userModel._userdata = this.userData;
                //V2 - 629
                _userModel.DefaultSiteId = userdet.DefaultSiteId;
                _userModel.DefaultPersonnelId = userdet.Personnel.PersonnelId;
                _userModel.SiteControlled = userdet.SiteControlled;
                //V2-803
                _userModel.GMailId=userdet.GmailId;
                _userModel.MicrosoftMailId=userdet.MicroSoftmailId;
                _userModel.WindowsADUserId = userdet.WindowsADUserId;
                _userModel.EmployeeId = userdet.EmployeeId;//v2-877
                var AllLookUps = commonWrapper.GetAllLookUpList();
                if (AllLookUps != null)
                {
                    var Shift = AllLookUps.Where(x => x.ListName == LookupListConstants.Shift).ToList();
                    if (Shift != null && Shift.Any(cus => cus.ListValue == userdet.Personnel.Shift))
                    {
                        _userModel.Shift = Shift.Where(x => x.ListValue == userdet.Personnel.Shift).Select(x => x.Description).First();
                    }
                }
                var craftDetails = GetLookUpList_Craft();
                if (craftDetails != null)
                {
                    var tempCraft = craftDetails.Where(x => x.CraftId == userdet.Personnel.CraftId).ToList();
                    if (tempCraft.Count > 0)
                    {
                        _userModel.CraftName = craftDetails.Where(x => x.CraftId == userdet.Personnel.CraftId).ToList().Select(x => x.Description).First();
                    }
                }
                LocalizeControls(userdetVM, LocalizeResourceSetConstants.UserDetails);
            }
            _userModel.PackageLevel = this.userData.DatabaseKey.Client.PackageLevel;
            passwordSettingsModel = _userWraperObj.PasswordSettingsDetails();
            userdetVM.passwordSettingsModel = passwordSettingsModel;
            userdetVM.userModels = _userModel;
            userdetVM.resetPasswordModel = new ResetPasswordModel();
            return PartialView("~/Views/Configuration/UserManagement/UserManagementDetails.cshtml", userdetVM);
        }

        [HttpGet]
        public ActionResult UserEdit(long UserInfoId)
        {
            UserManagementVM userdetVM = new UserManagementVM();
            UsermanagmentWrapper _userObj = new UsermanagmentWrapper(userData);
            var userdet = _userObj.GetPersonnelAndUserdetailsByUserInfoID(UserInfoId);
            UserModel _userModel = new UserModel();            
            if (userdet != null)
            {
                _userModel.UserInfoId = userdet.UserInfoId;
                _userModel.SiteName = userdet.SiteName;
                _userModel.UserName = userdet.UserName;
                _userModel.FirstName = userdet.FirstName;
                _userModel.MiddleName = userdet.MiddleName;
                _userModel.LastName = userdet.LastName;
                _userModel.CraftId = userdet.Personnel.CraftId;
                _userModel.Shift = userdet.Personnel.Shift;
                _userModel.SecurityQuestion = userdet.SecurityQuestion;
                _userModel.SecurityResponse = userdet.SecurityResponse;
                _userModel.Email = userdet.Email;
                _userModel.SecurityProfileId = userdet.SecurityProfileId;
                _userModel.SecurityProfileName = userdet.SecurityProfileName;
                _userModel.IsActive = userdet.IsActive;
                _userModel.UserType = userdet.UserType;
                _userModel.IsSuperUser = userdet.IsSuperUser;
                _userModel.Password = userdet.Password;
                _userModel.Buyer = userdet.Personnel.Buyer;
                _userModel.Phone1 = userdet.Personnel.Phone1;
                _userModel.Phone2 = userdet.Personnel.Phone2;
                _userModel.Address1 = userdet.Personnel.Address1;
                _userModel.Address2 = userdet.Personnel.Address2;
                _userModel.Address3 = userdet.Personnel.Address3;
                _userModel.AddressCity = userdet.Personnel.AddressCity;
                _userModel.AddressState = userdet.Personnel.AddressState;
                _userModel.AddressPostCode = userdet.Personnel.AddressPostCode;
                _userModel.AddressCountry = userdet.Personnel.AddressCountry;
                _userModel.PersonnelId = userdet.Personnel.PersonnelId;
                _userModel.ClientLookupId = userdet.Personnel.ClientLookupId;
                //V2-803
                _userModel.LoginSSOId = userdet.LoginSSOId;
                _userModel.GMailId = userdet.GmailId;
                _userModel.MicrosoftMailId= userdet.MicroSoftmailId;
                _userModel.WindowsADUserId = userdet.WindowsADUserId;
                _userModel._userdata = this.userData;
                _userModel.EmployeeId= userdet.EmployeeId; //V2-877
            }
            _userModel.SecurityBBUUser = _userObj.IsbbuUsers(userdet.UserName);
            _userModel = _userObj.PopulateDropdownControls(userdetVM, _userModel);
            _userModel.PackageLevel = this.userData.DatabaseKey.Client.PackageLevel;
            userdetVM.userModels = _userModel;
            LocalizeControls(userdetVM, LocalizeResourceSetConstants.UserDetails);
            return PartialView("~/Views/Configuration/UserManagement/_UserEditNew.cshtml", userdetVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UserEdit(UserManagementVM userdetVM)
        {
            List<string> errorMessages = new List<string>();
            if (ModelState.IsValid)
            {
                UsermanagmentWrapper _userObj = new UsermanagmentWrapper(userData);
                if (userdetVM.userModels.UserType == UserTypeConstants.Full)
                {
                    if (userdetVM.userModels.SecurityProfileId == 0 || userdetVM.userModels.SecurityProfileId == 9)
                    {
                        string ModelValidationFailedMessage = string.Empty;
                        ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalSecurityProfileValidationMsg", LocalizeResourceSetConstants.Global);
                        return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
                    }
                }
                //else if (userdetVM.userModels.UserType.ToLower() == UserTypeConstants.WorkRequest.ToLower())   /*not required for V2-332*/
                //{
                //    userdetVM.userModels.SecurityProfileId = 9;
                //}
                //else 
                if (userdetVM.userModels.UserType.ToLower() == UserTypeConstants.Reference.ToLower() && userdetVM.userModels.SecurityProfileId != 0)
                {
                    userdetVM.userModels.SecurityProfileId = 0;
                }
                UserModel _userdetailsObj = _userObj.EditUser(userdetVM.userModels, userdetVM.userModels.UserInfoId);
                if (_userdetailsObj.ErrorMessages != null && _userdetailsObj.ErrorMessages.Count > 0)
                {
                    return Json(_userdetailsObj.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), UserInfoId = _userdetailsObj.UserInfoId }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                string ModelValidationFailedMessage = string.Empty;
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult GetUserDetailsForResetPassWord(long UserInfoId)
        {
            UsermanagmentWrapper uWrapper = new UsermanagmentWrapper(userData);
            var userDetails = uWrapper.GetUserDetails(UserInfoId);
            return Json(userDetails, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult CreateTempPassword(UserManagementVM objUserManagementVM)
        {
            UsermanagmentWrapper uWrapper = new UsermanagmentWrapper(userData);
            var result = uWrapper.ResetPasswordForExistingUser(objUserManagementVM.resetPasswordModel);
            if (result == true)
            {
                return Json(new { result = "success", message = "Password has been reset successfully" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { result = "failed", message = "Failed to reset password " }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region V2-417 Inactivate and Active Users
        [HttpPost]
        public JsonResult ValidateUserStatusChange(long _userInfoid, bool inactiveFlag)
        {
            UsermanagmentWrapper uWrapper = new UsermanagmentWrapper(userData);
            UserDetails userdtls = new UserDetails();
            string flag = ActivationStatusConstant.Activate;
            if (inactiveFlag)
            {
                flag = ActivationStatusConstant.InActivate;
            }
            userdtls = uWrapper.ValidateUserStatusChange(_userInfoid, flag);
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
        public JsonResult UpdateUserStatus(long _userInfoid, bool inactiveFlag)
        {
            UserDetails userdtls = new UserDetails();
            UsermanagmentWrapper uWrapper = new UsermanagmentWrapper(userData);

            UserModel objuser = new UserModel();
            objuser._userdata = this.userData;
            bool CMMSUser = objuser.CMMSUser;


            var errMsg = uWrapper.UpdateUserActiveStatus(_userInfoid, inactiveFlag);
            if (errMsg != null && errMsg.Count > 0)
            {
                return Json(userdtls.ErrorMessages, JsonRequestBehavior.AllowGet);
            }
            else
            {
                errMsg = uWrapper.CreateUserEvent(_userInfoid, inactiveFlag);
                if (errMsg != null && errMsg.Count > 0)
                {
                    return Json(userdtls.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { result = JsonReturnEnum.success.ToString(), userInfoId = _userInfoid }, JsonRequestBehavior.AllowGet);
                }
            }

        }


        #endregion

        #region V2-419 Enterprise User Management - Add/Remove Sites
        [HttpPost]
        public string PopulatePersonnelSites(int? draw, int? start, int? length, long userInfoId)
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            UsermanagmentWrapper uWrapper = new UsermanagmentWrapper(userData);
            var PersonnelSiteList = uWrapper.PopulatePersonnelSites(userInfoId);
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
        public JsonResult ValidateAddSite(long _userInfoid, long _siteid, bool _siteControlled, string _userType)
        {
            UsermanagmentWrapper uWrapper = new UsermanagmentWrapper(userData);
            UserDetails userdtls = new UserDetails();
            userdtls = uWrapper.ValidateAddSite(_userInfoid, _siteid, _siteControlled, _userType);
            if (userdtls.ErrorMessages != null && userdtls.ErrorMessages.Count > 0)
            {
                return Json(userdtls.ErrorMessages, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { validationStatus = true, userInfoId = userdtls.UserInfoId }, JsonRequestBehavior.AllowGet);
            }
        }
        public PartialViewResult AddUmSite(long userInfoId, string userName, string userFirstName, string userMiddleName, string userLastName, string userType, bool isSuperUser, bool siteControlled)
        {
            UserManagementVM userdetVM = new UserManagementVM();
            UserSiteModel usrpersonnel = new UserSiteModel();
            usrpersonnel.UserInfoId = userInfoId;
            usrpersonnel.ClientLookupId = userName;
            usrpersonnel.FirstName = userFirstName;
            usrpersonnel.MiddleName = userMiddleName;
            usrpersonnel.LastName = userLastName;
            usrpersonnel.UserType = userType;
            usrpersonnel.IsSuperUser = isSuperUser;
            usrpersonnel.SiteControlled = siteControlled;
            ActiveSiteWrapper objActiveSiteWrapper = new ActiveSiteWrapper(userData);
            var SiteList = objActiveSiteWrapper.GetAllAssignedSites(userInfoId);
            if (SiteList != null)
            {
                usrpersonnel.SiteList = SiteList;
            }

            var craftDetails = GetLookUpList_Craft();
            if (craftDetails != null)
            {
                usrpersonnel.LookupCraftList = craftDetails.Select(x => new SelectListItem { Text = x.Description, Value = x.CraftId.ToString() });
            }
            //UserType WorkRequest or not
            if (userType == "WorkRequest")
            {
                usrpersonnel.IsUserTypeWorkRequest = true;
            }
            userdetVM.userSiteModel = usrpersonnel;
            LocalizeControls(userdetVM, LocalizeResourceSetConstants.UserDetails);
            return PartialView("~/Views/Configuration/UserManagement/_AddEditUMSite.cshtml", userdetVM);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveUmSites(UserManagementVM objPersonnel)
        {
            string Mode = string.Empty;
            if (ModelState.IsValid)
            {
                var userInfoId = objPersonnel.userSiteModel.UserInfoId;
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
                    return Json(new { Result = JsonReturnEnum.success.ToString(), userInfoId = userInfoId, mode = Mode }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                string ModelValidationFailedMessage = string.Empty;
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }


        public JsonResult ValidateRemoveSite(long _userInfoid, long _siteId, bool _siteControlled, string _userType)
        {
            UsermanagmentWrapper uWrapper = new UsermanagmentWrapper(userData);
            UserDetails userdtls = new UserDetails();
            userdtls = uWrapper.ValidateRemoveSite(_userInfoid, _siteId, _siteControlled, _userType);
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
        public ActionResult DeleteUmSites(long personnelId, long userInfoId, long siteId, string userType, bool isSuperUser, long defaultSiteId)
        {
            UsermanagmentWrapper uWrapper = new UsermanagmentWrapper(userData);
            var deleteResult = uWrapper.UmPersonnelDelete(personnelId, userInfoId, siteId, userType, isSuperUser, defaultSiteId);
            if (deleteResult)
            {
                return Json(new { Result = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Result = JsonReturnEnum.failed.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetDefaultBuyerCounts(long userInfoId, long siteId)
        {
            ActiveSiteWrapper aWrapper = new ActiveSiteWrapper(userData);
            var openJobsCount = aWrapper.GetCount(userInfoId, siteId);
            return Json(openJobsCount, JsonRequestBehavior.AllowGet);
        }
        #endregion


        #region ChangeUserAccess

        public ActionResult ChangeUserAccess(long UserInfoId)
        {
            UserManagementVM userdetVM = new UserManagementVM();
            UsermanagmentWrapper uWrapper = new UsermanagmentWrapper(userData);
            UserChangeAccessModel _userChangeAccessModel = new UserChangeAccessModel();

            var userdet = uWrapper.GetPersonnelAndUserdetailsByUserInfoID(UserInfoId);

            if (userdet.UserType.ToUpper() != UserTypeConstants.Reference.ToUpper())
            {
                _userChangeAccessModel.UserInfoId = userdet.UserInfoId;
                _userChangeAccessModel.SecurityProfileId = userdet.SecurityProfileId;
                _userChangeAccessModel.UserType = userdet.UserType;
                _userChangeAccessModel.OldUserType = userdet.UserType;                
                _userChangeAccessModel.UserUpdateIndex = userdet.UserUpdateIndex;
                if (userdet.SecurityProfileId == 0)
                {
                    _userChangeAccessModel.SecurityProfileName = string.Empty;
                }
                else
                {
                    _userChangeAccessModel.SecurityProfileName = userdet.SecurityProfileName;
                }
                _userChangeAccessModel._userdata = this.userData;
                _userChangeAccessModel.PackageLevel = this.userData.DatabaseKey.Client.PackageLevel;
                _userChangeAccessModel.DefaultSiteId = userdet.DefaultSiteId;
                _userChangeAccessModel.APM = userdet.APM;
                _userChangeAccessModel.CMMS = userdet.CMMS;
                _userChangeAccessModel.Sanitation = userdet.Sanitation;
                _userChangeAccessModel.Fleet = userdet.Fleet;
                _userChangeAccessModel.Production = this.userData.Site.Production;
                _userChangeAccessModel.ProductGrouping = userdet.ProductGrouping;
                userdetVM.userChangeAccessModel = _userChangeAccessModel;
            }

            LocalizeControls(userdetVM, LocalizeResourceSetConstants.UserDetails);
            return PartialView("~/Views/Configuration/UserManagement/_ChangeUserAccessModal.cshtml", userdetVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult ChangeUserAccess(UserManagementVM userVM)
        {
            UsermanagmentWrapper uWrapper = new UsermanagmentWrapper(userData);
            List<string> ErrorList = new List<string>();
            string ModelValidationFailedMessage = string.Empty;
            if (ModelState.IsValid)
            {
                UserChangeAccessModel _userdetailsObj = uWrapper.ChangeUserAccess(userVM.userChangeAccessModel);
                if (_userdetailsObj != null && _userdetailsObj.ErrorMessages != null && _userdetailsObj.ErrorMessages.Count > 0)
                {
                    return Json(_userdetailsObj.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), userInfoId = userVM.userChangeAccessModel.UserInfoId }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region V2-491 Unlock Account
        [HttpPost]
        public JsonResult UnlockAccount(long _userInfoid,long Siteid,long PersonnelId=0)
        {
            UserDetails userdtls = new UserDetails();
            UsermanagmentWrapper uWrapper = new UsermanagmentWrapper(userData);

            var errMsg = uWrapper.UnlockAccount(_userInfoid, Siteid, PersonnelId);
            if (errMsg != null && errMsg.Count>0)
            {
                return Json(userdtls.ErrorMessages, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { result = JsonReturnEnum.success.ToString(), userInfoId = _userInfoid }, JsonRequestBehavior.AllowGet);
            }

        }

        #region Reset password
        
        public ActionResult ManualResetPassWord(long UserInfoId)
        {
            UserManagementVM userdetVM = new UserManagementVM();
            UsermanagmentWrapper uWrapper = new UsermanagmentWrapper(userData);
            userdetVM.manualResetPasswordModel = new ManualResetPasswordModel();
            userdetVM.manualResetPasswordModel = uWrapper.GetUserDetailsManual(UserInfoId);

            //-------V2 - 491
            var passwordSettings = uWrapper.PasswordSettingsDetails();
            if (passwordSettings != null)
            {
                userdetVM.manualResetPasswordModel.PWReqMinLength = passwordSettings.PWReqMinLength;
                userdetVM.manualResetPasswordModel.PWMinLength = passwordSettings.PWMinLength;
                userdetVM.manualResetPasswordModel.PWRequireNumber = passwordSettings.PWRequireNumber;
                userdetVM.manualResetPasswordModel.PWRequireAlpha = passwordSettings.PWRequireAlpha;
                userdetVM.manualResetPasswordModel.PWRequireMixedCase = passwordSettings.PWRequireMixedCase;
                userdetVM.manualResetPasswordModel.PWRequireSpecialChar = passwordSettings.PWRequireSpecialChar;
                userdetVM.manualResetPasswordModel.PWNoRepeatChar = passwordSettings.PWNoRepeatChar;
                userdetVM.manualResetPasswordModel.PWNotEqualUserName = passwordSettings.PWNotEqualUserName;
            }
            //------- V2-491

            LocalizeControls(userdetVM, LocalizeResourceSetConstants.UserDetails);
            return PartialView("~/Views/Configuration/UserManagement/_ManualResetPasswordModal.cshtml", userdetVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult ManualResetPassWord(UserManagementVM UmVm)
        {
            if (ModelState.IsValid)
            {
                UserDetails userdtls = new UserDetails();
                UsermanagmentWrapper uWrapper = new UsermanagmentWrapper(userData);
                var result = uWrapper.ManualResetPassword(UmVm.manualResetPasswordModel);
                if (result == false)
                {
                    return Json(userdtls.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { result = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
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
        #endregion

        #region V2-629 - Change UserName

        public ActionResult ChangeUserName(long UserInfoId)
        {
            UserManagementVM userdetVM = new UserManagementVM();
            UsermanagmentWrapper uWrapper = new UsermanagmentWrapper(userData);
            userdetVM.userNameChangeModel = new UserNameChangeModel();
          
            //-------V2 - 491
            var passwordSettings = uWrapper.PasswordSettingsDetails();
            if (passwordSettings != null)
            {
                userdetVM.userNameChangeModel.PWReqMinLength = passwordSettings.PWReqMinLength;
                userdetVM.userNameChangeModel.PWMinLength = passwordSettings.PWMinLength;
                userdetVM.userNameChangeModel.PWRequireNumber = passwordSettings.PWRequireNumber;
                userdetVM.userNameChangeModel.PWRequireAlpha = passwordSettings.PWRequireAlpha;
                userdetVM.userNameChangeModel.PWRequireMixedCase = passwordSettings.PWRequireMixedCase;
                userdetVM.userNameChangeModel.PWRequireSpecialChar = passwordSettings.PWRequireSpecialChar;
                userdetVM.userNameChangeModel.PWNoRepeatChar = passwordSettings.PWNoRepeatChar;
                userdetVM.userNameChangeModel.PWNotEqualUserName = passwordSettings.PWNotEqualUserName;
            }
            //------- V2-491

            LocalizeControls(userdetVM, LocalizeResourceSetConstants.UserDetails);
            return PartialView("~/Views/Configuration/UserManagement/_ChangeUserNameModal.cshtml", userdetVM);
        }
        [HttpPost]
        public ActionResult CheckIfUserExistForUserNameChange(UserManagementVM userManagementVM)
        {
            return Json(!IfUserExist(userManagementVM.userNameChangeModel.UserName), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult ChangeUserName(UserManagementVM userVM)
        {
            UsermanagmentWrapper uWrapper = new UsermanagmentWrapper(userData);
            string ModelValidationFailedMessage = string.Empty;
            if (ModelState.IsValid)
            {
                UserNameChangeModel _userNameChangeObj = uWrapper.ChangeUserName(userVM.userNameChangeModel);
                if (_userNameChangeObj != null && _userNameChangeObj.ErrorMessages != null && _userNameChangeObj.ErrorMessages.Count > 0)
                {
                    return Json(_userNameChangeObj.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), userInfoId = userVM.userNameChangeModel.UserInfoId }, JsonRequestBehavior.AllowGet);
                }
                // Change User Name code will go here

            }
            else
            {
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Common
        private bool IfUserExist(string userName)
        {
            UsermanagmentWrapper _userObj = new UsermanagmentWrapper(userData);
            int count = _userObj.CountIfUserExist(userName);
            bool IfUserExist;
            if (count > 0)
            {
                IfUserExist = true;
            }
            else
            {
                IfUserExist = false;
            }
            return IfUserExist;
        }
        #endregion

        #region V2-680 User Management - Add/Remove Storerooms
        [HttpPost]
        public string PopulatePersonnelStorerooms(int? draw, int? start, int? length, long personnelId)
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            start = start.HasValue
                ? start / length
                : 0;
            int skip = start * length ?? 0;
            UsermanagmentWrapper uWrapper = new UsermanagmentWrapper(userData);
            var PersonnelStoreroomList = uWrapper.PopulatePersonnelStorerooms(personnelId, order, length ?? 0, orderDir, skip);
            var totalRecords = 0;
            var recordsFiltered = 0;
            if (PersonnelStoreroomList != null && PersonnelStoreroomList.Count > 0)
            {
                recordsFiltered = PersonnelStoreroomList[0].TotalCount;
                totalRecords = PersonnelStoreroomList[0].TotalCount;
            }
            
            //int initialPage = start.Value;
            var filteredResult = PersonnelStoreroomList.ToList();
               //.Skip(initialPage * length ?? 0)
               //.Take(length ?? 0)
               //.ToList();

            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult }, JsonSerializer12HoursDateAndTimeSettings);
        }

        public PartialViewResult AddUmStoreroom(long userInfoId, string userName)
        {
            UserManagementVM userdetVM = new UserManagementVM();
            UserStoreroomModel usrpersonnel = new UserStoreroomModel();
            usrpersonnel.UserInfoId = userInfoId;
            usrpersonnel.ClientLookupId = userName;
            UsermanagmentWrapper uWrapper = new UsermanagmentWrapper(userData);
            var SiteList = uWrapper.GetAllSelectedUserSites(userInfoId);
            if (SiteList != null)
            {
                usrpersonnel.SiteList = SiteList;
            }          
            userdetVM.userStoreroomModel = usrpersonnel;
            LocalizeControls(userdetVM, LocalizeResourceSetConstants.UserDetails);
            return PartialView("~/Views/Configuration/UserManagement/_AddEditUMStoreroom.cshtml", userdetVM);
        }

        [HttpPost]
        public JsonResult RetrieveAllStoreroomBySiteId(long SiteId)
        {
            UsermanagmentWrapper uWrapper = new UsermanagmentWrapper(userData);
            var StoreroomList = uWrapper.RetrieveAllStoreroomBySiteId(SiteId);
            if (StoreroomList != null)
            {
                return Json(new { Result = true, StoreroomList }, JsonRequestBehavior.AllowGet);
            }
            string ModelValidationFailedMessage = string.Empty;
            return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveUmStorerooms(UserManagementVM objPersonnel)
        {
            string Mode = string.Empty;
            if (ModelState.IsValid)
            {
                var userInfoId = objPersonnel.userStoreroomModel.UserInfoId;
                UsermanagmentWrapper uWrapper = new UsermanagmentWrapper(userData);
                List<string> errorMessages = new List<string>();
                if (objPersonnel.userStoreroomModel.StoreroomAuthId == 0)
                {
                    Mode = "add";
                    errorMessages = uWrapper.AddUmStorerooms(objPersonnel.userStoreroomModel);
                }
                else
                {
                    Mode = "edit";
                    errorMessages = uWrapper.EditUmStorerooms(objPersonnel.userStoreroomModel);
                }
                if (errorMessages != null && errorMessages.Count > 0)
                {
                    return Json(errorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), userInfoId = userInfoId, mode = Mode }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                string ModelValidationFailedMessage = string.Empty;
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public PartialViewResult EditUMStorerooms(long userInfoId, long storeroomAuthId, string clientLookupId)
        {
            UserManagementVM userdetVM = new UserManagementVM();
            UserStoreroomModel userStoreroomModel = new UserStoreroomModel();
            UsermanagmentWrapper uWrapper = new UsermanagmentWrapper(userData);
            userStoreroomModel = uWrapper.RetrieveStoreroomsForEdit(storeroomAuthId);           
            userStoreroomModel.ClientLookupId = clientLookupId;
            userStoreroomModel.UserInfoId = userInfoId;            
            userdetVM.userStoreroomModel = userStoreroomModel;
            LocalizeControls(userdetVM, LocalizeResourceSetConstants.UserDetails);
            return PartialView("~/Views/Configuration/UserManagement/_AddEditUMStoreroom.cshtml", userdetVM);
        }

        [HttpPost]
        public ActionResult DeleteUmStorerooms(long storeroomAuthId)
        {
            UsermanagmentWrapper uWrapper = new UsermanagmentWrapper(userData);
            var deleteResult = uWrapper.UmStoreroomDelete(storeroomAuthId);
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

        #region  V2-547 Add Reference User
        [HttpGet]
        public ActionResult AddReferenceUser()
        {
            ActiveSiteWrapper objActiveSiteWrapper = new ActiveSiteWrapper(userData);
            ReferenceUserModel objuser = new ReferenceUserModel();
            UserManagementVM objuserVM = new UserManagementVM();
            var craftDetails = GetLookUpList_Craft();
            if (craftDetails != null)
            {
                objuser.LookupCraftList = craftDetails.Select(x => new SelectListItem { Text = x.ClientLookUpId + " - " + x.Description + " - " + x.ChargeRate, Value = x.CraftId.ToString() });
            }
            objuserVM.referenceUserModel = objuser;
            LocalizeControls(objuserVM, LocalizeResourceSetConstants.UserDetails);
            return PartialView("~/Views/Configuration/UserManagement/_AddReferenceUserModal.cshtml", objuserVM);
        }
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddReferenceUser(UserManagementVM userVM)
        {
            UsermanagmentWrapper uWrapper = new UsermanagmentWrapper(userData);
            string ModelValidationFailedMessage = string.Empty;
            if (ModelState.IsValid)
            {
                ReferenceUserModel referenceUserModel = uWrapper.AddReferenceUser(userVM.referenceUserModel);
                if (referenceUserModel != null && referenceUserModel.ErrorMessages != null && referenceUserModel.ErrorMessages.Count > 0)
                {
                    return Json(referenceUserModel.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), userInfoId = userVM.referenceUserModel.UserInfoId }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult CheckIfUserExistForReferenceUserAdd(UserManagementVM userManagementVM)
        {
            return Json(!IfUserExist(userManagementVM.referenceUserModel.UserName), JsonRequestBehavior.AllowGet);
        }
        #endregion
    }


}
