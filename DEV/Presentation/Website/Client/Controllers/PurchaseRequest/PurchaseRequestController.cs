using Client.ActionFilters;
using Client.BusinessWrapper;
using Client.BusinessWrapper.Common;
using Client.Common;
using Client.Controllers.Common;
using Client.Models.Common;
using Client.Models.PartLookup;
using Client.Models.PreventiveMaintenance;
using Client.Models.PurchaseRequest;
using Client.Models.PunchoutExport;
using Common.Constants;
using DataContracts;
using Newtonsoft.Json;
using RazorEngine;
using Rotativa;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Xml.Serialization;
using Client.BusinessWrapper.Configuration.SiteSetUp;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO.Compression;
using System.Text;
using Client.Models.PunchoutModel;
using Client.BusinessWrapper.Purchase_Order;
using System.Diagnostics;
using System.Web;
using Client.Localization;
using System.Net.Http.Headers;
using Client.BusinessWrapper.Configuration.ClientSetUp;
using Database.Business;
using Client.DevExpressReport;
using SomaxMVC.BusinessWrapper;
using DevExpress.Charts.Native;

namespace Client.Controllers.PurchaseRequest
{
    public class PurchaseRequestController : SomaxBaseController
    {

        #region Search

        [CheckUserSecurity(securityType = SecurityConstants.PurchaseRequest)]
        public ActionResult Index()
        {
            PurchaseRequestVM objPurchaseRequestVM = new PurchaseRequestVM();
            PurchaseRequestModel objPurchaseRequestModel = new PurchaseRequestModel();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            PurchaseRequestWrapper pWrapper = new PurchaseRequestWrapper(userData);
            var ispurchaserequest = pWrapper.RetrieveApprovalGroupRequestStatus("PurchaseRequests");
            objPurchaseRequestVM.IsPurchaseRequestApproval = ispurchaserequest;
            objPurchaseRequestVM.udata = userData;
            string mode = Convert.ToString(TempData["Mode"]);
            // V2-851 
            //if ((mode != "addPurchaseRequest") && (mode != "DetailFromApproval"))
            //{
            //    var VendorsLookUplist = GetLookupList_Vendor();
            //    if (VendorsLookUplist != null)
            //    {
            //        objPurchaseRequestModel.VendorsList = VendorsLookUplist.Select(x => new SelectListItem { Text = x.Vendor + " - " + x.Name, Value = x.Vendor.ToString() });
            //    }
            //}
            var statuslist = commonWrapper.GetListFromConstVals("PurchaseRequestStatus");
            objPurchaseRequestModel.StatusList = statuslist.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() }).ToList();
            if (mode == "addPurchaseRequest")
            {
                objPurchaseRequestModel.ViewName = UiConfigConstants.PurchaseRequestAdd;
                objPurchaseRequestVM.purchaseRequestModel = objPurchaseRequestModel;

                #region uiconfig
                CommonWrapper cWrapper = new CommonWrapper(userData);
                var totalList = cWrapper.GetAllUiConfigList(UiConfigConstants.PurchaseRequestAdd);
                var hidList = totalList.Where(x => x.Hide == true);
                objPurchaseRequestVM.hiddenColumnList = new List<string>();
                foreach (var item in hidList)
                {
                    objPurchaseRequestVM.hiddenColumnList.Add(item.ColumnName);
                }
                var impList = totalList.Where(x => x.Required == true && x.Hide == false);
                objPurchaseRequestVM.requiredColumnList = new List<string>();
                foreach (var item in impList)
                {
                    objPurchaseRequestVM.requiredColumnList.Add(item.ColumnName);
                }
                #endregion

                objPurchaseRequestVM.purchaseRequestModel.IsPurchaseRequestAdd = true;
            }
            else if (mode == "addPurchaseRequestDynamic")
            {
                List<DataContracts.LookupList> AllLookUps = new List<DataContracts.LookupList>();
                AllLookUps = commonWrapper.GetAllLookUpList();
                objPurchaseRequestVM.UIConfigurationDetails = new RetrieveDataForUIConfiguration()
                                             .Retrieve(DataDictionaryViewNameConstant.AddPurchaseRequest, userData);
                IList<string> LookupNames = objPurchaseRequestVM.UIConfigurationDetails.ToList()
                                                .Where(x => x.LookupType == "LookupList" && !string.IsNullOrEmpty(x.LookupName))
                                                .Select(s => s.LookupName)
                                                .ToList();
                if (AllLookUps != null)
                {
                    objPurchaseRequestVM.AllRequiredLookUplist = AllLookUps.Where(x => LookupNames.Contains(x.ListName))
                                                              .Select(s => new Models.UILookupList
                                                              { text = s.ListValue + " - " + s.Description, value = s.ListValue, lookupname = s.ListName })
                                                              .ToList();
                }
                //V2-738
                if (userData.DatabaseKey.Client.UseMultiStoreroom)
                {
                    objPurchaseRequestVM.StoreroomList = commonWrapper.GetStoreroomList(StoreroomAuthType.Issue);
                }
                objPurchaseRequestVM.purchaseRequestModel = objPurchaseRequestModel;
                objPurchaseRequestVM.AddPurchaseRequest = new Models.PurchaseRequest.UIConfiguration.AddPurchaseRequestModelDynamic();
                objPurchaseRequestVM.purchaseRequestModel.IsPurchaseRequestAddDynamic = true;
            }
            else if (mode == "DetailFromApprovalDynamic" || mode == "DetailFromShoppingCart")
            {
                PurchaseRequestEmailModel prEmailModel = new PurchaseRequestEmailModel();
                long PurchaseRequestId = Convert.ToInt32(TempData["PurchaseRequestId"]);
                objPurchaseRequestModel = pWrapper.GetPurchaseRequestDetailById(PurchaseRequestId);
                if (objPurchaseRequestModel != null)
                {
                    objPurchaseRequestModel.CurrentDate = System.DateTime.UtcNow;
                    objPurchaseRequestVM.purchaseRequestModel = objPurchaseRequestModel;
                }


                PRHeaderUDF pRHeaderUDF = new PRHeaderUDF();
                objPurchaseRequestVM.ViewPurchaseRequest = new Models.PurchaseRequest.UIConfiguration.ViewPurchaseRequestModelDynamic();
                //Task attTask;
                objPurchaseRequestModel = pWrapper.GetPurchaseRequestDetailById(PurchaseRequestId);
                var purchaseRequestDetailsDynamic = pWrapper.GetPurchaseRequestDetailsByIdDynamic(PurchaseRequestId);
                Task[] tasks = new Task[3];
                CommonWrapper objCommonWrapper = new CommonWrapper(userData);
                tasks[0] = Task.Factory.StartNew(() => objPurchaseRequestVM.attachmentCount = objCommonWrapper.AttachmentCount(PurchaseRequestId, AttachmentTableConstant.PurchaseRequest, userData.Security.PurchaseRequest.Edit));
                tasks[1] = Task.Factory.StartNew(() => pRHeaderUDF = pWrapper.RetrievePRUDFByPurchaseRequestId(PurchaseRequestId));
                tasks[2] = Task.Factory.StartNew(() => objPurchaseRequestVM.UIConfigurationDetails = new RetrieveDataForUIConfiguration().Retrieve(DataDictionaryViewNameConstant.ViewPurchaseRequestWidget, userData));
                Task.WaitAll(tasks);
                List<DataContracts.LookupList> UOMs = new List<DataContracts.LookupList>();
                List<Models.EventLogModel> ListOfEvents = new List<Models.EventLogModel>();
                var AllLookUps = commonWrapper.GetAllLookUpList();
                if (AllLookUps != null)
                {
                    UOMs = AllLookUps.Where(x => x.ListName == LookupListConstants.Unit_Of_Measure).ToList();
                }
                if (UOMs != null)
                {
                    objPurchaseRequestVM.purchaseRequestModel.UOMList = UOMs.Select(x => new SelectListItem { Text = x.ListValue + " - " + x.Description, Value = x.ListValue.ToString() });
                }
                objPurchaseRequestVM.ViewPurchaseRequest = purchaseRequestDetailsDynamic;


                var PersonnelLookUplist = Get_PersonnelList(SecurityConstants.PurchaseRequest_Approve, "ItemAccess");
                if (PersonnelLookUplist != null)
                {
                    objPurchaseRequestVM.purchaseRequestModel.PersonnelList = PersonnelLookUplist.Select(x => new SelectListItem { Text = x.AssignedTo_PersonnelClientLookupId + " - " + x.NameFirst + " - " + x.NameLast, Value = x.AssignedTo_PersonnelId.ToString() });
                }
                //V2-379
                var AcclookUpList = GetAccountByActiveState(true);
                if (AcclookUpList != null)
                {
                    objPurchaseRequestVM.purchaseRequestModel.AccountList = AcclookUpList.Select(x => new SelectListItem { Text = x.Account + " - " + x.Name, Value = x.Account.ToString() });
                }
                bool lEdit = (userData.Security.PurchaseRequest.Edit && objPurchaseRequestModel.Status == PurchaseRequestStatusConstants.Open)
                         || (userData.Security.PurchaseRequest.Edit && objPurchaseRequestModel.Status == PurchaseRequestStatusConstants.Resubmit)
                         || (userData.Security.PurchaseRequest.EditAwaitApprove && objPurchaseRequestModel.Status == PurchaseRequestStatusConstants.AwaitApproval)
                         || (userData.Security.PurchaseRequest.EditApproved && objPurchaseRequestModel.Status == PurchaseRequestStatusConstants.Approved);
                ViewBag.LineItemSecurity = lEdit;
                ViewBag.IsPunchout = objPurchaseRequestModel.IsPunchOut;
                objPurchaseRequestVM.purchaseRequestModel.PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
                objPurchaseRequestVM.purchaseRequestModel.ApproveSecurity = userData.Security.PurchaseRequest.Approve;
                objPurchaseRequestVM.purchaseRequestModel.EditSecurity = userData.Security.PurchaseRequest.Edit;
                objPurchaseRequestVM.purchaseRequestModel.PrApproveSecurity = userData.Security.Purchasing.Approve;   //V2-375
                objPurchaseRequestVM.purchaseRequestModel.EditApprovedSecurity = userData.Security.PurchaseRequest.EditApproved;//V2-454
                objPurchaseRequestVM.purchaseRequestModel.CreateSecurity = userData.Security.Purchasing.Create;  //V2-375
                if ((userData.Security.PurchaseRequest.Approve) && (objPurchaseRequestModel.Status == PurchaseRequestStatusConstants.Open
                                                       || objPurchaseRequestModel.Status == PurchaseRequestStatusConstants.AwaitApproval
                                                       || objPurchaseRequestModel.Status == PurchaseRequestStatusConstants.Resubmit)
                                                       )
                {
                    objPurchaseRequestVM.purchaseRequestModel._menuAdd = true;
                }
                bool prLineAdd = (objPurchaseRequestModel.EditSecurity && (objPurchaseRequestModel.Status == PurchaseRequestStatusConstants.Open
                                 || objPurchaseRequestModel.Status == PurchaseRequestStatusConstants.Resubmit))
                              || (objPurchaseRequestModel.EditAwaitApproveSecurity && objPurchaseRequestModel.Status == PurchaseRequestStatusConstants.AwaitApproval);

                ViewBag.AddLineItemSecurity = prLineAdd;
                objPurchaseRequestVM.prEmailModel = prEmailModel;
                if (objPurchaseRequestVM.purchaseRequestModel.CcEmailId != null)
                {
                    objPurchaseRequestVM.prEmailModel.CcEmailId = objPurchaseRequestVM.purchaseRequestModel.CcEmailId;
                }
                objPurchaseRequestVM.purchaseRequestModel.IsPurchaseRequestFromApproval = true;

                ////--check PurchaseRequestExport interface 
                var isActiveInterfaceChk = commonWrapper.CheckIsActiveInterface(ApiConstants.PurchaseRequestExport);

                //V2-726 Start
                ApprovalRouteModel approvalRouteModel = new ApprovalRouteModel();
                //if (ispurchaserequest) V2-804
                //{
                approvalRouteModel = SendPRForApproval(PurchaseRequestId, purchaseRequestDetailsDynamic.BuyerReview);//V2-820
                //}
                approvalRouteModel.IsPurchaseRequest = ispurchaserequest;
                objPurchaseRequestVM.ApprovalRouteModel = approvalRouteModel;
                //V2 726 End
                #region V2-820
                ReviewPRSendForApprovalModel reviewPRSendForApprovalModel = new ReviewPRSendForApprovalModel();
                reviewPRSendForApprovalModel = ReviewAndSendPRForApproval(PurchaseRequestId);
                objPurchaseRequestVM.reviewPRSendForApprovalModel = reviewPRSendForApprovalModel;
                //V2-820
                //ApprovalRoute approvalRoute = new ApprovalRoute()
                //{
                //    ClientId = this.userData.DatabaseKey.Client.ClientId,
                //    ObjectId = PurchaseRequestId,
                //    RequestType = ApprovalGroupRequestTypes.PurchaseRequest,
                //    ApproverId = this.userData.DatabaseKey.Personnel.PersonnelId
                //};
                //List<ApprovalRoute> list = ApprovalRoute.ApprovalRoute_RetrievebyObjectId_V2(this.userData.DatabaseKey, approvalRoute);
                //if (list != null && list.Count > 0)
                //{
                //    objPurchaseRequestVM.IsValidApproverForPurchaseRequestReview = true;
                //}
                #endregion
                objPurchaseRequestVM.isActiveInterface = isActiveInterfaceChk;
            }
            else if (mode == "shoppingcartcheckout" || mode == "shoppingcartcheckouttab")
            {
                objPurchaseRequestVM.shoppingList = (List<ShoppingCartImportDataModel>)Session["SHOPPINGLIST"];
                objPurchaseRequestVM.purchaseRequestModel = objPurchaseRequestModel;
                objPurchaseRequestVM.IsPunchOutCheckOut = true;
                #region RKL Mail for close the vendor punchout website and send user back to the original tab
                if (mode == "shoppingcartcheckouttab")
                {
                    objPurchaseRequestVM.IsPunchOutCheckOutTab = true; // property is true if redirect from somax after getting information from punchout. this will be false when returnig from punchout
                }
                #endregion
                //V2-1119 Check if any interface property has Oracle Purchase Request Export in use
                bool OraclePurchaseRequestExportInUse = false;
                LoginWrapper loginWrapper = new LoginWrapper();
                var AllInterfacePropData = loginWrapper.RetrieveAllInterfacePropertiesByClientIdSiteId();
                // RKL - Lost this when coming back from punchout
                System.Web.HttpContext.Current.Session["InterfacePropData"] = AllInterfacePropData;
                if (AllInterfacePropData != null && AllInterfacePropData.Count > 0)
                {
                    OraclePurchaseRequestExportInUse = AllInterfacePropData.Where(x => x.InterfaceType == ApiConstants.OraclePurchaseRequestExport).Select(x => x.InUse).FirstOrDefault();
                }
                objPurchaseRequestVM.IsOraclePurchaseRequestExportInUse = OraclePurchaseRequestExportInUse;
            }
            else if (mode == "DetailFromPart")
            {
                objPurchaseRequestVM.purchaseRequestModel = objPurchaseRequestModel;
                long PRId = Convert.ToInt64(TempData["PurchaseRequestId"]);
                string Status = Convert.ToString(TempData["Status"]);
                objPurchaseRequestVM.purchaseRequestModel.PurchaseRequestId = PRId;
                objPurchaseRequestVM.purchaseRequestModel.IsRedirectFromPart = true;
                objPurchaseRequestVM.purchaseRequestModel.Status = Status;
                objPurchaseRequestModel.ScheduleWorkList = commonWrapper.PopulateCustomQueryDisplay(AttachmentTableConstant.PurchaseRequest);
                objPurchaseRequestVM.purchaseRequestModel.DateRangeDropList = UtilityFunction.GetTimeRangeDrop().Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
                objPurchaseRequestVM.purchaseRequestModel.DateRangeDropListForPR = UtilityFunction.GetTimeRangeDropForPR().Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
                objPurchaseRequestVM.purchaseRequestModel.DateRangeDropListForPRCancel = UtilityFunction.GetTimeRangeDropForCancelPR().Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
                objPurchaseRequestVM.purchaseRequestModel.DateRangeDropListAllStatus = UtilityFunction.GetTimeRangeDropForAllStatusPR().Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
            }
            else if (mode == "DetailFromPurchaseOrder")
            {
                objPurchaseRequestVM.purchaseRequestModel = objPurchaseRequestModel;
                long PRId = Convert.ToInt64(TempData["PurchaseRequestId"]);
                objPurchaseRequestVM.purchaseRequestModel.PurchaseRequestId = PRId;
                objPurchaseRequestVM.purchaseRequestModel.IsRedirectFromDetailPurchaseOrder = true;
                objPurchaseRequestModel.ScheduleWorkList = commonWrapper.PopulateCustomQueryDisplay(AttachmentTableConstant.PurchaseRequest);
                objPurchaseRequestVM.purchaseRequestModel.DateRangeDropList = UtilityFunction.GetTimeRangeDrop().Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
                objPurchaseRequestVM.purchaseRequestModel.DateRangeDropListForPR = UtilityFunction.GetTimeRangeDropForPR().Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
                objPurchaseRequestVM.purchaseRequestModel.DateRangeDropListForPRCancel = UtilityFunction.GetTimeRangeDropForCancelPR().Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
                objPurchaseRequestVM.purchaseRequestModel.DateRangeDropListAllStatus = UtilityFunction.GetTimeRangeDropForAllStatusPR().Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
            }
            else if (mode == "DetailFromPlusMenu")
            {
                objPurchaseRequestVM.purchaseRequestModel = objPurchaseRequestModel;
                long PRId = Convert.ToInt64(TempData["PurchaseRequestId"]);
                objPurchaseRequestVM.purchaseRequestModel.PurchaseRequestId = PRId;
                objPurchaseRequestVM.purchaseRequestModel.IsRedirectFromPlusMenu = true;

            }
            #region V2-1147
            else if (mode == "DetailFromNotification")
            {

                long PRId = Convert.ToInt64(TempData["PurchaseRequestId"]);

                long PurchaseRequestId = Convert.ToInt32(TempData["PurchaseRequestId"]);
                objPurchaseRequestModel = GetPurchaseRequestRecords(objPurchaseRequestVM, commonWrapper, pWrapper, ispurchaserequest, PRId, PurchaseRequestId);

            }
            #endregion
            else
            {
                objPurchaseRequestModel.ScheduleWorkList = commonWrapper.PopulateCustomQueryDisplay(AttachmentTableConstant.PurchaseRequest);
                objPurchaseRequestModel.DateRangeDropList = UtilityFunction.GetTimeRangeDrop().Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() }); //V2-347
                objPurchaseRequestModel.DateRangeDropListForPR = UtilityFunction.GetTimeRangeDropForPR().Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() }); //V2-347
                objPurchaseRequestModel.DateRangeDropListForPRCancel = UtilityFunction.GetTimeRangeDropForCancelPR().Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() }); //V2-523
                objPurchaseRequestModel.DateRangeDropListAllStatus = UtilityFunction.GetTimeRangeDropForAllStatusPR().Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
                objPurchaseRequestVM.purchaseRequestModel = objPurchaseRequestModel;
            }

            objPurchaseRequestVM.security = this.userData.Security;

            objPurchaseRequestVM.purchaseRequestModel.PRUsePunchOutSecurity = userData.Security.PurchaseRequest.UsePunchout;

            objPurchaseRequestVM.purchaseRequestModel.IsSitePunchOut = userData.Site.UsePunchOut;
            ////-----V2-375------
            ////--check PurchaseRequestExport interface 
            var isActiveInterface = commonWrapper.CheckIsActiveInterface(ApiConstants.PurchaseRequestExport);
            objPurchaseRequestVM.isActiveInterface = isActiveInterface;
            bool PurchaseRequestExportInUse = false;

            var InterfacePropData = (List<InterfacePropModel>)Session["InterfacePropData"];
            if (InterfacePropData != null && InterfacePropData.Count > 0)
            {
                PurchaseRequestExportInUse = InterfacePropData.Where(x => x.InterfaceType == InterfacePropConstants.PurchaseRequestExport).Select(x => x.InUse).FirstOrDefault();
            }

            objPurchaseRequestVM.ipropInUse = PurchaseRequestExportInUse;
            //-------------------
            SetIsSendToSAPFlag(ref objPurchaseRequestVM);
            LocalizeControls(objPurchaseRequestVM, LocalizeResourceSetConstants.PurchaseRequest);
            return View(objPurchaseRequestVM);
        }

        #region V2-1147
        private PurchaseRequestModel GetPurchaseRequestRecords(PurchaseRequestVM objPurchaseRequestVM, CommonWrapper commonWrapper, PurchaseRequestWrapper pWrapper, bool ispurchaserequest, long PRId, long PurchaseRequestId)
        {
            PurchaseRequestModel objPurchaseRequestModel;
            PurchaseRequestEmailModel prEmailModel = new PurchaseRequestEmailModel();
            objPurchaseRequestModel = pWrapper.GetPurchaseRequestDetailById(PurchaseRequestId);
            if (objPurchaseRequestModel != null)
            {
                objPurchaseRequestModel.CurrentDate = System.DateTime.UtcNow;
                objPurchaseRequestVM.purchaseRequestModel = objPurchaseRequestModel;
            }
            objPurchaseRequestVM.purchaseRequestModel.PurchaseRequestId = PRId;
            objPurchaseRequestVM.purchaseRequestModel.IsRedirectFromNotification = true;
            objPurchaseRequestVM.purchaseRequestModel.AlertNameRedirectFromNotification = Convert.ToString(TempData["alertName"]);

            PRHeaderUDF pRHeaderUDF = new PRHeaderUDF();
            objPurchaseRequestVM.ViewPurchaseRequest = new Models.PurchaseRequest.UIConfiguration.ViewPurchaseRequestModelDynamic();
            //Task attTask;
            objPurchaseRequestModel = pWrapper.GetPurchaseRequestDetailById(PurchaseRequestId);
            var purchaseRequestDetailsDynamic = pWrapper.GetPurchaseRequestDetailsByIdDynamic(PurchaseRequestId);
            Task[] tasks = new Task[3];
            CommonWrapper objCommonWrapper = new CommonWrapper(userData);
            tasks[0] = Task.Factory.StartNew(() => objPurchaseRequestVM.attachmentCount = objCommonWrapper.AttachmentCount(PurchaseRequestId, AttachmentTableConstant.PurchaseRequest, userData.Security.PurchaseRequest.Edit));
            tasks[1] = Task.Factory.StartNew(() => pRHeaderUDF = pWrapper.RetrievePRUDFByPurchaseRequestId(PurchaseRequestId));
            tasks[2] = Task.Factory.StartNew(() => objPurchaseRequestVM.UIConfigurationDetails = new RetrieveDataForUIConfiguration().Retrieve(DataDictionaryViewNameConstant.ViewPurchaseRequestWidget, userData));
            Task.WaitAll(tasks);
            List<DataContracts.LookupList> UOMs = new List<DataContracts.LookupList>();
            List<Models.EventLogModel> ListOfEvents = new List<Models.EventLogModel>();
            var AllLookUps = commonWrapper.GetAllLookUpList();
            if (AllLookUps != null)
            {
                UOMs = AllLookUps.Where(x => x.ListName == LookupListConstants.Unit_Of_Measure).ToList();
            }
            if (UOMs != null)
            {
                objPurchaseRequestVM.purchaseRequestModel.UOMList = UOMs.Select(x => new SelectListItem { Text = x.ListValue + " - " + x.Description, Value = x.ListValue.ToString() });
            }
            objPurchaseRequestVM.ViewPurchaseRequest = purchaseRequestDetailsDynamic;


            var PersonnelLookUplist = Get_PersonnelList(SecurityConstants.PurchaseRequest_Approve, "ItemAccess");
            if (PersonnelLookUplist != null)
            {
                objPurchaseRequestVM.purchaseRequestModel.PersonnelList = PersonnelLookUplist.Select(x => new SelectListItem { Text = x.AssignedTo_PersonnelClientLookupId + " - " + x.NameFirst + " - " + x.NameLast, Value = x.AssignedTo_PersonnelId.ToString() });
            }
            //V2-379
            var AcclookUpList = GetAccountByActiveState(true);
            if (AcclookUpList != null)
            {
                objPurchaseRequestVM.purchaseRequestModel.AccountList = AcclookUpList.Select(x => new SelectListItem { Text = x.Account + " - " + x.Name, Value = x.Account.ToString() });
            }
            bool lEdit = (userData.Security.PurchaseRequest.Edit && objPurchaseRequestModel.Status == PurchaseRequestStatusConstants.Open)
                     || (userData.Security.PurchaseRequest.Edit && objPurchaseRequestModel.Status == PurchaseRequestStatusConstants.Resubmit)
                     || (userData.Security.PurchaseRequest.EditAwaitApprove && objPurchaseRequestModel.Status == PurchaseRequestStatusConstants.AwaitApproval)
                     || (userData.Security.PurchaseRequest.EditApproved && objPurchaseRequestModel.Status == PurchaseRequestStatusConstants.Approved);
            ViewBag.LineItemSecurity = lEdit;
            ViewBag.IsPunchout = objPurchaseRequestModel.IsPunchOut;
            objPurchaseRequestVM.purchaseRequestModel.PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
            objPurchaseRequestVM.purchaseRequestModel.ApproveSecurity = userData.Security.PurchaseRequest.Approve;
            objPurchaseRequestVM.purchaseRequestModel.EditSecurity = userData.Security.PurchaseRequest.Edit;
            objPurchaseRequestVM.purchaseRequestModel.PrApproveSecurity = userData.Security.Purchasing.Approve;   //V2-375
            objPurchaseRequestVM.purchaseRequestModel.EditApprovedSecurity = userData.Security.PurchaseRequest.EditApproved;//V2-454
            objPurchaseRequestVM.purchaseRequestModel.CreateSecurity = userData.Security.Purchasing.Create;  //V2-375
            if ((userData.Security.PurchaseRequest.Approve) && (objPurchaseRequestModel.Status == PurchaseRequestStatusConstants.Open
                                                   || objPurchaseRequestModel.Status == PurchaseRequestStatusConstants.AwaitApproval
                                                   || objPurchaseRequestModel.Status == PurchaseRequestStatusConstants.Resubmit)
                                                   )
            {
                objPurchaseRequestVM.purchaseRequestModel._menuAdd = true;
            }
            bool prLineAdd = (objPurchaseRequestModel.EditSecurity && (objPurchaseRequestModel.Status == PurchaseRequestStatusConstants.Open
                             || objPurchaseRequestModel.Status == PurchaseRequestStatusConstants.Resubmit))
                          || (objPurchaseRequestModel.EditAwaitApproveSecurity && objPurchaseRequestModel.Status == PurchaseRequestStatusConstants.AwaitApproval);

            ViewBag.AddLineItemSecurity = prLineAdd;
            objPurchaseRequestVM.prEmailModel = prEmailModel;
            if (objPurchaseRequestVM.purchaseRequestModel.CcEmailId != null)
            {
                objPurchaseRequestVM.prEmailModel.CcEmailId = objPurchaseRequestVM.purchaseRequestModel.CcEmailId;
            }
            objPurchaseRequestVM.purchaseRequestModel.IsPurchaseRequestFromApproval = true;

            ////--check PurchaseRequestExport interface 
            var isActiveInterfaceChk = commonWrapper.CheckIsActiveInterface(ApiConstants.PurchaseRequestExport);

            //V2-726 Start
            ApprovalRouteModel approvalRouteModel = new ApprovalRouteModel();
            //if (ispurchaserequest) V2-804
            //{
            approvalRouteModel = SendPRForApproval(PurchaseRequestId, purchaseRequestDetailsDynamic.BuyerReview);//V2-820
                                                                                                                 //}
            approvalRouteModel.IsPurchaseRequest = ispurchaserequest;
            objPurchaseRequestVM.ApprovalRouteModel = approvalRouteModel;
            //V2 726 End
            #region V2-820
            ReviewPRSendForApprovalModel reviewPRSendForApprovalModel = new ReviewPRSendForApprovalModel();
            reviewPRSendForApprovalModel = ReviewAndSendPRForApproval(PurchaseRequestId);
            objPurchaseRequestVM.reviewPRSendForApprovalModel = reviewPRSendForApprovalModel;
            #endregion
            #region V2-1112
            var ShipTolist = GetLookupList_ShipToAddress();
            if (ShipTolist != null)
            {
                objPurchaseRequestVM.ShipToList = ShipTolist.Select(x => new SelectListItem { Text = x.ClientLookUpId, Value = x.ClientLookUpId.ToString() });
            }
            #endregion
            objPurchaseRequestVM.isActiveInterface = isActiveInterfaceChk;
            return objPurchaseRequestModel;
        }
        #endregion

        [HttpPost]
        public string GetPRGridData(int? draw, int? start, int? length, int CustomQueryDisplayId = 0, DateTime? ProcessedStartDateVw = null, DateTime? ProcessedEndDateVw = null, DateTime? CreateStartDateVw = null, DateTime? CreateEndDateVw = null, DateTime? CancelandDeniedStartDateVw = null, DateTime? CancelandDeniedEndDateVw = null, string PurchaseRequest = "", string Reason = "", string Status = "", string CreatedBy = "",
                                string Vendor = "", string VendorName = "", DateTime? CreateStartDate = null, DateTime? CreateEndDate = null, string PONumber = "", string ProcessedBy = "",
                                DateTime? ProcessedStartDate = null, DateTime? ProcessedEndDate = null, string txtSearchval = "", string Creator_PersonnelName = "", string Processed_PersonnelName = "",
                                string Order = "2"/*, string orderDir = "asc"*/)
        {
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            var filter = CustomQueryDisplayId;
            start = start.HasValue
          ? start / length
          : 0;
            int skip = start * length ?? 0;
            PurchaseRequestWrapper pWrapper = new PurchaseRequestWrapper(userData);
            List<PurchaseRequestModel> pRList = pWrapper.GetPurchaseRequestChunkList(CustomQueryDisplayId, skip, length ?? 0, Order, orderDir,
                 ProcessedStartDateVw, ProcessedEndDateVw, CreateStartDateVw, CreateEndDateVw, CancelandDeniedStartDateVw, CancelandDeniedEndDateVw, PurchaseRequest, Reason, Status, CreatedBy, Vendor, VendorName, PONumber, ProcessedBy, txtSearchval, CreateStartDate, CreateEndDate, ProcessedStartDate, ProcessedEndDate);
            var totalRecords = 0;
            var recordsFiltered = 0;
            if (pRList != null && pRList.Count > 0)
            {
                recordsFiltered = pRList[0].TotalCount;
                totalRecords = pRList[0].TotalCount;
            }
            else
            {
                recordsFiltered = 0;
                totalRecords = 0;
            }
            int initialPage = start.Value;
            var filteredResult = pRList
              .ToList();

            #region uiconfig

            CommonWrapper cWrapper = new CommonWrapper(userData);
            var hiddenList = cWrapper.GetHiddenList(UiConfigConstants.PurchaseRequestSearch).Select(x => x.ColumnName).ToList();

            #endregion

            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult, hiddenColumnList = hiddenList }, JsonSerializerDateSettings);
        }



        [HttpPost]
        public string GetPRShoppingCartGrid(int? draw, int? start, int? length)
        {

            start = start.HasValue
          ? start / length
          : 0;
            int skip = start * length ?? 0;

            List<ShoppingCartImportDataModel> shoppingList = new List<ShoppingCartImportDataModel>();
            List<Models.DropDownModel> ErrorList = new List<Models.DropDownModel>();

            if (Session["SHOPPINGLIST"] != null)
            {
                shoppingList = (List<ShoppingCartImportDataModel>)Session["SHOPPINGLIST"];
                ErrorList = ValidationCheckout(ref shoppingList);
            }
            var totalRecords = 0;
            var recordsFiltered = 0;
            if (shoppingList.Where(x => x.IsValid == 1) != null && shoppingList.Count > 0)
            {
                recordsFiltered = shoppingList.Count();
                totalRecords = shoppingList.Count();
            }
            var filteredResult = shoppingList.Where(x => x.IsValid == 1).ToList();
            // RKL - 2025-Feb-03 - Change to using populateChargeTypeForPurchaseRequestLineItem
            //                   - This excludes `account for clients using the Oracle PR Interface (BBU)
            //var ScheduleChargeTypeList = UtilityFunction.populateChargeTypeForLineItem();
            var ScheduleChargeTypeList = UtilityFunction.populateChargeTypeForPurchaseRequestLineItem();
            #region V2-1119 get and match value from the Purchase Request Shopping Cart UNIT_OF_MEASURE lookup list from the part/vendor cross-reference  
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            var AllLookUps = commonWrapper.GetAllLookUpList();
            var UnitOfMeasureList = AllLookUps.Where(x => x.ListName == LookupListConstants.Unit_Of_Measure).ToList().Select(x => new DataTableDropdownModel { label = x.ListValue + ' ' + '|' + ' ' + x.Description, value = x.ListValue.ToString() }).ToList();
            var ChargeTypeList = ScheduleChargeTypeList.Select(x => new DataTableDropdownModel { label = x.text, value = x.value }).ToList();
            if (ScheduleChargeTypeList == null && ScheduleChargeTypeList.Count == 0)
            {
                ChargeTypeList = new List<DataTableDropdownModel>();
            }
            if (UnitOfMeasureList == null && UnitOfMeasureList.Count == 0)
            {
                UnitOfMeasureList = new List<DataTableDropdownModel>();
            }
            foreach (var obj in filteredResult)
            {
                obj.ChargeTypeListdropDown = ChargeTypeList;
                obj.OrderUnitListdropDown = UnitOfMeasureList;
                if (obj.PartId == 0)
                {
                    obj.OrderUnit = UnitOfMeasureList.FirstOrDefault(m => string.Equals(m.value, obj.UnitofMeasure, StringComparison.OrdinalIgnoreCase))?.value ?? obj.UnitofMeasure;
                }
            }
            #endregion
            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult, IsUsePartMaster = this.userData.Site.UsePartMaster, ErrorList = ErrorList }, JsonSerializerDateSettings);
        }
        private List<Models.DropDownModel> ValidationCheckout(ref List<ShoppingCartImportDataModel> shoppingcart)
        {
            List<Models.DropDownModel> errorMessage = new List<Models.DropDownModel>();
            StringBuilder msg = new StringBuilder();
            foreach (var item in shoppingcart)
            {
                msg.Clear();
                if (item.PurchaseRequestId == 0)
                {
                    msg.Append("<P>" + "Purchase Request Id must be provided and be a valid purchase request" + "</P>");
                }
                if (item.ClientLookupId == "")
                {
                    msg.Append("<P>" + "Purchase Request ClientLookup Id must be provided and be a valid purchase request" + "</P>");
                }
                if (string.IsNullOrEmpty(item.Description))
                {
                    msg.Append("<P>" + "Description can't be empty" + "</P>");
                }
                if (item.UnitCost == 0)
                {
                    msg.Append("<P>" + "Unit Cost can't be 0" + "</P>");
                }
                if (item.OrderQuantity == 0)
                {
                    msg.Append("<P>" + "Quantity can't be 0" + "</P>");
                }
                if (msg.ToString() != "")
                {
                    item.IsValid = 0;
                    errorMessage.Add(new Models.DropDownModel() { text = item.SupplierPartAuxiliaryId.ToString(), value = msg.ToString() });
                }
                else
                {
                    item.IsValid = 1;
                }
            }
            return errorMessage;
        }

        [HttpGet]
        public string GetPRPrintData(string colname, string coldir, int CustomQueryDisplayId = 0, DateTime? ProcessedStartDateVw = null, DateTime? ProcessedEndDateVw = null, DateTime? CreateStartDateVw = null, DateTime? CreateEndDateVw = null, DateTime? CancelandDeniedStartDateVw = null, DateTime? CancelandDeniedEndDateVw = null, string PurchaseRequest = "", string Reason = "", string Status = "", string CreatedBy = "",
                        string Vendor = "", string VendorName = "", DateTime? CreateStartDate = null, DateTime? CreateEndDate = null, string PONumber = "", string ProcessedBy = "",
                        DateTime? ProcessedStartDate = null, DateTime? ProcessedEndDate = null, string txtSearchval = "", string Creator_PersonnelName = "", string Processed_PersonnelName = "")
        {
            List<PurchaseRequestPrintModel> pSearchModelList = new List<PurchaseRequestPrintModel>();
            PurchaseRequestPrintModel objPurchaseRequestPrintModel;
            PurchaseRequestWrapper pWrapper = new PurchaseRequestWrapper(userData);
            List<PurchaseRequestModel> pRList = pWrapper.GetPurchaseRequestChunkList(CustomQueryDisplayId, 0, 100000, colname, coldir, ProcessedStartDateVw, ProcessedEndDateVw, CreateStartDateVw, CreateEndDateVw, CancelandDeniedStartDateVw, CancelandDeniedEndDateVw, PurchaseRequest, Reason,
                Status, Creator_PersonnelName, Vendor, VendorName, PONumber, Processed_PersonnelName, txtSearchval, CreateStartDate, CreateEndDate, ProcessedStartDate, ProcessedEndDate);
            foreach (var p in pRList)
            {
                objPurchaseRequestPrintModel = new PurchaseRequestPrintModel();
                objPurchaseRequestPrintModel.ClientLookupId = p.ClientLookupId;
                objPurchaseRequestPrintModel.CreateDate = p.CreateDate;
                objPurchaseRequestPrintModel.Creator_PersonnelName = p.Creator_PersonnelName;
                objPurchaseRequestPrintModel.ProcessedDate = p.ProcessedDate;
                objPurchaseRequestPrintModel.Processed_PersonnelName = p.Processed_PersonnelName;
                objPurchaseRequestPrintModel.PurchaseOrderClientLookupId = p.PurchaseOrderClientLookupId;
                objPurchaseRequestPrintModel.Reason = p.Reason;
                objPurchaseRequestPrintModel.Status = p.Status;
                objPurchaseRequestPrintModel.VendorClientLookupId = p.VendorClientLookupId;
                objPurchaseRequestPrintModel.VendorName = p.VendorName;
                pSearchModelList.Add(objPurchaseRequestPrintModel);
            }
            return JsonConvert.SerializeObject(new { data = pSearchModelList }, JsonSerializerDateSettings);
        }



        public JsonResult GetPRSelectAllData(string colname, string coldir, int CustomQueryDisplayId = 0, DateTime? ProcessedStartDateVw = null, DateTime? ProcessedEndDateVw = null, DateTime? CreateStartDateVw = null, DateTime? CreateEndDateVw = null, DateTime? CancelandDeniedStartDateVw = null, DateTime? CancelandDeniedEndDateVw = null, string PurchaseRequest = "", string Reason = "", string Status = "", string CreatedBy = "",
                    string Vendor = "", string VendorName = "", DateTime? CreateStartDate = null, DateTime? CreateEndDate = null, string PONumber = "", string ProcessedBy = "",
                    DateTime? ProcessedStartDate = null, DateTime? ProcessedEndDate = null, string txtSearchval = "", string Creator_PersonnelName = "", string Processed_PersonnelName = "")
        {
            List<PurchaseRequestPrintModel> pSearchModelList = new List<PurchaseRequestPrintModel>();
            PurchaseRequestWrapper pWrapper = new PurchaseRequestWrapper(userData);
            List<PurchaseRequestModel> pRList = pWrapper.GetPurchaseRequestChunkList(CustomQueryDisplayId, 0, 100000, colname, coldir, ProcessedStartDateVw, ProcessedEndDateVw, CreateStartDateVw, CreateEndDateVw, CancelandDeniedStartDateVw, CancelandDeniedEndDateVw, PurchaseRequest, Reason,
                Status, Creator_PersonnelName, Vendor, VendorName, PONumber, Processed_PersonnelName, txtSearchval, CreateStartDate, CreateEndDate, ProcessedStartDate, ProcessedEndDate);

            return Json(pRList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SetPrintData(PRPrintParams pRPrintParams)
        {
            Session["PRINTPARAMS"] = pRPrintParams;
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        [EncryptedActionParameter]
        public ActionResult ExportASPDF(string d = "")
        {
            PurchaseRequestWrapper pWrapper = new PurchaseRequestWrapper(userData);
            PurchaseRequestPDFPrintModel objPurchaseRequestPrintModel;
            PurchaseRequestVM objPurchaseRequestVM = new PurchaseRequestVM();
            List<PurchaseRequestPDFPrintModel> pSearchModelList = new List<PurchaseRequestPDFPrintModel>();
            var locker = new object();

            PRPrintParams pRPrintParams = (PRPrintParams)Session["PRINTPARAMS"];

            List<PurchaseRequestModel> pRList = pWrapper.GetPurchaseRequestChunkList(pRPrintParams.CustomQueryDisplayId, 0, 100000, pRPrintParams.colname, pRPrintParams.coldir,
               pRPrintParams.ProcessedStartDateVw, pRPrintParams.ProcessedEndDateVw, pRPrintParams.CreateStartDateVw, pRPrintParams.CreateEndDateVw, pRPrintParams.CancelandDeniedStartDateVw, pRPrintParams.CancelandDeniedEndDateVw, pRPrintParams.PurchaseRequest, pRPrintParams.Reason, pRPrintParams.Status, pRPrintParams.Creator_PersonnelName, pRPrintParams.Vendor, pRPrintParams.VendorName, pRPrintParams.PONumber, pRPrintParams.Processed_PersonnelName, pRPrintParams.txtSearchval,
                pRPrintParams.CreateStartDate, pRPrintParams.CreateEndDate, pRPrintParams.ProcessedStartDate, pRPrintParams.ProcessedEndDate);

            foreach (var p in pRList)
            {
                objPurchaseRequestPrintModel = new PurchaseRequestPDFPrintModel();
                objPurchaseRequestPrintModel.ClientLookupId = p.ClientLookupId;
                if (p.CreateDate != null && p.CreateDate != default(DateTime))
                {
                    objPurchaseRequestPrintModel.CreateDateString = p.CreateDate.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                }
                else
                {
                    objPurchaseRequestPrintModel.CreateDateString = "";
                }

                objPurchaseRequestPrintModel.Creator_PersonnelName = p.Creator_PersonnelName;
                if (p.ProcessedDate != null && p.ProcessedDate != default(DateTime))
                {
                    objPurchaseRequestPrintModel.ProcessedDateString = p.ProcessedDate.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                }
                else
                {
                    objPurchaseRequestPrintModel.ProcessedDateString = "";
                }
                objPurchaseRequestPrintModel.Processed_PersonnelName = p.Processed_PersonnelName;
                objPurchaseRequestPrintModel.PurchaseOrderClientLookupId = p.PurchaseOrderClientLookupId;
                objPurchaseRequestPrintModel.Reason = p.Reason;
                objPurchaseRequestPrintModel.Status = UtilityFunction.GetMessageFromResource(p.Status, LocalizeResourceSetConstants.StatusDetails);
                objPurchaseRequestPrintModel.VendorClientLookupId = p.VendorClientLookupId;
                objPurchaseRequestPrintModel.VendorName = p.VendorName;
                if (p.ChildCount > 0)
                {
                    objPurchaseRequestPrintModel.LineItemModelList = pWrapper.PopulateLineitems(p.PurchaseRequestId);
                    objPurchaseRequestPrintModel.Total = objPurchaseRequestPrintModel.LineItemModelList.Sum(x => x.TotalCost);
                }
                lock (locker)
                {
                    pSearchModelList.Add(objPurchaseRequestPrintModel);
                }
            }
            objPurchaseRequestVM.purchaseRequestPDFPrintModel = pSearchModelList;
            objPurchaseRequestVM.tableHaederProps = pRPrintParams.tableHaederProps;
            LocalizeControls(objPurchaseRequestVM, LocalizeResourceSetConstants.PurchaseRequest);
            if (d == "d")
            {
                return new PartialViewAsPdf("PRGridPdfPrintTemplate", objPurchaseRequestVM)
                {
                    PageSize = Rotativa.Options.Size.A4,
                    FileName = "Purchase Request.pdf",
                    PageMargins = { Left = 3, Right = 3, Top = 3, Bottom = 3 },
                };
            }
            else
            {
                return new ViewAsPdf("PRGridPdfPrintTemplate", objPurchaseRequestVM)
                {
                    PageSize = Rotativa.Options.Size.A4,
                    PageMargins = { Left = 3, Right = 3, Top = 3, Bottom = 3 },
                };
            }

        }

        #endregion

        #region Add-Edit

        public RedirectResult Add()
        {
            TempData["Mode"] = "addPurchaseRequestDynamic";
            return Redirect("/PurchaseRequest/Index?page=Procurement_Requests");
        }
        public PartialViewResult AddPurchaseRequest()
        {
            PurchaseRequestVM objPurchaseRequestVM = new PurchaseRequestVM();
            PurchaseRequestModel objPurchaseRequestModel = new PurchaseRequestModel();
            PurchaseRequestWrapper pWrapper = new PurchaseRequestWrapper(userData);
            objPurchaseRequestModel.ViewName = UiConfigConstants.PurchaseRequestAdd;
            objPurchaseRequestVM.purchaseRequestModel = objPurchaseRequestModel;

            #region uiconfig
            CommonWrapper cWrapper = new CommonWrapper(userData);
            var totalList = cWrapper.GetAllUiConfigList(UiConfigConstants.PurchaseRequestAdd);
            var hidList = totalList.Where(x => x.Hide == true);
            objPurchaseRequestVM.hiddenColumnList = new List<string>();
            foreach (var item in hidList)
            {
                objPurchaseRequestVM.hiddenColumnList.Add(item.ColumnName);
            }
            var dsablList = totalList.Where(x => x.Disable == true);
            objPurchaseRequestVM.disabledColumnList = new List<string>();
            foreach (var item in dsablList)
            {
                objPurchaseRequestVM.disabledColumnList.Add(item.ColumnName);
            }
            var impList = totalList.Where(x => x.Required == true && x.Hide == false && x.Disable == false);
            objPurchaseRequestVM.requiredColumnList = new List<string>();
            foreach (var item in impList)
            {
                objPurchaseRequestVM.requiredColumnList.Add(item.ColumnName);
            }
            #endregion

            LocalizeControls(objPurchaseRequestVM, LocalizeResourceSetConstants.PurchaseRequest);
            return PartialView("_AddPurchaseRequest", objPurchaseRequestVM);
        }

        public PartialViewResult EditPurchaserequest(long PurchaseRequestId)
        {
            PurchaseRequestVM objPurchaseRequestVM = new PurchaseRequestVM();
            PurchaseRequestModel objPurchaseRequestModel = new PurchaseRequestModel();
            PurchaseRequestWrapper pWrapper = new PurchaseRequestWrapper(userData);
            objPurchaseRequestModel = pWrapper.GetPurchaseRequestDetailById(PurchaseRequestId);
            objPurchaseRequestModel.ViewName = UiConfigConstants.PurchaseRequestEdit;  //--V2-375--uiconfig//
            if (objPurchaseRequestModel != null)
            {
                objPurchaseRequestVM.purchaseRequestModel = objPurchaseRequestModel;
            }

            objPurchaseRequestVM.security = this.userData.Security;

            #region uiconfig
            CommonWrapper cWrapper = new CommonWrapper(userData);
            var totalList = cWrapper.GetAllUiConfigList(UiConfigConstants.PurchaseRequestEdit);
            var hidList = totalList.Where(x => x.Hide == true);
            objPurchaseRequestVM.hiddenColumnList = new List<string>();
            foreach (var item in hidList)
            {
                objPurchaseRequestVM.hiddenColumnList.Add(item.ColumnName);
            }
            var dsablList = totalList.Where(x => x.Disable == true);
            objPurchaseRequestVM.disabledColumnList = new List<string>();
            foreach (var item in dsablList)
            {
                objPurchaseRequestVM.disabledColumnList.Add(item.ColumnName);
            }
            var impList = totalList.Where(x => x.Required == true && x.Hide == false && x.Disable == false);
            objPurchaseRequestVM.requiredColumnList = new List<string>();
            foreach (var item in impList)
            {
                objPurchaseRequestVM.requiredColumnList.Add(item.ColumnName);
            }
            #endregion          
            LocalizeControls(objPurchaseRequestVM, LocalizeResourceSetConstants.PurchaseRequest);
            return PartialView("_AddPurchaseRequest", objPurchaseRequestVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddPurchaseRequest(PurchaseRequestVM objPurchaseRequestVM, string Command)
        {
            string ModelValidationFailedMessage = string.Empty;
            string Mode = string.Empty;

            if (ModelState.IsValid)
            {
                DataContracts.PurchaseRequest pRequest = new DataContracts.PurchaseRequest();
                PurchaseRequestModel objPurchaseRequestModel = new PurchaseRequestModel();
                PurchaseRequestWrapper pWrapper = new PurchaseRequestWrapper(userData);
                if (objPurchaseRequestVM.purchaseRequestModel.PurchaseRequestId != 0)
                {
                    pRequest = pWrapper.EditpurchaseRequest(objPurchaseRequestVM.purchaseRequestModel);
                }
                else
                {
                    Mode = "add";
                    pRequest = pWrapper.AddPurchaseRequest(objPurchaseRequestVM.purchaseRequestModel);
                }

                if (pRequest.ErrorMessages != null && pRequest.ErrorMessages.Count > 0)
                {
                    return Json(pRequest.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), Command = Command, purchaserequestid = pRequest.PurchaseRequestId, mode = Mode }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult AddPurchaseRequestPunchOut(long VendorID)
        {
            DataContracts.PurchaseRequest pRequest = new DataContracts.PurchaseRequest();
            PurchaseRequestPunchOutModel objPurchaseRequestModel = new PurchaseRequestPunchOutModel();
            PurchaseRequestWrapper pWrapper = new PurchaseRequestWrapper(userData);
            pRequest = pWrapper.AddPurchaseRequestPunchOut(VendorID);
            if (pRequest.ErrorMessages != null && pRequest.ErrorMessages.Count > 0)
            {
                return Json(pRequest.ErrorMessages, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Result = JsonReturnEnum.success.ToString(), purchaserequestid = pRequest.PurchaseRequestId }, JsonRequestBehavior.AllowGet);
            }

        }
        public JsonResult PrintPRListFromIndex(PurchaseRequestPrntModel model)
        {
            if (model.list.Count > 0)
            {
                return PrintPDFFromIndex(model);
            }
            else
            {
                var returnOjb = new { success = false };
                return Json(returnOjb, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult PrintPDFFromIndex(PurchaseRequestPrntModel model)
        {
            var ms = new MemoryStream();
            bool jsonStringExceed = false;
            HttpRuntimeSection section = ConfigurationManager.GetSection("system.web/httpRuntime") as HttpRuntimeSection;
            Int64 fileSizeCounter = 0;
            Int32 maxPdfSize = section.MaxRequestLength;
            string attachUrl = string.Empty;
            var doc = new iTextSharp.text.Document();
            var copy = new iTextSharp.text.pdf.PdfSmartCopy(doc, ms);
            doc.Open();
            foreach (var item in model.list)
            {
                CommonWrapper comWrapper = new CommonWrapper(userData);
                var msSinglePDf = new MemoryStream(PDFForMailAttachment(item.PurchaseRequestId));
                var reader = new iTextSharp.text.pdf.PdfReader(msSinglePDf);
                fileSizeCounter += reader.FileLength;
                if (fileSizeCounter < maxPdfSize)
                {
                    copy.AddDocument(reader);
                }
                else
                {
                    jsonStringExceed = true;
                    break;

                }

            }

            doc.Close();
            byte[] pdf = ms.ToArray();
            string strPdf = System.Convert.ToBase64String(pdf);
            if (jsonStringExceed)
            {
                strPdf = "";
            }
            var returnOjb = new { success = true, pdf = strPdf, jsonStringExceed = jsonStringExceed };
            var jsonResult = Json(returnOjb, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [HttpPost]
        public ActionResult UpdateEmailStatus(PurchaseRequestVM PurchaseRequestVM)
        {
            PurchaseRequestWrapper pWrapper = new PurchaseRequestWrapper(userData);
            MemoryStream stream = new MemoryStream();
            string emailHtmlBody = string.Empty;
            int PersonnelId = 0;
            var templateFilePath = System.Web.HttpContext.Current.Server.MapPath(@"\Views\Shared\CommonEmailTemplate.cshtml");
            var templateContent = string.Empty;
            using (var reader = new StreamReader(templateFilePath))
            {
                templateContent = reader.ReadToEnd();
            }
            emailHtmlBody = ParseTemplate(templateContent);
            stream = new MemoryStream(PDFForMailAttachment(PurchaseRequestVM.purchaseRequestModel.PurchaseRequestId));
            pWrapper.UpdateEmailToVendorStatus(emailHtmlBody, stream, PurchaseRequestVM.purchaseRequestModel.PurchaseRequestId, PersonnelId, PurchaseRequestVM.prEmailModel.ToEmailId, PurchaseRequestVM.prEmailModel.CcEmailId, PurchaseRequestVM.prEmailModel.MailBodyComments);
            return Json(new { Result = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
        }

        public Byte[] PDFForMailAttachment(long Requestid)
        {
            PurchaseRequestVM objPurchaseRequestVM = new PurchaseRequestVM();
            PurchaseRequestWrapper pWrapper = new PurchaseRequestWrapper(userData);
            PurchaseRequestModel objPurchaseRequestModel = new PurchaseRequestModel();
            objPurchaseRequestModel = pWrapper.RetrieveByPKForeignKeysForReport(Convert.ToInt64(Requestid));
            if (objPurchaseRequestModel != null)
            {
                objPurchaseRequestVM.purchaseRequestModel = objPurchaseRequestModel;
            }
            List<LineItemModel> lineItem = pWrapper.PopulateLineitems(Convert.ToInt64(Requestid));
            objPurchaseRequestVM.listLineItem = lineItem;
            string customSwitches = string.Format("--header-html  \"{0}\" " +
                                  "--header-spacing \"1\" " +
                                  "--header-font-size \"10\" ",
                                  Url.Action("Header", "PurchaseRequest", new { id = userData.LoginAuditing.SessionId, Requestid = Requestid }, Request.Url.Scheme)
                                  );
            LocalizeControls(objPurchaseRequestVM, LocalizeResourceSetConstants.PurchaseRequest);
            var mailpdft = new ViewAsPdf("PRTemplatePrint", objPurchaseRequestVM)
            {
                CustomSwitches = customSwitches
            };
            Byte[] PdfData = mailpdft.BuildPdf(ControllerContext);
            return PdfData;
        }
        public static string ParseTemplate(string content)
        {
            string _mode = DateTime.Now.Ticks + new Random().Next(1000, 100000).ToString();
            Razor.Compile(content, _mode);
            return Razor.Parse(content);
        }
        #endregion

        #region Details
        public PartialViewResult Details(long PurchaseRequestId)
        {
            PurchaseRequestVM objPurchaseRequestVM = new PurchaseRequestVM();
            PurchaseRequestModel objPurchaseRequestModel = new PurchaseRequestModel();
            PurchaseRequestWrapper pWrapper = new PurchaseRequestWrapper(userData);
            PurchaseRequestEmailModel prEmailModel = new PurchaseRequestEmailModel();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            PRHeaderUDF pRHeaderUDF = new PRHeaderUDF();


            objPurchaseRequestVM.ViewPurchaseRequest = new Models.PurchaseRequest.UIConfiguration.ViewPurchaseRequestModelDynamic();
            //Task attTask;
            objPurchaseRequestModel = pWrapper.GetPurchaseRequestDetailById(PurchaseRequestId);
            var purchaseRequestDetailsDynamic = pWrapper.GetPurchaseRequestDetailsByIdDynamic(PurchaseRequestId);
            Task[] tasks = new Task[3];
            CommonWrapper objCommonWrapper = new CommonWrapper(userData);
            tasks[0] = Task.Factory.StartNew(() => objPurchaseRequestVM.attachmentCount = objCommonWrapper.AttachmentCount(PurchaseRequestId, AttachmentTableConstant.PurchaseRequest, userData.Security.PurchaseRequest.Edit));
            tasks[1] = Task.Factory.StartNew(() => pRHeaderUDF = pWrapper.RetrievePRUDFByPurchaseRequestId(PurchaseRequestId));
            tasks[2] = Task.Factory.StartNew(() => objPurchaseRequestVM.UIConfigurationDetails = new RetrieveDataForUIConfiguration().Retrieve(DataDictionaryViewNameConstant.ViewPurchaseRequestWidget, userData));
            Task.WaitAll(tasks);

            List<DataContracts.LookupList> UOMs = new List<DataContracts.LookupList>();
            List<Models.EventLogModel> ListOfEvents = new List<Models.EventLogModel>();

            if (objPurchaseRequestModel != null)
            {
                objPurchaseRequestModel.CurrentDate = System.DateTime.UtcNow;
                objPurchaseRequestVM.purchaseRequestModel = objPurchaseRequestModel;
            }
            ViewBag.IsPunchout = objPurchaseRequestModel.IsPunchOut;

            #region V2-726
            //V2-726 Start
            ApprovalRouteModel approvalRouteModel = new ApprovalRouteModel();
            var ispurchaserequest = pWrapper.RetrieveApprovalGroupRequestStatus("PurchaseRequests");
            objPurchaseRequestVM.IsPurchaseRequestApproval = ispurchaserequest;
            //if (ispurchaserequest) V2-804
            //{
            approvalRouteModel = SendPRForApproval(PurchaseRequestId, purchaseRequestDetailsDynamic.BuyerReview);//V2-820
            //}
            approvalRouteModel.IsPurchaseRequest = ispurchaserequest;
            //V2 726 End
            #endregion
            #region V2-820
            //V2-820
            ReviewPRSendForApprovalModel reviewPRSendForApprovalModel = new ReviewPRSendForApprovalModel();
            reviewPRSendForApprovalModel = ReviewAndSendPRForApproval(PurchaseRequestId);
            objPurchaseRequestVM.reviewPRSendForApprovalModel = reviewPRSendForApprovalModel;
            //V2-820
            #endregion
            #region V2-730
            //V2-730 Start
            ApprovalRouteModelByObjectId approvalRouteModelByObjectId = new ApprovalRouteModelByObjectId();
            List<ApprovalRouteModelByObjectId> approvalRouteModelByObjectIdList = pWrapper.RetrieveApprovalGroupIdbyObjectId(userData.DatabaseKey.Personnel.PersonnelId, PurchaseRequestId, ApprovalGroupRequestTypes.PurchaseRequest);
            if (approvalRouteModelByObjectIdList != null && approvalRouteModelByObjectIdList.Count > 0)
            {
                objPurchaseRequestVM.IsPurchaseRequestApprovalAccessCheck = true;
                approvalRouteModelByObjectId.ApprovalGroupId = approvalRouteModelByObjectIdList[0].ApprovalGroupId;
            }
            else
            {
                objPurchaseRequestVM.IsPurchaseRequestApprovalAccessCheck = false;
                approvalRouteModelByObjectId.ApprovalGroupId = 0;
            }
            //V2-730 End
            #endregion



            var AllLookUps = commonWrapper.GetAllLookUpList();
            if (AllLookUps != null)
            {
                UOMs = AllLookUps.Where(x => x.ListName == LookupListConstants.Unit_Of_Measure).ToList();
            }
            if (UOMs != null)
            {
                objPurchaseRequestVM.purchaseRequestModel.UOMList = UOMs.Select(x => new SelectListItem { Text = x.ListValue + " - " + x.Description, Value = x.ListValue.ToString() });
            }
            // V2-478 
            // Security Items to include: Sanitation-WB
            // Security Properties      : ItemAccess
            //var PersonnelLookUplist = Get_PersonnelList();
            var PersonnelLookUplist = Get_PersonnelList(SecurityConstants.PurchaseRequest_Approve, "ItemAccess");
            if (PersonnelLookUplist != null)
            {
                objPurchaseRequestVM.purchaseRequestModel.PersonnelList = PersonnelLookUplist.Select(x => new SelectListItem { Text = x.AssignedTo_PersonnelClientLookupId + " - " + x.NameFirst + " - " + x.NameLast, Value = x.AssignedTo_PersonnelId.ToString() });
            }
            var AcclookUpList = GetAccountByActiveState(true);
            if (AcclookUpList != null)
            {
                objPurchaseRequestVM.purchaseRequestModel.AccountList = AcclookUpList.Select(x => new SelectListItem { Text = x.Account + " - " + x.Name, Value = x.Account.ToString() });
            }
            bool lEdit = (userData.Security.PurchaseRequest.Edit && objPurchaseRequestModel.Status == PurchaseRequestStatusConstants.Open)
                     || (userData.Security.PurchaseRequest.Edit && objPurchaseRequestModel.Status == PurchaseRequestStatusConstants.Resubmit)
                     || (userData.Security.PurchaseRequest.EditAwaitApprove && objPurchaseRequestModel.Status == PurchaseRequestStatusConstants.AwaitApproval)
                     || (userData.Security.PurchaseRequest.EditApproved && objPurchaseRequestModel.Status == PurchaseRequestStatusConstants.Approved);

            ViewBag.LineItemSecurity = lEdit;

            objPurchaseRequestVM.purchaseRequestModel.PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
            objPurchaseRequestVM.purchaseRequestModel.ApproveSecurity = userData.Security.PurchaseRequest.Approve;
            objPurchaseRequestVM.purchaseRequestModel.PrApproveSecurity = userData.Security.Purchasing.Approve;   //V2-375
            objPurchaseRequestVM.purchaseRequestModel.EditSecurity = userData.Security.PurchaseRequest.Edit;
            objPurchaseRequestVM.purchaseRequestModel.EditAwaitApproveSecurity = userData.Security.PurchaseRequest.EditAwaitApprove;
            objPurchaseRequestVM.purchaseRequestModel.EditApprovedSecurity = userData.Security.PurchaseRequest.EditApproved; //V2-454
            objPurchaseRequestVM.purchaseRequestModel.CreateSecurity = userData.Security.Purchasing.Create;  //V2-375

            if ((userData.Security.PurchaseRequest.Approve) && (objPurchaseRequestModel.Status == PurchaseRequestStatusConstants.Open
                                                   || objPurchaseRequestModel.Status == PurchaseRequestStatusConstants.AwaitApproval
                                                   || objPurchaseRequestModel.Status == PurchaseRequestStatusConstants.Resubmit))
            {
                objPurchaseRequestVM.purchaseRequestModel._menuAdd = true;
            }

            bool prLineAdd = (objPurchaseRequestModel.EditSecurity && (objPurchaseRequestModel.Status == PurchaseRequestStatusConstants.Open || objPurchaseRequestModel.Status == PurchaseRequestStatusConstants.Resubmit))
      || (objPurchaseRequestModel.EditAwaitApproveSecurity && objPurchaseRequestModel.Status == PurchaseRequestStatusConstants.AwaitApproval);
            ViewBag.AddLineItemSecurity = prLineAdd;
            objPurchaseRequestVM.prEmailModel = prEmailModel;
            if (objPurchaseRequestVM.purchaseRequestModel.CcEmailId != null)
            {
                objPurchaseRequestVM.prEmailModel.CcEmailId = objPurchaseRequestVM.purchaseRequestModel.CcEmailId;
            }
            if (objPurchaseRequestVM.purchaseRequestModel.ToEmailId != null)
            {
                objPurchaseRequestVM.prEmailModel.ToEmailId = objPurchaseRequestVM.purchaseRequestModel.ToEmailId;
            }
            objPurchaseRequestVM.udata = userData;
            objPurchaseRequestVM.security = this.userData.Security;
            //attTask.Wait();
            //V2-653

            objPurchaseRequestVM.ViewPurchaseRequest = purchaseRequestDetailsDynamic;
            //V2-726
            objPurchaseRequestVM.ApprovalRouteModel = approvalRouteModel;
            objPurchaseRequestVM.ApprovalRouteModelByObjectId = approvalRouteModelByObjectId;
            LocalizeControls(objPurchaseRequestVM, LocalizeResourceSetConstants.PurchaseRequest);

            ////--check PurchaseRequestExport interface 
            var isActiveInterface = commonWrapper.CheckIsActiveInterface(ApiConstants.PurchaseRequestExport);
            objPurchaseRequestVM.isActiveInterface = isActiveInterface;
            //-------------------
            objPurchaseRequestVM.purchaseRequestModel.PRUsePunchOutSecurity = userData.Security.PurchaseRequest.UsePunchout;

            objPurchaseRequestVM.purchaseRequestModel.IsSitePunchOut = userData.Site.UsePunchOut;
            objPurchaseRequestVM.purchaseRequestModel.SingleStockLineItem = userData.Site.SingleStockLineItem;
            SetIsSendToSAPFlag(ref objPurchaseRequestVM);
            #region V2-1112
            var ShipTolist = GetLookupList_ShipToAddress();
            if (ShipTolist != null)
            {
                objPurchaseRequestVM.ShipToList = ShipTolist.Select(x => new SelectListItem { Text = x.ClientLookUpId, Value = x.ClientLookUpId.ToString() });
            }
            #endregion
            return PartialView("_PurchaseRequestDetails", objPurchaseRequestVM);
        }
        public RedirectResult DetailFromApproval(long PurchaseRequestId)
        {
            TempData["Mode"] = "DetailFromApproval";
            TempData["PurchaseRequestId"] = PurchaseRequestId;
            return Redirect("/PurchaseRequest/Index?page=Procurement_Requests");
        }
        public RedirectResult DetailFromApprovalDynamic(long PurchaseRequestId)
        {
            TempData["Mode"] = "DetailFromApprovalDynamic";
            TempData["PurchaseRequestId"] = PurchaseRequestId;
            return Redirect("/PurchaseRequest/Index?page=Procurement_Requests");
        }
        public RedirectResult DetailFromShoppingCart(long PurchaseRequestId)
        {
            TempData["Mode"] = "DetailFromShoppingCart";
            TempData["PurchaseRequestId"] = PurchaseRequestId;
            return Redirect("/PurchaseRequest/Index?page=Procurement_Requests");
        }
        [EncryptedActionParameter]
        public ActionResult Print(string PurchaseRequestID)
        {
            PurchaseRequestVM objPurchaseRequestVM = new PurchaseRequestVM();
            PurchaseRequestWrapper pWrapper = new PurchaseRequestWrapper(userData);
            PurchaseRequestModel objPurchaseRequestModel = new PurchaseRequestModel();
            objPurchaseRequestModel = pWrapper.RetrieveByPKForeignKeysForReport(Convert.ToInt64(PurchaseRequestID));
            if (objPurchaseRequestModel != null)
            {
                objPurchaseRequestVM.purchaseRequestModel = objPurchaseRequestModel;
            }
            List<LineItemModel> lineItem = pWrapper.PopulateLineitems(Convert.ToInt64(PurchaseRequestID));
            objPurchaseRequestVM.listLineItem = lineItem;
            string customSwitches = string.Format("--header-html  \"{0}\" " +
                                  "--header-spacing \"1\" " +
                                  "--header-font-size \"10\" ",
                                  Url.Action("Header", "PurchaseRequest", new { id = userData.LoginAuditing.SessionId, Requestid = PurchaseRequestID }, Request.Url.Scheme)
                                  );
            LocalizeControls(objPurchaseRequestVM, LocalizeResourceSetConstants.PurchaseRequest);
            return new ViewAsPdf("PRTemplatePrint", objPurchaseRequestVM)
            {
                CustomSwitches = customSwitches
            };
        }
        [AllowAnonymous]
        [SkipSessionExpiaryActionFilter]
        public ActionResult Header(string id, long Requestid)
        {
            PurchaseRequestVM objPurchaseRequestVM = new PurchaseRequestVM();
            PurchaseRequestModel objPurchaseRequestModel = new PurchaseRequestModel();
            if (CheckLoginSession(id))
            {
                PurchaseRequestWrapper pWrapper = new PurchaseRequestWrapper(userData);
                CommonWrapper comWrapper = new CommonWrapper(userData);

                #region Purchase Request
                objPurchaseRequestModel = pWrapper.RetrieveByPKForeignKeysForReport(Convert.ToInt64(Requestid));
                objPurchaseRequestModel.AzureImageURL = comWrapper.GetClientLogoUrl();
                if (objPurchaseRequestModel != null)
                {
                    objPurchaseRequestVM.purchaseRequestModel = objPurchaseRequestModel;
                }
                string angelBgPath = Server.MapPath("~/Content/images/angleBg.jpg");
                #endregion
            }
            objPurchaseRequestVM.udata = userData;
            LocalizeControls(objPurchaseRequestVM, LocalizeResourceSetConstants.PurchaseRequest);
            return View("PrintHeader", objPurchaseRequestVM);
        }
        [AllowAnonymous]
        [SkipSessionExpiaryActionFilter]
        public ActionResult Footer(string id)
        {
            PurchaseRequestVM objPurchaseRequestVM = new PurchaseRequestVM();
            if (CheckLoginSession(id))
            {
                LocalizeControls(objPurchaseRequestVM, LocalizeResourceSetConstants.PurchaseRequest);
            }
            return View("PrintFooter", objPurchaseRequestVM);
        }

        public ActionResult GetPRInnerGrid(long PurchaseRequestID)
        {
            PurchaseRequestVM objPurchaseRequestVM = new PurchaseRequestVM();
            PurchaseRequestWrapper pWrapper = new PurchaseRequestWrapper(userData);
            objPurchaseRequestVM.listLineItem = pWrapper.PopulateLineitems(PurchaseRequestID);
            LocalizeControls(objPurchaseRequestVM, LocalizeResourceSetConstants.PurchaseRequest);
            return View("_InnerGridPRLineItem", objPurchaseRequestVM);
        }
        #endregion

        #region Line Item
        public string PopulateLineItem(int? draw, int? start, int? length, string searchText, long PurchaseRequestId = 0, string LineNumber = "", String PartId = "",
                                            string Description = "", decimal Quantity = 0, string UOM = "", decimal UnitCost = 0, decimal TotalCost = 0, string Account = "", string ChargeToClientLookupId = "")
        {
            PurchaseRequestWrapper pWrapper = new PurchaseRequestWrapper(userData);
            var PRId = PurchaseRequestId;
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            var LineItems = pWrapper.PopulateLineitems(PRId);
            LineItems = this.GetLineItemsByColumnWithOrder(order, orderDir, LineItems);
            {
                searchText = searchText.ToUpper();
                int VAL;
                bool res = int.TryParse(searchText, out VAL);
                LineItems = LineItems.Where(x => (!string.IsNullOrWhiteSpace(x.PartClientLookupId) && x.PartClientLookupId.ToUpper().Contains(searchText))
                                                        || (x.LineNumber != 0 && x.LineNumber.Equals(searchText)
                                                        || (!string.IsNullOrWhiteSpace(x.Description) && x.Description.ToUpper().Contains(searchText))
                                                        || (!string.IsNullOrWhiteSpace(x.PurchaseUOM) && x.PurchaseUOM.ToUpper().Contains(searchText))
                                                        || (res == true && x.OrderQuantity.Equals(VAL))
                                                        || (res == true && x.UnitCost.Equals(VAL))
                                                        || (res == true && x.TotalCost.Equals(VAL))
                                                        || (!string.IsNullOrWhiteSpace(x.Account_ClientLookupId) && x.Account_ClientLookupId.Equals(Account))
                                                        || (!string.IsNullOrWhiteSpace(x.ChargeToClientLookupId) && x.ChargeToClientLookupId.Equals(ChargeToClientLookupId)))
                                                        ).ToList();
            }
            if (LineItems != null)
            {
                if (!string.IsNullOrEmpty(LineNumber))
                {
                    int LineNumberVAL;
                    bool res = int.TryParse(LineNumber, out LineNumberVAL);
                    LineItems = LineItems.Where(x => x.LineNumber.Equals(Convert.ToInt32(LineNumberVAL))).ToList();
                }
                if (!string.IsNullOrEmpty(PartId))
                {
                    PartId = PartId.ToUpper();
                    LineItems = LineItems.Where(x => (!string.IsNullOrWhiteSpace(x.PartClientLookupId) && x.PartClientLookupId.ToUpper().Contains(PartId))).ToList();
                }
                if (!string.IsNullOrEmpty(Description))
                {
                    Description = Description.ToUpper();
                    LineItems = LineItems.Where(x => (!string.IsNullOrWhiteSpace(x.Description) && x.Description.ToUpper().Contains(Description))).ToList();
                }
                if (Quantity != 0)
                {
                    LineItems = LineItems.Where(x => x.OrderQuantity.Equals(Quantity)).ToList();
                }
                if (!string.IsNullOrEmpty(UOM))
                {
                    UOM = UOM.ToUpper();
                    LineItems = LineItems.Where(x => (!string.IsNullOrWhiteSpace(x.PurchaseUOM) && x.PurchaseUOM.ToUpper().Contains(UOM))).ToList();
                }
                if (UnitCost != 0)
                {
                    LineItems = LineItems.Where(x => x.UnitCost.Equals(UnitCost)).ToList();
                }
                if (TotalCost != 0)
                {
                    LineItems = LineItems.Where(x => x.TotalCost.Equals(TotalCost)).ToList();
                }
                if (!string.IsNullOrEmpty(Account))
                {
                    LineItems = LineItems.Where(x => x.Account_ClientLookupId.Equals(Account)).ToList();
                }
                if (!string.IsNullOrEmpty(ChargeToClientLookupId))
                {
                    LineItems = LineItems.Where(x => x.ChargeToClientLookupId.Equals(ChargeToClientLookupId)).ToList();
                }


            }
            var totalRecords = 0;
            var recordsFiltered = 0;
            start = start.HasValue
                ? start / length
                : 0;
            recordsFiltered = LineItems.Count();
            totalRecords = LineItems.Count();
            var GrandTotalCost = LineItems.Sum(m => m.TotalCost);
            int initialPage = start.Value;
            var filteredResult = LineItems
                .Skip(initialPage * length ?? 0)
                .Take(length ?? 0)
                .ToList();

            #region uiconfig

            CommonWrapper cWrapper = new CommonWrapper(userData);
            var hiddenList = cWrapper.GetHiddenList(UiConfigConstants.PurchaseRequestLineItemSearch).Select(x => x.ColumnName).ToList();

            #endregion
            var IsShoppingCart = userData.Site.ShoppingCart;
            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult, hiddenColumnList = hiddenList, IsShoppingCart = IsShoppingCart, GrandTotalCost = Math.Round(GrandTotalCost, 2) }, JsonSerializerDateSettings);
        }
        public string GetSelectPartsGridData(int? draw, int? start, int? length, string searchText, string PartId = "", string Description = "")
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            PurchaseRequestWrapper pWrapper = new PurchaseRequestWrapper(userData);
            var pSelectedList = pWrapper.PopulateSelectParts();
            pSelectedList = this.GetAllSelectedPartsByColumnWithOrder(order, orderDir, pSelectedList);
            if (pSelectedList != null && !string.IsNullOrEmpty(searchText))
            {
                searchText = searchText.ToUpper();
                int quantity;
                bool res = int.TryParse(searchText, out quantity);
                pSelectedList = pSelectedList.Where(x => (!string.IsNullOrWhiteSpace(x.ClientLookupId) && x.ClientLookupId.ToUpper().Contains(searchText))
                                                        || (!string.IsNullOrWhiteSpace(x.ClientLookupId) && x.ClientLookupId.ToUpper().Contains(searchText))
                                                        || (!string.IsNullOrWhiteSpace(x.Description) && x.Description.ToUpper().Contains(searchText))
                                                        || (!string.IsNullOrWhiteSpace(x.Manufacturer) && x.Manufacturer.ToUpper().Contains(searchText))
                                                        || (res == true && x.Quantity.Equals(Convert.ToInt32(quantity)))
                                                        ).ToList();
            }
            if (pSelectedList != null)
            {
                if (!string.IsNullOrEmpty(PartId))
                {
                    PartId = PartId.ToUpper();
                    pSelectedList = pSelectedList.Where(x => (!string.IsNullOrWhiteSpace(x.ClientLookupId) && x.ClientLookupId.ToUpper().Contains(PartId))).ToList();
                }
                if (!string.IsNullOrEmpty(Description))
                {
                    Description = Description.ToUpper();
                    pSelectedList = pSelectedList.Where(x => (!string.IsNullOrWhiteSpace(x.Description) && x.Description.ToUpper().Contains(Description))).ToList();
                }
            }
            var totalRecords = 0;
            var recordsFiltered = 0;
            start = start.HasValue
                ? start / length
                : 0;
            recordsFiltered = pSelectedList.Count();
            totalRecords = pSelectedList.Count();
            int initialPage = start.Value;
            var filteredResult = pSelectedList
                .Skip(initialPage * length ?? 0)
                .Take(length ?? 0)
                .ToList();
            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult });
        }
        [HttpGet]
        public ActionResult EditLineItem(long LineItemId, long PurchaseRequestId, string ClientLookupId, string status)
        {
            PurchaseRequestVM objPurchaseRequestVM = new PurchaseRequestVM();
            PurchaseRequestWrapper pWrapper = new PurchaseRequestWrapper(userData);
            var LineItem = pWrapper.GetLineItem(LineItemId, PurchaseRequestId);
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            List<DataContracts.LookupList> UOMs = new List<DataContracts.LookupList>();
            var ScheduleChargeTypeList = UtilityFunction.populateChargeTypeForLineItem();
            if (ScheduleChargeTypeList != null)
            {
                LineItem.ChargeTypeList = ScheduleChargeTypeList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
            }
            // 2022-Apr-21 - RKL - The edit view uses the "chuncked" lookups for this
            //  We do not need to set the LineItem.ChargeTypeLookupList
            /*
            if (LineItem.ChargeType != null)
            {
                var ChargeTypeLookUpList = PopulatelookUpListByType(LineItem.ChargeType);
                if (ChargeTypeLookUpList != null && ChargeTypeLookUpList.Count > 0)
                {
                    LineItem.ChargeTypelookUpList = ChargeTypeLookUpList.Select(x => new SelectListItem { Text = x.ChargeToClientLookupId + " - " + x.Name, Value = x.ChargeToId.ToString() });
                }
            }
            else
            {
                LineItem.ChargeTypelookUpList = new List<SelectListItem>();
            }
            LineItem.ChargeTypelookUpList = new List<SelectListItem>();
            */
            //V2-379
            var AcclookUpList = GetAccountByActiveState(true);
            LineItem.AccountList = AcclookUpList.Select(x => new SelectListItem { Text = x.Account + " - " + x.Name, Value = x.AccountId.ToString() });
            var AllLookUps = commonWrapper.GetAllLookUpList();
            if (AllLookUps != null)
            {
                UOMs = AllLookUps.Where(x => x.ListName == LookupListConstants.Unit_Of_Measure).ToList();
            }
            LineItem.UOMList = UOMs.Select(x => new SelectListItem { Text = x.ListValue + " - " + x.Description, Value = x.ListValue.ToString() });
            LineItem.ViewName = UiConfigConstants.PurchaseRequestLineItemEdit;  //--V2-375--uiconfig//
            objPurchaseRequestVM.lineItem = LineItem;
            objPurchaseRequestVM.lineItem.ClientLookupId = ClientLookupId;
            objPurchaseRequestVM.lineItem.ChargeToClientLookupIdToShow = LineItem.ChargeToClientLookupId;
            objPurchaseRequestVM.lineItem.IsShopingCart = userData.Site.ShoppingCart; //V2-563
            objPurchaseRequestVM.lineItem.status = status;
            #region uiconfig
            CommonWrapper cWrapper = new CommonWrapper(userData);
            var totalList = cWrapper.GetAllUiConfigList(UiConfigConstants.PurchaseRequestLineItemEdit);
            var hidList = totalList.Where(x => x.Hide == true);
            objPurchaseRequestVM.hiddenColumnList = new List<string>();
            foreach (var item in hidList)
            {
                objPurchaseRequestVM.hiddenColumnList.Add(item.ColumnName);
            }
            var dsablList = totalList.Where(x => x.Disable == true);
            objPurchaseRequestVM.disabledColumnList = new List<string>();
            foreach (var item in dsablList)
            {
                objPurchaseRequestVM.disabledColumnList.Add(item.ColumnName);
            }
            var impList = totalList.Where(x => x.Required == true && x.Hide == false && x.Disable == false);
            objPurchaseRequestVM.requiredColumnList = new List<string>();
            foreach (var item in impList)
            {
                objPurchaseRequestVM.requiredColumnList.Add(item.ColumnName);
            }
            #endregion

            LocalizeControls(objPurchaseRequestVM, LocalizeResourceSetConstants.PurchaseRequest);
            return PartialView("_EditLineItem", objPurchaseRequestVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditLineItem(PurchaseRequestVM PurchaseRequestVM)
        {
            string ModelValidationFailedMessage = string.Empty;
            PurchaseRequestWrapper pWrapper = new PurchaseRequestWrapper(userData);
            ModelState.Remove("lineItem.ChargeToClientLookupIdToShow");
            if (ModelState.IsValid)
            {
                var lineItem = pWrapper.UpdateLineItem(PurchaseRequestVM.lineItem);
                if (lineItem.ErrorMessages.Count == 0)
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), purchaserequestid = PurchaseRequestVM.lineItem.PurchaseRequestId }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(lineItem.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }

        }
        public PartialViewResult AddPartInInventory(long PurchaseRequestId, string ClientLookupId, long vendorId = 0, long StoreroomId = 0)
        {
            PartLookupVM partLookupVM = new PartLookupVM();
            partLookupVM.PurchaseRequestId = PurchaseRequestId;
            partLookupVM.ClientLookupId = ClientLookupId;
            partLookupVM.VendorId = vendorId;
            partLookupVM.StoreroomId = StoreroomId; /*738*/
            partLookupVM.ShoppingCart = userData.Site.ShoppingCart;
            partLookupVM.IsOnOderCheck = userData.Site.OnOrderCheck;
            LocalizeControls(partLookupVM, LocalizeResourceSetConstants.PartLookUpDetails);
            return PartialView("~/views/partlookup/index.cshtml", partLookupVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult PartNotInInventory(PurchaseRequestVM objPurchaseRequestVM)
        {
            PurchaseRequestWrapper pWrapper = new PurchaseRequestWrapper(userData);
            string ModelValidationFailedMessage = string.Empty;
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            if (ModelState.IsValid)
            {
                var lineItem = pWrapper.SavePartNotInInventory(objPurchaseRequestVM.PartNotInInventoryModel);
                if (lineItem.ErrorMessages != null && lineItem.ErrorMessages.Count > 0)
                {
                    return Json(lineItem.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), purchaserequestid = objPurchaseRequestVM.PartNotInInventoryModel.PurchaseRequestId }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }
        public PartialViewResult AddPartNotInInventory(long PurchaseRequestId, string ClientLookupId, String Status)
        {
            PurchaseRequestVM objPurchaseRequestVM = new PurchaseRequestVM();
            PurchaseRequestWrapper pWrapper = new PurchaseRequestWrapper(userData);
            PartNotInInventoryModel objPartNotInInventoryModel = new PartNotInInventoryModel();
            PurchaseRequestModel objPurchaseRequestModel = new PurchaseRequestModel();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            List<DataContracts.LookupList> UOMs = new List<DataContracts.LookupList>();
            var ChargeTypeList = UtilityFunction.populateChargeTypeForLineItem();
            if (ChargeTypeList != null)
            {
                objPartNotInInventoryModel.ChargeTypeList = ChargeTypeList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
            }
            objPartNotInInventoryModel.ChargeTypelookUpList = new List<SelectListItem>();
            //V2-379
            var AcclookUpList = GetAccountByActiveState(true);
            if (AcclookUpList != null)
            {
                objPartNotInInventoryModel.AccountList = AcclookUpList.Select(x => new SelectListItem { Text = x.Account + " - " + x.Name, Value = x.AccountId.ToString() });
            }
            objPartNotInInventoryModel.AccountId = userData.Site.NonStockAccountId;//RNJ
            var AllLookUps = commonWrapper.GetAllLookUpList();
            if (AllLookUps != null)
            {
                UOMs = AllLookUps.Where(x => x.ListName == LookupListConstants.Unit_Of_Measure).ToList();
            }
            if (UOMs != null)
            {
                objPartNotInInventoryModel.UOMList = UOMs.Select(x => new SelectListItem { Text = x.ListValue + " - " + x.Description, Value = x.ListValue.ToString() });
            }
            objPartNotInInventoryModel.PurchaseRequestId = PurchaseRequestId;
            objPartNotInInventoryModel.ViewName = UiConfigConstants.PurchaseRequestLineItemAdd;  //--V2-375--uiconfig//
            objPurchaseRequestVM.PartNotInInventoryModel = objPartNotInInventoryModel;
            objPurchaseRequestVM.PartNotInInventoryModel.ClientLookupId = ClientLookupId;
            objPurchaseRequestVM.PartNotInInventoryModel.IsShopingCart = userData.Site.ShoppingCart; //V2-563
            if (objPurchaseRequestVM.PartNotInInventoryModel.IsShopingCart)
            {
                objPurchaseRequestVM.PartNotInInventoryModel.RequiredDate = DateTime.Today.AddDays(7);
            }


            #region uiconfig
            CommonWrapper cWrapper = new CommonWrapper(userData);
            var totalList = cWrapper.GetAllUiConfigList(UiConfigConstants.PurchaseRequestLineItemAdd);
            var hidList = totalList.Where(x => x.Hide == true);
            objPurchaseRequestVM.hiddenColumnList = new List<string>();
            foreach (var item in hidList)
            {
                objPurchaseRequestVM.hiddenColumnList.Add(item.ColumnName);
            }
            var dsablList = totalList.Where(x => x.Disable == true);
            objPurchaseRequestVM.disabledColumnList = new List<string>();
            foreach (var item in dsablList)
            {
                objPurchaseRequestVM.disabledColumnList.Add(item.ColumnName);
            }
            var impList = totalList.Where(x => x.Required == true && x.Hide == false && x.Disable == false);
            objPurchaseRequestVM.requiredColumnList = new List<string>();
            foreach (var item in impList)
            {
                objPurchaseRequestVM.requiredColumnList.Add(item.ColumnName);
            }
            #endregion
            objPurchaseRequestVM.PartNotInInventoryModel.Status = Status;
            LocalizeControls(objPurchaseRequestVM, LocalizeResourceSetConstants.PurchaseRequest);
            return PartialView("_AddPartNotInInventory", objPurchaseRequestVM);
        }
        public JsonResult SavePartInInventory(List<LineItemModel> list)
        {
            PurchaseRequestWrapper pWrapper = new PurchaseRequestWrapper(userData);
            var result = pWrapper.UpadatePartIn(list);
            if (result != null && result.Count == 0)
            {
                return Json(new { Result = JsonReturnEnum.success.ToString(), purchaserequestid = list[0].PurchaseRequestId }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { Result = JsonReturnEnum.failed.ToString() }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DeleteLineItem(long PurchaseRequestLineItemId, long PurchaseRequestId, string Status)
        {
            PurchaseRequestWrapper pWrapper = new PurchaseRequestWrapper(userData);
            var deleteResult = pWrapper.DeleteLineItem(PurchaseRequestLineItemId, PurchaseRequestId, Status);
            if (deleteResult)
            {
                return Json(new { Result = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { Result = JsonReturnEnum.failed.ToString() }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Notes
        public string PopulateNotes(int? draw, int? start, int? length, long PurchaseRequestId)
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            PurchaseRequestWrapper pWrapper = new PurchaseRequestWrapper(userData);
            var Notes = pWrapper.PopulateNotes(PurchaseRequestId);
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
        [HttpGet]
        public ActionResult AddNotes(long PurchaseRequestId, string ClientLookupId)
        {
            PurchaseRequestVM objPurchaseRequestVM = new PurchaseRequestVM();
            NotesModel notesModel = new NotesModel();
            notesModel.PurchaseRequestId = PurchaseRequestId;
            notesModel.ClientLookupId = ClientLookupId;
            objPurchaseRequestVM.notesModel = notesModel;
            LocalizeControls(objPurchaseRequestVM, LocalizeResourceSetConstants.PurchaseRequest);
            return PartialView("_AddPRNotes", objPurchaseRequestVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddNotes(PurchaseRequestVM _PurchaseRequest)
        {
            string ModelValidationFailedMessage = string.Empty;
            if (ModelState.IsValid)
            {
                string Mode = string.Empty;
                List<String> errorList = new List<string>();
                if (_PurchaseRequest.notesModel.NotesId == 0)
                {
                    Mode = "add";
                }
                PurchaseRequestWrapper pWrapper = new PurchaseRequestWrapper(userData);
                errorList = pWrapper.UpdatePurchaseReqNote(_PurchaseRequest);
                if (errorList != null && errorList.Count > 0)
                {
                    return Json(errorList, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), purchaserequestid = _PurchaseRequest.notesModel.PurchaseRequestId, mode = Mode }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(new { Result = ModelValidationFailedMessage }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult EditNote(long PurchaseRequestId, long NotesId, string ClientLookupId)
        {
            PurchaseRequestVM objPurchaseRequestVM = new PurchaseRequestVM();
            NotesModel objNotesModel = new NotesModel();
            PurchaseRequestWrapper pWrapper = new PurchaseRequestWrapper(userData);
            objNotesModel = pWrapper.EditPurchaseReqNote(PurchaseRequestId, NotesId);
            objNotesModel.ClientLookupId = ClientLookupId;
            objPurchaseRequestVM.notesModel = objNotesModel;
            LocalizeControls(objPurchaseRequestVM, LocalizeResourceSetConstants.PurchaseRequest);
            return PartialView("_AddPRNotes", objPurchaseRequestVM);
        }
        [HttpPost]
        public ActionResult DeleteNotes(string _notesId)
        {
            PurchaseRequestWrapper pWrapper = new PurchaseRequestWrapper(userData);
            if (pWrapper.DeletePurchaseReqNote(_notesId))
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
        public string PopulateAttachments(int? draw, int? start, int? length, long PurchaseRequestId)
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            CommonWrapper objCommonWrapper = new CommonWrapper(userData);
            var Attachments = objCommonWrapper.PopulateAttachments(PurchaseRequestId, "PurchaseRequest", userData.Security.PurchaseRequest.Edit);
            if (Attachments != null)
            {
                Attachments = GetAllAttachmentsSortByColumnWithOrder(order, orderDir, Attachments);
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
            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult }, JsonSerializer12HoursDateAndTimeSettings);
        }
        [HttpGet]
        public PartialViewResult AddAttachments(long PurchaseRequestId, string ClientLookupId)
        {
            PurchaseRequestVM objPurchaseRequestVM = new PurchaseRequestVM();
            AttachmentModel Attachment = new AttachmentModel();
            Attachment.PurchaseRequestId = PurchaseRequestId;
            objPurchaseRequestVM.attachmentModel = Attachment;
            objPurchaseRequestVM.attachmentModel.ClientLookupId = ClientLookupId;
            Attachment.ClientLookupId = ClientLookupId;
            Attachment.PurchaseRequestId = PurchaseRequestId;
            LocalizeControls(objPurchaseRequestVM, LocalizeResourceSetConstants.PurchaseRequest);
            return PartialView("_AddPRAttachment", objPurchaseRequestVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddAttachments()
        {
            string ModelValidationFailedMessage = string.Empty;
            if (ModelState.IsValid)
            {
                Stream stream = Request.Files[0].InputStream;
                Client.Models.AttachmentModel attachmentModel = new Client.Models.AttachmentModel();
                CommonWrapper objCommonWrapper = new CommonWrapper(userData);
                Attachment fileAtt = new Attachment();
                attachmentModel.FileName = System.IO.Path.GetFileNameWithoutExtension(Request.Files[0].FileName);
                string fileExt = System.IO.Path.GetExtension(Request.Files[0].FileName);
                attachmentModel.FileType = fileExt.Substring(1);
                attachmentModel.FileSize = Request.Files[0].ContentLength;
                attachmentModel.ContentType = Request.Files[0].ContentType;
                attachmentModel.ObjectId = Convert.ToInt64(Request.Form["attachmentModel.PurchaseRequestId"]);
                attachmentModel.Subject = String.IsNullOrEmpty(Request.Form["attachmentModel.Subject"]) ? "No Subject" : Request.Form["attachmentModel.Subject"];
                attachmentModel.TableName = AttachmentTableConstant.PurchaseRequest;
                bool attachStatus = false;
                bool OnPremise = userData.DatabaseKey.Client.OnPremise;
                if (OnPremise)
                {
                    fileAtt = objCommonWrapper.AddAttachmentOnPremise(attachmentModel, stream, ref attachStatus, userData.Security.PurchaseRequest.Edit);
                }
                else
                {
                    fileAtt = objCommonWrapper.AddAttachment(attachmentModel, stream, ref attachStatus, userData.Security.PurchaseRequest.Edit);
                }
                if (attachStatus)
                {
                    if (fileAtt.ErrorMessages != null && fileAtt.ErrorMessages.Count > 0)
                    {
                        return Json(fileAtt.ErrorMessages, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { Result = JsonReturnEnum.success.ToString(), purchaserequestid = Convert.ToInt64(Request.Form["attachmentModel.PurchaseRequestId"]) }, JsonRequestBehavior.AllowGet);
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
        public ActionResult DeleteAttachments(long _fileAttachmentId)
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
        #endregion

        #region Private method
        private List<LineItemModel> GetLineItemsByColumnWithOrder(string order, string orderDir, List<LineItemModel> data)
        {
            List<LineItemModel> lst = new List<LineItemModel>();

            switch (order)
            {
                case "0":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.LineNumber).ToList() : data.OrderBy(p => p.LineNumber).ToList();
                    break;
                case "1":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.PartClientLookupId).ToList() : data.OrderBy(p => p.PartClientLookupId).ToList();
                    break;
                case "2":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Description).ToList() : data.OrderBy(p => p.Description).ToList();
                    break;
                case "3":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.OrderQuantity).ToList() : data.OrderBy(p => p.OrderQuantity).ToList();
                    break;
                case "4":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.UnitofMeasure).ToList() : data.OrderBy(p => p.UnitofMeasure).ToList();
                    break;
                case "5":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.UnitCost).ToList() : data.OrderBy(p => p.UnitCost).ToList();
                    break;
                case "6":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.TotalCost).ToList() : data.OrderBy(p => p.TotalCost).ToList();
                    break;
                case "7":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Account_ClientLookupId).ToList() : data.OrderBy(p => p.Account_ClientLookupId).ToList();
                    break;
                case "8":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.RequiredDate).ToList() : data.OrderBy(p => p.RequiredDate).ToList();
                    break;
                case "9":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ChargeToClientLookupId).ToList() : data.OrderBy(p => p.ChargeToClientLookupId).ToList();
                    break;
                default:
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.PartClientLookupId).ToList() : data.OrderBy(p => p.PartClientLookupId).ToList();
                    break;
            }
            return lst;
        }
        private List<NotesModel> GetAllNotesSortByColumnWithOrder(string order, string orderDir, List<NotesModel> data)
        {
            List<NotesModel> lst = new List<NotesModel>();
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
            return lst;
        }
        private List<PurchaseRequestModel> GetAllEquipmentsSortByColumnWithOrder(string order, string orderDir, List<PurchaseRequestModel> data)
        {
            List<PurchaseRequestModel> lst = new List<PurchaseRequestModel>();
            switch (order)
            {
                case "0":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ClientLookupId).ToList() : data.OrderBy(p => p.ClientLookupId).ToList();
                    break;
                case "1":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Reason).ToList() : data.OrderBy(p => p.Reason).ToList();
                    break;
                case "2":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Status).ToList() : data.OrderBy(p => p.Status).ToList();
                    break;
                case "3":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Creator_PersonnelName).ToList() : data.OrderBy(p => p.Creator_PersonnelName).ToList();
                    break;
                case "4":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.VendorClientLookupId).ToList() : data.OrderBy(p => p.VendorClientLookupId).ToList();
                    break;
                case "5":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.VendorName).ToList() : data.OrderBy(p => p.VendorName).ToList();
                    break;
                case "6":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.CreateDate).ToList() : data.OrderBy(p => p.CreateDate).ToList();
                    break;
                case "7":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.PurchaseOrderClientLookupId).ToList() : data.OrderBy(p => p.PurchaseOrderClientLookupId).ToList();
                    break;
                case "8":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Processed_PersonnelName).ToList() : data.OrderBy(p => p.Processed_PersonnelName).ToList();
                    break;
                case "9":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ProcessedDate).ToList() : data.OrderBy(p => p.ProcessedDate).ToList();
                    break;
                default:
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ClientLookupId).ToList() : data.OrderBy(p => p.ClientLookupId).ToList();
                    break;
            }
            return lst;
        }
        private List<PartInInventoryModel> GetAllSelectedPartsByColumnWithOrder(string order, string orderDir, List<PartInInventoryModel> data)
        {
            List<PartInInventoryModel> lst = new List<PartInInventoryModel>();
            switch (order)
            {
                case "1":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ClientLookupId).ToList() : data.OrderBy(p => p.ClientLookupId).ToList();
                    break;
                case "2":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Description).ToList() : data.OrderBy(p => p.Description).ToList();
                    break;
                case "3":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Manufacturer).ToList() : data.OrderBy(p => p.Manufacturer).ToList();
                    break;
                case "4":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Quantity).ToList() : data.OrderBy(p => p.Quantity).ToList();
                    break;
                default:
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ClientLookupId).ToList() : data.OrderBy(p => p.ClientLookupId).ToList();
                    break;
            }
            return lst;
        }
        #endregion

        #region Convert To Purchase Order
        [HttpPost]
        public string GetConvertToPurchaseOrderMainGrid(int? draw, int? start, int? length)
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            PurchaseRequestWrapper pWrapper = new PurchaseRequestWrapper(userData);
            List<ConvertToPOModel> ConvertToPurchaseOrderList = pWrapper.populateConvertToPurchaseOrder();
            ConvertToPurchaseOrderList = this.GetConvertToPurchaseOrderListGridSortByColumnWithOrder(order, orderDir, ConvertToPurchaseOrderList);
            var totalRecords = 0;
            var recordsFiltered = 0;
            start = start.HasValue
                ? start / length
                : 0;
            recordsFiltered = ConvertToPurchaseOrderList.Count();
            totalRecords = ConvertToPurchaseOrderList.Count();
            int initialPage = start.Value;
            var filteredResult = ConvertToPurchaseOrderList
                .Skip(initialPage * length ?? 0)
                .Take(length ?? 0)
                .ToList();

            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult }, JsonSerializerDateSettings);
        }
        private List<ConvertToPOModel> GetConvertToPurchaseOrderListGridSortByColumnWithOrder(string order, string orderDir, List<ConvertToPOModel> data)
        {
            List<ConvertToPOModel> lst = new List<ConvertToPOModel>();
            switch (order)
            {
                case "2":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ClientLookupId).ToList() : data.OrderBy(p => p.ClientLookupId).ToList();
                    break;
                case "3":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.CountLineItem).ToList() : data.OrderBy(p => p.CountLineItem).ToList();
                    break;
                case "4":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Reason).ToList() : data.OrderBy(p => p.Reason).ToList();
                    break;
                case "5":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Creator_PersonnelName).ToList() : data.OrderBy(p => p.Creator_PersonnelName).ToList();
                    break;
                case "6":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Approved_PersonnelName).ToList() : data.OrderBy(p => p.Approved_PersonnelName).ToList();
                    break;
                default:
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ClientLookupId).ToList() : data.OrderBy(p => p.ClientLookupId).ToList();
                    break;
            }
            return lst;
        }

        [HttpGet]
        public PartialViewResult InnerGrid(long PurchaseRequestId)
        {
            PurchaseRequestWrapper pWrapper = new PurchaseRequestWrapper(userData);
            PurchaseRequestVM objPurchaseRequestVM = new PurchaseRequestVM();
            LineItemModel obj = new LineItemModel();
            var LineItemsList = pWrapper.PopulateLineitems(PurchaseRequestId);
            if (LineItemsList != null)
            {
                objPurchaseRequestVM.listLineItem = LineItemsList;
            }
            LocalizeControls(objPurchaseRequestVM, LocalizeResourceSetConstants.PurchaseRequest);
            return PartialView("~/Views/PurchaseRequest/ConvertPRToPOLineItem.cshtml", objPurchaseRequestVM);
        }
        [HttpGet]
        public JsonResult GetConvertPRToPO()
        {
            PurchaseRequestWrapper pWrapper = new PurchaseRequestWrapper(userData);
            List<ConvertToPOModel> GetConvertToPurchaseOrderList = pWrapper.populateConvertToPurchaseOrder();
            return Json(GetConvertToPurchaseOrderList, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region PR AutoGeneration
        [HttpPost]
        public JsonResult PRAutoGenerate()
        {
            PurchaseRequestWrapper pWrapper = new PurchaseRequestWrapper(userData);
            ProcessLog processLog = new ProcessLog();
            processLog = pWrapper.PReqAutoGenerate();
            ProcesLogModel procesLogModel = new ProcesLogModel();
            procesLogModel.ItemsReviewed = processLog.ItemsReviewed;
            procesLogModel.HeadersCreated = processLog.HeadersCreated;
            procesLogModel.DetailsCreated = processLog.DetailsCreated;
            PrevMaintVM prevMaintVM = new PrevMaintVM();
            prevMaintVM.procesLogModel = procesLogModel;
            return Json(procesLogModel, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region V2-375
        public JsonResult UpdateStatus(long PurchaseRequestId, string Status, string ClientLookupId, string Comments = null, int PersonnelId = 0, int lineCount = 0)
        {
            PurchaseRequestWrapper pWrapper = new PurchaseRequestWrapper(userData);
            string errormessage = string.Empty;
            pWrapper.UpdateStatus(PurchaseRequestId, Status, Comments, PersonnelId, lineCount);

            if (pWrapper.errorMessage != null && pWrapper.errorMessage.Count > 0)
            {
                errormessage = ClientLookupId + " Failed " + ": " + pWrapper.errorMessage[0];
            }

            if (!string.IsNullOrEmpty(errormessage))
            {
                return Json(new { data = errormessage }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { data = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult UpdateStatusApproveBatch(List<long> model)
        {
            DataContracts.PurchaseRequest purchaseRequest = new DataContracts.PurchaseRequest();
            PurchaseRequestWrapper pWrapper = new PurchaseRequestWrapper(userData);
            List<string> errMsgList = new List<string>();
            foreach (var item in model)
            {
                purchaseRequest = pWrapper.UpdateStatusBatch(item, PurchaseReturnStatusEnum.approve);
                if (purchaseRequest.ErrorMessages != null && purchaseRequest.ErrorMessages.Count > 0)
                {
                    string errormessage = "Failed to approve " + purchaseRequest.ClientLookupId + ": " + purchaseRequest.ErrorMessages[0];
                    errMsgList.Add(errormessage);
                }
            }
            if (errMsgList.Count > 0)
            {
                return Json(new { data = errMsgList }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { data = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult UpdateStatusDenyBatch(PurchaseRequestStatusModel model)
        {
            List<string> errMsgList = new List<string>();
            List<string> failedPrList = new List<string>();
            string currentStatus = string.Empty;
            DataContracts.PurchaseRequest purchaseRequest = new DataContracts.PurchaseRequest();
            PurchaseRequestWrapper pWrapper = new PurchaseRequestWrapper(userData);
            foreach (var item in model.list)
            {
                if (!string.IsNullOrEmpty(item.Status))
                {
                    currentStatus = item.Status.ToUpper();
                }
                if (!(currentStatus == PurchaseRequestStatusConstants.AwaitApproval.ToUpper() || currentStatus == PurchaseRequestStatusConstants.Open.ToUpper() || currentStatus == PurchaseRequestStatusConstants.Resubmit.ToUpper()))
                {
                    failedPrList.Add(item.ClientLookupId);
                }
                else
                {
                    purchaseRequest = pWrapper.UpdateStatusBatch(item.PurchaseRequestId, PurchaseReturnStatusEnum.deny, model.comments, item.ChildCount);
                    if (purchaseRequest.ErrorMessages != null && purchaseRequest.ErrorMessages.Count > 0)
                    {
                        string errormessage = "Failed to approve " + purchaseRequest.ClientLookupId + ": " + purchaseRequest.ErrorMessages[0];
                        errMsgList.Add(errormessage);
                    }
                }
            }
            if (errMsgList.Count > 0 || failedPrList.Count > 0)
            {
                if (failedPrList.Count > 0)
                {
                    errMsgList.Add("Purchase Request(s) " + string.Join(", ", failedPrList) + " can't be denied. Please check the status.");
                }
                return Json(new { data = errMsgList }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { data = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult UpdateStatusReturnToRequestorBatch(PurchaseRequestStatusModel model)
        {
            List<string> errMsgList = new List<string>();
            List<string> failedPrList = new List<string>();
            string currentStatus = string.Empty;
            DataContracts.PurchaseRequest purchaseRequest = new DataContracts.PurchaseRequest();
            PurchaseRequestWrapper pWrapper = new PurchaseRequestWrapper(userData);
            foreach (var item in model.list)
            {
                if (!string.IsNullOrEmpty(item.Status))
                {
                    currentStatus = item.Status.ToUpper();
                }
                if (!(currentStatus == PurchaseRequestStatusConstants.AwaitApproval.ToUpper()))
                {
                    failedPrList.Add(item.ClientLookupId);
                }
                else
                {
                    purchaseRequest = pWrapper.UpdateStatusBatch(item.PurchaseRequestId, PurchaseReturnStatusEnum.ReturnToRequester, model.comments);
                    if (purchaseRequest.ErrorMessages != null && purchaseRequest.ErrorMessages.Count > 0)
                    {
                        string errormessage = "Failed to return to requester " + purchaseRequest.ClientLookupId + ": " + purchaseRequest.ErrorMessages[0];
                        errMsgList.Add(errormessage);
                    }
                }
            }
            if (errMsgList.Count > 0 || failedPrList.Count > 0)
            {
                if (failedPrList.Count > 0)
                {
                    errMsgList.Add("Purchase Request(s) " + string.Join(", ", failedPrList) + " can't be returned to requester. Please check the status.");
                }
                return Json(new { data = errMsgList }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { data = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public JsonResult ConvertPRToPO(List<long> model)
        {
            PurchaseRequestWrapper pWrapper = new PurchaseRequestWrapper(userData);
            List<string> errMsgList = new List<string>();
            long[] PurchaseRequestIds = new long[model.Count];
            List<long> PrIds = new List<long>();
            List<ConvertToPOModel> ConvertToPurchaseOrderList = new List<ConvertToPOModel>();
            foreach (var item in model)
            {
                PrIds.Add(item);
            }
            if (PrIds.Count > 0)
            {
                PurchaseRequestIds = PrIds.ToArray();
                #region V2-929
                string errormsg = "Error";
                string errormsgAssetMgt = "ErrorAssetMgt";
                var ConvertToPurchaseOrderListChecking = pWrapper.ConvertPRToPO(PurchaseRequestIds);
                ConvertToPurchaseOrderList = ConvertToPurchaseOrderListChecking.Item1;
                if (ConvertToPurchaseOrderListChecking.Item2 == true)
                {
                    return Json(new { data = errormsg }, JsonRequestBehavior.AllowGet);
                }
                //V2-933
                if (ConvertToPurchaseOrderListChecking.Item3 == true)
                {
                    return Json(new { data = errormsgAssetMgt }, JsonRequestBehavior.AllowGet);
                }
                #endregion
                if (ConvertToPurchaseOrderList.Count > 0)
                {
                    foreach (var v in ConvertToPurchaseOrderList)
                    {
                        if (v.Message == Convert.ToString(JsonReturnEnum.failed))
                        {
                            errMsgList.Add("Purchase Request " + v.ClientLookupId + " failed to convert to Purchase Order");
                        }
                    }
                }
            }
            errMsgList.AddRange(SendPunchOutOrder(PurchaseRequestIds, ConvertToPurchaseOrderList));
            if (errMsgList.Count > 0)
            {
                return Json(new { data = errMsgList }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { data = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }
        private List<string> SendPunchOutOrder(long[] PurchaseRequestIds, List<ConvertToPOModel> ConvertToPurchaseOrderList)
        {
            VendorWrapper vWrapper = new VendorWrapper(userData);
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            SiteSetUpWrapper siteSetUpWrapper = new SiteSetUpWrapper(userData);
            PurchaseOrderWrapper pWrapper = new PurchaseOrderWrapper(userData);
            PurchaseRequestWrapper purchaseRequestWrapper = new PurchaseRequestWrapper(userData);
            PunchoutAPIResponse punchoutAPIResponse;
            List<string> errMsgList = new List<string>();

            Models.VendorsModel objVen;
            Personnel personnel;
            Site site;
            PurchaseRequestModel purchaseRequestModel;

            long ClientId = userData.DatabaseKey.Client.ClientId;
            long SiteId, PurchaseOrderId, VendorId, CreatedBy_PersonnelId;

            foreach (long purchaseRequestId in PurchaseRequestIds)
            {
                objVen = new Models.VendorsModel();
                personnel = new Personnel();
                site = new Site();
                purchaseRequestModel = new PurchaseRequestModel();
                punchoutAPIResponse = new PunchoutAPIResponse();

                purchaseRequestModel = purchaseRequestWrapper.GetPurchaseRequestDetailById(purchaseRequestId);
                if (purchaseRequestModel.IsPunchOut)
                {
                    SiteId = purchaseRequestModel.SiteId;
                    PurchaseOrderId = purchaseRequestModel.PurchaseOrderId;
                    VendorId = purchaseRequestModel.VendorId ?? 0;
                    CreatedBy_PersonnelId = purchaseRequestModel.CreatedBy_PersonnelId;

                    objVen = vWrapper.populateVendorDetails(VendorId);
                    int Count = ConvertToPurchaseOrderList.Where(x => x.ClientLookupId == purchaseRequestModel.ClientLookupId &&
                                        x.Message == Convert.ToString(JsonReturnEnum.success)).Count();
                    if (objVen.AutoSendPunchOutPO && Count > 0)
                    {
                        Task[] tasks = new Task[2];
                        tasks[0] = Task.Factory.StartNew(() => personnel = commonWrapper.GetPersonnelByPersonnelId(CreatedBy_PersonnelId));
                        tasks[1] = Task.Factory.StartNew(() => site = siteSetUpWrapper.RetriveSiteDetailsByClientAndSite(ClientId, SiteId));
                        Task.WaitAll(tasks);

                        if (!tasks[0].IsFaulted && tasks[0].IsCompleted && !tasks[1].IsFaulted && tasks[1].IsCompleted)
                        {
                            var destinationURL = objVen.SendPunchoutPOURL;
                            if (string.IsNullOrEmpty(destinationURL))
                            {
                                errMsgList.Add("Purchase Request " + purchaseRequestModel.ClientLookupId + " failed to Send Purchase Order as Vendor's Send Punchout PO URL can't be empty.");
                            }
                            else
                            {
                                var requestToSend = pWrapper.GetPunchoutOrderMessageData(PurchaseOrderId, objVen, personnel, site, SiteId);
                                punchoutAPIResponse = pWrapper.postXMLData(destinationURL, requestToSend);

                                if (punchoutAPIResponse.ResponseCode == 200 && punchoutAPIResponse.ResponseMessage.ToLower() == "ok")
                                {
                                    pWrapper.UpdatePOOnOrderSetupResponse(PurchaseOrderId, ClientId);
                                    SiteId = userData.DatabaseKey.User.DefaultSiteId;
                                    pWrapper.UpdatePOEventLogOnOrderSetupResponse(PurchaseOrderId, ClientId, SiteId);
                                }
                                else
                                {
                                    errMsgList.Add("Purchase Request " + purchaseRequestModel.ClientLookupId + " failed to Send Purchase Order - " + punchoutAPIResponse.ResponseText);
                                }
                            }

                        }
                    }
                }
            }
            return errMsgList;
        }

        [HttpPost]
        public async Task<JsonResult> SendToCoupaListPR(PurchaseRequestStatusModel model)
        {
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            PurchaseRequestWrapper pWrapper = new PurchaseRequestWrapper(userData);
            List<string> errMsgList = new List<string>();
            List<long> PrIds = new List<long>();
            List<string> ClientLookupIds = new List<string>();
            List<PurchaseRequestLocalModel> PrLocalModelList = new List<PurchaseRequestLocalModel>();
            PurchaseRequestLocalModel prLocalModel;
            foreach (var item in model.list)
            {
                prLocalModel = new PurchaseRequestLocalModel();
                prLocalModel.PurchaseRequestId = item.PurchaseRequestId;
                prLocalModel.ClientLookupId = item.ClientLookupId;
                PrLocalModelList.Add(prLocalModel);
            }
            if (PrLocalModelList.Count > 0)
            {
                InterfaceProp iprop = commonWrapper.RetrieveInterfaceProperties(ApiConstants.PurchaseRequestExport);
                string int_url = iprop.APIKey1;   // Coupa - dean foods - "https://deanfoods-test.coupahost.com/api/requisitions/submit_for_approval"
                string int_key = iprop.APIKey2;   // Coupa - RKL Acct   - "8b2c306ada338ccc50e8afb8249ed344168f7ee7"

                foreach (var pritem in PrLocalModelList)
                {
                    ////-------Formatting the data for submission to Coupa-------
                    var vprc = pWrapper.GetApprovalListPR(pritem.PurchaseRequestId);

                    ////-------Submission to Coupa-------
                    JsonSerializerSettings settings = new JsonSerializerSettings()
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                    };

                    string strJson = JsonConvert.SerializeObject(vprc, settings);
#if DEBUG
                    // Copy the file to a web server directory
                    bool export = false;
                    string out_dir = System.Web.HttpContext.Current.Server.MapPath("~/JsonArchive/");
                    export = Directory.Exists(out_dir);
                    Debug.Assert(export, out_dir);
                    if (!export)
                    {
                        out_dir = "D:/clients/Dean Foods/Interface/Output/";
                        export = Directory.Exists(out_dir);
                    }
                    Debug.Assert(export, out_dir);
                    if (export)
                    {
                        string file_output = out_dir + vprc.CustomFields.PRNumber + '_' + DateTime.Now.ToString("yyyy_MM_dd_hh_mm_ss") + ".json";
                        using (StreamWriter w = new StreamWriter(file_output))
                        {
                            w.Write(strJson);
                        }
                    }
#endif

                    if (strJson.Length > 0)
                    {
                        // V2-852
                        string coupa_token = string.Empty;
                        bool useToken = true;
                        HttpResponseMessage httpResponse = new HttpResponseMessage();
                        if (useToken == true)
                        {
                            if (pWrapper.RetrieveCoupaToken(iprop, ref coupa_token))
                            {
                                using (var httpClient = new HttpClient())
                                {
                                    using (var request = new HttpRequestMessage(new HttpMethod("POST"), int_url))
                                    {
                                        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", coupa_token);
                                        request.Headers.TryAddWithoutValidation("Accept", "application/json");
                                        request.Content = new StringContent(strJson, System.Text.Encoding.UTF8, "application/json");
                                        httpResponse = await httpClient.SendAsync(request);
                                    }
                                }
                            }
                            else // Retrieve Coupa Token 
                            {
                                string resultJSON = "Error retrieving coupa token";
                                errMsgList.Add(pritem.ClientLookupId + " failed :" + resultJSON);
                            }
                        }
                        else  // Do Not Use Token
                        {
                            using (var httpClient = new HttpClient())
                            {
                                //using (var request = new HttpRequestMessage(new HttpMethod("POST"), "https://deanfoods-test.coupahost.com/api/requisitions/submit_for_approval"))
                                using (var request = new HttpRequestMessage(new HttpMethod("POST"), int_url))
                                {
                                    request.Headers.TryAddWithoutValidation("Accept", "application/json");
                                    request.Headers.TryAddWithoutValidation("X-COUPA-API-KEY", int_key);
                                    request.Content = new StringContent(strJson, System.Text.Encoding.UTF8, "application/json");
                                    httpResponse = await httpClient.SendAsync(request);
                                }
                            }
                        }
                        if (errMsgList.Count == 0)
                        {
                            if (httpResponse.IsSuccessStatusCode)
                            {
                                string resultJSON = httpResponse.Content.ReadAsStringAsync().Result;
                                // RKL - use the constant
                                // Retrieve the external request id from the returned data
                                // This may take a while - have to have a model for the returned JSON
                                //var errList = pWrapper.UpdatePrStatus(pritem.PurchaseRequestId, "Extracted", 101);
#if DEBUG
                                // Copy the response 
                                string out_dir2 = "D:/clients/Dean Foods/Interface/Output";
                                if (Directory.Exists(out_dir2))
                                {
                                    string file_output = "D:/clients/Dean Foods/Interface/Output/PR_Coupa_Return_" + vprc.CustomFields.PRNumber + '_' + DateTime.Now.ToString("yyyy_MM_dd_hh_mm_ss") + ".json";
                                    using (StreamWriter w = new StreamWriter(file_output))
                                    {
                                        w.Write(resultJSON);
                                    }
                                }
#endif
                                var errList = pWrapper.UpdatePrStatus(pritem.PurchaseRequestId,
                                      PurchaseRequestStatusConstants.Extracted,
                                      101,
                                      PurchasingEvents.SentToCoupa);
                                var erstr = string.Empty;
                                if (errList != null && errList.Count > 0)
                                {
                                    foreach (var v in errList)
                                    {
                                        erstr += v + ",";
                                    }
                                    errMsgList.Add(pritem.ClientLookupId + " failed to update status :" + erstr);
                                }
                            }
                            else // http response
                            {
                                string resultJSON = httpResponse.Content.ReadAsStringAsync().Result;
                                errMsgList.Add(pritem.ClientLookupId + " failed :" + resultJSON);
                            }
                        }
                    } // strJson.length > 0
                } // for each
            }
            if (errMsgList.Count > 0)
            {
                return Json(new { data = errMsgList }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { data = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Send Punchout Request
        [HttpPost]
        public JsonResult CreatePunchOut(long PurchaseRequestId, long ClientId, long SiteId, long CreatedBy_PersonnelId, long VendorId)
        {
            VendorWrapper vWrapper = new VendorWrapper(userData);
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            SiteSetUpWrapper siteSetUpWrapper = new SiteSetUpWrapper(userData);
            PurchaseRequestWrapper pWrapper = new PurchaseRequestWrapper(userData);
            Models.VendorsModel objVen = new Models.VendorsModel();
            Personnel personnel = new Personnel();
            Site site = new Site();
            PunchoutAPIResponse punchoutAPIResponse = new PunchoutAPIResponse();

            Task[] tasks = new Task[3];
            tasks[0] = Task.Factory.StartNew(() => objVen = vWrapper.populateVendorDetails(VendorId));
            tasks[1] = Task.Factory.StartNew(() => personnel = commonWrapper.GetPersonnelByPersonnelId(CreatedBy_PersonnelId));
            tasks[2] = Task.Factory.StartNew(() => site = siteSetUpWrapper.RetriveSiteDetailsByClientAndSite(ClientId, SiteId));
            Task.WaitAll(tasks);

            if (!tasks[0].IsFaulted && tasks[0].IsCompleted && !tasks[1].IsFaulted && tasks[1].IsCompleted && !tasks[2].IsFaulted && tasks[2].IsCompleted)
            {
                if (string.IsNullOrEmpty(objVen.PunchoutURL))
                {
                    punchoutAPIResponse.ResponseCode = 422;
                    punchoutAPIResponse.ResponseMessage = ErrorMessageConstants.Vendor_Punchout_URL_Not_Empty;
                }
                else
                {
                    var requestToSend = pWrapper.GetSetUpRequestData(PurchaseRequestId, objVen, personnel, site);
                    var destinationURL = objVen.PunchoutURL;
                    punchoutAPIResponse = pWrapper.postXMLData(destinationURL, requestToSend);
                }

            }
            return Json(punchoutAPIResponse, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        [SkipSessionExpiaryActionFilter]
        public ActionResult PRCheckOut()
        {
            PurchaseRequestModel purchaseRequestModel = new PurchaseRequestModel();
            List<ShoppingCartImportDataModel> shoppingList = new List<ShoppingCartImportDataModel>();
            var requestContent = GetRequestContentAsString();
            string xml = System.Web.HttpUtility.UrlDecode(requestContent, System.Text.Encoding.UTF8);
            var document = new Models.PunchoutImport.PunchOutOrderMessageResponse();
            if (requestContent.StartsWith("cxml-base64="))
            {
              document = XmlHelper.XmlDeserializeFromStringBase64<Models.PunchoutImport.PunchOutOrderMessageResponse>(xml.Remove(0, 12));
            }
            else
            {
              document = XmlHelper.XmlDeserializeFromString<Models.PunchoutImport.PunchOutOrderMessageResponse>(xml.Remove(0, 16));
            }
            /*
            string encoding_type = requestContent.Substring(0,11);
            if(encoding_type == "cxml-base64")
            {
              string test2 = requestContent.Decrypt();
              string test = requestContent.Remove(0,12);
              xml = System.Web.HttpUtility.UrlDecode(requestContent, System.Text.Encoding.UTF8).Remove(0,12);
              //var valueBytes = System.Convert.FromBase64String(test);
              //xml = System.Text.Encoding.UTF8.GetString(valueBytes);
            }
            else
            { 
              xml = System.Web.HttpUtility.UrlDecode(requestContent, System.Text.Encoding.UTF8).Remove(0,16);
            }
            var document = XmlHelper.XmlDeserializeFromString<Models.PunchoutImport.PunchOutOrderMessageResponse>(xml);
            //var document = XmlHelper.XmlDeserializeFromString<Models.PunchoutImport.PunchOutOrderMessageResponse>(xml.Remove(0, 16));
            */
            if (CheckLoginSession(document.Message.PunchOutOrderMessage.BuyerCookie.Substring(0, 36)))
            {
                PurchaseRequestWrapper purchaseRequestWrapper = new PurchaseRequestWrapper(userData);
                shoppingList = purchaseRequestWrapper.ImportShoppingCart_ToDataModel(document);

                long PRid = Convert.ToInt64(document.Message.PunchOutOrderMessage.BuyerCookie.Substring(36));
                purchaseRequestModel = purchaseRequestWrapper.GetPurchaseRequestDetailById(PRid);
                string PRClientlookupid = purchaseRequestModel.ClientLookupId.ToString();
                string Status = purchaseRequestModel.Status.ToString();
                foreach (var item in shoppingList)
                {
                    item.ClientId = userData.DatabaseKey.Client.ClientId;
                    item.SiteId = userData.Site.SiteId;
                    item.PurchaseRequestId = PRid;
                    item.ClientLookupId = PRClientlookupid;
                    item.Status = Status;
                }
                Session["SHOPPINGLIST"] = shoppingList;
                Session["userData"] = userData;
                var menuReturnList = LogInController.GetMenuList("site");
                if (!(menuReturnList.Count == 0 || menuReturnList.Count < 0))
                {
                    Session["MenuDetails"] = menuReturnList;
                }
                TempData["Mode"] = "shoppingcartcheckout";
                return Redirect("/PurchaseRequest/Index?page=Procurement_Requests");
            }
            return null;
        }
        #region RKL Mail for close the vendor punchout website and send user back to the original tab
        public RedirectResult RedirectForShoppingCart()
        {
            TempData["Mode"] = "shoppingcartcheckouttab";
            return Redirect("/PurchaseRequest/Index?page=Procurement_Requests");
        }
        #endregion
        private string GetRequestContentAsString()
        {
            string str;
            using (var receiveStream = Request.InputStream)
            {
                using (var readStream = new StreamReader(receiveStream, System.Text.Encoding.UTF8))
                {
                    str = readStream.ReadToEnd();
                }
            }

            return str;
        }

        // This method adds line items to a purchase request line items and validates the shopping cart data.
        public JsonResult AddLineItems(long PurchaseRequestId, string Status, List<PRShoppingcartModel> ShoppingCartData)
        {
            PurchaseRequestWrapper pWrapper = new PurchaseRequestWrapper(userData);
            string ModelValidationFailedMessage = string.Empty;
            var IsValidPartId = false;
            var IsValidUnitofMeasure = false;
            var IsValidRequiredDate = false;
            var IsValidAccount = false;
            var IsValidCategory = false;

            // Validate the shopping cart data and return the validation result
            if (!ValidateStockOrDirectBuy(ShoppingCartData, ref IsValidPartId, ref IsValidUnitofMeasure, ref IsValidRequiredDate, ref IsValidAccount, ref IsValidCategory))
            {
                return Json(new { Result = JsonReturnEnum.failed.ToString(), Mode = "Validation", IsValidPartId = IsValidPartId, IsValidUnitofMeasure = IsValidUnitofMeasure, IsValidRequiredDate = IsValidRequiredDate, IsValidAccount = IsValidAccount, IsValidCategory = IsValidCategory }, JsonRequestBehavior.AllowGet);
            }

            // Insert the validated shopping cart data into the purchase request line item
            var result = pWrapper.InsertPartShoppingCartPurchaseRequest(PurchaseRequestId, Status, ShoppingCartData);
            if (result != null && result.Count == 0)
            {
                return Json(new { Result = JsonReturnEnum.success.ToString(), purchaserequestid = PurchaseRequestId, Mode = "Add" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Result = JsonReturnEnum.failed.ToString(), purchaserequestid = PurchaseRequestId, Mode = "Add" }, JsonRequestBehavior.AllowGet);
            }
        }
        private bool ValidateStockOrDirectBuy(List<PRShoppingcartModel> ShoppingCartData, ref bool IsValidPartId, ref bool IsValidUnitofMeasure, ref bool IsValidRequiredDate, ref bool IsValidAccount, ref bool IsValidCategory)
        {
            // Validate the shopping cart data
            bool IsValid = true;
            // Check if Oracle Purchase Request Export is in use
            bool OraclePurchaseRequestExportInUse = false;
            LoginWrapper loginWrapper = new LoginWrapper();
            var isUsePartMaster = this.userData.Site.UsePartMaster;
            var InterfacePropData = loginWrapper.RetrieveAllInterfacePropertiesByClientIdSiteId();
            if (InterfacePropData != null && InterfacePropData.Count > 0)
            {
                // Determine if Oracle Purchase Request Export is in use based on interface properties
                OraclePurchaseRequestExportInUse = InterfacePropData.Where(x => x.InterfaceType == ApiConstants.OraclePurchaseRequestExport).Select(x => x.InUse).FirstOrDefault();
            }
            foreach (var item in ShoppingCartData)
            {
                // Check if PartId and ChargeToID are both zero or both greater than zero
                if (item.PartId == 0 && item.ChargeToID == 0)
                {
                    IsValid = false;
                    IsValidPartId = true;
                }
                else if (item.PartId > 0 && item.ChargeToID > 0)
                {
                    IsValid = false;
                    IsValidPartId = true;
                }
                // Check if Unit of Measure is empty
                if (string.IsNullOrEmpty(item.UnitofMeasure))
                {
                    IsValid = false;
                    IsValidUnitofMeasure = true;
                }
                else if (OraclePurchaseRequestExportInUse == true)
                {
                    if (item.PartId > 0)
                    {
                        IsValid = ValidateRequiredDate(item.RequiredDate);
                        IsValidRequiredDate = true;
                    }
                    else
                    {
                        // Check if AccountId is zero
                        if (item.AccountId == 0)
                        {
                            IsValid = false;
                            IsValidAccount = true;
                        }
                        // Check if UNSPSC is zero when PartMaster is in use
                        if (isUsePartMaster == true)
                        {
                            if (item.UNSPSC == 0)
                            {
                                IsValid = false;
                                IsValidCategory = true;
                            }
                        }
                        if (ValidateRequiredDate(item.RequiredDate) == false)
                        {
                            IsValid = false;
                            IsValidRequiredDate = true;
                        }
                    }
                }
            }
            return IsValid;
        }
        //Validate the RequiredDate
        private bool ValidateRequiredDate(DateTime? requiredDate)
        {
            if (!requiredDate.HasValue)
            {
                // RequiredDate is not selected/entered
                return false;
            }
            if (requiredDate.Value <= DateTime.Now)
            {
                // RequiredDate is not in the future
                return false;
            }
            // RequiredDate is selected/entered and is in the future
            return true;
        }
        #endregion

        #region Redirect Details From Part
        public RedirectResult DetailFromPart(long PurchaseRequestId, string Status)
        {
            TempData["Mode"] = "DetailFromPart";
            string PRString = Convert.ToString(PurchaseRequestId);
            TempData["PurchaseRequestId"] = PRString;
            TempData["Status"] = Status;
            return Redirect("/PurchaseRequest/Index?page=Procurement_Requests");
        }
        #endregion
        #region Redirect Details From Purchase Order
        public RedirectResult DetailFromPurchaseOrder(long PurchaseRequestId)
        {
            TempData["Mode"] = "DetailFromPurchaseOrder";
            string PRString = Convert.ToString(PurchaseRequestId);
            TempData["PurchaseRequestId"] = PRString;
            return Redirect("/PurchaseRequest/Index?page=Procurement_Requests");
        }
        #endregion
        #region Redirect Details From Menu
        public RedirectResult DetailFromPlusMenu(long PurchaseRequestId)
        {
            TempData["Mode"] = "DetailFromPlusMenu";
            string PRString = Convert.ToString(PurchaseRequestId);
            TempData["PurchaseRequestId"] = PRString;
            return Redirect("/PurchaseRequest/Index?page=Procurement_Requests");
        }
        #endregion
        #region V2-1147
        public RedirectResult DetailFromNotification(long PurchaseRequestId, string alertName)
        {
            TempData["Mode"] = "DetailFromNotification";
            TempData["alertName"] = alertName;
            string strPurchaseRequestId = Convert.ToString(PurchaseRequestId);
            TempData["PurchaseRequestId"] = strPurchaseRequestId;
            return Redirect("/PurchaseRequest/Index?page=Procurement_Requests");

        }
        #endregion
        #region V2-563
        [HttpPost]
        public JsonResult AddPurchaseRequestfromAdditionalCat(PurchaseRequestModel purchaseRequestModel)
        {
            string ErrorMessage = string.Empty;

            DataContracts.PurchaseRequest pRequest = new DataContracts.PurchaseRequest();
            PurchaseRequestModel objPurchaseRequestModel = new PurchaseRequestModel();
            PurchaseRequestWrapper pWrapper = new PurchaseRequestWrapper(userData);
            if (purchaseRequestModel.VendorId != 0)
            {
                pRequest = pWrapper.AddPurchaseRequest(purchaseRequestModel);
                if (pRequest.ErrorMessages != null && pRequest.ErrorMessages.Count > 0)
                {
                    return Json(pRequest.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), purchaserequestid = pRequest.PurchaseRequestId, PRClientlookupId = pRequest.ClientLookupId }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                ErrorMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ErrorMessage, JsonRequestBehavior.AllowGet);
            }

        }

        #endregion
        #region V2-653 Add Purchase Request Dynamic
        [HttpGet]
        public ActionResult ShowAddPurchaseRequestDynamic()
        {
            PurchaseRequestVM objPurchaseRequestVM = new PurchaseRequestVM();
            PurchaseRequestWrapper pWrapper = new PurchaseRequestWrapper(userData);
            objPurchaseRequestVM.UIConfigurationDetails = new RetrieveDataForUIConfiguration()
                                           .Retrieve(DataDictionaryViewNameConstant.AddPurchaseRequest, userData);
            objPurchaseRequestVM.AddPurchaseRequest = new Models.PurchaseRequest.UIConfiguration.AddPurchaseRequestModelDynamic();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            List<DataContracts.LookupList> AllLookUps = new List<DataContracts.LookupList>();
            AllLookUps = commonWrapper.GetAllLookUpList();
            IList<string> LookupNames = objPurchaseRequestVM.UIConfigurationDetails.ToList()
                                            .Where(x => x.LookupType == "LookupList" && !string.IsNullOrEmpty(x.LookupName))
                                            .Select(s => s.LookupName)
                                            .ToList();
            if (AllLookUps != null)
            {
                objPurchaseRequestVM.AllRequiredLookUplist = AllLookUps.Where(x => LookupNames.Contains(x.ListName))
                                                          .Select(s => new Models.UILookupList
                                                          { text = s.ListValue + " - " + s.Description, value = s.ListValue, lookupname = s.ListName })
                                                          .ToList();
            }
            //V2-738
            if (userData.DatabaseKey.Client.UseMultiStoreroom)
            {
                objPurchaseRequestVM.StoreroomList = commonWrapper.GetStoreroomList(StoreroomAuthType.Issue);
            }
            objPurchaseRequestVM.udata = userData;
            LocalizeControls(objPurchaseRequestVM, LocalizeResourceSetConstants.PurchaseRequest);
            return PartialView("_PurchaseRequestAddDynamic", objPurchaseRequestVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddPurchaseRequestDynamic(PurchaseRequestVM objVM, string Command)
        {
            DataContracts.PurchaseRequest pRequest = new DataContracts.PurchaseRequest();
            PurchaseRequestWrapper pWrapper = new PurchaseRequestWrapper(userData);
            string ModelValidationFailedMessage = string.Empty;

            if (ModelState.IsValid)
            {
                //pRequest = pWrapper.AddPurchaseRequestDynamic(objVM);
                #region V2-929 
                var pRequestAdd = pWrapper.AddPurchaseRequestDynamic(objVM);
                string errormsg = "Error";
                string errormsgAssetMgt = "ErrorAssetMgt";
                pRequest = pRequestAdd.Item1;

                if (pRequestAdd.Item2 == true)
                {
                    return Json(new { Result = errormsg }, JsonRequestBehavior.AllowGet);
                }
                //V2-933
                if (pRequestAdd.Item3 == true)
                {
                    return Json(new { Result = errormsgAssetMgt }, JsonRequestBehavior.AllowGet);
                }
                #endregion
                if (pRequest.ErrorMessages != null && pRequest.ErrorMessages.Count > 0)
                {
                    return Json(pRequest.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), Command = Command, purchaserequestid = pRequest.PurchaseRequestId }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region V2-653 Edit Purchase Request Dynamic
        public PartialViewResult EditPurchaseRequestDynamic(long PurchaseRequestId)
        {
            PurchaseRequestVM objPurchaseRequestVM = new PurchaseRequestVM();
            PurchaseRequestWrapper pWrapper = new PurchaseRequestWrapper(userData);
            objPurchaseRequestVM.EditPurchaseRequest = pWrapper.GetPurchaseRequestByIdDynamic(PurchaseRequestId);
            objPurchaseRequestVM.EditPurchaseRequest.ViewName = UiConfigConstants.PurchaseRequestEdit;
            objPurchaseRequestVM.security = this.userData.Security;
            objPurchaseRequestVM.UIConfigurationDetails = new RetrieveDataForUIConfiguration()
                               .Retrieve(DataDictionaryViewNameConstant.EditPurchaseRequest, userData);
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            List<DataContracts.LookupList> AllLookUps = new List<DataContracts.LookupList>();
            AllLookUps = commonWrapper.GetAllLookUpList();
            IList<string> LookupNames = objPurchaseRequestVM.UIConfigurationDetails.ToList()
                                            .Where(x => x.LookupType == "LookupList" && !string.IsNullOrEmpty(x.LookupName))
                                            .Select(s => s.LookupName)
                                            .ToList();
            if (AllLookUps != null)
            {
                objPurchaseRequestVM.AllRequiredLookUplist = AllLookUps.Where(x => LookupNames.Contains(x.ListName))
                                                          .Select(s => new Models.UILookupList
                                                          { text = s.ListValue + " - " + s.Description, value = s.ListValue, lookupname = s.ListName })
                                                          .ToList();
            }
            //V2-738
            if (userData.DatabaseKey.Client.UseMultiStoreroom)
            {
                objPurchaseRequestVM.StoreroomList = commonWrapper.GetStoreroomList(StoreroomAuthType.Issue);
            }
            objPurchaseRequestVM.udata = userData;
            LocalizeControls(objPurchaseRequestVM, LocalizeResourceSetConstants.PurchaseRequest);
            return PartialView("_PurchaseRequestEditDynamic", objPurchaseRequestVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult EditPurchaseRequestDynamic(PurchaseRequestVM objVM, string Command)
        {
            string ModelValidationFailedMessage = string.Empty;

            if (ModelState.IsValid)
            {
                DataContracts.PurchaseRequest pRequest = new DataContracts.PurchaseRequest();
                PurchaseRequestWrapper pWrapper = new PurchaseRequestWrapper(userData);
                pRequest = pWrapper.EditPurchaseRequestDynamic(objVM);
                if (pRequest.ErrorMessages != null && pRequest.ErrorMessages.Count > 0)
                {
                    return Json(pRequest.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), Command = Command, purchaserequestid = pRequest.PurchaseRequestId }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region V2-653 Add Part Not in inventory (PR Line Item)
        [HttpGet]
        public ActionResult ShowAddPartNotInInventoryDynamic(long PurchaseRequestId, string ClientLookupId, String Status)
        {
            PurchaseRequestVM objPurchaseRequestVM = new PurchaseRequestVM();
            PurchaseRequestWrapper pWrapper = new PurchaseRequestWrapper(userData);
            PurchaseRequestModel objPurchaseRequestModel = new PurchaseRequestModel();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            objPurchaseRequestVM.AddPRLineItemPartNotInInventory = new Models.PurchaseRequest.UIConfiguration.AddPRLineItemPartNotInInventoryModelDynamic();
            objPurchaseRequestVM.UIConfigurationDetails = new RetrieveDataForUIConfiguration()
                                            .Retrieve(DataDictionaryViewNameConstant.AddPurchaseRequestLineItemPartNotInInventory, userData);
            //V2-907
            var ChargeTypeList = UtilityFunction.populateChargeTypeForPurchaseRequestLineItem();
            if (ChargeTypeList != null)
            {
                objPurchaseRequestVM.ChargeTypeList = ChargeTypeList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
            }
            objPurchaseRequestVM.AddPRLineItemPartNotInInventory.AccountId = userData.Site.NonStockAccountId;//RNJ
            var AcclookUpList = GetAccountByActiveState(true);
            if (AcclookUpList != null && userData.Site.NonStockAccountId > 0)
            {
                AcclookUpList = AcclookUpList.Where(x => x.AccountId == userData.Site.NonStockAccountId).ToList();
                if (AcclookUpList != null)
                {
                    objPurchaseRequestVM.AddPRLineItemPartNotInInventory.AccountClientLookupId = AcclookUpList.Select(x => new SelectListItem { Text = x.Account + " - " + x.Name, Value = x.AccountId.ToString() }).Where(x => x.Value == userData.Site.NonStockAccountId.ToString()).First().Text;
                }
                else
                {
                    objPurchaseRequestVM.AddPRLineItemPartNotInInventory.AccountClientLookupId = "";
                }
            }
            else
            {
                objPurchaseRequestVM.AddPRLineItemPartNotInInventory.AccountClientLookupId = "";
            }
            var AllLookUps = commonWrapper.GetAllLookUpList();
            IList<string> LookupNames = LookupNames = objPurchaseRequestVM.UIConfigurationDetails.ToList()
                                        .Where(x => x.LookupType == "LookupList" && !string.IsNullOrEmpty(x.LookupName))
                                        .Select(s => s.LookupName)
                                        .ToList();
            if (AllLookUps != null)
            {
                objPurchaseRequestVM.AllRequiredLookUplist = AllLookUps.Where(x => LookupNames.Contains(x.ListName))
                                                      .Select(s => new Client.Models.UILookupList
                                                      { text = s.ListValue + " - " + s.Description, value = s.ListValue, lookupname = s.ListName })
                                                      .ToList();
            }
            objPurchaseRequestVM.AddPRLineItemPartNotInInventory.PurchaseRequestId = PurchaseRequestId;
            objPurchaseRequestVM.AddPRLineItemPartNotInInventory.ViewName = UiConfigConstants.PurchaseRequestLineItemAdd;
            objPurchaseRequestVM.AddPRLineItemPartNotInInventory.ClientLookupId = ClientLookupId;
            objPurchaseRequestVM.AddPRLineItemPartNotInInventory.IsShopingCart = userData.Site.ShoppingCart; //V2-563
            if (objPurchaseRequestVM.AddPRLineItemPartNotInInventory.IsShopingCart)
            {
                objPurchaseRequestVM.AddPRLineItemPartNotInInventory.RequiredDate = DateTime.Today.AddDays(7);
            }
            objPurchaseRequestVM.AddPRLineItemPartNotInInventory.Status = Status;
            LocalizeControls(objPurchaseRequestVM, LocalizeResourceSetConstants.PurchaseRequest);
            return PartialView("_AddPartNotInInventoryDynamic", objPurchaseRequestVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddPartNotInInventoryDynamic(PurchaseRequestVM objVM, string Command)
        {
            DataContracts.PurchaseRequestLineItem prLineItem = new DataContracts.PurchaseRequestLineItem();
            PurchaseRequestWrapper pWrapper = new PurchaseRequestWrapper(userData);
            string ModelValidationFailedMessage = string.Empty;

            if (ModelState.IsValid)
            {
                prLineItem = pWrapper.AddPartNotInInventoryDynamic(objVM);
                if (prLineItem.ErrorMessages != null && prLineItem.ErrorMessages.Count > 0)
                {
                    return Json(prLineItem.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), Command = Command, purchaserequestid = objVM.AddPRLineItemPartNotInInventory.PurchaseRequestId }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region V2-653 Edit Part in inventory (PR Line Item)
        public ActionResult EditPRPartInInventoryDynamic(long LineItemId, long PurchaseRequestId, string ClientLookupId, string status, long StoreroomId = 0)
        {
            PurchaseRequestVM objPurchaseRequestVM = new PurchaseRequestVM();
            PurchaseRequestWrapper pWrapper = new PurchaseRequestWrapper(userData);
            objPurchaseRequestVM.EditPRLineItemPartInInventory = pWrapper.GetPRLineItemInInventoryByIdDynamic(LineItemId, PurchaseRequestId);
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            objPurchaseRequestVM.UIConfigurationDetails = new RetrieveDataForUIConfiguration()
       .Retrieve(DataDictionaryViewNameConstant.EditPurchaseRequestLineItemPartInInventory, userData);
            var AllLookUps = commonWrapper.GetAllLookUpList();
            IList<string> LookupNames = LookupNames = objPurchaseRequestVM.UIConfigurationDetails.ToList()
                                        .Where(x => x.LookupType == "LookupList" && !string.IsNullOrEmpty(x.LookupName))
                                        .Select(s => s.LookupName)
                                        .ToList();
            if (AllLookUps != null)
            {
                objPurchaseRequestVM.AllRequiredLookUplist = AllLookUps.Where(x => LookupNames.Contains(x.ListName))
                                                      .Select(s => new Client.Models.UILookupList
                                                      { text = s.ListValue + " - " + s.Description, value = s.ListValue, lookupname = s.ListName })
                                                      .ToList();
            }
            objPurchaseRequestVM.EditPRLineItemPartInInventory.ViewName = UiConfigConstants.PurchaseRequestLineItemEdit;
            objPurchaseRequestVM.EditPRLineItemPartInInventory.ClientLookupId = ClientLookupId;
            objPurchaseRequestVM.EditPRLineItemPartInInventory.ChargeToClientLookupIdToShow = objPurchaseRequestVM.EditPRLineItemPartInInventory.ChargeToClientLookupId;
            objPurchaseRequestVM.EditPRLineItemPartInInventory.IsShopingCart = userData.Site.ShoppingCart;
            objPurchaseRequestVM.EditPRLineItemPartInInventory.Status = status;
            LocalizeControls(objPurchaseRequestVM, LocalizeResourceSetConstants.PurchaseRequest);
            return PartialView("_EditPartInInventoryDynamic", objPurchaseRequestVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPRPartInInventoryDynamic(PurchaseRequestVM PurchaseRequestVM)
        {
            string ModelValidationFailedMessage = string.Empty;
            PurchaseRequestWrapper pWrapper = new PurchaseRequestWrapper(userData);
            ModelState.Remove("EditPRLineItemPartInInventory.ChargeToClientLookupIdToShow");
            if (ModelState.IsValid)
            {
                var lineItem = pWrapper.UpdatePRPartInInventoryDynamic(PurchaseRequestVM);
                if (lineItem.ErrorMessages.Count == 0)
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), purchaserequestid = PurchaseRequestVM.EditPRLineItemPartInInventory.PurchaseRequestId }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(lineItem.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }

        }
        #endregion

        #region V2-653 Edit Part Not in inventory (PR Line Item)
        public ActionResult EditPRPartNotInInventoryDynamic(long LineItemId, long PurchaseRequestId, string ClientLookupId, string status)
        {
            PurchaseRequestVM objPurchaseRequestVM = new PurchaseRequestVM();
            PurchaseRequestWrapper pWrapper = new PurchaseRequestWrapper(userData);
            objPurchaseRequestVM.EditPRLineItemPartNotInInventory = pWrapper.GetPRLineItemNotInInventoryByIdDynamic(LineItemId, PurchaseRequestId);
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            objPurchaseRequestVM.UIConfigurationDetails = new RetrieveDataForUIConfiguration()
       .Retrieve(DataDictionaryViewNameConstant.EditPurchaseRequestLineItemPartNotInInventory, userData);
            List<DataContracts.LookupList> UOMs = new List<DataContracts.LookupList>();
            //V2-907
            var ScheduleChargeTypeList = UtilityFunction.populateChargeTypeForPurchaseRequestLineItem();
            if (ScheduleChargeTypeList != null)
            {
                objPurchaseRequestVM.ChargeTypeList = ScheduleChargeTypeList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
            }
            var AllLookUps = commonWrapper.GetAllLookUpList();
            IList<string> LookupNames = LookupNames = objPurchaseRequestVM.UIConfigurationDetails.ToList()
                                        .Where(x => x.LookupType == "LookupList" && !string.IsNullOrEmpty(x.LookupName))
                                        .Select(s => s.LookupName)
                                        .ToList();
            if (AllLookUps != null)
            {
                objPurchaseRequestVM.AllRequiredLookUplist = AllLookUps.Where(x => LookupNames.Contains(x.ListName))
                                                      .Select(s => new Client.Models.UILookupList
                                                      { text = s.ListValue + " - " + s.Description, value = s.ListValue, lookupname = s.ListName })
                                                      .ToList();
            }
            objPurchaseRequestVM.EditPRLineItemPartNotInInventory.ViewName = UiConfigConstants.PurchaseRequestLineItemEdit;  //--V2-375--uiconfig//
            objPurchaseRequestVM.EditPRLineItemPartNotInInventory.ClientLookupId = ClientLookupId;
            objPurchaseRequestVM.EditPRLineItemPartNotInInventory.ChargeToClientLookupIdToShow = objPurchaseRequestVM.EditPRLineItemPartNotInInventory.ChargeToClientLookupId;
            objPurchaseRequestVM.EditPRLineItemPartNotInInventory.IsShopingCart = userData.Site.ShoppingCart; //V2-563
            objPurchaseRequestVM.EditPRLineItemPartNotInInventory.Status = status;

            LocalizeControls(objPurchaseRequestVM, LocalizeResourceSetConstants.PurchaseRequest);
            return PartialView("_EditPartNotInInventoryDynamic", objPurchaseRequestVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPRPartNotInInventoryDynamic(PurchaseRequestVM PurchaseRequestVM)
        {
            string ModelValidationFailedMessage = string.Empty;
            PurchaseRequestWrapper pWrapper = new PurchaseRequestWrapper(userData);
            ModelState.Remove("EditPRLineItemPartInInventory.ChargeToClientLookupIdToShow");
            if (ModelState.IsValid)
            {
                var lineItem = pWrapper.UpdatePRPartNotInInventoryDynamic(PurchaseRequestVM);
                if (lineItem.ErrorMessages.Count == 0)
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), purchaserequestid = PurchaseRequestVM.EditPRLineItemPartNotInInventory.PurchaseRequestId }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(lineItem.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }

        }
        #endregion

        #region V2-726
        public ApprovalRouteModel SendPRForApproval(long PurchaseRequestId, bool BuyerReview)
        {
            PurchaseRequestVM objPRVM = new PurchaseRequestVM();

            ApprovalRouteModel approvalRouteModel = new ApprovalRouteModel();
            var dataModels = Get_ApproverList(this.userData.DatabaseKey.Personnel.PersonnelId, ApprovalGroupRequestTypes.PurchaseRequest, 1);
            var approvers = new List<SelectListItem>();
            PurchaseRequestWrapper purchaseRequestWrapper = new PurchaseRequestWrapper(userData);//V2-820

            if (this.userData.DatabaseKey.ApprovalGroupSettings.PurchaseRequests)//V2-804
            {
                if (dataModels.Count > 0)
                {
                    approvers = dataModels.Select(x => new SelectListItem
                    {
                        Text = x.ApproverName,
                        Value = x.ApproverId.ToString()
                    }).ToList();
                }
                else
                {
                    #region V2-820 PurchaseRequest-Approve or PurchaseRequest-Review
                    if ((userData.Site.IncludePRReview) && userData.Site.ShoppingCartIncludeBuyer == false)
                    {
                        dataModels = purchaseRequestWrapper.Get_PRApprovedPersonnelListBy();

                        approvers = dataModels.Select(x => new SelectListItem
                        {
                            Text = x.NameFirst + " " + x.NameLast,
                            Value = x.AssignedTo_PersonnelId.ToString()
                        }).ToList();
                    }
                    #endregion
                    else
                    {
                        if (userData.Site.ShoppingCartIncludeBuyer && BuyerReview == false)
                        {
                            dataModels = purchaseRequestWrapper.Get_PRApprovedPersonnelListBy(true);
                        }
                        else
                        {
                            var securityName = SecurityConstants.PurchaseRequest_Approve;
                            var ItemAccess = "ItemAccess";
                            dataModels = Get_PersonnelList(securityName, ItemAccess);
                        }
                        approvers = dataModels.Select(x => new SelectListItem
                        {
                            Text = x.NameFirst + " " + x.NameLast,
                            Value = x.AssignedTo_PersonnelId.ToString()
                        }).ToList();

                    }

                }
            }
            else
            {
                #region V2-820 PurchaseRequest-Approve or PurchaseRequest-Review
                if ((userData.Site.IncludePRReview) && userData.Site.ShoppingCartIncludeBuyer == false)
                {
                    dataModels = purchaseRequestWrapper.Get_PRApprovedPersonnelListBy();

                    approvers = dataModels.Select(x => new SelectListItem
                    {
                        Text = x.NameFirst + " " + x.NameLast,
                        Value = x.AssignedTo_PersonnelId.ToString()
                    }).ToList();
                }
                #endregion
                else
                {
                    //V2-804
                    if (userData.Site.ShoppingCartIncludeBuyer && BuyerReview == false)
                    {
                        dataModels = purchaseRequestWrapper.Get_PRApprovedPersonnelListBy(true);
                    }
                    else
                    {
                        var securityName = SecurityConstants.PurchaseRequest_Approve;
                        var ItemAccess = "ItemAccess";
                        dataModels = Get_PersonnelList(securityName, ItemAccess);
                    }
                    approvers = dataModels.Select(x => new SelectListItem
                    {
                        Text = x.NameFirst + " " + x.NameLast,
                        Value = x.AssignedTo_PersonnelId.ToString()
                    }).ToList();

                }
            }
            approvalRouteModel.ApproverList = approvers;
            approvalRouteModel.ApproverCount = approvers.Count;
            approvalRouteModel.ObjectId = PurchaseRequestId;

            objPRVM.ApprovalRouteModel = approvalRouteModel;
            LocalizeControls(objPRVM, LocalizeResourceSetConstants.PurchaseRequest);
            return approvalRouteModel;
        }
        [HttpPost]
        public JsonResult SendPRForApprovalSave(PurchaseRequestVM prVM)
        {
            PurchaseRequestWrapper woWrapper = new PurchaseRequestWrapper(userData);
            if (ModelState.IsValid)
            {
                string PurchaseRequest = ApprovalGroupRequestTypes.PurchaseRequest;
                long ApprovalGroupId = RetrieveApprovalGroupIdByRequestorIdAndRequestType(PurchaseRequest);
                prVM.ApprovalRouteModel.ApprovalGroupId = ApprovalGroupId;
                prVM.ApprovalRouteModel.RequestType = PurchaseRequest;
                if (ApprovalGroupId >= 0)
                {
                    var objPurchaseRequest = woWrapper.ApprovePR(prVM.ApprovalRouteModel);
                    if (objPurchaseRequest.ErrorMessages != null && objPurchaseRequest.ErrorMessages.Count > 0)
                    {
                        return Json(objPurchaseRequest.ErrorMessages, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { data = JsonReturnEnum.success.ToString(), PurchaseRequestId = prVM.ApprovalRouteModel.ObjectId, ApprovalGroupId = ApprovalGroupId }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(new { data = JsonReturnEnum.success.ToString(), PurchaseRequestId = prVM.ApprovalRouteModel.ObjectId, ApprovalGroupId = ApprovalGroupId }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                string ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region V2-693 SOMAX to SAP Purchase request export
        [HttpPost]
        public JsonResult SendToSAPListPR(List<long> PurchaseRequestIds)
        {
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            PurchaseRequestWrapper pWrapper = new PurchaseRequestWrapper(userData);
            List<string> errMsgList = new List<string>();

            if (PurchaseRequestIds.Count > 0)
            {
                InterfaceProp iprop = commonWrapper.RetrieveInterfaceProperties(ApiConstants.SAPPurchaseRequestExport);
                string int_url = iprop.APIKey1;

                foreach (var pritem in PurchaseRequestIds)
                {
                    var purchaseRequest = pWrapper.RetrievePurchaseRequestByIdForExportSAP(pritem);
                    var purchaseRequestLineItems = pWrapper.RetrievePRLineItemByIdForExportSAP(pritem);

                    if (purchaseRequest.Status != PurchaseRequestStatusConstants.Approved ||
                        !purchaseRequest.VendorIsExternal)
                    {
                        errMsgList.Add(purchaseRequest.ClientLookupId + "'s Status or vendor is not eligible");
                        continue;
                    }
                    List<PRExportModel_SAP> exportModels = BindExportModel(purchaseRequest, purchaseRequestLineItems);

                    JsonSerializerSettings settings = new JsonSerializerSettings()
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                    };

                    string strJson = JsonConvert.SerializeObject(exportModels, settings);

                    Task<string> result = SendSAPRequest(strJson, int_url, purchaseRequest.ClientLookupId,
                        purchaseRequest.PurchaseRequestId);
                    result.Wait();
                    string errorDetails = result.Result.ToString();
                    if (errorDetails != null && errorDetails.Length > 0)
                    {
                        errMsgList.Add(errorDetails);
                    }
                }
            }
            if (errMsgList.Count > 0)
            {
                return Json(new { data = errMsgList }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { data = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }
        private async Task<string> SendSAPRequest(string strJson, string PostUrl, string ClientLookupId,
            long PurchaseRequestId)
        {
            PurchaseRequestWrapper pWrapper = new PurchaseRequestWrapper(userData);
            string ErrorDetails = "";
            if (strJson.Length > 0)
            {
                HttpResponseMessage httpResponse = new HttpResponseMessage();
                using (var httpClient = new HttpClient())
                {

                    using (var request = new HttpRequestMessage(new HttpMethod("POST"), PostUrl))
                    {
                        request.Headers.TryAddWithoutValidation("Accept", "application/json");
                        //request.Headers.TryAddWithoutValidation("X-COUPA-API-KEY", int_key);
                        //request.Headers.TryAddWithoutValidation("X-COUPA-API-KEY", "8b2c306ada338ccc50e8afb8249ed344168f7ee7");
                        request.Content = new StringContent(strJson, System.Text.Encoding.UTF8, "application/json");
                        httpResponse = await httpClient.SendAsync(request);

                    }
                }
                if (httpResponse.IsSuccessStatusCode)
                {
                    string resultJSON = httpResponse.Content.ReadAsStringAsync().Result;
                    var errList = pWrapper.UpdatePrStatus(PurchaseRequestId, PurchaseRequestStatusConstants.Extracted, 101,
                        PurchasingEvents.SentToSAP);

                    var erstr = string.Empty;
                    if (errList != null && errList.Count > 0)
                    {
                        foreach (var v in errList)
                        {
                            erstr += v + ",";
                        }
                        ErrorDetails = ClientLookupId + " failed to update status :" + erstr;
                    }
                }
                else
                {
                    string resultJSON = httpResponse.Content.ReadAsStringAsync().Result;
                    ErrorDetails = ClientLookupId + " failed :" + resultJSON;
                }
            }
            return ErrorDetails;
        }
        private void SetIsSendToSAPFlag(ref PurchaseRequestVM objPurchaseRequestVM)
        {
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            objPurchaseRequestVM.IsSendToSAP = commonWrapper.CheckIsActiveInterface(ApiConstants.SAPPurchaseRequestExport);
        }
        private List<PRExportModel_SAP> BindExportModel(DataContracts.PurchaseRequest purchaseRequest,
                                                  List<PurchaseRequestLineItem> purchaseRequestLineItems)
        {
            PRExportModel_SAP exportModel;
            List<PRExportModel_SAP> exportModels = new List<PRExportModel_SAP>();
            foreach (var item in purchaseRequestLineItems)
            {
                exportModel = new PRExportModel_SAP();

                exportModel.PurchaseRequestClientLookupId = purchaseRequest.ClientLookupId;
                if (purchaseRequest.Approved_Date == null)
                {
                    exportModel.ApprovedDate = "";
                }
                else
                {
                    exportModel.ApprovedDate = purchaseRequest.Approved_Date?.ToString("yyyyMMdd", CultureInfo.InvariantCulture);
                }
                exportModel.VendorClientLookupId = purchaseRequest.VendorClientLookupId;
                exportModel.PersonnelEXOracleUserId = purchaseRequest.EXOracleUserId;
                exportModel.UserName = purchaseRequest.UserName;
                exportModel.ClientId = purchaseRequest.ClientId;
                exportModel.SiteId = purchaseRequest.SiteId;
                exportModel.PurchaseRequestId = purchaseRequest.PurchaseRequestId;
                exportModel.LineNumber = item.LineNumber;
                exportModel.Description = item.Description;
                exportModel.AccountClientLookupId = item.Account_ClientLookupId;
                exportModel.UnitOfMeasure = item.UnitofMeasure;
                exportModel.UnitCost = item.UnitCost;
                exportModel.OrderQuantity = item.OrderQuantity;

                exportModels.Add(exportModel);
            }
            return exportModels;
        }
        #endregion

        #region V2-730
        [HttpPost]
        public JsonResult MultiLevelApprovePR(long PurchaseRequestId, long ApprovalGroupId, string ClientLookupId)
        {
            var dataModels = Get_MultiLevelApproverList(this.userData.DatabaseKey.Personnel.PersonnelId, ApprovalGroupRequestTypes.PurchaseRequest, PurchaseRequestId, ApprovalGroupId);
            var approvers = new List<SelectListItem>();
            if (dataModels.Count > 0)
            {
                approvers = dataModels.Select(x => new SelectListItem
                {
                    Text = x.ApproverName,
                    Value = x.ApproverId.ToString()
                }).ToList();
            }
            return Json(new { data = JsonReturnEnum.success.ToString(), ApproverList = approvers }, JsonRequestBehavior.AllowGet);
        }
        public PartialViewResult SendPRForMultiLevelApproval(List<SelectListItem> Approvers, long PurchaseRequestId, long ApprovalGroupId)
        {
            PurchaseRequestVM objPRVM = new PurchaseRequestVM();

            ApprovalRouteModel approvalRouteModel = new ApprovalRouteModel();

            approvalRouteModel.ApproverList = Approvers;
            approvalRouteModel.ApproverCount = Approvers.Count;
            approvalRouteModel.ObjectId = PurchaseRequestId;
            approvalRouteModel.ApprovalGroupId = ApprovalGroupId;

            objPRVM.ApprovalRouteModel = approvalRouteModel;
            LocalizeControls(objPRVM, LocalizeResourceSetConstants.PurchaseRequest);
            return PartialView("_SendPurchaseRequestForMultiLevelApproval", objPRVM);
            //return approvalRouteModel;
        }
        [HttpPost]
        public JsonResult SendPRForMultiLevelApprovalSave(PurchaseRequestVM prVM)
        {
            PurchaseRequestWrapper woWrapper = new PurchaseRequestWrapper(userData);
            if (ModelState.IsValid)
            {
                string PurchaseRequest = ApprovalGroupRequestTypes.PurchaseRequest;
                prVM.ApprovalRouteModel.RequestType = PurchaseRequest;
                var objPurchaseRequest = woWrapper.MultiLevelApprovePR(prVM.ApprovalRouteModel);
                if (objPurchaseRequest.ErrorMessages != null && objPurchaseRequest.ErrorMessages.Count > 0)
                {
                    return Json(objPurchaseRequest.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { data = JsonReturnEnum.success.ToString(), PurchaseRequestId = prVM.ApprovalRouteModel.ObjectId, ApprovalGroupId = prVM.ApprovalRouteModel.ApprovalGroupId }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                string ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult MultiLevelFinalApprove(long PurchaseRequestId, long ApprovalGroupId)
        {
            PurchaseRequestWrapper woWrapper = new PurchaseRequestWrapper(userData);
            if (ModelState.IsValid)
            {
                var objPurchaseRequest = woWrapper.MultiLevelFinalApprovePR(PurchaseRequestId, ApprovalGroupId);
                if (objPurchaseRequest.ErrorMessages != null && objPurchaseRequest.ErrorMessages.Count > 0)
                {
                    return Json(objPurchaseRequest.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), PurchaseRequestId = PurchaseRequestId, ApprovalGroupId = ApprovalGroupId }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                string ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult MultiLevelDenyPR(long PurchaseRequestId, long ApprovalGroupId, string ClientLookupId)
        {
            PurchaseRequestWrapper pWrapper = new PurchaseRequestWrapper(userData);
            pWrapper.MultiLevelDenyPR(PurchaseRequestId, ApprovalGroupId);

            if (pWrapper.errorMessage != null && pWrapper.errorMessage.Count > 0)
            {
                return Json(new { data = pWrapper.errorMessage }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { data = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        #region V2-820
        public ReviewPRSendForApprovalModel ReviewAndSendPRForApproval(long PurchaseRequestId)
        {
            PurchaseRequestVM objPRVM = new PurchaseRequestVM();

            ReviewPRSendForApprovalModel approvalRouteModel = new ReviewPRSendForApprovalModel();
            var dataModels = Get_ApproverList(this.userData.DatabaseKey.Personnel.PersonnelId, ApprovalGroupRequestTypes.PurchaseRequest, 1);
            var approvers = new List<SelectListItem>();
            var securityName = SecurityConstants.PurchaseRequest_Approve;
            var ItemAccess = "ItemAccess";
            dataModels = Get_PersonnelList(securityName, ItemAccess);
            approvers = dataModels.Where(m => m.AssignedTo_PersonnelId != this.userData.DatabaseKey.Personnel.PersonnelId).Select(x => new SelectListItem
            {
                Text = x.NameFirst + " " + x.NameLast,
                Value = x.AssignedTo_PersonnelId.ToString()
            }).ToList();

            approvalRouteModel.ApproverList = approvers;
            approvalRouteModel.ApproverCount = approvers.Count;
            approvalRouteModel.ObjectId = PurchaseRequestId;

            objPRVM.reviewPRSendForApprovalModel = approvalRouteModel;
            LocalizeControls(objPRVM, LocalizeResourceSetConstants.PurchaseRequest);
            return approvalRouteModel;
        }
        [HttpPost]
        public JsonResult ReviewAndSendPRApprovalSave(PurchaseRequestVM prVM)
        {
            PurchaseRequestWrapper woWrapper = new PurchaseRequestWrapper(userData);
            if (ModelState.IsValid)
            {
                string PurchaseRequest = ApprovalGroupRequestTypes.PurchaseRequest;
                long ApprovalGroupId = RetrieveApprovalGroupIdByRequestorIdAndRequestType(PurchaseRequest);
                prVM.reviewPRSendForApprovalModel.ApprovalGroupId = ApprovalGroupId;
                prVM.reviewPRSendForApprovalModel.RequestType = PurchaseRequest;
                if (ApprovalGroupId >= 0)
                {
                    var objPurchaseRequest = woWrapper.ReviewAndSendApprovePR(prVM.reviewPRSendForApprovalModel);
                    if (objPurchaseRequest.ErrorMessages != null && objPurchaseRequest.ErrorMessages.Count > 0)
                    {
                        return Json(objPurchaseRequest.ErrorMessages, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { data = JsonReturnEnum.success.ToString(), PurchaseRequestId = prVM.reviewPRSendForApprovalModel.ObjectId, ApprovalGroupId = ApprovalGroupId }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(new { data = JsonReturnEnum.success.ToString(), PurchaseRequestId = prVM.reviewPRSendForApprovalModel.ObjectId, ApprovalGroupId = ApprovalGroupId }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                string ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region V2-945 DevExpress Print
        [HttpPost]
        public JsonResult SetPrintPRListFromIndex(PurchaseRequestPrntModel model)
        {
            List<long> PurchaseRequestIds = model.list.Select(x => x.PurchaseRequestId).ToList();
            Session["PrintPRList"] = PurchaseRequestIds;
            return Json(new { success = true });
        }
        [EncryptedActionParameter]
        public ActionResult GeneratePurchaseRequestPrint()
        {
            List<long> PurchaseRequestIds = new List<long>();
            if (Session["PrintPRList"] != null)
            {
                PurchaseRequestIds = (List<long>)Session["PrintPRList"];
            }
            var objPrintModelList = PrintDevExpressFromIndex(PurchaseRequestIds);
            return View("DevExpressPrint", objPrintModelList);
        }
        public List<PurchaseRequestDevExpressPrintModel> PrintDevExpressFromIndex(List<long> PurchaseRequestIds)
        {
            var PurchaseRequestDevExpressPrintModelList = new List<PurchaseRequestDevExpressPrintModel>();
            var PurchaseRequestDevExpressPrintModel = new PurchaseRequestDevExpressPrintModel();
            PurchaseRequestWrapper prWrapper = new PurchaseRequestWrapper(userData);
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            List<Notes> NotesList = new List<Notes>();
            var PurchaseRequestIDList = PurchaseRequestIds;
            var PurchaseRequestBunchListInfo = prWrapper.RetrieveAllByPurchaseRequestV2Print(PurchaseRequestIDList);
            Parallel.ForEach(PurchaseRequestIds, p =>
            {
                var notes = commonWrapper.PopulateComment(p, "PurchaseRequest");
                NotesList.AddRange(notes);
            });
            List<DataContracts.PurchaseRequest> listOfPR = PurchaseRequestBunchListInfo.listOfPR;
            List<PRHeaderUDF> listOfPRHeaderUDF = PurchaseRequestBunchListInfo.listOfPRHeaderUDF;
            List<PurchaseRequestLineItem> listOfPRLI = PurchaseRequestBunchListInfo.listOfPRLI;
            List<PRLineUDF> listOfPRLineUDF = PurchaseRequestBunchListInfo.listOfPRLineUDF;
            var ImageUrl = GenerateImageUrlDevExpress();// no need to call for each id as it is dependent on client id
            foreach (var item in PurchaseRequestIds)
            {
                DataContracts.PurchaseRequest purchaseRequestDetails = listOfPR.Where(m => m.PurchaseRequestId == item).FirstOrDefault();
                PRHeaderUDF pRHeaderUDFDetails = listOfPRHeaderUDF.Where(m => m.PurchaseRequestId == item).FirstOrDefault();
                List<PurchaseRequestLineItem> purchaseRequestLineItemList = listOfPRLI.Where(m => m.PurchaseRequestId == item).ToList();
                List<PRLineUDF> PRLineUDFList = listOfPRLineUDF.Where(m => m.PurchaseRequestId == item).ToList();
                List<Notes> listOfNotes = NotesList.Where(m => m.ObjectId == item).ToList();
                PurchaseRequestDevExpressPrintModel = new PurchaseRequestDevExpressPrintModel();
                BindPurchaseRequestDetails(purchaseRequestDetails, ref PurchaseRequestDevExpressPrintModel, ImageUrl);
                BindPRHeaderUDFDetails(pRHeaderUDFDetails, ref PurchaseRequestDevExpressPrintModel);
                BindPRLineItemDetails(purchaseRequestLineItemList, PRLineUDFList, ref PurchaseRequestDevExpressPrintModel);
                BindCommentsTable(listOfNotes, ref PurchaseRequestDevExpressPrintModel);
                ClientSetUpWrapper csWrapper = new ClientSetUpWrapper(userData);
                var formsettingdetails = csWrapper.FormSettingsDetails();
                if (formsettingdetails != null)
                {
                    PurchaseRequestDevExpressPrintModel.PRUIC = formsettingdetails.PRUIC;
                    PurchaseRequestDevExpressPrintModel.PRLine2 = formsettingdetails.PRLine2;
                    PurchaseRequestDevExpressPrintModel.PRLIUIC = formsettingdetails.PRLIUIC;
                    PurchaseRequestDevExpressPrintModel.PRComments = formsettingdetails.PRComments;
                }
                PurchaseRequestDevExpressPrintModel.OnPremise = userData.DatabaseKey.Client.OnPremise;
                PurchaseRequestDevExpressPrintModelList.Add(PurchaseRequestDevExpressPrintModel);
            }

            return PurchaseRequestDevExpressPrintModelList;
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
        private void BindPurchaseRequestDetails(DataContracts.PurchaseRequest PurchaseRequestDetails, ref PurchaseRequestDevExpressPrintModel purchaseRequestDevExpressPrintModel, string AzureImageUrl)
        {
            purchaseRequestDevExpressPrintModel.AzureImageUrl = AzureImageUrl;
            purchaseRequestDevExpressPrintModel.ClientlookupId = PurchaseRequestDetails.ClientLookupId;
            purchaseRequestDevExpressPrintModel.Reason = PurchaseRequestDetails.Reason;
            if (PurchaseRequestDetails.CreateDate == null || PurchaseRequestDetails.CreateDate == default(DateTime))
            {
                purchaseRequestDevExpressPrintModel.CreateDate = "";
            }
            else
            {
                purchaseRequestDevExpressPrintModel.CreateDate = Convert.ToDateTime(PurchaseRequestDetails.CreateDate)
                    .ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
            }
            purchaseRequestDevExpressPrintModel.VendorName = PurchaseRequestDetails.VendorName;
            purchaseRequestDevExpressPrintModel.VendorAddress1 = PurchaseRequestDetails.VendorAddress1;
            purchaseRequestDevExpressPrintModel.VendorAddress2 = PurchaseRequestDetails.VendorAddress2;
            purchaseRequestDevExpressPrintModel.VendorAddress3 = PurchaseRequestDetails.VendorAddress3;
            purchaseRequestDevExpressPrintModel.VendorCity = PurchaseRequestDetails.VendorAddressCity;
            purchaseRequestDevExpressPrintModel.VendorCountry = PurchaseRequestDetails.VendorAddressCountry;
            purchaseRequestDevExpressPrintModel.VendorZip = PurchaseRequestDetails.VendorAddressPostCode;
            purchaseRequestDevExpressPrintModel.VendorState = PurchaseRequestDetails.VendorAddressState;
            purchaseRequestDevExpressPrintModel.ShipToName = PurchaseRequestDetails.SiteName;
            purchaseRequestDevExpressPrintModel.ShipToAddress1 = PurchaseRequestDetails.SiteAddress1;
            purchaseRequestDevExpressPrintModel.ShipToAddress2 = PurchaseRequestDetails.SiteAddress2;
            purchaseRequestDevExpressPrintModel.ShipToAddress3 = PurchaseRequestDetails.SiteAddress3;
            purchaseRequestDevExpressPrintModel.ShipToCity = PurchaseRequestDetails.SiteAddressCity;
            purchaseRequestDevExpressPrintModel.ShipToCountry = PurchaseRequestDetails.SiteAddressCountry;
            purchaseRequestDevExpressPrintModel.ShipToZip = PurchaseRequestDetails.SiteAddressPostCode;
            purchaseRequestDevExpressPrintModel.ShipToState = PurchaseRequestDetails.SiteAddressState;
            purchaseRequestDevExpressPrintModel.BillToName = PurchaseRequestDetails.SiteBillToName;
            purchaseRequestDevExpressPrintModel.BillToAddress1 = PurchaseRequestDetails.SiteBillToAddress1;
            purchaseRequestDevExpressPrintModel.BillToAddress2 = PurchaseRequestDetails.SiteBillToAddress2;
            purchaseRequestDevExpressPrintModel.BillToAddress3 = PurchaseRequestDetails.SiteBillToAddress3;
            purchaseRequestDevExpressPrintModel.BillToCity = PurchaseRequestDetails.SiteBillToAddressCity;
            purchaseRequestDevExpressPrintModel.BillToCountry = PurchaseRequestDetails.SiteBillToAddressCountry;
            purchaseRequestDevExpressPrintModel.BillToZip = PurchaseRequestDetails.SiteBillToAddressPostCode;
            purchaseRequestDevExpressPrintModel.BillToState = PurchaseRequestDetails.SiteBillToAddressState;
            purchaseRequestDevExpressPrintModel.CreateBy = PurchaseRequestDetails.Creator_PersonnelName;
            #region Localization
            purchaseRequestDevExpressPrintModel.GlobalPurchaseRequest = UtilityFunction.GetMessageFromResource("spnPurchaseRequest", LocalizeResourceSetConstants.Global);
            purchaseRequestDevExpressPrintModel.spnCreatedBy = UtilityFunction.GetMessageFromResource("spnCreatedBy", LocalizeResourceSetConstants.Global);
            purchaseRequestDevExpressPrintModel.globalCreateDate = UtilityFunction.GetMessageFromResource("globalCreateDate", LocalizeResourceSetConstants.Global);
            purchaseRequestDevExpressPrintModel.spnVendor = UtilityFunction.GetMessageFromResource("spnVendor", LocalizeResourceSetConstants.Global);
            purchaseRequestDevExpressPrintModel.GlobalShipTo = UtilityFunction.GetMessageFromResource("GlobalShipTo", LocalizeResourceSetConstants.Global);
            purchaseRequestDevExpressPrintModel.spnPRBillTo = UtilityFunction.GetMessageFromResource("spnPRBillTo", LocalizeResourceSetConstants.PurchaseRequest);
            purchaseRequestDevExpressPrintModel.GlobalReason = UtilityFunction.GetMessageFromResource("GlobalReason", LocalizeResourceSetConstants.Global);
            #endregion
        }
        private void BindPRHeaderUDFDetails(PRHeaderUDF pRHeaderUDF, ref PurchaseRequestDevExpressPrintModel purchaseRequestDevExpressPrintModel)
        {
            purchaseRequestDevExpressPrintModel.PRHeaderUDF_PRId = pRHeaderUDF?.PurchaseRequestId ?? 0;
            if (pRHeaderUDF != null)
            {
                purchaseRequestDevExpressPrintModel.Text1 = pRHeaderUDF.Text1;
                purchaseRequestDevExpressPrintModel.Text2 = pRHeaderUDF.Text2;
                purchaseRequestDevExpressPrintModel.Text3 = pRHeaderUDF.Text3;
                purchaseRequestDevExpressPrintModel.Text4 = pRHeaderUDF.Text4;
                if (pRHeaderUDF.Date1 == null || pRHeaderUDF.Date1 == default(DateTime))
                {
                    purchaseRequestDevExpressPrintModel.Date1 = "";
                }
                else
                {
                    purchaseRequestDevExpressPrintModel.Date1 = Convert.ToDateTime(pRHeaderUDF.Date1)
                        .ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                }
                if (pRHeaderUDF.Date2 == null || pRHeaderUDF.Date2 == default(DateTime))
                {
                    purchaseRequestDevExpressPrintModel.Date2 = "";
                }
                else
                {
                    purchaseRequestDevExpressPrintModel.Date2 = Convert.ToDateTime(pRHeaderUDF.Date2)
                        .ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                }
                if (pRHeaderUDF.Date3 == null || pRHeaderUDF.Date3 == default(DateTime))
                {
                    purchaseRequestDevExpressPrintModel.Date3 = "";
                }
                else
                {
                    purchaseRequestDevExpressPrintModel.Date3 = Convert.ToDateTime(pRHeaderUDF.Date3)
                        .ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                }
                if (pRHeaderUDF.Date4 == null || pRHeaderUDF.Date4 == default(DateTime))
                {
                    purchaseRequestDevExpressPrintModel.Date4 = "";
                }
                else
                {
                    purchaseRequestDevExpressPrintModel.Date4 = Convert.ToDateTime(pRHeaderUDF.Date4)
                        .ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                }
                purchaseRequestDevExpressPrintModel.Bit1 = pRHeaderUDF.Bit1 ? "Yes" : "No";
                purchaseRequestDevExpressPrintModel.Bit2 = pRHeaderUDF.Bit2 ? "Yes" : "No";
                purchaseRequestDevExpressPrintModel.Bit3 = pRHeaderUDF.Bit3 ? "Yes" : "No";
                purchaseRequestDevExpressPrintModel.Bit4 = pRHeaderUDF.Bit4 ? "Yes" : "No";
                purchaseRequestDevExpressPrintModel.Numeric1 = pRHeaderUDF.Numeric1;
                purchaseRequestDevExpressPrintModel.Numeric2 = pRHeaderUDF.Numeric2;
                purchaseRequestDevExpressPrintModel.Numeric3 = pRHeaderUDF.Numeric3;
                purchaseRequestDevExpressPrintModel.Numeric4 = pRHeaderUDF.Numeric4;
                purchaseRequestDevExpressPrintModel.Select1 = pRHeaderUDF.Select1;
                purchaseRequestDevExpressPrintModel.Select2 = pRHeaderUDF.Select2;
                purchaseRequestDevExpressPrintModel.Select3 = pRHeaderUDF.Select3;
                purchaseRequestDevExpressPrintModel.Select4 = pRHeaderUDF.Select4;
                purchaseRequestDevExpressPrintModel.Text1Label = pRHeaderUDF.Text1Label;
                purchaseRequestDevExpressPrintModel.Text2Label = pRHeaderUDF.Text2Label;
                purchaseRequestDevExpressPrintModel.Text3Label = pRHeaderUDF.Text3Label;
                purchaseRequestDevExpressPrintModel.Text4Label = pRHeaderUDF.Text4Label;
                purchaseRequestDevExpressPrintModel.Date1Label = pRHeaderUDF.Date1Label;
                purchaseRequestDevExpressPrintModel.Date2Label = pRHeaderUDF.Date2Label;
                purchaseRequestDevExpressPrintModel.Date3Label = pRHeaderUDF.Date3Label;
                purchaseRequestDevExpressPrintModel.Date4Label = pRHeaderUDF.Date4Label;
                purchaseRequestDevExpressPrintModel.Bit1Label = pRHeaderUDF.Bit1Label;
                purchaseRequestDevExpressPrintModel.Bit2Label = pRHeaderUDF.Bit2Label;
                purchaseRequestDevExpressPrintModel.Bit3Label = pRHeaderUDF.Bit3Label;
                purchaseRequestDevExpressPrintModel.Bit4Label = pRHeaderUDF.Bit4Label;
                purchaseRequestDevExpressPrintModel.Numeric1Label = pRHeaderUDF.Numeric1Label;
                purchaseRequestDevExpressPrintModel.Numeric2Label = pRHeaderUDF.Numeric2Label;
                purchaseRequestDevExpressPrintModel.Numeric3Label = pRHeaderUDF.Numeric3Label;
                purchaseRequestDevExpressPrintModel.Numeric4Label = pRHeaderUDF.Numeric4Label;
                purchaseRequestDevExpressPrintModel.Select1Label = pRHeaderUDF.Select1Label;
                purchaseRequestDevExpressPrintModel.Select2Label = pRHeaderUDF.Select2Label;
                purchaseRequestDevExpressPrintModel.Select3Label = pRHeaderUDF.Select3Label;
                purchaseRequestDevExpressPrintModel.Select4Label = pRHeaderUDF.Select4Label;
            }
        }

        private void BindPRLineItemDetails(List<PurchaseRequestLineItem> listofPurchaseRequestLineItem, List<PRLineUDF> listOfPRLineUDF, ref PurchaseRequestDevExpressPrintModel purchaseRequestDevExpressPrintModel)
        {
            if (listofPurchaseRequestLineItem.Count > 0)
            {
                foreach (var item in listofPurchaseRequestLineItem)
                {
                    var objPRLineItemDevExpressPrintModel = new PRLineItemDevExpressPrintModel();
                    objPRLineItemDevExpressPrintModel.Description = item.Description;
                    if (item.RequiredDate == null || item.RequiredDate == default(DateTime))
                    {
                        objPRLineItemDevExpressPrintModel.RequiredDate = "";
                    }
                    else
                    {
                        objPRLineItemDevExpressPrintModel.RequiredDate = Convert.ToDateTime(item.RequiredDate)
                            .ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                    }
                    objPRLineItemDevExpressPrintModel.LineNumber = item.LineNumber;
                    objPRLineItemDevExpressPrintModel.OrderQuantity = item.OrderQuantity;
                    objPRLineItemDevExpressPrintModel.UOM = item.UnitofMeasure;
                    objPRLineItemDevExpressPrintModel.Price = item.UnitCost;
                    objPRLineItemDevExpressPrintModel.Total = item.TotalCost;
                    objPRLineItemDevExpressPrintModel.PartClientLookupId = item.PartClientLookupId;
                    objPRLineItemDevExpressPrintModel.ChargeTo = item.ChargeToClientLookupId;
                    objPRLineItemDevExpressPrintModel.AccountClientLookupId = item.Account_ClientLookupId;
                    PRLineUDF PRLineUDFDetails = listOfPRLineUDF.Where(m => m.PurchaseRequestLineItemId == item.PurchaseRequestLineItemId).FirstOrDefault();
                    BindPRLineUDFDetails(PRLineUDFDetails, ref objPRLineItemDevExpressPrintModel);
                    #region Localizations
                    objPRLineItemDevExpressPrintModel.globalLine = UtilityFunction.GetMessageFromResource("globalLine", LocalizeResourceSetConstants.Global);
                    objPRLineItemDevExpressPrintModel.spnPartID = UtilityFunction.GetMessageFromResource("spnPartID", LocalizeResourceSetConstants.Global);
                    objPRLineItemDevExpressPrintModel.spnDescription = UtilityFunction.GetMessageFromResource("spnDescription", LocalizeResourceSetConstants.Global);
                    objPRLineItemDevExpressPrintModel.spnQty = UtilityFunction.GetMessageFromResource("spnQty", LocalizeResourceSetConstants.PurchaseOrder);
                    objPRLineItemDevExpressPrintModel.spnUOM = UtilityFunction.GetMessageFromResource("spnUOM", LocalizeResourceSetConstants.WorkOrderDetails);
                    objPRLineItemDevExpressPrintModel.spnPrice = UtilityFunction.GetMessageFromResource("spnPrice", LocalizeResourceSetConstants.PartDetails);
                    objPRLineItemDevExpressPrintModel.spnTotal = UtilityFunction.GetMessageFromResource("spnTotal", LocalizeResourceSetConstants.FleetServiceOrder);
                    objPRLineItemDevExpressPrintModel.GlobalAccount = UtilityFunction.GetMessageFromResource("GlobalAccount", LocalizeResourceSetConstants.Global);
                    objPRLineItemDevExpressPrintModel.GlobalChargeTo = UtilityFunction.GetMessageFromResource("GlobalChargeTo", LocalizeResourceSetConstants.Global);
                    objPRLineItemDevExpressPrintModel.spnRequired = UtilityFunction.GetMessageFromResource("spnRequired", LocalizeResourceSetConstants.Global);
                    objPRLineItemDevExpressPrintModel.GlobalGrandTotal = UtilityFunction.GetMessageFromResource("GlobalGrandTotal", LocalizeResourceSetConstants.Global);
                    #endregion
                    purchaseRequestDevExpressPrintModel.LineItemDevExpressPrintModelList.Add(objPRLineItemDevExpressPrintModel);
                }
            }
        }
        private void BindPRLineUDFDetails(PRLineUDF PRLineUDFDetails, ref PRLineItemDevExpressPrintModel pRLineItemDevExpressPrintModel)
        {
            pRLineItemDevExpressPrintModel.PRLineUDF_PRLIId = PRLineUDFDetails?.PurchaseRequestLineItemId ?? 0;
            pRLineItemDevExpressPrintModel.PRLineUDF_PRId = PRLineUDFDetails?.PurchaseRequestId ?? 0;
            if (PRLineUDFDetails != null)
            {
                pRLineItemDevExpressPrintModel.Text1 = PRLineUDFDetails.Text1;
                pRLineItemDevExpressPrintModel.Text2 = PRLineUDFDetails.Text2;
                pRLineItemDevExpressPrintModel.Text3 = PRLineUDFDetails.Text3;
                pRLineItemDevExpressPrintModel.Text4 = PRLineUDFDetails.Text4;
                if (PRLineUDFDetails.Date1 == null || PRLineUDFDetails.Date1 == default(DateTime))
                {
                    pRLineItemDevExpressPrintModel.Date1 = "";
                }
                else
                {
                    pRLineItemDevExpressPrintModel.Date1 = Convert.ToDateTime(PRLineUDFDetails.Date1)
                        .ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                }
                if (PRLineUDFDetails.Date2 == null || PRLineUDFDetails.Date2 == default(DateTime))
                {
                    pRLineItemDevExpressPrintModel.Date2 = "";
                }
                else
                {
                    pRLineItemDevExpressPrintModel.Date2 = Convert.ToDateTime(PRLineUDFDetails.Date2)
                        .ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                }
                if (PRLineUDFDetails.Date3 == null || PRLineUDFDetails.Date3 == default(DateTime))
                {
                    pRLineItemDevExpressPrintModel.Date3 = "";
                }
                else
                {
                    pRLineItemDevExpressPrintModel.Date3 = Convert.ToDateTime(PRLineUDFDetails.Date3)
                        .ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                }
                if (PRLineUDFDetails.Date4 == null || PRLineUDFDetails.Date4 == default(DateTime))
                {
                    pRLineItemDevExpressPrintModel.Date4 = "";
                }
                else
                {
                    pRLineItemDevExpressPrintModel.Date4 = Convert.ToDateTime(PRLineUDFDetails.Date4)
                        .ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                }
                pRLineItemDevExpressPrintModel.Bit1 = PRLineUDFDetails.Bit1 ? "Yes" : "No";
                pRLineItemDevExpressPrintModel.Bit2 = PRLineUDFDetails.Bit2 ? "Yes" : "No";
                pRLineItemDevExpressPrintModel.Bit3 = PRLineUDFDetails.Bit3 ? "Yes" : "No";
                pRLineItemDevExpressPrintModel.Bit4 = PRLineUDFDetails.Bit4 ? "Yes" : "No";
                pRLineItemDevExpressPrintModel.Numeric1 = PRLineUDFDetails.Numeric1;
                pRLineItemDevExpressPrintModel.Numeric2 = PRLineUDFDetails.Numeric2;
                pRLineItemDevExpressPrintModel.Numeric3 = PRLineUDFDetails.Numeric3;
                pRLineItemDevExpressPrintModel.Numeric4 = PRLineUDFDetails.Numeric4;
                pRLineItemDevExpressPrintModel.Select1 = PRLineUDFDetails.Select1;
                pRLineItemDevExpressPrintModel.Select2 = PRLineUDFDetails.Select2;
                pRLineItemDevExpressPrintModel.Select3 = PRLineUDFDetails.Select3;
                pRLineItemDevExpressPrintModel.Select4 = PRLineUDFDetails.Select4;
                pRLineItemDevExpressPrintModel.Text1Label = PRLineUDFDetails.Text1Label;
                pRLineItemDevExpressPrintModel.Text2Label = PRLineUDFDetails.Text2Label;
                pRLineItemDevExpressPrintModel.Text3Label = PRLineUDFDetails.Text3Label;
                pRLineItemDevExpressPrintModel.Text4Label = PRLineUDFDetails.Text4Label;
                pRLineItemDevExpressPrintModel.Date1Label = PRLineUDFDetails.Date1Label;
                pRLineItemDevExpressPrintModel.Date2Label = PRLineUDFDetails.Date2Label;
                pRLineItemDevExpressPrintModel.Date3Label = PRLineUDFDetails.Date3Label;
                pRLineItemDevExpressPrintModel.Date4Label = PRLineUDFDetails.Date4Label;
                pRLineItemDevExpressPrintModel.Bit1Label = PRLineUDFDetails.Bit1Label;
                pRLineItemDevExpressPrintModel.Bit2Label = PRLineUDFDetails.Bit2Label;
                pRLineItemDevExpressPrintModel.Bit3Label = PRLineUDFDetails.Bit3Label;
                pRLineItemDevExpressPrintModel.Bit4Label = PRLineUDFDetails.Bit4Label;
                pRLineItemDevExpressPrintModel.Numeric1Label = PRLineUDFDetails.Numeric1Label;
                pRLineItemDevExpressPrintModel.Numeric2Label = PRLineUDFDetails.Numeric2Label;
                pRLineItemDevExpressPrintModel.Numeric3Label = PRLineUDFDetails.Numeric3Label;
                pRLineItemDevExpressPrintModel.Numeric4Label = PRLineUDFDetails.Numeric4Label;
                pRLineItemDevExpressPrintModel.Select1Label = PRLineUDFDetails.Select1Label;
                pRLineItemDevExpressPrintModel.Select2Label = PRLineUDFDetails.Select2Label;
                pRLineItemDevExpressPrintModel.Select3Label = PRLineUDFDetails.Select3Label;
                pRLineItemDevExpressPrintModel.Select4Label = PRLineUDFDetails.Select4Label;
            }
        }
        private void BindCommentsTable(List<Notes> notes, ref PurchaseRequestDevExpressPrintModel purchaseRequestDevExpressPrintModel)
        {
            if (notes != null && notes.Count > 0)
            {
                foreach (var item in notes.OrderBy(n => n.ObjectId))
                {
                    var note = new CommentsDevExpressPrintModel();
                    note.PurchaseRequestId = item.ObjectId;
                    note.Comments = item.Content;
                    note.OwnerName = item.OwnerName;
                    if (item.CreateDate == null || item.CreateDate == default(DateTime))
                    {
                        note.CreateDate = "";
                    }
                    else
                    {
                        note.CreateDate = Convert.ToDateTime(item.CreateDate).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                    }
                    note.spnGlobalNote = UtilityFunction.GetMessageFromResource("spnGlobalNote", LocalizeResourceSetConstants.Global);
                    note.spnDate = UtilityFunction.GetMessageFromResource("spnDate", LocalizeResourceSetConstants.Global);
                    note.globalOwner = UtilityFunction.GetMessageFromResource("globalOwner", LocalizeResourceSetConstants.Global);
                    note.noteContent = UtilityFunction.GetMessageFromResource("noteContent", LocalizeResourceSetConstants.Global);
                    purchaseRequestDevExpressPrintModel.CommentsDevExpressPrintModelList.Add(note);
                }
            }
        }
        [HttpPost]
        public ActionResult UpdateEmailStatusDevexpresss(PurchaseRequestVM PurchaseRequestVM)
        {
            PurchaseRequestWrapper pWrapper = new PurchaseRequestWrapper(userData);
            MemoryStream stream = new MemoryStream();
            string emailHtmlBody = string.Empty;
            int PersonnelId = 0;
            var templateFilePath = System.Web.HttpContext.Current.Server.MapPath(@"\Views\Shared\CommonEmailTemplate.cshtml");
            var templateContent = string.Empty;
            using (var reader = new StreamReader(templateFilePath))
            {
                templateContent = reader.ReadToEnd();
            }
            emailHtmlBody = ParseTemplate(templateContent);
            List<long> PurchaseRequestIds = new List<long>();
            PurchaseRequestIds.Add(PurchaseRequestVM.purchaseRequestModel.PurchaseRequestId);
            var objPrintModelList = PrintDevExpressFromIndex(PurchaseRequestIds);
            PurchaseRequestPrintTemplate report = new PurchaseRequestPrintTemplate();
            report.DataSource = objPrintModelList;
            report.ExportToPdf(stream);
            stream.Seek(0, System.IO.SeekOrigin.Begin);
            pWrapper.UpdateEmailToVendorStatus(emailHtmlBody, stream, PurchaseRequestVM.purchaseRequestModel.PurchaseRequestId, PersonnelId, PurchaseRequestVM.prEmailModel.ToEmailId, PurchaseRequestVM.prEmailModel.CcEmailId, PurchaseRequestVM.prEmailModel.MailBodyComments);
            return Json(new { Result = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
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
        #endregion

        #region V2-1046 Consolidate
        public PartialViewResult GetPurchaseRequestConsolidate()
        {
            PurchaseRequestVM objPRVM = new PurchaseRequestVM();
            LocalizeControls(objPRVM, LocalizeResourceSetConstants.PurchaseRequest);
            return PartialView("_PurchaseRequestConsolidatePopup", objPRVM);
        }
        public string GetPRLineGridDataForConsolidate(int? draw, int? start, int? length, long? PurchaseRequestId = null, string Description = "", string VendorClientLookupId = "", string VendorName = "")
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            start = start.HasValue
          ? start / length
          : 0;
            int skip = start * length ?? 0;
            PurchaseRequestWrapper prWrapper = new PurchaseRequestWrapper(userData);
            List<LineItemModel> lineItems = prWrapper.LineitemsChunkSearchForConsolidate(PurchaseRequestId ?? 0, skip, length ?? 0, order, orderDir, Description, VendorClientLookupId, VendorName);
            long totalRecords = 0;
            long recordsFiltered = 0;
            if (lineItems != null && lineItems.Count > 0)
            {
                recordsFiltered = lineItems[0].TotalCount;
                totalRecords = lineItems[0].TotalCount;
            }
            else
            {
                recordsFiltered = 0;
                totalRecords = 0;
            }
            int initialPage = start.Value;
            var filteredResult = lineItems
              .ToList();

            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult }, JsonSerializerDateSettings);
        }

        public JsonResult PRLineForConsolidateSelectAllData(string order, string orderDir, long? PurchaseRequestId = null, string Description = "", string VendorClientLookupId = "", string VendorName = "")
        {
            List<PurchaseRequestPrintModel> pSearchModelList = new List<PurchaseRequestPrintModel>();
            PurchaseRequestWrapper prWrapper = new PurchaseRequestWrapper(userData);
            List<LineItemModel> lineItemList = prWrapper.LineitemsChunkSearchForConsolidate(PurchaseRequestId ?? 0, 0, 10000, order, orderDir, Description, VendorClientLookupId, VendorName);

            return Json(lineItemList, JsonRequestBehavior.AllowGet);
        }
        public JsonResult PurchaseRequestConsolidate(List<string> PUrchaseRequestLineItemIds, long PurchaseRequestId)
        {
            PurchaseRequestWrapper pWrapper = new PurchaseRequestWrapper(userData);
            var errorList = pWrapper.PRLineItemConsolidateProcess(PUrchaseRequestLineItemIds, PurchaseRequestId);
            if (errorList != null && errorList.Count > 0)
            {
                return Json(errorList, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Result = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        #region V2-1032 SingleStockLineItem LineItem

        #region Add
        public PartialViewResult AddPartInInventorySingleStockLineItemDynamic(long PurchaseRequestId, string ClientLookupId, long vendorId = 0, long StoreroomId = 0, long LineItemId = 0, String Status = "")
        {
            PurchaseRequestVM objPurchaseRequestVM = new PurchaseRequestVM();
            PurchaseRequestWrapper pWrapper = new PurchaseRequestWrapper(userData);
            PurchaseRequestModel objPurchaseRequestModel = new PurchaseRequestModel();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            List<DataContracts.LookupList> UOMs = new List<DataContracts.LookupList>();
            objPurchaseRequestVM.AddPRLineItemPartInInventory = new Models.PurchaseRequest.UIConfiguration.AddPRLineItemPartInInventoryModelDynamic();
            objPurchaseRequestVM.UIConfigurationDetails = new RetrieveDataForUIConfiguration()
                                            .Retrieve(DataDictionaryViewNameConstant.AddPurchaseRequestLineItemStockSingle, userData);

            var ChargeTypeList = UtilityFunction.populateChargeTypeForPurchaseRequestLineItem();
            if (ChargeTypeList != null)
            {
                objPurchaseRequestVM.ChargeTypeList = ChargeTypeList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
            }
            objPurchaseRequestVM.AddPRLineItemPartInInventory.AccountId = userData.Site.NonStockAccountId;//RNJ
            var AcclookUpList = GetAccountByActiveState(true);
            if (AcclookUpList != null && userData.Site.NonStockAccountId > 0)
            {
                AcclookUpList = AcclookUpList.Where(x => x.AccountId == userData.Site.NonStockAccountId).ToList();
                if (AcclookUpList != null)
                {
                    objPurchaseRequestVM.AddPRLineItemPartInInventory.AccountClientLookupId = AcclookUpList.Select(x => new SelectListItem { Text = x.Account + " - " + x.Name, Value = x.AccountId.ToString() }).Where(x => x.Value == userData.Site.NonStockAccountId.ToString()).First().Text;
                }
                else
                {
                    objPurchaseRequestVM.AddPRLineItemPartInInventory.AccountClientLookupId = "";
                }
            }
            else
            {
                objPurchaseRequestVM.AddPRLineItemPartInInventory.AccountClientLookupId = "";
            }
            var AllLookUps = commonWrapper.GetAllLookUpList();
            IList<string> LookupNames = LookupNames = objPurchaseRequestVM.UIConfigurationDetails.ToList()
                                        .Where(x => x.LookupType == "LookupList" && !string.IsNullOrEmpty(x.LookupName))
                                        .Select(s => s.LookupName)
                                        .ToList();
            if (AllLookUps != null)
            {
                objPurchaseRequestVM.AllRequiredLookUplist = AllLookUps.Where(x => LookupNames.Contains(x.ListName))
                                                      .Select(s => new Client.Models.UILookupList
                                                      { text = s.ListValue + " - " + s.Description, value = s.ListValue, lookupname = s.ListName })
                                                      .ToList();
                UOMs = AllLookUps.Where(x => x.ListName == LookupListConstants.Unit_Of_Measure).ToList();
            }
            if (UOMs != null)
            {
                objPurchaseRequestVM.AddPRLineItemPartInInventory.UOMList = UOMs.Select(x => new SelectListItem
                {
                    Text = x.ListValue + " - " + x.Description,
                    Value = x.ListValue.ToString()
                });
            }

            objPurchaseRequestVM.AddPRLineItemPartInInventory.PurchaseRequestId = PurchaseRequestId;
            objPurchaseRequestVM.AddPRLineItemPartInInventory.ViewName = UiConfigConstants.PurchaseRequestLineItemAdd;
            objPurchaseRequestVM.AddPRLineItemPartInInventory.ClientLookupId = ClientLookupId;
            objPurchaseRequestVM.AddPRLineItemPartInInventory.IsShopingCart = userData.Site.ShoppingCart;
            if (objPurchaseRequestVM.AddPRLineItemPartInInventory.IsShopingCart)
            {
                objPurchaseRequestVM.AddPRLineItemPartInInventory.RequiredDate = DateTime.Today.AddDays(7);
            }
            objPurchaseRequestVM.AddPRLineItemPartInInventory.PartClientLookupId = "";
            objPurchaseRequestVM.AddPRLineItemPartInInventory.StoreroomId = StoreroomId;//RKL Mail - Issues with the alternate (Single Part) Line Item Add/Edit (Purchase Request and Purchase Order)

            objPurchaseRequestVM.AddPRLineItemPartInInventory.Status = Status;
            objPurchaseRequestVM.AddPRLineItemPartInInventory.IsOnOderCheck = userData.Site.OnOrderCheck;
            objPurchaseRequestVM.AddPRLineItemPartInInventory.MultiStoreroom = userData.DatabaseKey.Client.UseMultiStoreroom;
            objPurchaseRequestVM.AddPRLineItemPartInInventory.VendorId = vendorId;
            LocalizeControls(objPurchaseRequestVM, LocalizeResourceSetConstants.PurchaseRequest);
            return PartialView("_AddLineItemPartInInventoryDynamic", objPurchaseRequestVM);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddSingleStockLineItemPartInInventoryDynamic(PurchaseRequestVM objVM, string Command)
        {
            PurchaseRequestLineItem prLineItem = new PurchaseRequestLineItem();
            PurchaseRequestWrapper pWrapper = new PurchaseRequestWrapper(userData);
            string ModelValidationFailedMessage = string.Empty;

            if (ModelState.IsValid)
            {
                prLineItem = pWrapper.AddPartInInventorySigleStockDynamic(objVM);
                if (prLineItem.ErrorMessages != null && prLineItem.ErrorMessages.Count > 0)
                {
                    return Json(prLineItem.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), Command = Command, purchaserequestid = objVM.AddPRLineItemPartInInventory.PurchaseRequestId }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion
        #region Edit
        public ActionResult EditPRPartInInventorySingleStockDynamic(long LineItemId, long PurchaseRequestId, string ClientLookupId, string status, long StoreroomId = 0, long VendorId = 0)
        {
            PurchaseRequestVM objPurchaseRequestVM = new PurchaseRequestVM();
            PurchaseRequestWrapper pWrapper = new PurchaseRequestWrapper(userData);
            objPurchaseRequestVM.EditPRLineItemPartInInventorySingleStock = pWrapper.GetPRLineItemInInventorySingleStockByIdDynamic(LineItemId, PurchaseRequestId);
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            objPurchaseRequestVM.UIConfigurationDetails = new RetrieveDataForUIConfiguration()
       .Retrieve(DataDictionaryViewNameConstant.EditPurchaseRequestLineItemStockSingle, userData);

            var AllLookUps = commonWrapper.GetAllLookUpList();
            IList<string> LookupNames = LookupNames = objPurchaseRequestVM.UIConfigurationDetails.ToList()
                                        .Where(x => x.LookupType == "LookupList" && !string.IsNullOrEmpty(x.LookupName))
                                        .Select(s => s.LookupName)
                                        .ToList();
            if (AllLookUps != null)
            {
                objPurchaseRequestVM.AllRequiredLookUplist = AllLookUps.Where(x => LookupNames.Contains(x.ListName))
                                                      .Select(s => new Client.Models.UILookupList
                                                      { text = s.ListValue + " - " + s.Description, value = s.ListValue, lookupname = s.ListName })
                                                      .ToList();
            }
            objPurchaseRequestVM.EditPRLineItemPartInInventorySingleStock.ViewName = UiConfigConstants.PurchaseRequestLineItemEdit;
            objPurchaseRequestVM.EditPRLineItemPartInInventorySingleStock.ClientLookupId = ClientLookupId;
            objPurchaseRequestVM.EditPRLineItemPartInInventorySingleStock.ChargeToClientLookupIdToShow = objPurchaseRequestVM.EditPRLineItemPartInInventorySingleStock.ChargeToClientLookupId;
            objPurchaseRequestVM.EditPRLineItemPartInInventorySingleStock.IsShopingCart = userData.Site.ShoppingCart;
            objPurchaseRequestVM.EditPRLineItemPartInInventorySingleStock.Status = status;
            objPurchaseRequestVM.EditPRLineItemPartInInventorySingleStock.IsOnOderCheck = userData.Site.OnOrderCheck;
            objPurchaseRequestVM.EditPRLineItemPartInInventorySingleStock.MultiStoreroom = userData.DatabaseKey.Client.UseMultiStoreroom;
            objPurchaseRequestVM.EditPRLineItemPartInInventorySingleStock.StoreroomId = StoreroomId;//RKL Mail - Issues with the alternate (Single Part) Line Item Add/Edit (Purchase Request and Purchase Order)
            objPurchaseRequestVM.EditPRLineItemPartInInventorySingleStock.VendorId = VendorId;
            LocalizeControls(objPurchaseRequestVM, LocalizeResourceSetConstants.PurchaseRequest);
            return PartialView("_EditPartInInventorySingleStockDynamic", objPurchaseRequestVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPRPartInInventorySingleStockDynamic(PurchaseRequestVM PurchaseRequestVM)
        {
            string ModelValidationFailedMessage = string.Empty;
            PurchaseRequestWrapper pWrapper = new PurchaseRequestWrapper(userData);
            ModelState.Remove("EditPRLineItemPartInInventorySingleStock.ChargeToClientLookupIdToShow");
            if (ModelState.IsValid)
            {
                var lineItem = pWrapper.UpdatePRPartInInventorySingleStockDynamic(PurchaseRequestVM);
                if (lineItem.ErrorMessages.Count == 0)
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), purchaserequestid = PurchaseRequestVM.EditPRLineItemPartInInventorySingleStock.PurchaseRequestId }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(lineItem.ErrorMessages, JsonRequestBehavior.AllowGet);
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

        #region V2-1063 MaterialRequest Item
        public PartialViewResult GetPurchaseRequestMaterialRequest()
        {
            PurchaseRequestVM objPRVM = new PurchaseRequestVM();
            LocalizeControls(objPRVM, LocalizeResourceSetConstants.PurchaseRequest);
            return PartialView("_PurchaseRequestMaterialRequestPopup", objPRVM);
        }
        public string GetGridDataForMaterialRequest(int? draw, int? start, int? length, long? PurchaseRequestId = null, string Description = "", string PartClientLookupId = "", string WorkOrderClientLookupId = "")
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            start = start.HasValue
          ? start / length
          : 0;
            int skip = start * length ?? 0;
            PurchaseRequestWrapper prWrapper = new PurchaseRequestWrapper(userData);
            List<LineItemModel> lineItems = prWrapper.LineitemsChunkSearchForMaterialRequest(PurchaseRequestId ?? 0, skip, length ?? 0, order, orderDir, Description, PartClientLookupId, WorkOrderClientLookupId);
            long totalRecords = 0;
            long recordsFiltered = 0;
            if (lineItems != null && lineItems.Count > 0)
            {
                recordsFiltered = lineItems[0].TotalCount;
                totalRecords = lineItems[0].TotalCount;
            }
            else
            {
                recordsFiltered = 0;
                totalRecords = 0;
            }
            int initialPage = start.Value;
            var filteredResult = lineItems
              .ToList();

            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult }, JsonSerializerDateSettings);
        }

        public JsonResult MaterialRequestSelectAllData(string order, string orderDir, long? PurchaseRequestId = null, string Description = "", string PartClientLookupId = "", string WorkOrderClientLookupId = "")
        {
            List<PurchaseRequestPrintModel> pSearchModelList = new List<PurchaseRequestPrintModel>();
            PurchaseRequestWrapper prWrapper = new PurchaseRequestWrapper(userData);
            List<LineItemModel> lineItemList = prWrapper.LineitemsChunkSearchForMaterialRequest(PurchaseRequestId ?? 0, 0, 10000, order, orderDir, Description, PartClientLookupId, WorkOrderClientLookupId);

            return Json(lineItemList, JsonRequestBehavior.AllowGet);
        }
        public JsonResult PurchaseRequestMaterialRequest(List<int> EstimatedCostIds, long PurchaseRequestId)
        {
            PurchaseRequestWrapper pWrapper = new PurchaseRequestWrapper(userData);
            var errorList = pWrapper.PRLineItemMaterialRequestProcess(EstimatedCostIds, PurchaseRequestId);
            if (errorList != null && errorList.Count > 0)
            {
                return Json(errorList, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Result = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region V2-1112 ConvertCustomEPMPRToPO
        [HttpPost]
        public JsonResult ConvertCustomEPMPRToPO(PurchaseRequestVM model)
        {
            PurchaseRequestWrapper pWrapper = new PurchaseRequestWrapper(userData);
            List<string> errMsgList = new List<string>();
            ConvertToPOModel ConvertToPurchaseOrder = new ConvertToPOModel();
            
                string errormsg = "Error";
                string errormsgAssetMgt = "ErrorAssetMgt";

            var (convertToPO, isError, isAssetMgtError) = pWrapper.ConvertCustomEPMPRToPO(model);
            ConvertToPurchaseOrder = convertToPO;

            if (isError)
            {
                return Json(new { data = errormsg }, JsonRequestBehavior.AllowGet);
            }

           else if (isAssetMgtError)
            {
                return Json(new { data = errormsgAssetMgt }, JsonRequestBehavior.AllowGet);
            }

           else if (ConvertToPurchaseOrder?.Message == JsonReturnEnum.failed.ToString())
            {
                errMsgList.Add($"Purchase Request {ConvertToPurchaseOrder.ClientLookupId} failed to convert to Purchase Order");
            }
            else {
                errMsgList.AddRange(SendPunchOutOrder(model.purchaseRequestModel.PurchaseRequestId, ConvertToPurchaseOrder));
            }

            if (errMsgList.Count > 0)
            {
                return Json(new { data = errMsgList }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { data = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

        private List<string> SendPunchOutOrder(long purchaseRequestId, ConvertToPOModel ConvertToPurchaseOrder)
        {
            VendorWrapper vWrapper = new VendorWrapper(userData);
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            SiteSetUpWrapper siteSetUpWrapper = new SiteSetUpWrapper(userData);
            PurchaseOrderWrapper pWrapper = new PurchaseOrderWrapper(userData);
            PurchaseRequestWrapper purchaseRequestWrapper = new PurchaseRequestWrapper(userData);
            PunchoutAPIResponse punchoutAPIResponse;
            List<string> errMsgList = new List<string>();

            Models.VendorsModel objVen;
            Personnel personnel;
            Site site;
            PurchaseRequestModel purchaseRequestModel;

            long ClientId = userData.DatabaseKey.Client.ClientId;
            long SiteId, PurchaseOrderId, VendorId, CreatedBy_PersonnelId;

            
                objVen = new Models.VendorsModel();
                personnel = new Personnel();
                site = new Site();
                purchaseRequestModel = new PurchaseRequestModel();
                punchoutAPIResponse = new PunchoutAPIResponse();

                purchaseRequestModel = purchaseRequestWrapper.GetPurchaseRequestDetailById(purchaseRequestId);
                if (purchaseRequestModel.IsPunchOut)
                {
                    SiteId = purchaseRequestModel.SiteId;
                    PurchaseOrderId = purchaseRequestModel.PurchaseOrderId;
                    VendorId = purchaseRequestModel.VendorId ?? 0;
                    CreatedBy_PersonnelId = purchaseRequestModel.CreatedBy_PersonnelId;

                    objVen = vWrapper.populateVendorDetails(VendorId);

                if (objVen.AutoSendPunchOutPO)
                    {
                        Task[] tasks = new Task[2];
                        tasks[0] = Task.Factory.StartNew(() => personnel = commonWrapper.GetPersonnelByPersonnelId(CreatedBy_PersonnelId));
                        tasks[1] = Task.Factory.StartNew(() => site = siteSetUpWrapper.RetriveSiteDetailsByClientAndSite(ClientId, SiteId));
                        Task.WaitAll(tasks);

                        if (!tasks[0].IsFaulted && tasks[0].IsCompleted && !tasks[1].IsFaulted && tasks[1].IsCompleted)
                        {
                            var destinationURL = objVen.SendPunchoutPOURL;
                            if (string.IsNullOrEmpty(destinationURL))
                            {
                                errMsgList.Add("Purchase Request " + purchaseRequestModel.ClientLookupId + " failed to Send Purchase Order as Vendor's Send Punchout PO URL can't be empty.");
                            }
                            else
                            {
                                var requestToSend = pWrapper.GetPunchoutOrderMessageData(PurchaseOrderId, objVen, personnel, site, SiteId);
                                punchoutAPIResponse = pWrapper.postXMLData(destinationURL, requestToSend);

                                if (punchoutAPIResponse.ResponseCode == 200 && punchoutAPIResponse.ResponseMessage.ToLower() == "ok")
                                {
                                    pWrapper.UpdatePOOnOrderSetupResponse(PurchaseOrderId, ClientId);
                                    SiteId = userData.DatabaseKey.User.DefaultSiteId;
                                    pWrapper.UpdatePOEventLogOnOrderSetupResponse(PurchaseOrderId, ClientId, SiteId);
                                }
                                else
                                {
                                    errMsgList.Add("Purchase Request " + purchaseRequestModel.ClientLookupId + " failed to Send Purchase Order - " + punchoutAPIResponse.ResponseText);
                                }
                            }

                        }
                    }
                }
            return errMsgList;
        }
        #endregion
    }
}

