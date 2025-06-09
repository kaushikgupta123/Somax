using Client.BusinessWrapper.Configuration.SecurityProfile;
using Client.Controllers.Common;
using Client.Models.Configuration.SecurityProfile;
using DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common.Constants;
using Newtonsoft.Json;
using Client.Common;
namespace Client.Controllers.Configuration.SecurityProfileMaintenance
{
    public class SecurityProfileController : ConfigBaseController
    {
        public ActionResult Index()
        {
            SecurityProfileVM securityProfileVM = new SecurityProfileVM();
            SecurityProfileWrapper objSecurityProfileWrapper = new SecurityProfileWrapper(userData);
            LocalizeControls(securityProfileVM, LocalizeResourceSetConstants.SecurityProfileDetails);
            return View("~/Views/Configuration/SecurityProfile/index.cshtml", securityProfileVM);
        }
        [HttpPost]
        public string GetMSecurityProfileGridData(string filter, int? draw, int? start, int? length, string MaintOnDemandMasterId = "", string Description = "", string Type = "", DateTime? CreateDate = null, string srcData = "")
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            var colname = Request.Form.GetValues("columns[" + order + "][name]");
            SecurityProfileWrapper objSecurityProfileWrapper = new SecurityProfileWrapper(userData);
            var secprofileList = objSecurityProfileWrapper.GetProfiles();
            if (secprofileList != null)
            {
                secprofileList = this.GetAllEquipmentsSortByColumnWithOrder(colname[0], orderDir, secprofileList);
                if (!string.IsNullOrEmpty(filter))
                {
                    filter = filter.ToUpper();
                    secprofileList = secprofileList.Where(x => (!string.IsNullOrWhiteSpace(x.Name) && x.Name.ToUpper().Contains(filter))
                                                            || (!string.IsNullOrWhiteSpace(x.Description) && x.Description.ToUpper().Contains(filter))
                                                            ).ToList();
                }
            }
            var totalRecords = 0;
            var recordsFiltered = 0;
            start = start.HasValue
                ? start / length
                : 0;
            recordsFiltered = secprofileList.Count();
            totalRecords = secprofileList.Count();

            int initialPage = start.Value;

            var filteredResult = secprofileList
                .Skip(initialPage * length ?? 0)
                .Take(length ?? 0)
                .ToList();
            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult }, JsonSerializerDateSettings);
        }
        private List<SecurityProfileModel> GetAllEquipmentsSortByColumnWithOrder(string order, string orderDir, List<SecurityProfileModel> data)
        {
            List<SecurityProfileModel> lst = new List<SecurityProfileModel>();
            switch (order)
            {
                case "0":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Name).ToList() : data.OrderBy(p => p.Name).ToList();
                    break;
                case "1":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Description).ToList() : data.OrderBy(p => p.Description).ToList();
                    break;
                default:
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderBy(p => p.Name).ToList() : data.OrderBy(p => p.Name).ToList();
                    break;
            }
            return lst;
        }
        public ActionResult SecurityProfileDetail(long SecurityProfileId, string Description)
        {
            SecurityProfileVM securityProfileVM = new SecurityProfileVM();
            SecurityProfileModel securityProfileModel = new SecurityProfileModel();
            SecurityProfileWrapper objSecurityProfileWrapper = new SecurityProfileWrapper(userData);
            securityProfileModel.SecurityProfileId = SecurityProfileId;
            securityProfileModel.Description = Description;
            var securityItemList = objSecurityProfileWrapper.GetSecurityItemList(SecurityProfileId);
            securityProfileVM.securityProfileModel = securityProfileModel;
            securityProfileVM.SecurityItemModelList = securityItemList;
            LocalizeControls(securityProfileVM, LocalizeResourceSetConstants.SecurityProfileDetails);
            return PartialView("~/Views/Configuration/SecurityProfile/_SecurityProfileDetail.cshtml", securityProfileVM);
        }

        [HttpPost]
        public ActionResult SaveSecurityItems(List<SecurityItemModel> objlist)
        {
            SecurityProfileWrapper objSecurityProfileWrapper = new SecurityProfileWrapper(userData);
            objSecurityProfileWrapper.UpdateSecurityItem(objlist, false);
            return Json(new { Result = JsonReturnEnum.success.ToString(), securityprofileid = objlist[0].SecurityProfileId }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult SaveSecuritySingleItems(List<SecurityItemModel> objlist)
        {
            SecurityProfileWrapper objSecurityProfileWrapper = new SecurityProfileWrapper(userData);
            objSecurityProfileWrapper.UpdateSecurityItem(objlist, true);
            return Json(new { Result = JsonReturnEnum.success.ToString(), securityprofileid = objlist[0].SecurityProfileId }, JsonRequestBehavior.AllowGet);
        }
    }
}