using Client.ActionFilters;
using Client.BusinessWrapper.Common;
using Client.BusinessWrapper.PrevMaintWrapper;
using Client.Common;
using Client.Controllers.Common;
using Client.Localization;
using Client.Models;
using Client.Models.Common;
using Client.Models.PartLookup;
using Client.Models.PreventiveMaintenance;
using Client.Models.PreventiveMaintenance.UIConfiguration;
using Common.Constants;
using Common.Extensions;
using DataContracts;
using Newtonsoft.Json;
using Rotativa;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Client.Controllers.Maintenance
{
    public class PreventiveMaintenanceController : SomaxBaseController
    {
        #region Search
        [CheckUserSecurity(securityType = SecurityConstants.PrevMaint)]
        public ActionResult Index()
        {
            PrevMaintVM objPrevMaintVM = new PrevMaintVM();
            PreventiveMaintenanceModel pmModel = new PreventiveMaintenanceModel();
            PreventiveMaitenanceWOModel pmWoModel = new PreventiveMaitenanceWOModel();
            PrevMaintReassignModel prevMaintReassignModel = new PrevMaintReassignModel();
            PreventiveMaintenanceOptionModel prevMaintOptionModel = new PreventiveMaintenanceOptionModel();
            PrevMaintWrapper pWrapper = new PrevMaintWrapper(userData);
            string mode = Convert.ToString(TempData["Mode"]);
            if (mode == "DetailFromEquipment")
            {
                long PrevMaintMasterId = Convert.ToInt64(TempData["PrevMaintMasterId"]);
                #region Securities
                ViewBag.SecuritySchedule = userData.Security.PrevMaint.Edit;
                ViewBag.SecurityTask = userData.Security.PrevMaint.Edit;
                ViewBag.SecurityEstimatePart = userData.Security.PrevMaint.Edit;
                ViewBag.SecurityEstimateLabor = userData.Security.PrevMaint.Edit;
                ViewBag.SecurityEstimateOther = userData.Security.PrevMaint.Edit;
                objPrevMaintVM.security = userData.Security;
                #endregion
                #region  options
                var PrevMaintCreate = userData.Security.PrevMaint.Create;
                var PrevMaintDelete = userData.Security.PrevMaint.Delete;
                prevMaintOptionModel.IsPrevMaintCreate = PrevMaintCreate;
                prevMaintOptionModel.IsPrevMaintDelete = PrevMaintDelete;
                objPrevMaintVM.prevMaintOptionModel = prevMaintOptionModel;
                #endregion             

                objPrevMaintVM.createdLastUpdatedPMModel = pWrapper.createdLastUpdatedModel(PrevMaintMasterId);
                var prevDetail = pWrapper.populateMaintenanceDetails(PrevMaintMasterId);
                objPrevMaintVM.preventiveMaintenanceModel = prevDetail;
                #region V2-919
                long EquipmentId = Convert.ToInt64(TempData["EquipmentId"]);
                string EquipmentClientLookupId = Convert.ToString(TempData["EquipmentClientLookupId"]);
                ChangePreventiveIDModel _ChangePreventiveIDModel = new ChangePreventiveIDModel();
                Task attTask;
                CommonWrapper objCommonWrapper = new CommonWrapper(userData);
                attTask = Task.Factory.StartNew(() => objPrevMaintVM.attachmentCount = objCommonWrapper.AttachmentCount(PrevMaintMasterId, AttachmentTableConstant.PreventiveMaintenance, userData.Security.PrevMaint.Edit));
                ViewBag.UseVendorMaster = userData.Site.UseVendorMaster; attTask.Wait();
                _ChangePreventiveIDModel.PrevMaintMasterId = objPrevMaintVM.preventiveMaintenanceModel.PrevMaintMasterId;
                _ChangePreventiveIDModel.ClientLookupId = objPrevMaintVM.preventiveMaintenanceModel.ClientLookupId;
                objPrevMaintVM._ChangePreventiveIDModel = _ChangePreventiveIDModel;
                objPrevMaintVM.EquipmentId = EquipmentId;
                objPrevMaintVM.EquipmentClientLookupId = EquipmentClientLookupId;
                #endregion
                objPrevMaintVM.IsFromEquipment = true;
            }
            else
            {
                var ScheduleTypeList = UtilityFunction.GetScheduleType();
                if (ScheduleTypeList != null)
                {
                    pmModel.ScheduleTypeList = ScheduleTypeList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
                    pmWoModel.ScheduleTypeList = ScheduleTypeList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
                }
                var AllLookUps = pWrapper.GetAllLookUpList();
                if (AllLookUps != null)
                {
                    var typeList = AllLookUps.Where(x => x.ListName == LookupListConstants.Preventive_Maint_Type).ToList();
                    if (typeList != null)
                    {
                        pmModel.TypeList = typeList.Select(x => new SelectListItem { Text = x.ListValue + "  -  " + x.Description, Value = x.ListValue });
                    }
                    var OnDemandList = AllLookUps.Where(x => x.ListName == LookupListConstants.Preventive_Maint_Ondemand_Grp).ToList();
                    if (typeList != null)
                    {
                        pmWoModel.OnDemandList = OnDemandList.Select(x => new SelectListItem { Text = x.ListValue + "  -  " + x.Description, Value = x.ListValue });
                    }
                }
                var PrevMaintCreate = userData.Security.PrevMaint.Create;
                var PrevMaintEdit = userData.Security.PrevMaint.Edit;
                var sBusinessTypefacilities = (userData.DatabaseKey.Client.BusinessType.ToUpper() == BusinessTypeConstants.Facilities ? true : false);
                prevMaintOptionModel.IsPrevMaintCreate = PrevMaintCreate;
                prevMaintOptionModel.IsPrevMaintEdit = PrevMaintEdit;
                prevMaintOptionModel.IsBusinessTypefacilities = sBusinessTypefacilities;
                objPrevMaintVM.prevMaintOptionModel = prevMaintOptionModel;
                objPrevMaintVM.prevMaintReassignModel = prevMaintReassignModel;
                objPrevMaintVM.preventiveMaintenanceModel = pmModel;
            }
            pmWoModel.chkGenerate_WorkOrders = userData.Security.PrevMaint.Generate_WorkOrders;
            objPrevMaintVM.preventiveMaitenanceWOModel = pmWoModel;
            objPrevMaintVM.IsPMLibrary = userData.Site.PMLibrary;
            LocalizeControls(objPrevMaintVM, LocalizeResourceSetConstants.PrevMaintDetails);
            return View(objPrevMaintVM);
        }
        public JsonResult GetEquipmentLookUpIds()
        {
            IEnumerable<SelectListItem> items = null;
            var EquipmentLookUpList = ListofAllEquipment();
            if (EquipmentLookUpList != null)
            {
                items = EquipmentLookUpList.Where(a => a.InactiveFlag == false).Select(x => new SelectListItem { Text = x.Equipment + " - " + x.Name, Value = x.EquipmentId.ToString() }).ToList();

            }
            return Json(new { data = items }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetAssignmentLookUpIds()
        {
            IEnumerable<SelectListItem> items = null;
            var PersonnelLookUplist = GetList_Personnel();
            if (PersonnelLookUplist != null)
            {
                items = PersonnelLookUplist.Select(x => new SelectListItem { Text = x.AssignedTo_PersonnelClientLookupId + " - " + x.NameFirst + " - " + x.NameLast, Value = x.AssignedTo_PersonnelId.ToString() });
            }
            return Json(items, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetLocationLookUpIds()
        {
            IEnumerable<SelectListItem> items = null;
            var LocationLookUpList = ListOfLocation();
            if (LocationLookUpList != null)
            {
                items = LocationLookUpList.Select(x => new SelectListItem { Text = x.LocationClientLookupId + " - " + x.Name, Value = x.LocationId.ToString() });
            }
            return Json(items, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public string GetPrevMaintGrid(int? draw, int? start, int? length, string SearchText = "", bool inactiveFlag = false,
        int equipmentId = 0, int locationId = 0, int assignedId = 0, string MasterjobId = "",
        string Description = "", string Type = "", string ScheduleType = "", string ChargeTo = "", string ChargeToName = ""
        , string Order = "1"
            )
        {
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];

            start = start.HasValue
               ? start / length
               : 0;
            int skip = start * length ?? 0;
            PrevMaintWrapper pWrapper = new PrevMaintWrapper(userData);

            var totalRecords = 0;
            var recordsFiltered = 0;

            PreventiveMaintenanceSearchCriteriaModel preventiveMaintenanceSearchCriteriaModel = new PreventiveMaintenanceSearchCriteriaModel();
            preventiveMaintenanceSearchCriteriaModel.EquipmentId = equipmentId;
            preventiveMaintenanceSearchCriteriaModel.LocationId = locationId;
            preventiveMaintenanceSearchCriteriaModel.AssignedId = assignedId;
            if (inactiveFlag)
            {
                preventiveMaintenanceSearchCriteriaModel.CaseNo = 2;
            }
            else
            {
                preventiveMaintenanceSearchCriteriaModel.CaseNo = 1;
            }
            preventiveMaintenanceSearchCriteriaModel.orderbyColumn = Order;
            preventiveMaintenanceSearchCriteriaModel.offset1 = skip;
            preventiveMaintenanceSearchCriteriaModel.nextrow = length ?? 0;
            preventiveMaintenanceSearchCriteriaModel.ClientLookupId = MasterjobId;
            preventiveMaintenanceSearchCriteriaModel.Description = Description;
            preventiveMaintenanceSearchCriteriaModel.ScheduleType = ScheduleType;
            preventiveMaintenanceSearchCriteriaModel.Type = Type;
            preventiveMaintenanceSearchCriteriaModel.Chargeto = ChargeTo;
            preventiveMaintenanceSearchCriteriaModel.ChargetoName = ChargeToName;
            preventiveMaintenanceSearchCriteriaModel.SearchText = SearchText;
            preventiveMaintenanceSearchCriteriaModel.InactiveFlag = inactiveFlag;
            preventiveMaintenanceSearchCriteriaModel.orderBy = orderDir;
            List<PreventiveMaintenanceModel> prevMaintMasterList = pWrapper.populateChunkSearch(preventiveMaintenanceSearchCriteriaModel);

            if (prevMaintMasterList != null && prevMaintMasterList.Count > 0)
            {
                recordsFiltered = prevMaintMasterList[0].TotalCount;
                totalRecords = prevMaintMasterList[0].TotalCount;
            }
            else
            {
                recordsFiltered = 0;
                totalRecords = 0;
            }
            int initialPage = start.Value;

            var filteredResult = prevMaintMasterList.ToList();

            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult }, JsonSerializerDateSettings);
        }
        [HttpGet]


        public string GetPreventiveMaintenancePrintData(string colname, string coldir, string SearchText = "", bool inactiveFlag = false,
             int equipmentId = 0, int locationId = 0, int assignedId = 0, string MasterjobId = "", string Description = "", string Type = "", string ScheduleType = "", string ChargeTo = "", string ChargeToName = "")
        {
            List<PreventiveMaintenancePrintModel> pSearchModelList = new List<PreventiveMaintenancePrintModel>();
            PreventiveMaintenancePrintModel objPreventiveMaintenancePrintModel;
            PrevMaintWrapper pWrapper = new PrevMaintWrapper(userData);
            PreventiveMaintenanceSearchCriteriaModel preventiveMaintenanceSearchCriteriaModel = new PreventiveMaintenanceSearchCriteriaModel();
            preventiveMaintenanceSearchCriteriaModel.EquipmentId = equipmentId;
            preventiveMaintenanceSearchCriteriaModel.LocationId = locationId;
            preventiveMaintenanceSearchCriteriaModel.AssignedId = assignedId;
            if (inactiveFlag)
            {
                preventiveMaintenanceSearchCriteriaModel.CaseNo = 2;
            }
            else
            {
                preventiveMaintenanceSearchCriteriaModel.CaseNo = 1;
            }
            preventiveMaintenanceSearchCriteriaModel.orderbyColumn = colname;
            preventiveMaintenanceSearchCriteriaModel.offset1 = 0;
            preventiveMaintenanceSearchCriteriaModel.nextrow = 100000;
            preventiveMaintenanceSearchCriteriaModel.ClientLookupId = MasterjobId;
            preventiveMaintenanceSearchCriteriaModel.Description = Description;
            preventiveMaintenanceSearchCriteriaModel.ScheduleType = ScheduleType;
            preventiveMaintenanceSearchCriteriaModel.Type = Type;
            preventiveMaintenanceSearchCriteriaModel.Chargeto = ChargeTo;
            preventiveMaintenanceSearchCriteriaModel.ChargetoName = ChargeToName;
            preventiveMaintenanceSearchCriteriaModel.SearchText = SearchText;
            preventiveMaintenanceSearchCriteriaModel.InactiveFlag = inactiveFlag;
            preventiveMaintenanceSearchCriteriaModel.orderBy = coldir;
            List<PreventiveMaintenanceModel> prevMaintMasterList = pWrapper.populateChunkSearch(preventiveMaintenanceSearchCriteriaModel);
            if (prevMaintMasterList != null)
            {
                foreach (var p in prevMaintMasterList)
                {
                    objPreventiveMaintenancePrintModel = new PreventiveMaintenancePrintModel();
                    objPreventiveMaintenancePrintModel.ClientLookupId = p.ClientLookupId;
                    objPreventiveMaintenancePrintModel.ScheduleType = p.ScheduleType;
                    objPreventiveMaintenancePrintModel.Description = p.Description;
                    objPreventiveMaintenancePrintModel.Type = p.Type;
                    pSearchModelList.Add(objPreventiveMaintenancePrintModel);
                }
            }
            return JsonConvert.SerializeObject(new { data = pSearchModelList });
        }

        [HttpPost]
        [EncryptedActionParameter]
        public JsonResult SetPrintData(PMPrintParams pMPrintParams)
        {
            Session["PMPRINTPARAMS"] = pMPrintParams;
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        [NoDirectAccess]
        public ActionResult ExportASPDF()
        {
            PrevMaintVM objPrevMaintVM = new PrevMaintVM();
            PrevMaintWrapper pWrapper = new PrevMaintWrapper(userData);
            PMPDFPrintModel pMPDFPrintModel = new PMPDFPrintModel();
            List<PMPDFPrintModel> pMPDFPrintModelList = new List<PMPDFPrintModel>();
            var locker = new object();
            PMPrintParams pMPrintParams = (PMPrintParams)Session["PMPRINTPARAMS"];
            PreventiveMaintenanceSearchCriteriaModel preventiveMaintenanceSearchCriteriaModel = new PreventiveMaintenanceSearchCriteriaModel();
            preventiveMaintenanceSearchCriteriaModel.EquipmentId = pMPrintParams.EquipmentId;
            preventiveMaintenanceSearchCriteriaModel.LocationId = pMPrintParams.LocationId;
            preventiveMaintenanceSearchCriteriaModel.AssignedId = pMPrintParams.AssignedId;
            if (pMPrintParams.InactiveFlag)
            {
                preventiveMaintenanceSearchCriteriaModel.CaseNo = 2;
            }
            else
            {
                preventiveMaintenanceSearchCriteriaModel.CaseNo = 1;
            }
            preventiveMaintenanceSearchCriteriaModel.orderbyColumn = pMPrintParams.colname;
            preventiveMaintenanceSearchCriteriaModel.offset1 = 0;
            preventiveMaintenanceSearchCriteriaModel.nextrow = 100000;
            preventiveMaintenanceSearchCriteriaModel.ClientLookupId = pMPrintParams.MasterjobId;
            preventiveMaintenanceSearchCriteriaModel.Description = pMPrintParams.Description;
            preventiveMaintenanceSearchCriteriaModel.ScheduleType = pMPrintParams.ScheduleType;
            preventiveMaintenanceSearchCriteriaModel.Type = pMPrintParams.Type;
            preventiveMaintenanceSearchCriteriaModel.Chargeto = pMPrintParams.ChargeTo;
            preventiveMaintenanceSearchCriteriaModel.ChargetoName = pMPrintParams.ChargeToName;
            preventiveMaintenanceSearchCriteriaModel.SearchText = pMPrintParams.SearchText;
            preventiveMaintenanceSearchCriteriaModel.InactiveFlag = pMPrintParams.InactiveFlag;
            preventiveMaintenanceSearchCriteriaModel.orderBy = pMPrintParams.coldir;
            List<PreventiveMaintenanceModel> prevMaintMasterList = pWrapper.populateChunkSearch(preventiveMaintenanceSearchCriteriaModel);
            foreach (var item in prevMaintMasterList)
            {
                pMPDFPrintModel = new PMPDFPrintModel();
                pMPDFPrintModel.ClientLookupId = item.ClientLookupId;
                pMPDFPrintModel.ScheduleType = item.ScheduleType;
                pMPDFPrintModel.Description = item.Description;
                pMPDFPrintModel.Type = item.Type;
                if (item.ChildCount > 0)
                {
                    pMPDFPrintModel.scheduleRecordsList = pWrapper.GetScheduleRecords_V2(item.PrevMaintMasterId);
                }
                lock (locker)
                {
                    pMPDFPrintModelList.Add(pMPDFPrintModel);
                }

            }
            objPrevMaintVM.pMPDFPrintModel = pMPDFPrintModelList;
            objPrevMaintVM.tableHaederProps = pMPrintParams.tableHaederProps;
            LocalizeControls(objPrevMaintVM, LocalizeResourceSetConstants.PrevMaintDetails);
            return new ViewAsPdf("PMGridPdfPrintTemplate", objPrevMaintVM)
            {
                PageSize = Rotativa.Options.Size.A4,
                PageMargins = { Left = 3, Right = 3, Top = 3, Bottom = 3 },
            };

        }

        [NoDirectAccess]
        public ActionResult PrintASPDF()
        {
            PrevMaintVM objPrevMaintVM = new PrevMaintVM();
            PrevMaintWrapper pWrapper = new PrevMaintWrapper(userData);
            PMPDFPrintModel pMPDFPrintModel = new PMPDFPrintModel();
            List<PMPDFPrintModel> pMPDFPrintModelList = new List<PMPDFPrintModel>();
            var locker = new object();

            PMPrintParams pMPrintParams = (PMPrintParams)Session["PMPRINTPARAMS"];
            PreventiveMaintenanceSearchCriteriaModel preventiveMaintenanceSearchCriteriaModel = new PreventiveMaintenanceSearchCriteriaModel();
            preventiveMaintenanceSearchCriteriaModel.EquipmentId = pMPrintParams.EquipmentId;
            preventiveMaintenanceSearchCriteriaModel.LocationId = pMPrintParams.LocationId;
            preventiveMaintenanceSearchCriteriaModel.AssignedId = pMPrintParams.AssignedId;
            if (pMPrintParams.InactiveFlag)
            {
                preventiveMaintenanceSearchCriteriaModel.CaseNo = 2;
            }
            else
            {
                preventiveMaintenanceSearchCriteriaModel.CaseNo = 1;
            }
            preventiveMaintenanceSearchCriteriaModel.orderbyColumn = pMPrintParams.colname;
            preventiveMaintenanceSearchCriteriaModel.offset1 = 0;
            preventiveMaintenanceSearchCriteriaModel.nextrow = 100000;
            preventiveMaintenanceSearchCriteriaModel.ClientLookupId = pMPrintParams.MasterjobId;
            preventiveMaintenanceSearchCriteriaModel.Description = pMPrintParams.Description;
            preventiveMaintenanceSearchCriteriaModel.ScheduleType = pMPrintParams.ScheduleType;
            preventiveMaintenanceSearchCriteriaModel.Type = pMPrintParams.Type;
            preventiveMaintenanceSearchCriteriaModel.Chargeto = pMPrintParams.ChargeTo;
            preventiveMaintenanceSearchCriteriaModel.ChargetoName = pMPrintParams.ChargeToName;
            preventiveMaintenanceSearchCriteriaModel.SearchText = pMPrintParams.SearchText;
            preventiveMaintenanceSearchCriteriaModel.InactiveFlag = pMPrintParams.InactiveFlag;
            preventiveMaintenanceSearchCriteriaModel.orderBy = pMPrintParams.coldir;
            List<PreventiveMaintenanceModel> prevMaintMasterList = pWrapper.populateChunkSearch(preventiveMaintenanceSearchCriteriaModel);
            foreach (var item in prevMaintMasterList)
            {
                pMPDFPrintModel = new PMPDFPrintModel();
                pMPDFPrintModel.ClientLookupId = item.ClientLookupId;
                pMPDFPrintModel.ScheduleType = item.ScheduleType;
                pMPDFPrintModel.Description = item.Description;
                pMPDFPrintModel.Type = item.Type;
                if (item.ChildCount > 0)
                {
                    pMPDFPrintModel.scheduleRecordsList = pWrapper.GetScheduleRecords_V2(item.PrevMaintMasterId);
                }
                lock (locker)
                {
                    pMPDFPrintModelList.Add(pMPDFPrintModel);
                }

            }
            objPrevMaintVM.pMPDFPrintModel = pMPDFPrintModelList;
            objPrevMaintVM.tableHaederProps = pMPrintParams.tableHaederProps;
            LocalizeControls(objPrevMaintVM, LocalizeResourceSetConstants.PrevMaintDetails);

            return new PartialViewAsPdf("PMGridPdfPrintTemplate", objPrevMaintVM)
            {
                PageSize = Rotativa.Options.Size.A4,
                FileName = "PM.pdf",
                PageMargins = { Left = 3, Right = 3, Top = 3, Bottom = 3 },
            };


        }
        public PartialViewResult PrevDetails(long PrevMaintMasterId, bool IsFromEquipment = false)
        {

            PrevMaintVM objPrevMaintVM = new PrevMaintVM();
            PreventiveMaintenanceModel pmModel = new PreventiveMaintenanceModel();
            PrevMaintWrapper pWrapper = new PrevMaintWrapper(userData);
            PreventiveMaintenanceOptionModel prevMaintOptionModel = new PreventiveMaintenanceOptionModel();
            CreatedLastUpdatedPMModel _CreatedLastUpdatedModel = new CreatedLastUpdatedPMModel();
            ChangePreventiveIDModel _ChangePreventiveIDModel = new ChangePreventiveIDModel();
            Task attTask;
            CommonWrapper objCommonWrapper = new CommonWrapper(userData);
            attTask = Task.Factory.StartNew(() => objPrevMaintVM.attachmentCount = objCommonWrapper.AttachmentCount(PrevMaintMasterId, AttachmentTableConstant.PreventiveMaintenance, userData.Security.PrevMaint.Edit));

            #region Securities
            ViewBag.PlannerRequiredForUsePlanning = userData.Site.UsePlanning; //V2-1161
            ViewBag.SecuritySchedule = userData.Security.PrevMaint.Edit;
            ViewBag.SecurityTask = userData.Security.PrevMaint.Edit;
            ViewBag.SecurityEstimatePart = userData.Security.PrevMaint.Edit;
            ViewBag.SecurityEstimateLabor = userData.Security.PrevMaint.Edit;
            ViewBag.SecurityEstimateOther = userData.Security.PrevMaint.Edit;
            ViewBag.SecurityNotes = userData.Security.PrevMaint.Edit;
            objPrevMaintVM.security = userData.Security;
            #endregion

            #region  options
            var PrevMaintCreate = userData.Security.PrevMaint.Create;
            var PrevMaintDelete = userData.Security.PrevMaint.Delete;
            var PrevMaintChangeClientLookupId = userData.Security.PrevMaint.ChangeClientLookupId;
            prevMaintOptionModel.IsPrevMaintCreate = PrevMaintCreate;
            prevMaintOptionModel.IsPrevMaintDelete = PrevMaintDelete;
            prevMaintOptionModel.IsPrevMaintLookupId = PrevMaintChangeClientLookupId;
            objPrevMaintVM.prevMaintOptionModel = prevMaintOptionModel;
            #endregion            
            ViewBag.UseVendorMaster = userData.Site.UseVendorMaster;
            objPrevMaintVM.createdLastUpdatedPMModel = pWrapper.createdLastUpdatedModel(PrevMaintMasterId);
            var prevDetail = pWrapper.populateMaintenanceDetails(PrevMaintMasterId);
            objPrevMaintVM.preventiveMaintenanceModel = prevDetail;
            objPrevMaintVM.IsPMLibrary = userData.Site.PMLibrary;
            attTask.Wait();
            _ChangePreventiveIDModel.PrevMaintMasterId = objPrevMaintVM.preventiveMaintenanceModel.PrevMaintMasterId;
            _ChangePreventiveIDModel.ClientLookupId = objPrevMaintVM.preventiveMaintenanceModel.ClientLookupId;
            objPrevMaintVM._ChangePreventiveIDModel = _ChangePreventiveIDModel;
            objPrevMaintVM.IsFromEquipment = IsFromEquipment;
            objPrevMaintVM.IsUseMultiStoreroom = userData.DatabaseKey.Client.UseMultiStoreroom;
            LocalizeControls(objPrevMaintVM, LocalizeResourceSetConstants.PrevMaintDetails);
            return PartialView("_PreventiveDetail", objPrevMaintVM);
        }

        public RedirectResult DetailFromEquipment(long PrevMaintMasterId, long EquipmentId, string EquipmentClientLookupId)
        {
            TempData["Mode"] = "DetailFromEquipment";
            string PrevMaintMasterString = Convert.ToString(PrevMaintMasterId);
            TempData["PrevMaintMasterId"] = PrevMaintMasterString;
            TempData["EquipmentId"] = EquipmentId;
            TempData["EquipmentClientLookupId"] = EquipmentClientLookupId;
            return Redirect("/PreventiveMaintenance/Index?page=Maintenance_Preventive_Maintenance_Search");
        }
        public ActionResult GetPoInnerGrid(long PrevMasterID)
        {
            PrevMaintVM prevMaintVM = new PrevMaintVM();
            PrevMaintWrapper pWrapper = new PrevMaintWrapper(userData);
            prevMaintVM.scheduleRecordsList = pWrapper.GetScheduleRecords_V2(PrevMasterID);
            LocalizeControls(prevMaintVM, LocalizeResourceSetConstants.PrevMaintDetails);
            return View("_InnerGridPrevMaintSched", prevMaintVM);
        }

        #endregion

        #region Add/Edit
        public PartialViewResult AddPreventive()
        {
            PrevMaintVM objPrevMaintVM = new PrevMaintVM();
            PrevMaintWrapper pmWrapper = new PrevMaintWrapper(userData);
            PreventiveMaintenanceModel pmModel = new PreventiveMaintenanceModel();
            var AllLookUps = pmWrapper.GetAllLookUpList();
            if (AllLookUps != null)
            {
                var typeList = AllLookUps.Where(x => x.ListName == LookupListConstants.Preventive_Maint_Type).ToList();
                if (typeList != null)
                {
                    pmModel.TypeList = typeList.Select(x => new SelectListItem { Text = x.ListValue + "  -  " + x.Description, Value = x.ListValue });
                }
            }

            var ScheduleTypeList = UtilityFunction.GetScheduleType();
            if (ScheduleTypeList != null)
            {
                pmModel.ScheduleTypeList = ScheduleTypeList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
            }
            pmModel.JobDuration = 0;
            objPrevMaintVM.preventiveMaintenanceModel = pmModel;
            LocalizeControls(objPrevMaintVM, LocalizeResourceSetConstants.PrevMaintDetails);
            return PartialView("~/Views/PreventiveMaintenance/_PreventiveAdd.cshtml", objPrevMaintVM);
        }
        public PartialViewResult EditPreventive(long PrevMaintMasterId)
        {
            PrevMaintVM objPrevMaintVM = new PrevMaintVM();
            PrevMaintWrapper pmWrapper = new PrevMaintWrapper(userData);
            var prvMaint = pmWrapper.populateMaintenanceDetails(PrevMaintMasterId);
            var AllLookUps = pmWrapper.GetAllLookUpList();
            if (AllLookUps != null)
            {
                var typeList = AllLookUps.Where(x => x.ListName == LookupListConstants.Preventive_Maint_Type).ToList();
                if (typeList != null)
                {
                    prvMaint.TypeList = typeList.Select(x => new SelectListItem { Text = x.ListValue + "  -  " + x.Description, Value = x.ListValue });
                }
            }
            var ScheduleTypeList = UtilityFunction.GetScheduleType();
            prvMaint.ScheduleTypeList = ScheduleTypeList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
            objPrevMaintVM.preventiveMaintenanceModel = prvMaint;
            LocalizeControls(objPrevMaintVM, LocalizeResourceSetConstants.PrevMaintDetails);
            return PartialView("~/Views/PreventiveMaintenance/_PreventiveAdd.cshtml", objPrevMaintVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddPreventive(PrevMaintVM prevMaintVM, string Command)
        {
            string Mode = string.Empty;
            string ModelValidationFailedMessage = string.Empty;
            if (ModelState.IsValid)
            {
                PrevMaintWrapper pmWrapper = new PrevMaintWrapper(userData);
                PrevMaintMaster objPrevMaintMaster = new DataContracts.PrevMaintMaster();
                if (prevMaintVM.preventiveMaintenanceModel != null && prevMaintVM.preventiveMaintenanceModel.PrevMaintMasterId == 0)
                {
                    Mode = "add";
                    objPrevMaintMaster = pmWrapper.AddPrevMaint(prevMaintVM.preventiveMaintenanceModel);
                }
                else
                {
                    objPrevMaintMaster = pmWrapper.EditPrevMaint(prevMaintVM.preventiveMaintenanceModel);

                }
                if (objPrevMaintMaster.ErrorMessages != null && objPrevMaintMaster.ErrorMessages.Count > 0)
                {
                    return Json(objPrevMaintMaster.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), Command = Command, PrevMaintMasterId = objPrevMaintMaster.PrevMaintMasterId, mode = Mode }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public string PmProcGrid(int? draw, int? start, int? length, string ClientLookupId, decimal? JobDuration, string Description = "",
            string FrequencyType = "", int? Frequency = null)
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            var colname = Request.Form.GetValues("columns[" + order + "][name]");
            PrevMaintWrapper pWrapper = new PrevMaintWrapper(userData);
            List<PreventiveMaintenanceLibraryModel> prevMaintLibList = pWrapper.PopulatePmProcedure();
            if (prevMaintLibList != null)
            {
                prevMaintLibList = this.PmProcGridSortByColumnWithOrder(colname[0], orderDir, prevMaintLibList);
            }
            if (prevMaintLibList != null)
            {
                #region AdvSearch
                if (!string.IsNullOrEmpty(ClientLookupId))
                {
                    ClientLookupId = ClientLookupId.ToUpper();
                    prevMaintLibList = prevMaintLibList.Where(x => (!string.IsNullOrWhiteSpace(x.ClientLookupId) && x.ClientLookupId.ToUpper().Contains(ClientLookupId))).ToList();
                }
                if (!string.IsNullOrEmpty(Description))
                {
                    Description = Description.ToUpper();
                    prevMaintLibList = prevMaintLibList.Where(x => (!string.IsNullOrWhiteSpace(x.Description) && x.Description.ToUpper().Contains(Description))).ToList();
                }
                if (JobDuration != null)
                {
                    prevMaintLibList = prevMaintLibList.Where(x => x.JobDuration.Equals(JobDuration)).ToList();
                }
                if (!string.IsNullOrEmpty(FrequencyType))
                {
                    FrequencyType = FrequencyType.ToUpper();
                    prevMaintLibList = prevMaintLibList.Where(x => (!string.IsNullOrWhiteSpace(x.FrequencyType) && x.FrequencyType.ToUpper().Contains(FrequencyType))).ToList();
                }
                if (Frequency != null)
                {
                    prevMaintLibList = prevMaintLibList.Where(x => x.Frequency.Equals(Frequency)).ToList();
                }
                #endregion
            }
            var totalRecords = 0;
            var recordsFiltered = 0;
            start = start.HasValue
                ? start / length
                : 0;
            recordsFiltered = prevMaintLibList.Count();
            totalRecords = prevMaintLibList.Count();
            int initialPage = start.Value;
            var filteredResult = prevMaintLibList
                .Skip(initialPage * length ?? 0)
                .Take(length ?? 0)
                .ToList();
            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult }, JsonSerializerDateSettings);
        }

        //*** V2-694
        [HttpPost]
        public string PmProcGridByInactiveFlag(int? draw, int? start, int? length, string ClientLookupId, decimal? JobDuration, string Description = "",
            string FrequencyType = "", int? Frequency = null)
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            var colname = Request.Form.GetValues("columns[" + order + "][name]");
            PrevMaintWrapper pWrapper = new PrevMaintWrapper(userData);
            List<PreventiveMaintenanceLibraryModel> prevMaintLibList = pWrapper.PopulatePmProcedure_InactiveFlag(0);
            if (prevMaintLibList != null)
            {
                prevMaintLibList = this.PmProcGridSortByColumnWithOrder(colname[0], orderDir, prevMaintLibList);
            }
            if (prevMaintLibList != null)
            {
                #region AdvSearch
                if (!string.IsNullOrEmpty(ClientLookupId))
                {
                    ClientLookupId = ClientLookupId.ToUpper();
                    prevMaintLibList = prevMaintLibList.Where(x => (!string.IsNullOrWhiteSpace(x.ClientLookupId) && x.ClientLookupId.ToUpper().Contains(ClientLookupId))).ToList();
                }
                if (!string.IsNullOrEmpty(Description))
                {
                    Description = Description.ToUpper();
                    prevMaintLibList = prevMaintLibList.Where(x => (!string.IsNullOrWhiteSpace(x.Description) && x.Description.ToUpper().Contains(Description))).ToList();
                }
                if (JobDuration != null)
                {
                    prevMaintLibList = prevMaintLibList.Where(x => x.JobDuration.Equals(JobDuration)).ToList();
                }
                if (!string.IsNullOrEmpty(FrequencyType))
                {
                    FrequencyType = FrequencyType.ToUpper();
                    prevMaintLibList = prevMaintLibList.Where(x => (!string.IsNullOrWhiteSpace(x.FrequencyType) && x.FrequencyType.ToUpper().Contains(FrequencyType))).ToList();
                }
                if (Frequency != null)
                {
                    prevMaintLibList = prevMaintLibList.Where(x => x.Frequency.Equals(Frequency)).ToList();
                }
                #endregion
            }
            var totalRecords = 0;
            var recordsFiltered = 0;
            start = start.HasValue
                ? start / length
                : 0;
            recordsFiltered = prevMaintLibList.Count();
            totalRecords = prevMaintLibList.Count();
            int initialPage = start.Value;
            var filteredResult = prevMaintLibList
                .Skip(initialPage * length ?? 0)
                .Take(length ?? 0)
                .ToList();
            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult }, JsonSerializerDateSettings);
        }
        private List<PreventiveMaintenanceLibraryModel> PmProcGridSortByColumnWithOrder(string order, string orderDir, List<PreventiveMaintenanceLibraryModel> data)
        {
            List<PreventiveMaintenanceLibraryModel> lst = new List<PreventiveMaintenanceLibraryModel>();
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
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.JobDuration).ToList() : data.OrderBy(p => p.JobDuration).ToList();
                        break;
                    case "3":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.FrequencyType).ToList() : data.OrderBy(p => p.FrequencyType).ToList();
                        break;
                    case "4":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Frequency).ToList() : data.OrderBy(p => p.Frequency).ToList();
                        break;
                    default:
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ClientLookupId).ToList() : data.OrderBy(p => p.ClientLookupId).ToList();
                        break;
                }
            }
            return lst;
        }

        [HttpPost]
        public JsonResult AddPmLibrary(long PrevMaintLibraryId)
        {
            string Mode = string.Empty;
            string ModelValidationFailedMessage = string.Empty;
            PrevMaintWrapper pmWrapper = new PrevMaintWrapper(userData);
            PrevMaintMaster pmm = new PrevMaintMaster();
            bool LibraryActivationStatus = false;
            pmm = pmWrapper.AddPreventiveProc(PrevMaintLibraryId, out LibraryActivationStatus);
            Mode = "add";

            if (pmm.ErrorMessages != null && pmm.ErrorMessages.Count > 0)
            {
                return Json(new { ErrorMessages = pmm.ErrorMessages }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Result = JsonReturnEnum.success.ToString(), PrevMaintMasterId = pmm.PrevMaintMasterId, LibraryActivationStatus = LibraryActivationStatus, mode = Mode }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region Schedule Records
        [HttpPost]
        public string PopulateScheduleRecords(int? draw, int? start, int? length, long PrevMasterID, string ChargeToClientLookupId = "",string ChargeToName = "", string Frequency = "", DateTime? NextDueDate = null, string WorkOrder_ClientLookupId = "", DateTime? LastScheduled = null, DateTime? LastPerformed = null, string Meter_ClientLookupId = "", string OnDemandGroup = "")
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            start = start.HasValue
          ? start / length
          : 0;
            int skip = start * length ?? 0;
            PrevMaintWrapper pWrapper = new PrevMaintWrapper(userData);
            List<ScheduleRecords> ScheduleRecordsList = pWrapper.GetScheduleRecordsChunkSearch_V2(PrevMasterID, skip, length ?? 0, order, orderDir, ChargeToClientLookupId, ChargeToName, Frequency, NextDueDate, WorkOrder_ClientLookupId, LastScheduled, LastPerformed, Meter_ClientLookupId, OnDemandGroup); // Changed to chunk search for V2-1212
            var totalRecords = 0;
            var recordsFiltered = 0;
            if (ScheduleRecordsList != null && ScheduleRecordsList.Count > 0)
            {
                recordsFiltered = ScheduleRecordsList[0].TotalCount;
                totalRecords = ScheduleRecordsList[0].TotalCount;
            }
            else
            {
                recordsFiltered = 0;
                totalRecords = 0;
            }
            int initialPage = start.Value;
            var filteredResult = ScheduleRecordsList
                .ToList();
            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult }, JsonSerializerDateSettings);
        }
        private List<ScheduleRecords> GetAllSchRecordsSortByColumnWithOrder(string order, string orderDir, List<ScheduleRecords> data)
        {
            List<ScheduleRecords> lst = new List<ScheduleRecords>();
            if (data != null)
            {
                switch (order)
                {
                    case "1":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ChargeToClientLookupId).ToList() : data.OrderBy(p => p.ChargeToClientLookupId).ToList();
                        break;
                    case "2":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ChargeToName).ToList() : data.OrderBy(p => p.ChargeToName).ToList();
                        break;
                    case "3":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Frequency).ToList() : data.OrderBy(p => p.Frequency).ToList();
                        break;
                    case "4":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.NextDueDate).ToList() : data.OrderBy(p => p.NextDueDate).ToList();
                        break;
                    case "5":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.WorkOrder_ClientLookupId).ToList() : data.OrderBy(p => p.WorkOrder_ClientLookupId).ToList();
                        break;
                    case "6":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.LastScheduled).ToList() : data.OrderBy(p => p.LastScheduled).ToList();
                        break;
                    case "7":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.LastPerformed).ToList() : data.OrderBy(p => p.LastPerformed).ToList();
                        break;
                    default:
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ChargeToClientLookupId).ToList() : data.OrderBy(p => p.ChargeToClientLookupId).ToList();
                        break;
                }

            }
            return lst;
        }
        [HttpPost]
        [ActionName("AddScheduleRecordsGet")]
        public PartialViewResult AddScheduleRecords(long PrevMaintMasterID, string ClientLookupId, string ScheduleType, long PrevMaintLibraryID)
        {
            PrevMaintWrapper prevMaintWrapper = new PrevMaintWrapper(userData);
            PrevMaintVM objPrevMaintVM = new PrevMaintVM();
            ScheduleRecords objScheduleRecords = new ScheduleRecords();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            List<DataContracts.LookupList> AllLookUps = new List<DataContracts.LookupList>();
            objPrevMaintVM.userData = this.userData;
            objScheduleRecords.PrevMaintMasterId = PrevMaintMasterID;
            objScheduleRecords.ScheduleType = ScheduleType;
            objScheduleRecords.PrevmaintClientlookUp = ClientLookupId;
            objScheduleRecords.PrevMaintLibraryID = PrevMaintLibraryID;
            PopulateDropdownControls(objPrevMaintVM, prevMaintWrapper, objScheduleRecords);
            var FrequencyTypeList = UtilityFunction.populateFrequencyTypeList();
            var ScheduleMethodList = UtilityFunction.populateScheduleMethodList();
            if (ScheduleMethodList != null)
            {
                objScheduleRecords.ScheduleMethodList = ScheduleMethodList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
            }
            var PersonnelLookUplist = GetList_PalnnerPersonnel();
            if (PersonnelLookUplist != null)
            {
                objScheduleRecords.PlannerPersonnelList = PersonnelLookUplist.Select(x => new SelectListItem { Text = x.NameFirst + " - " + x.NameLast, Value = x.PersonnelId.ToString() }).ToList();
            }
            AllLookUps = commonWrapper.GetAllLookUpList();
            objScheduleRecords.FailureCodeList = AllLookUps.Where(x => x.ListName == "WO_FAILURE")
                .GroupBy(x => x.ListValue).Select(x => x.FirstOrDefault()).Select(s => new SelectListItem { Text = s.ListValue + " - " + s.Description, Value = s.ListValue }).ToList();
            objScheduleRecords.ActionCodeList = AllLookUps.Where(x => x.ListName == "WO_ACTION")
                    .GroupBy(x => x.ListValue).Select(x => x.FirstOrDefault()).Select(s => new SelectListItem
                    { Text = s.ListValue + " - " + s.Description, Value = s.ListValue }).ToList();
            objScheduleRecords.RootCauseCodeList = AllLookUps.Where(s => s.ListName == "RootCause").Select(s => new SelectListItem { Text = s.ListValue + " - " + s.Description, Value = s.ListValue }).ToList();
            if (PrevMaintLibraryID > 0)
            {
                var LibraryData = prevMaintWrapper.GetPrevMaintSchedFromLibray(PrevMaintLibraryID);
                if (LibraryData != null)
                {
                    objScheduleRecords.DownRequired = LibraryData.DownRequired;
                    objScheduleRecords.Frequency = LibraryData.Frequency;
                    objScheduleRecords.FrequencyType = LibraryData.FrequencyType;
                    if (!string.IsNullOrWhiteSpace(LibraryData.FrequencyType))
                    {
                        var freqTypes = FrequencyTypeList.Where(x => x.value.Equals(LibraryData.FrequencyType));
                        if (freqTypes != null && freqTypes.Count() > 0)
                        {
                            objScheduleRecords.FrequencyType = freqTypes.FirstOrDefault().text;
                        }
                    }
                    objScheduleRecords.ScheduleMethod = LibraryData.ScheduleMethod;

                    if (!string.IsNullOrWhiteSpace(LibraryData.ScheduleMethod))
                    {
                        var ScheduleMethod = ScheduleMethodList.Where(x => x.value.Equals(LibraryData.ScheduleMethod));
                        if (ScheduleMethod != null && ScheduleMethod.Count() > 0)
                        {
                            objScheduleRecords.ScheduleMethod = ScheduleMethod.FirstOrDefault().text;
                        }
                    }
                }
            }
            var daysOfWeek = UtilityFunction.DaysOfWeekList();
            if (daysOfWeek != null)
            {
                objPrevMaintVM.scheduleRecords.DaysOfWeekList = daysOfWeek.Select(x => new SelectListItem { Text = x.text, Value = x.value });
            }
            objPrevMaintVM.scheduleRecords = objScheduleRecords;
            LocalizeControls(objPrevMaintVM, LocalizeResourceSetConstants.PrevMaintDetails);
            return PartialView("_AddSchedule", objPrevMaintVM);
        }
        [HttpPost]
        public PartialViewResult EditScheduleRecords(long PrevMaintMasterId, long PrevMaintScheId, string ClientLookupId, string ScheduleType, long PrevMaintLibraryID)
        {
            PrevMaintVM objPrevMaintVM = new PrevMaintVM();
            PrevMaintWrapper _PrevMaintObj = new PrevMaintWrapper(userData);
            objPrevMaintVM.userData = this.userData;
            ScheduleRecords objSchd = _PrevMaintObj.PoupalateScheduleDetailsbyPK(PrevMaintMasterId, PrevMaintScheId);
            objPrevMaintVM.scheduleRecords = objSchd;
            objPrevMaintVM.scheduleRecords.PrevmaintClientlookUp = ClientLookupId;
            objPrevMaintVM.scheduleRecords.ScheduleType = ScheduleType;
            objPrevMaintVM.scheduleRecords.PrevMaintLibraryID = PrevMaintLibraryID;
            if (objPrevMaintVM.scheduleRecords.LastPerformed != null && objPrevMaintVM.scheduleRecords.LastPerformed.Value == default(DateTime))
            {
                objPrevMaintVM.scheduleRecords.LastPerformed = null;
            }
            if (objPrevMaintVM.scheduleRecords.LastScheduled != null && objPrevMaintVM.scheduleRecords.LastScheduled.Value == default(DateTime))
            {
                objPrevMaintVM.scheduleRecords.LastScheduled = null;
            }
            if (objPrevMaintVM.scheduleRecords.CurrentWOComplete != null && objPrevMaintVM.scheduleRecords.CurrentWOComplete.Value == default(DateTime))
            {
                objPrevMaintVM.scheduleRecords.CurrentWOComplete = null;
            }
            if (objPrevMaintVM.scheduleRecords.NextDueDate != null && objPrevMaintVM.scheduleRecords.NextDueDate.Value == default(DateTime))
            {
                objPrevMaintVM.scheduleRecords.NextDueDate = null;
            }
            var FrequencyTypeList = UtilityFunction.populateFrequencyTypeList();
            if (FrequencyTypeList != null && FrequencyTypeList.Count() > 0 && !string.IsNullOrEmpty(objSchd.FrequencyType))
            {
                // V2-275 - RKL - 2019-10-14 - have to match against the value not the text
                objPrevMaintVM.scheduleRecords.FrequencyType = FrequencyTypeList.Where(x => x.value.Equals(objSchd.FrequencyType)).FirstOrDefault().value;
            }
            var ScheduleChargeTypeList = UtilityFunction.populateChargeType();
            var meterLookUplist = GetLookUpList_Meter();
            if (PrevMaintLibraryID > 0)
            {
                if (ScheduleChargeTypeList != null && ScheduleChargeTypeList.Count() > 0)
                {
                    // V2-275  RKL - 2019-10-14 - have to match against the value not the text
                    objPrevMaintVM.scheduleRecords.ChargeType = ScheduleChargeTypeList.Where(x => x.value.Equals(objSchd.ChargeType)).FirstOrDefault().value;
                }
                if (objSchd.MeterId != 0)
                {

                    if (meterLookUplist != null && meterLookUplist.Count() > 0)
                    {
                        objPrevMaintVM.scheduleRecords.Meter_ClientLookupId = meterLookUplist.Where(x => x.MeterId.Equals(objSchd.MeterId)).FirstOrDefault().Meter_ClientLookupId;
                    }
                }
                if (!string.IsNullOrEmpty(objSchd.OnDemandGroup))
                {
                    var AllLookUp = _PrevMaintObj.GetAllLookUpList();
                    if (AllLookUp != null)
                    {
                        var typeList = AllLookUp.Where(x => x.ListName == LookupListConstants.Preventive_Maint_Ondemand_Grp).ToList();
                        if (typeList != null && typeList.Count() > 0)
                        {
                            objPrevMaintVM.scheduleRecords.OnDemandGroup = typeList.Where(x => x.ListValue.Equals(objSchd.OnDemandGroup)).FirstOrDefault().ListValue;
                        }
                    }
                }
            }
            var PersonnelLookUplist = GetList_PalnnerPersonnel();
            if (PersonnelLookUplist != null)
            {
                objPrevMaintVM.scheduleRecords.PlannerPersonnelList = PersonnelLookUplist.Select(x => new SelectListItem { Text = x.NameFirst + " - " + x.NameLast, Value = x.PersonnelId.ToString() }).ToList();
            }
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            var daysOfWeek = UtilityFunction.DaysOfWeekList();
            if (daysOfWeek != null)
            {
                objPrevMaintVM.scheduleRecords.DaysOfWeekList = daysOfWeek.Select(x => new SelectListItem { Text = x.text, Value = x.value });
            }
            objPrevMaintVM.scheduleRecords.ExclusionDaysStringHdn = string.Join(",", objSchd.ExclusionDaysString);
            List<DataContracts.LookupList> AllLookUps = new List<DataContracts.LookupList>();
            AllLookUps = commonWrapper.GetAllLookUpList();
            objPrevMaintVM.scheduleRecords.FailureCodeList = AllLookUps.Where(x => x.ListName == "WO_FAILURE")
                .GroupBy(x => x.ListValue).Select(x => x.FirstOrDefault()).Select(s => new SelectListItem { Text = s.ListValue + " - " + s.Description, Value = s.ListValue }).ToList();
            objPrevMaintVM.scheduleRecords.ActionCodeList = AllLookUps.Where(x => x.ListName == "WO_ACTION")
                    .GroupBy(x => x.ListValue).Select(x => x.FirstOrDefault()).Select(s => new SelectListItem
                    { Text = s.ListValue + " - " + s.Description, Value = s.ListValue }).ToList();
            objPrevMaintVM.scheduleRecords.RootCauseCodeList = AllLookUps.Where(s => s.ListName == "RootCause").Select(s => new SelectListItem { Text = s.ListValue + " - " + s.Description, Value = s.ListValue }).ToList();

            #region Inactive Days
            if (!string.IsNullOrEmpty(objSchd.ExcludeDOW) && objSchd.ExcludeDOW[0] == '1')
            {
                objSchd.Sunday = true;
            }
            if (!string.IsNullOrEmpty(objSchd.ExcludeDOW) && objSchd.ExcludeDOW[1] == '1')
            {
                objSchd.Monday = true;
            }
            if (!string.IsNullOrEmpty(objSchd.ExcludeDOW) && objSchd.ExcludeDOW[2] == '1')
            {
                objSchd.Tuesday = true;
            }
            if (!string.IsNullOrEmpty(objSchd.ExcludeDOW) && objSchd.ExcludeDOW[3] == '1')
            {
                objSchd.Wednesday = true;
            }
            if (!string.IsNullOrEmpty(objSchd.ExcludeDOW) && objSchd.ExcludeDOW[4] == '1')
            {
                objSchd.Thursday = true;
            }
            if (!string.IsNullOrEmpty(objSchd.ExcludeDOW) && objSchd.ExcludeDOW[5] == '1')
            {
                objSchd.Friday = true;
            }
            if (!string.IsNullOrEmpty(objSchd.ExcludeDOW) && objSchd.ExcludeDOW[6] == '1')
            {
                objSchd.Saturday = true;
            }
            #endregion

            PopulateDropdownControls(objPrevMaintVM, _PrevMaintObj, objSchd);
            LocalizeControls(objPrevMaintVM, LocalizeResourceSetConstants.PrevMaintDetails);
            return PartialView("_AddSchedule", objPrevMaintVM);
        }
        private void PopulateDropdownControls(PrevMaintVM objPartsVM, PrevMaintWrapper prevMaintWrapper, ScheduleRecords objScheduleRecords)
        {
            #region LookUpList
            var AllLookUps = prevMaintWrapper.GetAllLookUpList();
            if (AllLookUps != null)
            {
                var onDemandLookUplist = AllLookUps.Where(x => x.ListName == LookupListConstants.Preventive_Maint_Ondemand_Grp).ToList();
                if (onDemandLookUplist != null)
                {
                    objScheduleRecords.OnDemandList = onDemandLookUplist.Select(x => new SelectListItem { Text = x.ListValue + " - " + x.Description, Value = x.ListValue.ToString() });
                }
                var ScheduleTypeLookUplist = AllLookUps.Where(x => x.ListName == LookupListConstants.Preventive_Maint_WO_Type).ToList();
                if (ScheduleTypeLookUplist != null)
                {
                    objScheduleRecords.ScheduledTypeList = ScheduleTypeLookUplist.Select(x => new SelectListItem { Text = x.ListValue + " - " + x.Description, Value = x.ListValue.ToString() });
                }
                var SchedulePriorityLookUplist = AllLookUps.Where(x => x.ListName == LookupListConstants.WO_Priority).ToList();
                if (SchedulePriorityLookUplist != null)
                {
                    objScheduleRecords.ScheduledPriorityList = SchedulePriorityLookUplist.Select(x => new SelectListItem { Text = x.Description, Value = x.ListValue.ToString() });
                }
                var ScheduleShiftLookUplist = AllLookUps.Where(x => x.ListName == LookupListConstants.Shift).ToList();
                if (ScheduleShiftLookUplist != null)
                {
                    objScheduleRecords.ScheduledShiftList = ScheduleShiftLookUplist.Select(x => new SelectListItem { Text = x.Description, Value = x.ListValue.ToString() });
                }
            }

            #endregion

            var meterLookUplist = GetLookUpList_Meter();
            if (meterLookUplist != null)
            {
                objScheduleRecords.MeterLookUpList = meterLookUplist.Select(x => new SelectListItem { Text = x.Meter_ClientLookupId + " - " + x.Name + " - " + x.ReadingCurrent, Value = x.MeterId.ToString() });
            }
            var PersonnelLookUplist = GetList_PersonnelV2();
            if (PersonnelLookUplist != null)
            {
                objScheduleRecords.WorkAssignedLookUpList = PersonnelLookUplist.Select(x => new SelectListItem { Text = x.AssignedTo_PersonnelClientLookupId + " - " + x.NameFirst + " - " + x.NameLast, Value = x.AssignedTo_PersonnelId.ToString() });
            }

            var ScheduleChargeTypeList = UtilityFunction.populateChargeType();
            if (ScheduleChargeTypeList != null)
            {
                objScheduleRecords.ChargeTypeList = ScheduleChargeTypeList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
            }

            var ScheduleMethodList = UtilityFunction.populateScheduleMethodList();
            if (ScheduleMethodList != null)
            {
                objScheduleRecords.ScheduleMethodList = ScheduleMethodList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
            }
            var FrequencyTypeList = UtilityFunction.populateFrequencyTypeList();
            if (FrequencyTypeList != null)
            {
                objScheduleRecords.FrequencyTypeList = FrequencyTypeList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
            }

            var ChargeTypeLookUpList = PopulatelookUpListByType(objScheduleRecords.ChargeType);
            if (ChargeTypeLookUpList != null)
            {
                objScheduleRecords.ChargeTypelookUpList = ChargeTypeLookUpList.Select(x => new SelectListItem { Text = x.ChargeToClientLookupId + " - " + x.Name, Value = x.ChargeToClientLookupId.ToString() });
            }
            objPartsVM.scheduleRecords = objScheduleRecords;
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddScheduleRecords(PrevMaintVM prevMaintVM)
        {
            string Mode = string.Empty;
            string ModelValidationFailedMessage = string.Empty;
            if (ModelState.IsValid)
            {
                PrevMaintWrapper wrapper = new PrevMaintWrapper(userData);
                PrevMaintSched objPrvSch = new PrevMaintSched();
                if (prevMaintVM.scheduleRecords.PrevMaintScheId != 0)
                {
                    objPrvSch = wrapper.EditPrevMaintSched(prevMaintVM.scheduleRecords);
                }
                else
                {
                    Mode = "add";
                    objPrvSch = wrapper.AddPrevMaintSched(prevMaintVM.scheduleRecords);
                }
                if (objPrvSch.ErrorMessages != null && objPrvSch.ErrorMessages.Count > 0 && objPrvSch.ErrorMessages[0].ToString() != ErrorMessageConstants.Schedule_Record_Exists)
                {
                    return Json(objPrvSch.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), prevemaintmasterid = prevMaintVM.scheduleRecords.PrevMaintMasterId, mode = Mode }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult DeleteScheduleRecords(long PrevMaintScheId)
        {
            PrevMaintWrapper wrapper = new PrevMaintWrapper(userData);
            var deleteResult = wrapper.DeletePrevMaintSched(PrevMaintScheId);
            if (deleteResult)
            {
                return Json(new { Result = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Result = JsonReturnEnum.failed.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

        #region Exclusion Days
        public PartialViewResult ExclusionDays()
        {
            PrevMaintVM objPrevMaintVM = new PrevMaintVM();
            PrevMaintWrapper _PrevMaintObj = new PrevMaintWrapper(userData);
            objPrevMaintVM.userData = this.userData;
            objPrevMaintVM.scheduleRecords = new ScheduleRecords();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            var daysOfWeek = UtilityFunction.DaysOfWeekList();
            if (daysOfWeek != null)
            {
                objPrevMaintVM.scheduleRecords.DaysOfWeekList = daysOfWeek.Select(x => new SelectListItem { Text = x.text, Value = x.value });
            }
            LocalizeControls(objPrevMaintVM, LocalizeResourceSetConstants.PrevMaintDetails);
            return PartialView("_ExclusionDaysModal", objPrevMaintVM);
        }
        #endregion
        #endregion

        #region Tasks
        [HttpPost]
        public string PopulateTasks(int? draw, int? start, int? length, long PrevMasterID, long prevMaintLibraryId)
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            PrevMaintWrapper pWrapper = new PrevMaintWrapper(userData);
            var Tasks = pWrapper.populateTaskList(PrevMasterID, prevMaintLibraryId);
            foreach (var item in Tasks)
            {
                item.ChargeTypeText = UtilityFunction.populateChargeType().Where(x => x.value == item.ChargeType).Select(x => x.text).FirstOrDefault();
            }
            Tasks = this.GetAllTasksSortByColumnWithOrder(order, orderDir, Tasks);
            var totalRecords = 0;
            var recordsFiltered = 0;
            start = start.HasValue
                ? start / length
                : 0;
            recordsFiltered = Tasks.Count();
            totalRecords = Tasks.Count();
            int initialPage = start.Value;
            var filteredResult = Tasks
                .Skip(initialPage * length ?? 0)
                .Take(length ?? 0)
                .ToList();
            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult }, JsonSerializer12HoursDateAndTimeSettings);
        }
        private List<PrevMaintTaskModel> GetAllTasksSortByColumnWithOrder(string order, string orderDir, List<PrevMaintTaskModel> data)
        {
            List<PrevMaintTaskModel> lst = new List<PrevMaintTaskModel>();
            if (data != null)
            {
                switch (order)
                {
                    case "0":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.TaskNumber).ToList() : data.OrderBy(p => p.TaskNumber).ToList();
                        break;
                    case "1":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Description).ToList() : data.OrderBy(p => p.Description).ToList();
                        break;
                    case "2":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ChargeTypeText).ToList() : data.OrderBy(p => p.ChargeTypeText).ToList();
                        break;
                    case "3":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ChargeToClientLookupId).ToList() : data.OrderBy(p => p.ChargeToClientLookupId).ToList();
                        break;
                    default:
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.TaskNumber).ToList() : data.OrderBy(p => p.TaskNumber).ToList();
                        break;
                }
            }
            return lst;
        }
        [HttpPost]
        [ActionName("AddPMTasks")]
        public PartialViewResult AddTasks(long PrevMasterID, string ClientLookupId)
        {
            PrevMaintWrapper pmWrapper = new PrevMaintWrapper(userData);
            PrevMaintVM objPrevMaintVM = new PrevMaintVM();
            PrevMaintTaskModel objTaskModel = new PrevMaintTaskModel();
            objTaskModel.PrevMaintMasterId = PrevMasterID;
            objTaskModel.PrevmaintClientlookUp = ClientLookupId;
            var ScheduleChargeTypeList = UtilityFunction.populateChargeType();
            if (ScheduleChargeTypeList != null)
            {
                objTaskModel.ChargeTypeList = ScheduleChargeTypeList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
            }
            objTaskModel.ChargeTypelookUpList = new List<SelectListItem>();
            objPrevMaintVM.prevMaintTaskModel = objTaskModel;
            LocalizeControls(objPrevMaintVM, LocalizeResourceSetConstants.PrevMaintDetails);
            return PartialView("~/Views/PreventiveMaintenance/_AddPmTask.cshtml", objPrevMaintVM);
        }
        [HttpPost]
        public PartialViewResult EditTasks(long PrevMasterID, long _taskId, string ClientLookupId)
        {
            PrevMaintWrapper pmWrapper = new PrevMaintWrapper(userData);
            PrevMaintVM objPrevMaintVM = new PrevMaintVM();
            PrevMaintTaskModel objPrevTaskModel = new PrevMaintTaskModel();
            objPrevTaskModel = pmWrapper.GaetTask(PrevMasterID, _taskId, ClientLookupId);
            objPrevMaintVM.prevMaintTaskModel = objPrevTaskModel;
            var ScheduleChargeTypeList = UtilityFunction.populateChargeType();
            if (ScheduleChargeTypeList != null && ScheduleChargeTypeList.Count > 0)
            {
                objPrevTaskModel.ChargeTypeList = ScheduleChargeTypeList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
            }
            var ChargeTypeLookUpList = PopulatelookUpListByType(objPrevTaskModel.ChargeType);

            if (ChargeTypeLookUpList != null)
            {
                objPrevTaskModel.ChargeTypelookUpList = ChargeTypeLookUpList.Select(x => new SelectListItem { Text = x.ChargeToClientLookupId + " - " + x.Name, Value = x.ChargeToClientLookupId.ToString() });
            }
            LocalizeControls(objPrevMaintVM, LocalizeResourceSetConstants.PrevMaintDetails);
            return PartialView("~/Views/PreventiveMaintenance/_AddPmTask.cshtml", objPrevMaintVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddTasks(PrevMaintVM prevMaintVM)
        {
            if (ModelState.IsValid)
            {
                string Mode = string.Empty;
                List<String> errorList = new List<string>();
                PrevMaintWrapper pmWrapper = new PrevMaintWrapper(userData);
                PrevMaintTask pmTask = pmWrapper.AddOrUpdatePrev(prevMaintVM, ref errorList, out Mode);
                if (errorList != null && errorList.Count > 0)
                {
                    return Json(pmTask.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { data = JsonReturnEnum.success.ToString(), pmid = prevMaintVM.prevMaintTaskModel.PrevMaintMasterId, mode = Mode }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                string ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult DeleteTasks(long taskNumber)
        {
            PrevMaintWrapper wrapper = new PrevMaintWrapper(userData);
            var deleteResult = wrapper.DeletePrevMaintTask(taskNumber);
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

        #region Notes
        [HttpPost]
        public string PopulateNotes(int? draw, int? start, int? length, long PrevMasterID)
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            CommonWrapper objCommonWrapper = new CommonWrapper(userData);
            var Notes = objCommonWrapper.PopulateNotes(PrevMasterID, AttachmentTableConstant.PreventiveMaintenance);
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
        [HttpPost]
        [ActionName("AddPMNotes")]
        public PartialViewResult AddNotes(long PrevMasterID, string ClientLookupId)
        {
            PrevMaintVM objPrevMaintVM = new PrevMaintVM();
            NotesModel Notes = new NotesModel();
            Notes.PrevMasterID = PrevMasterID;
            Notes.ClientLookupId = ClientLookupId;
            Notes.ObjectId = PrevMasterID;
            objPrevMaintVM.notesModel = Notes;
            LocalizeControls(objPrevMaintVM, LocalizeResourceSetConstants.PrevMaintDetails);
            return PartialView("~/Views/PreventiveMaintenance/_PmNotesAdd.cshtml", objPrevMaintVM);
        }
        [HttpPost]
        public PartialViewResult EditNotes(long PrevMasterID, long _notesId, string ClientLookupId)
        {
            PrevMaintVM objPrevMaintVM = new PrevMaintVM();
            NotesModel _NotesModel = new NotesModel();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            _NotesModel = commonWrapper.EditNotes(PrevMasterID, _notesId);
            _NotesModel.NotesId = _notesId;
            _NotesModel.PrevMasterID = PrevMasterID;
            _NotesModel.ClientLookupId = ClientLookupId;
            objPrevMaintVM.notesModel = _NotesModel;
            LocalizeControls(objPrevMaintVM, LocalizeResourceSetConstants.PrevMaintDetails);
            return PartialView("~/Views/PreventiveMaintenance/_PmNotesAdd.cshtml", objPrevMaintVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddNotes(PrevMaintVM prevMaintVM)
        {
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            List<string> errorMessages = new List<string>();
            if (ModelState.IsValid)
            {
                string Mode = string.Empty;
                if (prevMaintVM.notesModel.NotesId == 0)
                {
                    Mode = "add";
                }
                errorMessages = commonWrapper.AddNotes(prevMaintVM.notesModel, AttachmentTableConstant.PreventiveMaintenance);
                if (errorMessages != null && errorMessages.Count > 0)
                {
                    return Json(errorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), PrevMasterID = prevMaintVM.notesModel.PrevMasterID, mode = Mode }, JsonRequestBehavior.AllowGet);
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
        public ActionResult DeleteNotes(long _notesId)
        {
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            if (commonWrapper.DeleteNotes(_notesId))
            {
                return Json(new { Result = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Result = JsonReturnEnum.failed.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Attachments
        [HttpPost]
        public string PopulateAttachments(int? draw, int? start, int? length, long prevMasterID)
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            CommonWrapper objCommonWrapper = new CommonWrapper(userData);
            var Attachments = objCommonWrapper.PopulateAttachments(prevMasterID, AttachmentTableConstant.PreventiveMaintenance, userData.Security.PrevMaint.Edit);
            if (Attachments != null)
            {
                Attachments = GetAllAttachmentsPrintWithFormSortByColumnWithOrder(order, orderDir, Attachments);
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
            var Attachsecurity = userData.Security.PrevMaint.Edit;
            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult, Attachsecurity = Attachsecurity }, JsonSerializer12HoursDateAndTimeSettings);
        }
        [HttpGet]
        public PartialViewResult AddAttachments(long PrevMasterID, string ClientLookupId)
        {
            PrevMaintVM objPrevMaintVM = new PrevMaintVM();
            AttachmentModel Attachment = new AttachmentModel();
            PreventiveMaintenanceModel objPM = new PreventiveMaintenanceModel();
            objPM.ClientLookupId = ClientLookupId;
            objPM.PrevMaintMasterId = PrevMasterID;
            Attachment.PrevMaintMasterId = PrevMasterID;
            objPrevMaintVM.attachmentModel = Attachment;
            objPrevMaintVM.preventiveMaintenanceModel = objPM;
            LocalizeControls(objPrevMaintVM, LocalizeResourceSetConstants.PrevMaintDetails);
            return PartialView("~/Views/PreventiveMaintenance/_PmAttachmentAdd.cshtml", objPrevMaintVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddAttachments()
        {
            string ModelValidationFailedMessage = string.Empty;
            if (ModelState.IsValid)
            {
                string Mode = string.Empty;
                Mode = "add";
                Stream stream = Request.Files[0].InputStream;
                Client.Models.AttachmentModel attachmentModel = new Client.Models.AttachmentModel();
                CommonWrapper objCommonWrapper = new CommonWrapper(userData);
                Attachment fileAtt = new Attachment();
                attachmentModel.FileName = System.IO.Path.GetFileNameWithoutExtension(Request.Files[0].FileName);
                string fileExt = System.IO.Path.GetExtension(Request.Files[0].FileName);
                attachmentModel.FileType = fileExt.Substring(1);
                attachmentModel.FileSize = Request.Files[0].ContentLength;
                attachmentModel.ContentType = Request.Files[0].ContentType;
                attachmentModel.ObjectId = Convert.ToInt64(Request.Form["attachmentModel.PrevMaintMasterId"]);
                attachmentModel.Subject = String.IsNullOrEmpty(Request.Form["attachmentModel.Subject"]) ? "No Subject" : Request.Form["attachmentModel.Subject"];
                attachmentModel.TableName = AttachmentTableConstant.PreventiveMaintenance;
                attachmentModel.PrintwithForm = Convert.ToBoolean(Request.Form["attachmentModel.PrintwithForm"].Split(',').FirstOrDefault()); //V2-949
                bool attachStatus = false;
                bool OnPremise = userData.DatabaseKey.Client.OnPremise;
                if (OnPremise)
                {
                    fileAtt = objCommonWrapper.AddAttachmentOnPremise(attachmentModel, stream, ref attachStatus, userData.Security.PrevMaintLibrary.Edit);
                }
                else
                {
                    fileAtt = objCommonWrapper.AddAttachment(attachmentModel, stream, ref attachStatus, userData.Security.PrevMaintLibrary.Edit);
                }
                if (attachStatus)
                {
                    if (fileAtt.ErrorMessages != null && fileAtt.ErrorMessages.Count > 0)
                    {
                        return Json(fileAtt.ErrorMessages, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { Result = JsonReturnEnum.success.ToString(), pmid = Convert.ToInt64(Request.Form["attachmentModel.PrevMaintMasterId"]), mode = Mode }, JsonRequestBehavior.AllowGet);
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
        public ActionResult DeletePMAttachments(long _fileAttachmentId)
        {
            CommonWrapper objCommonWrapper = new CommonWrapper(userData);
            bool deleteResult = false;
            string Message = string.Empty;
            bool OnPremise = userData.DatabaseKey.Client.OnPremise;
            if (OnPremise)
            {
                deleteResult = objCommonWrapper.DeleteAttachmentOnPremise(_fileAttachmentId, ref Message);
            }
            else
            {
                deleteResult = objCommonWrapper.DeleteAttachment(_fileAttachmentId);

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
        #region V2-949
        public ActionResult EditAttachment(long FileAttachmentId, long prevMasterID, string ClientLookupId)
        {
            PrevMaintVM objPrevMaintVM = new PrevMaintVM();
            AttachmentModel Attachment = new AttachmentModel();
            PrevMaintWrapper pWrapper = new PrevMaintWrapper(userData);
            PreventiveMaintenanceModel objPM = new PreventiveMaintenanceModel();
            objPM.ClientLookupId = ClientLookupId;
            objPM.PrevMaintMasterId = prevMasterID;
            Attachment.PrevMaintMasterId = prevMasterID;
            objPrevMaintVM.attachmentModel = Attachment;
            objPrevMaintVM.preventiveMaintenanceModel = objPM;
            Attachment = pWrapper.EditPMAttachment(prevMasterID, FileAttachmentId);
            Attachment.ClientLookupId = ClientLookupId;
            objPrevMaintVM.attachmentModel = Attachment;
            LocalizeControls(objPrevMaintVM, LocalizeResourceSetConstants.PrevMaintDetails);
            return PartialView("~/Views/PreventiveMaintenance/_EditPMAttachment.cshtml", objPrevMaintVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditAttachments()
        {
            string ModelValidationFailedMessage = string.Empty;
            if (ModelState.IsValid)
            {
                string Mode = string.Empty;
                Mode = "edit";
                Client.Models.AttachmentModel attachmentModel = new Client.Models.AttachmentModel();
                CommonWrapper objCommonWrapper = new CommonWrapper(userData);
                Boolean fileAtt = new Boolean();
                attachmentModel.AttachmentId = Convert.ToInt64(Request.Form["attachmentModel.AttachmentId"]);
                attachmentModel.ObjectId = Convert.ToInt64(Request.Form["attachmentModel.PrevMaintMasterId"]);
                attachmentModel.Description = String.IsNullOrEmpty(Request.Form["attachmentModel.Subject"]) ? "No Subject" : Request.Form["attachmentModel.Subject"];
                attachmentModel.PrintwithForm = Convert.ToBoolean(Request.Form["attachmentModel.PrintwithForm"].Split(',').FirstOrDefault());
                fileAtt = objCommonWrapper.EditAttachment(attachmentModel);
                if (fileAtt)
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), pmid = Convert.ToInt64(Request.Form["attachmentModel.PrevMaintMasterId"]), mode = Mode }, JsonRequestBehavior.AllowGet);
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

        #endregion
        #endregion

        #region EstimatesPart
        [HttpPost]
        public string PopulateEstimatesPart(int? draw, int? start, int? length, long PrevMasterID)
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            PrevMaintWrapper pWrapper = new PrevMaintWrapper(userData);
            var EstimatePart = pWrapper.populateEstimatedParts(PrevMasterID);
            EstimatePart = this.GetAllEstPartSortByColumnWithOrder(order, orderDir, EstimatePart);
            var totalRecords = 0;
            var recordsFiltered = 0;
            start = start.HasValue
                ? start / length
                : 0;
            recordsFiltered = EstimatePart.Count();
            totalRecords = EstimatePart.Count();
            int initialPage = start.Value;
            var filteredResult = EstimatePart
                .Skip(initialPage * length ?? 0)
                .Take(length ?? 0)
                .ToList();

            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult }, JsonSerializer12HoursDateAndTimeSettings);
        }
        private List<EstimatedCostModel> GetAllEstPartSortByColumnWithOrder(string order, string orderDir, List<EstimatedCostModel> data)
        {
            List<EstimatedCostModel> lst = new List<EstimatedCostModel>();
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
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.UnitCost).ToList() : data.OrderBy(p => p.UnitCost).ToList();
                        break;
                    case "3":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Quantity).ToList() : data.OrderBy(p => p.Quantity).ToList();
                        break;
                    case "4":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.TotalCost).ToList() : data.OrderBy(p => p.TotalCost).ToList();
                        break;
                    default:
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ClientLookupId).ToList() : data.OrderBy(p => p.ClientLookupId).ToList();
                        break;
                }
            }
            return lst;
        }
        [HttpPost]
        [ActionName("AddEstimatesPMPart")]
        public PartialViewResult AddEstimatesPart(long PrevMaintMasterID, string ClientLookupId)
        {
            PrevMaintVM objPrevMaintVM = new PrevMaintVM();
            EstimatedCostModel objEstimatedCostModel = new EstimatedCostModel();
            PrevMaintWrapper _PrevMaintObj = new PrevMaintWrapper(userData);
            EstimatePart estimatePart = new EstimatePart();
            estimatePart.PrevMaintMasterId = PrevMaintMasterID;
            estimatePart.PrevmaintClientlookUp = ClientLookupId;
            objPrevMaintVM.estimatePart = estimatePart;
            LocalizeControls(objPrevMaintVM, LocalizeResourceSetConstants.PrevMaintDetails);
            return PartialView("_AddEstimatePart", objPrevMaintVM);
        }
        [HttpPost]
        public PartialViewResult EditEstimatesPart(long PrevMaintMasterID, long EstimatedCostsId, string ClientLookupId, long CategoryId)
        {
            PrevMaintVM objPrevMaintVM = new PrevMaintVM();
            EstimatedCostModel objEstimatedCostModel = new EstimatedCostModel();
            PrevMaintWrapper _PrevMaintObj = new PrevMaintWrapper(userData);
            EstimatePart estimatePart = new EstimatePart();
            CommonWrapper commonWrapper = new CommonWrapper(userData);

            var AllLookUps = commonWrapper.GetAllLookUpList();
            var UNIT_OF_MEASURE = AllLookUps.Where(x => x.ListName == LookupListConstants.Unit_Of_Measure).ToList();
            if (UNIT_OF_MEASURE != null)
            {
                objPrevMaintVM.UnitOfmesureListWo = UNIT_OF_MEASURE.Select(x => new SelectListItem { Text = x.ListValue + ' ' + '|' + ' ' + x.Description, Value = x.ListValue.ToString() });
            }
            var resultEstimateCostsByObjectId = _PrevMaintObj.RetrieveEstimateCostsByObjectId(EstimatedCostsId, PrevMaintMasterID);

            objPrevMaintVM.PartNotInInventoryModel = resultEstimateCostsByObjectId;
            LocalizeControls(objPrevMaintVM, LocalizeResourceSetConstants.PrevMaintDetails);
            return PartialView("~/Views/PreventiveMaintenance/_EditMaterialRequest.cshtml", objPrevMaintVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEstimatesPart(PrevMaintVM prevMaintVM)
        {
            string Mode = string.Empty;
            string ModelValidationFailedMessage = string.Empty;
            if (ModelState.IsValid)
            {
                PrevMaintWrapper wrapper = new PrevMaintWrapper(userData);
                EstimatedCosts objEstimatedCost = new EstimatedCosts();
                EstimatedCostModel objEstimatedCostModel = new EstimatedCostModel();
                objEstimatedCostModel.ClientLookupId = prevMaintVM.estimatePart.ClientLookupId;
                objEstimatedCostModel.PrevMaintMasterId = prevMaintVM.estimatePart.PrevMaintMasterId;
                objEstimatedCostModel.EstimatedCostsId = prevMaintVM.estimatePart.EstimatedCostsId;
                objEstimatedCostModel.Quantity = prevMaintVM.estimatePart.Quantity;
                if (objEstimatedCostModel.EstimatedCostsId != 0)
                {
                    objEstimatedCost = wrapper.EditPart(objEstimatedCostModel);
                }
                else
                {
                    Mode = "add";
                    objEstimatedCost = wrapper.AddPart(objEstimatedCostModel);
                }
                if (objEstimatedCost.ErrorMessages != null && objEstimatedCost.ErrorMessages.Count > 0)
                {
                    return Json(objEstimatedCost.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), prevemaintmasterid = objEstimatedCostModel.PrevMaintMasterId, mode = Mode }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(new { Result = ModelValidationFailedMessage }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult DeleteEstimatesPart(long estimatedCostsId)
        {
            PrevMaintWrapper wrapper = new PrevMaintWrapper(userData);
            var deleteResult = wrapper.DeleteEstimate(estimatedCostsId);
            if (deleteResult)
            {
                return Json(new { Result = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Result = JsonReturnEnum.failed.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

        #region V2-1151 Material Request Support
        public PartialViewResult PopulateStorerooms()
        {
            PrevMaintVM prevMaintVM = new PrevMaintVM();
            var commonWrapper = new CommonWrapper(userData);
            if (userData.DatabaseKey.Client.UseMultiStoreroom)
            {
                prevMaintVM.StoreroomList = commonWrapper.GetStoreroomList(StoreroomAuthType.Issue);
            }
            LocalizeControls(prevMaintVM, LocalizeResourceSetConstants.PrevMaintDetails);
            return PartialView("_StoreroomList", prevMaintVM);
        }

        [HttpPost]
        public JsonResult SelectStoreroom(PrevMaintVM pmVM)
        {
            if (ModelState.IsValid)
            {
                return Json(new { data = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                string ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }
        public PartialViewResult AddPartInInventory(long PreventiveMaintainId, string PMClientLookupId, long StoreroomId = 0)
        {
            PartLookupVM partLookupVM = new PartLookupVM();
            var commonWrapper = new CommonWrapper(userData);
            partLookupVM.PreventiveMaintainId = PreventiveMaintainId;
            partLookupVM.PMClientLookupId = PMClientLookupId;
            partLookupVM.ShoppingCart = userData.Site.ShoppingCart;
            partLookupVM.StoreroomId = StoreroomId;
            LocalizeControls(partLookupVM, LocalizeResourceSetConstants.PartLookUpDetails);
            return PartialView("~/views/partlookup/indexWO.cshtml", partLookupVM);
        }

        [HttpGet]
        public PartialViewResult AddMaterialRequestPartNotInInventory(long PreventiveMaintId, string ClientLookupId)
        {
            PrevMaintWrapper pmWrapper = new PrevMaintWrapper(userData);
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            PrevMaintVM objVM = new PrevMaintVM()
            {
                estimatePart = new EstimatePart
                {
                    PrevMaintMasterId = PreventiveMaintId,
                    ClientLookupId = ClientLookupId,
                    ShoppingCart = userData.Site.ShoppingCart

                }
            };
            var AllLookUps = commonWrapper.GetAllLookUpList();
            var UNIT_OF_MEASURE = AllLookUps.Where(x => x.ListName == LookupListConstants.Unit_Of_Measure).ToList();
            if (UNIT_OF_MEASURE != null)
            {
                objVM.UnitOfmesureListWo = UNIT_OF_MEASURE.Select(x => new SelectListItem { Text = x.ListValue + ' ' + '|' + ' ' + x.Description, Value = x.ListValue.ToString() });
            }

            LocalizeControls(objVM, LocalizeResourceSetConstants.PrevMaintDetails);
            return PartialView("~/Views/PreventiveMaintenance/_AddMaterialRequestPartNotInInventory.cshtml", objVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddMRPartNotInInventory(PrevMaintVM objVM)
        {
            if (ModelState.IsValid)
            {
                string Mode = string.Empty;
                PrevMaintWrapper pmWrapper = new PrevMaintWrapper(userData);
                EstimatedCosts objEstimatedCost = new EstimatedCosts();
                Mode = "add";
                objEstimatedCost = pmWrapper.AddMaterialRequestPartNotInInventory(objVM);
                if (objEstimatedCost.ErrorMessages != null && objEstimatedCost.ErrorMessages.Count > 0)
                {
                    return Json(objEstimatedCost.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), pmId = objVM.estimatePart.PrevMaintMasterId, mode = Mode }, JsonRequestBehavior.AllowGet);
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
        [ValidateAntiForgeryToken]
        public ActionResult EditPartInInventory(PrevMaintVM pmObjVM)
        {
            string ModelValidationFailedMessage = string.Empty;
            PrevMaintWrapper pmWrapper = new PrevMaintWrapper(userData);
            if (ModelState.IsValid)
            {
                var lineItem = pmWrapper.EditMaterialRequest(pmObjVM);
                if (lineItem.ErrorMessages != null && lineItem.ErrorMessages.Count > 0)
                {
                    return Json(lineItem.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), prevMaintId = pmObjVM.PartNotInInventoryModel.ObjectId }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }

        }
        #endregion
        #endregion

        #region EstimatesLabor
        [HttpPost]
        public string PopulateEstimatesLabor(int? draw, int? start, int? length, long PrevMasterID)
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            PrevMaintWrapper pWrapper = new PrevMaintWrapper(userData);
            var EstimatesLabor = pWrapper.populateEstimatedLabors(PrevMasterID);
            EstimatesLabor = this.GetAllEstLaborSortByColumnWithOrder(order, orderDir, EstimatesLabor);
            var totalRecords = 0;
            var recordsFiltered = 0;
            start = start.HasValue
                ? start / length
                : 0;
            recordsFiltered = EstimatesLabor.Count();
            totalRecords = EstimatesLabor.Count();
            int initialPage = start.Value;
            var filteredResult = EstimatesLabor
                .Skip(initialPage * length ?? 0)
                .Take(length ?? 0)
                .ToList();
            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult }, JsonSerializer12HoursDateAndTimeSettings);
        }
        private List<EstimatedCostModel> GetAllEstLaborSortByColumnWithOrder(string order, string orderDir, List<EstimatedCostModel> data)
        {
            List<EstimatedCostModel> lst = new List<EstimatedCostModel>();
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
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.UnitCost).ToList() : data.OrderBy(p => p.UnitCost).ToList();
                        break;
                    case "3":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Quantity).ToList() : data.OrderBy(p => p.Quantity).ToList();
                        break;
                    case "4":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Duration).ToList() : data.OrderBy(p => p.Duration).ToList();
                        break;
                    case "5":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.TotalCost).ToList() : data.OrderBy(p => p.TotalCost).ToList();
                        break;
                    default:
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ClientLookupId).ToList() : data.OrderBy(p => p.ClientLookupId).ToList();
                        break;
                }
            }
            return lst;
        }
        [HttpPost]
        [ActionName("AddEstimatesPMLabor")]
        public PartialViewResult AddEstimatesLabor(long PrevMasterID, string ClientLookupId)
        {
            PrevMaintVM objPrevMaintVM = new PrevMaintVM();
            EstimatedCostModel objEstimatedCostModel = new EstimatedCostModel();
            EstimateLabor objEstimateLabor = new EstimateLabor();
            PrevMaintWrapper _PrevMaintObj = new PrevMaintWrapper(userData);
            objEstimateLabor.PrevMaintMasterId = PrevMasterID;
            objEstimateLabor.PrevmaintClientlookUp = ClientLookupId;
            var craftDetails = GetLookUpList_Craft();
            if (craftDetails != null)
            {
                objEstimateLabor.CraftList = craftDetails.Select(x => new SelectListItem { Text = x.ClientLookUpId + " - " + x.Description + " - " + x.ChargeRate, Value = x.CraftId.ToString() });
            }
            objPrevMaintVM.estimateLabor = objEstimateLabor;
            LocalizeControls(objPrevMaintVM, LocalizeResourceSetConstants.PrevMaintDetails);
            return PartialView("_AddEstimateLabor", objPrevMaintVM);
        }
        [HttpPost]
        public PartialViewResult EditEstimatesLabor(long PrevMaintMasterID, long EstimatedCostsId, string ClientLookupId)
        {
            PrevMaintVM objPrevMaintVM = new PrevMaintVM();
            EstimatedCostModel objEstimatedCostModel = new EstimatedCostModel();
            PrevMaintWrapper _PrevMaintObj = new PrevMaintWrapper(userData);
            EstimateLabor objEstimateLabor = new EstimateLabor();
            objEstimateLabor.PrevmaintClientlookUp = ClientLookupId;
            EstimatedCosts estimatecost = new EstimatedCosts()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                ObjectId = PrevMaintMasterID,
                EstimatedCostsId = EstimatedCostsId,
            };
            estimatecost.Retrieve(this.userData.DatabaseKey);
            if (estimatecost != null)
            {
                objEstimateLabor.PrevMaintMasterId = PrevMaintMasterID;
                objEstimateLabor.EstimatedCostsId = EstimatedCostsId;
                objEstimateLabor.Duration = estimatecost.Duration;
                objEstimateLabor.CraftId = estimatecost.CategoryId;
                objEstimateLabor.Quantity = estimatecost.Quantity;
            }
            var craftDetails = GetLookUpList_Craft();
            if (craftDetails != null)
            {
                objEstimateLabor.CraftList = craftDetails.Select(x => new SelectListItem { Text = x.ClientLookUpId + " - " + x.Description + " - " + x.ChargeRate, Value = x.CraftId.ToString() });
            }
            objPrevMaintVM.estimateLabor = objEstimateLabor;
            LocalizeControls(objPrevMaintVM, LocalizeResourceSetConstants.PrevMaintDetails);
            return PartialView("_AddEstimateLabor", objPrevMaintVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEstimatesLabor(PrevMaintVM prevMaintVM)
        {
            string Mode = string.Empty;
            string ModelValidationFailedMessage = string.Empty;
            if (ModelState.IsValid)
            {
                PrevMaintWrapper wrapper = new PrevMaintWrapper(userData);
                EstimatedCosts objEstimatedCost = new EstimatedCosts();
                EstimatedCostModel objEstimatedCostModel = new EstimatedCostModel();
                objEstimatedCostModel.EstimatedCostsId = prevMaintVM.estimateLabor.EstimatedCostsId;
                objEstimatedCostModel.PrevMaintMasterId = prevMaintVM.estimateLabor.PrevMaintMasterId;
                objEstimatedCostModel.PrevmaintClientlookUp = prevMaintVM.estimateLabor.PrevmaintClientlookUp;
                objEstimatedCostModel.Duration = prevMaintVM.estimateLabor.Duration;
                objEstimatedCostModel.CraftId = prevMaintVM.estimateLabor.CraftId;
                objEstimatedCostModel.Quantity = prevMaintVM.estimateLabor.Quantity;
                if (prevMaintVM.estimateLabor.EstimatedCostsId != 0)
                {
                    objEstimatedCost = wrapper.EditLabor(objEstimatedCostModel);
                }
                else
                {
                    Mode = "add";
                    objEstimatedCost = wrapper.AddLabor(objEstimatedCostModel);
                }
                if (objEstimatedCost.ErrorMessages != null && objEstimatedCost.ErrorMessages.Count > 0)
                {
                    return Json(objEstimatedCost.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), prevemaintmasterid = prevMaintVM.estimateLabor.PrevMaintMasterId, mode = Mode }, JsonRequestBehavior.AllowGet);
                }
            }
            ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
            return Json(new { Result = ModelValidationFailedMessage }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult DeleteEstimatesLabor(long estimatedCostsId)
        {
            PrevMaintWrapper wrapper = new PrevMaintWrapper(userData);
            var deleteResult = wrapper.DeleteEstimate(estimatedCostsId);
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

        #region EstimatesOther
        [HttpPost]
        public string PopulateEstimatesOther(int? draw, int? start, int? length, long PrevMasterID)
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            PrevMaintWrapper pWrapper = new PrevMaintWrapper(userData);
            var EstimateOther = pWrapper.populateEstimatedOthers(PrevMasterID);
            EstimateOther = this.GetAllEstOtersSortByColumnWithOrder(order, orderDir, EstimateOther);
            var totalRecords = 0;
            var recordsFiltered = 0;
            start = start.HasValue
                ? start / length
                : 0;
            recordsFiltered = EstimateOther.Count();
            totalRecords = EstimateOther.Count();
            int initialPage = start.Value;
            var filteredResult = EstimateOther
                .Skip(initialPage * length ?? 0)
                .Take(length ?? 0)
                .ToList();

            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult }, JsonSerializer12HoursDateAndTimeSettings);
        }
        private List<EstimatedCostModel> GetAllEstOtersSortByColumnWithOrder(string order, string orderDir, List<EstimatedCostModel> data)
        {
            List<EstimatedCostModel> lst = new List<EstimatedCostModel>();
            if (data != null)
            {
                switch (order)
                {
                    case "0":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Source).ToList() : data.OrderBy(p => p.Source).ToList();
                        break;
                    case "1":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.VendorClientLookupId).ToList() : data.OrderBy(p => p.VendorClientLookupId).ToList();
                        break;
                    case "2":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Description).ToList() : data.OrderBy(p => p.Description).ToList();
                        break;
                    case "3":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Quantity).ToList() : data.OrderBy(p => p.Quantity).ToList();
                        break;
                    case "4":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.TotalCost).ToList() : data.OrderBy(p => p.TotalCost).ToList();
                        break;
                    default:
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Source).ToList() : data.OrderBy(p => p.Source).ToList();
                        break;
                }
            }
            return lst;
        }
        [HttpPost]
        [ActionName("AddEstimatesPMOther")]
        public PartialViewResult AddEstimatesOther(long PrevMasterID, string ClientLookupId)
        {
            PrevMaintWrapper pmWrapper = new PrevMaintWrapper(userData);
            PrevMaintVM objPrevMaintVM = new PrevMaintVM();
            EstimateOtherModel objEstimateOther = new EstimateOtherModel();
            PreventiveMaintenanceModel objPM = new PreventiveMaintenanceModel();
            objPM.PrevMaintMasterId = PrevMasterID;
            objPM.ClientLookupId = ClientLookupId;
            objEstimateOther.PrevMaintMasterId = PrevMasterID;
            objEstimateOther.EstimatedCostsId = 0;
            var SourceTypeList = UtilityFunction.populateSourceList();
            if (SourceTypeList != null)
            {
                objEstimateOther.SourceList = SourceTypeList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
            }
            var VendorLookUpList = GetLookupList_Vendor5000();
            if (VendorLookUpList != null)
            {
                objEstimateOther.VendorLookUpList = VendorLookUpList.Select(x => new SelectListItem { Text = x.Vendor + " - " + x.Name, Value = x.VendorId.ToString() });
            }
            objPrevMaintVM.preventiveMaintenanceModel = objPM;
            objPrevMaintVM.estimateOtherModel = objEstimateOther;
            LocalizeControls(objPrevMaintVM, LocalizeResourceSetConstants.PrevMaintDetails);
            return PartialView("~/Views/PreventiveMaintenance/_AddEstimateOther.cshtml", objPrevMaintVM);
        }
        [HttpPost]
        public PartialViewResult EditEstimatesOther(long PrevMasterID, long CostId, string ClientLookupId, string VendorClientLookupId)
        {
            PrevMaintWrapper pmWrapper = new PrevMaintWrapper(userData);
            PrevMaintVM objPrevMaintVM = new PrevMaintVM();
            EstimateOtherModel eom = new EstimateOtherModel();
            EstimatedCosts eCost = new EstimatedCosts()
            {
                EstimatedCostsId = CostId
            };
            PreventiveMaintenanceModel objPM = new PreventiveMaintenanceModel();
            objPM.PrevMaintMasterId = PrevMasterID;
            objPM.ClientLookupId = ClientLookupId;
            eCost.Retrieve(userData.DatabaseKey);
            eom.EstimatedCostsId = CostId;
            eom.Description = eCost.Description;
            eom.Source = eCost.Source;
            eom.VendorId = eCost.VendorId;
            eom.UnitCost = eCost.UnitCost;
            eom.Quantity = eCost.Quantity;
            eom.PrevMaintMasterId = PrevMasterID;
            eom.UpdateIndex = eCost.UpdateIndex;
            eom.VendorClientLookupId = VendorClientLookupId;
            objPrevMaintVM.estimateOtherModel = eom;
            objPrevMaintVM.preventiveMaintenanceModel = objPM;
            var SourceTypeList = UtilityFunction.populateSourceList();
            if (SourceTypeList != null)
            {
                eom.SourceList = SourceTypeList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
            }
            var VendorLookUpList = GetLookupList_Vendor5000();
            if (VendorLookUpList != null)
            {
                eom.VendorLookUpList = VendorLookUpList.Select(x => new SelectListItem { Text = x.Vendor + " - " + x.Name, Value = x.VendorId.ToString() });
            }
            LocalizeControls(objPrevMaintVM, LocalizeResourceSetConstants.PrevMaintDetails);
            return PartialView("~/Views/PreventiveMaintenance/_AddEstimateOther.cshtml", objPrevMaintVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddEstimatesOther(PrevMaintVM prevMaintVM)
        {
            string ModelValidationFailedMessage = string.Empty;
            if (ModelState.IsValid)
            {
                string Mode = string.Empty;
                EstimatedCosts estimatecost = new EstimatedCosts()
                {
                    ClientId = this.userData.DatabaseKey.Client.ClientId,
                    EstimatedCostsId = prevMaintVM.estimateOtherModel.EstimatedCostsId,
                    ObjectId = prevMaintVM.estimateOtherModel.PrevMaintMasterId,
                    ObjectType = "PrevMaintMaster",
                    Category = "Other"
                };
                if (userData.Site.UseVendorMaster)
                {
                    estimatecost.VendorId = 0;
                }
                else
                {
                    estimatecost.VendorId = prevMaintVM.estimateOtherModel.VendorId ?? 0;
                }
                estimatecost.CategoryId = 0;
                estimatecost.Description = prevMaintVM.estimateOtherModel.Description ?? " ";
                estimatecost.UnitCost = prevMaintVM.estimateOtherModel.UnitCost ?? 0;
                estimatecost.Quantity = prevMaintVM.estimateOtherModel.Quantity ?? 0;
                estimatecost.Source = prevMaintVM.estimateOtherModel.Source;
                estimatecost.UpdateIndex = prevMaintVM.estimateOtherModel.UpdateIndex;
                if (prevMaintVM.estimateOtherModel.EstimatedCostsId == 0)
                {
                    Mode = "add";
                    estimatecost.Create(userData.DatabaseKey);
                }
                else
                {
                    Mode = "update";
                    estimatecost.Update(userData.DatabaseKey);
                }
                return Json(new { Result = JsonReturnEnum.success.ToString(), pmid = prevMaintVM.estimateOtherModel.PrevMaintMasterId, mode = Mode }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult DeleteEstimatesOther(long estimatedCostId)
        {
            PrevMaintWrapper wrapper = new PrevMaintWrapper(userData);
            var deleteResult = wrapper.DeleteEstimate(estimatedCostId);
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

        #region EstimatesSummery
        [HttpPost]
        public string PopulateEstimatesSummery(long PrevMasterID)
        {
            PrevMaintWrapper pWrapper = new PrevMaintWrapper(userData);
            var EstimateSummery = pWrapper.populateEstimatedSummery(PrevMasterID);
            return JsonConvert.SerializeObject(new { data = EstimateSummery }, JsonSerializer12HoursDateAndTimeSettings);
        }

        #endregion

        #region PMSchedulingReassign
        [HttpGet]
        public JsonResult GetPMSAll(string AssignedTo, string PMID, string Description, string ChargeTo, string ChargeToName, DateTime? NextDue)
        {
            PrevMaintVM objPrevMaintVM = new PrevMaintVM();
            PreventiveMaintenanceModel pmModel = new PreventiveMaintenanceModel();
            PrevMaintWrapper pWrapper = new PrevMaintWrapper(userData);
            List<PMSchedulingReassignModel> PMSchedulingReassignList = pWrapper.populatePMSchedulingReassign();
            if (PMSchedulingReassignList != null)
            {

                #region Advance Search
                if (!string.IsNullOrEmpty(AssignedTo))
                {
                    AssignedTo = AssignedTo.ToUpper();
                    PMSchedulingReassignList = PMSchedulingReassignList.Where(x => (!string.IsNullOrWhiteSpace(x.AssignedTo) && x.AssignedTo.ToUpper().Contains(AssignedTo))).ToList();
                }
                if (!string.IsNullOrEmpty(PMID))
                {
                    PMID = PMID.ToUpper();
                    PMSchedulingReassignList = PMSchedulingReassignList.Where(x => (!string.IsNullOrWhiteSpace(x.PMID) && x.PMID.ToUpper().Contains(PMID))).ToList();
                }
                if (!string.IsNullOrEmpty(Description))
                {
                    Description = Description.ToUpper();
                    PMSchedulingReassignList = PMSchedulingReassignList.Where(x => (!string.IsNullOrWhiteSpace(x.Description.Trim()) && x.Description.ToUpper().Trim().Contains(Description))).ToList();
                }
                if (!string.IsNullOrEmpty(ChargeTo))
                {
                    ChargeTo = ChargeTo.ToUpper();
                    PMSchedulingReassignList = PMSchedulingReassignList.Where(x => (!string.IsNullOrWhiteSpace(x.ChargeTo) && x.ChargeTo.ToUpper().Contains(ChargeTo))).ToList();
                }
                if (!string.IsNullOrEmpty(ChargeToName))
                {
                    ChargeToName = ChargeToName.ToUpper();
                    PMSchedulingReassignList = PMSchedulingReassignList.Where(x => (!string.IsNullOrWhiteSpace(x.ChargeToName) && x.ChargeToName.ToUpper().Contains(ChargeToName))).ToList();
                }
                if (NextDue != null)
                {
                    PMSchedulingReassignList = PMSchedulingReassignList.Where(x => (x.NextDue != null && x.NextDue.Equals(NextDue.Value.Date))).ToList();
                }
                #endregion
            }
            LocalizeControls(objPrevMaintVM, LocalizeResourceSetConstants.PrevMaintDetails);
            return Json(PMSchedulingReassignList, JsonRequestBehavior.AllowGet);
        }
        public PartialViewResult PMSchedulingReassignGrid()
        {
            PrevMaintVM objPrevMaintVM = new PrevMaintVM();
            PreventiveMaintenanceModel pmModel = new PreventiveMaintenanceModel();
            PrevMaintReassignModel prevMaintReassignModel = new PrevMaintReassignModel();
            var PersonnelLookUplist = GetList_Personnel();
            if (PersonnelLookUplist != null)
            {
                prevMaintReassignModel.PersonnelIdList = PersonnelLookUplist.Select(x => new SelectListItem { Text = x.AssignedTo_PersonnelClientLookupId + " - " + x.NameFirst + " - " + x.NameLast, Value = x.AssignedTo_PersonnelId.ToString() });
            }
            objPrevMaintVM.prevMaintReassignModel = prevMaintReassignModel;
            PrevMaintWrapper pWrapper = new PrevMaintWrapper(userData);
            LocalizeControls(objPrevMaintVM, LocalizeResourceSetConstants.PrevMaintDetails);
            return PartialView("~/Views/PreventiveMaintenance/PMSchedulingRecordsReAssign.cshtml", objPrevMaintVM);
        }
        [HttpPost]
        public string GetPMSchedulingReassign(int? draw, int? start, int? length, string AssignedTo, string PMID, string Description,
            string ChargeTo, string ChargeToName, DateTime? NextDue)
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            PrevMaintVM objPrevMaintVM = new PrevMaintVM();
            PreventiveMaintenanceModel pmModel = new PreventiveMaintenanceModel();
            PrevMaintWrapper pWrapper = new PrevMaintWrapper(userData);
            start = start.HasValue
               ? start / length
               : 0;
            int skip = start * length ?? 0;
            string _NextDue = string.Empty;
            _NextDue = NextDue.HasValue ? NextDue.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : string.Empty;
            List<PMSchedulingReassignModel> PMSchedulingReassignList = pWrapper.populatePMSchedulingReassign(order, orderDir, skip, length ?? 0, AssignedTo, PMID, Description, ChargeTo, ChargeToName, _NextDue);
            var recordsFiltered = 0;
            var totalRecords = 0;

            start = start.HasValue
                ? start / length
                : 0;

            recordsFiltered = PMSchedulingReassignList.Select(o => o.TotalCount).FirstOrDefault();
            totalRecords = PMSchedulingReassignList.Select(o => o.TotalCount).FirstOrDefault();
            var filteredResult = PMSchedulingReassignList.ToList();
            LocalizeControls(objPrevMaintVM, LocalizeResourceSetConstants.PrevMaintDetails);
            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult }, JsonSerializerDateSettings);

        }
        public JsonResult PMSScheduleReassign(PrevMaintVM ovm)
        {
            string ModelValidationFailedMessage = string.Empty;
            string result = string.Empty;
            string IsAddOrUpdate = string.Empty;
            PrevMaintSched obj = new PrevMaintSched();
            long PersonnelId = ovm.prevMaintReassignModel.PersonnelId;
            if (ModelState.IsValid)
            {
                PrevMaintWrapper pWrapper = new PrevMaintWrapper(userData);
                if (ovm.prevMaintReassignModel != null && ovm.prevMaintReassignModel.PrevMainIdsList != null && ovm.prevMaintReassignModel.PrevMainIdsList.Length > 0)
                {
                    obj = pWrapper.UpdatePMSchedulingReassign(PersonnelId, ovm.prevMaintReassignModel.PrevMainIdsList.Split(','));
                    if (obj.ErrorMessages != null && obj.ErrorMessages.Count > 0)
                    {
                        return Json(obj.ErrorMessages, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { Issuccess = true }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    result = ErrorMessageConstants.Invalid_Preventive_Maintenance_Id;
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult PMSFilterByAssigned(PrevMaintVM ovm)
        {
            string ModelValidationFailedMessage = string.Empty;
            string result = string.Empty;
            string IsAddOrUpdate = string.Empty;
            PrevMaintSched objPM = new PrevMaintSched();
            long PersonnelId = ovm.prevMaintReassignModel.PersonnelId;
            if (ModelState.IsValid)
            {
                PrevMaintWrapper pWrapper = new PrevMaintWrapper(userData);
                if (ovm.prevMaintReassignModel != null && ovm.prevMaintReassignModel.PrevMainIdsList != null && ovm.prevMaintReassignModel.PrevMainIdsList.Length > 0)
                {
                    PrevMaintSched obj = pWrapper.UpdatePMSchedulingReassign(PersonnelId, ovm.prevMaintReassignModel.PrevMainIdsList.Split(','));
                    if (obj.ErrorMessages != null && obj.ErrorMessages.Count > 0)
                    {
                        return Json(obj.ErrorMessages, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { Issuccess = true }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    result = ErrorMessageConstants.Invalid_Preventive_Maintenance_Id;
                    List<string> error = new List<string>() { result };
                    objPM.ErrorMessages = error;
                    return Json(objPM.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                List<string> error = new List<string>() { ModelValidationFailedMessage };
                objPM.ErrorMessages = error;
                return Json(objPM.ErrorMessages, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult PMSFilterByEquipment(PrevMaintVM ovm)
        {
            string ModelValidationFailedMessage = string.Empty;
            string result = string.Empty;
            string IsAddOrUpdate = string.Empty;
            PrevMaintSched objPM = new PrevMaintSched();
            long PersonnelId = ovm.prevMaintReassignModel.EquipmentId;
            if (ModelState.IsValid)
            {
                PrevMaintWrapper pWrapper = new PrevMaintWrapper(userData);
                if (ovm.prevMaintReassignModel.PrevMainIdsList.Length > 0)
                {
                    PrevMaintSched obj = pWrapper.UpdatePMSchedulingReassign(PersonnelId, ovm.prevMaintReassignModel.PrevMainIdsList.Split(','));
                    if (obj.ErrorMessages != null && obj.ErrorMessages.Count > 0)
                    {
                        return Json(obj.ErrorMessages, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { Issuccess = true }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    result = ErrorMessageConstants.Invalid_Preventive_Maintenance_Id;
                    List<string> error = new List<string>() { result };
                    objPM.ErrorMessages = error;
                    return Json(objPM.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                List<string> error = new List<string>() { ModelValidationFailedMessage };
                objPM.ErrorMessages = error;
                return Json(objPM.ErrorMessages, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult PMSFilterByLocation(PrevMaintVM ovm)
        {
            string ModelValidationFailedMessage = string.Empty;
            string result = string.Empty;
            string IsAddOrUpdate = string.Empty;
            PrevMaintSched objPM = new PrevMaintSched();
            long PersonnelId = ovm.prevMaintReassignModel.LocationId;
            if (ModelState.IsValid)
            {
                PrevMaintWrapper pWrapper = new PrevMaintWrapper(userData);
                if (ovm.prevMaintReassignModel.PrevMainIdsList.Length > 0)
                {
                    PrevMaintSched obj = pWrapper.UpdatePMSchedulingReassign(PersonnelId, ovm.prevMaintReassignModel.PrevMainIdsList.Split(','));
                    if (obj.ErrorMessages != null && obj.ErrorMessages.Count > 0)
                    {
                        return Json(obj.ErrorMessages, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { Issuccess = true }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    result = ErrorMessageConstants.Invalid_Preventive_Maintenance_Id;
                    List<string> error = new List<string>() { result };
                    objPM.ErrorMessages = error;
                    return Json(objPM.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                List<string> error = new List<string>() { ModelValidationFailedMessage };
                objPM.ErrorMessages = error;
                return Json(objPM.ErrorMessages, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Options
        [HttpPost]
        public JsonResult DeletePM(long _PMid)
        {
            string result = string.Empty;
            List<String> errorList = new List<string>();
            PreventiveMaintenanceModel pmModel = new PreventiveMaintenanceModel();
            PrevMaintWrapper pWrapper = new PrevMaintWrapper(userData);
            errorList = pWrapper.DeletePM(_PMid);
            if (errorList != null && errorList.Count > 0)
            {
                return Json(errorList, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Result = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePrevMaintMasterID(PrevMaintVM prevMaintVM)
        {
            string ModelValidationFailedMessage = string.Empty;
            List<String> errorList = new List<string>();
            if (ModelState.IsValid)
            {
                PrevMaintVM objprevMaintVM = new PrevMaintVM();
                ChangePreventiveIDModel _ChangePreventiveIDModel = new ChangePreventiveIDModel();
                PrevMaintWrapper pWrapper = new PrevMaintWrapper(userData);
                long _pvId = prevMaintVM._ChangePreventiveIDModel.PrevMaintMasterId;
                _ChangePreventiveIDModel = prevMaintVM._ChangePreventiveIDModel;
                objprevMaintVM.createdLastUpdatedPMModel = pWrapper.createdLastUpdatedModel(_pvId);
                errorList = pWrapper.ChangePrevMaintId(_ChangePreventiveIDModel);
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
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region Private Methods
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
        #endregion

        #region Instruction
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult AddOrUpdateInstructions(PrevMaintVM prevMaintVM)
        {
            CommonWrapper coWrapper = new CommonWrapper(userData);

            if (ModelState.IsValid)
            {
                string Mode = string.Empty;
                List<String> errorList = new List<string>();
                errorList = coWrapper.AddOrUpdateInstruction(prevMaintVM.InstructionPMModel, ref Mode, AttachmentTableConstant.PreventiveMaintenance);
                if (errorList != null && errorList.Count > 0)
                {
                    return Json(errorList, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), PrevMaintId = prevMaintVM.InstructionPMModel.ObjectId, mode = Mode }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                string ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpGet]
        public JsonResult PopulateInstructions(long PrevMaintId)
        {
            PrevMaintVM objPrevMaintVM = new PrevMaintVM();
            CommonWrapper objCommonWrapper = new CommonWrapper(userData);
            var instructiondata = objCommonWrapper.PopulateInstructions(PrevMaintId, AttachmentTableConstant.PreventiveMaintenance);
            objPrevMaintVM.InstructionPMModel = instructiondata.FirstOrDefault();
            if (instructiondata.Count != 0)
            {
                return Json(new { instructionid = instructiondata[0].InstructionId, contents = instructiondata[0].Content }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { instructionid = 0, contents = "" }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Print
        [HttpPost]
        public JsonResult SetPrintPMList(List<PMPrintInputModel> PrevMaintMasterIds)
        {
            Session["PrintPMList"] = PrevMaintMasterIds;
            return Json(new { success = true });
        }
        [EncryptedActionParameter]
        public ActionResult GeneratePMPrint()
        {
            List<PMPrintInputModel> PrevMaintMasterIds = new List<PMPrintInputModel>();
            if (Session["PrintPMList"] != null)
            {
                PrevMaintMasterIds = (List<PMPrintInputModel>)Session["PrintPMList"];
            }
            var objPrintModelList = PrintPMUsingDevExpress(PrevMaintMasterIds);
            return View("PMDevExpressPrint", objPrintModelList);
        }
        private List<PMDevExpressPrintModel> PrintPMUsingDevExpress(List<PMPrintInputModel> PrevMaintMasterIds)
        {
            var PMDevExpressPrintModelList = RetrieveDataFromTables(PrevMaintMasterIds);
            return PMDevExpressPrintModelList;
        }
        private List<PMDevExpressPrintModel> RetrieveDataFromTables(List<PMPrintInputModel> PrevMaintMasterIds)
        {
            Task[] tasks = null;
            var PMDevExpressPrintModelList = new List<PMDevExpressPrintModel>();
            PMDevExpressPrintModel model;
            PreventiveMaintenanceModel preventiveMaintenanceModel;
            List<ScheduleRecords> ScheduleRecordsList;
            List<PrevMaintTaskModel> TaskList;
            List<EstimatedCostModel> PartsList;
            List<EstimatedCostModel> LaborsList;
            List<EstimatedCostModel> OthersList;
            List<EstimatedCostModel> SummaryList;
            List<InstructionModel> InstructionsList;
            var prevmaintWrapper = new PrevMaintWrapper(userData);
            var comWrapper = new CommonWrapper(userData);
            var ImageUrl = GenerateImageUrlDevExpress();
            for (int i = 0; i < PrevMaintMasterIds.Count; i++)
            {
                tasks = new Task[7];
                model = new PMDevExpressPrintModel();

                tasks[0] = Task.Factory.StartNew(() =>
                {
                    preventiveMaintenanceModel = prevmaintWrapper.populateMaintenanceDetails(PrevMaintMasterIds[i].PrevMaintMasterId);
                    BindPMDetailsPrintModel(preventiveMaintenanceModel, ref model, ImageUrl);
                });
                ScheduleRecordsList = prevmaintWrapper.GetScheduleRecords_V2(PrevMaintMasterIds[i].PrevMaintMasterId);
                if (ScheduleRecordsList.Count > 0)
                {
                    BindScheduledRecordsPrintModel(ScheduleRecordsList, ref model);
                }
                tasks[1] = Task.Factory.StartNew(() =>
                {
                    TaskList = prevmaintWrapper.populateTaskList(PrevMaintMasterIds[i].PrevMaintMasterId, PrevMaintMasterIds[i].PrevMaintLibraryID);
                    if (TaskList.Count > 0)
                    {
                        BindTasksPrintModel(TaskList, ref model);
                    }
                });

                tasks[2] = Task.Factory.StartNew(() =>
                {
                    PartsList = prevmaintWrapper.populateEstimatedParts(PrevMaintMasterIds[i].PrevMaintMasterId);
                    if (PartsList.Count > 0)
                    {
                        BindPartPrintModel(PartsList, ref model);
                    }
                });

                tasks[3] = Task.Factory.StartNew(() =>
                {
                    LaborsList = prevmaintWrapper.populateEstimatedLabors(PrevMaintMasterIds[i].PrevMaintMasterId);
                    if (LaborsList.Count > 0)
                    {
                        BindLaborPrintModel(LaborsList, ref model);
                    }
                });

                tasks[4] = Task.Factory.StartNew(() =>
                {
                    OthersList = prevmaintWrapper.populateEstimatedOthers(PrevMaintMasterIds[i].PrevMaintMasterId);
                    if (OthersList.Count > 0)
                    {
                        BindOthersPrintModel(OthersList, ref model);
                    }
                });

                tasks[5] = Task.Factory.StartNew(() =>
                {
                    SummaryList = prevmaintWrapper.populateEstimatedSummery(PrevMaintMasterIds[i].PrevMaintMasterId);
                    if (SummaryList.Count > 0)
                    {
                        BindSummaryPrintModel(SummaryList, ref model);
                    }
                });

                tasks[6] = Task.Factory.StartNew(() =>
                {
                    InstructionsList = comWrapper.PopulateInstructions(PrevMaintMasterIds[i].PrevMaintMasterId, "PreventiveMaintenance");
                    if (InstructionsList.Count > 0)
                    {
                        model.InstructionsList = InstructionsList;
                    }
                });
                Task.WaitAll(tasks);
                model.OnPremise = userData.DatabaseKey.Client.OnPremise;
                PMDevExpressPrintModelList.Add(model);
            }

            BindLocalizations(PMDevExpressPrintModelList);
            return PMDevExpressPrintModelList;
        }
        private string GenerateImageUrl()
        {
            CommonWrapper comWrapper = new CommonWrapper(userData);
            var ImagePath = comWrapper.GetClientLogoUrl();
            if (string.IsNullOrEmpty(ImagePath))
            {
                var path = "~/Scripts/ImageZoom/images/NoImage.jpg";
                ImagePath = Request.Url.Scheme + "://" + Request.Url.Authority + Url.Content(path);
            }
            else if (ImagePath.StartsWith("../"))
            {
                ImagePath = ImagePath.Replace("../", Request.Url.Scheme + "://" + Request.Url.Authority + "/");
            }
            return ImagePath;
        }
        private string GenerateImageUrlDevExpress()
        {
            CommonWrapper comWrapper = new CommonWrapper(userData);
            var ImagePath = comWrapper.GetClientLogoUrlForDevExpressPrint();
            if (string.IsNullOrEmpty(ImagePath))
            {
                var path = "~/Scripts/ImageZoom/images/NoImage.jpg";
                ImagePath = Request.Url.Scheme + "://" + Request.Url.Authority + Url.Content(path);
            }
            else if (ImagePath.StartsWith("../"))
            {
                ImagePath = ImagePath.Replace("../", Request.Url.Scheme + "://" + Request.Url.Authority + "/");
            }
            return ImagePath;
        }
        private void BindPMDetailsPrintModel(PreventiveMaintenanceModel preventive, ref PMDevExpressPrintModel model, string ImageUrl)
        {
            model.MasterJobId = preventive.ClientLookupId;
            model.Type = preventive.Type;
            model.Inactive = preventive.InactiveFlag;
            model.Description = preventive.Description;
            model.ScheduleType = preventive.ScheduleType;
            model.JobDuration = preventive.JobDuration ?? 0;
            model.CurrentDateTime = Convert.ToDateTime(DateTime.UtcNow.ToUserTimeZone(userData.Site.TimeZone)).ToString("MM/dd/yyyy HH:mm", CultureInfo.InvariantCulture);
            model.AzureImageUrl = ImageUrl;
        }
        private void BindScheduledRecordsPrintModel(List<ScheduleRecords> ScheduleRecordsList, ref PMDevExpressPrintModel printmodel)
        {
            var modelList = new List<PMScheduledRecordsDevExpressPrintModel>();
            PMScheduledRecordsDevExpressPrintModel model;
            foreach (var item in ScheduleRecordsList)
            {
                model = new PMScheduledRecordsDevExpressPrintModel();
                model.ChargeType = item.ChargeType;
                model.ChargeToClientLookupId = item.ChargeToClientLookupId;
                model.AssignedTo_PersonnelClientLookupId = item.AssignedTo_PersonnelClientLookupId;
                model.Frequency = item.Frequency;
                model.FrequencyType = item.FrequencyType;
                model.NextDueDateString = item.NextDueDateString;
                model.Scheduled = item.Scheduled;
                modelList.Add(model);
            }
            printmodel.SchdeduledRecordsList = modelList;
        }
        private void BindTasksPrintModel(List<PrevMaintTaskModel> TaskList, ref PMDevExpressPrintModel printmodel)
        {
            var modelList = new List<PMTaskDevExpressPrintModel>();
            PMTaskDevExpressPrintModel model;
            foreach (var item in TaskList)
            {
                model = new PMTaskDevExpressPrintModel();
                model.TaskNumber = item.TaskNumber;
                model.Description = item.Description;
                model.ChargeType = item.ChargeType;
                model.ChargeToClientLookupId = item.ChargeToClientLookupId;

                modelList.Add(model);
            }
            printmodel.TasksList = modelList;
        }
        private void BindPartPrintModel(List<EstimatedCostModel> PartList, ref PMDevExpressPrintModel printmodel)
        {
            var modelList = new List<PMPartDevExpressPrintModel>();
            PMPartDevExpressPrintModel model;
            foreach (var item in PartList)
            {
                model = new PMPartDevExpressPrintModel();
                model.PartClientlookupId = item.ClientLookupId;
                model.Description = item.Description;
                model.UnitCost = item.UnitCost;
                model.Quantity = item.Quantity;
                model.TotalCost = item.TotalCost;
                modelList.Add(model);
            }
            printmodel.PartsList = modelList;
        }
        private void BindLaborPrintModel(List<EstimatedCostModel> LaborList, ref PMDevExpressPrintModel printmodel)
        {
            var modelList = new List<PMLaborDevExpressPrintModel>();
            PMLaborDevExpressPrintModel model;
            foreach (var item in LaborList)
            {
                model = new PMLaborDevExpressPrintModel();
                model.Craft = item.ClientLookupId;
                model.Description = item.Description;
                model.UnitCost = item.UnitCost;
                model.Quantity = item.Quantity;
                model.Duration = item.Duration;
                model.TotalCost = item.TotalCost;

                modelList.Add(model);
            }
            printmodel.LaborsList = modelList;
        }
        private void BindOthersPrintModel(List<EstimatedCostModel> OthersList, ref PMDevExpressPrintModel printmodel)
        {
            var modelList = new List<PMOtherDevExpressPrintModel>();
            PMOtherDevExpressPrintModel model;
            foreach (var item in OthersList)
            {
                model = new PMOtherDevExpressPrintModel();
                model.Source = item.Source;
                model.Description = item.Description;
                model.VendorClientLookupId = item.VendorClientLookupId;
                model.UnitCost = item.UnitCost;
                model.Quantity = item.Quantity;
                model.TotalCost = item.TotalCost;
                modelList.Add(model);
            }
            printmodel.OthersList = modelList;
        }
        private void BindSummaryPrintModel(List<EstimatedCostModel> SummaryList, ref PMDevExpressPrintModel printmodel)
        {
            var modelList = new List<PMSummaryDevExpressPrintModel>();
            PMSummaryDevExpressPrintModel model;
            foreach (var item in SummaryList)
            {
                model = new PMSummaryDevExpressPrintModel();
                model.TotalPartCost = item.TotalPartCost;
                model.TotalLaborHours = item.TotalLaborHours;
                model.TotalCraftCost = item.TotalCraftCost;
                model.TotalExternalCost = item.TotalExternalCost;
                model.TotalInternalCost = item.TotalInternalCost;
                model.TotalSummeryCost = item.TotalSummeryCost;

                modelList.Add(model);
            }
            printmodel.SummaryList = modelList;
        }
        private void BindLocalizations(List<PMDevExpressPrintModel> models)
        {
            #region Details
            string spnPMMasterJob = UtilityFunction.GetMessageFromResource("", LocalizeResourceSetConstants.Global);
            string spnMasterJobID = UtilityFunction.GetMessageFromResource("spnPrevMstrId", LocalizeResourceSetConstants.PrevMaintDetails);
            string spnType = UtilityFunction.GetMessageFromResource("GlobalType", LocalizeResourceSetConstants.Global);
            string spnInactive = UtilityFunction.GetMessageFromResource("globalInActive", LocalizeResourceSetConstants.Global);
            string spnDescription = UtilityFunction.GetMessageFromResource("spnDescription", LocalizeResourceSetConstants.Global);
            string spnScheduleType = UtilityFunction.GetMessageFromResource("spnScheduleType", LocalizeResourceSetConstants.Global);
            string spnJobDuration = UtilityFunction.GetMessageFromResource("spnJobDuration", LocalizeResourceSetConstants.PrevMaintDetails);
            string spnAddInstructions = UtilityFunction.GetMessageFromResource("spnAddInstructions", LocalizeResourceSetConstants.Global);
            string spnEquipment = UtilityFunction.GetMessageFromResource("Equipment", LocalizeResourceSetConstants.SecurityProfileItemsDetails);
            string spnLocation = UtilityFunction.GetMessageFromResource("spnLocation", LocalizeResourceSetConstants.Global);
            #endregion

            #region Scheduled Records
            string spnScheduleRecords = "", spnChargeType = "", spnChargeTo = "", spnAssignedTo = "", spnOnDemandGroup = "", spnPerformEvery = "", spnNextDue = "", spnScheduled = "";
            if (models.Select(x => x.SchdeduledRecordsList).Any(x => x.Count() > 0))
            {
                spnScheduleRecords = UtilityFunction.GetMessageFromResource("spnScheduleRecords", LocalizeResourceSetConstants.PrevMaintDetails);
                spnChargeType = UtilityFunction.GetMessageFromResource("spnChargeType", LocalizeResourceSetConstants.Global);
                spnChargeTo = UtilityFunction.GetMessageFromResource("GlobalChargeTo", LocalizeResourceSetConstants.Global);
                spnAssignedTo = UtilityFunction.GetMessageFromResource("spnAssignedTo", LocalizeResourceSetConstants.Global);
                spnOnDemandGroup = UtilityFunction.GetMessageFromResource("spnFrequencyType", LocalizeResourceSetConstants.Global);
                spnPerformEvery = UtilityFunction.GetMessageFromResource("spnPerformEvery", LocalizeResourceSetConstants.PrevMaintDetails);
                spnNextDue = UtilityFunction.GetMessageFromResource("spnNextDue", LocalizeResourceSetConstants.Global);
                spnScheduled = UtilityFunction.GetMessageFromResource("spnGlobalScheduled", LocalizeResourceSetConstants.Global);
            }
            #endregion

            #region Tasks
            string spnTasks = "", spnOrder = "", spnTaskChargeType = "", spnTaskChargeTo = "";
            if (models.Select(x => x.TasksList).Any(x => x.Count() > 0))
            {
                spnTasks = UtilityFunction.GetMessageFromResource("spnTasks", LocalizeResourceSetConstants.Global);
                spnOrder = UtilityFunction.GetMessageFromResource("spnOrder", LocalizeResourceSetConstants.Global);
                spnTaskChargeType = spnChargeType;
                spnTaskChargeTo = spnChargeTo;
            }
            #endregion

            #region Parts
            string spnParts = "", spnPart = "", spnUnitCost = "", spnQuantity = "", spnTotalCost = "";
            if (models.Select(x => x.PartsList).Any(x => x.Count() > 0))
            {
                spnParts = UtilityFunction.GetMessageFromResource("spnParts", LocalizeResourceSetConstants.Global);
                spnPart = UtilityFunction.GetMessageFromResource("spnPart", LocalizeResourceSetConstants.Global);
                spnUnitCost = UtilityFunction.GetMessageFromResource("spnUnitCost", LocalizeResourceSetConstants.Global);
                spnQuantity = UtilityFunction.GetMessageFromResource("spnQuantity", LocalizeResourceSetConstants.Global); ;
                spnTotalCost = UtilityFunction.GetMessageFromResource("spnTotalCost", LocalizeResourceSetConstants.Global); ;
            }
            #endregion

            #region Labor
            string spnCraft = "", spnLaborUnitCost = "", spnLaborQuantity = "", spnLaborDuration = "", spnLaborTotalCost = "", spnLabor = "";
            if (models.Select(x => x.LaborsList).Any(x => x.Count() > 0))
            {
                spnLabor = UtilityFunction.GetMessageFromResource("spnLabor", LocalizeResourceSetConstants.Global);
                spnCraft = UtilityFunction.GetMessageFromResource("spnCraft", LocalizeResourceSetConstants.Global);
                spnLaborUnitCost = UtilityFunction.GetMessageFromResource("spnUnitCost", LocalizeResourceSetConstants.Global);
                spnLaborQuantity = UtilityFunction.GetMessageFromResource("spnQuantity", LocalizeResourceSetConstants.Global);
                spnLaborDuration = UtilityFunction.GetMessageFromResource("spnDuration", LocalizeResourceSetConstants.Global);
                spnLaborTotalCost = UtilityFunction.GetMessageFromResource("spnTotalCost", LocalizeResourceSetConstants.Global); ;
            }
            #endregion

            #region Other
            string spnSource = "", spnVendor = "", spnOtherUnitCost = "", spnOtherQuantity = "", spnOtherTotalCost = "", spnOther = "";
            if (models.Select(x => x.OthersList).Any(x => x.Count() > 0))
            {
                spnOther = UtilityFunction.GetMessageFromResource("spnOther", LocalizeResourceSetConstants.Global);
                spnSource = UtilityFunction.GetMessageFromResource("spnSource", LocalizeResourceSetConstants.Global);
                spnVendor = UtilityFunction.GetMessageFromResource("GlobalVendor", LocalizeResourceSetConstants.Global);
                spnOtherUnitCost = UtilityFunction.GetMessageFromResource("spnUnitCost", LocalizeResourceSetConstants.Global);
                spnOtherQuantity = UtilityFunction.GetMessageFromResource("spnQuantity", LocalizeResourceSetConstants.Global);
                spnOtherTotalCost = UtilityFunction.GetMessageFromResource("spnTotalCost", LocalizeResourceSetConstants.Global);
            }
            #endregion

            #region Summary
            string spnPartsCosts = "", spnLaborHours = "", spnCraftCosts = "", spnOtherExternalCosts = "", spnOtherInternalCosts = "", spnSummaryTotalCosts = "", spnSummary = "";
            if (models.Select(x => x.SummaryList).Any(x => x.Count() > 0))
            {
                spnSummary = UtilityFunction.GetMessageFromResource("spnSummary", LocalizeResourceSetConstants.Global);
                spnPartsCosts = UtilityFunction.GetMessageFromResource("spnPartsCosts", LocalizeResourceSetConstants.Global);
                spnLaborHours = UtilityFunction.GetMessageFromResource("spnTotalLaborHours", LocalizeResourceSetConstants.Global);
                spnCraftCosts = UtilityFunction.GetMessageFromResource("spnTotalCraftCost", LocalizeResourceSetConstants.PrevMaintDetails);
                spnOtherExternalCosts = UtilityFunction.GetMessageFromResource("spnOtherExternalCosts", LocalizeResourceSetConstants.Global);
                spnOtherInternalCosts = UtilityFunction.GetMessageFromResource("spnOtherInternalCosts", LocalizeResourceSetConstants.PrevMaintDetails);
                spnSummaryTotalCosts = UtilityFunction.GetMessageFromResource("spnTotalCost", LocalizeResourceSetConstants.Global); ;
            }
            #endregion

            foreach (var model in models)
            {
                model.spnPMMasterJob = spnPMMasterJob;
                model.spnMasterJobID = spnMasterJobID;
                model.spnType = spnType;
                model.spnInactive = spnInactive;
                model.spnDescription = spnDescription;
                model.spnScheduleType = spnScheduleType;
                model.spnJobDuration = spnJobDuration;
                model.spnAddInstructions = spnAddInstructions;
                foreach (var item in model.SchdeduledRecordsList)
                {
                    item.spnScheduleRecords = spnScheduleRecords;
                    item.spnChargeType = spnChargeType;
                    item.spnChargeTo = spnChargeTo;
                    item.spnAssignedTo = spnAssignedTo;
                    item.spnOnDemandGroup = spnOnDemandGroup;
                    item.spnPerformEvery = spnPerformEvery;
                    item.spnNextDue = spnNextDue;
                    item.spnScheduled = spnScheduled;

                    if (item.ChargeType == ChargeType.Equipment)
                    {
                        item.ChargeType = spnEquipment;
                    }
                    else if (item.ChargeType == ChargeType.Location)
                    {
                        item.ChargeType = spnLocation;
                    }
                }

                foreach (var item in model.TasksList)
                {
                    item.spnTasks = spnTasks;
                    item.spnOrder = spnOrder;
                    item.spnTaskDescription = spnDescription;
                    item.spnTaskChargeTo = spnTaskChargeTo;
                    item.spnTaskChargeType = spnTaskChargeType;
                    if (item.ChargeType == ChargeType.Equipment)
                    {
                        item.ChargeType = spnEquipment;
                    }
                    else if (item.ChargeType == ChargeType.Location)
                    {
                        item.ChargeType = spnLocation;
                    }
                }

                foreach (var item in model.PartsList)
                {
                    item.spnParts = spnParts;
                    item.spnPart = spnPart;
                    item.spnDescription = spnDescription;
                    item.spnUnitCost = spnUnitCost;
                    item.spnTotalCost = spnTotalCost;
                    item.spnQuantity = spnQuantity;
                }

                foreach (var item in model.LaborsList)
                {
                    item.spnLabor = spnLabor;
                    item.spnCraft = spnCraft;
                    item.spnDescription = spnDescription;
                    item.spnUnitCost = spnLaborUnitCost;
                    item.spnTotalCost = spnLaborTotalCost;
                    item.spnQuantity = spnLaborQuantity;
                    item.spnLaborDuration = spnLaborDuration;
                }

                foreach (var item in model.OthersList)
                {
                    item.spnOther = spnOther;
                    item.spnSource = spnSource;
                    item.spnOtherDescription = spnDescription;
                    item.spnVendor = spnVendor;
                    item.spnOtherUnitCost = spnOtherUnitCost;
                    item.spnOtherQuantity = spnOtherQuantity;
                    item.spnOtherTotalCost = spnOtherTotalCost;
                }

                foreach (var item in model.SummaryList)
                {
                    item.spnSummary = spnSummary;
                    item.spnPartsCosts = spnPartsCosts;
                    item.spnLaborHours = spnLaborHours;
                    item.spnCraftCosts = spnCraftCosts;
                    item.spnOtherExternalCosts = spnOtherExternalCosts;
                    item.spnOtherInternalCosts = spnOtherInternalCosts;
                    item.spnSummaryTotalCosts = spnSummaryTotalCosts;
                }
            }
        }
        #endregion

        #region V2-950 Add-Edit PrevMaint Schedule Record Dynamically
        public PartialViewResult AddPMSRecordsDynamic_Calendar(long PrevMaintMasterID, string ClientLookupId, string ScheduleType, long PrevMaintLibraryID)
        {
            PrevMaintWrapper prevMaintWrapper = new PrevMaintWrapper(userData);
            PrevMaintVM objPrevMaintVM = new PrevMaintVM();
            objPrevMaintVM.userData = this.userData;
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            var AllLookUps = commonWrapper.GetAllLookUpList();

            AddPMSRecordsModelDynamic_Calendar addPreventiveMaitenanceScheduleRecordsModelDynamic = new AddPMSRecordsModelDynamic_Calendar();
            addPreventiveMaitenanceScheduleRecordsModelDynamic.PrevMaintMasterId = PrevMaintMasterID;
            addPreventiveMaitenanceScheduleRecordsModelDynamic.ScheduleType = ScheduleType;
            addPreventiveMaitenanceScheduleRecordsModelDynamic.PrevmaintClientlookUp = ClientLookupId;
            addPreventiveMaitenanceScheduleRecordsModelDynamic.PrevMaintLibraryID = PrevMaintLibraryID;
            objPrevMaintVM.UIConfigurationDetails = new RetrieveDataForUIConfiguration()
                                            .Retrieve(DataDictionaryViewNameConstant.AddScheduleRecords_Calendar, userData);

            IList<string> LookupNames = objPrevMaintVM.UIConfigurationDetails.ToList()
                                          .Where(x => x.LookupType == "LookupList" && !string.IsNullOrEmpty(x.LookupName))
                                          .Select(s => s.LookupName)
                                          .ToList();
            objPrevMaintVM.AllRequiredLookUplist = AllLookUps.Where(x => LookupNames.Contains(x.ListName))
                                                      .GroupBy(x => new { x.ListName, x.ListValue }).Select(x => x.FirstOrDefault())
                                                      .Select(s => new PMSAddUILookupList
                                                      { text = s.ListValue + " - " + s.Description, value = s.ListValue, lookupname = s.ListName })
                                                      .ToList();

            var FrequencyTypeList = UtilityFunction.populateFrequencyTypeList();
            if (FrequencyTypeList != null)
            {
                objPrevMaintVM.FrequencyTypeList = FrequencyTypeList.Select(x => new SelectListItem { Text = x.text.ToString(), Value = x.value.ToString() });
            }
            var ScheduleMethodList = UtilityFunction.populateScheduleMethodList();
            if (ScheduleMethodList != null)
            {
                objPrevMaintVM.ScheduleMethodList = ScheduleMethodList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
            }

            var daysOfWeek = UtilityFunction.DaysOfWeekList();
            if (daysOfWeek != null)
            {
                objPrevMaintVM.DaysOfWeekList = daysOfWeek.Select(x => new SelectListItem { Text = x.text, Value = x.value });
            }
            objPrevMaintVM.PMSScheduleType = ScheduleType;
            objPrevMaintVM.AddPMSRecordsModelDynamic_Calendar = addPreventiveMaitenanceScheduleRecordsModelDynamic;
            LocalizeControls(objPrevMaintVM, LocalizeResourceSetConstants.PrevMaintDetails);
            return PartialView("_AddScheduleDynamicCalendar", objPrevMaintVM);
        }
        public PartialViewResult AddPMSRecordsDynamic_Meter(long PrevMaintMasterID, string ClientLookupId, string ScheduleType, long PrevMaintLibraryID)
        {
            PrevMaintWrapper prevMaintWrapper = new PrevMaintWrapper(userData);
            PrevMaintVM objPrevMaintVM = new PrevMaintVM();
            objPrevMaintVM.userData = this.userData;
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            var AllLookUps = commonWrapper.GetAllLookUpList();
            AddPMSRecordsModelDynamic_Meter addPreventiveMaitenanceScheduleRecordsModelDynamic = new AddPMSRecordsModelDynamic_Meter();
            addPreventiveMaitenanceScheduleRecordsModelDynamic.PrevMaintMasterId = PrevMaintMasterID;
            addPreventiveMaitenanceScheduleRecordsModelDynamic.ScheduleType = ScheduleType;
            addPreventiveMaitenanceScheduleRecordsModelDynamic.PrevmaintClientlookUp = ClientLookupId;
            addPreventiveMaitenanceScheduleRecordsModelDynamic.PrevMaintLibraryID = PrevMaintLibraryID;

            objPrevMaintVM.UIConfigurationDetails = new RetrieveDataForUIConfiguration()
                                            .Retrieve(DataDictionaryViewNameConstant.AddScheduleRecords_Meter, userData);

            IList<string> LookupNames = objPrevMaintVM.UIConfigurationDetails.ToList()
                                          .Where(x => x.LookupType == "LookupList" && !string.IsNullOrEmpty(x.LookupName))
                                          .Select(s => s.LookupName)
                                          .ToList();
            objPrevMaintVM.AllRequiredLookUplist = AllLookUps.Where(x => LookupNames.Contains(x.ListName))
                                                      .GroupBy(x => new { x.ListName, x.ListValue }).Select(x => x.FirstOrDefault())
                                                      .Select(s => new PMSAddUILookupList
                                                      { text = s.ListValue + " - " + s.Description, value = s.ListValue, lookupname = s.ListName })
                                                      .ToList();

            var FrequencyTypeList = UtilityFunction.populateFrequencyTypeList();
            if (FrequencyTypeList != null)
            {
                objPrevMaintVM.FrequencyTypeList = FrequencyTypeList.Select(x => new SelectListItem { Text = x.text.ToString(), Value = x.value.ToString() });
            }
            var ScheduleMethodList = UtilityFunction.populateScheduleMethodList();
            if (ScheduleMethodList != null)
            {
                objPrevMaintVM.ScheduleMethodList = ScheduleMethodList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
            }

            objPrevMaintVM.PMSScheduleType = ScheduleType;
            objPrevMaintVM.AddPMSRecordsModelDynamic_Meter = addPreventiveMaitenanceScheduleRecordsModelDynamic;
            LocalizeControls(objPrevMaintVM, LocalizeResourceSetConstants.PrevMaintDetails);
            return PartialView("_AddScheduleDynamicMeter", objPrevMaintVM);
        }
        public PartialViewResult AddPMSRecordsDynamic_OnDemand(long PrevMaintMasterID, string ClientLookupId, string ScheduleType, long PrevMaintLibraryID)
        {
            PrevMaintWrapper prevMaintWrapper = new PrevMaintWrapper(userData);
            PrevMaintVM objPrevMaintVM = new PrevMaintVM();
            objPrevMaintVM.userData = this.userData;
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            var AllLookUps = commonWrapper.GetAllLookUpList();

            AddPMSRecordsModelDynamic_OnDemand addPreventiveMaitenanceScheduleRecordsModelDynamic = new AddPMSRecordsModelDynamic_OnDemand();
            addPreventiveMaitenanceScheduleRecordsModelDynamic.PrevMaintMasterId = PrevMaintMasterID;
            addPreventiveMaitenanceScheduleRecordsModelDynamic.ScheduleType = ScheduleType;
            addPreventiveMaitenanceScheduleRecordsModelDynamic.PrevmaintClientlookUp = ClientLookupId;
            addPreventiveMaitenanceScheduleRecordsModelDynamic.PrevMaintLibraryID = PrevMaintLibraryID;

            objPrevMaintVM.UIConfigurationDetails = new RetrieveDataForUIConfiguration()
                                            .Retrieve(DataDictionaryViewNameConstant.AddScheduleRecords_OnDemand, userData);

            IList<string> LookupNames = objPrevMaintVM.UIConfigurationDetails.ToList()
                                          .Where(x => x.LookupType == "LookupList" && !string.IsNullOrEmpty(x.LookupName))
                                          .Select(s => s.LookupName)
                                          .ToList();
            objPrevMaintVM.AllRequiredLookUplist = AllLookUps.Where(x => LookupNames.Contains(x.ListName))
                                                      .GroupBy(x => new { x.ListName, x.ListValue }).Select(x => x.FirstOrDefault())
                                                      .Select(s => new PMSAddUILookupList
                                                      { text = s.ListValue + " - " + s.Description, value = s.ListValue, lookupname = s.ListName })
                                                      .ToList();

            var FrequencyTypeList = UtilityFunction.populateFrequencyTypeList();
            if (FrequencyTypeList != null)
            {
                objPrevMaintVM.FrequencyTypeList = FrequencyTypeList.Select(x => new SelectListItem { Text = x.text.ToString(), Value = x.value.ToString() });
            }
            var ScheduleMethodList = UtilityFunction.populateScheduleMethodList();
            if (ScheduleMethodList != null)
            {
                objPrevMaintVM.ScheduleMethodList = ScheduleMethodList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
            }

            objPrevMaintVM.PMSScheduleType = ScheduleType;
            objPrevMaintVM.AddPMSRecordsModelDynamic_OnDemand = addPreventiveMaitenanceScheduleRecordsModelDynamic;
            LocalizeControls(objPrevMaintVM, LocalizeResourceSetConstants.PrevMaintDetails);
            return PartialView("_AddScheduleDynamicOnDemand", objPrevMaintVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddPrevMaintScheduleRecordsDynamic(PrevMaintVM prevMaintVM)
        {
            string Mode = string.Empty;
            string ModelValidationFailedMessage = string.Empty;
            long prevmaintmasterid = 0;
            // V2-1161 Validate if planning is required and return validation result if not valid
            var validationResult = ValidatePlanningRequired(prevMaintVM);
            if (validationResult != null)
            {
                return validationResult;
            }
            if (ModelState.IsValid)
            {
                PrevMaintWrapper wrapper = new PrevMaintWrapper(userData);
                PrevMaintSched objPrvSch = new PrevMaintSched();
                Mode = "add";
                if (prevMaintVM.PMSScheduleType == ScheduleTypeConstants.Calendar)
                {
                    objPrvSch = wrapper.AddPrevMaintSchedDynamicCalendar(prevMaintVM);
                }
                else if (prevMaintVM.PMSScheduleType == ScheduleTypeConstants.Meter)
                {
                    objPrvSch = wrapper.AddPrevMaintSchedDynamicMeter(prevMaintVM);
                }
                else if (prevMaintVM.PMSScheduleType == ScheduleTypeConstants.OnDemand)
                {
                    objPrvSch = wrapper.AddPrevMaintSchedDynamicOnDemand(prevMaintVM);
                }

                if (objPrvSch.ErrorMessages != null && objPrvSch.ErrorMessages.Count > 0 && objPrvSch.ErrorMessages[0].ToString() != "A Schedule record exists for this master and charge to")
                {
                    return Json(objPrvSch.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    if (prevMaintVM.PMSScheduleType == ScheduleTypeConstants.Calendar)
                    {
                        prevmaintmasterid = prevMaintVM.AddPMSRecordsModelDynamic_Calendar.PrevMaintMasterId ?? 0;
                    }
                    else if (prevMaintVM.PMSScheduleType == ScheduleTypeConstants.Meter)
                    {
                        prevmaintmasterid = prevMaintVM.AddPMSRecordsModelDynamic_Meter.PrevMaintMasterId ?? 0;
                    }
                    else if (prevMaintVM.PMSScheduleType == ScheduleTypeConstants.OnDemand)
                    {
                        prevmaintmasterid = prevMaintVM.AddPMSRecordsModelDynamic_OnDemand.PrevMaintMasterId ?? 0;
                    }
                    return Json(new { Result = JsonReturnEnum.success.ToString(), prevemaintmasterid = prevmaintmasterid, mode = Mode }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }
        public PartialViewResult EditScheduleRecordsDynamicCalendar(long PrevMaintMasterId, long PrevMaintScheId, string ClientLookupId, string ScheduleType, long PrevMaintLibraryID)
        {
            PrevMaintVM objPrevMaintVM = new PrevMaintVM();
            PrevMaintWrapper _PrevMaintObj = new PrevMaintWrapper(userData);
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            EditPMSRecordsModelDynamic_Calendar objSchd = new EditPMSRecordsModelDynamic_Calendar();
            objPrevMaintVM.userData = this.userData;

            var AllLookUps = commonWrapper.GetAllLookUpList();
            objPrevMaintVM.UIConfigurationDetails = new RetrieveDataForUIConfiguration()
                                            .Retrieve(DataDictionaryViewNameConstant.ScheduleRecordsEdit_Calendar, userData);
            Task t1 = Task.Factory.StartNew(() => objSchd = _PrevMaintObj.getEditPMScheduleRecordsDynamicCalendar(PrevMaintMasterId, PrevMaintScheId));
            Task t2 = Task.Factory.StartNew(() => AllLookUps = commonWrapper.GetAllLookUpList());
            Task.WaitAll(t1, t2);
            IList<string> LookupNames = objPrevMaintVM.UIConfigurationDetails.ToList()
                                       .Where(x => x.LookupType == "LookupList" && !string.IsNullOrEmpty(x.LookupName))
                                       .Select(s => s.LookupName)
                                       .ToList();

            objPrevMaintVM.AllRequiredLookUplist = AllLookUps.Where(x => LookupNames.Contains(x.ListName))
                                                      .GroupBy(x => new { x.ListName, x.ListValue }).Select(x => x.FirstOrDefault())
                                                      .Select(s => new PMSAddUILookupList
                                                      { text = s.ListValue + " - " + s.Description, value = s.ListValue, lookupname = s.ListName })
                                                      .ToList();

            objPrevMaintVM.EditPMSRecordsModelDynamic_Calendar = objSchd;
            objPrevMaintVM.EditPMSRecordsModelDynamic_Calendar.PrevmaintClientlookUp = ClientLookupId;
            objPrevMaintVM.EditPMSRecordsModelDynamic_Calendar.ScheduleType = ScheduleType;
            objPrevMaintVM.EditPMSRecordsModelDynamic_Calendar.PrevMaintLibraryID = PrevMaintLibraryID;
            var FrequencyTypeList = UtilityFunction.populateFrequencyTypeList();
            if (FrequencyTypeList != null)
            {
                objPrevMaintVM.FrequencyTypeList = FrequencyTypeList.Select(x => new SelectListItem { Text = x.text.ToString(), Value = x.value.ToString() });
            }
            var ScheduleMethodList = UtilityFunction.populateScheduleMethodList();
            if (ScheduleMethodList != null)
            {
                objPrevMaintVM.ScheduleMethodList = ScheduleMethodList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
            }

            var daysOfWeek = UtilityFunction.DaysOfWeekList();
            if (daysOfWeek != null)
            {
                objPrevMaintVM.DaysOfWeekList = daysOfWeek.Select(x => new SelectListItem { Text = x.text, Value = x.value });
            }
            objPrevMaintVM.EditPMSRecordsModelDynamic_Calendar.ExclusionDaysStringHdn = string.Join(",", objSchd.ExclusionDaysString);

            #region Inactive Days
            if (!string.IsNullOrEmpty(objSchd.ExcludeDOW) && objSchd.ExcludeDOW[0] == '1')
            {
                objSchd.Sunday = true;
            }
            if (!string.IsNullOrEmpty(objSchd.ExcludeDOW) && objSchd.ExcludeDOW[1] == '1')
            {
                objSchd.Monday = true;
            }
            if (!string.IsNullOrEmpty(objSchd.ExcludeDOW) && objSchd.ExcludeDOW[2] == '1')
            {
                objSchd.Tuesday = true;
            }
            if (!string.IsNullOrEmpty(objSchd.ExcludeDOW) && objSchd.ExcludeDOW[3] == '1')
            {
                objSchd.Wednesday = true;
            }
            if (!string.IsNullOrEmpty(objSchd.ExcludeDOW) && objSchd.ExcludeDOW[4] == '1')
            {
                objSchd.Thursday = true;
            }
            if (!string.IsNullOrEmpty(objSchd.ExcludeDOW) && objSchd.ExcludeDOW[5] == '1')
            {
                objSchd.Friday = true;
            }
            if (!string.IsNullOrEmpty(objSchd.ExcludeDOW) && objSchd.ExcludeDOW[6] == '1')
            {
                objSchd.Saturday = true;
            }
            #endregion
            objPrevMaintVM.PMSScheduleType = ScheduleType;
            LocalizeControls(objPrevMaintVM, LocalizeResourceSetConstants.PrevMaintDetails);
            return PartialView("_EditScheduleDynamicCalendar", objPrevMaintVM);
        }
        public PartialViewResult EditScheduleRecordsDynamicMeter(long PrevMaintMasterId, long PrevMaintScheId, string ClientLookupId, string ScheduleType, long PrevMaintLibraryID)
        {
            PrevMaintVM objPrevMaintVM = new PrevMaintVM();
            PrevMaintWrapper _PrevMaintObj = new PrevMaintWrapper(userData);
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            EditPMSRecordsModelDynamic_Meter objSchd = new EditPMSRecordsModelDynamic_Meter();
            objPrevMaintVM.userData = this.userData;

            var AllLookUps = commonWrapper.GetAllLookUpList();
            objPrevMaintVM.UIConfigurationDetails = new RetrieveDataForUIConfiguration()
                                            .Retrieve(DataDictionaryViewNameConstant.ScheduleRecordsEdit_Meter, userData);
            Task t1 = Task.Factory.StartNew(() => objSchd = _PrevMaintObj.getEditPMScheduleRecordsDynamicMeter(PrevMaintMasterId, PrevMaintScheId));
            Task t2 = Task.Factory.StartNew(() => AllLookUps = commonWrapper.GetAllLookUpList());
            Task.WaitAll(t1, t2);
            IList<string> LookupNames = objPrevMaintVM.UIConfigurationDetails.ToList()
                                       .Where(x => x.LookupType == "LookupList" && !string.IsNullOrEmpty(x.LookupName))
                                       .Select(s => s.LookupName)
                                       .ToList();

            objPrevMaintVM.AllRequiredLookUplist = AllLookUps.Where(x => LookupNames.Contains(x.ListName))
                                                      .GroupBy(x => new { x.ListName, x.ListValue }).Select(x => x.FirstOrDefault())
                                                      .Select(s => new PMSAddUILookupList
                                                      { text = s.ListValue + " - " + s.Description, value = s.ListValue, lookupname = s.ListName })
                                                      .ToList();

            objPrevMaintVM.EditPMSRecordsModelDynamic_Meter = objSchd;
            objPrevMaintVM.EditPMSRecordsModelDynamic_Meter.PrevmaintClientlookUp = ClientLookupId;
            objPrevMaintVM.EditPMSRecordsModelDynamic_Meter.ScheduleType = ScheduleType;
            objPrevMaintVM.EditPMSRecordsModelDynamic_Meter.PrevMaintLibraryID = PrevMaintLibraryID;
            objPrevMaintVM.PMSScheduleType = ScheduleType;
            LocalizeControls(objPrevMaintVM, LocalizeResourceSetConstants.PrevMaintDetails);
            return PartialView("_EditScheduleDynamicMeter", objPrevMaintVM);
        }
        public PartialViewResult EditScheduleRecordsDynamicOnDemand(long PrevMaintMasterId, long PrevMaintScheId, string ClientLookupId, string ScheduleType, long PrevMaintLibraryID)
        {
            PrevMaintVM objPrevMaintVM = new PrevMaintVM();
            PrevMaintWrapper _PrevMaintObj = new PrevMaintWrapper(userData);
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            EditPMSRecordsModelDynamic_OnDemand objSchd = new EditPMSRecordsModelDynamic_OnDemand();
            objPrevMaintVM.userData = this.userData;

            var AllLookUps = commonWrapper.GetAllLookUpList();
            objPrevMaintVM.UIConfigurationDetails = new RetrieveDataForUIConfiguration()
                                            .Retrieve(DataDictionaryViewNameConstant.ScheduleRecordsEdit_OnDemand, userData);
            Task t1 = Task.Factory.StartNew(() => objSchd = _PrevMaintObj.getEditPMScheduleRecordsDynamicOnDemand(PrevMaintMasterId, PrevMaintScheId));
            Task t2 = Task.Factory.StartNew(() => AllLookUps = commonWrapper.GetAllLookUpList());
            Task.WaitAll(t1, t2);
            IList<string> LookupNames = objPrevMaintVM.UIConfigurationDetails.ToList()
                                       .Where(x => x.LookupType == "LookupList" && !string.IsNullOrEmpty(x.LookupName))
                                       .Select(s => s.LookupName)
                                       .ToList();

            objPrevMaintVM.AllRequiredLookUplist = AllLookUps.Where(x => LookupNames.Contains(x.ListName))
                                                      .GroupBy(x => new { x.ListName, x.ListValue }).Select(x => x.FirstOrDefault())
                                                      .Select(s => new PMSAddUILookupList
                                                      { text = s.ListValue + " - " + s.Description, value = s.ListValue, lookupname = s.ListName })
                                                      .ToList();

            objPrevMaintVM.EditPMSRecordsModelDynamic_OnDemand = objSchd;
            objPrevMaintVM.EditPMSRecordsModelDynamic_OnDemand.PrevmaintClientlookUp = ClientLookupId;
            objPrevMaintVM.EditPMSRecordsModelDynamic_OnDemand.ScheduleType = ScheduleType;
            objPrevMaintVM.EditPMSRecordsModelDynamic_OnDemand.PrevMaintLibraryID = PrevMaintLibraryID;

            var ScheduleMethodList = UtilityFunction.populateScheduleMethodList();
            if (ScheduleMethodList != null)
            {
                objPrevMaintVM.ScheduleMethodList = ScheduleMethodList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
            }

            objPrevMaintVM.PMSScheduleType = ScheduleType;
            LocalizeControls(objPrevMaintVM, LocalizeResourceSetConstants.PrevMaintDetails);
            return PartialView("_EditScheduleDynamicOnDemand", objPrevMaintVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult EditPMScheduleRecordsDynamic(PrevMaintVM prevMaintVM, string Command)
        {
            string ModelValidationFailedMessage = string.Empty;
            string Mode = string.Empty;
            long prevmaintmasterid = 0;
            // V2-1161 Validate if planning is required and return validation result if not valid
            var validationResult = ValidatePlanningRequired(prevMaintVM);
            if (validationResult != null)
            {
                return validationResult;
            }
            if (ModelState.IsValid)
            {
                PrevMaintWrapper pmWrapper = new PrevMaintWrapper(userData);
                PrevMaintSched objPrvSch = new PrevMaintSched();
                if (Command == "save")
                {
                    if (prevMaintVM.PMSScheduleType == ScheduleTypeConstants.Calendar)
                    {
                        objPrvSch = pmWrapper.editPMSRecordsDynamicCalendar(prevMaintVM);
                    }
                    else if (prevMaintVM.PMSScheduleType == ScheduleTypeConstants.Meter)
                    {
                        objPrvSch = pmWrapper.editPMSRecordsDynamicMeter(prevMaintVM);
                    }
                    else if (prevMaintVM.PMSScheduleType == ScheduleTypeConstants.OnDemand)
                    {
                        objPrvSch = pmWrapper.editPMSRecordsDynamicOnDemand(prevMaintVM);
                    }
                }

                if (objPrvSch.ErrorMessages != null && objPrvSch.ErrorMessages.Count > 0 && objPrvSch.ErrorMessages[0].ToString() != ErrorMessageConstants.Schedule_Record_Exists)
                {
                    return Json(objPrvSch.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    if (prevMaintVM.PMSScheduleType == ScheduleTypeConstants.Calendar)
                    {
                        prevmaintmasterid = prevMaintVM.EditPMSRecordsModelDynamic_Calendar.PrevMaintMasterId ?? 0;
                    }
                    else if (prevMaintVM.PMSScheduleType == ScheduleTypeConstants.Meter)
                    {
                        prevmaintmasterid = prevMaintVM.EditPMSRecordsModelDynamic_Meter.PrevMaintMasterId ?? 0;
                    }
                    else if (prevMaintVM.PMSScheduleType == ScheduleTypeConstants.OnDemand)
                    {
                        prevmaintmasterid = prevMaintVM.EditPMSRecordsModelDynamic_OnDemand.PrevMaintMasterId ?? 0;
                    }
                    return Json(new { Result = JsonReturnEnum.success.ToString(), prevemaintmasterid = prevmaintmasterid, mode = Mode }, JsonRequestBehavior.AllowGet);
                }
            }
            ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
            return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
        }
        #region Exclusion Days
        public PartialViewResult ExclusionDaysForDynamic()
        {
            PrevMaintVM objPrevMaintVM = new PrevMaintVM();
            objPrevMaintVM.userData = this.userData;
            var daysOfWeek = UtilityFunction.DaysOfWeekList();
            if (daysOfWeek != null)
            {
                objPrevMaintVM.DaysOfWeekList = daysOfWeek.Select(x => new SelectListItem { Text = x.text, Value = x.value });
            }
            LocalizeControls(objPrevMaintVM, LocalizeResourceSetConstants.PrevMaintDetails);
            return PartialView("_ExclusionDaysDynamicModal", objPrevMaintVM);
        }
        public JsonResult SaveExcludeDOW(long PrevMaintSchedId, string[] ExclusionDaysString)
        {
            PrevMaintWrapper PMWrapper = new PrevMaintWrapper(userData);
            PrevMaintSched objPrvSch = new PrevMaintSched();
            objPrvSch = PMWrapper.SavePrevMaintSchedExcludeDOW(PrevMaintSchedId, ExclusionDaysString);
            if (objPrvSch.ErrorMessages != null && objPrvSch.ErrorMessages.Count > 0)
            {
                return Json(objPrvSch.ErrorMessages, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Result = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        #endregion

        #region V2-712
        [HttpPost]
        public string PopulatePMScheduleAssignRecords(int? draw, int? start, int? length, long PMScheduleId)
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            PrevMaintWrapper pWrapper = new PrevMaintWrapper(userData);
            start = start.HasValue
                ? start / length
                : 0;
            int skip = start * length ?? 0;
            List<PMSchedAssignModel> PMScheduleAssignList = pWrapper.GetPMScheduleAssignGrid_V2(order, orderDir, length ?? 0, skip, PMScheduleId);
            var totalRecords = 0;
            var recordsFiltered = 0;
            recordsFiltered = PMScheduleAssignList.Count();
            totalRecords = PMScheduleAssignList.Count();
            int initialPage = start.Value;
            var filteredResult = PMScheduleAssignList.ToList();
            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult });
        }

        #region Add Assignment

        public ActionResult AddPMScheduleAssignment(long PmSchedId)
        {
            PrevMaintVM objPrevMaintVM = new PrevMaintVM();
            objPrevMaintVM.pMSchedAssignModel = new PMSchedAssignModel();
            objPrevMaintVM.pMSchedAssignModel.PrevMaintSchedId = PmSchedId;
            objPrevMaintVM.security = this.userData.Security;
            LocalizeControls(objPrevMaintVM, LocalizeResourceSetConstants.PrevMaintDetails);
            return PartialView("_AddPmScheduleAssignment", objPrevMaintVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddPMScheduleAssignment(PrevMaintVM _pmSchedAssign)
        {
            if (ModelState.IsValid)
            {
                var pmSchedId = _pmSchedAssign.pMSchedAssignModel.PrevMaintSchedId;
                PrevMaintWrapper _wrapperObj = new PrevMaintWrapper(userData);
                PMSchedAssign objPMSchedAssign = _wrapperObj.CreatePMSchedAssignment(_pmSchedAssign.pMSchedAssignModel);
                if (objPMSchedAssign.ErrorMessages != null && objPMSchedAssign.ErrorMessages.Count > 0)
                {
                    return Json(objPMSchedAssign.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), pmSchedAssignId = objPMSchedAssign.PMSchedAssignId, pmSchedId = pmSchedId }, JsonRequestBehavior.AllowGet);
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

        [HttpPost]
        public string GetPMAssignPersonnelLookupListGridData(int? draw, int? start, int? length, string ClientLookupId = "", string NameFirst = "", string NameLast = "")
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            PrevMaintWrapper _wrapperObj = new PrevMaintWrapper(userData);
            start = start.HasValue
          ? start / length
          : 0;
            int skip = start * length ?? 0;
            List<PersonnelLookUpModel> pList = _wrapperObj.GetPMAssignPersonnelLookupList(order, orderDir, skip, length ?? 0, ClientLookupId, NameFirst, NameLast);
            var totalRecords = 0;
            var recordsFiltered = 0;
            if (pList != null && pList.Count > 0)
            {
                recordsFiltered = pList[0].TotalCount;
                totalRecords = pList[0].TotalCount;
            }
            else
            {
                recordsFiltered = 0;
                totalRecords = 0;
            }
            int initialPage = start.Value;
            var filteredResult = pList
              .ToList();

            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult }, JsonSerializer12HoursDateAndTimeSettings);

        }

        public JsonResult UpdatePmScheduleAssignHours(long PMSchedAssignId, decimal hours)
        {
            PrevMaintWrapper pWrapper = new PrevMaintWrapper(userData);
            PMSchedAssign pmSchedAssign = new PMSchedAssign();
            pmSchedAssign = pWrapper.UpdatePmScheduleAssign(PMSchedAssignId, hours);

            if (pmSchedAssign.ErrorMessages != null && pmSchedAssign.ErrorMessages.Count > 0)
            {
                return Json(pmSchedAssign.ErrorMessages, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Result = JsonReturnEnum.success.ToString(), PMSchedAssignId = pmSchedAssign.PMSchedAssignId }, JsonRequestBehavior.AllowGet);
            }

        }
        [HttpPost]
        public JsonResult DeletePmSchedAssign(long pmSchedAssignId)
        {
            PrevMaintWrapper wrapper = new PrevMaintWrapper(userData);
            var deleteResult = wrapper.DeletePmSchedassign(pmSchedAssignId);
            if (deleteResult)
            {
                return Json(new { Result = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Result = JsonReturnEnum.failed.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult GetPrevMaintInnerGrid(long PrevMaintSchedId)
        {
            PrevMaintVM objPrevMaintVM = new PrevMaintVM();
            PrevMaintWrapper pWrapper = new PrevMaintWrapper(userData);
            objPrevMaintVM.innerGridPMSchedAssignModel = pWrapper.GetPrevMaintInnerGridData(PrevMaintSchedId);
            LocalizeControls(objPrevMaintVM, LocalizeResourceSetConstants.PrevMaintDetails);
            return View("_InnerAssignGridPrevMaint", objPrevMaintVM);
        }
        #endregion

        #region V2-977
        public JsonResult PMSScheduleReassignForMultipleAssignment(PrevMaintVM ovm)
        {
            string ModelValidationFailedMessage = string.Empty;
            string result = string.Empty;
            string IsAddOrUpdate = string.Empty;
            PMSchedAssign obj = new PMSchedAssign();
            long PersonnelId = ovm.prevMaintReassignModel.PersonnelId;
            if (ModelState.IsValid)
            {
                PrevMaintWrapper pWrapper = new PrevMaintWrapper(userData);
                if (ovm.prevMaintReassignModel != null && ovm.prevMaintReassignModel.PMSchedAssignIdsList != null && ovm.prevMaintReassignModel.PMSchedAssignIdsList.Length > 0)
                {
                    obj = pWrapper.UpdatePMSchedulingReassignForMultipleAssignment(PersonnelId, ovm.prevMaintReassignModel.PMSchedAssignIdsList.Split(','), ovm.prevMaintReassignModel.PrevMainIdsList.Split(','));
                    if (obj.ErrorMessages != null && obj.ErrorMessages.Count > 0)
                    {
                        return Json(obj.ErrorMessages, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { Issuccess = true }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    result = ErrorMessageConstants.Invalid_Preventive_Maintenance_Id;
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion  

        #region V2-1161
        // This method validates if planning is required for the given Preventive Maintenance ViewModel (PrevMaintVM).
        // It checks if the site uses planning and if any of the planning required models have a null or zero PlannerPersonnelId.
        // If any such model is found, it returns a JSON result with a validation message.
        private JsonResult ValidatePlanningRequired(PrevMaintVM prevMaintVM)
        {
            // Check if the site uses planning
            if (userData.Site.UsePlanning)
            {
                // List of tuples containing PlanningRequired and PlannerPersonnelId for different schedule types
                var planningRequiredModels = new List<(bool PlanningRequired, long? PlannerPersonnelId)>
                {
                    (prevMaintVM.AddPMSRecordsModelDynamic_OnDemand.PlanningRequired, prevMaintVM.AddPMSRecordsModelDynamic_OnDemand.Planner_PersonnelId),
                    (prevMaintVM.AddPMSRecordsModelDynamic_Meter.PlanningRequired, prevMaintVM.AddPMSRecordsModelDynamic_Meter.Planner_PersonnelId),
                    (prevMaintVM.AddPMSRecordsModelDynamic_Calendar.PlanningRequired, prevMaintVM.AddPMSRecordsModelDynamic_Calendar.Planner_PersonnelId),
                    (prevMaintVM.EditPMSRecordsModelDynamic_OnDemand.PlanningRequired, prevMaintVM.EditPMSRecordsModelDynamic_OnDemand.Planner_PersonnelId),
                    (prevMaintVM.EditPMSRecordsModelDynamic_Meter.PlanningRequired, prevMaintVM.EditPMSRecordsModelDynamic_Meter.Planner_PersonnelId),
                    (prevMaintVM.EditPMSRecordsModelDynamic_Calendar.PlanningRequired, prevMaintVM.EditPMSRecordsModelDynamic_Calendar.Planner_PersonnelId)
                };

                // Check if any model has PlanningRequired set to true and PlannerPersonnelId is null or zero
                if (planningRequiredModels.Any(model => model.PlanningRequired && (model.PlannerPersonnelId == 0 || model.PlannerPersonnelId == null)))
                {
                    // Get the validation message from resources and return as JSON result
                    var ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalPlanningRequired", LocalizeResourceSetConstants.Global);
                    return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
                }
            }
            // Return null if no validation errors are found
            return null;
        }
        #endregion

        #region V2-1204 PrevMaint Model

        [HttpPost]
        public PartialViewResult PrevMaintModelWizard(long PrevMaintMasterId)
        {
            PrevMaintVM objPrevMaintVM = new PrevMaintVM();
            PrevMaintWrapper pmWrapper = new PrevMaintWrapper(userData);
            var prvMaint = pmWrapper.populateMaintenanceDetails(PrevMaintMasterId);
            var AllLookUps = pmWrapper.GetAllLookUpList();
            if (AllLookUps != null)
            {
                var typeList = AllLookUps.Where(x => x.ListName == LookupListConstants.Preventive_Maint_Type).ToList();
                if (typeList != null)
                {
                    prvMaint.TypeList = typeList.Select(x => new SelectListItem { Text = x.ListValue + "  -  " + x.Description, Value = x.ListValue });
                }
            }
            var ScheduleTypeList = UtilityFunction.GetScheduleType();
            prvMaint.ScheduleTypeList = ScheduleTypeList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
            objPrevMaintVM.preventiveMaintenanceModel = prvMaint;
            LocalizeControls(objPrevMaintVM, LocalizeResourceSetConstants.PrevMaintDetails);
            return PartialView("~/Views/PreventiveMaintenance/_AddPrevMaintModelWizard.cshtml", objPrevMaintVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddPrevMaintModel(PrevMaintVM prevMaintVM)
        {
            string ModelValidationFailedMessage = string.Empty;
            if (ModelState.IsValid)
            {
                PrevMaintWrapper pmWrapper = new PrevMaintWrapper(userData);
                PrevMaintMaster objPrevMaintMaster = new PrevMaintMaster();
                
                objPrevMaintMaster = pmWrapper.AddPrevMaintModel(prevMaintVM.preventiveMaintenanceModel);
                
                if (objPrevMaintMaster.ErrorMessages != null && objPrevMaintMaster.ErrorMessages.Count > 0)
                {
                    return Json(objPrevMaintMaster.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), PrevMaintMasterId = objPrevMaintMaster.PrevMaintMasterId }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
    }
}



