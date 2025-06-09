using Client.ActionFilters;
using Client.BusinessWrapper.Common;
using Client.BusinessWrapper.ProjectsCosting;
using Client.Common;
using Client.Common.Constants;
using Client.Controllers.Common;
using Client.Localization;
using Client.Models;
using Client.Models.ProjectCosting;
using Common.Constants;
using DataContracts;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml;
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
using static Client.Models.Common.UserMentionDataModel;
namespace Client.Controllers.ProjectCosting
{
    public class ProjectCostingController : SomaxBaseController
    {
        [CheckUserSecurity(securityType = SecurityConstants.Project)]
        public ActionResult Index()
        {
            ProjectCostingVM objProjectCostingVM = new ProjectCostingVM();
            ProjectCostingModel pcm = new ProjectCostingModel();
            objProjectCostingVM.security = this.userData.Security;
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            ProjectCostingSearchWrapper projSearchWrapper = new ProjectCostingSearchWrapper(userData);
            objProjectCostingVM.ProjectViewList = commonWrapper.PopulateCustomQueryDisplay(AttachmentTableConstant.Project);
            pcm.DateRangeDropListForAllStatus = UtilityFunction.GetTimeRangeDropForAllStatusProj().Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
            pcm.DateRangeDropListForCompletedProject = UtilityFunction.GetTimeRangeDropForCompletedProj().Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
            pcm.DateRangeDropListForClosedProject = UtilityFunction.GetTimeRangeDropForClosedProj().Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
            //status
            var StatusList = commonWrapper.GetListFromConstVals(LookupListConstants.Project_Status);
            if (StatusList != null)
            {
                pcm.StatusList = StatusList.Select(x => new SelectListItem { Text = x.text.ToString(), Value = x.value.ToString() });
            }


            // Retrieve Asset Group 1 dropdown data
            var ast1 = projSearchWrapper.GetAssetGroup1Dropdowndata();
            if (ast1 != null)
            {
                // Map the retrieved data to a SelectListItem for AssignedGroup1List
                objProjectCostingVM.AssignedGroup1List = ast1.Select(x => new SelectListItem
                {
                    Text = x.AssetGroup1DescWithClientLookupId,
                    Value = x.AssetGroup1Id.ToString()
                });
            }

            // Retrieve Asset Group 2 dropdown data
            var ast2 = projSearchWrapper.GetAssetGroup2Dropdowndata();
            if (ast2 != null)
            {
                // Map the retrieved data to a SelectListItem for AssignedGroup2List
                objProjectCostingVM.AssignedGroup2List = ast2.Select(x => new SelectListItem
                {
                    Text = x.AssetGroup2DescWithClientLookupId,
                    Value = x.AssetGroup2Id.ToString()
                });
            }

            // Retrieve Asset Group 3 dropdown data
            var ast3 = projSearchWrapper.GetAssetGroup3Dropdowndata();
            if (ast3 != null)
            {
                // Map the retrieved data to a SelectListItem for AssignedGroup3List
                objProjectCostingVM.AssignedGroup3List = ast3.Select(x => new SelectListItem
                {
                    Text = x.AssetGroup3DescWithClientLookupId,
                    Value = x.AssetGroup3Id.ToString()
                });
            }
            // This method sets the header names for the asset groups in the ProjectCostingVM object.
            // It uses the userData.Site properties to determine the names for AssetGroup1, AssetGroup2, and AssetGroup3.
            // If the names are not provided in userData.Site, it falls back to default constants from AssetGroupConstants.
            GetAssetGroupHeaderName(objProjectCostingVM);

            objProjectCostingVM.projectCostingModel = pcm;
            LocalizeControls(objProjectCostingVM, LocalizeResourceSetConstants.Project);
            return View(objProjectCostingVM);

        }

        #region Project Costing search

        [HttpPost]
        public string GetProjectGridData(int? draw, int? start, int? length, int CustomQueryDisplayId = 0, string ClientlookupId = "", string Description = "",  string CreateStartDateVw = "", string CreateEndDateVw = "",
            string CompleteStartDateVw = "", string CompleteEndDateVw = "", string CloseStartDateVw = "", string CloseEndDateVw = "",
            string Status = "", string Order = "1", string txtSearchval = "", string assignedAssetGroup1 = "", string assignedAssetGroup2 = "", string assignedAssetGroup3 = "")
        {
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            int skip = (start ?? 0) / (length ?? 1) * (length ?? 0);
            ProjectCostingSearchWrapper ProjSearchWrapper = new ProjectCostingSearchWrapper(userData);
            List<ProjectCostingSearchModel> ProjList = ProjSearchWrapper.GetProjectCostingGridData(CustomQueryDisplayId, ClientlookupId, Description,  CreateStartDateVw, CreateEndDateVw, CompleteStartDateVw, CompleteEndDateVw, CloseStartDateVw, CloseEndDateVw, Status, Order, orderDir, skip, length ?? 0, txtSearchval, assignedAssetGroup1, assignedAssetGroup2, assignedAssetGroup3);

            int totalRecords = ProjList?.FirstOrDefault()?.TotalCount ?? 0;
            int recordsFiltered = totalRecords;

            return JsonConvert.SerializeObject(new { draw, recordsTotal = totalRecords, recordsFiltered, data = ProjList, hiddenColumnList = "" }, JsonSerializerDateSettings);
        }
        private void GetAssetGroupHeaderName(ProjectCostingVM projectCostingVM)
        {
            projectCostingVM.AssignedGroup1Label = String.IsNullOrEmpty(this.userData.Site.AssetGroup1Name) ? AssetGroupConstants.AssetGroup1 : this.userData.Site.AssetGroup1Name;
            projectCostingVM.AssignedGroup2Label = String.IsNullOrEmpty(this.userData.Site.AssetGroup2Name) ? AssetGroupConstants.AssetGroup2 : this.userData.Site.AssetGroup2Name;
            projectCostingVM.AssignedGroup3Label = String.IsNullOrEmpty(this.userData.Site.AssetGroup3Name) ? AssetGroupConstants.AssetGroup3 : this.userData.Site.AssetGroup3Name;
        }
        #endregion

        #region Print Project Costing
        [HttpPost]
        public JsonResult ProjectSetPrintData(ProjectCostingPrintParam projCostPrintParams)
        {
            Session["PRCOSTINTPARAMS"] = projCostPrintParams;
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [EncryptedActionParameter]
        public ActionResult ProjExportASPDF(string d = "")
        {
            ProjectCostingSearchWrapper projectCostingSearchWrapper = new ProjectCostingSearchWrapper(userData);
            ProjectCostingPDFPrintModel objProjectCostingPDFPrintModel;

            ProjectCostingVM objProjectCostingVM = new ProjectCostingVM();
            List<ProjectCostingPDFPrintModel> projectCostingPDFPrintModelList = new List<ProjectCostingPDFPrintModel>();
            var locker = new object();

            ProjectCostingPrintParam pRPrintParams = (ProjectCostingPrintParam)Session["PRCOSTINTPARAMS"];


            List<ProjectCostingSearchModel> ProjList = projectCostingSearchWrapper.GetProjectCostingGridData(pRPrintParams.CustomQueryDisplayId,  "", "",
                pRPrintParams.CreateStartDateVw, pRPrintParams.CreateEndDateVw, pRPrintParams.CompleteStartDateVw, pRPrintParams.CompleteEndDateVw, pRPrintParams.CloseStartDateVw, pRPrintParams.CloseEndDateVw,
                "", pRPrintParams.colname, pRPrintParams.coldir, 0, 100000, "");

            foreach (var p in ProjList)
            {
                objProjectCostingPDFPrintModel = new ProjectCostingPDFPrintModel();
                objProjectCostingPDFPrintModel.ProjectId = p.ProjectId;
                objProjectCostingPDFPrintModel.ClientlookupId = p.ClientlookupId;
                objProjectCostingPDFPrintModel.Description = p.Description;
                if (p.ActualStart != null && p.ActualStart != default(DateTime))
                {
                    objProjectCostingPDFPrintModel.StartDateString = p.ActualStart.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                }
                else
                {
                    objProjectCostingPDFPrintModel.StartDateString = "";
                }
                if (p.ActualFinish != null && p.ActualFinish != default(DateTime))
                {
                    objProjectCostingPDFPrintModel.EndDateString = p.ActualFinish.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                }
                else
                {
                    objProjectCostingPDFPrintModel.EndDateString = "";
                }

                objProjectCostingPDFPrintModel.Status = p.Status;
                if (p.Created != null && p.Created != default(DateTime))
                {
                    objProjectCostingPDFPrintModel.CreatedDateString = p.Created.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                }
                else
                {
                    objProjectCostingPDFPrintModel.CreatedDateString = "";
                }
                if (p.CompleteDate != null && p.CompleteDate != default(DateTime))
                {
                    objProjectCostingPDFPrintModel.CompletedDateString = p.CompleteDate.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                }
                else
                {
                    objProjectCostingPDFPrintModel.CompletedDateString = "";
                }
                objProjectCostingPDFPrintModel.Budget = p.Budget;
                objProjectCostingPDFPrintModel.AG1ClientLookupId = p.AG1ClientLookupId;
                objProjectCostingPDFPrintModel.AG2ClientLookupId = p.AG2ClientLookupId;
                objProjectCostingPDFPrintModel.AG3ClientLookupId = p.AG3ClientLookupId;
                objProjectCostingPDFPrintModel.Coordinator = p.Coordinator;
                projectCostingPDFPrintModelList.Add(objProjectCostingPDFPrintModel);
            }

            objProjectCostingVM.projectCostingPDFPrintModel = projectCostingPDFPrintModelList;
            objProjectCostingVM.tableHaederProps = pRPrintParams.tableHaederProps;
            LocalizeControls(objProjectCostingVM, LocalizeResourceSetConstants.Project);
            if (d == "excel")
            {
                return GenerateExcelReportPROJ(objProjectCostingVM, "Project Costing List");
            }
            if (d == "csv")
            {
                return GenerateCsvReportPROJ(objProjectCostingVM, "Project Costing List");
            }
            if (d == "d")
            {
                return new PartialViewAsPdf("~/Views/ProjectCosting/ProjectCostingSearch/ProjGridPdfPrintTemplate.cshtml", objProjectCostingVM)
                {
                    PageSize = Rotativa.Options.Size.A4,
                    FileName = "Project Costing List.pdf",
                    PageMargins = { Left = 3, Right = 3, Top = 3, Bottom = 3 },
                };
            }
            else
            {
                return new ViewAsPdf("~/Views/ProjectCosting/ProjectCostingSearch/ProjGridPdfPrintTemplate.cshtml", objProjectCostingVM)
                {
                    PageSize = Rotativa.Options.Size.A4,
                    PageMargins = { Left = 3, Right = 3, Top = 3, Bottom = 3 },
                };
            }

        }
        #endregion
        #region ExportToExcel Functionality for Project Costing
        //This code used to handle export functionality
        public ActionResult GenerateExcelReportPROJ(ProjectCostingVM projectVM, string reportName)
        {
            MemoryStream memoryStream = new MemoryStream();
            using (SpreadsheetDocument package = SpreadsheetDocument.Create(memoryStream, SpreadsheetDocumentType.Workbook))
            {

                CreatePartsForExcelOrCsvWOP(package, projectVM);
                package.Close();
                memoryStream.Seek(0, SeekOrigin.Begin);
                return File(memoryStream.ToArray(),
                  "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
               string.Concat(reportName, ".xlsx"));
            }
        }
        public ActionResult GenerateCsvReportPROJ(ProjectCostingVM projectVM, string reportName)
        {
            MemoryStream memoryStream = new MemoryStream();
            using (SpreadsheetDocument package = SpreadsheetDocument.Create(memoryStream, SpreadsheetDocumentType.Workbook))
            {

                CreatePartsForExcelOrCsvWOP(package, projectVM);
                package.Close();
                memoryStream.Seek(0, SeekOrigin.Begin);
                return File(memoryStream.ToArray(),
                  ".csv",
               string.Concat(reportName, ".csv"));
            }
        }
        //This method creating different parts of excel sheet
        private void CreatePartsForExcelOrCsvWOP(SpreadsheetDocument document, ProjectCostingVM data)
        {
            int length = 0;
            List<int> groupRowIndexes = new List<int>();
            int TotalHeaderColumns = 7;
            SheetData partSheetData = GenerateSheetdataForDetailsWOP(data, ref groupRowIndexes, ref TotalHeaderColumns, ref length);
            WorkbookPart workbookPart1 = document.AddWorkbookPart();
            GenerateWorkbookPartContent(workbookPart1);
            bool hasBigRows = true;
            WorkbookStylesPart workbookStylesPart1 = workbookPart1.AddNewPart<WorkbookStylesPart>("rId3");
            GenerateWorkbookStylesPartContent(workbookStylesPart1, hasBigRows);

            WorksheetPart worksheetPart1 = workbookPart1.AddNewPart<WorksheetPart>("rId1");
            GenerateWorksheetPartContent(worksheetPart1, partSheetData, groupRowIndexes, TotalHeaderColumns, hasBigRows);

        }
        private SheetData GenerateSheetdataForDetailsWOP(ProjectCostingVM projectVM, ref List<int> groupRowIndexes, ref int TotalHeaderColumns, ref int length)
        {
            //creating content of excel sheet logic is same as genere pdf
            SheetData sheetData1 = new SheetData();
            List<string> Parentcells = new List<string>();
            List<string> Childcells = new List<string>();
            List<string> Allcells = new List<string>();
            sheetData1.Append(CreateHeaderRowPROJ(projectVM));

            List<string> headercells = new List<string>();

            foreach (var item in projectVM.projectCostingPDFPrintModel)
            {
                //create header row
                foreach (var hed in projectVM.tableHaederProps)
                {
                    if (!string.IsNullOrWhiteSpace(hed.property))
                    {
                        headercells.Add(item.GetType().GetProperty(hed.property).GetValue(item, null).ToString());
                    }
                }
                sheetData1.Append(CreateRowData(headercells, TotalHeaderColumns, 1U));
                headercells.Clear();
            }
            return sheetData1;
        }

        //create header row parent grid
        private Row CreateHeaderRowPROJ(ProjectCostingVM data)
        {
            var headrs = new List<string>();
            headrs = (from d in data.tableHaederProps.ToList().Where(x => x.title != null && x.title.Length > 0).ToList()
                      select d.title).ToList();
            return CreateRowData(headrs, 7, 2U);
        }
        #endregion

        #region Project Costing Details
        public PartialViewResult Details(long ProjectId, string ClientLookupId = "0")
        {
            ProjectCostingVM objVM = new ProjectCostingVM();
            DataContracts.Project project = new DataContracts.Project();
            ProjectUDF projectUDF = new ProjectUDF();
            ProjectCostingDetailsHeaderModel headerModal = new ProjectCostingDetailsHeaderModel();
            ProjectCostingDetailsWrapper wrapper = new ProjectCostingDetailsWrapper(userData);
            objVM.security = this.userData.Security;
            objVM._userdata = this.userData;

            Task[] tasks = new Task[4];
            tasks[0] = Task.Factory.StartNew(() => headerModal = wrapper.GetProjectByProjectIdForProjectDetailsHeader(ProjectId));
            tasks[1] = Task.Factory.StartNew(() => project = wrapper.RetrieveProjectByProjectId(ProjectId));
            tasks[2] = Task.Factory.StartNew(() => projectUDF = wrapper.RetrieveProjectUDFByProjectId(ProjectId));
            tasks[3] = Task.Factory.StartNew(() => objVM.UIConfigurationDetails = new RetrieveDataForUIConfiguration().Retrieve(DataDictionaryViewNameConstant.ViewProjectWidget, userData));
            Task.WaitAll(tasks);
            objVM.projectHeaderSummaryModel = headerModal;
            objVM.ViewProjectCosting = new Models.ProjectCosting.UIConfiguration.ViewProjectCostingModelDynamic();
            objVM.ViewProjectCosting = wrapper.MapProjectDataForView(objVM.ViewProjectCosting, project);
            objVM.ViewProjectCosting = wrapper.MapProjectUDFDataForView(objVM.ViewProjectCosting, projectUDF);
            GetAssetGroupHeaderName(objVM);
            LocalizeControls(objVM, LocalizeResourceSetConstants.Project);
            return PartialView("~/Views/ProjectCosting/ProjectCostingDetails/Project/_ProjectCostingDetails.cshtml", objVM);
        }


        #region Update Project Status
        public JsonResult UpdatingProjectStatus(long ProjectId, string Status)
        {
            var errorList = new List<string>();
            if (ProjectId > 0)
            {
                ProjectCostingDetailsWrapper projectCostingDetailsWrapper = new ProjectCostingDetailsWrapper(userData);
                errorList = projectCostingDetailsWrapper.ProjectCostingStatusUpdating(ProjectId, Status);
                if (errorList != null && errorList.Count > 0)
                {
                    var returnOjb = new { success = false, errorList = errorList };
                    return Json(returnOjb, JsonRequestBehavior.AllowGet);
                }
                else
                {

                    var returnOjb = new { success = true };
                    return Json(returnOjb, JsonRequestBehavior.AllowGet);
                }

            }
            else
            {
                var returnOjb = new { success = false, errorList = errorList };
                return Json(returnOjb, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Activity Event Log

        [HttpPost]
        public PartialViewResult LoadActivity(long ProjectId)
        {
            ProjectCostingVM objProjectVM = new ProjectCostingVM();
            ProjectCostingDetailsWrapper projectDetailsWrapper = new ProjectCostingDetailsWrapper(userData);
            List<EventLogModel> EventLogList = new List<EventLogModel>();
            EventLogList = projectDetailsWrapper.PopulateEventLog(ProjectId);
            objProjectVM.EventLogList = EventLogList;
            LocalizeControls(objProjectVM, LocalizeResourceSetConstants.Project);
            return PartialView("~/Views/ProjectCosting/ProjectCostingDetails/Project/_ActivityList.cshtml", objProjectVM);
        }
        #endregion

        #region Comments Event Log
        [HttpPost]
        public PartialViewResult LoadComments(long ProjectId)
        {
            ProjectCostingVM objProjectVM = new ProjectCostingVM();
            CommonWrapper coWrapper = new CommonWrapper(userData);
            List<Personnel> personnelsList = new List<Personnel>();
            List<Notes> NotesList = new List<Notes>();
            UserMentionData userMentionData;
            List<UserMentionData> userMentionDatas = new List<UserMentionData>();

            Task[] tasks = new Task[2];
            tasks[0] = Task.Factory.StartNew(() => personnelsList = coWrapper.MentionList(""));
            tasks[1] = Task.Factory.StartNew(() => NotesList = coWrapper.PopulateComment(ProjectId, "Project"));
            Task.WaitAll(tasks);

            if (!tasks[0].IsFaulted && tasks[0].IsCompleted)
            {
                foreach (var myuseritem in personnelsList)
                {
                    userMentionData = new UserMentionData();
                    userMentionData.id = myuseritem.UserName;
                    userMentionData.name = myuseritem.FullName;
                    userMentionData.type = myuseritem.PersonnelInitial;
                    userMentionDatas.Add(userMentionData);
                }
                objProjectVM.userMentionData = userMentionDatas;
            }
            if (!tasks[1].IsFaulted && tasks[1].IsCompleted)
            {
                objProjectVM.NotesList = NotesList;
            }
            LocalizeControls(objProjectVM, LocalizeResourceSetConstants.Project);
            return PartialView("~/Views/ProjectCosting/ProjectCostingDetails/Project/_CommentsList.cshtml", objProjectVM);
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult AddComments(long ProjectId, string content, string ClientLookupId, List<string> userList, long noteId = 0, long updatedindex = 0)
        {

            CommonWrapper coWrapper = new CommonWrapper(userData);
            var namelist = coWrapper.MentionList("");
            List<UserMentionData> userMentionDataList = new List<UserMentionData>();
            UserMentionData objUserMentionData;
            if (userList != null && userList.Count > 0)
            {
                foreach (var item in userList)
                {
                    objUserMentionData = new UserMentionData();//new UserMentionData();
                    objUserMentionData.userId = namelist.Where(x => x.UserName == item).Select(y => y.PersonnelId).FirstOrDefault();
                    objUserMentionData.userName = item;
                    objUserMentionData.emailId = namelist.Where(x => x.UserName == item).Select(y => y.Email).FirstOrDefault();
                    userMentionDataList.Add(objUserMentionData);
                }
            }
            NotesModel notesModel = new NotesModel();
            notesModel.ObjectId = ProjectId;
            notesModel.Content = content;
            notesModel.NotesId = noteId;
            notesModel.ClientLookupId = ClientLookupId;
            notesModel.updatedindex = updatedindex;
            if (ModelState.IsValid)
            {
                string Mode = string.Empty;
                List<String> errorList = new List<string>();
                errorList = coWrapper.AddOrUpdateComment(notesModel, ref Mode, "Project", userMentionDataList);
                if (errorList != null && errorList.Count > 0)
                {
                    return Json(errorList, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), ProjectId = ProjectId, mode = Mode }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                string ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #endregion

        #region Add
        public ActionResult AddProjectCosting(long ProjectId)
        {
            ProjectCostingVM objVM = new ProjectCostingVM();
            ProjectCostingDetailsWrapper wrapper = new ProjectCostingDetailsWrapper(userData);
            objVM.AddProject = new Models.ProjectCosting.UIConfiguration.AddProjectCostingModelDynamic();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            objVM.security = userData.Security;
            List<DataContracts.LookupList> AllLookUps = new List<DataContracts.LookupList>();
            AllLookUps = commonWrapper.GetAllLookUpList();
            objVM.UIConfigurationDetails = new RetrieveDataForUIConfiguration()
                                         .Retrieve(DataDictionaryViewNameConstant.AddProject, userData);
            IList<string> LookupNames = objVM.UIConfigurationDetails.ToList()
                                            .Where(x => x.LookupType == "LookupList" && !string.IsNullOrEmpty(x.LookupName))
                                            .Select(s => s.LookupName)
                                            .ToList();
            var ProjectCategoryList = UtilityFunction.ProjectCategoryList();
            if (ProjectCategoryList != null)
            {
                objVM.ProjectCategoryList = ProjectCategoryList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
            }
            if (AllLookUps != null)
            {
                objVM.AllRequiredLookUplist = AllLookUps.Where(x => LookupNames.Contains(x.ListName))
                                                          .Select(s => new UILookupList
                                                          { text = s.ListValue + " - " + s.Description, value = s.ListValue, lookupname = s.ListName })
                .ToList();

            }
            var plist = commonWrapper.PersonnelListForActiveFullUser();
            objVM.OwnerPersonnelList = plist.Select(x => new SelectListItem { Text = x.NameFirst + " " + x.NameLast, Value = x.PersonnelId.ToString() }).ToList();
            objVM.CoordinatorPersonnelList = plist.Select(x => new SelectListItem { Text = x.NameFirst + " " + x.NameLast, Value = x.PersonnelId.ToString() }).ToList();
            #region AssignedGroup
            Task AssetGroup1LookUp, AssetGroup2LookUp, AssetGroup3LookUp;
            List<AssetGroup1Model> astGroup1 = new List<AssetGroup1Model>();
            AssetGroup1LookUp = Task.Factory.StartNew(() => astGroup1 = wrapper.GetAssetGroup1Dropdowndata());

            List<AssetGroup2Model> astGroup2 = new List<AssetGroup2Model>();
            AssetGroup2LookUp = Task.Factory.StartNew(() => astGroup2 = wrapper.GetAssetGroup2Dropdowndata());

            List<AssetGroup3Model> astGroup3 = new List<AssetGroup3Model>();
            AssetGroup3LookUp = Task.Factory.StartNew(() => astGroup3 = wrapper.GetAssetGroup3Dropdowndata());

            Task.WaitAll(AssetGroup1LookUp, AssetGroup2LookUp, AssetGroup3LookUp);

            if (astGroup1 != null)
            {
                objVM.AssignedGroup1List = astGroup1.Select(x => new SelectListItem { Text = x.AssetGroup1DescWithClientLookupId, Value = x.AssetGroup1Id.ToString() });
            }
            if (astGroup2 != null)
            {
                objVM.AssignedGroup2List = astGroup2.Select(x => new SelectListItem { Text = x.AssetGroup2DescWithClientLookupId, Value = x.AssetGroup2Id.ToString() });
            }
            if (astGroup3 != null)
            {
                objVM.AssignedGroup3List = astGroup3.Select(x => new SelectListItem { Text = x.AssetGroup3DescWithClientLookupId, Value = x.AssetGroup3Id.ToString() });
            }
            GetAssetGroupHeaderName(objVM);
            #endregion
            LocalizeControls(objVM, LocalizeResourceSetConstants.Project);
            return PartialView("~/Views/ProjectCosting/ProjectCostingDetails/Project/_ProjectCostingAdd.cshtml", objVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddProjectCosting(ProjectCostingVM _project)
        {
            if (ModelState.IsValid)
            {
                ProjectCostingDetailsWrapper wrapper = new ProjectCostingDetailsWrapper(userData);
                DataContracts.Project objProject = wrapper.AddProjectCostingDynamic(_project);
                if (objProject.ErrorMessages != null && objProject.ErrorMessages.Count > 0)
                {
                    return Json(objProject.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), projectId = objProject.ProjectId, clientLookupId = objProject.ClientLookupId }, JsonRequestBehavior.AllowGet);
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

        #region WorkOrder Tab
        [HttpPost]
        public PartialViewResult ProjectCostingWorkOrder(long ProjectId)
        {
            var objProjectCostingVM = new ProjectCostingVM();
            LocalizeControls(objProjectCostingVM, LocalizeResourceSetConstants.Global);
            return PartialView("~/Views/ProjectCosting/ProjectCostingDetails/WorkOrders/_ProjectCostingWorkOrderTab.cshtml", objProjectCostingVM);
        }

        #region  WorkOrder  chunk lookup list
        [HttpPost]
        public JsonResult GetProjectCostingWorkOrderSearch(int? draw, int? start, int? length, long ProjectId, string clientLookupId = "", string Description = "", string Status = "", string Planner = "", DateTime? CompleteDate = null, decimal MaterialCost = 0, decimal LaborCost = 0, decimal TotalCost = 0)
        {
            var projectCostingDetailsWrapper = new ProjectCostingDetailsWrapper(userData);
            var order = Request.Form.GetValues("order[0][column]")[0];
            var orderDir = Request.Form.GetValues("order[0][dir]")[0];

            start = start.HasValue ? start / length : 0;
            var skip = start * length ?? 0;

            var _CompleteDate = CompleteDate.HasValue ? CompleteDate.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : string.Empty;
            clientLookupId = clientLookupId?.Trim() ?? string.Empty;
            Description = Description?.Trim() ?? string.Empty;
            Status = Status?.Trim() ?? string.Empty;
            Planner = Planner?.Trim() ?? string.Empty;

            var modelList = projectCostingDetailsWrapper.GetProjectCostingWorkOrderSearchGridData(order, orderDir, skip, length.Value, ProjectId, clientLookupId, Description, Status, Planner, _CompleteDate, MaterialCost, LaborCost, TotalCost);

            var totalRecords = modelList?.FirstOrDefault()?.TotalCount ?? 0;
            var recordsFiltered = totalRecords;

            var jsonResult = Json(new { draw, recordsTotal = totalRecords, recordsFiltered, data = modelList }, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        #endregion
        #endregion

        #region Purchasing Tab
        [HttpPost]
        public PartialViewResult ProjectCostingPurchasing(long ProjectId)
        {
            var objProjectCostingVM = new ProjectCostingVM();
            LocalizeControls(objProjectCostingVM, LocalizeResourceSetConstants.Global);
            return PartialView("~/Views/ProjectCosting/ProjectCostingDetails/Purchasing/_ProjectCostingPurchasingTab.cshtml", objProjectCostingVM);
        }

        #region  Purchasing chunk lookup list
        [HttpPost]
        public JsonResult GetProjectCostingPurchesingSearch(int? draw, int? start, int? length, long ProjectId, string clientLookupId = "", int Line = 0, string PartID = "", string Description = "", decimal Quantity = 0, decimal UnitCost = 0, decimal TotalCost = 0, string Status = "", string Buyer = "", DateTime? CompleteDate = null)
        {
            var projectCostingDetailsWrapper = new ProjectCostingDetailsWrapper(userData);
            var order = Request.Form.GetValues("order[0][column]")[0];
            var orderDir = Request.Form.GetValues("order[0][dir]")[0];

            start = start.HasValue ? start / length : 0;
            var skip = start * length ?? 0;

            var _CompleteDate = CompleteDate.HasValue ? CompleteDate.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : string.Empty;
            clientLookupId = clientLookupId?.Trim() ?? string.Empty;
            Description = Description?.Trim() ?? string.Empty;
            Status = Status?.Trim() ?? string.Empty;
            Buyer = Buyer?.Trim() ?? string.Empty;

            var modelList = projectCostingDetailsWrapper.GetProjectCostingPurchasingSearchGridData(order, orderDir, skip, length.Value, ProjectId, clientLookupId, Line, PartID, Description, Quantity, UnitCost, TotalCost, Status, Buyer, _CompleteDate);

            var totalRecords = modelList?.FirstOrDefault()?.TotalCount ?? 0;
            var recordsFiltered = totalRecords;

            var jsonResult = Json(new { draw, recordsTotal = totalRecords, recordsFiltered, data = modelList }, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        #endregion
        #endregion

        #region Dashboard Tab
        public PartialViewResult ProjectCostingDashboard(long ProjectId)
        {
            ProjectCostingVM objProjectCostingVM = new ProjectCostingVM()
            {

            };

            LocalizeControls(objProjectCostingVM, LocalizeResourceSetConstants.Project);
            return PartialView("~/Views/ProjectCosting/ProjectCostingDetails/Dashboard/_DashboardDetails.cshtml", objProjectCostingVM);
        }

        [HttpPost]
        public ActionResult ProjectCostingWorkOrderStatuses(long ProjectId)
        {
            List<KeyValuePair<string, long>> workOrderStatusCount = new List<KeyValuePair<string, long>>();
            List<KeyValuePair<string, long>> woStatusChartCount = new List<KeyValuePair<string, long>>();
            ProjectTaskStatus projectTaskStatus;
            List<ProjectTaskStatus> _chart = new List<ProjectTaskStatus>();
            DashboardDetailsWrapper projectTaskWrapper = new DashboardDetailsWrapper(userData);
            workOrderStatusCount = projectTaskWrapper.WorkOrderStatusesCountForDashboard_V2(ProjectId);

            #region ScheduleComplience Chart
            woStatusChartCount = workOrderStatusCount.Take(2).ToList();
            long Total = woStatusChartCount.Sum(x => x.Value);
            foreach (var ent in woStatusChartCount)
            {
                projectTaskStatus = new ProjectTaskStatus();

                if (ent.Key.ToLower() == ProjectTaskConstants.Complete.ToLower())
                {
                    projectTaskStatus.label = UtilityFunction.GetMessageFromResource("GlobalComplete", LocalizeResourceSetConstants.Global);
                }
                else if (ent.Key.ToLower() == ProjectTaskConstants.Incomplete.ToLower())
                {
                    projectTaskStatus.label = UtilityFunction.GetMessageFromResource("GlobalIncomplete", LocalizeResourceSetConstants.Global);
                }
                if (Total > 0)
                {
                    projectTaskStatus.value = Convert.ToDecimal(Math.Round((float)ent.Value / Total * 100, 2));
                }
                else
                {
                    projectTaskStatus.value = ent.Value;
                }
                _chart.Add(projectTaskStatus);
            }
            #endregion

            return Json(new { StatusCount = workOrderStatusCount, ChartData = _chart });
        }



        [HttpPost]
        public ActionResult ProjectCostingDashboardGrid(long ProjectId)
        {
            DashboardGridModel dashboardDetails = new DashboardGridModel();
            DashboardSpendingModel dashboardSpendingModel = new DashboardSpendingModel();
            DashboardDetailsWrapper dashboardWrapper = new DashboardDetailsWrapper(userData);
            dashboardDetails = dashboardWrapper.RetrieveProjectByProjectIdForWorkOrderCostDetails(ProjectId);

            Dictionary<string, decimal?> detailsGridDictionary = dashboardWrapper.GetProjectCostingDashboardGridAsDictionary(dashboardDetails);

            #region spendingChart
            List<ProjectTaskStatus> _chart = new List<ProjectTaskStatus>();
            ProjectTaskStatus projectTaskStatus;
            dashboardSpendingModel.Spent = dashboardDetails.SpentPercentage;
            dashboardSpendingModel.Remaining = dashboardDetails.RemainingPercentage;
            Dictionary<string, decimal> detailsSpendingDictionary = dashboardWrapper.GetProjectCostingSpendingAsDictionary(dashboardSpendingModel);

            decimal Total = detailsSpendingDictionary.Sum(x => x.Value);

            foreach (var ent in detailsSpendingDictionary)
            {
                projectTaskStatus = new ProjectTaskStatus();

                if (ent.Key.ToLower() == GlobalConstants.Spent.ToLower())
                {
                    projectTaskStatus.label = GlobalConstants.Spent;
                }
                else if (ent.Key.ToLower() == GlobalConstants.Remaining.ToLower())
                {
                    projectTaskStatus.label = GlobalConstants.Remaining;
                }
                if (Total > 0)
                {
                    projectTaskStatus.value = (Math.Round(ent.Value / Total * 100, 2));
                }
                else
                {
                    projectTaskStatus.value = ent.Value;
                }
                _chart.Add(projectTaskStatus);
            }
            #endregion
            return Json(new { gridData = detailsGridDictionary.ToList(), spendingChartData = _chart });
        }
        #endregion

        #region Edit
        public ActionResult EditProjectCosting(long ProjectId)
        {
            ProjectCostingVM objVM = new ProjectCostingVM();
            ProjectCostingDetailsWrapper wrapper = new ProjectCostingDetailsWrapper(userData);
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            objVM.security = userData.Security;
            objVM.EditProject = wrapper.RetrieveProjectCostingDetailsByProjectId(ProjectId);
            List<DataContracts.LookupList> AllLookUps = new List<DataContracts.LookupList>();
            AllLookUps = commonWrapper.GetAllLookUpList();
            objVM.UIConfigurationDetails = new RetrieveDataForUIConfiguration()
                                         .Retrieve(DataDictionaryViewNameConstant.EditProject, userData);
            IList<string> LookupNames = objVM.UIConfigurationDetails.ToList()
                                            .Where(x => x.LookupType == "LookupList" && !string.IsNullOrEmpty(x.LookupName))
                                            .Select(s => s.LookupName)
                                            .ToList();
            if (AllLookUps != null)
            {
                objVM.AllRequiredLookUplist = AllLookUps.Where(x => LookupNames.Contains(x.ListName))
                                                          .Select(s => new UILookupList
                                                          { text = s.ListValue + " - " + s.Description, value = s.ListValue, lookupname = s.ListName })
                                                          .ToList();

            }
            var plist = commonWrapper.PersonnelListForActiveFullUser();
            objVM.OwnerPersonnelList = plist.Select(x => new SelectListItem { Text = x.NameFirst + " " + x.NameLast, Value = x.PersonnelId.ToString() }).ToList();
            objVM.CoordinatorPersonnelList = plist.Select(x => new SelectListItem { Text = x.NameFirst + " " + x.NameLast, Value = x.PersonnelId.ToString() }).ToList();
            #region AssignedGroup
            Task AssetGroup1LookUp, AssetGroup2LookUp, AssetGroup3LookUp;
            List<AssetGroup1Model> astGroup1 = new List<AssetGroup1Model>();
            AssetGroup1LookUp = Task.Factory.StartNew(() => astGroup1 = wrapper.GetAssetGroup1Dropdowndata());

            List<AssetGroup2Model> astGroup2 = new List<AssetGroup2Model>();
            AssetGroup2LookUp = Task.Factory.StartNew(() => astGroup2 = wrapper.GetAssetGroup2Dropdowndata());

            List<AssetGroup3Model> astGroup3 = new List<AssetGroup3Model>();
            AssetGroup3LookUp = Task.Factory.StartNew(() => astGroup3 = wrapper.GetAssetGroup3Dropdowndata());

            Task.WaitAll(AssetGroup1LookUp, AssetGroup2LookUp, AssetGroup3LookUp);

            if (astGroup1 != null)
            {
                objVM.AssignedGroup1List = astGroup1.Select(x => new SelectListItem { Text = x.AssetGroup1DescWithClientLookupId, Value = x.AssetGroup1Id.ToString() });
            }
            if (astGroup2 != null)
            {
                objVM.AssignedGroup2List = astGroup2.Select(x => new SelectListItem { Text = x.AssetGroup2DescWithClientLookupId, Value = x.AssetGroup2Id.ToString() });
            }
            if (astGroup3 != null)
            {
                objVM.AssignedGroup3List = astGroup3.Select(x => new SelectListItem { Text = x.AssetGroup3DescWithClientLookupId, Value = x.AssetGroup3Id.ToString() });
            }
            GetAssetGroupHeaderName(objVM);
            #endregion
            LocalizeControls(objVM, LocalizeResourceSetConstants.Project);
            return PartialView("~/Views/ProjectCosting/ProjectCostingDetails/Project/_ProjectCostingEdit.cshtml", objVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditProjectCosting(ProjectCostingVM _project)
        {
            if (ModelState.IsValid)
            {
                ProjectCostingDetailsWrapper wrapper = new ProjectCostingDetailsWrapper(userData);
                DataContracts.Project objProject = wrapper.EditProjectCostingDynamic(_project);
                if (objProject.ErrorMessages != null && objProject.ErrorMessages.Count > 0)
                {
                    return Json(objProject.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), projectId = objProject.ProjectId, clientLookupId = objProject.ClientLookupId }, JsonRequestBehavior.AllowGet);
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
    }
}