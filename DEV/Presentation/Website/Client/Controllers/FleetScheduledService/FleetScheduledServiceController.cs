using Client.ActionFilters;
using Client.BusinessWrapper.Common;
using Client.BusinessWrapper.ScheduledService;
using Client.Common;
using Client.Controllers.Common;
using Client.Models.FleetScheduledService;
using Common.Constants;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Web.Mvc;
using Rotativa;
using Client.BusinessWrapper.Configuration.FleetServiceTask;
using System.Text.RegularExpressions;

namespace Client.Controllers.FleetScheduledService
{
    public class FleetScheduledServiceController : SomaxBaseController
    {
        #region  Scheduled Service Search
        [CheckUserSecurity(securityType = SecurityConstants.Fleet_Scheduled_Service)]
        public ActionResult Index()
        {        
            ScheduledServiceWrapper scServWrapper = new ScheduledServiceWrapper(userData);
            FleetScheduledServiceVM scServVM = new FleetScheduledServiceVM();
            scServVM.ScheduledServiceModel = new FleetScheduledServiceModel();
            scServVM.security = this.userData.Security;
            var StatusList = UtilityFunction.InactiveActiveStatusTypes();
            if (StatusList != null)
            {
                scServVM.InactiveFlagList = StatusList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
            }
            LocalizeControls(scServVM, LocalizeResourceSetConstants.EquipmentDetails);
            return View(scServVM);
        }
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public string GetScheduledServicetGridData(int? draw, int? start, int? length, int customQueryDisplayId = 1, string ClientLookupId = "", string Name = "", string ServiceTaskDesc = "", string SearchText = "",
        string order = "0")
        {
            List<FleetScheduledServiceSearchModel> scServSearchModelList = new List<FleetScheduledServiceSearchModel>();           
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            var colname = Request.Form.GetValues("columns[" + order + "][name]");
            SearchText = SearchText.Replace("%", "[%]");
            ClientLookupId = ClientLookupId.Replace("%", "[%]");
            Name = Name.Replace("%", "[%]");
            ServiceTaskDesc = ServiceTaskDesc.Replace("%", "[%]");
            start = start.HasValue
               ? start / length
               : 0;
            int skip = start * length ?? 0;
            List<string> typeList = new List<string>();
            ScheduledServiceWrapper scServWrapper = new ScheduledServiceWrapper(userData);
            List<FleetScheduledServiceSearchModel> fleetScheduledServiceList = scServWrapper.GetFleetScheduledServiceGridData(customQueryDisplayId,order, orderDir, skip, length ?? 0, ClientLookupId, Name, ServiceTaskDesc, SearchText);
            var totalRecords = 0;
            var recordsFiltered = 0;
            if (fleetScheduledServiceList != null && fleetScheduledServiceList.Count > 0)
            {
                recordsFiltered = fleetScheduledServiceList[0].TotalCount;
                totalRecords = fleetScheduledServiceList[0].TotalCount;
            }
            int initialPage = start.Value;
            var filteredResult = fleetScheduledServiceList
              .ToList();
            bool IsScheduledServiceAccessSecurity = userData.Security.Fleet_Scheduled.Access;
            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult, IsScheduledServiceAccessSecurity = IsScheduledServiceAccessSecurity }, JsonSerializerDateSettings);
        }

        #endregion

        #region Fuel Add or Edit
        public PartialViewResult ScheduledServiceAddOrEdit(long EquipmentId, long SchedServiceId)
        {
            ScheduledServiceWrapper ffWrapper = new ScheduledServiceWrapper(userData);
            FleetScheduledServiceModel objFleetScheduledServiceModel = new FleetScheduledServiceModel();
            FleetScheduledServiceVM sServVM = new FleetScheduledServiceVM();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            List<DataContracts.LookupList> AllLookUps = new List<DataContracts.LookupList>();
            List<DataContracts.HierarchicalList> VMRSSystemList = new List<DataContracts.HierarchicalList>();

            AllLookUps = commonWrapper.GetAllLookUpList();
            if (AllLookUps != null)
            {             
                FleetServiceTaskWrapper fltServiceTaskWrapper = new FleetServiceTaskWrapper(userData);
                List<SelectListItem> stList = fltServiceTaskWrapper.GetServiceTask();        
                var TimeTypeList = commonWrapper.GetListFromConstVals(LookupListConstants.TIME_TYPES);
                if (TimeTypeList != null)
                {
                    objFleetScheduledServiceModel.LookUpTimeTypeList = TimeTypeList.Select(x => new SelectListItem { Text = x.text.ToString(), Value = x.text.ToString() });
                    objFleetScheduledServiceModel.LookUpServiceTypeList = stList;
                    objFleetScheduledServiceModel.TimeIntervalType = TimeTypeConstants.Days;
                    objFleetScheduledServiceModel.TimeThresoldType = TimeTypeConstants.Days;
                }

                var RepairReasonList = AllLookUps.Where(x => x.ListName == LookupListConstants.SO_RepairReason).ToList();
                if (RepairReasonList != null)
                {
                    sServVM.LookUpRepairReasonList = RepairReasonList.Select(x => new SelectListItem { Text = x.ListValue + "  -  " + x.Description, Value = x.ListValue });
                }
            }

            VMRSSystemList = commonWrapper.GetHierarchicalListByName(LookupListConstants.VMRS_Code);
            if (VMRSSystemList != null)
            {
                sServVM.VMRSSystemList = VMRSSystemList;
            }

            objFleetScheduledServiceModel.Pagetype = "Add";
            if (EquipmentId != 0 && SchedServiceId != 0)
            {
            
                objFleetScheduledServiceModel = ffWrapper.GetEditScheduledServiceDetailsById(EquipmentId, SchedServiceId);
                objFleetScheduledServiceModel.Pagetype = "Edit";
                FleetServiceTaskWrapper fltServiceTaskWrapper = new FleetServiceTaskWrapper(userData);
                List<SelectListItem> stList = fltServiceTaskWrapper.GetServiceTask();
                var TimeTypeList = commonWrapper.GetListFromConstVals(LookupListConstants.TIME_TYPES);
                if (TimeTypeList != null)
                {
                    objFleetScheduledServiceModel.LookUpTimeTypeList = TimeTypeList.Select(x => new SelectListItem { Text = x.text.ToString(), Value = x.text.ToString() });
                    objFleetScheduledServiceModel.LookUpServiceTypeList = stList;

                }           
            }
            else
            {
                objFleetScheduledServiceModel.TimeIntervalType = TimeTypeConstants.Days;
                objFleetScheduledServiceModel.TimeThresoldType = TimeTypeConstants.Days;
            }
            sServVM.ScheduledServiceModel = objFleetScheduledServiceModel;
            LocalizeControls(sServVM, LocalizeResourceSetConstants.EquipmentDetails);
            return PartialView("~/Views/FleetScheduledService/_ScheduledServiceAddOrEdit.cshtml", sServVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ScheduledServicelAddOrEdit(FleetScheduledServiceVM objSS)
        {
            List<string> ErrorList = new List<string>();
            ScheduledServiceWrapper SSWrapper = new ScheduledServiceWrapper(userData);
            string ModelValidationFailedMessage = string.Empty;
            string Mode = string.Empty;
            if (ModelState.IsValid)
            {
                if (objSS.ScheduledServiceModel.ScheduledServiceId > 0)
                {
                    Mode = "Edit";
                }
                else
                {
                    Mode = "Add";
                }
               
                DataContracts.ScheduledService scheduledService = new DataContracts.ScheduledService();
                string SS_ClientLookupId = objSS.ScheduledServiceModel.ClientLookupId.ToUpper().Trim();
                if(objSS.ScheduledServiceModel.TimeInterval == 0)
                {
                    objSS.ScheduledServiceModel.TimeIntervalType = string.Empty;
                }

                if (objSS.ScheduledServiceModel.TimeThreshold == 0)
                {
                    objSS.ScheduledServiceModel.TimeThresoldType = string.Empty;
                }
                scheduledService = SSWrapper.AddOrEditScheduledService(SS_ClientLookupId, objSS);
                if (scheduledService.ErrorMessages != null && scheduledService.ErrorMessages.Count > 0)
                {
                    return Json(scheduledService.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), EquipmentId = scheduledService.EquipmentId, Mode = Mode }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult GetHierarchicalList(string ListName, string List1Value)
        {
            CommonWrapper comWrapper = new CommonWrapper(userData);
            List<DataContracts.HierarchicalList> HierarchicalList = new List<DataContracts.HierarchicalList>();
            List<SelectListItem> AssemblyList = new List<SelectListItem>();

            HierarchicalList = comWrapper.GetHierarchicalListByName(ListName);
            if (HierarchicalList != null && HierarchicalList.Count > 0)
            {
                AssemblyList = HierarchicalList
                            .Select(x => new { x.Level1Value, x.Level2Value, x.Level2Description })
                            .Distinct()
                            .Where(x => x.Level1Value == List1Value)
                            .Select(x => new SelectListItem { Text = x.Level2Value + " - " + x.Level2Description, Value = x.Level2Value })
                            .OrderBy(x => x.Value).ToList();
            }
            return Json(new { Result = JsonReturnEnum.success.ToString(), data = AssemblyList });
        }
        #endregion

        #region Print Data
        [HttpPost]
        [EncryptedActionParameter]
        public JsonResult SetPrintData(FleetScheduledPrintParams fleetScheduledPrintParams)
        {
            Session["FLEETSCHDULEDPRINTPARAMS"] = fleetScheduledPrintParams;
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        [NoDirectAccess]
        public ActionResult ExportASPDF()
        {
            FleetScheduledServiceVM fltschVM = new FleetScheduledServiceVM();
            ScheduledServiceWrapper scServWrapper = new ScheduledServiceWrapper(userData);
            FleetScheduledPDFPrintModel fleetScheduledPDFPrintModel = new FleetScheduledPDFPrintModel();
            List<FleetScheduledPDFPrintModel> fleetScheduledPDFPrintModelList = new List<FleetScheduledPDFPrintModel>();
            FleetScheduledPrintParams fleetScheduledPrintParams = (FleetScheduledPrintParams)Session["FLEETSCHDULEDPRINTPARAMS"];
            FleetScheduledServiceVM fleetScheduledSearchModel = new FleetScheduledServiceVM();
            int customQueryDisplayId = fleetScheduledPrintParams.customQueryDisplayId;
            string order = fleetScheduledPrintParams.colname;
            string orderDir = fleetScheduledPrintParams.coldir;
            string _startReadingDate = string.Empty;
            string _endReadingDate = string.Empty;
            string SearchText = fleetScheduledPrintParams.SearchText;
            string ClientLookupId = fleetScheduledPrintParams.ClientLookupId;
            string Name = fleetScheduledPrintParams.Name;
            string ServiceTask = fleetScheduledPrintParams.ServiceTask;
            string Schedule = fleetScheduledPrintParams.Schedule;
            string NextDue = fleetScheduledPrintParams.NextDue;
            string LastCompleted = fleetScheduledPrintParams.LastCompleted;
            string ServiceTaskDesc = fleetScheduledPrintParams.ServiceTaskDesc;
            List<FleetScheduledServiceSearchModel> fleetScheduledServiceList = scServWrapper.GetFleetScheduledServiceGridData(customQueryDisplayId, order, orderDir, 0, 100000, ClientLookupId, Name, ServiceTaskDesc, SearchText);

            foreach (var item in fleetScheduledServiceList)
            {
                fleetScheduledPDFPrintModel = new FleetScheduledPDFPrintModel();
                fleetScheduledPDFPrintModel.ClientLookupId = item.ClientLookupId;
                fleetScheduledPDFPrintModel.Name = item.Name;
                fleetScheduledPDFPrintModel.ServiceTask = item.ServiceTask;
                fleetScheduledPDFPrintModel.ImageUrl = item.ImageUrl;
                fleetScheduledPDFPrintModel.ServiceTaskDesc = item.ServiceTaskDesc;
                
                string scheduleInterval = "";
                if (item.TimeInterval > 0 || item.Meter1Interval > 0 || item.Meter2Interval > 0)
                {
                    scheduleInterval = "Every ";
                    if (item.TimeInterval > 0)
                    {
                        scheduleInterval += item.TimeInterval + " " + item.TimeIntervalType + " or ";
                        if (item.NextDueDate != null && item.NextDueDate == default(DateTime))
                        {
                            fleetScheduledPDFPrintModel.NextDueScheduledate = "";
                        }
                        else
                        {
                            fleetScheduledPDFPrintModel.NextDueScheduledate = item.NextDueDate.Value.ToString("MM/dd/yyyy") + " from now ";
                        }
                    }
                    string meterscheduleinterval = "";
                    if (item.Meter1Interval > 0)
                    {
                        scheduleInterval += item.Meter1Interval + " " + item.Meter1Units + " or ";
                        if (item.Meter1Units != "")
                        {
                            meterscheduleinterval += item.NextDueMeter1 + " " + item.Meter1Units.Substring(0, 2) + " from now or ";
                        }
                        else
                        {
                            meterscheduleinterval += item.NextDueMeter1 + " " + item.Meter1Units + " from now or ";
                        }

                    }
                    if (item.Meter2Interval > 0)
                    {
                        scheduleInterval += item.Meter2Interval + " " + item.Meter2Units + " or ";
                        if (item.Meter2Units != "")
                        {
                            meterscheduleinterval += item.NextDueMeter2 + " " + item.Meter2Units.Substring(0, 2) + " from now";
                        }
                        else
                        {
                            meterscheduleinterval += item.NextDueMeter2 + " " + item.Meter2Units + " from now";
                        }

                    }
                    else
                    {
                        if (meterscheduleinterval != "")
                        {
                            meterscheduleinterval = meterscheduleinterval.Trim();
                            meterscheduleinterval = regex.Replace(meterscheduleinterval, "");
                        }
                    }
                    scheduleInterval = scheduleInterval.Trim();
                    scheduleInterval = regex.Replace(scheduleInterval, "");

                    fleetScheduledPDFPrintModel.Schedule = scheduleInterval;
                    fleetScheduledPDFPrintModel.NextDueScheduleInterval = meterscheduleinterval;
                }


                fleetScheduledPDFPrintModel.LastCompleted = "";
                if (item.LastPerformedDate != null)
                {
                    var ResourceLastCompleted = UtilityFunction.GetMessageFromResource("spnLastCompletedOn", LocalizeResourceSetConstants.EquipmentDetails);
                    fleetScheduledPDFPrintModel.LastCompletedLine1 = ResourceLastCompleted + " " + item.LastPerformedDate.Value.ToString("MM/dd/yyyy");
                    fleetScheduledPDFPrintModel.LastCompletedLine2 = item.LastPerformedMeter1 + " " + item.Meter1Units + " | " + item.LastPerformedMeter2 + " " + item.Meter2Units;
                }
                else
                {
                    var ResourceNeverPerformedForThisAsset = UtilityFunction.GetMessageFromResource("spnNeverPerformedForThisAsset", LocalizeResourceSetConstants.EquipmentDetails);
                    fleetScheduledPDFPrintModel.LastCompletedLine1 = ResourceNeverPerformedForThisAsset;
                    fleetScheduledPDFPrintModel.LastCompletedLine2 = "";

                }

                fleetScheduledPDFPrintModelList.Add(fleetScheduledPDFPrintModel);

            }
            fltschVM.fleetScheduledPDFPrintModel = fleetScheduledPDFPrintModelList;

            LocalizeControls(fltschVM, LocalizeResourceSetConstants.EquipmentDetails);
            return new ViewAsPdf("FleetScheduledGridPdfPrintTemplate", fltschVM)
            {
                PageSize = Rotativa.Options.Size.A4,
                PageMargins = { Left = 3, Right = 3, Top = 3, Bottom = 3 },
            };
        }

        [NoDirectAccess]
        public ActionResult PrintASPDF()
        {
            FleetScheduledServiceVM fltScheVM = new FleetScheduledServiceVM();
            ScheduledServiceWrapper scServWrapper = new ScheduledServiceWrapper(userData);
            FleetScheduledPDFPrintModel fleetScheduledPDFPrintModel = new FleetScheduledPDFPrintModel();
            List<FleetScheduledPDFPrintModel> fleetScheduledPDFPrintModelList = new List<FleetScheduledPDFPrintModel>();
            FleetScheduledPrintParams fleetScheduledPrintParams = (FleetScheduledPrintParams)Session["FLEETSCHDULEDPRINTPARAMS"];
            FleetScheduledServiceSearchModel fleetScheduledSearchModel = new FleetScheduledServiceSearchModel();
            string order = fleetScheduledPrintParams.colname;
            string orderDir = fleetScheduledPrintParams.coldir;
            string SearchText = fleetScheduledPrintParams.SearchText;
            string ClientLookupId = fleetScheduledPrintParams.ClientLookupId;
            string Name = fleetScheduledPrintParams.Name;
            string ServiceTaskDesc = fleetScheduledPrintParams.ServiceTaskDesc;
            string Schedule = fleetScheduledPrintParams.Schedule;

            int customQueryDisplayId = fleetScheduledPrintParams.customQueryDisplayId;

            List<FleetScheduledServiceSearchModel> fleetScheduledList = scServWrapper.GetFleetScheduledServiceGridData(customQueryDisplayId,order, orderDir, 0, 100000, ClientLookupId, Name, ServiceTaskDesc, SearchText);
            foreach (var item in fleetScheduledList)
            {
                fleetScheduledPDFPrintModel = new FleetScheduledPDFPrintModel();
                fleetScheduledPDFPrintModel.ClientLookupId = item.ClientLookupId;
                fleetScheduledPDFPrintModel.Name = item.Name;
                fleetScheduledPDFPrintModel.ImageUrl = item.ImageUrl;
                fleetScheduledPDFPrintModel.ServiceTask = item.ServiceTask;
                fleetScheduledPDFPrintModel.ServiceTaskDesc = item.ServiceTaskDesc;
                
                string scheduleInterval = "";
                if(item.TimeInterval > 0 || item.Meter1Interval > 0 || item.Meter2Interval > 0)
                {
                    scheduleInterval = "Every ";
                    if (item.TimeInterval > 0)
                    {
                        scheduleInterval += item.TimeInterval + " " + item.TimeIntervalType +" or ";
                        if (item.NextDueDate != null && item.NextDueDate == default(DateTime))
                        {
                            fleetScheduledPDFPrintModel.NextDueScheduledate = "";
                        }
                        else
                        {
                            fleetScheduledPDFPrintModel.NextDueScheduledate = item.NextDueDate.Value.ToString("MM/dd/yyyy") +" from now ";
                        }
                    }
                    string meterscheduleinterval = "";
                     if (item.Meter1Interval > 0)
                    {
                        scheduleInterval += item.Meter1Interval + " " + item.Meter1Units+" or ";
                        if (item.Meter1Units != "") {
                            meterscheduleinterval += item.NextDueMeter1 + " " + item.Meter1Units.Substring(0, 2) + " from now or ";
                        }
                        else
                        {
                            meterscheduleinterval += item.NextDueMeter1 + " " + item.Meter1Units + " from now or ";
                        }
                        
                    }
                     if (item.Meter2Interval > 0)
                    {
                        scheduleInterval += item.Meter2Interval + " " + item.Meter2Units + " or ";
                        if (item.Meter2Units != "")
                        {
                            meterscheduleinterval += item.NextDueMeter2 + " " + item.Meter2Units.Substring(0, 2) + " from now";
                        }
                        else
                        {
                            meterscheduleinterval += item.NextDueMeter2 + " " + item.Meter2Units + " from now";
                        }
                           
                    }
                    else
                    {
                        if (meterscheduleinterval != "")
                        {
                            meterscheduleinterval = meterscheduleinterval.Trim();
                            meterscheduleinterval = regex.Replace(meterscheduleinterval, "");
                        }
                    }
                    scheduleInterval = scheduleInterval.Trim();
                    scheduleInterval = regex.Replace(scheduleInterval, "");

                    fleetScheduledPDFPrintModel.Schedule = scheduleInterval;
                    fleetScheduledPDFPrintModel.NextDueScheduleInterval = meterscheduleinterval;
                }
                


                fleetScheduledPDFPrintModel.LastCompleted = "";
                if (item.LastPerformedDate != null)
                {
                    var ResourceLastCompleted = UtilityFunction.GetMessageFromResource("spnLastCompletedOn", LocalizeResourceSetConstants.EquipmentDetails);
                    fleetScheduledPDFPrintModel.LastCompletedLine1 = ResourceLastCompleted + " " + item.LastPerformedDate.Value.ToString("MM/dd/yyyy");
                    fleetScheduledPDFPrintModel.LastCompletedLine2 = item.LastPerformedMeter1 + " " + item.Meter1Units + " | " + item.LastPerformedMeter2 + " " + item.Meter2Units;
                }
                else
                {
                    var ResourceNeverPerformedForThisAsset = UtilityFunction.GetMessageFromResource("spnNeverPerformedForThisAsset", LocalizeResourceSetConstants.EquipmentDetails);
                    fleetScheduledPDFPrintModel.LastCompletedLine1 = ResourceNeverPerformedForThisAsset;
                    fleetScheduledPDFPrintModel.LastCompletedLine2 = "";

                }
                fleetScheduledPDFPrintModelList.Add(fleetScheduledPDFPrintModel);

            }
            fltScheVM.fleetScheduledPDFPrintModel = fleetScheduledPDFPrintModelList;
            LocalizeControls(fltScheVM, LocalizeResourceSetConstants.EquipmentDetails);
            return new PartialViewAsPdf("FleetScheduledGridPdfPrintTemplate", fltScheVM)
            {
                PageSize = Rotativa.Options.Size.A4,
                FileName = "FleetScheduled.pdf",
                PageMargins = { Left = 3, Right = 3, Top = 3, Bottom = 3 },
            };
        }
        public static Regex regex = new Regex("(\\s+(or)\\s*)$");
        #endregion

        #region Active/Inactive Scheduled Service

        [HttpPost]
        public JsonResult UpdateFleetScheduledStatus(long _scServeid, bool inactiveFlag)
        {
            DataContracts.ScheduledService scheduledService = new DataContracts.ScheduledService();
            ScheduledServiceWrapper fltAstWrapper = new ScheduledServiceWrapper(userData);
            var errMsg = fltAstWrapper.UpdateSchServiceActiveStatus(_scServeid, inactiveFlag);
            if (errMsg != null && errMsg.Count > 0)
            {
                return Json(errMsg, JsonRequestBehavior.AllowGet);
            }
            else
            {
                if (errMsg != null && errMsg.Count > 0)
                {
                    return Json(scheduledService.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { result = JsonReturnEnum.success.ToString(), scheduledServiceId = scheduledService.ScheduledServiceId }, JsonRequestBehavior.AllowGet);
                }
            }
        }
        #endregion
    }

}
 
 