using Client.ActionFilters;
using Client.BusinessWrapper.Configuration.EquipmentMaster;
using Client.Common;
using Client.Controllers.Common;
using Client.Models.Configuration.EquipmentMaster;
using Common.Constants;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Client.Controllers.Configuration.EquipmentMaster
{
    public class EquipmentMasterController : ConfigBaseController
    {
        #region Search
        [CheckUserSecurity(securityType = SecurityConstants.EquipmentMaster)]
        public ActionResult Index()
        {
            EquipmentMasterVM equipmentMasterVM = new EquipmentMasterVM();
            equipmentMasterVM.security = this.userData.Security;
            LocalizeControls(equipmentMasterVM, LocalizeResourceSetConstants.ConfigMasterDetail);
            return View("~/Views/Configuration/EquipmentMaster/index.cshtml", equipmentMasterVM);
        }
        private List<EquipmentMasterModel> GetEqMasterSearchResult(List<EquipmentMasterModel> EquipmentMasterModelList, string name, string make, string model, string type)
        {
            if (EquipmentMasterModelList != null)
            {
                if (!string.IsNullOrEmpty(name))
                {
                    name = name.ToUpper();
                    EquipmentMasterModelList = EquipmentMasterModelList.Where(x => (!string.IsNullOrWhiteSpace(x.Name) && x.Name.ToUpper().Contains(name))).ToList();
                }
                if (!string.IsNullOrEmpty(make))
                {
                    make = make.ToUpper();
                    EquipmentMasterModelList = EquipmentMasterModelList.Where(x => (!string.IsNullOrWhiteSpace(x.Make) && x.Make.ToUpper().Contains(make))).ToList();
                }
                if (!string.IsNullOrEmpty(model))
                {
                    model = model.ToUpper();
                    EquipmentMasterModelList = EquipmentMasterModelList.Where(x => (!string.IsNullOrWhiteSpace(x.Model) && x.Model.ToUpper().Contains(model))).ToList();
                }
                if (!string.IsNullOrEmpty(type))
                {
                    type = type.ToUpper();
                    EquipmentMasterModelList = EquipmentMasterModelList.Where(x => (!string.IsNullOrWhiteSpace(x.Type) && x.Type.ToUpper().Contains(type))).ToList();
                }
            }
            return EquipmentMasterModelList;
        }
        [HttpPost]
        public string GetEquipmentMasterGrid(int? draw, int? start, int? length, string name = "", string make = "", string model = "", string type = "", bool inactiveFlag = false)
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            var colname = Request.Form.GetValues("columns[" + order + "][name]");
            EquipmentMasterWrapper emWrapper = new EquipmentMasterWrapper(userData);
            var EqMasterList = emWrapper.GetEquipmentMasterData(inactiveFlag);
            if (EqMasterList != null)
            {
                EqMasterList = GetEqMasterSearchResult(EqMasterList, name, make, model, type);
                EqMasterList = this.GetEquipMastGridSortByColumnWithOrder(colname[0], orderDir, EqMasterList);
            }
            var totalRecords = 0;
            var recordsFiltered = 0;
            start = start.HasValue
                ? start / length
                : 0;
            recordsFiltered = EqMasterList.Count();
            totalRecords = EqMasterList.Count();
            int initialPage = start.Value;
            var filteredResult = EqMasterList
                .Skip(initialPage * length ?? 0)
                .Take(length ?? 0)
                .ToList();
            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult }, JsonSerializerDateSettings);
        }
        private List<EquipmentMasterModel> GetEquipMastGridSortByColumnWithOrder(string order, string orderDir, List<EquipmentMasterModel> data)
        {
            List<EquipmentMasterModel> lst = new List<EquipmentMasterModel>();
            if (data != null)
            {
                switch (order)
                {
                    case "0":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Name).ToList() : data.OrderBy(p => p.Name).ToList();
                        break;
                    case "1":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Make).ToList() : data.OrderBy(p => p.Make).ToList();
                        break;
                    case "2":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Model).ToList() : data.OrderBy(p => p.Model).ToList();
                        break;
                    case "3":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Type).ToList() : data.OrderBy(p => p.Type).ToList();
                        break;
                    default:
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Name).ToList() : data.OrderBy(p => p.Name).ToList();
                        break;
                }
            }
            return lst;
        }

        public string GetEqpMasterPrintData(string colname, string coldir, string name = "", string make = "", string model = "", string type = "", bool inactiveFlag = false)
        {
            List<EquipmentMasterPrintModel> EquipmentMasterPrintModelList = new List<EquipmentMasterPrintModel>();
            EquipmentMasterPrintModel objEquipmentMasterPrintModel;
            EquipmentMasterWrapper emWrapper = new EquipmentMasterWrapper(userData);
            var EqMasterList = emWrapper.GetEquipmentMasterData(inactiveFlag);
            if (EqMasterList != null)
            {                
                EqMasterList = this.GetEqMasterSearchResult(EqMasterList, name, make, model, type);
                EqMasterList = this.GetEquipMastGridSortByColumnWithOrder(colname, coldir, EqMasterList);
                foreach (var p in EqMasterList)
                {
                    objEquipmentMasterPrintModel = new EquipmentMasterPrintModel();
                    objEquipmentMasterPrintModel.Name = p.Name;
                    objEquipmentMasterPrintModel.Make = p.Make;
                    objEquipmentMasterPrintModel.Model = p.Model;
                    objEquipmentMasterPrintModel.Type = p.Type;
                    objEquipmentMasterPrintModel.InactiveFlag = p.InactiveFlag;
                    EquipmentMasterPrintModelList.Add(objEquipmentMasterPrintModel);
                }
            }
            return JsonConvert.SerializeObject(new { data = EquipmentMasterPrintModelList }, JsonSerializerDateSettings);
        }
        #endregion Search

        #region Details
        public PartialViewResult EquipmentMasterDetails(long equipmentMasterId)
        {
            EquipmentMasterVM equipmentMasterVM = new EquipmentMasterVM();
            EquipmentMasterWrapper emWrapper = new EquipmentMasterWrapper(userData);
            EquipmentMasterModel equipmentMasterModel = new EquipmentMasterModel();
            equipmentMasterModel = emWrapper.GetEquipmentMasterDetails(equipmentMasterId);
            equipmentMasterVM.EquipmentMasterModel = equipmentMasterModel;
            equipmentMasterVM.security = this.userData.Security;//.EquipmentMaster.Edit

            LocalizeControls(equipmentMasterVM, LocalizeResourceSetConstants.ConfigMasterDetail);
            return PartialView("~/Views/Configuration/EquipmentMaster/_EquipmentMasterDetails.cshtml", equipmentMasterVM);
        }
        #endregion Details

        #region Add_Edit
        public ActionResult AddEquipmentMaster()
        {
            EquipmentMasterVM equipmentMasterVM = new EquipmentMasterVM();
            EquipmentMasterModel equipmentMasterModel = new EquipmentMasterModel();
            EquipmentMasterWrapper emWrapper = new EquipmentMasterWrapper(userData);
            List<DataContracts.LookupList> Type = new List<DataContracts.LookupList>();
            var typeList = emWrapper.GetTypeList();
            if (typeList != null)
            {
                equipmentMasterModel.TypeList = typeList.Select(x => new SelectListItem { Text = x.ListValue.ToString() + "-" + x.Description, Value = x.ListValue.ToString() });
            }
            equipmentMasterVM.EquipmentMasterModel = equipmentMasterModel;
            LocalizeControls(equipmentMasterVM, LocalizeResourceSetConstants.ConfigMasterDetail);
            return PartialView("~/Views/Configuration/EquipmentMaster/_AddOrEditEquipmentMaster.cshtml", equipmentMasterVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddEquipmentMaster(EquipmentMasterVM objVM, string Command)
        {
            if (ModelState.IsValid)
            {
                EquipmentMasterWrapper emWrapper = new EquipmentMasterWrapper(userData);
                string Mode = string.Empty;
                long equpMasterId = 0;
                List<String> errorList = emWrapper.AddOrEditEqpMaster(objVM.EquipmentMasterModel, ref Mode, ref equpMasterId);
                if (errorList != null && errorList.Count > 0)
                {
                    return Json(errorList, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), EquipmentMasterId = equpMasterId, Command = Command, mode = Mode }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                string ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult EditEquipmentMaster(long equipmentMasterId, string name, string type, string model, string make,  bool inactiveFlag)
        //public ActionResult EditEquipmentMaster(long equipmentMasterId)
        {
            EquipmentMasterVM equipmentMasterVM = new EquipmentMasterVM();
            EquipmentMasterWrapper emWrapper = new EquipmentMasterWrapper(userData);
            EquipmentMasterModel equipmentMasterModel = new EquipmentMasterModel()
            {
                EquipmentMasterId = equipmentMasterId,
                Name = name,
                Type = type,               
                Model = model,
                Make = make,
                InactiveFlag = inactiveFlag
            };
            List<DataContracts.LookupList> Type = new List<DataContracts.LookupList>();
            var typeList = emWrapper.GetTypeList();
            if (typeList != null)
            {
                equipmentMasterModel.TypeList = typeList.Select(x => new SelectListItem { Text = x.ListValue.ToString() + "-" + x.Description, Value = x.ListValue.ToString() });
            }
            equipmentMasterVM.EquipmentMasterModel = equipmentMasterModel;
            LocalizeControls(equipmentMasterVM, LocalizeResourceSetConstants.ConfigMasterDetail);
            return PartialView("~/Views/Configuration/EquipmentMaster/_AddOrEditEquipmentMaster.cshtml", equipmentMasterVM);
        }
        #endregion Add_Edit

        #region Preventive Maintenance
        [HttpPost]
        public string PopulatePM(int? draw, int? start, int? length, long equipmentMasterId)
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            EquipmentMasterWrapper emWrapper = new EquipmentMasterWrapper(userData);
            var pmList = emWrapper.GetPmData( equipmentMasterId);
            pmList = this.GetAllPMsSortByColumnWithOrder(order, orderDir, pmList);
            var totalRecords = 0;
            var recordsFiltered = 0;
            start = start.HasValue
                ? start / length
                : 0;
            recordsFiltered = pmList.Count();
            totalRecords = pmList.Count();
            int initialPage = start.Value;
            var filteredResult = pmList
                .Skip(initialPage * length ?? 0)
                .Take(length ?? 0)
                .ToList();

            bool showBtn = userData.Security.EquipmentMaster.Edit;
           // bool showEditBtn = false;// userData.Security.EquipmentMaster.Edit; //---edit button visibility has been made disabled in V1 also
            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult, showBtn = showBtn});
        }
        private List<EquipmentMasterPmModel> GetAllPMsSortByColumnWithOrder(string order, string orderDir, List<EquipmentMasterPmModel> data)
        {
            List<EquipmentMasterPmModel> lst = new List<EquipmentMasterPmModel>();
            if (data != null)
            {
                switch (order)
                {
                    case "0":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ClientLookupId).ToList() : data.OrderBy(p => p.ClientLookupId).ToList();
                        break;
                    case "1":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Description).ToList() : data.OrderBy(p => p.Description).ToList();
                        break;
                    case "2":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Frequency).ToList() : data.OrderBy(p => p.Frequency).ToList();
                        break;
                    case "3":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.FrequencyType).ToList() : data.OrderBy(p => p.FrequencyType).ToList();
                        break;
                     default:
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ClientLookupId).ToList() : data.OrderBy(p => p.ClientLookupId).ToList();
                        break;
                }
            }
            return lst;
        }
        [HttpPost]
        public ActionResult DeletePm(long eQMaster_PMLibraryId)
        {
            EquipmentMasterWrapper emWrapper = new EquipmentMasterWrapper(userData);
            if (emWrapper.DeleteEqPm(eQMaster_PMLibraryId))
            {
                return Json(new { Result = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Result = JsonReturnEnum.failed.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult AddPM(long eQMasterId, string name)
        {
            EquipmentMasterVM equipmentMasterVM = new EquipmentMasterVM();
            EquipmentMasterPmModel equipmentMasterPmModel = new EquipmentMasterPmModel();
            EquipmentMasterWrapper emWrapper = new EquipmentMasterWrapper(userData);
            equipmentMasterPmModel.EQMasterId = eQMasterId;
            equipmentMasterPmModel.Name = name;
            equipmentMasterVM.EquipmentMasterPmModel = equipmentMasterPmModel;
            equipmentMasterVM.security = this.userData.Security;

            LocalizeControls(equipmentMasterVM, LocalizeResourceSetConstants.ConfigMasterDetail);
            return PartialView("~/Views/Configuration/EquipmentMaster/_AddOrEditPM.cshtml", equipmentMasterVM);
        }

        public string GetPmLookupList(int? draw, int? start, int? length, string ClientLookupId = "", string Description = "")
        {
            List<EqMastPMGridDataModel> model = new List<EqMastPMGridDataModel>();
            EquipmentMasterWrapper emWrapper = new EquipmentMasterWrapper(userData);
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];

            start = start.HasValue
              ? start / length
              : 0;
            int skip = start * length ?? 0;

            ClientLookupId = !string.IsNullOrEmpty(ClientLookupId) ? ClientLookupId.Trim() : string.Empty;
            Description = !string.IsNullOrEmpty(Description) ? Description.Trim() : string.Empty;
           

            model = emWrapper.PopulatePMIds(start.Value, length.Value, order, orderDir, ClientLookupId, Description);
            long totalRecords = 0;
            long recordsFiltered = 0;
            if (model != null && model.Count > 0)
            {
                totalRecords = model[0].TotalCount;
                recordsFiltered = model[0].TotalCount;
            }

            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = model });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddPM(EquipmentMasterVM objVM) 
        {
            string ModelValidationFailedMessage = string.Empty;
           
            if (ModelState.IsValid)
            {
                string Mode = string.Empty;
                EquipmentMasterWrapper emWrapper = new EquipmentMasterWrapper(userData);
                List<String> errorList = emWrapper.AddEqPm(objVM.EquipmentMasterPmModel, ref Mode);
                if (errorList != null && errorList.Count > 0)
                {
                    return Json(errorList, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), EQMasterId = objVM.EquipmentMasterPmModel.EQMasterId, mode = Mode }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult EditPM(long eQMasterId, string name,long eQMaster_PMLibraryId)
        {
            EquipmentMasterVM equipmentMasterVM = new EquipmentMasterVM();
            EquipmentMasterPmModel equipmentMasterPmModel = new EquipmentMasterPmModel();
            EquipmentMasterWrapper emWrapper = new EquipmentMasterWrapper(userData);
            equipmentMasterPmModel.EQMasterId = eQMasterId;
            equipmentMasterPmModel.Name = name;
            equipmentMasterPmModel.EQMaster_PMLibraryId = eQMaster_PMLibraryId;
            equipmentMasterVM.EquipmentMasterPmModel = equipmentMasterPmModel;
            equipmentMasterVM.security = this.userData.Security;


            LocalizeControls(equipmentMasterVM, LocalizeResourceSetConstants.ConfigMasterDetail);
            return PartialView("~/Views/Configuration/EquipmentMaster/_AddOrEditPM.cshtml", equipmentMasterVM);
        }
        #endregion Preventive Maintenance
    }
}
