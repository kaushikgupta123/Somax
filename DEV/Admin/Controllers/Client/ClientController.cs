using Admin.BusinessWrapper;
using Admin.BusinessWrapper.Client;
using Admin.Common;
using Admin.Controllers.Common;
using Admin.Models;
using Admin.Models.Client;

using Common.Constants;

using Newtonsoft.Json;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
namespace Admin.Controllers.Client
{
    public class ClientController : SomaxBaseController
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
            var StatusList = UtilityFunction.InactiveActiveStatusTypes();
            if (StatusList != null)
            {
                ClientVM.InactiveFlagList = StatusList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
            }
            LocalizeControls(ClientVM, LocalizeResourceSetConstants.ClientDetails);
            return View(ClientVM);
        }
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public string GetClientGridData(int? draw, int? start, int? length, int customQueryDisplayId, long ClientId = 0, string Name = "", string Contact = "", string Mail = "", string SearchText = "", string order = "", string orderDir = "")
        {
            List<ClientSearchModel> ClientSearchModelList = new List<ClientSearchModel>();
            SearchText = SearchText.Replace("%", "[%]");
            Name = Name.Replace("%", "[%]");
            Contact = Contact.Replace("%", "[%]");
            Mail = Mail.Replace("%", "[%]");
            start = start.HasValue
                  ? start / length
                  : 0;
            int skip = start * length ?? 0;
            ClientWrapper clientWrapper = new ClientWrapper(userData);
            clientWrapper.userData = userData;
            List<ClientSearchModel> ClientList = clientWrapper.GetClientGridData(customQueryDisplayId, order, orderDir, skip, length ?? 0, ClientId, Name, Contact, Mail, SearchText);
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
        #endregion
        #region Client Details
        public PartialViewResult ClientDetails(long ClientId)
        {
            ClientVM clientVM = new ClientVM();
            ClientWrapper ClientWrapper = new ClientWrapper(userData);
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            var Localizationslist = commonWrapper.GetListFromConstVals(LookupListConstants.LocalizationTypes);
            var ClientDetails = ClientWrapper.GetClientDetailsById(ClientId);          
            CommonWrapper objCommonWrapper = new CommonWrapper(userData);
            clientVM.ClientModel = ClientDetails;
            var localizationvalue = clientVM.ClientModel.Localization;
            clientVM.ClientModel.LocalizationName = Localizationslist.Where(x => x.value == localizationvalue).Select(x => x.text).SingleOrDefault();
            clientVM.ClientModel.ClientId = ClientId;
            LocalizeControls(clientVM, LocalizeResourceSetConstants.ClientDetails);
            return PartialView("_ClientDetails", clientVM);
        }
        #endregion
        #region Print
        public string GetClientPrintData(string _colname, string _coldir, int? draw, int? start, int? length, int customQueryDisplayId = 1, long _ClientId = 0, string _Name = "", string _Contact = "", string _Mail = "", string _searchText = "")
        {
            ClientPrintModel objClientPrintModel;
            List<ClientPrintModel> ClientPrintModelList = new List<ClientPrintModel>();
            _searchText = _searchText.Replace("%", "[%]");
            _Name = _Name.Replace("%", "[%]");
            _Contact = _Contact.Replace("%", "[%]");
            _Mail = _Mail.Replace("%", "[%]");
            start = start.HasValue
               ? start / length
               : 0;
            int skip = start * length ?? 0;
            int lengthForPrint = 100000;
            ClientWrapper clientWrapper = new ClientWrapper(userData);
            clientWrapper.userData = userData; 
            List<ClientSearchModel> ClientSearchList = clientWrapper.GetClientGridData(customQueryDisplayId, _colname, _coldir, 0, lengthForPrint, _ClientId, _Name, _Contact, _Mail, _searchText);
            foreach (var item in ClientSearchList)
            {
                objClientPrintModel = new ClientPrintModel();
                objClientPrintModel.ClientId = item.ClientId;
                objClientPrintModel.Name = item.Name;
                objClientPrintModel.Contact = item.Contact;
                objClientPrintModel.Email = item.Email;
                objClientPrintModel.BusinessType = item.BusinessType;
                objClientPrintModel.PackageLevel = item.PackageLevel;
                objClientPrintModel.CreateDate = item.CreateDate;
                ClientPrintModelList.Add(objClientPrintModel);
            }
            return JsonConvert.SerializeObject(new { data = ClientPrintModelList }, JsonSerializerDateSettings);
        }

        #endregion
        #region Add 
        public PartialViewResult AddClient()
        {
            ClientWrapper ClientWrapper = new ClientWrapper(userData);
            ClientVM ClientVM = new ClientVM();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            List<DataContracts.LookupList> LocalizationTypeList = new List<DataContracts.LookupList>();
            List<DropDownModel> BusinessTypeList=new List<DropDownModel>();
            List<DropDownModel> packagelevelList = new List<DropDownModel>();
            List<DropDownModel> TimeZonelist = new List<DropDownModel>();
            List<DropDownModel> Localizationslist = new List<DropDownModel>();
            ClientVM.ClientModel = new ClientModel();

            Task[] tasks = new Task[4];
            tasks[0] = Task.Factory.StartNew(() => BusinessTypeList = commonWrapper.GetListFromConstVals(LookupListConstants.Client_BusinessType));
            tasks[1] = Task.Factory.StartNew(() => packagelevelList = commonWrapper.GetListFromConstVals(LookupListConstants.Client_PackageLevel));
            tasks[2] = Task.Factory.StartNew(() => TimeZonelist = commonWrapper.GetListFromConstVals(LookupListConstants.TimeZoneList));
            tasks[3] = Task.Factory.StartNew(() => Localizationslist = commonWrapper.GetListFromConstVals(LookupListConstants.LocalizationTypes));
            Task.WaitAll(tasks);
            var StatusList = UtilityFunction.InactiveActiveStatusTypes();

            if (!tasks[0].IsFaulted && tasks[0].IsCompleted && BusinessTypeList != null && BusinessTypeList.Count > 0)
            {
                ClientVM.ClientModel.BusinessTypeList = BusinessTypeList.Select(x => new SelectListItem { Text = x.text.ToString(), Value = x.value.ToString() });
            }
            if (!tasks[1].IsFaulted && tasks[1].IsCompleted && packagelevelList != null && packagelevelList.Count > 0)
            {
                ClientVM.ClientModel.PackagelevelList = packagelevelList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
            }
            if (!tasks[2].IsFaulted && tasks[2].IsCompleted && TimeZonelist != null && TimeZonelist.Count > 0) 
            {
                ClientVM.TimeZonelist = TimeZonelist.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
            }
            if (!tasks[3].IsFaulted && tasks[3].IsCompleted && Localizationslist != null && Localizationslist.Count > 0) 
            {
                ClientVM.LocalizationsList = Localizationslist.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() }).OrderBy(x => x.Text);
            }
            if (StatusList != null && StatusList.Count > 0) 
            {
                ClientVM.InactiveFlagList = StatusList.Select(x => new SelectListItem { Text = x.text, Value = x.text.ToString() });
            }            
            
            LocalizeControls(ClientVM, LocalizeResourceSetConstants.ClientDetails);
            return PartialView("~/Views/Client/_ClientAdd.cshtml", ClientVM);
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
                string constr = objClient.ClientModel.ConnectionString;                
                DataContracts.Client client = new DataContracts.Client();                             
                if (!string.IsNullOrEmpty(constr))
                {
                    if (client.CheckIsClientConnectionStringValid(constr) == true)
                    {
                        client = ClientWrapper.AddorEditClient(objClient);
                    }
                    else
                    {
                        ConnectionStringValidationFailedMessage= UtilityFunction.GetMessageFromResource("GlobalConnectionStringValidationMsg", LocalizeResourceSetConstants.Global);
                        return Json(ConnectionStringValidationFailedMessage, JsonRequestBehavior.AllowGet);
                    }
                }
                if (client.ErrorMessages != null && client.ErrorMessages.Count > 0)
                {
                    return Json(client.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), Command = Command, ClientId = client.CreatedClientId,Status= client.Status }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
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
        #region Edit
        [HttpPost]
        public ActionResult EditClient(long ClientId)
        {
            ClientWrapper ClientWrapper = new ClientWrapper(userData);
            ClientVM ClientVM = new ClientVM();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            List<DataContracts.LookupList> LocalizationTypeList = new List<DataContracts.LookupList>();
            ClientVM.ClientModel = new ClientModel();
            List<DropDownModel> BusinessTypeList = new List<DropDownModel>();
            List<DropDownModel> packagelevelList = new List<DropDownModel>();
            List<DropDownModel> TimeZonelist = new List<DropDownModel>();
            List<DropDownModel> Localizationslist = new List<DropDownModel>();

            Task[] tasks = new Task[4];
            tasks[0] = Task.Factory.StartNew(() => BusinessTypeList = commonWrapper.GetListFromConstVals(LookupListConstants.Client_BusinessType));
            tasks[1] = Task.Factory.StartNew(() => packagelevelList = commonWrapper.GetListFromConstVals(LookupListConstants.Client_PackageLevel));
            tasks[2] = Task.Factory.StartNew(() => TimeZonelist = commonWrapper.GetListFromConstVals(LookupListConstants.TimeZoneList));
            tasks[3] = Task.Factory.StartNew(() => Localizationslist = commonWrapper.GetListFromConstVals(LookupListConstants.LocalizationTypes));
            Task.WaitAll(tasks);
            var StatusList = UtilityFunction.InactiveActiveStatusTypes();
           
            ClientVM.ClientModel = ClientWrapper.GetClientDetailsById(ClientId);
            if (!tasks[0].IsFaulted && tasks[0].IsCompleted && BusinessTypeList != null && BusinessTypeList.Count > 0)
            {
                ClientVM.ClientModel.BusinessTypeList = BusinessTypeList.Select(x => new SelectListItem { Text = x.text.ToString(), Value = x.value.ToString() });
            }
            if (!tasks[1].IsFaulted && tasks[1].IsCompleted && packagelevelList != null && packagelevelList.Count > 0) 
            {
                ClientVM.ClientModel.PackagelevelList = packagelevelList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
            }
            if (!tasks[2].IsFaulted && tasks[2].IsCompleted && TimeZonelist != null && TimeZonelist.Count > 0) 
            {
                ClientVM.TimeZonelist = TimeZonelist.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
            }
            if (!tasks[3].IsFaulted && tasks[3].IsCompleted && Localizationslist != null && Localizationslist.Count > 0) 
            {
                ClientVM.LocalizationsList = Localizationslist.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
            }            
            if (StatusList != null && StatusList.Count > 0) 
            {
                ClientVM.InactiveFlagList = StatusList.Select(x => new SelectListItem { Text = x.text, Value = x.text.ToString() });
            }
            LocalizeControls(ClientVM, LocalizeResourceSetConstants.ClientDetails);
            return PartialView("~/Views/Client/_ClientEdit.cshtml", ClientVM);            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateClient(ClientVM ClientVM)
        {
            string emptyValue = string.Empty;            
            ClientWrapper ClientWrapper = new ClientWrapper(userData);
            DataContracts.Client client = new DataContracts.Client();
            string constr =ClientVM.ClientModel.ConnectionString;
            string ConnectionStringValidationFailedMessage = string.Empty;            
            if (ModelState.IsValid)
            {   
                if (!string.IsNullOrEmpty(constr))
                {
                    if (client.CheckIsClientConnectionStringValid(constr) == true)
                    {
                        client = ClientWrapper.AddorEditClient(ClientVM);                       
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
    }

}
