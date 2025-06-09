using Client.ActionFilters;
using Client.BusinessWrapper.NewLaborScheduling;
using Client.Common;
using Rotativa;
using Client.Models.NewLaborScheduling;
using Common.Constants;
using DataContracts;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Rotativa.Options;
using System.IO;
using RazorEngine;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Spreadsheet;
using Client.BusinessWrapper.Common;
using Client.BusinessWrapper.Work_Order;



namespace Client.Controllers.NewLaborScheduling
{
    public class LaborSchedulingController : BaseController
    {
        [CheckUserSecurity(securityType = SecurityConstants.WorkOrder_LaborScheduling)]
        public ActionResult Index()
        {
            NewLaborSchedulingWrapper woWrapper = new NewLaborSchedulingWrapper(userData);
            NewLaborSchedulingVM newLaborSchedulingVM = new NewLaborSchedulingVM();
            WoRescheduleModel rescheduleModel = new WoRescheduleModel();
            ReassignModel reassignModel = new ReassignModel();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            string mode = Convert.ToString(TempData["Mode"]);
            if (mode == "calendarview")
            {
                ViewBag.IsListPage = false;
            }
            else
            {
                ViewBag.IsListPage = true;
            }
            // 2024-Nov-04-RKL
            // The Reassign List should "honor" the Asset Group Master Query rules
            // No action take at this time - revisit
            var totalList = woWrapper.SchedulePersonnelList();
            if (totalList != null && totalList.Count > 0)
            {
                newLaborSchedulingVM.Personnellist = totalList[0].Select(x => new SelectListItem { Text = x.FullName, Value = x.PersonnelId.ToString() }).ToList();
                rescheduleModel.Personnellist = newLaborSchedulingVM.Personnellist;
                newLaborSchedulingVM.woRescheduleModel = rescheduleModel;
                var plist = commonWrapper.PersonnelListForActiveFullUser().Where(x => x.ScheduleEmployee);
                reassignModel.Personnellist = plist.Select(x => new SelectListItem { Text = x.NameFirst + " " + x.NameLast, Value = x.PersonnelId.ToString() }).ToList();
                newLaborSchedulingVM.reassignModel = reassignModel;
            }
            newLaborSchedulingVM.ScheduledDateList = UtilityFunction.GetTimeRangeDropForScheduledDate().Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
            newLaborSchedulingVM.ScheduledGroupingList = UtilityFunction.GetGroupingDataForLaborSchedulling().Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
            LocalizeControls(newLaborSchedulingVM, LocalizeResourceSetConstants.WorkOrderDetails);
            return View(newLaborSchedulingVM);
        }
        public ActionResult ListView()
        {
            TempData["Mode"] = "listview";
            return Redirect("/LaborScheduling/Index?page=Maintenance_Work_Order_Labor_Scheduling");
        }
        public ActionResult CalendarView()
        {
            TempData["Mode"] = "calendarview";
            return Redirect("/LaborScheduling/Index?page=Maintenance_Work_Order_Labor_Scheduling");
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public string GetListLaborSchedulingGridData(int? draw, int? start, int? length, int customQueryDisplayId = 1, string ClientLookupId = "", string Name = "", string Description = "", DateTime? RequiredDate = null, DateTime? StartScheduledDate = null, DateTime? EndScheduledDate = null, string Type = "", List<string> PersonnelList = null, string SearchText = "", string Order = "0"
   )
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            var colname = Request.Form.GetValues("columns[" + order + "][name]");
            string _requiredDate = string.Empty;
            string _startScheduledDate = string.Empty;
            string _endScheduledDate = string.Empty;
            SearchText = SearchText.Replace("%", "[%]");
            ClientLookupId = ClientLookupId.Replace("%", "[%]");
            Name = Name.Replace("%", "[%]");
            Description = Description.Replace("%", "[%]");
            Type = Type.Replace("%", "[%]");
            _requiredDate = RequiredDate.HasValue ? RequiredDate.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : string.Empty;
            _startScheduledDate = StartScheduledDate.HasValue ? StartScheduledDate.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : string.Empty;
            _endScheduledDate = EndScheduledDate.HasValue ? EndScheduledDate.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : string.Empty;

            start = start.HasValue
               ? start / length
               : 0;
            int skip = start * length ?? 0;
            List<string> typeList = new List<string>();
            NewLaborSchedulingWrapper newLaborSchedulingWrapper = new NewLaborSchedulingWrapper(userData);
            List<NewLaborSchedulingSearchModel> LaborSchedulingList = newLaborSchedulingWrapper.GetListlaborSchedulingGridData(customQueryDisplayId, Order, orderDir, skip, length ?? 0, ClientLookupId, Name, Description, _requiredDate, _startScheduledDate, _endScheduledDate, Type, PersonnelList, SearchText);
            var totalRecords = 0;
            var recordsFiltered = 0;
            if (LaborSchedulingList != null && LaborSchedulingList.Count > 0)
            {
                recordsFiltered = LaborSchedulingList[0].TotalCount;
                totalRecords = LaborSchedulingList[0].TotalCount;

            }
            int initialPage = start.Value;
            var filteredResult = LaborSchedulingList
              .ToList();
            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult }, JsonSerializerDateSettings);
        }


        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public string GetListLaborSchedulingPrintData(string colname, string coldir, int customQueryDisplayId = 1, string ClientLookupId = "", string Name = "", string Description = "", DateTime? RequiredDate = null, DateTime? StartScheduledDate = null, DateTime? EndScheduledDate = null, string Type = "", List<string> PersonnelList = null, string SearchText = ""
)
        {
            List<NewLaborSchedulingPrintModel> newLaborSchedulingPrintModelList = new List<NewLaborSchedulingPrintModel>();
            NewLaborSchedulingPrintModel objNewLaborSchedulingPrintModel;
            string _requiredDate = string.Empty;
            string _startScheduledDate = string.Empty;
            string _endScheduledDate = string.Empty;
            SearchText = SearchText.Replace("%", "[%]");
            ClientLookupId = ClientLookupId.Replace("%", "[%]");
            Name = Name.Replace("%", "[%]");
            Description = Description.Replace("%", "[%]");
            Type = Type.Replace("%", "[%]");
            _requiredDate = RequiredDate.HasValue ? RequiredDate.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : string.Empty;
            _startScheduledDate = StartScheduledDate.HasValue ? StartScheduledDate.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : string.Empty;
            _endScheduledDate = EndScheduledDate.HasValue ? EndScheduledDate.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : string.Empty;

            List<string> typeList = new List<string>();
            NewLaborSchedulingWrapper newLaborSchedulingWrapper = new NewLaborSchedulingWrapper(userData);
            List<NewLaborSchedulingSearchModel> LaborSchedulingList = newLaborSchedulingWrapper.GetListlaborSchedulingGridData(customQueryDisplayId, colname, coldir, 0, 100000, ClientLookupId, Name, Description, _requiredDate, _startScheduledDate, _endScheduledDate, Type, PersonnelList, SearchText);
            foreach (var LaborScheduling in LaborSchedulingList)
            {
                objNewLaborSchedulingPrintModel = new NewLaborSchedulingPrintModel();
                objNewLaborSchedulingPrintModel.PersonnelName = LaborScheduling.PersonnelName;
                objNewLaborSchedulingPrintModel.WorkOrderClientLookupId = LaborScheduling.WorkOrderClientLookupId;
                objNewLaborSchedulingPrintModel.Description = LaborScheduling.Description;
                objNewLaborSchedulingPrintModel.Type = LaborScheduling.Type;
                objNewLaborSchedulingPrintModel.ScheduledStartDate = LaborScheduling.ScheduledStartDate;
                objNewLaborSchedulingPrintModel.ScheduledHours = LaborScheduling.ScheduledHours;
                objNewLaborSchedulingPrintModel.RequiredDate = LaborScheduling.RequiredDate;
                objNewLaborSchedulingPrintModel.EquipmentClientLookupId = LaborScheduling.EquipmentClientLookupId;
                objNewLaborSchedulingPrintModel.ChargeTo_Name = LaborScheduling.ChargeTo_Name;

                newLaborSchedulingPrintModelList.Add(objNewLaborSchedulingPrintModel);
            }
            return JsonConvert.SerializeObject(new { data = newLaborSchedulingPrintModelList }, JsonSerializerDateSettings);
        }
        public JsonResult SetPrintData(LSPrintParams LSPrintParams)
        {
            Session["LSPRINTPARAMS"] = LSPrintParams;
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        [NoDirectAccess]
        public ActionResult ExportASPDF(string d = "")
        {
            NewLaborSchedulingWrapper newLaborSchedulingWrapper = new NewLaborSchedulingWrapper(userData);
            NewLaborSchedulingPdfPrintModel objNewLaborSchedulingPdfPrintModel;
            List<NewLaborSchedulingPdfPrintModel> ListLaborSchedulingPdfPrintModelList = new List<NewLaborSchedulingPdfPrintModel>();
            NewLaborSchedulingVM newLaborSchedulingVM = new NewLaborSchedulingVM();
            var locker = new object();

            LSPrintParams LSPrintParams = (LSPrintParams)Session["LSPRINTPARAMS"];
            List<NewLaborSchedulingSearchModel> LaborSchedulingList = newLaborSchedulingWrapper.GetListlaborSchedulingGridData(LSPrintParams.CustomQueryDisplayId, LSPrintParams.colname, LSPrintParams.coldir, 0, 100000, LSPrintParams.ClientLookupId, LSPrintParams.Name, LSPrintParams.Description, LSPrintParams.RequiredDate, LSPrintParams.StartScheduledDate, LSPrintParams.EndScheduledDate, LSPrintParams.Type, LSPrintParams.PersonnelList, LSPrintParams.SearchText);

            foreach (var LaborScheduling in LaborSchedulingList)
            {
                objNewLaborSchedulingPdfPrintModel = new NewLaborSchedulingPdfPrintModel();
                objNewLaborSchedulingPdfPrintModel.PersonnelName = LaborScheduling.PersonnelName;
                objNewLaborSchedulingPdfPrintModel.WorkOrderClientLookupId = LaborScheduling.WorkOrderClientLookupId;
                objNewLaborSchedulingPdfPrintModel.Description = LaborScheduling.Description;
                objNewLaborSchedulingPdfPrintModel.Type = LaborScheduling.Type;
                if (LaborScheduling.ScheduledStartDate != null && LaborScheduling.ScheduledStartDate != default(DateTime))
                {
                    objNewLaborSchedulingPdfPrintModel.ScheduledStartDateString = LaborScheduling.ScheduledStartDate.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                }
                else
                {
                    objNewLaborSchedulingPdfPrintModel.ScheduledStartDateString = "";
                }
                objNewLaborSchedulingPdfPrintModel.ScheduledHours = LaborScheduling.ScheduledHours;
                if (LaborScheduling.RequiredDate != null && LaborScheduling.RequiredDate != default(DateTime))
                {
                    objNewLaborSchedulingPdfPrintModel.RequiredDateString = LaborScheduling.RequiredDate.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                }
                else
                {
                    objNewLaborSchedulingPdfPrintModel.RequiredDateString = "";
                }
                objNewLaborSchedulingPdfPrintModel.EquipmentClientLookupId = LaborScheduling.EquipmentClientLookupId;
                objNewLaborSchedulingPdfPrintModel.ChargeTo_Name = LaborScheduling.ChargeTo_Name;
                objNewLaborSchedulingPdfPrintModel.PerNextValue = LaborScheduling.PerNextValue;
                objNewLaborSchedulingPdfPrintModel.SDNextValue = LaborScheduling.SDNextValue;
                objNewLaborSchedulingPdfPrintModel.SumPersonnelHour = LaborScheduling.SumPersonnelHour;
                objNewLaborSchedulingPdfPrintModel.SumScheduledateHour = LaborScheduling.SumScheduledateHour;
                objNewLaborSchedulingPdfPrintModel.GrandTotalHour = LaborScheduling.GrandTotalHour;
                objNewLaborSchedulingPdfPrintModel.PerIDNextValue = LaborScheduling.PerIDNextValue;
                objNewLaborSchedulingPdfPrintModel.PersonnelId = LaborScheduling.PersonnelId;
                objNewLaborSchedulingPdfPrintModel.WorkOrderScheduleId = LaborScheduling.WorkOrderScheduleId;
                objNewLaborSchedulingPdfPrintModel.GroupType = LSPrintParams.colname;
                lock (locker)
                {
                    ListLaborSchedulingPdfPrintModelList.Add(objNewLaborSchedulingPdfPrintModel);
                }
            }
            newLaborSchedulingVM.NewLaborSchedulingPdfPrintModel = ListLaborSchedulingPdfPrintModelList;
            newLaborSchedulingVM.tableHaederProps = LSPrintParams.tableHaederProps;
            LocalizeControls(newLaborSchedulingVM, LocalizeResourceSetConstants.Global);

            if (d == "excel")
            {
                return GenerateExcelReport(newLaborSchedulingVM, "Labor Scheduling List");
            }

            if (d == "d")
            {
                return new Rotativa.PartialViewAsPdf("LSGridPdfPrintTemplate", newLaborSchedulingVM)
                {
                    PageSize = Size.A4,
                    FileName = "Labor Scheduling.pdf",
                    PageMargins = { Left = 3, Right = 3, Top = 3, Bottom = 3 },
                };
            }
            else
            {
                return new ViewAsPdf("LSGridPdfPrintTemplate", newLaborSchedulingVM)
                {
                    PageSize = Size.A4,
                    PageMargins = { Left = 3, Right = 3, Top = 3, Bottom = 3 },
                };
            }
        }

        #region ExportToExcelFunctionality
        //This code used to handle export functionality
        public ActionResult GenerateExcelReport(NewLaborSchedulingVM newLaborSchedulingVM, string reportName)
        {
            MemoryStream memoryStream = new MemoryStream();
            using (SpreadsheetDocument package = SpreadsheetDocument.Create(memoryStream, SpreadsheetDocumentType.Workbook))
            {

                CreatePartsForExcel(package, newLaborSchedulingVM);
                package.Close();
                memoryStream.Seek(0, SeekOrigin.Begin);
                return File(memoryStream.ToArray(),
                  "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
               string.Concat(reportName, ".xlsx"));
            }

        }

        //This method creating different parts of excel sheet
        private void CreatePartsForExcel(SpreadsheetDocument document, NewLaborSchedulingVM data)
        {
            int length = 0;
            List<int> groupRowIndexes = new List<int>();
            int TotalHeaderColumns = 0;
            SheetData partSheetData = GenerateSheetdataForDetails(data, ref groupRowIndexes, ref TotalHeaderColumns, ref length);
            WorkbookPart workbookPart1 = document.AddWorkbookPart();
            GenerateWorkbookPartContent(workbookPart1);
            bool hasBigRows = false;
            if (length > 50)
            {
                hasBigRows = true;
            }

            WorkbookStylesPart workbookStylesPart1 = workbookPart1.AddNewPart<WorkbookStylesPart>("rId3");
            GenerateWorkbookStylesPartContent(workbookStylesPart1, hasBigRows);

            WorksheetPart worksheetPart1 = workbookPart1.AddNewPart<WorksheetPart>("rId1");
            GenerateWorksheetPartContent(worksheetPart1, partSheetData, groupRowIndexes, TotalHeaderColumns, hasBigRows);

        }

        private SheetData GenerateSheetdataForDetails(NewLaborSchedulingVM newLaborSchedulingVM, ref List<int> groupRowIndexes, ref int TotalHeaderColumns, ref int length)
        {
            //creating content of excel sheet logic is same as genere pdf
            SheetData sheetData1 = new SheetData();
            List<string> Parentcells = new List<string>();
            List<string> Childcells = new List<string>();
            List<string> Allcells = new List<string>();

            string prevGroup = newLaborSchedulingVM.NewLaborSchedulingPdfPrintModel.Select(x => x.PersonnelName).FirstOrDefault().ToString();
            string currentGroup = prevGroup;
            long PerIDNextValue = newLaborSchedulingVM.NewLaborSchedulingPdfPrintModel.Select(x => x.PerIDNextValue).FirstOrDefault();
            long PersonnelId = 0;
            int printCount = 0;
            int thisRow = 0;
            decimal GrandTotal = newLaborSchedulingVM.NewLaborSchedulingPdfPrintModel.Select(x => x.GrandTotalHour).FirstOrDefault();
            string SDNextValue = newLaborSchedulingVM.NewLaborSchedulingPdfPrintModel.Select(x => x.SDNextValue).FirstOrDefault().ToString();
            string GroupType = newLaborSchedulingVM.NewLaborSchedulingPdfPrintModel.Select(x => x.GroupType).FirstOrDefault();

            int headercount = (from d in newLaborSchedulingVM.tableHaederProps.ToList().Where(x => x.title != null && x.title.Length > 0).ToList()
                               select d.title).ToList().Count;
            TotalHeaderColumns = headercount;

            sheetData1.Append(CreateHeaderRow(newLaborSchedulingVM));



            foreach (var item in newLaborSchedulingVM.NewLaborSchedulingPdfPrintModel)
            {
                PerIDNextValue = item.PerIDNextValue;
                PersonnelId = item.PersonnelId;
                if (GroupType == "0")
                {
                    prevGroup = item.PersonnelName;
                }
                else
                {
                    prevGroup = item.ScheduledStartDateString;
                }


                if (printCount == 0)
                {
                    printCount = 1;

                    //create group header 
                    sheetData1.Append(CreateGroupHeader(prevGroup, headercount));
                    groupRowIndexes.Add(sheetData1.ChildElements.Count);
                }



                if (thisRow % 2 == 0)
                {

                    foreach (var hed in newLaborSchedulingVM.tableHaederProps)
                    {
                        if (!string.IsNullOrEmpty(hed.title))
                        {
                            Childcells.Add(item.GetType().GetProperty(hed.property).GetValue(item, null).ToString());
                        }
                    }
                    //create row for child grid
                    sheetData1.Append(CreateRowData(Childcells, TotalHeaderColumns, 1U));
                    Allcells.AddRange(Childcells);
                    Childcells.Clear();

                }
                else
                {

                    foreach (var hed in newLaborSchedulingVM.tableHaederProps)
                    {
                        if (!string.IsNullOrEmpty(hed.title))
                        {
                            Childcells.Add(item.GetType().GetProperty(hed.property).GetValue(item, null).ToString());
                        }
                    }
                    //create row for child cells    
                    sheetData1.Append(CreateRowData(Childcells, TotalHeaderColumns, 1U));
                    Allcells.AddRange(Childcells);
                    Childcells.Clear();

                }

                if (GroupType == "0")
                {
                    if (PerIDNextValue != PersonnelId && (!String.IsNullOrEmpty(item.PerNextValue)))
                    {
                        printCount = 1;
                        prevGroup = item.PerNextValue;

                        //create row
                        foreach (var hed in newLaborSchedulingVM.tableHaederProps)
                        {
                            if (!string.IsNullOrEmpty(hed.title))
                            {
                                if (hed.title == "Hours")
                                {
                                    Parentcells.Add(item.SumPersonnelHour.ToString());
                                }
                                else
                                {
                                    //blank td
                                    Parentcells.Add("");
                                }
                            }

                        }

                        sheetData1.Append(CreateRowData(Parentcells, TotalHeaderColumns, 2U));
                        //creating row
                        Allcells.AddRange(Parentcells);
                        Parentcells.Clear();

                        //create row for group cell
                        sheetData1.Append(CreateGroupHeader(prevGroup, headercount));
                        groupRowIndexes.Add(sheetData1.ChildElements.Count);

                    }

                    //Print Total Hour count on the last
                    if ((String.IsNullOrEmpty(item.PerNextValue)))
                    {
                        foreach (var hed in newLaborSchedulingVM.tableHaederProps)
                        {
                            if (!string.IsNullOrEmpty(hed.title))
                            {
                                if (hed.title == "Hours")
                                {
                                    //create cell
                                    Parentcells.Add(item.SumPersonnelHour.ToString());
                                }
                                else
                                {
                                    Parentcells.Add("");
                                    //create cell
                                }

                            }
                        }
                        //create row darker using 2u
                        sheetData1.Append(CreateRowData(Parentcells, TotalHeaderColumns, 2U));
                        Allcells.AddRange(Parentcells);
                        Parentcells.Clear();

                    }
                }
                else
                {
                    SDNextValue = item.SDNextValue.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                    if (SDNextValue != item.ScheduledStartDateString && (item.SDNextValue != DateTime.MinValue))
                    {
                        printCount = 1;
                        prevGroup = SDNextValue;
                        foreach (var hed in newLaborSchedulingVM.tableHaederProps)
                        {
                            if (!string.IsNullOrEmpty(hed.title))
                            {
                                if (hed.title == "Hours")
                                {
                                    Parentcells.Add(item.SumScheduledateHour.ToString());
                                }
                                else
                                {
                                    Parentcells.Add("");
                                }
                            }

                        }

                        //create row
                        sheetData1.Append(CreateRowData(Parentcells, TotalHeaderColumns, 2U));
                        Allcells.AddRange(Parentcells);
                        Parentcells.Clear();

                        //creat parent row
                        sheetData1.Append(CreateGroupHeader(prevGroup, headercount));
                        groupRowIndexes.Add(sheetData1.ChildElements.Count);

                    }

                    if ((item.SDNextValue == DateTime.MinValue))
                    {
                        foreach (var hed in newLaborSchedulingVM.tableHaederProps)
                        {
                            if (!string.IsNullOrEmpty(hed.title))
                            {
                                if (hed.title == "Hours")
                                {
                                    Parentcells.Add(item.SumScheduledateHour.ToString());
                                    //parent cell
                                }
                                else
                                {
                                    Parentcells.Add("");
                                    //blank cell
                                }

                            }
                        }
                        //create row for parent
                        sheetData1.Append(CreateRowData(Parentcells, TotalHeaderColumns, 2U));
                        Allcells.AddRange(Parentcells);
                        Parentcells.Clear();

                    }

                    thisRow++;
                }

            }
            foreach (var hed in newLaborSchedulingVM.tableHaederProps)
            {
                if (!string.IsNullOrEmpty(hed.title))
                {
                    if (hed.title == "Hours")
                    {
                        //create grand total lable cell and grand total cell
                        Parentcells.Add("Grand Total:");
                        Parentcells.Add(GrandTotal.ToString());

                    }
                    else
                    {
                        Parentcells.Add("");
                        //blank td
                    }
                }

            }
            Parentcells.RemoveAt(0);
            sheetData1.Append(CreateRowData(Parentcells, TotalHeaderColumns, 2U));
            Parentcells.Clear();

            //based on this length we identify content has 
            //bigger row so we make cell width fixed while export
            if (Allcells.Count > 0)
            {
                length = Allcells.OrderByDescending(s => s.Length).First().Length;
            }
            return sheetData1;
        }

        //create header row parent grid
        private Row CreateHeaderRow(NewLaborSchedulingVM data)
        {
            var headrs = new List<string>();
            headrs = (from d in data.tableHaederProps.ToList().Where(x => x.title != null && x.title.Length > 0).ToList()
                      select d.title).ToList();
            return CreateRowData(headrs, 0, 2U);
        }
        //create header row for child grid
        private Row CreateHeaderRowForChildGrid(NewLaborSchedulingVM data)
        {
            var headrs = new List<string>();
            headrs = (from d in data.tableHaederProps.ToList().Where(x => x.title != null && x.title.Length > 0).ToList()
                      select d.title).ToList();
            return CreateRowData(headrs, 0, 2U);
        }

        #endregion


        #region Add ReSchedule  /*(V2-524)*/
        [HttpPost]
        public JsonResult AddReSchedule(NewLaborSchedulingVM lsVM)
        {
            NewLaborSchedulingVM objLSVM = new NewLaborSchedulingVM();
            NewLaborSchedulingWrapper newLaborSchedulingWrapper = new NewLaborSchedulingWrapper(userData);
            System.Text.StringBuilder PersonnelWoList = new System.Text.StringBuilder();
            if (string.IsNullOrEmpty(lsVM.woRescheduleModel.WorkOrderIds))
            {
                var objWorkOrder = newLaborSchedulingWrapper.AddReScheduleRecord(lsVM.woRescheduleModel);
                if (objWorkOrder.ErrorMessages != null && objWorkOrder.ErrorMessages.Count > 0)
                {
                    return Json(objWorkOrder.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { data = JsonReturnEnum.success.ToString(), workorderid = lsVM.woRescheduleModel.WorkOrderId }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                string[] workorderIds = lsVM.woRescheduleModel.WorkOrderIds.Split(',');
                string[] clientLookupIds = lsVM.woRescheduleModel.ClientLookupIds.Split(',');
                string[] status = lsVM.woRescheduleModel.Status.Split(',');
                string[] ScheduledDurations = lsVM.woRescheduleModel.ScheduledDurations.Split(',');
                List<string> errorMessage = new List<string>();
                System.Text.StringBuilder failedWoList = new System.Text.StringBuilder();
                for (int i = 0; i < workorderIds.Length; i++)
                {
                    string Statusmsg = string.Empty;
                    lsVM.woRescheduleModel.WorkOrderId = Convert.ToInt64(workorderIds[i]);
                    lsVM.woRescheduleModel.ScheduledDuration = Convert.ToDecimal(ScheduledDurations[i]);
                    var objWorkOrder = newLaborSchedulingWrapper.AddReScheduleRecord(lsVM.woRescheduleModel);
                    if (objWorkOrder.ErrorMessages != null && objWorkOrder.ErrorMessages.Count > 0)
                    {
                        string errormessage = "Failed to schedule " + objWorkOrder.ClientLookupId + ": " + objWorkOrder.ErrorMessages[0];
                        errorMessage.Add(errormessage);
                    }
                }
                if (errorMessage.Count > 0 || !string.IsNullOrEmpty(failedWoList.ToString()))
                {
                    return Json(new { data = errorMessage }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { data = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
                }
            }
        }
        #endregion

        #region Reassign 
        [HttpPost]
        public JsonResult Reassign(NewLaborSchedulingVM lsVM)
        {
            NewLaborSchedulingWrapper newLaborSchedulingWrapper = new NewLaborSchedulingWrapper(userData);
            List<string> NotAssignedWorkOrderClientLookupIdsList = new List<string>();
            List<string> errorMessage = new List<string>();
            var objWorkOrder = newLaborSchedulingWrapper.ReassignWorkOrderScheduleRecord(lsVM.reassignModel, ref NotAssignedWorkOrderClientLookupIdsList);
            if (objWorkOrder.ErrorMessages != null && objWorkOrder.ErrorMessages.Count > 0)
            {
                string errormessage = objWorkOrder.ErrorMessages[0];
                errorMessage.Add(errormessage);
            }
            if (errorMessage.Count > 0)
            {
                return Json(new { data = errorMessage }, JsonRequestBehavior.AllowGet);
            }
            else if(NotAssignedWorkOrderClientLookupIdsList.Count > 0)
            {
                return Json(new { NotAssignedWorkOrderClientLookupIdsList = NotAssignedWorkOrderClientLookupIdsList }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { data = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
            }

        }
        #endregion

        #region Remove Schedule V2-524
        public JsonResult RemoveScheduleList(RemoveScheduleModel model)
        {
            NewLaborSchedulingVM objLaborScheduleVM = new NewLaborSchedulingVM();
            NewLaborSchedulingWrapper newLaborSchedulingWrapper = new NewLaborSchedulingWrapper(userData);
            List<string> errorMessage = new List<string>();
            System.Text.StringBuilder failedWoList = new System.Text.StringBuilder();
            foreach (var item in model.list)
            {
                if (item.Status != WorkOrderStatusConstants.Scheduled)
                {
                    failedWoList.Append(item.ClientLookupId + ",");
                }
                else
                {
                    string Statusmsg = string.Empty;
                    var objWorkOrder = newLaborSchedulingWrapper.RemoveWorkOrderScheduleForLaborScheduling(item.WorkOrderId, item.WorkOrderSchedId);
                    if (objWorkOrder.ErrorMessages != null && objWorkOrder.ErrorMessages.Count > 0)
                    {
                        string errormessage = "Failed to remove schedule " + item.ClientLookupId + ": " + objWorkOrder.ErrorMessages[0];
                        errorMessage.Add(errormessage);
                    }
                }
            }
            if (errorMessage.Count > 0 || !string.IsNullOrEmpty(failedWoList.ToString()))
            {
                if (!string.IsNullOrEmpty(failedWoList.ToString()))
                {
                    errorMessage.Add("Work Order(s) " + failedWoList + " can't be remove schedule. Please check the status.");
                }
                return Json(new { data = errorMessage }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { data = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
            }


        }
        #endregion

        #region Available Work V2-524
        public PartialViewResult AvailableWorkOrders()
        {
            NewLaborSchedulingWrapper woWrapper = new NewLaborSchedulingWrapper(userData);
            NewLaborSchedulingVM objLSVM = new NewLaborSchedulingVM();
            AvailableWorkAssignModel availableWorkAssign = new AvailableWorkAssignModel();
            var totalList = woWrapper.SchedulePersonnelList();
            if (totalList != null && totalList.Count > 0)
            {
                availableWorkAssign.Personnellist = totalList[0].Select(x => new SelectListItem { Text = x.FullName, Value = x.PersonnelId.ToString() }).ToList();
                objLSVM.availableWorkAssignModel = availableWorkAssign;
            }

            #region V2-984
            List<DataContracts.LookupList> AllLookUps = new List<DataContracts.LookupList>();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            AvailableWoScheduleModel availableWorkOrder = new AvailableWoScheduleModel();
            AllLookUps = commonWrapper.GetAllLookUpList();
            //status
            var StatusList = commonWrapper.GetListFromConstVals(LookupListConstants.WO_Status);
            if (StatusList != null)
            {
                availableWorkOrder.StatusList = StatusList.Select(x => new SelectListItem { Text = x.text.ToString(), Value = x.value.ToString() });
            }
            //priority
            var Priority = AllLookUps.Where(x => x.ListName == LookupListConstants.WO_Priority);
            if (Priority != null)
            {
                var tmpPriority = Priority.GroupBy(x => x.ListValue).Select(x => x.FirstOrDefault()).ToList();
                availableWorkOrder.PriorityList = tmpPriority.Select(x => new SelectListItem { Text = x.Description, Value = x.ListValue }).Distinct().ToList();
            }
            //Downrequired
            var DownRequiredStatusList = UtilityFunction.DownRequiredStatusTypesWithBoolValue();
            if (DownRequiredStatusList != null)
            {
                availableWorkOrder.DownRequiredInactiveFlagList = DownRequiredStatusList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
            }
            //assigned
            var PersonnelLookUplist = GetList_PersonnelV2();
            if (PersonnelLookUplist != null)
            {
                availableWorkOrder.PersonnelIdList = PersonnelLookUplist.Select(x => new SelectListItem { Text = x.AssignedTo_PersonnelClientLookupId + " - " + x.NameFirst + " - " + x.NameLast, Value = x.AssignedTo_PersonnelId.ToString() });
            }
            //type
            var Type = AllLookUps.Where(x => x.ListName == LookupListConstants.WO_TYPE).ToList();
            if (Type != null)
            {
                var tmpTypeList = Type.GroupBy(x => x.ListValue).Select(x => x.FirstOrDefault()).ToList();
                availableWorkOrder.TypeList = tmpTypeList.Select(x => new SelectListItem { Text = x.ListValue + " - " + x.Description, Value = x.ListValue });
            }
            objLSVM.availableWOModel = availableWorkOrder;
            #endregion

            LocalizeControls(objLSVM, LocalizeResourceSetConstants.WorkOrderDetails);
            return PartialView("~/Views/LaborScheduling/_AvailableWorkOrder.cshtml", objLSVM);
        }

        [HttpGet]
        public JsonResult GetLaborAvailable(string flag)
        {
            NewLaborSchedulingWrapper pWrapper = new NewLaborSchedulingWrapper(userData);
            List<AvailableWoScheduleModel> LabourAvailableList = pWrapper.PopulateLaborAvailable(flag);
            var jsonresult= Json(LabourAvailableList, JsonRequestBehavior.AllowGet);
            jsonresult.MaxJsonLength = int.MaxValue;
            return jsonresult;
        }

        [HttpPost]
        public string GetAvailableWorkOrderMainGrid(int? draw, int? start, int? length, string ClientLookupId, string ChargeTo, string ChargeToName,
         string Description, List<string> Status, List<string> Priority, List<string> Type, DateTime? RequiredDate, List<string> Assigned, string flag = "0")
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            string _RequiredDate = string.Empty; //V2-984
            _RequiredDate = RequiredDate.HasValue ? RequiredDate.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : string.Empty; //V2-984
            NewLaborSchedulingWrapper newLaborSchedulingWrapper = new NewLaborSchedulingWrapper(userData);
            start = start.HasValue
              ? start / length
              : 0;
            int skip = start * length ?? 0;
            List<AvailableWoScheduleModel> LabourAvailableList = newLaborSchedulingWrapper.GetAvailableWorklaborSchedulingGridData(order, orderDir, skip, length ?? 0, ClientLookupId, ChargeTo,
                ChargeToName, Description, Status, Priority, Type, Assigned, _RequiredDate, flag);

            var totalRecords = 0;
            var recordsFiltered = 0;
            if (LabourAvailableList != null && LabourAvailableList.Count > 0)
            {
                recordsFiltered = LabourAvailableList[0].TotalCount;
                totalRecords = LabourAvailableList[0].TotalCount;
            }
            int initialPage = start.Value;
            var filteredResult = LabourAvailableList
              .ToList();

            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult }, JsonSerializerDateSettings);
        }

        [HttpPost]
        public JsonResult AddAvailableWorkAssign(NewLaborSchedulingVM lsVM)
        {
            NewLaborSchedulingVM objLSVM = new NewLaborSchedulingVM();
            NewLaborSchedulingWrapper newLaborSchedulingWrapper = new NewLaborSchedulingWrapper(userData);
            System.Text.StringBuilder PersonnelWoList = new System.Text.StringBuilder();
            if (string.IsNullOrEmpty(lsVM.availableWorkAssignModel.WorkOrderIds))
            {
                var objWorkOrder = newLaborSchedulingWrapper.AddAvailableWorkAssign(lsVM.availableWorkAssignModel);
                if (objWorkOrder.ErrorMessages != null && objWorkOrder.ErrorMessages.Count > 0)
                {
                    return Json(objWorkOrder.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { data = JsonReturnEnum.success.ToString(), workorderid = lsVM.availableWorkAssignModel.WorkOrderId }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                string[] workorderIds = lsVM.availableWorkAssignModel.WorkOrderIds.Split(',');
                string[] clientLookupIds = lsVM.availableWorkAssignModel.ClientLookupIds.Split(',');
                string[] status = lsVM.availableWorkAssignModel.Status.Split(',');
                List<string> errorMessage = new List<string>();
                System.Text.StringBuilder failedWoList = new System.Text.StringBuilder();
                for (int i = 0; i < workorderIds.Length; i++)
                {
                    string Statusmsg = string.Empty;
                    lsVM.availableWorkAssignModel.WorkOrderId = Convert.ToInt64(workorderIds[i]);
                    var objWorkOrder = newLaborSchedulingWrapper.AddAvailableWorkAssign(lsVM.availableWorkAssignModel);
                    if (objWorkOrder.ErrorMessages != null && objWorkOrder.ErrorMessages.Count > 0)
                    {
                        string errormessage = "Failed to schedule " + objWorkOrder.ClientLookupId + ": " + objWorkOrder.ErrorMessages[0];
                        errorMessage.Add(errormessage);
                    }
                }
                if (errorMessage.Count > 0 || !string.IsNullOrEmpty(failedWoList.ToString()))
                {
                    return Json(new { data = errorMessage }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { data = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
                }
            }
        }
        #endregion

        #region Labor Scheduling Calendar Data
        [HttpPost]
        public JsonResult GetLaborSchedulingCalendarData(string StartDt, string EndDt, List<string> PersonnelList = null)
        {
            NewLaborSchedulingWrapper newLaborSchedulingWrapper = new NewLaborSchedulingWrapper(userData);

            List<NewLaborSchedulingCalendarModel> LaborSchedulingList = newLaborSchedulingWrapper.GetLaborSchedulingCalendarData(StartDt, EndDt, PersonnelList);

            var ListPersonnel = LaborSchedulingList
                                .Select(x => new { x.PersonnelFull, x.PersonnelId, x.ScheduledHours, x.PartOnOrder })
                                .GroupBy(g => new { g.PersonnelFull, g.PersonnelId })
                                .Select(y => new { y.Key.PersonnelFull, y.Key.PersonnelId, ScheduledHours = y.Sum(h => h.ScheduledHours) })
                                .OrderBy(x => x.PersonnelFull)
                                .ToList();
            var ReturnObj = new { LaborSchedulingList, ListPersonnel };
            return Json(ReturnObj);
        }
        #endregion

        #region Add schedule calendar        
        public ActionResult AddScheduleCalendar()
        {
            NewLaborSchedulingWrapper wrapper = new NewLaborSchedulingWrapper(userData);
            NewLaborSchedulingVM newLaborSchedulingVM = new NewLaborSchedulingVM();
            AddSchedlingCalendarModal addSchedlingCalendarModal = new AddSchedlingCalendarModal();

            var totalList = wrapper.SchedulePersonnelList();
            if (totalList != null && totalList.Count > 0)
            {
                newLaborSchedulingVM.Personnellist = totalList[0].Select(x => new SelectListItem { Text = x.FullName, Value = x.PersonnelId.ToString() }).ToList();
            }
            List<SelectListItem> WorkOrderList = wrapper.RetrieveApprovedWorkOrderForLaborScheduling();
            if (WorkOrderList != null && WorkOrderList.Count > 0)
            {
                newLaborSchedulingVM.WorkOrderList = WorkOrderList;
            }
            else
            {
                return Json(new { WorkOrderListCount = 0 }, JsonRequestBehavior.AllowGet);
            }
            LocalizeControls(newLaborSchedulingVM, LocalizeResourceSetConstants.WorkOrderDetails);
            return PartialView("_AddScheduleCalendar", newLaborSchedulingVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddScheduleCalendar(NewLaborSchedulingVM newLaborSchedulingVM)
        {
            string ModelValidationFailedMessage = string.Empty;
            if (ModelState.IsValid)
            {
                NewLaborSchedulingVM objLSVM = new NewLaborSchedulingVM();
                NewLaborSchedulingWrapper newLaborSchedulingWrapper = new NewLaborSchedulingWrapper(userData);

                WoRescheduleModel woRescheduleModel = new WoRescheduleModel()
                {
                    WorkOrderId = newLaborSchedulingVM.AddSchedlingCalendarModal.WorkOrderID,
                    ScheduledDuration = newLaborSchedulingVM.AddSchedlingCalendarModal.Hours ?? 0,
                    Schedulestartdate = newLaborSchedulingVM.AddSchedlingCalendarModal.ScheduleDate,
                    PersonnelIds = newLaborSchedulingVM.AddSchedlingCalendarModal.Personnels.ToList()
                };

                var objWorkOrder = newLaborSchedulingWrapper.AddReScheduleRecord(woRescheduleModel);
                if (objWorkOrder.ErrorMessages != null && objWorkOrder.ErrorMessages.Count > 0)
                {
                    return Json(objWorkOrder.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { data = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }

        }
        #endregion

        #region Edit and remove Schedule calendar
        public PartialViewResult EditScheduleCalendar(long WorkOrderSchedId, long Workorderid)
        {
            NewLaborSchedulingWrapper wrapper = new NewLaborSchedulingWrapper(userData);
            NewLaborSchedulingVM newLaborSchedulingVM = new NewLaborSchedulingVM();
            EditSchedlingCalendarModal editSchedlingCalendarModal = new EditSchedlingCalendarModal();
            WorkOrderSchedule workOrderSchedule = wrapper.RetrieveWorkOrderSchedule(Workorderid, WorkOrderSchedId);

            if (workOrderSchedule != null)
            {
                editSchedlingCalendarModal.WorkOrderID = Workorderid;
                editSchedlingCalendarModal.WorkOrderScheduledID = WorkOrderSchedId;
                editSchedlingCalendarModal.Description = workOrderSchedule.Description;
                editSchedlingCalendarModal.ScheduleDate = workOrderSchedule.ScheduledStartDate;
                editSchedlingCalendarModal.Hours = workOrderSchedule.ScheduledHours;
                editSchedlingCalendarModal.PersonnelName = workOrderSchedule.NameFirst + " " + workOrderSchedule.NameLast;
                editSchedlingCalendarModal.ClientLookupId = workOrderSchedule.ClientLookupId;
            }
            newLaborSchedulingVM.EditSchedlingCalendarModal = editSchedlingCalendarModal;
            LocalizeControls(newLaborSchedulingVM, LocalizeResourceSetConstants.WorkOrderDetails);
            return PartialView("_EditScheduleCalendar", newLaborSchedulingVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult EditScheduleCalendar(NewLaborSchedulingVM newLaborSchedulingVM)
        {
            string ModelValidationFailedMessage = string.Empty;
            if (ModelState.IsValid)
            {
                NewLaborSchedulingWrapper newLaborSchedulingWrapper = new NewLaborSchedulingWrapper(userData);

                var responsetxt = newLaborSchedulingWrapper.UpdateSchedulingRecords_V2(
                                newLaborSchedulingVM.EditSchedlingCalendarModal.WorkOrderScheduledID,
                                newLaborSchedulingVM.EditSchedlingCalendarModal.Hours ?? 0,
                                newLaborSchedulingVM.EditSchedlingCalendarModal.ScheduleDate);
                if (string.IsNullOrEmpty(responsetxt))
                {
                    return Json(new { data = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(JsonReturnEnum.failed.ToString(), JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult RemoveScheduleCalendar(long WOId, long WOSchedId)
        {
            NewLaborSchedulingWrapper newLaborSchedulingWrapper = new NewLaborSchedulingWrapper(userData);

            var objWorkOrder = newLaborSchedulingWrapper.RemoveWorkOrderScheduleForLaborScheduling(WOId, WOSchedId);
            if (objWorkOrder.ErrorMessages != null && objWorkOrder.ErrorMessages.Count > 0)
            {
                return Json(objWorkOrder.ErrorMessages, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { data = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region drag schedule calendar
        [HttpPost]
        public JsonResult DragScheduleCalendar(long WorkOrderScheduledID, long WOId, string ScheduleDate)
        {
            NewLaborSchedulingVM objLSVM = new NewLaborSchedulingVM();
            NewLaborSchedulingWrapper newLaborSchedulingWrapper = new NewLaborSchedulingWrapper(userData);
            DateTime _ScheduleDate = DateTime.ParseExact(ScheduleDate, "MM/dd/yyyy", CultureInfo.InvariantCulture);
            var objWorkOrder = newLaborSchedulingWrapper.DragWorkOrderScheduleFromCalendar(WOId, WorkOrderScheduledID, _ScheduleDate);
            if (objWorkOrder.ErrorMessages != null && objWorkOrder.ErrorMessages.Count > 0)
            {
                return Json(objWorkOrder.ErrorMessages, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { data = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Available Work calendar view
        public PartialViewResult AvailableWorkOrdersCalendar()
        {
            NewLaborSchedulingWrapper woWrapper = new NewLaborSchedulingWrapper(userData);
            NewLaborSchedulingVM objLSVM = new NewLaborSchedulingVM();
            AvailableWorkAssignCalendarModel availableWorkAssign = new AvailableWorkAssignCalendarModel();
            var totalList = woWrapper.SchedulePersonnelList();
            if (totalList != null && totalList.Count > 0)
            {
                availableWorkAssign.Personnellist = totalList[0].Select(x => new SelectListItem { Text = x.FullName, Value = x.PersonnelId.ToString() }).ToList();

                objLSVM.availableWorkAssignCalendarModel = availableWorkAssign;
            }
            #region V2-984
            List<DataContracts.LookupList> AllLookUps = new List<DataContracts.LookupList>();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            AvailableWoScheduleModel availableWorkOrder = new AvailableWoScheduleModel();
            AllLookUps = commonWrapper.GetAllLookUpList();
            //status
            var StatusList = commonWrapper.GetListFromConstVals(LookupListConstants.WO_Status);
            if (StatusList != null)
            {
                availableWorkOrder.StatusList = StatusList.Select(x => new SelectListItem { Text = x.text.ToString(), Value = x.value.ToString() });
            }
            //priority
            var Priority = AllLookUps.Where(x => x.ListName == LookupListConstants.WO_Priority);
            if (Priority != null)
            {
                var tmpPriority = Priority.GroupBy(x => x.ListValue).Select(x => x.FirstOrDefault()).ToList();
                availableWorkOrder.PriorityList = tmpPriority.Select(x => new SelectListItem { Text = x.Description, Value = x.ListValue }).Distinct().ToList();
            }
            //Downrequired
            var DownRequiredStatusList = UtilityFunction.DownRequiredStatusTypesWithBoolValue();
            if (DownRequiredStatusList != null)
            {
                availableWorkOrder.DownRequiredInactiveFlagList = DownRequiredStatusList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
            }
            //assigned
            var PersonnelLookUplist = GetList_PersonnelV2();
            if (PersonnelLookUplist != null)
            {
                availableWorkOrder.PersonnelIdList = PersonnelLookUplist.Select(x => new SelectListItem { Text = x.AssignedTo_PersonnelClientLookupId + " - " + x.NameFirst + " - " + x.NameLast, Value = x.AssignedTo_PersonnelId.ToString() });
            }
            //type
            var Type = AllLookUps.Where(x => x.ListName == LookupListConstants.WO_TYPE).ToList();
            if (Type != null)
            {
                var tmpTypeList = Type.GroupBy(x => x.ListValue).Select(x => x.FirstOrDefault()).ToList();
                availableWorkOrder.TypeList = tmpTypeList.Select(x => new SelectListItem { Text = x.ListValue + " - " + x.Description, Value = x.ListValue });
            }
            objLSVM.availableWOModel = availableWorkOrder;
            #endregion
            LocalizeControls(objLSVM, LocalizeResourceSetConstants.WorkOrderDetails);
            return PartialView("~/Views/LaborScheduling/_AvailableWorkOrderCalendar.cshtml", objLSVM);
        }

        [HttpGet]
        public JsonResult GetLaborAvailableCalendar(string flag)
        {
            NewLaborSchedulingWrapper pWrapper = new NewLaborSchedulingWrapper(userData);
            List<AvailableWoScheduleModel> LabourAvailableList = pWrapper.PopulateLaborAvailable(flag);          
            var jsonresult = Json(LabourAvailableList, JsonRequestBehavior.AllowGet);
            jsonresult.MaxJsonLength = int.MaxValue;
            return jsonresult;
        }

        [HttpPost]
        public string GetAvailableWorkOrderCalendarMainGrid(int? draw, int? start, int? length, string ClientLookupId, string ChargeTo, string ChargeToName,
         string Description, List<string> Status, List<string> Priority, List<string> Assigned, List<string> Type, DateTime? RequiredDate, string flag = "0")
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            string _RequiredDate = string.Empty;
            _RequiredDate = RequiredDate.HasValue ? RequiredDate.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : string.Empty;
            NewLaborSchedulingWrapper newLaborSchedulingWrapper = new NewLaborSchedulingWrapper(userData);
            start = start.HasValue
              ? start / length
              : 0;
            int skip = start * length ?? 0;
            List<AvailableWoScheduleModel> LabourAvailableList = newLaborSchedulingWrapper.GetAvailableWorklaborSchedulingGridDataCalendar(order, orderDir, skip, length ?? 0, ClientLookupId, ChargeTo,
                ChargeToName, Description, Status, Priority, Type, Assigned, _RequiredDate, flag);
            var totalRecords = 0;
            var recordsFiltered = 0;
            if (LabourAvailableList != null && LabourAvailableList.Count > 0)
            {
                recordsFiltered = LabourAvailableList[0].TotalCount;
                totalRecords = LabourAvailableList[0].TotalCount;
            }
            int initialPage = start.Value;
            var filteredResult = LabourAvailableList
              .ToList();
            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult }, JsonSerializerDateSettings);
        }


        [HttpPost]
        public JsonResult AddAvailableWorkAssignCalendar(NewLaborSchedulingVM lsVM)
        {
            string ModelValidationFailedMessage = "";
            if (ModelState.IsValid)
            {
                NewLaborSchedulingVM objLSVM = new NewLaborSchedulingVM();
                NewLaborSchedulingWrapper newLaborSchedulingWrapper = new NewLaborSchedulingWrapper(userData);
                AvailableWorkAssignModel availableWorkAssignModel = new AvailableWorkAssignModel()
                {
                    PersonnelId = lsVM.availableWorkAssignCalendarModel.PersonnelId,
                    Schedulestartdate = lsVM.availableWorkAssignCalendarModel.Schedulestartdate,
                    ScheduledDuration = lsVM.availableWorkAssignCalendarModel.ScheduledDuration,
                    WorkOrderId = lsVM.availableWorkAssignCalendarModel.WorkOrderId,
                    PersonnelIds = lsVM.availableWorkAssignCalendarModel.PersonnelIds.ToList(),
                    WorkOrderIds = lsVM.availableWorkAssignCalendarModel.WorkOrderIds,
                    AssignedPersonnelId = lsVM.availableWorkAssignCalendarModel.AssignedPersonnelId,
                    ClientLookupIds = lsVM.availableWorkAssignCalendarModel.ClientLookupIds,
                    Status = lsVM.availableWorkAssignCalendarModel.Status,
                };
                lsVM.availableWorkAssignModel = availableWorkAssignModel;
                System.Text.StringBuilder PersonnelWoList = new System.Text.StringBuilder();
                if (string.IsNullOrEmpty(lsVM.availableWorkAssignModel.WorkOrderIds))
                {
                    var objWorkOrder = newLaborSchedulingWrapper.AddAvailableWorkAssignCalendar(lsVM.availableWorkAssignModel);
                    if (objWorkOrder.ErrorMessages != null && objWorkOrder.ErrorMessages.Count > 0)
                    {
                        return Json(objWorkOrder.ErrorMessages, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { data = JsonReturnEnum.success.ToString(), workorderid = lsVM.availableWorkAssignModel.WorkOrderId }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    string[] workorderIds = lsVM.availableWorkAssignModel.WorkOrderIds.Split(',');
                    string[] clientLookupIds = lsVM.availableWorkAssignModel.ClientLookupIds.Split(',');
                    string[] status = lsVM.availableWorkAssignModel.Status.Split(',');
                    List<string> errorMessage = new List<string>();
                    System.Text.StringBuilder failedWoList = new System.Text.StringBuilder();
                    for (int i = 0; i < workorderIds.Length; i++)
                    {
                        string Statusmsg = string.Empty;
                        lsVM.availableWorkAssignModel.WorkOrderId = Convert.ToInt64(workorderIds[i]);
                        var objWorkOrder = newLaborSchedulingWrapper.AddAvailableWorkAssignCalendar(lsVM.availableWorkAssignModel);
                        if (objWorkOrder.ErrorMessages != null && objWorkOrder.ErrorMessages.Count > 0)
                        {
                            string errormessage = "Failed to schedule " + objWorkOrder.ClientLookupId + ": " + objWorkOrder.ErrorMessages[0];
                            errorMessage.Add(errormessage);
                        }
                    }
                    if (errorMessage.Count > 0 || !string.IsNullOrEmpty(failedWoList.ToString()))
                    {
                        return Json(new { data = errorMessage }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { data = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            else
            {
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion


        public JsonResult UpdateWoHours(long WorkOrderSchedId, decimal hours)
        {
            NewLaborSchedulingWrapper laborScheduleWrapper = new NewLaborSchedulingWrapper(userData);
            var updateErrorResult = laborScheduleWrapper.UpdateSchedulingRecords(WorkOrderSchedId, hours);
            if (string.IsNullOrEmpty(updateErrorResult))
            {

                return Json(new { Result = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Result = JsonReturnEnum.failed.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }
        #region  Update Wo Hours and  Schedule Date V2-562
        public JsonResult UpdateWoHoursScheduleDate(long WorkOrderSchedId, decimal hours, string ScheduleDate)
        {
            NewLaborSchedulingWrapper laborScheduleWrapper = new NewLaborSchedulingWrapper(userData);
            var dtScheduled = DateTime.ParseExact(ScheduleDate, "MM/dd/yyyy", CultureInfo.InvariantCulture);
            DateTime dtScheduleDate = Convert.ToDateTime(dtScheduled);
            var updateErrorResult = laborScheduleWrapper.UpdateSchedulingRecords_V2(WorkOrderSchedId, hours, dtScheduleDate);
            if (string.IsNullOrEmpty(updateErrorResult))
            {

                return Json(new { Result = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Result = JsonReturnEnum.failed.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Hover for Assigned user V2-984
        [HttpPost]
        public JsonResult PopulateHover(long workOrderId = 0)
        {
            WorkOrderWrapper woWrapper = new WorkOrderWrapper(userData);
            string personnelList = woWrapper.PopulateHoverList(workOrderId);
            if (!string.IsNullOrEmpty(personnelList))
            {
                personnelList = personnelList.Trim().TrimEnd(',');
            }
            return Json(new { personnelList = personnelList }, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}
