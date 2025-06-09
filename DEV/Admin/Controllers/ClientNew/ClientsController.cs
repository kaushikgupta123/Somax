using Admin.BusinessWrapper;
using Admin.BusinessWrapper.Client;
using Admin.BusinessWrapper.Site;
using Admin.Common;
using Admin.Controllers.Common;
using Admin.Models;
using Admin.Models.Client;
using Admin.Models.ClientMessages;

using Common.Constants;

using DataContracts;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
namespace Admin.Controllers.Client
{
    public class ClientsController : SomaxBaseController
    {
        #region Client Search
        public ActionResult Index()
        {
            ClientWrapper clientWrapper = new ClientWrapper(userData);
            ClientVM ClientVM = new ClientVM();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            ClientVM.ClientModel = new ClientModel();
            ClientVM.security = this.userData.Security;
            List<DataContracts.LookupList> AllLookUps = new List<DataContracts.LookupList>();
            var StatusList = UtilityFunction.AllClientInactiveActiveStatusTypes();
            if (StatusList != null)
            {
                ClientVM.InactiveFlagList = StatusList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
            }
            var ClientList = commonWrapper.GetAllActiveClient();
            if (ClientList != null)
            {
                ClientVM.ActiveClientList = ClientList.Select(x => new SelectListItem { Text = x.Name, Value = x.ClientId.ToString() });
            }
            var SiteList = commonWrapper.AllActiveSiteList();
            if (SiteList != null)
            {
                ClientVM.ActiveSiteList = SiteList.Select(x => new SelectListItem { Text = x.Name, Value = x.SiteId.ToString() }).ToList();
            }
            LocalizeControls(ClientVM, LocalizeResourceSetConstants.ClientDetails);
            return View(ClientVM);
        }
        public string GetClientGridData(int? draw, int? start, int? length, int customQueryDisplayId, long ClientId = 0, long Siteid = 0, string Name = "", string Contact = "", string Mail = "", string SearchText = "", string Order = "1")
        {
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            start = start.HasValue
                  ? start / length
                  : 0;
            int skip = start * length ?? 0;
            ClientWrapper clientWrapper = new ClientWrapper(userData);
            clientWrapper.userData = userData;
            List<ClientSearchModel> ClientList = clientWrapper.GetClientGridDataNew(customQueryDisplayId, Order, orderDir, skip, length ?? 0, ClientId, Siteid, Name, Contact, Mail, SearchText);
            var totalRecords = 0;
            var recordsFiltered = 0;
            if (ClientList != null && ClientList.Count > 0)
            {
                recordsFiltered = ClientList[0].TotalCount;
                totalRecords = ClientList[0].TotalCount;
            }
            int initialPage = start.Value;
            var filteredResult = ClientList
             .ToList();
            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult }, JsonSerializerDateSettings);
        }
        public ActionResult GetClientInnerGrid(long ClientId)
        {
            ClientVM objClientVM = new ClientVM();
            ClientWrapper cWrapper = new ClientWrapper(userData);
            objClientVM.listChildGridModel = cWrapper.PopulateChilditems(ClientId);
            LocalizeControls(objClientVM, LocalizeResourceSetConstants.ClientDetails);
            return View("_ClientSearchInnerGrid", objClientVM);
        }
        #endregion
        #region Add Client
        public PartialViewResult AddClient()
        {
            ClientWrapper ClientWrapper = new ClientWrapper(userData);
            ClientVM ClientVM = new ClientVM();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            List<DataContracts.LookupList> LocalizationTypeList = new List<DataContracts.LookupList>();
            List<DropDownModel> BusinessTypeList = new List<DropDownModel>();
            List<DropDownModel> packagelevelList = new List<DropDownModel>();
            List<DropDownModel> TimeZonelist = new List<DropDownModel>();
            ClientVM.CreateClientModel = new CreateClientModel();

            Task[] tasks = new Task[3];
            tasks[0] = Task.Factory.StartNew(() => BusinessTypeList = commonWrapper.GetListFromConstVals(LookupListConstants.Client_BusinessType));
            tasks[1] = Task.Factory.StartNew(() => packagelevelList = commonWrapper.GetListFromConstVals(LookupListConstants.Client_PackageLevel));
            tasks[2] = Task.Factory.StartNew(() => TimeZonelist = commonWrapper.GetListFromConstVals(LookupListConstants.TimeZoneList));
            Task.WaitAll(tasks);

            if (!tasks[0].IsFaulted && tasks[0].IsCompleted && BusinessTypeList != null && BusinessTypeList.Count > 0)
            {
                ClientVM.CreateClientModel.BusinessTypeList = BusinessTypeList.Select(x => new SelectListItem { Text = x.text.ToString(), Value = x.value.ToString() });
            }
            if (!tasks[1].IsFaulted && tasks[1].IsCompleted && packagelevelList != null && packagelevelList.Count > 0)
            {
                ClientVM.CreateClientModel.PackagelevelList = packagelevelList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
            }
            if (!tasks[2].IsFaulted && tasks[2].IsCompleted && TimeZonelist != null && TimeZonelist.Count > 0)
            {
                ClientVM.TimeZonelist = TimeZonelist.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
            }
            var Localizationslist = UtilityFunction.LocalizationsTypes();
            if (Localizationslist != null)
            {
                ClientVM.LocalizationsList = Localizationslist.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() }).OrderBy(x => x.Text);
            }
            ClientVM.CreateClientModel.IsAdd = true;

            LocalizeControls(ClientVM, LocalizeResourceSetConstants.ClientDetails);
            return PartialView("~/Views/Clients/_ClientAdd.cshtml", ClientVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddClient(ClientVM objClient, string Command)
        {
            List<string> ErrorList = new List<string>();
            ClientWrapper ClientWrapper = new ClientWrapper(userData);
            string ModelValidationFailedMessage = string.Empty;
            string ConnectionStringValidationFailedMessage = string.Empty;
            if (ModelState.IsValid)
            {
                var apm = objClient.CreateClientModel.APM;
                var cmms = objClient.CreateClientModel.CMMS;
                var sanitation = objClient.CreateClientModel.Sanitation;
                DataContracts.Client client = new DataContracts.Client();
                if (!apm && !cmms && !sanitation)
                {
                    ConnectionStringValidationFailedMessage = UtilityFunction.GetMessageFromResource("APMCMMSSanitationValidationMsg", LocalizeResourceSetConstants.ClientDetails);
                    return Json(ConnectionStringValidationFailedMessage, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    client = ClientWrapper.SaveClient(objClient);
                }
                if (client.ErrorMessages != null && client.ErrorMessages.Count > 0)
                {
                    return Json(client.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), Command = Command, ClientId = client.CreatedClientId, Status = client.Status }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        #region Edit Client
        [HttpPost]
        public ActionResult EditClient(long ClientId)
        {
            ClientWrapper ClientWrapper = new ClientWrapper(userData);
            ClientVM ClientVM = new ClientVM();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            List<DataContracts.LookupList> LocalizationTypeList = new List<DataContracts.LookupList>();
            ClientVM.CreateClientModel = new CreateClientModel();
            List<DropDownModel> BusinessTypeList = new List<DropDownModel>();
            List<DropDownModel> packagelevelList = new List<DropDownModel>();
            List<DropDownModel> TimeZonelist = new List<DropDownModel>();
            List<DropDownModel> Localizationslist = new List<DropDownModel>();

            Task[] tasks = new Task[2];
            tasks[0] = Task.Factory.StartNew(() => BusinessTypeList = commonWrapper.GetListFromConstVals(LookupListConstants.Client_BusinessType));
            tasks[1] = Task.Factory.StartNew(() => packagelevelList = commonWrapper.GetListFromConstVals(LookupListConstants.Client_PackageLevel));
            Task.WaitAll(tasks);

            ClientVM.CreateClientModel = ClientWrapper.GetClientDetailsForEdit(ClientId);
            if (!tasks[0].IsFaulted && tasks[0].IsCompleted && BusinessTypeList != null && BusinessTypeList.Count > 0)
            {
                ClientVM.CreateClientModel.BusinessTypeList = BusinessTypeList.Select(x => new SelectListItem { Text = x.text.ToString(), Value = x.value.ToString() });
            }
            if (!tasks[1].IsFaulted && tasks[1].IsCompleted && packagelevelList != null && packagelevelList.Count > 0)
            {
                ClientVM.CreateClientModel.PackagelevelList = packagelevelList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
            }

            LocalizeControls(ClientVM, LocalizeResourceSetConstants.ClientDetails);
            return PartialView("~/Views/Clients/_ClientEdit.cshtml", ClientVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateClient(ClientVM ClientVM)
        {
            string emptyValue = string.Empty;
            ClientWrapper ClientWrapper = new ClientWrapper(userData);
            DataContracts.Client client = new DataContracts.Client();
            string constr = ClientVM.CreateClientModel.ConnectionString;
            string ConnectionStringValidationFailedMessage = string.Empty;
            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(constr))
                {
                    if (client.CheckIsClientConnectionStringValid(constr) == true)
                    {
                        client = ClientWrapper.SaveClient(ClientVM);
                    }
                    else
                    {
                        ConnectionStringValidationFailedMessage = UtilityFunction.GetMessageFromResource("GlobalConnectionStringValidationMsg", LocalizeResourceSetConstants.Global);
                        return Json(ConnectionStringValidationFailedMessage, JsonRequestBehavior.AllowGet);
                    }
                }
                if (client.ErrorMessages != null && client.ErrorMessages.Count > 0)
                {
                    return Json(client.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), ClientId = client.ClientId, Status = client.Status }, JsonRequestBehavior.AllowGet);
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
        #region User Exists
        [HttpPost]
        public ActionResult CheckIfClientNameExist(ClientVM objClient)
        {
            bool IfClientNameExist = false;
            int count = 0;
            ClientWrapper ClientWrapper = new ClientWrapper(userData);
            objClient.ClientModel = new ClientModel();
            objClient.ClientModel.ClientId = objClient.CreateClientModel.ClientId;
            objClient.ClientModel.Name = objClient.CreateClientModel.Name;
            count = ClientWrapper.CountIfUserExist(objClient.ClientModel);
            if (count > 0)
            {
                IfClientNameExist = true;
            }
            else
            {
                IfClientNameExist = false;
            }
            return Json(!IfClientNameExist, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region TestConnectionString
        [HttpPost]
        public ActionResult TestConnectionString(string conStr)
        {
            DataContracts.Client client = new DataContracts.Client();
            if (!string.IsNullOrEmpty(conStr))
            {
                if (client.CheckIsClientConnectionStringValid(conStr) == false)
                {
                    return Json(new { Result = JsonReturnEnum.failed.ToString(), conStr = conStr }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), conStr = conStr }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new { Result = JsonReturnEnum.empty.ToString(), conStr = conStr }, JsonRequestBehavior.AllowGet);

            }
        }
        #endregion
        #region Active/Inactive Client
        [HttpPost]
        public JsonResult MakeClientActiveInactive(bool InActiveFlag, long ClientId)
        {
            ClientWrapper accWrapper = new ClientWrapper(userData);
            var ErrorMessages = accWrapper.MakeActiveInactiveClient(ClientId, InActiveFlag);
            if (ErrorMessages != null && ErrorMessages.Count > 0)
            {
                return Json(ErrorMessages, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Result = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        #region Client Details
        public PartialViewResult ClientDetailsById(long clientid)
        {
            ClientWrapper clientWrapper = new ClientWrapper(userData);
            ClientVM clientVM = new ClientVM();
            clientWrapper.userData = userData;
            var ClientDetails = clientWrapper.GetClientDetailsByClientId(clientid);
            clientVM.ClientModel = ClientDetails;
            clientVM.ClientModel.ClientId = clientid;
            LocalizeControls(clientVM, LocalizeResourceSetConstants.ClientDetails);
            return PartialView("~/Views/Clients/_ClientDetails.cshtml", clientVM);
        }
        [HttpPost]
        public string PopulateEventLog(int? draw, int? start, int? length, long ClientId)
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            ClientWrapper clientWrapper = new ClientWrapper(userData);
            start = start.HasValue
                ? start / length
                : 0;
            int skip = start * length ?? 0;
            List<EventLogModel> EventLogList = clientWrapper.PopulateEventLog(ClientId, order, orderDir, skip, length ?? 0);

            var totalRecords = 0;
            var recordsFiltered = 0;
            start = start.HasValue
                ? start / length
                : 0;
            recordsFiltered = EventLogList.Select(o => o.TotalCount).FirstOrDefault();
            totalRecords = EventLogList.Select(o => o.TotalCount).FirstOrDefault();
            var filteredResult = EventLogList
                .ToList();
            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult }, JsonSerializerDateSettings);
        }
        [HttpPost]
        public string PopulateActivityTableGrid(int? draw, int? start, int? length, long ClientId)
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            ClientWrapper clientWrapper = new ClientWrapper(userData);
            start = start.HasValue
                ? start / length
                : 0;
            int skip = start * length ?? 0;
            List<LoginAuditingInfo> LoginAuditingInfoList = clientWrapper.PopulateActivity(ClientId, order, orderDir, skip, length ?? 0);

            var totalRecords = 0;
            var recordsFiltered = 0;
            start = start.HasValue
                ? start / length
                : 0;
            recordsFiltered = LoginAuditingInfoList.Select(o => o.TotalCount).FirstOrDefault();
            totalRecords = LoginAuditingInfoList.Select(o => o.TotalCount).FirstOrDefault();
            var filteredResult = LoginAuditingInfoList
                .ToList();
            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult }, JsonSerializerDateSettings);
        }

        [HttpPost]
        public string PopulateSiteTableGrid(int? draw, int? start, int? length, long ClientId)
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            ClientWrapper clientWrapper = new ClientWrapper(userData);

            List<ChildGridModel> ChildGridModelList = clientWrapper.PopulateChilditems(ClientId); ;
            ChildGridModelList = this.GetSiteByColumnWithOrder(order, orderDir, ChildGridModelList);
            var totalRecords = 0;
            var recordsFiltered = 0;
            start = start.HasValue
                ? start / length
                : 0;
            recordsFiltered = ChildGridModelList.Count();
            totalRecords = ChildGridModelList.Count();

            int initialPage = start.Value;
            var filteredResult = ChildGridModelList
                .Skip(initialPage * length ?? 0)
                .Take(length ?? 0)
                .ToList();
            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult }, JsonSerializerDateSettings);
        }
        private List<ChildGridModel> GetSiteByColumnWithOrder(string order, string orderDir, List<ChildGridModel> data)
        {
            List<ChildGridModel> lst = new List<ChildGridModel>();

            switch (order)
            {
                case "0":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.SiteId).ToList() : data.OrderBy(p => p.SiteId).ToList();
                    break;
                case "1":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Name).ToList() : data.OrderBy(p => p.Name).ToList();
                    break;
                case "2":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Status).ToList() : data.OrderBy(p => p.Status).ToList();
                    break;
                case "3":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.APM).ToList() : data.OrderBy(p => p.APM).ToList();
                    break;
                case "4":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.CMMS).ToList() : data.OrderBy(p => p.CMMS).ToList();
                    break;
                case "5":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Sanitation).ToList() : data.OrderBy(p => p.Sanitation).ToList();
                    break;
            }
            return lst;
        }

        public PartialViewResult GetSiteDetailsByClientIdSiteId(long clientid, long siteid)
        {
            SiteWrapper siteWrapper = new SiteWrapper(userData);
            ClientVM clientVM = new ClientVM();
            SiteModelView siteModelView = new SiteModelView();
            siteWrapper.userData = userData;
            var SiteDetailsByClientIdSiteId = siteWrapper.GetSiteDetailsByClientIdSiteId(clientid, siteid);
            siteModelView = SiteDetailsByClientIdSiteId;
            clientVM.SiteModelView = siteModelView;
            LocalizeControls(clientVM, LocalizeResourceSetConstants.ClientDetails);
            return PartialView("~/Views/Clients/_SiteDetailsByClientIdSiteId.cshtml", clientVM);
        }
        public PartialViewResult GetSiteBillingDetailsByClientIdSiteId(long clientid, long siteid)
        {
            SiteBillingWrapper siteBillingWrapper = new SiteBillingWrapper(userData);
            ClientVM clientVM = new ClientVM();
            SiteBillingModelView siteBillingModelView = new SiteBillingModelView();
            siteBillingWrapper.userData = userData;
            var SiteDetailsByClientIdSiteId = siteBillingWrapper.GetSiteBillingDetailsByClientIdSiteId(clientid, siteid);
            siteBillingModelView = SiteDetailsByClientIdSiteId;
            clientVM.SiteBillingModelView = siteBillingModelView;
            LocalizeControls(clientVM, LocalizeResourceSetConstants.ClientDetails);
            return PartialView("~/Views/Clients/_SiteBillingDetailsByClientIdSiteId.cshtml", clientVM);
        }
        #endregion

        #region Edit Site
        public PartialViewResult EditSiteByClientIdSiteId(long ClientId, long SiteId)
        {
            ClientWrapper ClientWrapper = new ClientWrapper(userData);
            SiteWrapper siteWrapper = new SiteWrapper(userData);
            ClientVM ClientVM = new ClientVM();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            List<DataContracts.LookupList> LocalizationTypeList = new List<DataContracts.LookupList>();
            ClientVM.ClientModel = new ClientModel();
            ClientVM.SiteModelView = new SiteModelView();
            List<DropDownModel> TimeZonelist = new List<DropDownModel>();

            Task[] tasks = new Task[1];
            tasks[0] = Task.Factory.StartNew(() => TimeZonelist = commonWrapper.GetListFromConstVals(LookupListConstants.TimeZoneList));
            Task.WaitAll(tasks);

            ClientVM.SiteModelView = siteWrapper.GetSiteDetailsByClientIdSiteId(ClientId, SiteId);
            if (!tasks[0].IsFaulted && tasks[0].IsCompleted && TimeZonelist != null && TimeZonelist.Count > 0)
            {
                ClientVM.TimeZonelist = TimeZonelist.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
            }

            LocalizeControls(ClientVM, LocalizeResourceSetConstants.ClientDetails);
            return PartialView("~/Views/Clients/_SiteEdit.cshtml", ClientVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditSite(ClientVM objClient, string Command)
        {
            List<string> ErrorList = new List<string>();
            ClientWrapper ClientWrapper = new ClientWrapper(userData);
            ClientWrapper.userData = userData;
            string ModelValidationFailedMessage = string.Empty;
            string ConnectionStringValidationFailedMessage = string.Empty;
            if (ModelState.IsValid)
            {
                var apm = objClient.SiteModelView.APM;
                var cmms = objClient.SiteModelView.CMMS;
                var sanitation = objClient.SiteModelView.Sanitation;

                DataContracts.Site site = new DataContracts.Site();
                if (!apm && !cmms && !sanitation)
                {
                    ConnectionStringValidationFailedMessage = UtilityFunction.GetMessageFromResource("APMCMMSSanitationValidationMsg", LocalizeResourceSetConstants.ClientDetails);
                    return Json(ConnectionStringValidationFailedMessage, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    site = ClientWrapper.SaveSite(objClient);
                }
                if (site.ErrorMessages != null && site.ErrorMessages.Count > 0)
                {
                    return Json(site.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), Command = Command, SiteId = site.SiteId, Status = site.Status }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Edit Site Billing
        public PartialViewResult EditSiteBillingByClientIdSiteId(long ClientId, long SiteId)
        {
            ClientWrapper ClientWrapper = new ClientWrapper(userData);
            SiteBillingWrapper siteBillingWrapper = new SiteBillingWrapper(userData);
            ClientVM ClientVM = new ClientVM();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            ClientVM.SiteBillingModelView = siteBillingWrapper.GetSiteBillingDetailsByClientIdSiteId(ClientId, SiteId);
            LocalizeControls(ClientVM, LocalizeResourceSetConstants.ClientDetails);
            return PartialView("~/Views/Clients/_SiteBillingEdit.cshtml", ClientVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditSiteBilling(ClientVM objClient, string Command)
        {
            List<string> ErrorList = new List<string>();
            ClientWrapper ClientWrapper = new ClientWrapper(userData);
            ClientWrapper.userData = userData;
            string ModelValidationFailedMessage = string.Empty;
            string ConnectionStringValidationFailedMessage = string.Empty;
            if (ModelState.IsValid)
            {

                DataContracts.SiteBilling siteBilling = new DataContracts.SiteBilling();

                siteBilling = ClientWrapper.EditSiteBilling(objClient);

                if (siteBilling.ErrorMessages != null && siteBilling.ErrorMessages.Count > 0)
                {
                    return Json(siteBilling.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), Command = Command, SiteId = siteBilling.SiteId, Status = /*siteBilling.Status*/"Success" }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        #region Active/Inactive Sites
        [HttpPost]
        public JsonResult MakeSiteActiveInactive(bool InActiveFlag, long ClientId, long SiteId, int UpdateIndex)
        {
            ClientWrapper accWrapper = new ClientWrapper(userData);
            var ErrorMessages = accWrapper.MakeActiveInactive(ClientId, SiteId, InActiveFlag, UpdateIndex);
            if (ErrorMessages != null && ErrorMessages.Count > 0)
            {
                return Json(ErrorMessages, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Result = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        #region Add Site
        public PartialViewResult AddSite(long ClientId)
        {
            ClientWrapper ClientWrapper = new ClientWrapper(userData);
            ClientVM ClientVM = new ClientVM();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            List<DataContracts.LookupList> LocalizationTypeList = new List<DataContracts.LookupList>();

            List<DropDownModel> TimeZonelist = new List<DropDownModel>();

            ClientVM.SiteModelView = new SiteModelView();

            Task[] tasks = new Task[1];
            tasks[0] = Task.Factory.StartNew(() => TimeZonelist = commonWrapper.GetListFromConstVals(LookupListConstants.TimeZoneList));
            Task.WaitAll(tasks);

            if (!tasks[0].IsFaulted && tasks[0].IsCompleted && TimeZonelist != null && TimeZonelist.Count > 0)
            {
                ClientVM.TimeZonelist = TimeZonelist.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
            }

            ClientVM.SiteModelView.ClientId = ClientId;

            LocalizeControls(ClientVM, LocalizeResourceSetConstants.ClientDetails);
            return PartialView("~/Views/Clients/_SiteAdd.cshtml", ClientVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddSite(ClientVM objClient, string Command)
        {
            List<string> ErrorList = new List<string>();
            ClientWrapper ClientWrapper = new ClientWrapper(userData);
            string ModelValidationFailedMessage = string.Empty;
            string ConnectionStringValidationFailedMessage = string.Empty;
            if (ModelState.IsValid)
            {
                var apm = objClient.SiteModelView.APM;
                var cmms = objClient.SiteModelView.CMMS;
                var sanitation = objClient.SiteModelView.Sanitation;
                DataContracts.Site site = new DataContracts.Site();
                if (!apm && !cmms && !sanitation)
                {
                    ConnectionStringValidationFailedMessage = UtilityFunction.GetMessageFromResource("APMCMMSSanitationValidationMsg", LocalizeResourceSetConstants.ClientDetails);
                    return Json(ConnectionStringValidationFailedMessage, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    site = ClientWrapper.SaveSite(objClient);
                }
                if (site.ErrorMessages != null && site.ErrorMessages.Count > 0)
                {
                    return Json(site.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), Command = Command, ClientId = site.ClientId, Status = site.Status }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region WebSiteMaintainenceMessage Details V2-994
        public PartialViewResult SiteMaintainenceDetails()
        {
            ClientVM clientVM = new ClientVM();
            LocalizeControls(clientVM, LocalizeResourceSetConstants.ClientDetails);
            return PartialView("_ClientSiteMaintainanceNewSearch", clientVM);
        }
        public string GetSiteMaintainanceGridData(int? draw, int? start, int? length = 0)
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            start = start.HasValue
                ? start / length
                : 0;
            int skip = start * length ?? 0;
            SiteMaintenanceWrapper siteMaintenanceWrapper = new SiteMaintenanceWrapper(userData);
            List<SiteMaintenanceModel> sList = siteMaintenanceWrapper.SiteMaintenanceDetailsChunkSearch
                (order, length ?? 0, orderDir, skip);
            var totalRecords = 0;
            var recordsFiltered = 0;
            recordsFiltered = sList.Select(x => x.TotalCount).FirstOrDefault();
            totalRecords = sList.Select(x => x.TotalCount).FirstOrDefault();
            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = sList }, JsonSerializer12HoursDateAndTimeUptoMinuteSettings);
        }

        #region Add Edit SiteMaintainance
        public PartialViewResult AddEditSiteMaintenanceView(long SiteMaintenanceId)
        {
            var viewModel = new ClientVM();
            var siteMaintenanceModel = new SiteMaintenanceModel();
            string ViewPath = "";
            if (SiteMaintenanceId > 0)
            {
                var siteMaintenanceWrapper = new SiteMaintenanceWrapper(userData);
                siteMaintenanceModel = siteMaintenanceWrapper.RetrieveSiteMaintenance(SiteMaintenanceId);
                ViewPath = "~/Views/Clients/_EditSiteMaintenance.cshtml";
            }
            else
            {
                siteMaintenanceModel.SiteMaintenanceId = SiteMaintenanceId;
                ViewPath = "~/Views/Clients/_AddSiteMaintenance.cshtml";
            }
            viewModel._SiteMaintenanceModel.SiteMaintenanceModel = siteMaintenanceModel;
            LocalizeControls(viewModel, LocalizeResourceSetConstants.ClientDetails);
            return PartialView(ViewPath, viewModel);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public JsonResult AddEditSiteMaintenance(ClientVM model)
        {
            if (ModelState.IsValid)
            {
                SiteMaintenance siteMaintenance = new SiteMaintenance();
                SiteMaintenanceWrapper siteMaintenanceWrapper = new SiteMaintenanceWrapper(userData);
                string Mode = string.Empty;
                if (model._SiteMaintenanceModel.SiteMaintenanceModel.SiteMaintenanceId == 0)
                {
                    Mode = "add";
                    siteMaintenance = siteMaintenanceWrapper.AddSiteMaintenance(model._SiteMaintenanceModel.SiteMaintenanceModel);
                }
                else
                {
                    Mode = "edit";
                    siteMaintenance = siteMaintenanceWrapper.EditSiteMaintenance(model._SiteMaintenanceModel.SiteMaintenanceModel);
                }
                if (siteMaintenance.ErrorMessages != null && siteMaintenance.ErrorMessages.Count > 0)
                {
                    return Json(siteMaintenance.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), mode = Mode }, JsonRequestBehavior.AllowGet);
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

        #region MessageSelectedClient Details V2-993
        public PartialViewResult MessageSelectedClientDetails(long ClientCustomId)
        {
            ClientVM clientVM = new ClientVM();
            LocalizeControls(clientVM, LocalizeResourceSetConstants.ClientDetails);
            if (ClientCustomId > 0)
            {
                return PartialView("_MessageSelectedClientSearch", clientVM);
            }
            else
            {
                return PartialView("_MessageAllClientSearch", clientVM);
            }

        }
        public string GetMessageSelectedClientGridData(int? draw, int? start, long ClientCustomId = 0, int? length = 0)
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            start = start.HasValue
                ? start / length
                : 0;
            int skip = start * length ?? 0;
            ClientWrapper clientWrapper = new ClientWrapper(userData);
            List<ClientMessageModel> sList = clientWrapper.MessageSelectedClientDetailsChunkSearch
                (order, ClientCustomId, length ?? 0, orderDir, skip);
            var totalRecords = 0;
            var recordsFiltered = 0;
            recordsFiltered = sList.Select(x => x.TotalCount).FirstOrDefault();
            totalRecords = sList.Select(x => x.TotalCount).FirstOrDefault();
            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = sList }, JsonSerializer12HoursDateAndTimeUptoMinuteSettings);
        }


        #region Add Edit MessageSelectedClient
        public PartialViewResult AddEditMessageSelectedClientView(long ClientMessageId, long ClientId)
        {
            var viewModel = new ClientVM();
            var clientMessageModel = new ClientMessageModel();
            string ViewPath = "";
            if (ClientMessageId > 0)
            {
                var clientWrapper = new ClientWrapper(userData);

                clientMessageModel = clientWrapper.RetrieveMessageSelectedClient(ClientMessageId, ClientId);
                clientMessageModel.ClientId = ClientId;
                ViewPath = "~/Views/Clients/_EditMessageSelectedClient.cshtml";
            }
            else
            {
                clientMessageModel.ClientMessageId = ClientMessageId;
                clientMessageModel.ClientId = ClientId;
                ViewPath = "~/Views/Clients/_AddMessageSelectedClient.cshtml";
            }
            viewModel._ClientMessage = clientMessageModel;
            LocalizeControls(viewModel, LocalizeResourceSetConstants.ClientDetails);
            return PartialView(ViewPath, viewModel);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public JsonResult AddEditMessageSelectedClient(ClientVM model)
        {
            if (ModelState.IsValid)
            {
                ClientMessage clientMessage = new ClientMessage();
                ClientWrapper clientWrapper = new ClientWrapper(userData);
                string Mode = string.Empty;
                if (model._ClientMessage.ClientMessageId == 0)
                {
                    Mode = "add";
                    clientMessage = clientWrapper.AddMessageSelectedClient(model._ClientMessage);
                }
                else
                {
                    Mode = "edit";
                    clientMessage = clientWrapper.EditMessageSelectedClient(model._ClientMessage);
                }
                if (clientMessage.ErrorMessages != null && clientMessage.ErrorMessages.Count > 0)
                {
                    return Json(clientMessage.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), mode = Mode }, JsonRequestBehavior.AllowGet);
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





        public string GetMessageAllClientGridData(int? draw, int? start, long ClientCustomId = 0, int? length = 0)
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            start = start.HasValue
                ? start / length
                : 0;
            int skip = start * length ?? 0;
            ClientWrapper clientWrapper = new ClientWrapper(userData);
            List<ClientMessageModel> sList = clientWrapper.MessageSelectedClientDetailsChunkSearch
                (order, ClientCustomId, length ?? 0, orderDir, skip);
            var totalRecords = 0;
            var recordsFiltered = 0;
            recordsFiltered = sList.Select(x => x.TotalCount).FirstOrDefault();
            totalRecords = sList.Select(x => x.TotalCount).FirstOrDefault();
            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = sList }, JsonSerializer12HoursDateAndTimeUptoMinuteSettings);
        }

        #region Add Edit MessageAllClient
        public PartialViewResult AddEditMessageAllClientView(long ClientMessageId, long ClientId)
        {
            var viewModel = new ClientVM();
            var clientMessageModel = new ClientMessageModel();
            string ViewPath = "";
            if (ClientMessageId > 0)
            {
                var clientWrapper = new ClientWrapper(userData);

                clientMessageModel = clientWrapper.RetrieveMessageSelectedClient(ClientMessageId, ClientId);
                clientMessageModel.ClientId = ClientId;
                ViewPath = "~/Views/Clients/_EditMessageAllClient.cshtml";
            }
            else
            {
                clientMessageModel.ClientMessageId = ClientMessageId;
                clientMessageModel.ClientId = ClientId;
                ViewPath = "~/Views/Clients/_AddMessageAllClient.cshtml";
            }
            viewModel._ClientMessage = clientMessageModel;
            LocalizeControls(viewModel, LocalizeResourceSetConstants.ClientDetails);
            return PartialView(ViewPath, viewModel);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public JsonResult AddEditMessageAllClient(ClientVM model)
        {
            if (ModelState.IsValid)
            {
                ClientMessage clientMessage = new ClientMessage();
                ClientWrapper clientWrapper = new ClientWrapper(userData);
                string Mode = string.Empty;
                if (model._ClientMessage.ClientMessageId == 0)
                {
                    Mode = "add";
                    clientMessage = clientWrapper.AddMessageAllClient(model._ClientMessage);
                }
                else
                {
                    Mode = "edit";
                    clientMessage = clientWrapper.EditMessageAllClient(model._ClientMessage);
                }
                if (clientMessage.ErrorMessages != null && clientMessage.ErrorMessages.Count > 0)
                {
                    return Json(clientMessage.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), mode = Mode }, JsonRequestBehavior.AllowGet);
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
    }

}
