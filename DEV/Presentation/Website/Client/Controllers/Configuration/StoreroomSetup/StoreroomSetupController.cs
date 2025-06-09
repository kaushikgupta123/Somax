using Client.ActionFilters;
using Client.BusinessWrapper.Configuration;
using Client.BusinessWrapper.Configuration.DashBoard;
using Client.Models.Configuration.Dashboard;
using Client.Common;
using Client.Controllers.Common;
using Client.Models.Configuration.StoreroomSetup;
using Common.Constants;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Client.Controllers.Configuration.StoreroomSetup
{
    public class StoreroomSetupController : ConfigBaseController
    {

        #region Search
        [CheckUserSecurity(securityType = SecurityConstants.Storeroom)]
        public ActionResult Index()
        {
            ConfigDashboardWrapper wrapper = new ConfigDashboardWrapper(userData);
            StoreroomSetupVM sVM = new StoreroomSetupVM();
            var sites = wrapper.GetSites();
            sVM.SiteList = sites;
            var StatusList = UtilityFunction.InactiveActiveStatusTypes();
            if (StatusList != null)
            {
                sVM.InactiveFlagList = StatusList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
            }
            LocalizeControls(sVM, LocalizeResourceSetConstants.StoreroomDetails);
            return View("~/Views/Configuration/StoreroomSetup/index.cshtml", sVM);
        }

        public string GetGridDataforStoreroomSetup(int? draw, int? start, int? length = 0, string Name = "", string Description = "", long SiteId = 0, string SearchText = "", int Case = 0)
        {
            List<StoreroomModel> mStoreroomModelList = new List<StoreroomModel>();
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            start = start.HasValue
                ? start / length
                : 0;
            int skip = start * length ?? 0;
            StoreroomWrapper sWrapper = new StoreroomWrapper(userData);
            mStoreroomModelList = sWrapper.GetStoreroomSetupList(order, length ?? 0, orderDir, skip, Name, Description, SiteId, SearchText, Case);
            var totalRecords = 0;
            var recordsFiltered = 0;
            if (mStoreroomModelList != null && mStoreroomModelList.Count>0)
            {
                recordsFiltered = mStoreroomModelList[0].totalCount;
                totalRecords = mStoreroomModelList[0].totalCount;
            }

            var filteredResult = mStoreroomModelList
                .ToList();
            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult }, JsonSerializer12HoursDateAndTimeSettings);
        }

        #endregion

        #region Details Storeroom
        public PartialViewResult StoreroomDetail(long StoreroomId = 0)
        {
            StoreroomSetupVM objVM = new StoreroomSetupVM();
            StoreroomModel obj = new StoreroomModel();
            StoreroomWrapper srWrapper = new StoreroomWrapper(userData);
            obj = srWrapper.StoreroomDetails(StoreroomId);
            objVM.storeroomModel = obj;
            LocalizeControls(objVM, LocalizeResourceSetConstants.StoreroomDetails);


            return PartialView("~/Views/Configuration/StoreroomSetup/_StoreroomDetail.cshtml", objVM);
        }



        #endregion

        #region AddOrEdit
        public ActionResult AddOrEditStoreroom(long? StoreroomId, bool IsAdd)
        {
            StoreroomWrapper srWrapper = new StoreroomWrapper(userData);
            StoreroomSetupVM objVM = new StoreroomSetupVM();
            StoreroomModel obj = new StoreroomModel();
            ConfigDashboardWrapper wrapper = new ConfigDashboardWrapper(userData);
            if (!IsAdd)
            {
                obj = srWrapper.StoreroomDetails(StoreroomId ?? 0);
            }
            else
            {
                var sites = wrapper.GetSites();
                obj.SiteList = sites;

                obj.StoreroomId = StoreroomId ?? 0;
                obj.IsAdd = true;
            }

            objVM.storeroomModel = obj;
            LocalizeControls(objVM, LocalizeResourceSetConstants.StoreroomDetails);
            return PartialView("~/Views/Configuration/StoreroomSetup/_StoreroomAddEdit.cshtml", objVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveStoreroom(StoreroomSetupVM objVM, string Command)
        {
            if (ModelState.IsValid)
            {
                List<String> errorList = new List<string>();
                StoreroomWrapper srWrapper = new StoreroomWrapper(userData);
                string Mode = string.Empty;
                long StoreroomID = 0;
                if (!objVM.storeroomModel.IsAdd)
                {
                    Mode = "update";
                    errorList = srWrapper.UpdateStoreroom(objVM.storeroomModel, ref StoreroomID);
                }
                else
                {
                    Mode = "add";
                    errorList = srWrapper.AddStoreroom(objVM.storeroomModel, ref StoreroomID);

                }
                if (errorList != null && errorList.Count > 0)
                {
                    return Json(errorList, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), StoreroomID = StoreroomID, mode = Mode, Command = Command }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                string ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region print
        public string GetStoreroomPrintData(string colname, string coldir, long Siteid, string Name, string Description, string SearchText = "", int CustomQueryDisplayId = 0)
        {

            List<StoreroomSetupModelPrint> StoreroomPrintModelList = new List<StoreroomSetupModelPrint>();
            StoreroomSetupModelPrint objStoreroomPrintModel;
            StoreroomWrapper strWrapper = new StoreroomWrapper(userData);
            var Case = CustomQueryDisplayId;
            var storeroomList = strWrapper.GetStoreroomSetupList(colname, 100000, coldir, 0, Name, Description, Siteid, SearchText, Case);
            if (storeroomList != null && storeroomList.Count > 0)
            {
                foreach (var p in storeroomList)
                {
                    objStoreroomPrintModel = new StoreroomSetupModelPrint();
                    objStoreroomPrintModel.Name = p.Name;
                    objStoreroomPrintModel.Description = p.Description;
                    objStoreroomPrintModel.SiteName = p.SiteName;
                    objStoreroomPrintModel.InactiveFlag = p.InactiveFlag;
                    StoreroomPrintModelList.Add(objStoreroomPrintModel);
                }
            }

            return JsonConvert.SerializeObject(new { data = StoreroomPrintModelList }, JsonSerializerDateSettings);
        }
        #endregion
    }
}