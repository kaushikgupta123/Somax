using Client.BusinessWrapper.ActiveSiteWrapper;
using Client.BusinessWrapper.Configuration.NotificationSetUp;
using Client.Common;
using Client.Controllers.Common;
using Client.Models.Configuration.NotificationSetUp;
using Common.Constants;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace Client.Controllers.Configuration.NotificationSetUp
{
    public class NotificationSetUpController : ConfigBaseController
    {
        public ActionResult Index()
        {
            NotificationSetUpWrapper nWrapper = new NotificationSetUpWrapper(userData);
            ActiveSiteWrapper sWrappper = new ActiveSiteWrapper(userData);
            var getAllsites = sWrappper.GetSites();
            string PackageLevelDef = string.Empty;
            bool IsSuperUser = false;
            if (userData.DatabaseKey.Client.PackageLevel.ToUpper() == PackageLevelConstant.Enterprise)
            {
                PackageLevelDef = PackageLevelConstant.Enterprise;
            }
            else if (userData.DatabaseKey.Client.PackageLevel.ToUpper() == PackageLevelConstant.Basic)
            {
                PackageLevelDef = PackageLevelConstant.Basic;
            }
            else if (userData.DatabaseKey.Client.PackageLevel.ToUpper() == PackageLevelConstant.Professional)
            {
                PackageLevelDef = PackageLevelConstant.Professional;
            }

            if (userData.DatabaseKey.User.IsSuperUser)
            {
                IsSuperUser = true;
            }
            #region BindSetUpList
            DataTable dtSetUptable = new DataTable();
            if (!(userData.DatabaseKey.User.IsSuperUser && userData.DatabaseKey.Client.PackageLevel.ToUpper() == PackageLevelConstant.Enterprise))
            {
                dtSetUptable = nWrapper.AlertSetUpList(userData.DatabaseKey.User.DefaultSiteId);
            }
            var SetUpList = ConvertDataTable<AlertSetUpListUp>(dtSetUptable);
            #endregion
            long DefaultAlertSetupId;
            if (TempData["AlertSetupId"] != null)
            {
                DefaultAlertSetupId = Convert.ToInt32(TempData["AlertSetupId"].ToString());
            }
            else
            {
                DefaultAlertSetupId = SetUpList != null && SetUpList.Count > 0 ? SetUpList[0].AlertSetupId : 1;
            }
            long AlertSetupId = 0;
            int UpdateIndex = 0;
            bool IsActive = false;
            bool IsEmailSend = false;
            bool IsIncludeEmailAttachedment = false;
            var noti = nWrapper.NotificationDetails(DefaultAlertSetupId, ref AlertSetupId, ref UpdateIndex, ref IsActive, ref IsEmailSend, ref IsIncludeEmailAttachedment);
            var SiteDrpList = new SelectList(getAllsites, "Value", "Text");
            NotificationSetUpModel notificationModel = new NotificationSetUpModel()
            {
                AlertSetupId = (userData.DatabaseKey.User.IsSuperUser && userData.DatabaseKey.Client.PackageLevel.ToUpper() == PackageLevelConstant.Enterprise) ? 0 : AlertSetupId,
                AlertDefineId = noti.AlertDefineId,
                IsTargetListShow = noti.TargetList,
                UpdateIndex = UpdateIndex,
                Description = (userData.DatabaseKey.User.IsSuperUser && userData.DatabaseKey.Client.PackageLevel.ToUpper() == PackageLevelConstant.Enterprise) ? "" : noti.Description,
                IsActive = (userData.DatabaseKey.User.IsSuperUser && userData.DatabaseKey.Client.PackageLevel.ToUpper() == PackageLevelConstant.Enterprise) ? false : IsActive,
                IsEmailSend = (userData.DatabaseKey.User.IsSuperUser && userData.DatabaseKey.Client.PackageLevel.ToUpper() == PackageLevelConstant.Enterprise) ? false : IsEmailSend,
                IsShowEmailSend = (userData.DatabaseKey.User.IsSuperUser && userData.DatabaseKey.Client.PackageLevel.ToUpper() == PackageLevelConstant.Enterprise) ? false : noti.EmailSend,
                IsIncludeEmailAttachedment = IsIncludeEmailAttachedment,
                AlertSetUpListUpList = SetUpList.Select(x => new SelectListItem { Text = x.Name, Value = x.AlertSetupId.ToString() })
                ,AlertSetUpSiteList= SiteDrpList
                ,PackageLevelDef = PackageLevelDef
                ,IsSuperUser= IsSuperUser
            };
            LocalizeControls(notificationModel, LocalizeResourceSetConstants.NotificationDetails);
            return View("~/Views/Configuration/NotificationSetUp/index.cshtml", notificationModel);
        }

        #region TargetList
        [HttpPost]
        public string PopulateTargetList(int? draw, int? start, int? length, long targetId)
        {
            NotificationSetUpWrapper nWrapper = new NotificationSetUpWrapper(userData);
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];          
            var alertlist = nWrapper.populateTargetGrid(targetId);
            if(alertlist!=null && alertlist.Count > 0)
            {
                alertlist = GetAllAccountSortByColumnWithOrder(order, orderDir, alertlist);
            }
            var totalRecords = 0;
            var recordsFiltered = 0;
            start = start.HasValue
                ? start / length
                : 0;
            recordsFiltered = alertlist.Count();
            totalRecords = alertlist.Count();
            int initialPage = start.Value;
            var filteredResult = alertlist
                .Skip(initialPage * length ?? 0)
                .Take(length ?? 0)
                .ToList();
            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult }, JsonSerializer12HoursDateAndTimeSettings);
        }
        private List<AlertTargetModel> GetAllAccountSortByColumnWithOrder(string order, string orderDir, List<AlertTargetModel> data)
        {
            List<AlertTargetModel> lst = new List<AlertTargetModel>();
            switch (order)
            {
                case "0":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Personnel_ClientLookupId).ToList() : data.OrderBy(p => p.Personnel_ClientLookupId).ToList();
                    break;
                case "1":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.FirstName).ToList() : data.OrderBy(p => p.FirstName).ToList();
                    break;
                case "2":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.LastName).ToList() : data.OrderBy(p => p.LastName).ToList();
                    break;
                case "3":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.IsActive).ToList() : data.OrderBy(p => p.IsActive).ToList();
                    break;
                default:
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Personnel_ClientLookupId).ToList() : data.OrderBy(p => p.Personnel_ClientLookupId).ToList();
                    break;
            }
            return lst;
        }
        public ActionResult AddOrEditTargetAlert(long AlertSetUpId, long AlertTargetId, bool IsActive, int UpdateIndex, string personalLoookupId, string FirstName, string LastName)
        {
            string PackageLevelDef = "Other";
            bool IsSuperUser = false;
            if (userData.DatabaseKey.Client.PackageLevel.ToUpper() == PackageLevelConstant.Enterprise)
            {
                PackageLevelDef = PackageLevelConstant.Enterprise;
            }
            else if (userData.DatabaseKey.Client.PackageLevel.ToUpper() == PackageLevelConstant.Basic)
            {
                PackageLevelDef = PackageLevelConstant.Basic;
            }
            else if (userData.DatabaseKey.Client.PackageLevel.ToUpper() == PackageLevelConstant.Professional)
            {
                PackageLevelDef = PackageLevelConstant.Professional;
            }

            if (userData.DatabaseKey.User.IsSuperUser)
            {
                IsSuperUser = true;
            }
            NotificationSetUpWrapper nWrapper = new NotificationSetUpWrapper(userData);
            AlertTargetModel obj = new AlertTargetModel();
            var ClientLookupList = nWrapper.PersonalList();
            if (AlertTargetId == 0)
            {
                obj.ClientLookupList = ClientLookupList.Select(x => new SelectListItem { Text = x.ClientLookupId + "  " + x.NameFirst + "  " + x.NameLast, Value = x.PersonnelId.ToString() });
            }
            else
            {
                obj.IsActive = IsActive;
                obj.Personnel_ClientLookupId = personalLoookupId;
                obj.FirstName = FirstName;
                obj.LastName = LastName;
            }
            obj.AlertTargetId = AlertTargetId;
            obj.AlertSetupId = AlertSetUpId;
            obj.PackageLevelDef = PackageLevelDef;
            obj.IsSuperUser = IsSuperUser;
            TempData["AlertSetupId"] = AlertSetUpId;
            LocalizeControls(obj, LocalizeResourceSetConstants.NotificationDetails);
            return View("~/Views/Configuration/NotificationSetUp/_AddOrEditTarget.cshtml", obj);
        }
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult AddOrUpadteTarget(AlertTargetModel objTarget)
        {
            if (ModelState.IsValid)
            {
                NotificationSetUpWrapper nWrapper = new NotificationSetUpWrapper(userData);
                string Mode = string.Empty;
                List<String> errorList = new List<string>();
                if (objTarget.AlertTargetId > 0)
                {
                    Mode = "update";
                    errorList = nWrapper.UpadteTarget(objTarget);
                }
                else
                {
                    Mode = "add";
                    errorList = nWrapper.AddTarget(objTarget);
                }
                if (errorList != null && errorList.Count > 0)
                {
                    return Json(errorList, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    TempData["AlertSetupId"] = objTarget.AlertSetupId;
                    return Json(new { Result = JsonReturnEnum.success.ToString(), mode = Mode, DropdownId = objTarget.AlertSetupId }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                string ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult DeleteTarget(long AlertTargetId, long AlertSetupId)
        {
            NotificationSetUpWrapper nWrapper = new NotificationSetUpWrapper(userData);
            var deleteResult = nWrapper.DeleteTarget(AlertTargetId);
            if (deleteResult)
            {
                return Json(new { Result = JsonReturnEnum.success.ToString(), AlertSetupId = AlertSetupId }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Result = JsonReturnEnum.failed.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        [HttpPost]
        public ActionResult GetValueAlertDropDownChange(long DropDownId)
        {
            NotificationSetUpWrapper nWrapper = new NotificationSetUpWrapper(userData);
            long SetupId = 0;
            int UpdateIndex = 0;
            bool IsActive = false;
            bool IsEmailSend = false;
            bool IsIncludeEmailAttachedment = false;
            var noti = nWrapper.NotificationDetails(DropDownId, ref SetupId, ref UpdateIndex, ref IsActive, ref IsEmailSend, ref IsIncludeEmailAttachedment);
            if (noti.ErrorMessages == null)
            {
                return Json(new { Result = JsonReturnEnum.success.ToString(), Description = noti.Description, IsActive = IsActive, SetupId = SetupId, IsTargetListShow = noti.TargetList, UpdateIndex = UpdateIndex, IsEmailSend = IsEmailSend, IsShowMailSend = noti.EmailSend, IsIncludeEmailAttachedment = IsIncludeEmailAttachedment }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Result = JsonReturnEnum.failed.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult UpdateNotificationSetup(NotificationSetUpModel obj)
        {
            if (ModelState.IsValid)
            {
                NotificationSetUpWrapper nWrapper = new NotificationSetUpWrapper(userData);
                string Mode = string.Empty;

                List<String> errorList = new List<string>();
                errorList = nWrapper.UpdateNotificationAlertSetUp(obj);
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

        #region Configuration App UI Modifications
        [HttpPost]
        public ActionResult GetAlertNotification(long AlertSiteId)
        {
            NotificationSetUpWrapper nWrapper = new NotificationSetUpWrapper(userData);
            #region BindSetUpList
            long defaultSite = userData.DatabaseKey.User.DefaultSiteId;
            DataTable dtSetUptable = new DataTable();
            dtSetUptable = nWrapper.AlertSetUpList(AlertSiteId);
            var SetUpList = ConvertDataTable<AlertSetUpListUp>(dtSetUptable);
            #endregion
            if (SetUpList!=null && SetUpList.Count>0)
            {
                return Json(new { Result = JsonReturnEnum.success.ToString(), data= SetUpList }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Result = JsonReturnEnum.failed.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        [HttpPost]
        public PartialViewResult NotificationDetails(long SetupId)
        {
            NotificationSetUpWrapper nWrapper = new NotificationSetUpWrapper(userData);
            ActiveSiteWrapper sWrappper = new ActiveSiteWrapper(userData);
            var getAllsites = sWrappper.GetSites();
            string PackageLevelDef = "Other";
            bool IsSuperUser = true;
            if (userData.DatabaseKey.Client.PackageLevel.ToUpper() == PackageLevelConstant.Enterprise)
            {
                PackageLevelDef = PackageLevelConstant.Enterprise;
            }
            else if (userData.DatabaseKey.Client.PackageLevel.ToUpper() == PackageLevelConstant.Basic)
            {
                PackageLevelDef = PackageLevelConstant.Basic;
            }
            else if (userData.DatabaseKey.Client.PackageLevel.ToUpper() == PackageLevelConstant.Professional)
            {
                PackageLevelDef = PackageLevelConstant.Professional;
            }

            if (userData.DatabaseKey.User.IsSuperUser)
            {
                IsSuperUser = true;
            }
            #region BindSetUpList
            DataTable dtSetUptable = new DataTable();
            if (!(userData.DatabaseKey.User.IsSuperUser && userData.DatabaseKey.Client.PackageLevel.ToUpper() == PackageLevelConstant.Enterprise))
            {
                dtSetUptable = nWrapper.AlertSetUpList(userData.DatabaseKey.User.DefaultSiteId);
            }
            var SetUpList = ConvertDataTable<AlertSetUpListUp>(dtSetUptable);
            #endregion
            long AlertSetupId = 0;
            int UpdateIndex = 0;
            bool IsActive = false;
            bool IsEmailSend = false;
            bool IsIncludeEmailAttachedment = false;
            var noti = nWrapper.NotificationDetails(SetupId, ref AlertSetupId, ref UpdateIndex, ref IsActive, ref IsEmailSend, ref IsIncludeEmailAttachedment);
            var SiteDrpList = new SelectList(getAllsites, "Value", "Text");
            NotificationSetUpModel notificationModel = new NotificationSetUpModel()
            {
                AlertSetupId = SetupId,
                AlertDefineId = noti.AlertDefineId,
                IsTargetListShow = noti.TargetList,
                UpdateIndex = UpdateIndex,
                Description = (userData.DatabaseKey.User.IsSuperUser && userData.DatabaseKey.Client.PackageLevel.ToUpper() == PackageLevelConstant.Enterprise) ? "" : noti.Description,
                IsActive = (userData.DatabaseKey.User.IsSuperUser && userData.DatabaseKey.Client.PackageLevel.ToUpper() == PackageLevelConstant.Enterprise) ? false : IsActive,
                IsEmailSend = (userData.DatabaseKey.User.IsSuperUser && userData.DatabaseKey.Client.PackageLevel.ToUpper() == PackageLevelConstant.Enterprise) ? false : IsEmailSend,
                IsShowEmailSend = (userData.DatabaseKey.User.IsSuperUser && userData.DatabaseKey.Client.PackageLevel.ToUpper() == PackageLevelConstant.Enterprise) ? false : noti.EmailSend,
                IsIncludeEmailAttachedment = IsIncludeEmailAttachedment,
                AlertSetUpListUpList = SetUpList.Select(x => new SelectListItem { Text = x.Name, Value = x.AlertSetupId.ToString() }),       
                AlertSetUpSiteList = SiteDrpList,
                PackageLevelDef = PackageLevelDef,
                IsSuperUser= IsSuperUser
            };
            LocalizeControls(notificationModel, LocalizeResourceSetConstants.NotificationDetails);
            return PartialView("~/Views/Configuration/NotificationSetUp/_EditNotificationSetUp.cshtml", notificationModel);
        }
        }
}