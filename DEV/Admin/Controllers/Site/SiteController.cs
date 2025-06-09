using Admin.BusinessWrapper.Client;
using Admin.Common;
using Admin.Controllers.Common;
using Admin.Models.Site;
using Common.Constants;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Admin.BusinessWrapper;
using Admin.BusinessWrapper.Site;
using Admin.Models.Client;
using System.Threading.Tasks;
using Admin.Models;
using Admin.Models.UserManagement;
using DataContracts;

namespace Admin.Controllers.Site
{
    public class SiteController : SomaxBaseController
    {
        #region Site Search
        public ActionResult Index()
        {
            SiteWrapper siteWrapper = new SiteWrapper(userData);
            siteWrapper.userData = userData;
            SiteVM SiteVM = new SiteVM();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            SiteModel SiteModel = new SiteModel();
            SiteVM.security = this.userData.Security;
            var ClientList = siteWrapper.GetAllActiveClient();
            if (ClientList != null)
            {
                SiteModel.ClientList = ClientList.Select(x => new SelectListItem { Text = x.Name, Value = x.ClientId.ToString() });
            }
            SiteVM.SiteModel = SiteModel;
            LocalizeControls(SiteVM, LocalizeResourceSetConstants.Global);
            return View(SiteVM);
        }
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public string GetSiteGridData(int? draw, int? start, int? length, long SiteId = 0, long ClientId = 0, string Name = "", string AddressCity = "", string AddressState = "", string SearchText = "", string order = "2", string orderDir = "asc")
        {
            List<SiteSearchModel> SiteSearchModelList = new List<SiteSearchModel>();
            var colname = Request.Form.GetValues("columns[" + order + "][name]");
            SearchText = SearchText.Replace("%", "[%]");
            Name = Name.Replace("%", "[%]");
            AddressCity = AddressCity.Replace("%", "[%]");
            AddressState = AddressState.Replace("%", "[%]");
            start = start.HasValue
                 ? start / length
                 : 0;
            int skip = start * length ?? 0;
            SiteWrapper siteWrapper = new SiteWrapper(userData);
            siteWrapper.userData = userData;
            List<SiteSearchModel> SiteList = siteWrapper.GetSiteGridData(order, orderDir, skip, length ?? 0, SiteId, ClientId, Name, AddressCity, AddressState, SearchText);
            var totalRecords = 0;
            var recordsFiltered = 0;
            if (SiteList != null && SiteList.Count > 0)
            {
                recordsFiltered = SiteList[0].TotalCount;
                totalRecords = SiteList[0].TotalCount;
            }
            int initialPage = start.Value;
            var filteredResult = SiteList
             .ToList();
            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult }, JsonSerializerDateSettings);
        }
        public string GetSitePrintData(string _colname, string _coldir, int? draw, int? start, int? length, long _SiteId = 0, long _ClientId = 0, string _Name = "", string _AddressCity = "", string _AddressState = "", string _searchText = "")
        {
            SitePrintModel objSitePrintModel;
            List<SitePrintModel> SitePrintModelList = new List<SitePrintModel>();
            _searchText = _searchText.Replace("%", "[%]");
            _Name = _Name.Replace("%", "[%]");
            _AddressCity = _AddressCity.Replace("%", "[%]");
            _AddressState = _AddressState.Replace("%", "[%]");
            start = start.HasValue
               ? start / length
               : 0;
            int skip = start * length ?? 0;
            int lengthForPrint = 100000;
            SiteWrapper siteWrapper = new SiteWrapper(userData);
            siteWrapper.userData = userData;
            List<SiteSearchModel> SiteSearchList = siteWrapper.GetSiteGridData(_colname, _coldir, 0, lengthForPrint, _SiteId, _ClientId, _Name, _AddressCity, _AddressState, _searchText);
            foreach (var item in SiteSearchList)
            {
                objSitePrintModel = new SitePrintModel();
                objSitePrintModel.SiteId = item.SiteId;
                objSitePrintModel.Name = item.Name;
                objSitePrintModel.AddressCity = item.AddressCity;
                objSitePrintModel.AddressState = item.AddressState;
                SitePrintModelList.Add(objSitePrintModel);
            }
            return JsonConvert.SerializeObject(new { data = SitePrintModelList }, JsonSerializerDateSettings);
        }
        #endregion

        #region Site Details
        public PartialViewResult SiteDetails(long ClientId, long SiteId)
        {
            SiteVM siteVM = new SiteVM();
            SiteWrapper siteWrapper = new SiteWrapper(userData);
            siteWrapper.userData = userData;
            SiteModel siteModel = new SiteModel();
            UserValidateModel userValidateModel = new UserValidateModel();
            var SiteDetails = siteWrapper.GetSiteDetailsBySiteId(ClientId, SiteId);
            siteVM._CreatedLastUpdatedModel = siteWrapper.createdLastUpdatedModel(SiteId);
            siteVM.SiteModel = SiteDetails;
            userValidateModel.ClientId = SiteDetails.ClientId;
            userValidateModel.SiteId = SiteDetails.SiteId;
            siteVM.UserValidateModel = userValidateModel;
            string temp = string.Empty;
            LocalizeControls(siteVM, LocalizeResourceSetConstants.Global);
            return PartialView("_SiteDetails", siteVM);
        }
        #endregion

        #region Add ,Edit Site
        public PartialViewResult AddSite(string ClientName, long ClientId)
        {
            SiteWrapper SiteWrapper = new SiteWrapper(userData);
            SiteWrapper.userData = userData;
            SiteVM SiteVM = new SiteVM();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            List<DataContracts.LookupList> LocalizationTypeList = new List<DataContracts.LookupList>();
            SiteModel SiteModel = new SiteModel();
            ClientModel ClientModel = new ClientModel();
            ClientWrapper ClientWrapper = new ClientWrapper(userData);
            List<DropDownModel> Localizationslist = new List<DropDownModel>();
            List<DropDownModel> TimeZonelist = new List<DropDownModel>();

            Task[] tasks = new Task[3];
            tasks[0] = Task.Factory.StartNew(() => ClientModel = ClientWrapper.GetClientDetailsById(ClientId));
            tasks[1] = Task.Factory.StartNew(() => Localizationslist = commonWrapper.GetListFromConstVals(LookupListConstants.LocalizationTypes));
            tasks[2] = Task.Factory.StartNew(() => TimeZonelist = commonWrapper.GetListFromConstVals(LookupListConstants.TimeZoneList));
            Task.WaitAll(tasks);

            if(!tasks[0].IsFaulted && tasks[0].IsCompleted)
            {              
                SiteModel.ClientSiteControl = ClientModel.SiteControl;
                SiteModel.Localization = ClientModel.Localization;
                SiteModel.TimeZone = ClientModel.DefaultTimeZone;
                SiteModel.UIConfigurationLocation = ClientModel.UIConfigurationLocation;
                SiteModel.UIConfigurationCompany = ClientModel.UIConfiguration;
            }            

            if (!tasks[1].IsFaulted && tasks[1].IsCompleted && Localizationslist != null && Localizationslist.Count > 0) 
            {
                SiteModel.LocalizationList = Localizationslist.Select(x => new SelectListItem { Text = x.text.ToString(), Value = x.value.ToString() }).OrderBy(x=>x.Text);
            }  
            if (!tasks[2].IsFaulted && tasks[2].IsCompleted && TimeZonelist != null && TimeZonelist.Count > 0) 
            {
                SiteModel.TimeZoneList = TimeZonelist.Select(x => new SelectListItem { Text = x.text.ToString(), Value = x.value.ToString() });
            }
            var StatusList = UtilityFunction.InactiveActiveStatusTypes();
            if (StatusList != null)
            {
                SiteModel.InactiveFlagList = StatusList.Select(x => new SelectListItem { Text = x.text, Value = x.text.ToString() });
            }
            SiteModel.ClientName = ClientName;
            SiteModel.ClientId = ClientId;
            SiteVM.SiteModel = SiteModel;
            LocalizeControls(SiteVM, LocalizeResourceSetConstants.Global);
            return PartialView("~/Views/Site/_SiteAdd.cshtml", SiteVM);
        }
        [HttpPost]
        public ActionResult EditSite(long SiteId, long ClientId)
        {
            SiteWrapper SiteWrapper = new SiteWrapper(userData);
            SiteWrapper.userData = userData;
            SiteVM SiteVM = new SiteVM();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            List<DataContracts.LookupList> LocalizationTypeList = new List<DataContracts.LookupList>();
            SiteModel SiteModel = new SiteModel();
            List<DropDownModel> Localizationslist = new List<DropDownModel>();
            List<DropDownModel> TimeZonelist = new List<DropDownModel>();

            Task[] tasks = new Task[3];
            tasks[0] = Task.Factory.StartNew(() => SiteModel = SiteWrapper.GetSiteDetailsBySiteId(ClientId, SiteId));
            tasks[1] = Task.Factory.StartNew(() => Localizationslist = commonWrapper.GetListFromConstVals(LookupListConstants.LocalizationTypes));
            tasks[2] = Task.Factory.StartNew(() => TimeZonelist = commonWrapper.GetListFromConstVals(LookupListConstants.TimeZoneList));
            Task.WaitAll(tasks);
      
            if (!tasks[1].IsFaulted && tasks[1].IsCompleted && Localizationslist != null && Localizationslist.Count > 0) 
            {
                SiteModel.LocalizationList = Localizationslist.Select(x => new SelectListItem { Text = x.text.ToString(), Value = x.value.ToString() });
            }
            if (!tasks[2].IsFaulted && tasks[2].IsCompleted && TimeZonelist != null && TimeZonelist.Count > 0) 
            {
                SiteModel.TimeZoneList = TimeZonelist.Select(x => new SelectListItem { Text = x.text.ToString(), Value = x.value.ToString() });
            }
            var StatusList = UtilityFunction.InactiveActiveStatusTypes();
            if (StatusList != null)
            {
                SiteModel.InactiveFlagList = StatusList.Select(x => new SelectListItem { Text = x.text, Value = x.text.ToString() });
            }
            if (!tasks[0].IsFaulted && tasks[0].IsCompleted)
            {
                SiteVM.SiteModel = SiteModel;
            }            
            LocalizeControls(SiteVM, LocalizeResourceSetConstants.Global);
            return PartialView("~/Views/Site/_SiteEdit.cshtml", SiteVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddorEditSite(SiteVM objSite, string Command)
        {
            List<string> ErrorList = new List<string>();
            SiteWrapper siteWrapper = new SiteWrapper(userData);
            siteWrapper.userData = userData;
            string ModelValidationFailedMessage = string.Empty;
            string Mode = string.Empty;
            if (ModelState.IsValid)
            {
                DataContracts.Site site = new DataContracts.Site();
                if (objSite.SiteModel.SiteId > 0)
                {
                    Mode = "Edit";
                }
                else
                {
                    Mode = "Add";
                }
                site = siteWrapper.AddorEditSite(objSite);
                if (site.ErrorMessages != null && site.ErrorMessages.Count > 0)
                {
                    return Json(site.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), Command = Command, SiteId = site.SiteId, ClientId = site.ClientId, Mode = Mode }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region V2-1176 Create Guest URL
        public JsonResult CreateGuestURL(long ClientId, long SiteId)
        {
            string url = "clientid=" + ClientId + "&siteid=" + SiteId;
            string encryptedURL = url.Encrypt();
            string baseURL = System.Configuration.ConfigurationManager.AppSettings["GuestWorkRequestBaseURL"].ToString();
            string GuestURL = baseURL + "GuestWorkRequest/Index?url=" + encryptedURL;
            return Json(new { Result = JsonReturnEnum.success.ToString(), data = GuestURL }, JsonRequestBehavior.AllowGet);
        }
        #endregion


        #region V2-1178
         [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ValidateUserExist(SiteVM siteVM)
        {

            List<string> ErrorList = new List<string>();           
            string ModelValidationFailedMessage = string.Empty;
            if (ModelState.IsValid)
            {
                SiteWrapper _siteObj = new SiteWrapper(userData);
                long LoginInfoId = _siteObj.ValidateUserExist(siteVM);               
                if (LoginInfoId > 0)
                {
                    string url = string.Empty;

                    return Json(new { Result = JsonReturnEnum.success.ToString(), LoginInfoId=LoginInfoId }, JsonRequestBehavior.AllowGet);
                }
                else
                {                  
                    return Json(new { Result = JsonReturnEnum.failed.ToString()}, JsonRequestBehavior.AllowGet);
                }
               
            }
            else
            {
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult CreatePartIssueURL(long ClientId, long SiteId, long LoginInfoId)
        {
            string url = "clientid=" + ClientId + "&siteid=" + SiteId + "&logininfoid=" + LoginInfoId;
            string encryptedURL = url.Encrypt();
            string baseURL = System.Configuration.ConfigurationManager.AppSettings["GuestWorkRequestBaseURL"].ToString();
            string PartIssueURL = baseURL + "PartIssue/index?url=" + encryptedURL;
            return Json(new { Result = JsonReturnEnum.success.ToString(), data = PartIssueURL }, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}