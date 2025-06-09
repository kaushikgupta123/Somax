using Client.ActionFilters;
using Client.BusinessWrapper.Configuration.CustomSecurityProfile;
using Client.Common;
using Client.Controllers.Common;
using Client.Models.Configuration.CustomSecurityProfile;
using Common.Constants;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Client.Controllers.Configuration.CustomSecurityProfile
{
    public class CustomSecurityProfileController : ConfigBaseController
    {
        // GET: CustomSecurityProfile
        [CheckUserSecurity(securityType = SecurityConstants.CustomSecurityProfile)]
        public ActionResult Index()
        {
            CustomSecurityProfileVM customSecurityProfileVM = new CustomSecurityProfileVM();
            CustomSecurityProfileWrapper objCustomSecurityProfileWrapper = new CustomSecurityProfileWrapper(userData);
            LocalizeControls(customSecurityProfileVM, LocalizeResourceSetConstants.SecurityProfileDetails);
            return View("~/Views/Configuration/CustomSecurityProfile/index.cshtml", customSecurityProfileVM);
        }
        #region Search grid
        [HttpPost]
        public string GetCustomSecurityProfileGridData(int? draw, int? start, int? length, string SearchText = "", string Name = "", string Description = "")
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            var colname = Request.Form.GetValues("columns[" + order + "][name]");
            CustomSecurityProfileWrapper objCustomSecurityProfileWrapper = new CustomSecurityProfileWrapper(userData);
            start = start.HasValue
               ? start / length
               : 0;
            int skip = start * length ?? 0;
            var secprofileList = objCustomSecurityProfileWrapper.GetCustomSecurityProfiles(order, orderDir, skip, length, SearchText, Name, Description);
            var totalRecords = 0;
            var recordsFiltered = 0;
            if (secprofileList != null && secprofileList.Count > 0)
            {
                recordsFiltered = secprofileList[0].TotalCount;
                totalRecords = secprofileList[0].TotalCount;
            }
            int initialPage = start.Value;
            var filteredResult = secprofileList
             .ToList();
            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult }, JsonSerializerDateSettings);
        }

       
        #endregion

        #region Security profile add or edit
        public PartialViewResult SecurityProfileAddOrEdit(long SecurityProfileId)
        {
            CustomSecurityProfileWrapper SecurityProfileWrapper = new CustomSecurityProfileWrapper(userData);
            CustomSecurityProfileVM SecurityprofileVM = new CustomSecurityProfileVM();

            SecurityprofileVM.customSecurityProfileModel = new CustomSecurityProfileModel();
            SecurityprofileVM.customSecurityProfileModel.Pagetype = "Add";
            if (SecurityProfileId != 0)
            {
                SecurityprofileVM.customSecurityProfileModel = SecurityProfileWrapper.GetEditSecurityProfileDetailsById(SecurityProfileId);
                SecurityprofileVM.customSecurityProfileModel.Pagetype = "Edit";
            }
           

            LocalizeControls(SecurityprofileVM, LocalizeResourceSetConstants.SecurityProfileDetails);
            return PartialView("~/Views/Configuration/CustomSecurityProfile/_CustomSecurityProfileAddOrEdit.cshtml", SecurityprofileVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddOrEditSecurityProfile(CustomSecurityProfileVM objSP, string Command)
        {
            CustomSecurityProfileWrapper SPWrapper = new CustomSecurityProfileWrapper(userData);
            string ModelValidationFailedMessage = string.Empty;
            string Mode = string.Empty;
            if (ModelState.IsValid)
            {
                if (objSP.customSecurityProfileModel.SecurityProfileId > 0)
                {
                    Mode = "Edit";
                }
                else
                {
                    Mode = "Add";
                }
                DataContracts.SecurityProfile securityprofile = new DataContracts.SecurityProfile();
                securityprofile = SPWrapper.AddOrEditSecurityProfile(objSP);
                if (securityprofile.ErrorMessages != null && securityprofile.ErrorMessages.Count > 0)
                {
                    return Json(securityprofile.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), Command = Command, SecurityProfileId = securityprofile.SecurityProfileId, Description = securityprofile.Description, Name = securityprofile.Name, Mode = Mode }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion
        #region Details

        public ActionResult CustomSecurityProfileDetail(long SecurityProfileId, string Description, string Name, string mode)
        {
            CustomSecurityProfileVM customSecurityProfileVM = new CustomSecurityProfileVM();
            CustomSecurityProfileModel customsecurityProfileModel = new CustomSecurityProfileModel();
            CustomSecurityProfileWrapper objCustomSecurityProfileWrapper = new CustomSecurityProfileWrapper(userData);
            customsecurityProfileModel.SecurityProfileId = SecurityProfileId;
            customsecurityProfileModel.Description = Description;
            customsecurityProfileModel.Name = Name;
            var customsecurityItemList = objCustomSecurityProfileWrapper.GetCustomAllSecurityItemV2List(SecurityProfileId, mode);
            customSecurityProfileVM.customSecurityProfileModel = customsecurityProfileModel;
            customSecurityProfileVM.customsecurityItemList = customsecurityItemList;
            LocalizeControls(customSecurityProfileVM, LocalizeResourceSetConstants.SecurityProfileDetails);
            return PartialView("~/Views/Configuration/CustomSecurityProfile/_CustomSecurityProfileDetail.cshtml", customSecurityProfileVM);
        }
        [HttpPost]
        public ActionResult SaveSecurityItems(List<CustomSecurityItemModel> objlist)
        {
            CustomSecurityProfileWrapper objCustomSecurityProfileWrapper = new CustomSecurityProfileWrapper(userData);
            objCustomSecurityProfileWrapper.UpdateSecurityItem(objlist);          
            var jsonResult = Json(new { Result = JsonReturnEnum.success.ToString(), securityprofileid = objlist[0].SecurityProfileId }, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
       
        #endregion

        #region Print
        [HttpGet]
        public string GetCustomSecurityProfileGridPrintData(string colname, string coldir, int? draw, int? start, int? length, string name = "", string description = "", string searchText = "")
        {
            CustomSecurityProfilePrintModel objCSPrintModel;
            List<CustomSecurityProfilePrintModel> objCSPrintModelList = new List<CustomSecurityProfilePrintModel>();                       

            CustomSecurityProfileWrapper objCustomSecurityProfileWrapper = new CustomSecurityProfileWrapper(userData);
            start = start.HasValue
               ? start / length
               : 0;
            int skip = start * length ?? 0;
            length = 100000;
            var secprofileList = objCustomSecurityProfileWrapper.GetCustomSecurityProfiles(colname, coldir, skip, length, searchText, name, description);           

            foreach (var item in secprofileList)
            {
                objCSPrintModel = new CustomSecurityProfilePrintModel();
                objCSPrintModel.SecurityProfileId = item.SecurityProfileId;
                objCSPrintModel.Name = item.Name;
                objCSPrintModel.Description = item.Description;
                objCSPrintModelList.Add(objCSPrintModel);
            }
            return JsonConvert.SerializeObject(new { data = objCSPrintModelList }, JsonSerializerDateSettings);
        }
        #endregion
    }
}