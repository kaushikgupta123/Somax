using Client.BusinessWrapper;
using Client.BusinessWrapper.Common;
using Client.ActionFilters;
using Client.BusinessWrapper.Configuration.ApprovalGroups;
using Client.Common;
using Client.Controllers.Common;
using Client.Models;
using Client.Models.Configuration.ApprovalGroups;
using Client.Models.Equipment;
using Common.Constants;
using DataContracts;
using Newtonsoft.Json;
using Rotativa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;


namespace Client.Controllers.Configuration.ApprovalGroups
{
    public class ApprovalGroupsController : ConfigBaseController
    {
        //Implementation of V2-720 specs ApprovalGroups

        #region Search page
        [CheckUserSecurity(securityType = SecurityConstants.ApprovalGroupsConfiguration)]
        public ActionResult Index()
        {
            ApprovalGroupsVM approvalGroupsVM = new ApprovalGroupsVM();
            approvalGroupsVM.security = userData.Security;
            ApprovalGroupsModel approvalGroupsModel = new ApprovalGroupsModel();
            GetAssetGroupHeaderName(approvalGroupsModel);
            BusinessWrapper.EquipmentWrapper eWrapper = new BusinessWrapper.EquipmentWrapper(userData);
            //getting dropdown values of  AssetGroup1 ,AssetGroup2,AssetGroup3
            var ast1 = eWrapper.GetAssetGroup1Dropdowndata();
            if (ast1 != null)
            {
                approvalGroupsModel.AssetGroup1List = ast1.Select(x => new SelectListItem { Text = x.AssetGroup1DescWithClientLookupId, Value = x.AssetGroup1Id.ToString() });
            }
            var ast2 = eWrapper.GetAssetGroup2Dropdowndata();
            if (ast2 != null)
            {
                approvalGroupsModel.AssetGroup2List = ast2.Select(x => new SelectListItem { Text = x.AssetGroup2DescWithClientLookupId, Value = x.AssetGroup2Id.ToString() });
            }
            var ast3 = eWrapper.GetAssetGroup3Dropdowndata();
            if (ast3 != null)
            {
                approvalGroupsModel.AssetGroup3List = ast3.Select(x => new SelectListItem { Text = x.AssetGroup3DescWithClientLookupId, Value = x.AssetGroup3Id.ToString() });
            }
            // RequestType dropdown 
            List<DropDownModel> RequestTypeList = new List<DropDownModel>();
            RequestTypeList = UtilityFunction.RequestTypeList();
            if (RequestTypeList != null)
            {
                approvalGroupsModel.RequestTypeList = RequestTypeList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
            }
            //
            approvalGroupsVM.ApprovalGroupsModel = approvalGroupsModel;
            LocalizeControls(approvalGroupsVM, LocalizeResourceSetConstants.ApprovalGroups);
            return View("~/Views/Configuration/ApprovalGroups/index.cshtml", approvalGroupsVM);
        }
        public string GetGridDataforApprovalGroup(int? draw, int? start, int? length = 0, string Order = "1", string RequestType = "", string Description = "", string ApprovalGroupId = "", string AssetGroup1Id = "", string AssetGroup2Id = "", string AssetGroup3Id = "", string txtSearchval = "")
        {
            //string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            start = start.HasValue
                ? start / length
                : 0;
            int skip = start * length ?? 0;
            //var colname = Request.Form.GetValues("columns[" + order + "][name]");
            List<string> typeList = new List<string>();
            ApprovalGroupsWrapper approvalGroupsWrapper = new ApprovalGroupsWrapper(userData);
            List<ApprovalGroupsModel> cList = approvalGroupsWrapper.getApprovalGroupsearchData(Order, length ?? 0, orderDir, skip, RequestType, Description, ApprovalGroupId, (String.IsNullOrEmpty(AssetGroup1Id) ? 0 : Convert.ToInt32(AssetGroup1Id)), (String.IsNullOrEmpty(AssetGroup2Id) ? 0 : Convert.ToInt32(AssetGroup2Id)), (String.IsNullOrEmpty(AssetGroup3Id) ? 0 : Convert.ToInt32(AssetGroup3Id)), txtSearchval);
            var totalRecords = 0;
            var recordsFiltered = 0;

            recordsFiltered = cList.Select(x => x.TotalCount).FirstOrDefault();
            totalRecords = cList.Select(x => x.TotalCount).FirstOrDefault();


            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = cList });
        }
        private void GetAssetGroupHeaderName(ApprovalGroupsModel approvalGroupsModel)
        {
            approvalGroupsModel.AssetGroup1ClientLookupId = String.IsNullOrEmpty(this.userData.Site.AssetGroup1Name) ? UtilityFunction.GetMessageFromResource("spnAssetGroup1", LocalizeResourceSetConstants.ApprovalGroups) : this.userData.Site.AssetGroup1Name;
            approvalGroupsModel.AssetGroup2ClientLookupId = String.IsNullOrEmpty(this.userData.Site.AssetGroup2Name) ? UtilityFunction.GetMessageFromResource("spnAssetGroup2", LocalizeResourceSetConstants.ApprovalGroups) : this.userData.Site.AssetGroup2Name;
            approvalGroupsModel.AssetGroup3ClientLookupId = String.IsNullOrEmpty(this.userData.Site.AssetGroup3Name) ? UtilityFunction.GetMessageFromResource("spnAssetGroup3", LocalizeResourceSetConstants.ApprovalGroups) : this.userData.Site.AssetGroup3Name;

        }
        public ActionResult GetApprovalGroupsInnerGrid(long ApprovalGroupId)
        {
            ApprovalGroupsVM approvalGroupsVM = new ApprovalGroupsVM();
            ApprovalGroupsWrapper pWrapper = new ApprovalGroupsWrapper(userData);
            approvalGroupsVM.listLineItem = pWrapper.PopulateLineitems(ApprovalGroupId);
            LocalizeControls(approvalGroupsVM, LocalizeResourceSetConstants.Global);
            return View("~/Views/Configuration/ApprovalGroups/_InnerGridApprovalGroupLineItem.cshtml", approvalGroupsVM);
        }
        [HttpGet]
        public string GetApprovalGroupsPrintData(string colname, string coldir, string RequestType = "", string Description = "", string ApprovalGroupId = "", string AssetGroup1Id = "", string AssetGroup2Id = "", string AssetGroup3Id = "", string txtSearchval = "")
        {
            List<ApprovalGroupsPrintModel> mrSearchModelList = new List<ApprovalGroupsPrintModel>();
            ApprovalGroupsPrintModel objApprovalGroupsPrintModel;
            ApprovalGroupsWrapper approvalGroupsWrapper = new ApprovalGroupsWrapper(userData);
            List<ApprovalGroupsModel> AGList = approvalGroupsWrapper.getApprovalGroupsearchData(colname, 100000, coldir, 0, RequestType, Description, ApprovalGroupId, (String.IsNullOrEmpty(AssetGroup1Id) ? 0 : Convert.ToInt32(AssetGroup1Id)), (String.IsNullOrEmpty(AssetGroup2Id) ? 0 : Convert.ToInt32(AssetGroup2Id)), (String.IsNullOrEmpty(AssetGroup3Id) ? 0 : Convert.ToInt32(AssetGroup3Id)), txtSearchval);
            foreach (var p in AGList)
            {
                objApprovalGroupsPrintModel = new ApprovalGroupsPrintModel();
                objApprovalGroupsPrintModel.ApprovalGroupId = p.ApprovalGroupId;
                objApprovalGroupsPrintModel.RequestType = p.RequestType;
                objApprovalGroupsPrintModel.Description = p.Description;
                objApprovalGroupsPrintModel.AssetGroup1ClientLookupId = p.AssetGroup1ClientLookupId;
                objApprovalGroupsPrintModel.AssetGroup2ClientLookupId = p.AssetGroup2ClientLookupId;
                objApprovalGroupsPrintModel.AssetGroup3ClientLookupId = p.AssetGroup3ClientLookupId;

                mrSearchModelList.Add(objApprovalGroupsPrintModel);
            }
            return JsonConvert.SerializeObject(new { data = mrSearchModelList }, JsonSerializerDateSettings);
        }
        [HttpPost]
        public JsonResult SetPrintData(ApprovalGroupsPrintParams agPrintParams)
        {
            Session["PRINTPARAMS"] = agPrintParams;
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        [EncryptedActionParameter]
        public ActionResult ExportASPDF(string d = "")
        {
            ApprovalGroupsWrapper approvalGroupsWrapper = new ApprovalGroupsWrapper(userData);
            ApprovalGroupsPDFPrintModel objAGPrintModel;
            ApprovalGroupsVM objAGVM = new ApprovalGroupsVM();
            List<ApprovalGroupsPDFPrintModel> AGPrintModelList = new List<ApprovalGroupsPDFPrintModel>();
            var locker = new object();

            ApprovalGroupsPrintParams AGPrintParams = (ApprovalGroupsPrintParams)Session["PRINTPARAMS"];

            List<ApprovalGroupsModel> AGList = approvalGroupsWrapper.getApprovalGroupsearchData(AGPrintParams.colname, 100000, AGPrintParams.coldir, 0, AGPrintParams.RequestType, AGPrintParams.Description, AGPrintParams.ApprovalGroupId, AGPrintParams.AssetGroup1Id, Convert.ToInt64(AGPrintParams.AssetGroup2Id), Convert.ToInt64(AGPrintParams.AssetGroup3Id), AGPrintParams.SearchText);

            foreach (var p in AGList)
            {
                objAGPrintModel = new ApprovalGroupsPDFPrintModel();
                objAGPrintModel.ApprovalGroupId = p.ApprovalGroupId;
                objAGPrintModel.Description = p.Description;
                objAGPrintModel.RequestType = p.RequestType;
                objAGPrintModel.AssetGroup1ClientLookupId = p.AssetGroup1ClientLookupId;
                objAGPrintModel.AssetGroup2ClientLookupId = p.AssetGroup2ClientLookupId;
                objAGPrintModel.AssetGroup3ClientLookupId = p.AssetGroup3ClientLookupId;
                if (p.ChildCount > 0)
                {
                    objAGPrintModel.LineItemModelList = approvalGroupsWrapper.PopulateLineitems(p.ApprovalGroupId);
                }
                lock (locker)
                {
                    AGPrintModelList.Add(objAGPrintModel);
                }
            }
            objAGVM.approvalGroupsPDFPrintModelList = AGPrintModelList;
            objAGVM.tableHaederProps = AGPrintParams.tableHaederProps;
            LocalizeControls(objAGVM, LocalizeResourceSetConstants.Global);
            if (d == "PDF")
            {
                return new PartialViewAsPdf("~/Views/Configuration/ApprovalGroups/ApprovalGroupsGridPdfPrintTemplate.cshtml", objAGVM)
                {
                    PageSize = Rotativa.Options.Size.A4,
                    FileName = "ApprovalGroups.pdf",
                    PageMargins = { Left = 3, Right = 3, Top = 3, Bottom = 3 },
                };
            }
            else
            {
                return new ViewAsPdf("~/Views/Configuration/ApprovalGroups/ApprovalGroupsGridPdfPrintTemplate.cshtml", objAGVM)
                {
                    PageSize = Rotativa.Options.Size.A4,
                    PageMargins = { Left = 3, Right = 3, Top = 3, Bottom = 3 },
                };
            }

        }
        #endregion

        #region Details page
        public PartialViewResult ApprovalGroupDetails(long ApprovalGroupId)
        {
            ApprovalGroupsVM approvalGroupsVM = new ApprovalGroupsVM();
            ApprovalGroupsModel approvalGroupsModel = new ApprovalGroupsModel();
            ApprovalGroupMasterModel approvalGroupsMasterModel = new ApprovalGroupMasterModel();
            ApprovalGroupsWrapper approvalGroupsWrapper = new ApprovalGroupsWrapper(userData);
            approvalGroupsVM.security = userData.Security;
            GetAssetGroupHeaderName(approvalGroupsModel);
            approvalGroupsMasterModel.AssetGroup1ClientLookupId = approvalGroupsModel.AssetGroup1ClientLookupId;
            approvalGroupsMasterModel.AssetGroup2ClientLookupId = approvalGroupsModel.AssetGroup2ClientLookupId;
            approvalGroupsMasterModel.AssetGroup3ClientLookupId = approvalGroupsModel.AssetGroup3ClientLookupId;

            approvalGroupsModel = approvalGroupsWrapper.ApprovalGroupsDetails(ApprovalGroupId);
            approvalGroupsVM.ApprovalGroupsModel = approvalGroupsModel;
            approvalGroupsVM.ApprovalGroupMasterModel = approvalGroupsMasterModel;

            LocalizeControls(approvalGroupsVM, LocalizeResourceSetConstants.ApprovalGroups);
            return PartialView("~/Views/Configuration/ApprovalGroups/_ApprovalGroupsDetails.cshtml", approvalGroupsVM);
        }
        public string AppGroupApproverForDetailsPage(int? draw, int? start, int? length = 0, long ApprovalGroupId = 0)
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            start = start.HasValue
                ? start / length
                : 0;
            int skip = start * length ?? 0;
            var colname = Request.Form.GetValues("columns[" + order + "][name]");

            ApprovalGroupsWrapper approvalGroupsWrapper = new ApprovalGroupsWrapper(userData);
            List<AppGroupApproverModel> cList = approvalGroupsWrapper.AppGroupApproverDetailsChunkSearch(order, length ?? 0, orderDir, skip, ApprovalGroupId);
            var totalRecords = 0;
            var recordsFiltered = 0;

            recordsFiltered = cList.Select(x => x.TotalCount).FirstOrDefault();
            totalRecords = cList.Select(x => x.TotalCount).FirstOrDefault();

            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = cList });
        }

        #region Add Edit delete AppGroupApprover
        [HttpPost]
        public PartialViewResult AddEditAppGroupApprovalView(long ApprovalGroupId, string RequestType, long AppGroupApproversId)
        {
            var wrapper = new ApprovalGroupsWrapper(userData);
            var viewModel = new ApprovalGroupsVM();
            var appGroupApproverModel = new AppGroupApproverModel();
            string ViewPath = "";
            ViewBag.RequestTypes = RequestType;

            if (AppGroupApproversId > 0) //for edit
            {
                appGroupApproverModel = wrapper.RetrieveAppGroupApprovers(AppGroupApproversId);
                ViewPath = "~/Views/Configuration/ApprovalGroups/_EditAppGroupApprover.cshtml";
            }
            else //for add
            {
                appGroupApproverModel.ApprovalGroupId = ApprovalGroupId;
                appGroupApproverModel.AppGroupApproversId = AppGroupApproversId;

                var AppGroupApproversList = wrapper.PopulateLineitems(ApprovalGroupId); //sweta
                var MaxLevel = AppGroupApproversList.Count > 0 ? AppGroupApproversList.Max(x => x.Level) : 0;

                viewModel.LevelCount = AppGroupApproversList.Count;
                viewModel.LevelList = BindApproverLevel(MaxLevel);
                viewModel.ApproverPersonnelList = BindApproverPersonnel(RequestType);
                ViewPath = "~/Views/Configuration/ApprovalGroups/_AddAppGroupApprover.cshtml";
            }
            viewModel.appGroupApproverModel = appGroupApproverModel;

            LocalizeControls(viewModel, LocalizeResourceSetConstants.ApprovalGroups);
            return PartialView(ViewPath, viewModel);
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public JsonResult AddEditAppGroupApproval(ApprovalGroupsVM model)
        {
            string ModelValidationFailedMessage = string.Empty;
            if (ModelState.IsValid)
            {
                AppGroupApprovers appGroupApprovers = new AppGroupApprovers();
                ApprovalGroupsWrapper wrapper = new ApprovalGroupsWrapper(userData);
                string Mode = string.Empty;
                if (model.appGroupApproverModel.AppGroupApproversId == 0)
                {
                    Mode = "add";
                    appGroupApprovers = wrapper.AddAppGroupApprovers(model.appGroupApproverModel);
                }
                else
                {
                    Mode = "edit";
                    appGroupApprovers = wrapper.EditAppGroupApprovers(model.appGroupApproverModel);
                }
                if (appGroupApprovers.ErrorMessages != null && appGroupApprovers.ErrorMessages.Count > 0)
                {
                    return Json(appGroupApprovers.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), mode = Mode }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }
        private List<SelectListItem> BindApproverPersonnel(string RequestType)
        {
            var personnel = new List<SelectListItem>();
            var securityName = "";
            var ItemAccess = "ItemAccess";
            if (RequestType == ApprovalGroupRequestTypes.MaterialRequest)
            {
                securityName = SecurityConstants.MaterialRequest_Approve;
            }
            else if (RequestType == ApprovalGroupRequestTypes.PurchaseRequest)
            {
                securityName = SecurityConstants.PurchaseRequest_Approve;
            }
            else if (RequestType == ApprovalGroupRequestTypes.SanitationRequest)
            {
                securityName = SecurityConstants.SanitationJob_ApprovalWorkbench;
            }
            else if (RequestType == ApprovalGroupRequestTypes.WorkRequest)
            {
                securityName = SecurityConstants.WorkOrder_Approve;
            }
            var dataModels = Get_PersonnelList(securityName, ItemAccess);
            personnel = dataModels.Select(x => new SelectListItem
            {
                Text = x.NameFirst + " " + x.NameLast,
                Value = x.AssignedTo_PersonnelId.ToString()
            }).ToList();
            return personnel;
        }
        private List<SelectListItem> BindApproverLevel(int maxLevel)
        {
            var level = new List<SelectListItem>();
            maxLevel++;
            for (int i = 1; i <= maxLevel; i++)
            {
                level.Add(new SelectListItem { Text = i.ToString(), Value = i.ToString() });
            }
            return level;
        }
        [HttpPost]
        public JsonResult DeleteAppGroupApproval(long ApprovalGroupId, long AppGroupApproversId)
        {
            var appGroupApprovers = new AppGroupApprovers();
            var wrapper = new ApprovalGroupsWrapper(userData);
            if (AppGroupApproversId > 0)
            {
                appGroupApprovers = wrapper.DeleteAppGroupApprovers(ApprovalGroupId, AppGroupApproversId);
            }
            if (appGroupApprovers.ErrorMessages != null && appGroupApprovers.ErrorMessages.Count > 0)
            {
                return Json(appGroupApprovers.ErrorMessages, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Result = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
            }

        }
        #endregion

        #endregion

        #region Add/Edit
        [HttpGet]
        public PartialViewResult AddEditApprovalGroups(int ApprovalGroupId = 0, string Requesttype = "", string Description = "", int AssetGroup1 = 0, int AssetGroup2 = 0, int AssetGroup3 = 0)
        {
            ApprovalGroupsVM objApprovalGroupsVM = new ApprovalGroupsVM();
            objApprovalGroupsVM.ApprovalGroupMasterModel = new ApprovalGroupMasterModel();
            ApprovalGroupsWrapper agWrapper = new ApprovalGroupsWrapper(userData);
            CommonWrapper objCommonWrapper = new CommonWrapper(userData);
            ApprovalGroupsModel approvalGroupsModel = new ApprovalGroupsModel();
            GetAssetGroupHeaderName(approvalGroupsModel);
            objApprovalGroupsVM.ApprovalGroupsModel = approvalGroupsModel;
            if (ApprovalGroupId != 0)
            {
                objApprovalGroupsVM.ApprovalGroupMasterModel.ApprovalGroupId = ApprovalGroupId;
                objApprovalGroupsVM.ApprovalGroupMasterModel.RequestType = Requesttype;
                objApprovalGroupsVM.ApprovalGroupMasterModel.Description = Description;
                objApprovalGroupsVM.ApprovalGroupMasterModel.AssetGroup1 = AssetGroup1;
                objApprovalGroupsVM.ApprovalGroupMasterModel.AssetGroup2 = AssetGroup2;
                objApprovalGroupsVM.ApprovalGroupMasterModel.AssetGroup3 = AssetGroup3;
            }
            #region Get AssetGroup1,AssetGroup2,AssetGroup3 dropdown value
            List<DataModel> AcclookUpList = new List<DataModel>();
            EquipmentWrapper eWrapper = new EquipmentWrapper(userData);
            Task taskAssetGroup1LookUp;
            Task taskAssetGroup2LookUp;
            Task taskAssetGroup3LookUp;
            List<AssetGroup1Model> astGroup1 = new List<AssetGroup1Model>();
            taskAssetGroup1LookUp = Task.Factory.StartNew(() => astGroup1 = eWrapper.GetAssetGroup1Dropdowndata());

            List<AssetGroup2Model> astGroup2 = new List<AssetGroup2Model>();
            taskAssetGroup2LookUp = Task.Factory.StartNew(() => astGroup2 = eWrapper.GetAssetGroup2Dropdowndata());

            List<AssetGroup3Model> astGroup3 = new List<AssetGroup3Model>();
            taskAssetGroup3LookUp = Task.Factory.StartNew(() => astGroup3 = eWrapper.GetAssetGroup3Dropdowndata());

            Task.WaitAll(taskAssetGroup1LookUp, taskAssetGroup2LookUp, taskAssetGroup3LookUp);
            if (astGroup1 != null)
            {
                objApprovalGroupsVM.ApprovalGroupMasterModel.AssetGroup1List = astGroup1.Select(x => new SelectListItem { Text = x.AssetGroup1DescWithClientLookupId, Value = x.AssetGroup1Id.ToString() });
            }
            if (astGroup2 != null)
            {
                objApprovalGroupsVM.ApprovalGroupMasterModel.AssetGroup2List = astGroup2.Select(x => new SelectListItem { Text = x.AssetGroup2DescWithClientLookupId, Value = x.AssetGroup2Id.ToString() });
            }
            if (astGroup3 != null)
            {
                objApprovalGroupsVM.ApprovalGroupMasterModel.AssetGroup3List = astGroup3.Select(x => new SelectListItem { Text = x.AssetGroup3DescWithClientLookupId, Value = x.AssetGroup3Id.ToString() });
            }
            #endregion

            List<DropDownModel> RequestTypeList = new List<DropDownModel>();
            RequestTypeList = UtilityFunction.RequestTypeList();
            if (RequestTypeList != null)
            {
                objApprovalGroupsVM.ApprovalGroupMasterModel.RequestTypeList = RequestTypeList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
            }
            objApprovalGroupsVM.security = userData.Security;
            LocalizeControls(objApprovalGroupsVM, LocalizeResourceSetConstants.ApprovalGroups);
            return PartialView("~/Views/Configuration/ApprovalGroups/_AddEditApprovalGroup.cshtml", objApprovalGroupsVM);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditApprovalGroups(ApprovalGroupsVM approvalGroupsVM, string Command)
        {
            string ModelValidationFailedMessage = string.Empty;
            if (ModelState.IsValid)
            {
                ApprovalGroup approvalGroup = new ApprovalGroup();
                ApprovalGroupMasterModel approvalGroupMasterModel = new ApprovalGroupMasterModel();
                ApprovalGroupsWrapper agWrapper = new ApprovalGroupsWrapper(userData);
                string Mode = string.Empty;
                long agMasterId = 0;
                approvalGroup = agWrapper.AddOrEditApprovalGroupMaster(approvalGroupsVM.ApprovalGroupMasterModel, ref Mode, ref agMasterId);
                if (approvalGroup.ErrorMessages != null && approvalGroup.ErrorMessages.Count > 0)
                {
                    return Json(approvalGroup.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), ApprovalGroupMasterId = agMasterId, Command = Command, mode = Mode }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region V2-720 Requestors Grid
        public string AppGroupRequestors(int? draw, int? start, int? length = 0, long ApprovalGroupId = 0)
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            start = start.HasValue
                ? start / length
                : 0;
            int skip = start * length ?? 0;
            var colname = Request.Form.GetValues("columns[" + order + "][name]");

            ApprovalGroupsWrapper approvalGroupsWrapper = new ApprovalGroupsWrapper(userData);
            List<AppGroupRequestorsModel> cList = approvalGroupsWrapper.AppGroupRequestorsChunkSearch(order, length ?? 0, orderDir, skip, ApprovalGroupId);
            var totalRecords = 0;
            var recordsFiltered = 0;

            recordsFiltered = cList.Select(x => x.TotalCount).FirstOrDefault();
            totalRecords = cList.Select(x => x.TotalCount).FirstOrDefault();

            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = cList });
        }

        [HttpPost]
        public JsonResult AppGroupDeleteRequestor(long AppGroupRequestorsId)
        {
            var aapGroupRequestor = new AppGroupRequestors();
            var wrapper = new ApprovalGroupsWrapper(userData);
            if (AppGroupRequestorsId > 0)
            {
                aapGroupRequestor = wrapper.DeleteRequestor(AppGroupRequestorsId);
            }
            if (aapGroupRequestor.ErrorMessages != null && aapGroupRequestor.ErrorMessages.Count > 0)
            {
                return Json(aapGroupRequestor.ErrorMessages, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Result = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
            }

        }

        #region Add Requestor      
        public string AddrequestorListchunksearch_AutoAddRequestorGeneration(int? draw, int? start, int? length, string clientLookupId = "",
            string name = "", string AssetGroup1 = "", string AssetGroup2 = "", string AssetGroup3 = "", string requestType = "", long ApprovalGroupId = 0)
        {
            List<PersonnelApprovalModel> modelList = new List<PersonnelApprovalModel>();
            ApprovalGroupsWrapper approvalGroupsWrapper = new ApprovalGroupsWrapper(userData);
            string order = Convert.ToInt32(Request.Form.GetValues("order[0][column]")[0]).ToString();
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];

            start = start.HasValue
              ? start / length
              : 0;
            int skip = start * length ?? 0;

            name = !string.IsNullOrEmpty(name) ? name.Trim() : string.Empty;
            modelList = approvalGroupsWrapper.PopulatePersonnelLookupListForApprovalGroup(order, orderDir, skip, length.Value, clientLookupId,
                name, AssetGroup1, AssetGroup2, AssetGroup3, requestType, ApprovalGroupId);

            long totalRecords = 0;
            long recordsFiltered = 0;
            if (modelList != null && modelList.Count > 0)
            {
                totalRecords = modelList[0].TotalCount;
                recordsFiltered = modelList[0].TotalCount;
            }

            return JsonConvert.SerializeObject(new
            {
                draw = draw,
                recordsTotal = totalRecords,
                recordsFiltered = recordsFiltered,
                data = modelList
            });
        }

        #region Process requestors to add using Check Box
        public JsonResult ProcessRequestorsToAddItem(string[] SelectPersonalIDsItem, string approvalGroupId)
        {
            var errorList = new List<string>();
            if (SelectPersonalIDsItem.Length > 0 && !string.IsNullOrEmpty(approvalGroupId))
            {
                long approval_GroupId = string.IsNullOrEmpty(approvalGroupId) ? 0 : Convert.ToInt64(approvalGroupId);
                ApprovalGroupsWrapper approvalGroupsWrapper = new ApprovalGroupsWrapper(userData);
                errorList = approvalGroupsWrapper.AddRequestorsToApproval(SelectPersonalIDsItem, approval_GroupId);
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
        #endregion end Process Button

        #region Process requestors to add using Select All

        public JsonResult ProcessRequestorsToAddItemsSelectAll(string colname, string coldir, string clientLookupId = "",
            string name = "", string AssetGroup1 = "", string AssetGroup2 = "", string AssetGroup3 = "", string requestType = "", long ApprovalGroupId = 0)
        {
            List<PersonnelApprovalModel> modelList = new List<PersonnelApprovalModel>();
            ApprovalGroupsWrapper approvalGroupsWrapper = new ApprovalGroupsWrapper(userData);
            name = !string.IsNullOrEmpty(name) ? name.Trim() : string.Empty;
            modelList = approvalGroupsWrapper.PopulatePersonnelLookupListForApprovalGroup(colname, coldir, 0, 100000, clientLookupId,
                name, AssetGroup1, AssetGroup2, AssetGroup3, requestType, ApprovalGroupId);

            var jsonResult = Json(modelList, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        #endregion end Select All

        #endregion end Add Requestor

        #endregion
    }
}