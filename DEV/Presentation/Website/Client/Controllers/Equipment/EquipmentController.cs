using Client.ActionFilters;
using Client.BusinessWrapper;
using Client.BusinessWrapper.Common;
using Client.BusinessWrapper.Configuration.SiteSetUp;
using Client.Common;
using Client.Controllers.Common;
using Client.Localization;
using Client.Models;
using Client.Models.Common;
using Client.Models.Configuration.LookupLists;
using Client.Models.Equipment;
using Client.Models.Equipment.UIConfiguration;
using Common.Constants;
using Common.Extensions;
using DataContracts;
using Newtonsoft.Json;
using PagedList;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using static Client.Models.Common.UserMentionDataModel;
using Rotativa;
using Client.Models.Parts;
using DevExpress.XtraReports.UI;
using Client.DevExpressReport;
using Client.DevExpressReport.EPM;
using Client.Models.Work_Order;

namespace Client.Controllers
{

    public class EquipmentController : SomaxBaseController
    {
        #region Equipment Search
        [CheckUserSecurity(securityType = SecurityConstants.Equipment)]
        public ActionResult Index()
        {
            EquipmentWrapper eWrapper = new EquipmentWrapper(userData);
            EquipmentCombined objComb = new EquipmentCombined();
            objComb.EquipModel = new EquipmentModel();
            objComb.EquipModel.UserId = userData.DatabaseKey.User.UserInfoId;
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            List<DataContracts.LookupList> TypeList = new List<DataContracts.LookupList>();
            var AllLookUps = commonWrapper.GetAllLookUpList();
            if (AllLookUps != null)
            {
                TypeList = AllLookUps.Where(x => x.ListName == LookupListConstants.Equipment_EquipType).ToList();
            }
            if (TypeList != null)
            {
                objComb.LookupTypeList = TypeList.Select(x => new SelectListItem { Text = x.ListValue + " - " + x.Description, Value = x.ListValue }).ToList();
            }

            var ast1 = eWrapper.GetAssetGroup1Dropdowndata();
            if (ast1 != null)
            {
                objComb.EquipModel.AssetGroup1List = ast1.Select(x => new SelectListItem { Text = x.AssetGroup1DescWithClientLookupId, Value = x.AssetGroup1Id.ToString() });
            }
            var ast2 = eWrapper.GetAssetGroup2Dropdowndata();
            if (ast2 != null)
            {
                objComb.EquipModel.AssetGroup2List = ast2.Select(x => new SelectListItem { Text = x.AssetGroup2DescWithClientLookupId, Value = x.AssetGroup2Id.ToString() });
            }
            var ast3 = eWrapper.GetAssetGroup3Dropdowndata();
            if (ast3 != null)
            {
                objComb.EquipModel.AssetGroup3List = ast3.Select(x => new SelectListItem { Text = x.AssetGroup3DescWithClientLookupId, Value = x.AssetGroup3Id.ToString() });
            }
            //V2-379
            var AcclookUpList = GetAccountByActiveState(true);
            if (AcclookUpList != null)
            {
                objComb.EquipModel.AccountList = AcclookUpList.Select(x => new SelectListItem { Text = x.Account + " - " + x.Name, Value = x.Account });
            }

            string mode = Convert.ToString(TempData["Mode"]);
            if (mode == "add")
            {

                objComb.EquipModel.BusinessType = userData.DatabaseKey.Client.BusinessType;
                objComb.EquipModel.PlantLocation = userData.Site.PlantLocation;
                this.GetAssetGroupHeaderName(objComb);
                var VenlookUpList = GetLookupList_Vendor();
                if (VenlookUpList != null)
                {
                    objComb.EquipModel.VendorList = VenlookUpList.Select(x => new SelectListItem { Text = x.Vendor + " - " + x.Name, Value = x.Vendor });
                }
                var AssetCategoryList = UtilityFunction.AssetCategoryList();
                if (AssetCategoryList != null)
                {
                    objComb.EquipModel.AssetCategoryList = AssetCategoryList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
                }
                ViewBag.IsEquipAdd = true;
            }
            else if (mode == "adddynamicmobile")
            {
                ViewBag.IsEquipAddDynamic = true;
                objComb._userdata = userData;
            }
            else if (mode == "adddynamic")
            {
                objComb.UIConfigurationDetails = new RetrieveDataForUIConfiguration()
                                                .Retrieve(DataDictionaryViewNameConstant.AddAsset, userData);

                objComb.PlantLocation = userData.Site.PlantLocation;
                objComb.BusinessType = userData.DatabaseKey.Client.BusinessType;

                var VenlookUpList = GetLookupList_Vendor();
                if (VenlookUpList != null)
                {
                    objComb.VendorList = VenlookUpList.Select(x => new SelectListItem { Text = x.Vendor + " - " + x.Name, Value = x.Vendor });
                }
                var AssetCategoryList = UtilityFunction.AssetCategoryList();
                if (AssetCategoryList != null)
                {
                    objComb.AssetCategoryList = AssetCategoryList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
                }
                objComb.AssetGroup1List = objComb.EquipModel.AssetGroup1List;
                objComb.AssetGroup2List = objComb.EquipModel.AssetGroup2List;
                objComb.AssetGroup3List = objComb.EquipModel.AssetGroup3List;

                IList<string> LookupNames = objComb.UIConfigurationDetails.ToList()
                                            .Where(x => x.LookupType == "LookupList" && !string.IsNullOrEmpty(x.LookupName))
                                            .Select(s => s.LookupName)
                                            .ToList();
                objComb.AllRequiredLookUplist = AllLookUps.Where(x => LookupNames.Contains(x.ListName))
                                                          .Select(s => new UILookupList
                                                          { text = s.ListValue + " - " + s.Description, value = s.ListValue, lookupname = s.ListName })
                                                          .ToList();
                objComb.AssetGroup1Label = userData.Site.AssetGroup1Name;
                objComb.AssetGroup2Label = userData.Site.AssetGroup2Name;
                objComb.AssetGroup3Label = userData.Site.AssetGroup3Name;
                ViewBag.IsEquipAddDynamic = true;
                objComb.AddAssetOperation = true;
                objComb._userdata = userData;
                objComb.AddEquipment = new Models.Equipment.UIConfiguration.AddEquipmentModelDynamic();
            }
            else if (mode == "DetailFromWorkOrder" || mode == "RedirectFromWorkOrderPrint")
            {
                ViewBag.IsDetailFromWorkOrder = true;
                ViewBag.DetailFromWorkOrderIsRepairableSpare = false;

                long EquipmentId = Convert.ToInt64(TempData["EquipmentId"]);
                var EquipmentDetails = eWrapper.GetEquipmentDetailsById(EquipmentId);
                if (EquipmentDetails.AssetCategory == AssetCategoryConstant.RepairableSpare)
                {
                    ViewBag.DetailFromWorkOrderIsRepairableSpare = true;
                    var objCombRepairableSpare = RepairableSpareEquipmentDetails(EquipmentId, EquipmentDetails);
                    objComb = objCombRepairableSpare;
                    LocalizeControls(objComb, LocalizeResourceSetConstants.EquipmentDetails);
                    return View(objComb);
                }
                else
                {

                    Task[] tasks = new Task[3];

                    ChangeEquipmentIDModel _ChangeEquipmentIDModel = new ChangeEquipmentIDModel();
                    EquipmentUDF EquipmentUDF = new EquipmentUDF();
                    EquipmentModel equipmentModel = new EquipmentModel();
                    AssetAvailabilityRemoveModel assetAvailabilityRemoveModel = new AssetAvailabilityRemoveModel();
                    AssetAvailabilityUpdateModel assetAvailabilityUpdateModel = new AssetAvailabilityUpdateModel();
                    CommonWrapper objCommonWrapper = new CommonWrapper(userData);
                    tasks[0] = Task.Factory.StartNew(() => objComb.attachmentCount = objCommonWrapper.AttachmentCount(EquipmentId, AttachmentTableConstant.Equipment, userData.Security.Equipment.Edit));
                    tasks[1] = Task.Factory.StartNew(() => EquipmentUDF = eWrapper.RetrieveEquipmentUDFByEquipmentId(EquipmentId));
                    tasks[2] = Task.Factory.StartNew(() => objComb.UIConfigurationDetails = new RetrieveDataForUIConfiguration().Retrieve(DataDictionaryViewNameConstant.ViewAssetWidget, userData));
                    Task.WaitAll(tasks);
                    objComb.EquipData = EquipmentDetails;
                    if (objComb.EquipData.Maint_WarrantyExpire != null && objComb.EquipData.Maint_WarrantyExpire.Value == default(DateTime))
                    {
                        objComb.EquipData.Maint_WarrantyExpire = null;
                    }
                    objComb._EquipmentSummaryModel = GetEquipmentSummary(EquipmentId, objComb.EquipData.ClientLookupId, objComb.EquipData.Name, objComb.EquipData.RemoveFromService, objComb.EquipData.Status);

                    var equipmentDownTimeList = UtilityFunction.EquipmentDownTimeDatesList();
                    if (equipmentDownTimeList != null)
                    {
                        objComb.equipmentDownTimeDateList = equipmentDownTimeList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
                    }
                    var workOrderTypeList = UtilityFunction.WorkOrderDatesList();
                    if (workOrderTypeList != null)
                    {
                        objComb.workOrderTypeDateList = workOrderTypeList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
                    }
                    string temp = string.Empty;
                    temp = EquipmentDetails.ClientLookupId + "][" + EquipmentDetails.Name + "][" + EquipmentDetails.SerialNumber + "][" + EquipmentDetails.Make + "][" + EquipmentDetails.Model;
                    QRCodeModel qRCodeModel = new QRCodeModel();
                    List<string> equipmentClientLookUpNames = new List<string>();
                    equipmentClientLookUpNames.Add(temp);
                    qRCodeModel.EquipmentIdsList = equipmentClientLookUpNames;
                    objComb.qRCodeModel = qRCodeModel;
                    equipmentModel.AlertFollowedEquipment = eWrapper.AlertFollowIdExist(EquipmentId);
                    equipmentModel.BusinessType = userData.DatabaseKey.Client.BusinessType;
                    _ChangeEquipmentIDModel.EquipmentId = Convert.ToInt64(objComb.EquipData.EquipmentId);
                    _ChangeEquipmentIDModel.ClientLookupId = objComb.EquipData.ClientLookupId;
                    _ChangeEquipmentIDModel.OldClientLookupId = objComb.EquipData.ClientLookupId;
                    _ChangeEquipmentIDModel.UpdateIndex = objComb.EquipData.UpdateIndex;
                    objComb.EquipModel = equipmentModel;
                    objComb._ChangeEquipmentIDModel = _ChangeEquipmentIDModel;
                    objComb._CreatedLastUpdatedModel = eWrapper.createdLastUpdatedModel(EquipmentId);
                    objComb.security = this.userData.Security;
                    objComb._userdata = this.userData;
                    objComb.ViewEquipment = new ViewEquipmentModelDynamic();
                    objComb.ViewEquipment = eWrapper.MapEquipmentDataForView(objComb.ViewEquipment, EquipmentDetails);
                    objComb.ViewEquipment = eWrapper.MapEquipmentUDFDataForView(objComb.ViewEquipment, EquipmentUDF);


                    #region Asset availability
                    var removefromServiceReasonCode = objCommonWrapper.GetListFromConstVals(LookupListConstants.RemoveFromServiceReasonCode);
                    if (removefromServiceReasonCode != null)
                    {
                        objComb.LookupRemoveFromServiceReasonCode = removefromServiceReasonCode.Select(x => new SelectListItem { Text = x.text.ToString(), Value = x.value.ToString() });
                        if (objComb.EquipData.RemoveFromServiceReasonCode != "")
                        {
                            objComb.ServiceReasonCode = AssetStatusConstant.Scrapped;
                            if (objComb.EquipData.RemoveFromServiceReasonCode != AssetStatusConstant.Scrapped)
                            {
                                objComb.ServiceReasonCode = objComb.LookupRemoveFromServiceReasonCode.Where(x => x.Value == objComb.EquipData.RemoveFromServiceReasonCode).Select(x => x.Text).FirstOrDefault().ToString();
                            }
                        }
                    }

                    assetAvailabilityRemoveModel.EquipmentId = EquipmentId;
                    assetAvailabilityRemoveModel.RemoveFromService = objComb.EquipData.RemoveFromService;
                    objComb._AssetAvailabilityRemoveModel = assetAvailabilityRemoveModel;
                    assetAvailabilityUpdateModel.EquipmentId = EquipmentId;
                    objComb._AssetAvailabilityUpdateModel = assetAvailabilityUpdateModel;
                    objComb.IsAssetAvailability = userData.Security.Asset_Availability.Access;
                    objComb.RemoveServiceDate = Convert.ToDateTime(objComb.EquipData.RemoveFromServiceDate).ToUserTimeZone(userData.Site.TimeZone);
                    #endregion
                    PartModel objPartModel = new PartModel();
                    if (AllLookUps != null)
                    {
                        var LookupStokeType = AllLookUps.Where(x => x.ListName == LookupListConstants.STOCK_TYPE).ToList();
                        if (LookupStokeType != null)
                        {
                            objPartModel.LookupStokeTypeList = LookupStokeType.Select(x => new SelectListItem { Text = x.ListValue + "  -  " + x.Description, Value = x.ListValue });
                        }

                    }
                    objComb.PartModel = objPartModel;
                }

            }
            #region V2-919
            else if (mode == "DetailFromWorkOrder_Mobile")
            {
                ViewBag.IsDetailFromWorkOrder = true;
                long EquipmentId = Convert.ToInt64(TempData["EquipmentId"]);
                var EquipmentDetails = eWrapper.GetEquipmentDetailsById(EquipmentId);
                Task[] tasks = new Task[1];
                ChangeEquipmentIDModel _ChangeEquipmentIDModel = new ChangeEquipmentIDModel();
                EquipmentModel equipmentModel = new EquipmentModel();
                AssetAvailabilityRemoveModel assetAvailabilityRemoveModel = new AssetAvailabilityRemoveModel();
                AssetAvailabilityUpdateModel assetAvailabilityUpdateModel = new AssetAvailabilityUpdateModel();
                CommonWrapper objCommonWrapper = new CommonWrapper(userData);
                tasks[0] = Task.Factory.StartNew(() => objComb.attachmentCount = objCommonWrapper.AttachmentCount(EquipmentId, AttachmentTableConstant.Equipment, userData.Security.Equipment.Edit));
                Task.WaitAll(tasks);
                objComb.EquipData = EquipmentDetails;
                if (objComb.EquipData.Maint_WarrantyExpire != null && objComb.EquipData.Maint_WarrantyExpire.Value == default(DateTime))
                {
                    objComb.EquipData.Maint_WarrantyExpire = null;
                }
                objComb._EquipmentSummaryModel = GetEquipmentSummary(EquipmentId, objComb.EquipData.ClientLookupId, objComb.EquipData.Name, objComb.EquipData.RemoveFromService, objComb.EquipData.Status);
                var item = EquipmentDetails;
                EquipmentSearchModel eSearchModel = new EquipmentSearchModel();
                eSearchModel.EquipmentId = item.EquipmentId;
                eSearchModel.ClientLookupId = item.ClientLookupId;
                eSearchModel.Name = item.Name;
                eSearchModel.Location = item.Location;
                eSearchModel.SerialNumber = item.SerialNumber;
                eSearchModel.Type = item.Type;
                eSearchModel.Make = item.Make;
                eSearchModel.Model = item.Model;
                eSearchModel.LaborAccountClientLookupId = item.LaborAccountClientLookupId;
                eSearchModel.AssetNumber = item.AssetNumber;
                eSearchModel.AssetGroup1ClientLookupId = Convert.ToString(item.AssetGroup1ClientLookupId);
                eSearchModel.AssetGroup2ClientLookupId = Convert.ToString(item.AssetGroup2ClientLookupId);
                eSearchModel.AssetGroup3ClientLookupId = Convert.ToString(item.AssetGroup3ClientLookupId);
                eSearchModel.RemoveFromService = item.RemoveFromService;
                if (item.RemoveFromServiceDate != null && item.RemoveFromServiceDate == default(DateTime))
                {
                    eSearchModel.RemoveFromServiceDate = null;
                }
                else
                {
                    eSearchModel.RemoveFromServiceDate = item.RemoveFromServiceDate;
                }
                eSearchModel.BusinessType = userData.DatabaseKey.Client.BusinessType;
                objComb.EquipmentDetailsCard = eSearchModel;
                var equipmentDownTimeList = UtilityFunction.EquipmentDownTimeDatesList();
                if (equipmentDownTimeList != null)
                {
                    objComb.equipmentDownTimeDateList = equipmentDownTimeList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
                }
                var workOrderTypeList = UtilityFunction.WorkOrderDatesList();
                if (workOrderTypeList != null)
                {
                    objComb.workOrderTypeDateList = workOrderTypeList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
                }
                string temp = string.Empty;
                temp = EquipmentDetails.ClientLookupId + "][" + EquipmentDetails.Name + "][" + EquipmentDetails.SerialNumber + "][" + EquipmentDetails.Make + "][" + EquipmentDetails.Model;
                QRCodeModel qRCodeModel = new QRCodeModel();
                List<string> equipmentClientLookUpNames = new List<string>();
                equipmentClientLookUpNames.Add(temp);
                qRCodeModel.EquipmentIdsList = equipmentClientLookUpNames;
                objComb.qRCodeModel = qRCodeModel;
                equipmentModel.AlertFollowedEquipment = eWrapper.AlertFollowIdExist(EquipmentId);
                equipmentModel.BusinessType = userData.DatabaseKey.Client.BusinessType;
                _ChangeEquipmentIDModel.EquipmentId = Convert.ToInt64(objComb.EquipData.EquipmentId);
                _ChangeEquipmentIDModel.ClientLookupId = objComb.EquipData.ClientLookupId;
                _ChangeEquipmentIDModel.OldClientLookupId = objComb.EquipData.ClientLookupId;
                _ChangeEquipmentIDModel.UpdateIndex = objComb.EquipData.UpdateIndex;
                objComb.EquipModel = equipmentModel;
                objComb._ChangeEquipmentIDModel = _ChangeEquipmentIDModel;
                objComb._CreatedLastUpdatedModel = eWrapper.createdLastUpdatedModel(EquipmentId);
                objComb.security = this.userData.Security;
                objComb._userdata = this.userData;
                #region Asset availability
                var removefromServiceReasonCode = objCommonWrapper.GetListFromConstVals(LookupListConstants.RemoveFromServiceReasonCode);
                if (removefromServiceReasonCode != null)
                {
                    objComb.LookupRemoveFromServiceReasonCode = removefromServiceReasonCode.Select(x => new SelectListItem { Text = x.text.ToString(), Value = x.value.ToString() });
                    if (objComb.EquipData.RemoveFromServiceReasonCode != "")
                    {
                        objComb.ServiceReasonCode = AssetStatusConstant.Scrapped;
                        if (objComb.EquipData.RemoveFromServiceReasonCode != AssetStatusConstant.Scrapped)
                        {
                            objComb.ServiceReasonCode = objComb.LookupRemoveFromServiceReasonCode.Where(x => x.Value == objComb.EquipData.RemoveFromServiceReasonCode).Select(x => x.Text).FirstOrDefault().ToString();
                        }
                    }
                }

                assetAvailabilityRemoveModel.EquipmentId = EquipmentId;
                assetAvailabilityRemoveModel.RemoveFromService = objComb.EquipData.RemoveFromService;
                objComb._AssetAvailabilityRemoveModel = assetAvailabilityRemoveModel;
                assetAvailabilityUpdateModel.EquipmentId = EquipmentId;
                objComb._AssetAvailabilityUpdateModel = assetAvailabilityUpdateModel;
                objComb.IsAssetAvailability = userData.Security.Asset_Availability.Access;
                objComb.RemoveServiceDate = Convert.ToDateTime(objComb.EquipData.RemoveFromServiceDate).ToUserTimeZone(userData.Site.TimeZone);
                #endregion
                PartModel objPartModel = new PartModel();
                if (AllLookUps != null)
                {
                    var LookupStokeType = AllLookUps.Where(x => x.ListName == LookupListConstants.STOCK_TYPE).ToList();
                    if (LookupStokeType != null)
                    {
                        objPartModel.LookupStokeTypeList = LookupStokeType.Select(x => new SelectListItem { Text = x.ListValue + "  -  " + x.Description, Value = x.ListValue });
                    }

                }
                objComb.PartModel = objPartModel;
            }
            #endregion
            #region V2-1147
            else if (mode == "DetailFromNotification")
            {
                ViewBag.IsDetailFromNotification = true;
                ViewBag.DetailFromNotificationIsRepairableSpare = false;
                ViewBag.DetailFromNotificationAlertType = Convert.ToString(TempData["AlertName"]);
                long EquipmentId = Convert.ToInt64(TempData["EquipmentId"]);
                var EquipmentDetails = eWrapper.GetEquipmentDetailsById(EquipmentId);
                if (EquipmentDetails.AssetCategory == AssetCategoryConstant.RepairableSpare)
                {
                    ViewBag.DetailFromNotificationIsRepairableSpare = true;
                    var objCombRepairableSpare = RepairableSpareEquipmentDetails(EquipmentId, EquipmentDetails);
                    objComb = objCombRepairableSpare;
                    LocalizeControls(objComb, LocalizeResourceSetConstants.EquipmentDetails);
                    return View(objComb);
                }
                else
                {

                    Task[] tasks = new Task[3];

                    ChangeEquipmentIDModel _ChangeEquipmentIDModel = new ChangeEquipmentIDModel();
                    EquipmentUDF EquipmentUDF = new EquipmentUDF();
                    EquipmentModel equipmentModel = new EquipmentModel();
                    AssetAvailabilityRemoveModel assetAvailabilityRemoveModel = new AssetAvailabilityRemoveModel();
                    AssetAvailabilityUpdateModel assetAvailabilityUpdateModel = new AssetAvailabilityUpdateModel();
                    CommonWrapper objCommonWrapper = new CommonWrapper(userData);
                    tasks[0] = Task.Factory.StartNew(() => objComb.attachmentCount = objCommonWrapper.AttachmentCount(EquipmentId, AttachmentTableConstant.Equipment, userData.Security.Equipment.Edit));
                    tasks[1] = Task.Factory.StartNew(() => EquipmentUDF = eWrapper.RetrieveEquipmentUDFByEquipmentId(EquipmentId));
                    tasks[2] = Task.Factory.StartNew(() => objComb.UIConfigurationDetails = new RetrieveDataForUIConfiguration().Retrieve(DataDictionaryViewNameConstant.ViewAssetWidget, userData));
                    Task.WaitAll(tasks);
                    objComb.EquipData = EquipmentDetails;
                    if (objComb.EquipData.Maint_WarrantyExpire != null && objComb.EquipData.Maint_WarrantyExpire.Value == default(DateTime))
                    {
                        objComb.EquipData.Maint_WarrantyExpire = null;
                    }
                    objComb._EquipmentSummaryModel = GetEquipmentSummary(EquipmentId, objComb.EquipData.ClientLookupId, objComb.EquipData.Name, objComb.EquipData.RemoveFromService, objComb.EquipData.Status);

                    var equipmentDownTimeList = UtilityFunction.EquipmentDownTimeDatesList();
                    if (equipmentDownTimeList != null)
                    {
                        objComb.equipmentDownTimeDateList = equipmentDownTimeList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
                    }
                    var workOrderTypeList = UtilityFunction.WorkOrderDatesList();
                    if (workOrderTypeList != null)
                    {
                        objComb.workOrderTypeDateList = workOrderTypeList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
                    }
                    string temp = string.Empty;
                    temp = EquipmentDetails.ClientLookupId + "][" + EquipmentDetails.Name + "][" + EquipmentDetails.SerialNumber + "][" + EquipmentDetails.Make + "][" + EquipmentDetails.Model;
                    QRCodeModel qRCodeModel = new QRCodeModel();
                    List<string> equipmentClientLookUpNames = new List<string>();
                    equipmentClientLookUpNames.Add(temp);
                    qRCodeModel.EquipmentIdsList = equipmentClientLookUpNames;
                    objComb.qRCodeModel = qRCodeModel;
                    equipmentModel.AlertFollowedEquipment = eWrapper.AlertFollowIdExist(EquipmentId);
                    equipmentModel.BusinessType = userData.DatabaseKey.Client.BusinessType;
                    _ChangeEquipmentIDModel.EquipmentId = Convert.ToInt64(objComb.EquipData.EquipmentId);
                    _ChangeEquipmentIDModel.ClientLookupId = objComb.EquipData.ClientLookupId;
                    _ChangeEquipmentIDModel.OldClientLookupId = objComb.EquipData.ClientLookupId;
                    _ChangeEquipmentIDModel.UpdateIndex = objComb.EquipData.UpdateIndex;
                    objComb.EquipModel = equipmentModel;
                    objComb._ChangeEquipmentIDModel = _ChangeEquipmentIDModel;
                    objComb._CreatedLastUpdatedModel = eWrapper.createdLastUpdatedModel(EquipmentId);
                    objComb.security = this.userData.Security;
                    objComb._userdata = this.userData;
                    objComb.ViewEquipment = new ViewEquipmentModelDynamic();
                    objComb.ViewEquipment = eWrapper.MapEquipmentDataForView(objComb.ViewEquipment, EquipmentDetails);
                    objComb.ViewEquipment = eWrapper.MapEquipmentUDFDataForView(objComb.ViewEquipment, EquipmentUDF);


                    #region Asset availability
                    var removefromServiceReasonCode = objCommonWrapper.GetListFromConstVals(LookupListConstants.RemoveFromServiceReasonCode);
                    if (removefromServiceReasonCode != null)
                    {
                        objComb.LookupRemoveFromServiceReasonCode = removefromServiceReasonCode.Select(x => new SelectListItem { Text = x.text.ToString(), Value = x.value.ToString() });
                        if (objComb.EquipData.RemoveFromServiceReasonCode != "")
                        {
                            objComb.ServiceReasonCode = AssetStatusConstant.Scrapped;
                            if (objComb.EquipData.RemoveFromServiceReasonCode != AssetStatusConstant.Scrapped)
                            {
                                objComb.ServiceReasonCode = objComb.LookupRemoveFromServiceReasonCode.Where(x => x.Value == objComb.EquipData.RemoveFromServiceReasonCode).Select(x => x.Text).FirstOrDefault().ToString();
                            }
                        }
                    }

                    assetAvailabilityRemoveModel.EquipmentId = EquipmentId;
                    assetAvailabilityRemoveModel.RemoveFromService = objComb.EquipData.RemoveFromService;
                    objComb._AssetAvailabilityRemoveModel = assetAvailabilityRemoveModel;
                    assetAvailabilityUpdateModel.EquipmentId = EquipmentId;
                    objComb._AssetAvailabilityUpdateModel = assetAvailabilityUpdateModel;
                    objComb.IsAssetAvailability = userData.Security.Asset_Availability.Access;
                    objComb.RemoveServiceDate = Convert.ToDateTime(objComb.EquipData.RemoveFromServiceDate).ToUserTimeZone(userData.Site.TimeZone);
                    #endregion
                    PartModel objPartModel = new PartModel();
                    if (AllLookUps != null)
                    {
                        var LookupStokeType = AllLookUps.Where(x => x.ListName == LookupListConstants.STOCK_TYPE).ToList();
                        if (LookupStokeType != null)
                        {
                            objPartModel.LookupStokeTypeList = LookupStokeType.Select(x => new SelectListItem { Text = x.ListValue + "  -  " + x.Description, Value = x.ListValue });
                        }

                    }
                    objComb.PartModel = objPartModel;
                }

            }
            #endregion
            objComb.security = this.userData.Security;
            string localurl = string.Empty;
            localurl = GetUrl();
            objComb.EquipModel.hdLoginId = userData.LoginAuditing.SessionId;
            objComb.EquipModel.localurl = localurl;
            //V2-636 
            var StatusList = UtilityFunction.InactiveActiveStatusTypesforAsset();
            if (StatusList != null)
            {
                objComb.InactiveFlagList = StatusList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
            }
            //V2-636  Asset Availability
            var AssetAvailability = commonWrapper.GetListFromConstVals(LookupListConstants.Asset_Availability);
            if (AssetAvailability != null)
            {
                objComb.LookupAssetAvailability = AssetAvailability.Select(x => new SelectListItem { Text = x.text.ToString(), Value = x.value.ToString() });
            }
            objComb._userdata = userData;
            this.GetAssetGroupHeaderName(objComb);
            #region V2-1115
            bool EPMInvoiceImportInUse = false;

            var InterfacePropData = (List<InterfacePropModel>)Session["InterfacePropData"];
            if (InterfacePropData != null && InterfacePropData.Count > 0)
            {
                EPMInvoiceImportInUse = InterfacePropData.Where(x => x.InterfaceType == InterfacePropConstants.EPMInvoiceImport).Select(x => x.InUse).FirstOrDefault();
            }
            objComb.EPMInvoiceImportInUse = EPMInvoiceImportInUse;
            #endregion
            LocalizeControls(objComb, LocalizeResourceSetConstants.EquipmentDetails);
            return View(objComb);
        }

        private void GetAssetGroupHeaderName(EquipmentCombined objComb)
        {
            objComb.EquipModel.AssetGroup1Name = String.IsNullOrEmpty(this.userData.Site.AssetGroup1Name) ? "Asset Group 1" : this.userData.Site.AssetGroup1Name;
            objComb.EquipModel.AssetGroup2Name = String.IsNullOrEmpty(this.userData.Site.AssetGroup1Name) ? "Asset Group 2" : this.userData.Site.AssetGroup2Name;
            objComb.EquipModel.AssetGroup3Name = String.IsNullOrEmpty(this.userData.Site.AssetGroup1Name) ? "Asset Group 3" : this.userData.Site.AssetGroup3Name;

        }

        [HttpGet]
        public JsonResult GetEquipment(int inactiveFlag = 1, string ClientLookupId = "", string Name = "", string Location = "",
                                string SerialNumber = "", string Type = "", string Make = "", string Model = "", string LaborAccountClientLookupId = "", string AssetNumber = "", string Area_Desc = "", int AssetGroup1Id = 0, int AssetGroup2Id = 0, int AssetGroup3Id = 0, string searchText = ""
        )
        {
            EquipmentWrapper eWrapper = new EquipmentWrapper(userData);
            var equipmentList = eWrapper.RetrieveequipmentDetailsByInactiveFlag(inactiveFlag);

            equipmentList = GetEquipmentsSearchResult(equipmentList, ClientLookupId, Name, Location, SerialNumber, Type, Make, Model, LaborAccountClientLookupId, AssetNumber, Area_Desc, AssetGroup1Id, AssetGroup2Id, AssetGroup3Id, searchText);
            var jsonResult = Json(equipmentList, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public string GetequipmentGridData(int? draw, int? start, int? length, int customQueryDisplayId = 1, string ClientLookupId = "", string Name = "", string Location = "",
                               string SerialNumber = "", string Type = "", string Make = "", string Model = "", string LaborAccountClientLookupId = "", string AssetNumber = "",
                               string SearchText = "", int AssetGroup1Id = 0, int AssetGroup2Id = 0, int AssetGroup3Id = 0, string Order = "1", string AssetAvailability = ""
       )
        {

            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            EquipmentSearchModel eSearchModel;
            List<EquipmentSearchModel> eSearchModelList = new List<EquipmentSearchModel>();
            SearchText = SearchText.Replace("%", "[%]");
            ClientLookupId = ClientLookupId.Replace("%", "[%]");
            Name = Name.Replace("%", "[%]");
            Location = Location.Replace("%", "[%]");
            SerialNumber = SerialNumber.Replace("%", "[%]");
            Make = Make.Replace("%", "[%]");
            Model = Model.Replace("%", "[%]");
            AssetNumber = AssetNumber.Replace("%", "[%]");
            SerialNumber = SerialNumber.Replace("%", "[%]");
            LaborAccountClientLookupId = LaborAccountClientLookupId.Replace("%", "[%]");
            start = start.HasValue
               ? start / length
               : 0;
            int skip = start * length ?? 0;
            List<string> typeList = new List<string>();
            EquipmentWrapper eWrapper = new EquipmentWrapper(userData);
            var equipmentList = eWrapper.GetEquipmentGridData(customQueryDisplayId, Order, orderDir, skip, length ?? 0, ClientLookupId, Name, Location, SerialNumber, Type, Make, Model, LaborAccountClientLookupId, AssetNumber, SearchText, AssetGroup1Id, AssetGroup2Id, AssetGroup3Id, AssetAvailability);

            var totalRecords = 0;
            var recordsFiltered = 0;

            if (equipmentList != null && equipmentList.Count > 0)
            {
                recordsFiltered = equipmentList[0].TotalCount;
                totalRecords = equipmentList[0].TotalCount;
            }
            else
            {
                recordsFiltered = 0;
                totalRecords = 0;
            }
            int initialPage = start.Value;

            var filteredResult = equipmentList
              .ToList();
            foreach (var item in filteredResult)
            {
                eSearchModel = new EquipmentSearchModel();
                eSearchModel.EquipmentId = item.EquipmentId;
                eSearchModel.ClientLookupId = item.ClientLookupId;
                eSearchModel.Name = item.Name;
                // RKL - 2020-12-11 - Not using the Location table anymore
                //eSearchModel.Location = (userData.DatabaseKey.Client.BusinessType.ToUpper() == BusinessTypeConstants.Facilities) ? item.LocationIdClientLookupId : item.Location;
                eSearchModel.Location = item.Location;
                eSearchModel.SerialNumber = item.SerialNumber;
                eSearchModel.Type = item.Type;
                eSearchModel.Make = item.Make;
                eSearchModel.Model = item.Model;
                eSearchModel.LaborAccountClientLookupId = item.LaborAccountClientLookupId;
                eSearchModel.AssetNumber = item.AssetNumber;
                eSearchModel.AssetGroup1ClientLookupId = Convert.ToString(item.AssetGroup1ClientLookupId);
                eSearchModel.AssetGroup2ClientLookupId = Convert.ToString(item.AssetGroup2ClientLookupId);
                eSearchModel.AssetGroup3ClientLookupId = Convert.ToString(item.AssetGroup3ClientLookupId);
                //V2-636 
                eSearchModel.RemoveFromService = item.RemoveFromService;
                if (item.RemoveFromServiceDate != null && item.RemoveFromServiceDate == default(DateTime))
                {
                    eSearchModel.RemoveFromServiceDate = null;
                }
                else
                {
                    eSearchModel.RemoveFromServiceDate = item.RemoveFromServiceDate;
                }
                eSearchModel.BusinessType = userData.DatabaseKey.Client.BusinessType;
                eSearchModelList.Add(eSearchModel);
            }

            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = eSearchModelList, }, JsonSerializerDateSettings);
        }

        public string GetEquipmentPrintData(string _colname, string _coldir, int? draw, int? start, int? length, int customQueryDisplayId = 1, string _ClientLookupId = "", string _Name = "", string _Location = "", string _SerialNumber = "", string _Type = "", string _Make = "", string _Model = "", string _LaborAccountClientLookupId = "", string _AssetNumber = "", string _searchText = "", int _AssetGroup1Id = 0, int _AssetGroup2Id = 0, int _AssetGroup3Id = 0, string AssetAvailability = "")
        {
            EquipmentPrintModel objEquipmentPrintModel;
            List<EquipmentPrintModel> EquipmentPrintModelList = new List<EquipmentPrintModel>();
            _searchText = _searchText.Replace("%", "[%]");
            _ClientLookupId = _ClientLookupId.Replace("%", "[%]");
            _Name = _Name.Replace("%", "[%]");
            _Location = _Location.Replace("%", "[%]");
            _SerialNumber = _SerialNumber.Replace("%", "[%]");
            _Make = _Make.Replace("%", "[%]");
            _Model = _Model.Replace("%", "[%]");
            _AssetNumber = _AssetNumber.Replace("%", "[%]");
            _SerialNumber = _SerialNumber.Replace("%", "[%]");
            AssetAvailability = AssetAvailability.Replace("%", "[%]");
            start = start.HasValue
               ? start / length
               : 0;
            int skip = start * length ?? 0;
            int lengthForPrint = 100000;

            EquipmentWrapper eWrapper = new EquipmentWrapper(userData);
            var equipmentList = eWrapper.GetEquipmentGridData(customQueryDisplayId, _colname, _coldir, 0, lengthForPrint, _ClientLookupId, _Name, _Location, _SerialNumber, _Type, _Make, _Model, _LaborAccountClientLookupId, _AssetNumber, _searchText, _AssetGroup1Id, _AssetGroup2Id, _AssetGroup3Id, AssetAvailability);

            foreach (var item in equipmentList)
            {
                objEquipmentPrintModel = new EquipmentPrintModel();
                objEquipmentPrintModel.ClientLookupId = item.ClientLookupId;
                objEquipmentPrintModel.Name = item.Name;
                objEquipmentPrintModel.Location = (userData.DatabaseKey.Client.BusinessType.ToUpper() == BusinessTypeConstants.Facilities) ? item.LocationIdClientLookupId : item.Location;
                objEquipmentPrintModel.SerialNumber = item.SerialNumber;
                objEquipmentPrintModel.Type = item.Type;
                objEquipmentPrintModel.Make = item.Make;
                objEquipmentPrintModel.Model = item.Model;
                objEquipmentPrintModel.LaborAccountClientLookupId = item.LaborAccountClientLookupId;
                objEquipmentPrintModel.AssetNumber = item.AssetNumber;
                objEquipmentPrintModel.AsseteGroup1ClientLookupId = item.AssetGroup1ClientLookupId;
                objEquipmentPrintModel.AsseteGroup2ClientLookupId = item.AssetGroup2ClientLookupId;
                objEquipmentPrintModel.AsseteGroup3ClientLookupId = item.AssetGroup3ClientLookupId;

                // V2-636
                if (item.RemoveFromService == false)
                {
                    objEquipmentPrintModel.RemoveFromService = AssetAvailabilityStatusConstant.InService;
                }
                else
                {
                    objEquipmentPrintModel.RemoveFromService = AssetAvailabilityStatusConstant.OutService;
                }
                if (item.RemoveFromServiceDate != null && item.RemoveFromServiceDate != default(DateTime))
                {
                    objEquipmentPrintModel.RemoveFromServiceDate = item.RemoveFromServiceDate.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                }
                else
                {
                    objEquipmentPrintModel.RemoveFromServiceDate = "";
                }
                EquipmentPrintModelList.Add(objEquipmentPrintModel);
            }
            return JsonConvert.SerializeObject(new { data = EquipmentPrintModelList }, JsonSerializerDateSettings);
        }

        public JsonResult GetEquipmentTypeList()
        {
            List<SelectListItem> List = new List<SelectListItem>();
            List<DataContracts.LookupList> TypeList = new List<DataContracts.LookupList>();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            var AllLookUps = commonWrapper.GetAllLookUpList();
            if (AllLookUps != null)
            {
                TypeList = AllLookUps.Where(x => x.ListName == LookupListConstants.Equipment_EquipType).ToList();
            }
            if (TypeList != null)
            {
                List = TypeList.Select(x => new SelectListItem { Text = x.ListValue + " - " + x.Description, Value = x.ListValue }).ToList();
            }
            return Json(List, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult UpdateEquipmentsType(string[] EqpId, string Type)
        {
            EquipmentWrapper eWrapper = new EquipmentWrapper(userData);
            List<long> equipmentIds = new List<long>();
            foreach (var e in EqpId)
            {
                equipmentIds.Add(Convert.ToInt64(e));
            }
            var retValue = eWrapper.updateBasedOnCriteria(equipmentIds, Type);

            return Json(new { Result = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
        }
        public PartialViewResult EquipmentDetails(long EquipmentId)
        {
            EquipmentWrapper eWrapper = new EquipmentWrapper(userData);
            #region V2-1115
            bool EPMInvoiceImportInUse = false;
            var InterfacePropData = (List<InterfacePropModel>)Session["InterfacePropData"];
            if (InterfacePropData != null && InterfacePropData.Count > 0)
            {
                EPMInvoiceImportInUse = InterfacePropData.Where(x => x.InterfaceType == InterfacePropConstants.EPMInvoiceImport).Select(x => x.InUse).FirstOrDefault();
            }
            #endregion
            var EquipmentDetails = eWrapper.GetEquipmentDetailsById(EquipmentId);
            if (EquipmentDetails.AssetCategory == AssetCategoryConstant.RepairableSpare)
            {
                var objComb = RepairableSpareEquipmentDetails(EquipmentId, EquipmentDetails);
                objComb.EPMInvoiceImportInUse = EPMInvoiceImportInUse; //V2-1115
                return PartialView("~/Views/Equipment/RepairableSpare/RepairableSpareEquipmentDetails.cshtml", objComb);
            }
            else
            {
                Task[] tasks = new Task[3];

                EquipmentCombined objComb = new EquipmentCombined();
                ChangeEquipmentIDModel _ChangeEquipmentIDModel = new ChangeEquipmentIDModel();
                EquipmentUDF EquipmentUDF = new EquipmentUDF();
                EquipmentModel equipmentModel = new EquipmentModel();
                AssetAvailabilityRemoveModel assetAvailabilityRemoveModel = new AssetAvailabilityRemoveModel();
                AssetAvailabilityUpdateModel assetAvailabilityUpdateModel = new AssetAvailabilityUpdateModel();
                CommonWrapper objCommonWrapper = new CommonWrapper(userData);
                tasks[0] = Task.Factory.StartNew(() => objComb.attachmentCount = objCommonWrapper.AttachmentCount(EquipmentId, AttachmentTableConstant.Equipment, userData.Security.Equipment.Edit));
                tasks[1] = Task.Factory.StartNew(() => EquipmentUDF = eWrapper.RetrieveEquipmentUDFByEquipmentId(EquipmentId));
                tasks[2] = Task.Factory.StartNew(() => objComb.UIConfigurationDetails = new RetrieveDataForUIConfiguration().Retrieve(DataDictionaryViewNameConstant.ViewAssetWidget, userData));
                Task.WaitAll(tasks);
                objComb.EquipData = EquipmentDetails;
                if (objComb.EquipData.Maint_WarrantyExpire != null && objComb.EquipData.Maint_WarrantyExpire.Value == default(DateTime))
                {
                    objComb.EquipData.Maint_WarrantyExpire = null;
                }
                objComb._EquipmentSummaryModel = GetEquipmentSummary(EquipmentId, objComb.EquipData.ClientLookupId, objComb.EquipData.Name, objComb.EquipData.RemoveFromService, objComb.EquipData.Status);

                var equipmentDownTimeList = UtilityFunction.EquipmentDownTimeDatesList();
                if (equipmentDownTimeList != null)
                {
                    objComb.equipmentDownTimeDateList = equipmentDownTimeList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
                }
                var workOrderTypeList = UtilityFunction.WorkOrderDatesList();
                if (workOrderTypeList != null)
                {
                    objComb.workOrderTypeDateList = workOrderTypeList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
                }
                string temp = string.Empty;
                temp = EquipmentDetails.ClientLookupId + "][" + EquipmentDetails.Name + "][" + EquipmentDetails.SerialNumber + "][" + EquipmentDetails.Make + "][" + EquipmentDetails.Model;
                QRCodeModel qRCodeModel = new QRCodeModel();
                List<string> equipmentClientLookUpNames = new List<string>();
                equipmentClientLookUpNames.Add(temp);
                qRCodeModel.EquipmentIdsList = equipmentClientLookUpNames;
                objComb.qRCodeModel = qRCodeModel;
                equipmentModel.AlertFollowedEquipment = eWrapper.AlertFollowIdExist(EquipmentId);
                equipmentModel.BusinessType = userData.DatabaseKey.Client.BusinessType;
                _ChangeEquipmentIDModel.EquipmentId = Convert.ToInt64(objComb.EquipData.EquipmentId);
                _ChangeEquipmentIDModel.ClientLookupId = objComb.EquipData.ClientLookupId;
                _ChangeEquipmentIDModel.OldClientLookupId = objComb.EquipData.ClientLookupId;
                _ChangeEquipmentIDModel.UpdateIndex = objComb.EquipData.UpdateIndex;
                objComb.EquipModel = equipmentModel;
                objComb._ChangeEquipmentIDModel = _ChangeEquipmentIDModel;
                objComb._CreatedLastUpdatedModel = eWrapper.createdLastUpdatedModel(EquipmentId);
                objComb.security = this.userData.Security;
                objComb._userdata = this.userData;
                objComb.ViewEquipment = new ViewEquipmentModelDynamic();
                objComb.ViewEquipment = eWrapper.MapEquipmentDataForView(objComb.ViewEquipment, EquipmentDetails);
                objComb.ViewEquipment = eWrapper.MapEquipmentUDFDataForView(objComb.ViewEquipment, EquipmentUDF);

                #region Asset availability
                var removefromServiceReasonCode = objCommonWrapper.GetListFromConstVals(LookupListConstants.RemoveFromServiceReasonCode);
                if (removefromServiceReasonCode != null)
                {
                    objComb.LookupRemoveFromServiceReasonCode = removefromServiceReasonCode.Select(x => new SelectListItem { Text = x.text.ToString(), Value = x.value.ToString() });
                    if (objComb.EquipData.RemoveFromServiceReasonCode != "")
                    {
                        objComb.ServiceReasonCode = AssetStatusConstant.Scrapped;
                        if (objComb.EquipData.RemoveFromServiceReasonCode != AssetStatusConstant.Scrapped)
                        {
                            objComb.ServiceReasonCode = objComb.LookupRemoveFromServiceReasonCode.Where(x => x.Value == objComb.EquipData.RemoveFromServiceReasonCode).Select(x => x.Text).FirstOrDefault().ToString();
                        }
                    }
                }

                assetAvailabilityRemoveModel.EquipmentId = EquipmentId;
                assetAvailabilityRemoveModel.RemoveFromService = objComb.EquipData.RemoveFromService;
                objComb._AssetAvailabilityRemoveModel = assetAvailabilityRemoveModel;
                assetAvailabilityUpdateModel.EquipmentId = EquipmentId;
                objComb._AssetAvailabilityUpdateModel = assetAvailabilityUpdateModel;
                objComb.IsAssetAvailability = userData.Security.Asset_Availability.Access;
                objComb.RemoveServiceDate = Convert.ToDateTime(objComb.EquipData.RemoveFromServiceDate).ToUserTimeZone(userData.Site.TimeZone);
                #endregion
                PartModel objPartModel = new PartModel();
                var AllLookUps = objCommonWrapper.GetAllLookUpList();
                if (AllLookUps != null)
                {
                    var LookupStokeType = AllLookUps.Where(x => x.ListName == LookupListConstants.STOCK_TYPE).ToList();
                    if (LookupStokeType != null)
                    {
                        objPartModel.LookupStokeTypeList = LookupStokeType.Select(x => new SelectListItem { Text = x.ListValue + "  -  " + x.Description, Value = x.ListValue });
                    }

                }
                objComb.PartModel = objPartModel;
                this.GetAssetGroupHeaderName(objComb);
                objComb.EPMInvoiceImportInUse = EPMInvoiceImportInUse;
                LocalizeControls(objComb, LocalizeResourceSetConstants.EquipmentDetails);
                return PartialView("EquipmentDetails", objComb);
            }
        }

        public RedirectResult DetailFromWorkOrder(long EquipmentId) //This method is used to redirect from both WorkOrder & PreventiveMaintainance & Device BreadCrumb link on Details pages
        {
            if (userData.IsLoggedInFromMobile)
            {
                TempData["Mode"] = "DetailFromWorkOrder_Mobile";
            }
            else
            {
                TempData["Mode"] = "DetailFromWorkOrder";
            }
            string EquipmentIdString = Convert.ToString(EquipmentId);
            TempData["EquipmentId"] = EquipmentIdString;
            return Redirect("/Equipment/Index?page=Maintenance_Assets");
        }
        public RedirectResult RedirectFromWorkOrderPrint(long EquipmentId)
        {
            TempData["Mode"] = "RedirectFromWorkOrderPrint";
            string EquipmentIdString = Convert.ToString(EquipmentId);
            TempData["EquipmentId"] = EquipmentIdString;
            return Redirect("/Equipment/Index?page=Maintenance_Assets");
        }

        #region V2-1147
        public ActionResult DetailFromNotification(long EquipmentId, string alertName)
        {
            TempData["Mode"] = "DetailFromNotification";
            TempData["AlertName"] = alertName;
            string EquipmentIdString = Convert.ToString(EquipmentId);
            TempData["EquipmentId"] = EquipmentIdString;
            return Redirect("/Equipment/Index?page=Maintenance_Assets");
        }
        #endregion
        #endregion

        #region Photos
        public JsonResult DeleteImageFromAzure(string _EquimentId, string TableName, bool Profile, bool Image)
        {
            CommonWrapper comWrapper = new CommonWrapper(userData);
            string isSuccess = string.Empty;
            comWrapper.DeleteAzureImage(Convert.ToInt64(_EquimentId), AttachmentTableConstant.Equipment, ref isSuccess);
            return Json(isSuccess.ToLower(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult DeleteImageFromOnPremise(string _EquimentId, string TableName, bool Profile, bool Image)
        {
            CommonWrapper comWrapper = new CommonWrapper(userData);
            string isSuccess = string.Empty;
            comWrapper.DeleteOnPremiseImage(Convert.ToInt64(_EquimentId), AttachmentTableConstant.Equipment, ref isSuccess);
            return Json(isSuccess.ToLower(), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Equipment Add-Edit
        public ActionResult Add()
        {

            if (this.userData.IsLoggedInFromMobile)
            {
                TempData["Mode"] = "adddynamicmobile";
            }
            else
            {
                TempData["Mode"] = "adddynamic";
            }

            return Redirect("/Equipment/Index?page=Maintenance_Assets");
        }
        public PartialViewResult AddEquipment()
        {
            Task taskAllLookUp;
            Task taskAssetGroup1LookUp;
            Task taskAssetGroup2LookUp;
            Task taskAssetGroup3LookUp;
            Task taskAssetAccountLookUp;
            EquipmentWrapper eWrapper = new EquipmentWrapper(userData);
            EquipmentCombined objComb = new EquipmentCombined();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            List<DataContracts.LookupList> TypeList = new List<DataContracts.LookupList>();
            List<DataContracts.LookupList> AllLookUps = new List<DataContracts.LookupList>();
            objComb.EquipModel = new EquipmentModel();
            objComb.EquipModel.BusinessType = userData.DatabaseKey.Client.BusinessType;
            objComb.EquipModel.PlantLocation = userData.Site.PlantLocation;
            //V2-379

            #region task
            List<DataModel> AcclookUpList = new List<DataModel>();
            taskAssetAccountLookUp = Task.Factory.StartNew(() => AcclookUpList = GetAccountByActiveState(true));
            taskAllLookUp = Task.Factory.StartNew(() => AllLookUps = commonWrapper.GetAllLookUpList());
            List<AssetGroup1Model> astGroup1 = new List<AssetGroup1Model>();
            taskAssetGroup1LookUp = Task.Factory.StartNew(() => astGroup1 = eWrapper.GetAssetGroup1Dropdowndata());
            List<AssetGroup2Model> astGroup2 = new List<AssetGroup2Model>();
            taskAssetGroup2LookUp = Task.Factory.StartNew(() => astGroup2 = eWrapper.GetAssetGroup2Dropdowndata());
            List<AssetGroup3Model> astGroup3 = new List<AssetGroup3Model>();
            taskAssetGroup3LookUp = Task.Factory.StartNew(() => astGroup3 = eWrapper.GetAssetGroup3Dropdowndata());

            Task.WaitAll(taskAssetAccountLookUp, taskAllLookUp, taskAssetGroup1LookUp, taskAssetGroup2LookUp, taskAssetGroup3LookUp);
            #endregion

            List<DropDownModel> AssetCategoryList = new List<DropDownModel>();
            AssetCategoryList = UtilityFunction.AssetCategoryList();

            if (AssetCategoryList != null)
            {
                objComb.EquipModel.AssetCategoryList = AssetCategoryList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
            }
            if (AcclookUpList != null)
            {
                objComb.EquipModel.AccountList = AcclookUpList.Select(x => new SelectListItem { Text = x.Account + " - " + x.Name, Value = x.Account });
            }
            if (astGroup1 != null)
            {
                objComb.EquipModel.AssetGroup1List = astGroup1.Select(x => new SelectListItem { Text = x.AssetGroup1DescWithClientLookupId, Value = x.AssetGroup1Id.ToString() });
            }
            if (astGroup2 != null)
            {
                objComb.EquipModel.AssetGroup2List = astGroup2.Select(x => new SelectListItem { Text = x.AssetGroup2DescWithClientLookupId, Value = x.AssetGroup2Id.ToString() });
            }
            if (astGroup3 != null)
            {
                objComb.EquipModel.AssetGroup3List = astGroup3.Select(x => new SelectListItem { Text = x.AssetGroup3DescWithClientLookupId, Value = x.AssetGroup3Id.ToString() });
            }
            if (AllLookUps != null)
            {
                TypeList = AllLookUps.Where(x => x.ListName == LookupListConstants.Equipment_EquipType).ToList();
                if (TypeList != null)
                {
                    objComb.LookupTypeList = TypeList.Select(x => new SelectListItem { Text = x.ListValue + " - " + x.Description, Value = x.ListValue }).ToList();
                }
            }
            this.GetAssetGroupHeaderName(objComb);
            LocalizeControls(objComb, LocalizeResourceSetConstants.EquipmentDetails);
            return PartialView("~/Views/Equipment/EquipmentAdd.cshtml", objComb);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddEquipment(EquipmentCombined objEM, string Command)
        {
            EquipmentCombined objComb = new EquipmentCombined();
            List<string> ErrorList = new List<string>();
            EquipmentWrapper eWrapper = new EquipmentWrapper(userData);
            string ModelValidationFailedMessage = string.Empty;

            if (ModelState.IsValid)
            {
                Equipment equipment = new Equipment();
                string EQ_ClientLookupId = objEM.EquipModel.EquipmentID.ToUpper().Trim();
                equipment = eWrapper.AddEquipment(EQ_ClientLookupId, objEM, objComb);
                if (equipment.ErrorMessages != null && equipment.ErrorMessages.Count > 0)
                {
                    return Json(equipment.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), Command = Command, EquipmentId = equipment.EquipmentId }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult EditEquipment(long EquipmentId, string ClientLookupId, string Name, bool isRemoveFromService, string Status)
        {
            Task taskAllLookUp;
            Task taskAssetGroup1LookUp;
            Task taskAssetGroup2LookUp;
            Task taskAssetGroup3LookUp;
            Task taskAssetAccountLookUp;
            Task taskAssetDetailsById;
            EquipmentWrapper eWrapper = new EquipmentWrapper(userData);
            EquipmentCombined objComb = new EquipmentCombined();
            objComb.EquipModel = new EquipmentModel();
            objComb.EquipModel.PlantLocation = userData.Site.PlantLocation;

            CommonWrapper commonWrapper = new CommonWrapper(userData);
            List<DataContracts.LookupList> TypeList = new List<DataContracts.LookupList>();
            List<DataContracts.LookupList> AllLookUps = new List<DataContracts.LookupList>();

            #region task
            taskAssetDetailsById = Task.Factory.StartNew(() => objComb.EquipModel = eWrapper.GetEditEquipmentDetailsById(EquipmentId));

            List<DataModel> AcclookUpList = new List<DataModel>();
            taskAssetAccountLookUp = Task.Factory.StartNew(() => AcclookUpList = GetAccountByActiveState(true));

            taskAllLookUp = Task.Factory.StartNew(() => AllLookUps = commonWrapper.GetAllLookUpList());
            List<AssetGroup1Model> astGroup1 = new List<AssetGroup1Model>();

            taskAssetGroup1LookUp = Task.Factory.StartNew(() => astGroup1 = eWrapper.GetAssetGroup1Dropdowndata());

            List<AssetGroup2Model> astGroup2 = new List<AssetGroup2Model>();
            taskAssetGroup2LookUp = Task.Factory.StartNew(() => astGroup2 = eWrapper.GetAssetGroup2Dropdowndata());

            List<AssetGroup3Model> astGroup3 = new List<AssetGroup3Model>();
            taskAssetGroup3LookUp = Task.Factory.StartNew(() => astGroup3 = eWrapper.GetAssetGroup3Dropdowndata());

            Task.WaitAll(taskAssetDetailsById, taskAssetAccountLookUp, taskAllLookUp, taskAssetGroup1LookUp, taskAssetGroup2LookUp, taskAssetGroup3LookUp);
            #endregion

            objComb.EquipModel.BusinessType = userData.DatabaseKey.Client.BusinessType;
            var AssetCategoryList = UtilityFunction.AssetCategoryList();
            if (AssetCategoryList != null)
            {
                objComb.EquipModel.AssetCategoryList = AssetCategoryList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
            }
            if (objComb.EquipData.Maint_WarrantyExpire != null && objComb.EquipData.Maint_WarrantyExpire.Value == default(DateTime))
            {
                objComb.EquipData.Maint_WarrantyExpire = null;
            }

            if (AcclookUpList != null)
            {
                objComb.AccountList = AcclookUpList.Select(x => new SelectListItem { Text = x.Account + " - " + x.Name, Value = x.Account });
            }
            if (astGroup1 != null)
            {
                objComb.EquipModel.AssetGroup1List = astGroup1.Select(x => new SelectListItem { Text = x.AssetGroup1DescWithClientLookupId, Value = x.AssetGroup1Id.ToString() });
            }
            if (astGroup2 != null)
            {
                objComb.EquipModel.AssetGroup2List = astGroup2.Select(x => new SelectListItem { Text = x.AssetGroup2DescWithClientLookupId, Value = x.AssetGroup2Id.ToString() });
            }
            if (astGroup3 != null)
            {
                objComb.EquipModel.AssetGroup3List = astGroup3.Select(x => new SelectListItem { Text = x.AssetGroup3DescWithClientLookupId, Value = x.AssetGroup3Id.ToString() });
            }
            if (AllLookUps != null)
            {
                TypeList = AllLookUps.Where(x => x.ListName == LookupListConstants.Equipment_EquipType).ToList();
                if (TypeList != null)
                {
                    objComb.LookupTypeList = TypeList.Select(x => new SelectListItem { Text = x.ListValue + " - " + x.Description, Value = x.ListValue }).ToList();
                }
            }

            objComb._userdata = userData;
            objComb._EquipmentSummaryModel = GetEquipmentSummary(EquipmentId, ClientLookupId, Name, isRemoveFromService, Status);
            this.GetAssetGroupHeaderName(objComb);
            LocalizeControls(objComb, LocalizeResourceSetConstants.EquipmentDetails);
            return PartialView("EquipmentEdit", objComb);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateEquipmentInfo(EquipmentCombined equip)
        {
            string emptyValue = string.Empty;
            EquipmentWrapper eWrapper = new EquipmentWrapper(userData);
            Equipment equipment = new Equipment();

            if (ModelState.IsValid)
            {
                equipment = eWrapper.UpdateEquipment(equip);
                if (equipment.ErrorMessages != null && equipment.ErrorMessages.Count > 0)
                {
                    return Json(equipment.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), equipmentid = equipment.EquipmentId }, JsonRequestBehavior.AllowGet);
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
        public JsonResult DeleteEquipment(long _eqid)
        {
            string result = string.Empty;
            List<String> errorList = new List<string>();
            string deleteResult = string.Empty;
            EquipmentWrapper eWrapper = new EquipmentWrapper(userData);

            errorList = eWrapper.DeleteEquipment(_eqid, ref deleteResult);
            if (errorList != null && errorList.Count > 0)
            {
                return Json(new { Result = deleteResult, errorList = errorList }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Result = deleteResult }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult ChangeEquipmentId(EquipmentCombined objComb)
        {
            if (ModelState.IsValid)
            {
                EquipmentWrapper eWrapper = new EquipmentWrapper(userData);
                string result = string.Empty;
                List<String> errorList = new List<string>();
                errorList = eWrapper.ChangeEquipmentId(objComb._ChangeEquipmentIDModel);
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
                string ModelValidationFailedMessage = string.Empty;
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult ValidateEqpStatusChange(long _eqid, bool inactiveFlag, string clientLookupId)
        {
            EquipmentWrapper eWrapper = new EquipmentWrapper(userData);
            Equipment equipment = new Equipment();
            string flag = ActivationStatusConstant.InActivate;
            if (inactiveFlag)
            {
                flag = ActivationStatusConstant.Activate;
            }
            equipment = eWrapper.ValidateEquStatusChange(_eqid, flag, clientLookupId);
            if (equipment.ErrorMessages != null && equipment.ErrorMessages.Count > 0)
            {
                return Json(equipment.ErrorMessages, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { validationStatus = true, equipmentid = equipment.EquipmentId }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult UpdateEquStatus(long _eqid, bool inactiveFlag)
        {
            EquipmentWrapper eWrapper = new EquipmentWrapper(userData);
            string Event = string.Empty;
            string ReasonCode = string.Empty;//V2-1133
            var errMsg = eWrapper.UpdateEquActiveStatus(_eqid, inactiveFlag);
            if (errMsg != null && errMsg.Count > 0)
            {
                return Json(errMsg, JsonRequestBehavior.AllowGet);
            }
            else
            {
                if (inactiveFlag)
                {
                    Event = "Activate";

                }
                else
                {
                    Event = "Inactivate";
                }
                errMsg = eWrapper.CreateAssetEvent(_eqid, Event, "");
                if (errMsg != null && errMsg.Count > 0)
                {
                    return Json(errMsg, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    #region V2-1133
                    if (inactiveFlag)
                    {
                        Event = EventStatusConstants.Return;
                        ReasonCode = "";
                    }
                    else
                    {
                        Event = EventStatusConstants.Remove;
                        ReasonCode = RemoveFromServiceReasonCodeConstant.NotinUse;
                    }
                    errMsg = eWrapper.CreateAssetAvailability(_eqid, Event, ReasonCode);
                    #endregion
                    if (errMsg != null && errMsg.Count > 0)
                    {
                        return Json(errMsg, JsonRequestBehavior.AllowGet);
                    }
                    else { return Json(new { result = JsonReturnEnum.success.ToString(), equipmentid = _eqid }, JsonRequestBehavior.AllowGet); }

                }
            }
        }
        #endregion

        #region Childrens
        [HttpPost]
        public string GetAllChildrens(int? draw, int? start, int? length, long EquipmentId)
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            EquipmentWrapper eWrapper = new EquipmentWrapper(userData);
            var Children = eWrapper.GetChildren(EquipmentId);
            if (Children != null)
            {
                Children = this.GetAllChildrenSortByColumnWithOrder(order, orderDir, Children);
            }
            var totalRecords = 0;
            var recordsFiltered = 0;
            start = start.HasValue
                ? start / length
                : 0;
            recordsFiltered = Children.Count();
            totalRecords = Children.Count();
            int initialPage = start.Value;
            var filteredResult = Children
                .Skip(initialPage * length ?? 0)
                .Take(length ?? 0)
                .ToList();
            var SecurityEditVal = userData.Security.Equipment.Edit;
            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult, SecurityEditVal = SecurityEditVal }, JsonSerializer12HoursDateAndTimeSettings);
        }
        private List<ChildrenGridDataModel> GetAllChildrenSortByColumnWithOrder(string order, string orderDir, List<ChildrenGridDataModel> data)
        {
            List<ChildrenGridDataModel> lst = new List<ChildrenGridDataModel>();
            if (data != null)
            {
                switch (order)
                {
                    case "0":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ClientLookupId).ToList() : data.OrderBy(p => p.ClientLookupId).ToList();
                        break;
                    case "1":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Name).ToList() : data.OrderBy(p => p.Name).ToList();
                        break;
                    case "2":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.SerialNumber).ToList() : data.OrderBy(p => p.SerialNumber).ToList();
                        break;
                    case "3":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Type).ToList() : data.OrderBy(p => p.Type).ToList();
                        break;
                    case "4":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Make).ToList() : data.OrderBy(p => p.Make).ToList();
                        break;
                    case "5":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Model).ToList() : data.OrderBy(p => p.Model).ToList();
                        break;
                    default:
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ClientLookupId).ToList() : data.OrderBy(p => p.ClientLookupId).ToList();
                        break;
                }
            }
            return lst;
        }
        #endregion

        #region Sensor
        [HttpPost]
        public string GetEquipment_Sensor(int? draw, int? start, int? length, long EquipmentId)
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            EquipmentWrapper eWrapper = new EquipmentWrapper(userData);
            var Sensors = eWrapper.GetEquipment_Sensor(EquipmentId);
            Sensors = this.GetAllSensorsSortByColumnWithOrder(order, orderDir, Sensors);
            var totalRecords = 0;
            var recordsFiltered = 0;
            start = start.HasValue
                ? start / length
                : 0;
            recordsFiltered = Sensors.Count();
            totalRecords = Sensors.Count();
            int initialPage = start.Value;
            var filteredResult = Sensors
                .Skip(initialPage * length ?? 0)
                .Take(length ?? 0)
                .ToList();
            var sensorCreate = this.userData.Security.Sensors.Create;
            var sensorEdit = this.userData.Security.Sensors.Create && this.userData.Security.Sensors.Edit;
            var sensorDelete = this.userData.Security.Sensors.Delete && this.userData.Security.Sensors.Edit;
            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult, sensorCreate = sensorCreate, sensorEdit = sensorEdit, sensorDelete = sensorDelete }, JsonSerializer12HoursDateAndTimeSettings);
        }
        public ActionResult ShowEditSensor(long EquipmentId, long SensorId, long Equipment_Sensor_XrefId)
        {
            EquipmentCombined objComb = new EquipmentCombined();
            EquipmentWrapper eWrapper = new EquipmentWrapper(userData);
            SensorGridDataModel objSensorGridDataModel = new SensorGridDataModel();
            var obj_AlertProcList = eWrapper.GetAlertProcList();

            if (obj_AlertProcList != null)
            {
                objSensorGridDataModel.SensorPrecedureList = obj_AlertProcList.Select(x => new SelectListItem { Text = x.ClientLookUpId + " - " + x.Description, Value = x.SensorAlertProcedureId.ToString() });
            }
            var PersonnelLookUplist = GetList_Personnel();
            if (PersonnelLookUplist != null)
            {
                objSensorGridDataModel.AssignedToPersonnelList = PersonnelLookUplist.Select(x => new SelectListItem { Text = x.AssignedTo_PersonnelClientLookupId + " - " + x.NameFirst + " - " + x.NameLast, Value = x.AssignedTo_PersonnelId.ToString() });
            }
            var equipment = eWrapper.GetEditEquipmentDetailsById(EquipmentId);
            Equipment_Sensor_Xref sensor_Xref = new Equipment_Sensor_Xref()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                Equipment_Sensor_XrefId = Equipment_Sensor_XrefId
            };

            var sensorval = eWrapper.ShowEditSensor(EquipmentId, SensorId, Equipment_Sensor_XrefId);

            objSensorGridDataModel.EquipmentId = EquipmentId;
            objSensorGridDataModel.SensorId = SensorId;
            objSensorGridDataModel.Equipment_Sensor_XrefId = Equipment_Sensor_XrefId;
            objSensorGridDataModel.SensorName = sensorval.SensorName;
            objSensorGridDataModel.TriggerLow = sensorval.TriggerLow;
            objSensorGridDataModel.TriggerHigh = sensorval.TriggerHigh;
            objSensorGridDataModel.SensorAlertProcedureId = sensorval.SensorAlertProcedureId;
            objSensorGridDataModel.AssignedTo_PersonnelId = sensorval.AssignedTo_PersonnelId;
            objSensorGridDataModel.EquipmentClientLookupId = equipment.ClientLookupId;
            EquipmentSummaryModel objEquipmentSummaryModel = new EquipmentSummaryModel();
            objEquipmentSummaryModel.Equipment_ClientLookupId = equipment.ClientLookupId;
            objComb.sensorGridDataModel = objSensorGridDataModel;
            objComb._EquipmentSummaryModel = objEquipmentSummaryModel;
            objComb._userdata = userData;
            LocalizeControls(objComb, LocalizeResourceSetConstants.EquipmentDetails);
            return PartialView("_EditSensor", objComb);
        }
        [HttpPost]
        public ActionResult EditSensor(EquipmentCombined objComb)
        {
            string ModelValidationFailedMessage = string.Empty;
            List<string> errorList = new List<string>();
            if (ModelState.IsValid)
            {
                EquipmentWrapper eWrapper = new EquipmentWrapper(userData);
                errorList = eWrapper.UpdateSensor(objComb);

                if (errorList != null && errorList.Count > 0)
                {
                    return Json(errorList, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), equipmentid = objComb.sensorGridDataModel.EquipmentId }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult DeleteSensor(long _xref)
        {
            EquipmentWrapper EWrapper = new EquipmentWrapper(userData);
            if (EWrapper.SensorDelete(_xref))
            {
                return Json(new { Result = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Result = JsonReturnEnum.failed.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }
        private List<SensorGridDataModel> GetAllSensorsSortByColumnWithOrder(string order, string orderDir, List<SensorGridDataModel> data)
        {
            List<SensorGridDataModel> lst = new List<SensorGridDataModel>();
            switch (order)
            {
                case "0":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.SensorName).ToList() : data.OrderBy(p => p.SensorName).ToList();
                    break;
                case "1":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Sensor).ToList() : data.OrderBy(p => p.Sensor).ToList();
                    break;
                case "2":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.AssignedTo_Name).ToList() : data.OrderBy(p => p.AssignedTo_Name).ToList();
                    break;
                case "3":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.SensorAlertProcedureClientLookUpId).ToList() : data.OrderBy(p => p.SensorAlertProcedureClientLookUpId).ToList();
                    break;
                case "4":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.LastReading).ToList() : data.OrderBy(p => p.LastReading).ToList();
                    break;
                case "5":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.TriggerLow).ToList() : data.OrderBy(p => p.TriggerLow).ToList();
                    break;
                case "6":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.TriggerHigh).ToList() : data.OrderBy(p => p.TriggerHigh).ToList();
                    break;
                case "7":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Equipment_Sensor_XrefId).ToList() : data.OrderBy(p => p.Equipment_Sensor_XrefId).ToList();
                    break;
                default:
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.SensorName).ToList() : data.OrderBy(p => p.SensorName).ToList();
                    break;
            }
            return lst;
        }
        #endregion

        #region Parts
        [HttpPost]
        public string GetEquipment_Parts(int? draw, int? start, int? length, long EquipmentId, string PartClientLookUpId, string Description, string StockType)
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            EquipmentWrapper eWrapper = new EquipmentWrapper(userData);
            var Parts = eWrapper.GetEquipmentParts(EquipmentId, PartClientLookUpId, Description, StockType);
            Parts = this.GetAllPartsSortByColumnWithOrder(order, orderDir, Parts);
            var totalRecords = 0;
            var recordsFiltered = 0;
            start = start.HasValue
                ? start / length
                : 0;
            recordsFiltered = Parts.Count();
            totalRecords = Parts.Count();
            int initialPage = start.Value;
            var filteredResult = Parts
                .Skip(initialPage * length ?? 0)
                .Take(length ?? 0)
                .ToList();
            var partSecurity = this.userData.Security.Parts.Part_Equipment_XRef; //V2-1214
            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult, partSecurity = partSecurity }, JsonSerializer12HoursDateAndTimeSettings);
        }
        private List<PartsModel> GetAllPartsSortByColumnWithOrder(string order, string orderDir, List<PartsModel> data)
        {
            List<PartsModel> lst = new List<PartsModel>();

            switch (order)
            {
                case "0":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Comment).ToList() : data.OrderBy(p => p.Comment).ToList();
                    break;
                case "1":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.EquipmentId).ToList() : data.OrderBy(p => p.EquipmentId).ToList();
                    break;
                case "2":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Part_ClientLookupId).ToList() : data.OrderBy(p => p.Part_ClientLookupId).ToList();
                    break;
                case "3":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Part_Description).ToList() : data.OrderBy(p => p.Part_Description).ToList();
                    break;
                case "4":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.QuantityNeeded).ToList() : data.OrderBy(p => p.QuantityNeeded).ToList();
                    break;
                case "5":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.QuantityUsed).ToList() : data.OrderBy(p => p.QuantityUsed).ToList();
                    break;
                case "6":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Equipment_Parts_XrefId).ToList() : data.OrderBy(p => p.Equipment_Parts_XrefId).ToList();
                    break;
                case "7":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.UpdatedIndex).ToList() : data.OrderBy(p => p.UpdatedIndex).ToList();
                    break;
                default:
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Comment).ToList() : data.OrderBy(p => p.Comment).ToList();
                    break;
            }

            return lst;
        }
        public ActionResult PartsAdd(long EquipmentId, string ClientLookupId, string Name, bool isRemoveFromService, string Status)
        {
            EquipmentCombined objComb = new EquipmentCombined();
            PartsSessionData objPartsSessionData = new PartsSessionData();
            EquipmentWrapper EWrapper = new EquipmentWrapper(userData);
            objComb._EquipmentSummaryModel = GetEquipmentSummary(EquipmentId, ClientLookupId, Name, isRemoveFromService, Status);
            objPartsSessionData.EquipmentId = EquipmentId;
            objComb.partsSessionData = objPartsSessionData;
            objComb._userdata = this.userData;
            LocalizeControls(objComb, LocalizeResourceSetConstants.EquipmentDetails);
            return PartialView("AddParts", objComb);
        }
        public ActionResult PartsEdit(long EquipmentId, long Equipment_Parts_XrefId, string ClientLookupId, string Name, bool isRemoveFromService, string Status)
        {
            EquipmentCombined objComb = new EquipmentCombined();
            EquipmentWrapper EWrapper = new EquipmentWrapper(userData);
            PartsSessionData objPartsSessionData = new PartsSessionData();
            objComb._EquipmentSummaryModel = GetEquipmentSummary(EquipmentId, ClientLookupId, Name, isRemoveFromService, Status);
            objComb = EWrapper.EditParts(EquipmentId, Equipment_Parts_XrefId, objComb);
            objComb._userdata = this.userData;
            LocalizeControls(objComb, LocalizeResourceSetConstants.EquipmentDetails);
            return PartialView("EditParts", objComb);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult PartsEdit(EquipmentCombined ec)
        {
            EquipmentWrapper EWrapper = new EquipmentWrapper(userData);
            string ModelValidationFailedMessage = string.Empty;
            if (ModelState.IsValid)
            {
                ec = EWrapper.UpdatePart(ec);
                if (ec.partsSessionData.ErrorMessage != null && ec.partsSessionData.ErrorMessage.Count > 0)
                {
                    return Json(ec.partsSessionData.ErrorMessage, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), equipmentid = ec.partsSessionData.EquipmentId }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult PartsAdd(EquipmentCombined ec)
        {
            string ModelValidationFailedMessage = string.Empty;
            EquipmentWrapper EWrapper = new EquipmentWrapper(userData);
            if (ModelState.IsValid)
            {
                ec = EWrapper.AddPart(ec);

                if (ec.partsSessionData.ErrorMessage != null && ec.partsSessionData.ErrorMessage.Count > 0)
                {
                    return Json(ec.partsSessionData.ErrorMessage, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), equipmentid = ec.partsSessionData.EquipmentId }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult PartsDelete(string _EquipmentPartSpecsId)
        {
            EquipmentWrapper EWrapper = new EquipmentWrapper(userData);
            if (EWrapper.DeleteParts(_EquipmentPartSpecsId))
            {
                return Json(new { Result = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Result = JsonReturnEnum.failed.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region PartIssued
        [HttpPost]
        public string GetEquipment_PartIssued(int? draw, int? start, int? length, long EquipmentId, DateTime? TransactionDate, string srcData = "", string PartClientLookupId = "", string ChargeToClientLookupId = "", string UnitofMeasure = "", string Description = "", string IssuedTo = "", string TransactionQuantity = "", string Cost = "")
        {
            List<PartIssues> PartIssuesModelList = new List<PartIssues>();
            var filter = srcData;
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];

            EquipmentWrapper eWrapper = new EquipmentWrapper(userData);
            List<PartIssues> PartIssuesList = new List<PartIssues>();
            PartIssuesList = eWrapper.GetEquipmentPartIssued(EquipmentId);

            PartIssuesList = this.GetEquipment_PartIssuedGridSortByColumnWithOrder(order, orderDir, PartIssuesList);
            if (PartIssuesList != null)
            {
                #region TextSearch
                if (!string.IsNullOrEmpty(filter))
                {
                    filter = filter.ToUpper();
                    PartIssuesList = PartIssuesList.Where(x => (!string.IsNullOrWhiteSpace(x.PartClientLookupId) && x.PartClientLookupId.ToUpper().Contains(filter))
                                                            || (!string.IsNullOrWhiteSpace(x.ChargeToClientLookupId) && x.ChargeToClientLookupId.ToUpper().Contains(filter))
                                                            || (!string.IsNullOrWhiteSpace(x.UnitofMeasure) && x.UnitofMeasure.ToUpper().Contains(filter))
                                                            || (!string.IsNullOrWhiteSpace(x.Description) && x.Description.Trim().ToUpper().Contains(filter))
                                                            || (x.TransactionDate.HasValue && x.TransactionDate.Value.Date.ToString("MM/dd/yyyy").Equals(filter))
                                                            || (Convert.ToString(x.TransactionQuantity).ToUpper().Contains(filter))
                                                            || (Convert.ToString(x.Cost).ToUpper().Contains(filter))
                                                            || (!string.IsNullOrWhiteSpace(x.IssuedTo) && x.IssuedTo.ToUpper().Contains(filter))
                                                            ).ToList();
                }
                #endregion

                #region AdvSearch
                if (TransactionDate != null)
                {
                    PartIssuesList = PartIssuesList.Where(x => (x.TransactionDate != null && x.TransactionDate.Value.Date.Equals(TransactionDate.Value.Date))).ToList();
                }
                if (!string.IsNullOrEmpty(PartClientLookupId) && PartClientLookupId != "0")
                {
                    PartClientLookupId = PartClientLookupId.ToUpper();
                    PartIssuesList = PartIssuesList.Where(x => (!string.IsNullOrWhiteSpace(x.PartClientLookupId) && x.PartClientLookupId.ToUpper().Contains(PartClientLookupId))).ToList();
                }
                if (!string.IsNullOrEmpty(Description))
                {
                    Description = Description.ToUpper();
                    PartIssuesList = PartIssuesList.Where(x => (!string.IsNullOrWhiteSpace(x.Description) && x.Description.ToUpper().Contains(Description))).ToList();
                }
                if (!string.IsNullOrEmpty(ChargeToClientLookupId) && ChargeToClientLookupId != "0")
                {
                    ChargeToClientLookupId = ChargeToClientLookupId.ToUpper();
                    PartIssuesList = PartIssuesList.Where(x => (!string.IsNullOrWhiteSpace(x.ChargeToClientLookupId) && x.ChargeToClientLookupId.ToUpper().Contains(ChargeToClientLookupId))).ToList();
                }
                if (!string.IsNullOrEmpty(UnitofMeasure))
                {
                    UnitofMeasure = UnitofMeasure.ToUpper();
                    PartIssuesList = PartIssuesList.Where(x => (!string.IsNullOrWhiteSpace(x.UnitofMeasure) && x.UnitofMeasure.ToUpper().Contains(UnitofMeasure))).ToList();
                }
                if (!string.IsNullOrEmpty(IssuedTo))
                {
                    IssuedTo = IssuedTo.ToUpper();
                    PartIssuesList = PartIssuesList.Where(x => (!string.IsNullOrWhiteSpace(x.IssuedTo) && x.IssuedTo.ToUpper().Contains(IssuedTo))).ToList();
                }
                if (!string.IsNullOrEmpty(TransactionQuantity))
                {
                    decimal tValue;
                    bool ParseResult = decimal.TryParse(TransactionQuantity, out tValue);
                    if (ParseResult)
                    {
                        PartIssuesList = PartIssuesList.Where(x => (x.TransactionQuantity.Equals(tValue))).ToList();
                    }

                }
                if (!string.IsNullOrEmpty(Cost))
                {
                    decimal costValue;
                    bool ParseResult = decimal.TryParse(Cost, out costValue);
                    PartIssuesList = PartIssuesList.Where(x => (x.Cost.Equals(costValue))).ToList();
                }
                #endregion
            }
            var totalRecords = 0;
            var recordsFiltered = 0;
            start = start.HasValue
                ? start / length
                : 0;
            recordsFiltered = PartIssuesList.Count();
            totalRecords = PartIssuesList.Count();

            int initialPage = start.Value;

            var filteredResult = PartIssuesList
                .Skip(initialPage * length ?? 0)
                .Take(length ?? 0)
                .ToList();

            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult }, JsonSerializerDateSettings);
        }
        private List<PartIssues> GetEquipment_PartIssuedGridSortByColumnWithOrder(string order, string orderDir, List<PartIssues> data)
        {
            List<PartIssues> lst = new List<PartIssues>();
            switch (order)
            {
                case "0":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.PartClientLookupId).ToList() : data.OrderBy(p => p.PartClientLookupId).ToList();
                    break;
                case "1":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Description).ToList() : data.OrderBy(p => p.Description).ToList();
                    break;
                case "2":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.TransactionDate).ToList() : data.OrderBy(p => p.TransactionDate).ToList();
                    break;
                case "3":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ChargeToClientLookupId).ToList() : data.OrderBy(p => p.ChargeToClientLookupId).ToList();
                    break;
                case "4":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.TransactionQuantity).ToList() : data.OrderBy(p => p.TransactionQuantity).ToList();
                    break;
                case "5":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.UnitofMeasure).ToList() : data.OrderBy(p => p.UnitofMeasure).ToList();
                    break;
                case "6":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Cost).ToList() : data.OrderBy(p => p.Cost).ToList();
                    break;
                case "7":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.IssuedTo).ToList() : data.OrderBy(p => p.IssuedTo).ToList();
                    break;
                default:
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.PartClientLookupId).ToList() : data.OrderBy(p => p.PartClientLookupId).ToList();
                    break;
            }
            return lst;
        }
        #endregion

        #region PMList
        [HttpPost]
        public string GetEquipment_PMList(int? draw, int? start, int? length, long EquipmentId)
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            EquipmentWrapper eWrapper = new EquipmentWrapper(userData);
            var PMList = eWrapper.GetEquipmentPMList(EquipmentId);
            PMList = this.GetAllPMListSortByColumnWithOrder(order, orderDir, PMList);
            var totalRecords = 0;
            var recordsFiltered = 0;
            start = start.HasValue
                ? start / length
                : 0;
            recordsFiltered = PMList.Count();
            totalRecords = PMList.Count();
            int initialPage = start.Value;
            var filteredResult = PMList
                .Skip(initialPage * length ?? 0)
                .Take(length ?? 0)
                .ToList();
            var pmSecurity = this.userData.Security.Equipment.Edit; //this.userData.Security.Parts.Part_Equipment_XRef; V2-845           
            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult, pmSecurity = pmSecurity }, JsonSerializer12HoursDateAndTimeSettings);
        }
        private List<PMListModel> GetAllPMListSortByColumnWithOrder(string order, string orderDir, List<PMListModel> data)
        {
            List<PMListModel> lst = new List<PMListModel>();

            switch (order)
            {
                case "0":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.AssignedTo_PersonnelName).ToList() : data.OrderBy(p => p.AssignedTo_PersonnelName).ToList();
                    break;
                case "1":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ClientLookupId).ToList() : data.OrderBy(p => p.ClientLookupId).ToList();
                    break;
                case "2":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Description).ToList() : data.OrderBy(p => p.Description).ToList();
                    break;
                case "3":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.LastPerformed).ToList() : data.OrderBy(p => p.LastPerformed).ToList();
                    break;
                case "4":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.LastScheduled).ToList() : data.OrderBy(p => p.LastScheduled).ToList();
                    break;
                default:
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.AssignedTo_PersonnelName).ToList() : data.OrderBy(p => p.AssignedTo_PersonnelName).ToList();
                    break;
            }
            return lst;
        }
        #endregion

        #region TechSpecs
        [HttpPost]
        public string GetEquipment_TechSpecs(int? draw, int? start, int? length, long EquipmentId)
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            EquipmentWrapper eWrapper = new EquipmentWrapper(userData);
            var TechSpecs = eWrapper.GetEquipmentTechSpecs(EquipmentId);
            TechSpecs = this.GetAllTechSpecsSortByColumnWithOrder(order, orderDir, TechSpecs);
            var totalRecords = 0;
            var recordsFiltered = 0;
            start = start.HasValue
                ? start / length
                : 0;
            recordsFiltered = TechSpecs.Count();
            totalRecords = TechSpecs.Count();
            int initialPage = start.Value;
            var filteredResult = TechSpecs
                .Skip(initialPage * length ?? 0)
                .Take(length ?? 0)
                .ToList();
            var techSpecsSecurity = this.userData.Security.Equipment.Edit;
            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult, techSpecsSecurity = techSpecsSecurity }, JsonSerializer12HoursDateAndTimeSettings);
        }
        private List<TechSpecsModel> GetAllTechSpecsSortByColumnWithOrder(string order, string orderDir, List<TechSpecsModel> data)
        {
            List<TechSpecsModel> lst = new List<TechSpecsModel>();
            switch (order)
            {
                case "0":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ClientLookupId).ToList() : data.OrderBy(p => p.ClientLookupId).ToList();
                    break;
                case "1":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.SpecValue).ToList() : data.OrderBy(p => p.SpecValue).ToList();
                    break;
                //case "2":      /*V2-295*/
                //    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.UnitOfMeasure).ToList() : data.OrderBy(p => p.UnitOfMeasure).ToList();
                //    break;
                case "2":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Description).ToList() : data.OrderBy(p => p.Description).ToList();
                    break;
                case "3":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Comments).ToList() : data.OrderBy(p => p.Comments).ToList();
                    break;
                default:
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ClientLookupId).ToList() : data.OrderBy(p => p.ClientLookupId).ToList();
                    break;
            }
            return lst;
        }
        public ActionResult TechSpecsDelete(string _EquipmentTechSpecsId)
        {
            EquipmentWrapper EWrapper = new EquipmentWrapper(userData);
            if (EWrapper.DeleteTechSpecs(_EquipmentTechSpecsId))
            {
                return Json(new { Result = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Result = JsonReturnEnum.failed.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [ActionName("AddTechSpecs")]
        public ActionResult TechSpecsAdd(long EquipmentId, string TechMode, string ClientLookupId, string Name, long TechSpecId = 0, bool isRemoveFromService = false, string Status = "")
        {
            EquipmentCombined objComb = new EquipmentCombined();
            TechSpecsModel model = new TechSpecsModel();
            string Mode = TechMode;
            EquipmentWrapper EWrapper = new EquipmentWrapper(userData);
            model.Mode = Mode;

            objComb._EquipmentSummaryModel = GetEquipmentSummary(EquipmentId, ClientLookupId, Name, isRemoveFromService, Status);
            model.EquipmentId = EquipmentId;

            List<DataContracts.LookupList> AllLookUps = new List<DataContracts.LookupList>();
            List<DataContracts.LookupList> TechSpecList = new List<DataContracts.LookupList>();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            AllLookUps = commonWrapper.GetAllLookUpList();
            if (AllLookUps != null)
            {
                TechSpecList = AllLookUps.Where(x => x.ListName == LookupListConstants.TECH_SPEC).ToList();
                TechSpecList = TechSpecList.GroupBy(x => x.ListValue).Select(x => x.FirstOrDefault()).ToList();
            }


            objComb.techSpecsModel = EWrapper.TechSpecsAdd(EquipmentId, model, TechSpecList, TechSpecId);
            objComb._userdata = userData;
            LocalizeControls(objComb, LocalizeResourceSetConstants.EquipmentDetails);
            return PartialView("AddTechSpecs", objComb);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TechSpecsAdd(EquipmentCombined ec)
        {
            EquipmentCombined objComb = new EquipmentCombined();
            EquipmentWrapper EWrapper = new EquipmentWrapper(userData);
            string ModelValidationFailedMessage = string.Empty;
            if (ModelState.IsValid)
            {
                objComb = EWrapper.TechSpecsAddEdit(ec);
                if (objComb.techSpecsModel.ErrorMessage != null && objComb.techSpecsModel.ErrorMessage.Count > 0)
                {
                    return Json(objComb.techSpecsModel.ErrorMessage, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), mode = objComb.techSpecsModel.Mode, equipmentid = objComb.techSpecsModel.EquipmentId }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Downtime
        [HttpPost]
        public string GetEquipment_Downtime(int? draw, int? start, int? length, long EquipmentId)
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            EquipmentWrapper eWrapper = new EquipmentWrapper(userData);
            start = start.HasValue
                ? start / length
                : 0;
            int skip = start * length ?? 0;
            //V2-695
            var Downtime = eWrapper.GetEquipmentDowntime_V2(EquipmentId, order, orderDir, skip, length ?? 0);

            var totalRecords = 0;
            var recordsFiltered = 0;
            start = start.HasValue
                ? start / length
                : 0;
            if (Downtime != null && Downtime.Count > 0)
            {
                recordsFiltered = Downtime[0].TotalCount;
                totalRecords = Downtime[0].TotalCount;
            }
            else
            {
                recordsFiltered = 0;
                totalRecords = 0;
            }

            int initialPage = start.Value;
            var filteredResult = Downtime
                .Skip(initialPage * length ?? 0)
                .Take(length ?? 0)
                .ToList();

            //V2-695
            var secDownTimeAdd = this.userData.Security.Asset_Downtime.Create;
            var secDownTimeEdit = this.userData.Security.Asset_Downtime.Edit;
            var secDownTimeDelete = this.userData.Security.Asset_Downtime.Delete;
            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult, secDownTimeAdd = secDownTimeAdd, secDownTimeEdit = secDownTimeEdit, secDownTimeDelete = secDownTimeDelete }, JsonSerializerDateSettings);
        }
        private List<DownTimeModel> GetAllDowntimeSortByColumnWithOrder(string order, string orderDir, List<DownTimeModel> data)
        {
            List<DownTimeModel> lst = new List<DownTimeModel>();

            switch (order)
            {
                case "0":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.WorkOrderClientLookupId).ToList() : data.OrderBy(p => p.WorkOrderClientLookupId).ToList();
                    break;
                case "1":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.DateDown).ToList() : data.OrderBy(p => p.DateDown).ToList();
                    break;
                case "2":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.MinutesDown).ToList() : data.OrderBy(p => p.MinutesDown).ToList();
                    break;
                default:
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.WorkOrderClientLookupId).ToList() : data.OrderBy(p => p.WorkOrderClientLookupId).ToList();
                    break;
            }

            return lst;
        }
        [HttpPost]
        [ActionName("RedirectDowntime")]
        public ActionResult DownTimeAdd(long EquipmentId, string ClientLookupId, string Name, bool isRemoveFromService, string Status)
        {
            EquipmentCombined objComb = new EquipmentCombined();
            DownTimeModel objDownTimeModel = new DownTimeModel();
            EquipmentWrapper EWrapper = new EquipmentWrapper(userData);
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            objComb._EquipmentSummaryModel = GetEquipmentSummary(EquipmentId, ClientLookupId, Name, isRemoveFromService, Status);
            objDownTimeModel.EquipmentId = EquipmentId;
            objDownTimeModel.PersonnelClientLookupId = userData.DatabaseKey.Personnel.ClientLookupId;
            objDownTimeModel.DateDown = DateTime.Now;
            objComb.downTimeModel = objDownTimeModel;
            objComb._userdata = userData;
            //V2-695
            var AllLookUps = commonWrapper.GetAllLookUpList();
            var ReasonforDown = AllLookUps.Where(x => x.ListName == LookupListConstants.WO_DOWN_REASON).ToList();
            if (ReasonforDown != null)
            {
                objComb.ReasonForDownList = ReasonforDown.Select(x => new SelectListItem { Text = x.ListValue + " - " + x.Description, Value = x.ListValue }).ToList();
            }
            LocalizeControls(objComb, LocalizeResourceSetConstants.EquipmentDetails);
            return PartialView("AddDowntime", objComb);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DownTimeAdd(EquipmentCombined ec)
        {
            List<string> errorMessage = new List<string>();
            EquipmentWrapper EWrapper = new EquipmentWrapper(userData);
            string ModelValidationFailedMessage = string.Empty;

            if (ModelState.IsValid)
            {
                errorMessage = EWrapper.AddDownTime(ec);
                if (errorMessage != null && errorMessage.Count > 0)
                {
                    return Json(errorMessage, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), equipmentid = ec.downTimeModel.EquipmentId }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult DownTimeDelete(string _DowntimeId)
        {
            EquipmentWrapper EWrapper = new EquipmentWrapper(userData);
            if (EWrapper.DeleteDownTime(_DowntimeId))
            {
                return Json(new { Result = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Result = JsonReturnEnum.failed.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult ShowDownTimeEdit(long EquipmentId, long DownTimeId, string ClientLookupId, string Name, bool isRemoveFromService, string Status)
        {

            EquipmentCombined objComb = new EquipmentCombined();
            DownTimeModel objDownTimeModel = new DownTimeModel();
            DowntimeSessionData _DowntimeSessionData = new DowntimeSessionData();
            EquipmentWrapper EWrapper = new EquipmentWrapper(userData);
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            objComb._EquipmentSummaryModel = GetEquipmentSummary(EquipmentId, ClientLookupId, Name, isRemoveFromService, Status);
            objComb = EWrapper.ShowEditDownTime(EquipmentId, DownTimeId, objComb, objDownTimeModel);
            objComb._userdata = userData;
            //V2-695
            var AllLookUps = commonWrapper.GetAllLookUpList();
            var ReasonforDown = AllLookUps.Where(x => x.ListName == LookupListConstants.WO_DOWN_REASON).ToList();
            if (ReasonforDown != null)
            {
                objComb.ReasonForDownList = ReasonforDown.Select(x => new SelectListItem { Text = x.ListValue + " - " + x.Description, Value = x.ListValue }).ToList();
            }
            LocalizeControls(objComb, LocalizeResourceSetConstants.EquipmentDetails);
            return PartialView("EditDowntime", objComb);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DownTimeEdit(EquipmentCombined ec)
        {
            EquipmentWrapper EWrapper = new EquipmentWrapper(userData);
            string ModelValidationFailedMessage = string.Empty;
            if (ModelState.IsValid)
            {
                var downTime = EWrapper.EditDownTime(ec);
                if (downTime.ErrorMessages != null && downTime.ErrorMessages.Count > 0)
                {
                    return Json(downTime.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), equipmentid = ec.downTimeModel.EquipmentId }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region WOActive
        [HttpPost]
        public string GetEquipment_WOActive(int? draw, int? start, int? length, long EquipmentId, DateTime? CreateDate, string srcData = "", string ClientLookupId = "", string Description = "", string WorkAssigned_PersonnelClientLookupId = "", string Status_Display = "", string Type = "")
        {
            var filter = srcData;
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            EquipmentWrapper eWrapper = new EquipmentWrapper(userData);
            List<WOActiveModel> WOActiveModelList = eWrapper.getDetailsWOActive(true, EquipmentId);
            WOActiveModelList = this.GetWOActiveGridSortByColumnWithOrder(order, orderDir, WOActiveModelList);
            if (WOActiveModelList != null)
            {
                #region TextSearch
                if (!string.IsNullOrEmpty(filter))
                {
                    filter = filter.ToUpper();
                    WOActiveModelList = WOActiveModelList.Where(x => (!string.IsNullOrWhiteSpace(x.ClientLookupId) && x.ClientLookupId.ToUpper().Contains(filter))
                                                            || (!string.IsNullOrWhiteSpace(x.WorkAssigned_PersonnelClientLookupId) && x.WorkAssigned_PersonnelClientLookupId.ToUpper().Contains(filter))
                                                            || (!string.IsNullOrWhiteSpace(x.Status_Display) && x.Status_Display.ToUpper().Contains(filter))
                                                            || (!string.IsNullOrWhiteSpace(x.Description) && x.Description.Trim().ToUpper().Contains(filter))
                                                            || ((x.CreateDate.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)).Contains(filter))
                                                            || (!string.IsNullOrWhiteSpace(x.Type) && x.Type.ToUpper().Contains(filter))
                                                            ).ToList();
                }
                #endregion
                #region AdvSearch
                if (CreateDate != null)
                {
                    WOActiveModelList = WOActiveModelList.Where(x => (x.CreateDate != null && x.CreateDate.Date.Equals(CreateDate.Value.Date))).ToList();
                }
                if (!string.IsNullOrEmpty(ClientLookupId) && ClientLookupId != "0")
                {
                    ClientLookupId = ClientLookupId.ToUpper();
                    WOActiveModelList = WOActiveModelList.Where(x => (!string.IsNullOrWhiteSpace(x.ClientLookupId) && x.ClientLookupId.ToUpper().Contains(ClientLookupId))).ToList();
                }
                if (!string.IsNullOrEmpty(Description))
                {
                    Description = Description.ToUpper();
                    WOActiveModelList = WOActiveModelList.Where(x => (!string.IsNullOrWhiteSpace(x.Description) && x.Description.ToUpper().Contains(Description))).ToList();
                }
                if (!string.IsNullOrEmpty(WorkAssigned_PersonnelClientLookupId) && WorkAssigned_PersonnelClientLookupId != "0")
                {
                    WorkAssigned_PersonnelClientLookupId = WorkAssigned_PersonnelClientLookupId.ToUpper();
                    WOActiveModelList = WOActiveModelList.Where(x => (!string.IsNullOrWhiteSpace(x.WorkAssigned_PersonnelClientLookupId) && x.WorkAssigned_PersonnelClientLookupId.ToUpper().Contains(WorkAssigned_PersonnelClientLookupId))).ToList();
                }
                if (!string.IsNullOrEmpty(Status_Display))
                {
                    Status_Display = Status_Display.ToUpper();
                    WOActiveModelList = WOActiveModelList.Where(x => (!string.IsNullOrWhiteSpace(x.Status_Display) && x.Status_Display.ToUpper().Contains(Status_Display))).ToList();
                }
                if (!string.IsNullOrEmpty(Type))
                {
                    Type = Type.ToUpper();
                    WOActiveModelList = WOActiveModelList.Where(x => (!string.IsNullOrWhiteSpace(x.Type) && x.Type.ToUpper().Contains(Type))).ToList();
                }
                #endregion
            }
            var totalRecords = 0;
            var recordsFiltered = 0;
            start = start.HasValue
                ? start / length
                : 0;
            recordsFiltered = WOActiveModelList.Count();
            totalRecords = WOActiveModelList.Count();
            int initialPage = start.Value;
            var filteredResult = WOActiveModelList
                .Skip(initialPage * length ?? 0)
                .Take(length ?? 0)
                .ToList();
            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult }, JsonSerializerDateSettings);
        }

        private List<WOActiveModel> GetWOActiveGridSortByColumnWithOrder(string order, string orderDir, List<WOActiveModel> data)
        {
            List<WOActiveModel> lst = new List<WOActiveModel>();

            switch (order)
            {
                case "0":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ClientLookupId).ToList() : data.OrderBy(p => p.ClientLookupId).ToList();
                    break;
                case "1":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Description).ToList() : data.OrderBy(p => p.Description).ToList();
                    break;
                case "2":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.WorkAssigned_PersonnelClientLookupId).ToList() : data.OrderBy(p => p.WorkAssigned_PersonnelClientLookupId).ToList();
                    break;
                case "3":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Status_Display).ToList() : data.OrderBy(p => p.Status_Display).ToList();
                    break;
                case "4":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Type).ToList() : data.OrderBy(p => p.Type).ToList();
                    break;
                case "5":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.CreateDate).ToList() : data.OrderBy(p => p.CreateDate).ToList();
                    break;
                default:
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ClientLookupId).ToList() : data.OrderBy(p => p.ClientLookupId).ToList();
                    break;
            }
            return lst;
        }
        #endregion

        #region WOComplete
        [HttpPost]
        public string GetEquipment_WOComplete(int? draw, int? start, int? length, long EquipmentId, DateTime? CreateDate, string srcData = "", string ClientLookupId = "", string Description = "", string WorkAssigned_PersonnelClientLookupId = "", string Status_Display = "", string Type = "")
        {
            var filter = srcData;
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            EquipmentWrapper eWrapper = new EquipmentWrapper(userData);
            List<WOComplete> WOCompleteList = eWrapper.getDetailsWOComplete(false, EquipmentId);
            WOCompleteList = this.GetWOCompleteGridSortByColumnWithOrder(order, orderDir, WOCompleteList);
            if (WOCompleteList != null)
            {
                #region TextSearch
                if (!string.IsNullOrEmpty(filter))
                {
                    filter = filter.ToUpper();
                    WOCompleteList = WOCompleteList.Where(x => (!string.IsNullOrWhiteSpace(x.ClientLookupId) && x.ClientLookupId.ToUpper().Contains(filter))
                                                            || (!string.IsNullOrWhiteSpace(x.WorkAssigned_PersonnelClientLookupId) && x.WorkAssigned_PersonnelClientLookupId.ToUpper().Contains(filter))
                                                            || (!string.IsNullOrWhiteSpace(x.Status_Display) && x.Status_Display.ToUpper().Contains(filter))
                                                            || (!string.IsNullOrWhiteSpace(x.Description) && x.Description.Trim().ToUpper().Contains(filter))
                                                            || ((x.CreateDate.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)).Contains(filter))
                                                            || (!string.IsNullOrWhiteSpace(x.Type) && x.Type.ToUpper().Contains(filter))
                                                            ).ToList();
                }
                #endregion
                #region Advsearch
                if (CreateDate != null)
                {
                    WOCompleteList = WOCompleteList.Where(x => (x.CreateDate != null && x.CreateDate.Date.Equals(CreateDate.Value.Date))).ToList();
                }
                if (!string.IsNullOrEmpty(ClientLookupId) && ClientLookupId != "0")
                {
                    ClientLookupId = ClientLookupId.ToUpper();
                    WOCompleteList = WOCompleteList.Where(x => (!string.IsNullOrWhiteSpace(x.ClientLookupId) && x.ClientLookupId.ToUpper().Contains(ClientLookupId))).ToList();
                }
                if (!string.IsNullOrEmpty(Description))
                {
                    Description = Description.ToUpper();
                    WOCompleteList = WOCompleteList.Where(x => (!string.IsNullOrWhiteSpace(x.Description) && x.Description.ToUpper().Contains(Description))).ToList();
                }
                if (!string.IsNullOrEmpty(WorkAssigned_PersonnelClientLookupId) && WorkAssigned_PersonnelClientLookupId != "0")
                {
                    WorkAssigned_PersonnelClientLookupId = WorkAssigned_PersonnelClientLookupId.ToUpper();
                    WOCompleteList = WOCompleteList.Where(x => (!string.IsNullOrWhiteSpace(x.WorkAssigned_PersonnelClientLookupId) && x.WorkAssigned_PersonnelClientLookupId.ToUpper().Contains(WorkAssigned_PersonnelClientLookupId))).ToList();
                }
                if (!string.IsNullOrEmpty(Status_Display))
                {
                    Status_Display = Status_Display.ToUpper();
                    WOCompleteList = WOCompleteList.Where(x => (!string.IsNullOrWhiteSpace(x.Status_Display) && x.Status_Display.ToUpper().Contains(Status_Display))).ToList();
                }
                if (!string.IsNullOrEmpty(Type))
                {
                    Type = Type.ToUpper();
                    WOCompleteList = WOCompleteList.Where(x => (!string.IsNullOrWhiteSpace(x.Type) && x.Type.ToUpper().Contains(Type))).ToList();
                }
                #endregion
            }
            var totalRecords = 0;
            var recordsFiltered = 0;
            start = start.HasValue
                ? start / length
                : 0;
            recordsFiltered = WOCompleteList.Count();
            totalRecords = WOCompleteList.Count();
            int initialPage = start.Value;
            var filteredResult = WOCompleteList
                .Skip(initialPage * length ?? 0)
                .Take(length ?? 0)
                .ToList();
            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult }, JsonSerializerDateSettings);
        }

        private List<WOComplete> GetWOCompleteGridSortByColumnWithOrder(string order, string orderDir, List<WOComplete> data)
        {
            List<WOComplete> lst = new List<WOComplete>();
            switch (order)
            {
                case "0":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ClientLookupId).ToList() : data.OrderBy(p => p.ClientLookupId).ToList();
                    break;
                case "1":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Description).ToList() : data.OrderBy(p => p.Description).ToList();
                    break;
                case "2":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.WorkAssigned_PersonnelClientLookupId).ToList() : data.OrderBy(p => p.WorkAssigned_PersonnelClientLookupId).ToList();
                    break;
                case "3":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Status_Display).ToList() : data.OrderBy(p => p.Status_Display).ToList();
                    break;
                case "4":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Type).ToList() : data.OrderBy(p => p.Type).ToList();
                    break;
                case "5":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.CreateDate).ToList() : data.OrderBy(p => p.CreateDate).ToList();
                    break;
                default:
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ClientLookupId).ToList() : data.OrderBy(p => p.ClientLookupId).ToList();
                    break;
            }
            return lst;
        }
        #endregion

        #region Children
        public JsonResult GetAllFreeEquipmentList(long EquipmentId)
        {
            Equipment equipment = new Equipment();
            equipment.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            equipment.ClientId = userData.DatabaseKey.Client.ClientId;
            equipment.EquipmentId = EquipmentId;
            List<Equipment> equipmentFreeList = equipment.GetAllEquipmentFreeChildren(this.userData.DatabaseKey);
            var eqpList = equipmentFreeList != null ? equipmentFreeList.Select(x => new { x.EquipmentId, x.ClientLookupId, x.Name, x.SerialNumber, x.Type, x.Make, x.Model }).ToList() : null;
            return Json(new { data = eqpList }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult AddSelectedChildren(string ListofIds, long EquipmentId)
        {
            EquipmentWrapper eWrapper = new EquipmentWrapper(userData);
            eWrapper.AddChildren(ListofIds, EquipmentId);
            return Json(new { data = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DeleteChild(string _eqid)
        {
            EquipmentWrapper eWrapper = new EquipmentWrapper(userData);
            if (eWrapper.DeleteChild(_eqid))
            {
                return Json(new { Result = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Result = JsonReturnEnum.failed.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }



        #region V2-1213
        [HttpPost]
        public string GetAllChildrenEquipmentList_SensorChunkSearch(int? draw, int? start, int? length, long EquipmentId, string ClientLookupId = "", string Name = "", string SerialNumber = "", string Type = "", string Make = "", string Model = "")
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            start = start.HasValue
              ? start / length
              : 0;
            int skip = start * length ?? 0;
            EquipmentWrapper eWrapper = new EquipmentWrapper(userData);
            var ChildrenList = eWrapper.GetEquipment_ChildrenGridData(skip, length ?? 0, order, orderDir, EquipmentId, ClientLookupId, Name, SerialNumber, Type, Make, Model);
            var totalRecords = 0;
            var recordsFiltered = 0;
            if (ChildrenList != null)
            {
                recordsFiltered = ChildrenList[0].TotalCount;
                totalRecords = ChildrenList[0].TotalCount;
            }
            int initialPage = start.Value;
            var filteredResult = ChildrenList
              .ToList();
            var SecurityEditVal = userData.Security.Equipment.Edit;
            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult, SecurityEditVal = SecurityEditVal }, JsonSerializer12HoursDateAndTimeSettings);

        }
        #endregion
        #endregion

        #region Comment       
        [HttpPost]
        public PartialViewResult LoadComments(long EquipmentId)
        {
            EquipmentCombined _EquipmentObj = new EquipmentCombined();
            CommonWrapper coWrapper = new CommonWrapper(userData);
            List<Personnel> personnelsList = new List<Personnel>();
            List<Notes> NotesList = new List<Notes>();
            UserMentionData userMentionData;
            List<UserMentionData> userMentionDatas = new List<UserMentionData>();
            Task[] tasks = new Task[2];
            tasks[0] = Task.Factory.StartNew(() => personnelsList = coWrapper.MentionList(""));
            tasks[1] = Task.Factory.StartNew(() => NotesList = coWrapper.PopulateComment(EquipmentId, "Equipment"));
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
                _EquipmentObj.userMentionDatas = userMentionDatas;
            }
            if (!tasks[1].IsFaulted && tasks[1].IsCompleted)
            {
                _EquipmentObj.NotesList = NotesList;
            }
            LocalizeControls(_EquipmentObj, LocalizeResourceSetConstants.EquipmentDetails);
            return PartialView("_CommentsList", _EquipmentObj);
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult AddOrUpdateComment(long EquipmentId, string content, string EqClientLookupId, List<string> userList, long noteId = 0, long updatedindex = 0)
        {
            CommonWrapper coWrapper = new CommonWrapper(userData);
            var namelist = coWrapper.MentionList("");
            List<UserMentionData> userMentionDataList = new List<UserMentionData>();
            UserMentionData objUserMentionData;
            if (userList != null && userList.Count > 0)
            {
                foreach (var item in userList)
                {
                    objUserMentionData = new UserMentionData();
                    objUserMentionData.userId = namelist.Where(x => x.UserName == item).Select(y => y.PersonnelId).FirstOrDefault();
                    objUserMentionData.userName = item;
                    objUserMentionData.emailId = namelist.Where(x => x.UserName == item).Select(y => y.Email).FirstOrDefault();
                    userMentionDataList.Add(objUserMentionData);
                }
            }
            NotesModel notesModel = new NotesModel();
            notesModel.ObjectId = EquipmentId;
            notesModel.Content = content;
            notesModel.NotesId = noteId;
            notesModel.ClientLookupId = EqClientLookupId;
            notesModel.updatedindex = updatedindex;
            if (ModelState.IsValid)
            {
                string Mode = string.Empty;
                List<String> errorList = new List<string>();
                errorList = coWrapper.AddOrUpdateComment(notesModel, ref Mode, AttachmentTableConstant.Equipment, userMentionDataList);
                if (errorList != null && errorList.Count > 0)
                {
                    return Json(errorList, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), equipmentid = EquipmentId, mode = Mode }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                string ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Attachment
        [HttpPost]
        public string PopulateAttachment(int? draw, int? start, int? length, long EquipmentId)
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            CommonWrapper objCommonWrapper = new CommonWrapper(userData);
            var AttachmentList = objCommonWrapper.PopulateAttachments(EquipmentId, AttachmentTableConstant.Equipment, userData.Security.Equipment.Edit);
            if (AttachmentList != null)
            {
                AttachmentList = GetAllAttachmentsSortByColumnWithOrder(order, orderDir, AttachmentList);
            }
            var totalRecords = 0;
            var recordsFiltered = 0;
            start = start.HasValue
                ? start / length
                : 0;
            recordsFiltered = AttachmentList.Count();
            totalRecords = AttachmentList.Count();
            int initialPage = start.Value;
            var filteredResult = AttachmentList
                .Skip(initialPage * length ?? 0)
                .Take(length ?? 0)
                .ToList();
            var Attachsecurity = userData.Security.Equipment.Edit;
            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult, Attachsecurity = Attachsecurity }, JsonSerializer12HoursDateAndTimeSettings);
        }
        [HttpGet]
        public ActionResult ShowAddAttachment(long EquipmentId, string ClientLookupId, string Name, bool isRemoveFromService, string Status)
        {
            EquipmentCombined _EquipmentObj = new EquipmentCombined();
            EquipmentWrapper EWrapper = new EquipmentWrapper(userData);
            _EquipmentObj._EquipmentSummaryModel = GetEquipmentSummary(EquipmentId, ClientLookupId, Name, isRemoveFromService, Status);
            AttachmentModel Attachment = new AttachmentModel();
            Attachment.EquipmentId = EquipmentId;
            Attachment.ClientLookupId = ClientLookupId;
            _EquipmentObj.attachmentModel = Attachment;
            _EquipmentObj._userdata = userData;
            LocalizeControls(_EquipmentObj, LocalizeResourceSetConstants.EquipmentDetails);
            return PartialView("AttachmentAdd", _EquipmentObj);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddAttachment()
        {
            string ModelValidationFailedMessage = string.Empty;
            if (ModelState.IsValid)
            {
                Stream stream = Request.Files[0].InputStream;
                var fileName = Request.Files[0].FileName;
                AttachmentModel attachmentModel = new AttachmentModel();
                CommonWrapper objCommonWrapper = new CommonWrapper(userData);
                Attachment fileAtt = new Attachment();
                attachmentModel.FileName = System.IO.Path.GetFileNameWithoutExtension(Request.Files[0].FileName);
                string fileExt = System.IO.Path.GetExtension(Request.Files[0].FileName);
                attachmentModel.FileType = fileExt.Substring(1);
                attachmentModel.FileSize = Request.Files[0].ContentLength;
                attachmentModel.ContentType = Request.Files[0].ContentType;
                attachmentModel.ObjectId = Convert.ToInt64(Request.Form["attachmentModel.EquipmentId"]);
                attachmentModel.Subject = String.IsNullOrEmpty(Request.Form["attachmentModel.Subject"]) ? "No Subject" : Request.Form["attachmentModel.Subject"];
                attachmentModel.TableName = AttachmentTableConstant.Equipment;
                //V2-725 start
                bool IsduplicateAttachmentFileExist = false;
                IsduplicateAttachmentFileExist = objCommonWrapper.IsduplicateFileExist(attachmentModel.ObjectId, attachmentModel.TableName, userData.Security.Equipment.Edit, attachmentModel.FileName, attachmentModel.FileType);
                if (IsduplicateAttachmentFileExist)
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), IsduplicateAttachmentFileExist = IsduplicateAttachmentFileExist, equipmentid = attachmentModel.ObjectId }, JsonRequestBehavior.AllowGet);
                }
                //end
                bool attachStatus = false;
                bool OnPremise = userData.DatabaseKey.Client.OnPremise;
                if (OnPremise)
                {
                    fileAtt = objCommonWrapper.AddAttachmentOnPremise(attachmentModel, stream, ref attachStatus, userData.Security.Equipment.Edit);
                }
                else
                {
                    fileAtt = objCommonWrapper.AddAttachment(attachmentModel, stream, ref attachStatus, userData.Security.Equipment.Edit);
                }

                if (attachStatus)
                {
                    if (fileAtt.ErrorMessages != null && fileAtt.ErrorMessages.Count > 0)
                    {
                        return Json(fileAtt.ErrorMessages, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { Result = JsonReturnEnum.success.ToString(), IsduplicateAttachmentFileExist = IsduplicateAttachmentFileExist, equipmentid = Convert.ToInt64(Request.Form["attachmentModel.EquipmentId"]) }, JsonRequestBehavior.AllowGet);
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
        public ActionResult DeleteAttachment(long _fileAttachmentId)
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

        #region Sensor
        [HttpGet]
        public ActionResult BindSensorList(long EquipmentId)
        {
            MonnitSensor.Sensor Senr = new MonnitSensor.Sensor();
            List<MonnitSensor.SensorModel> GridSource = Senr.SensorList(userData.DatabaseKey.Client.ClientId, userData.Site.SiteId);
            EquipmentWrapper EWrapper = new EquipmentWrapper(userData);
            var DataSource = EWrapper.BindSensors(EquipmentId);

            foreach (var item in DataSource)
            {
                GridSource.RemoveAll(sen => sen.SensorID == item.SensorId.ToString());
            }
            return Json(new { data = GridSource }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddSensor(string _SensorIds, long EquipmentId)
        {
            EquipmentWrapper EWrapper = new EquipmentWrapper(userData);
            EWrapper.AddNewSensor(_SensorIds, EquipmentId);
            return Json(new { data = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Equipment Dashboard graph
        [HttpPost]
        public ActionResult EquipMentChartData(int EquipmentId, int timeframe = 1)
        {
            Chart _chart = new Chart();
            EquipmentWrapper EWrapper = new EquipmentWrapper(userData);
            _chart = EWrapper.EquipMentChartData(EquipmentId, timeframe);

            return Json(_chart, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult WObyTypeData(int wotimeframe = 1)
        {
            List<int> dataList = new List<int>();
            List<string> LabelList = new List<string>();
            List<string> bColorList = new List<string>();
            List<string> hColorList = new List<string>();

            Random random = new Random();
            int flag = 1;

            List<KeyValuePair<string, long>> entries = DashboardReports.WorkOrder_ByWorkOrderType(userData.DatabaseKey, wotimeframe, flag);

            foreach (var ent in entries)
            {
                dataList.Add((int)ent.Value);
                LabelList.Add(ent.Key);
                string bColor = string.Format("#{0:X6}", random.Next(0x1000000));
                bColorList.Add(bColor);
                string hColor = string.Format("#{0:X6}", random.Next(0x1000000));
                hColorList.Add(hColor);
            }
            return Json(new { series = dataList.ToArray(), labels = LabelList.ToArray(), backgroundColor = bColorList.ToArray(), hoverBackgroundColor = hColorList.ToArray() }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Private method
        private EquipmentSummaryModel GetEquipmentSummary(long EquipmentId, string ClientLookupId, string Name, bool isRemoveFromService, string Status)
        {
            int Flag = 1;
            long thisCount = 0;
            EquipmentSummaryModel summary = new EquipmentSummaryModel();

            summary.Equipment_ClientLookupId = ClientLookupId;
            summary.EquipmentName = Name;

            //V2-636 Asset Availability
            CommonWrapper objCommonWrapper = new CommonWrapper(userData);
            var AssetStatusList = objCommonWrapper.GetListFromConstVals(LookupListConstants.AssetStatus);
            if (AssetStatusList != null)
            {
                var AssetStatusAllList = AssetStatusList.Select(x => new SelectListItem { Text = x.text.ToString(), Value = x.value.ToString() });
                if (Status != "")
                {
                    var statusText = AssetStatusAllList.Where(x => x.Value == Status).Select(x => x.Text).FirstOrDefault();
                    if (statusText != null)
                    {
                        summary.Status = statusText.ToString();
                    }

                }
            }
            summary.RemoveFromService = isRemoveFromService;

            //V2-636
            EquipmentWrapper eWrapper = new EquipmentWrapper(userData);
            CommonWrapper comWrapper = new CommonWrapper(userData);

            List<KeyValuePair<string, long>> widgetsCount = DashboardReports.WorkOrder_RetrieveByWOStatusForChart_1(userData.DatabaseKey, Flag, EquipmentId);
            if (widgetsCount != null && widgetsCount.Count > 0)
            {
                thisCount = widgetsCount.Where(x => x.Key.Equals("Approved") || x.Key.Equals("Scheduled") || x.Key.Equals("WorkRequest")).Sum(x => x.Value);
                summary.OpenWorkOrders = thisCount;
            }
            if (widgetsCount != null && widgetsCount.Count > 0)
            {
                thisCount = widgetsCount.Where(x => x.Key.Equals("WorkRequest")).Sum(x => x.Value);
                summary.WorkRequests = thisCount;
            }
            List<KeyValuePair<long, long>> overDueEntries = DashboardReports.WorkOrder_RetrieveByWOStatusForChart_3(userData.DatabaseKey, Flag, EquipmentId);
            if (overDueEntries != null && overDueEntries.Count > 0)
            {
                thisCount = overDueEntries[0].Value;
                summary.OverduePms = thisCount;
            }
            summary.ClientOnPremise = userData.DatabaseKey.Client.OnPremise;
            if (summary.ClientOnPremise)
            {
                summary.ImageUrl = comWrapper.GetOnPremiseImageUrl(EquipmentId, AttachmentTableConstant.Equipment);
            }
            else
            {
                summary.ImageUrl = comWrapper.GetAzureImageUrl(EquipmentId, AttachmentTableConstant.Equipment);
            }
            return summary;
        }
        private List<DataContracts.Equipment> GetAllEquipmentsSortByColumnWithOrder(string order, string orderDir, List<DataContracts.Equipment> data)
        {
            List<DataContracts.Equipment> lst = new List<DataContracts.Equipment>();
            switch (order)
            {
                case "1":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ClientLookupId).ToList() : data.OrderBy(p => p.ClientLookupId).ToList();
                    break;
                case "2":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Name).ToList() : data.OrderBy(p => p.Name).ToList();
                    break;
                case "3":
                    if (userData.DatabaseKey.Client.BusinessType.ToUpper() != BusinessTypeConstants.Facilities)
                    {
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Location).ToList() : data.OrderBy(p => p.Location).ToList();
                    }
                    else
                    {
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.LocationIdClientLookupId).ToList() : data.OrderBy(p => p.LocationIdClientLookupId).ToList();
                    }
                    break;


                case "4":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.SerialNumber).ToList() : data.OrderBy(p => p.SerialNumber).ToList();
                    break;
                case "5":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Type).ToList() : data.OrderBy(p => p.Type).ToList();
                    break;
                case "6":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Make).ToList() : data.OrderBy(p => p.Make).ToList();
                    break;
                case "7":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Model).ToList() : data.OrderBy(p => p.Model).ToList();
                    break;
                case "8":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Type).ToList() : data.OrderBy(p => p.Type).ToList();
                    break;
                case "9":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.AssetNumber).ToList() : data.OrderBy(p => p.AssetNumber).ToList();
                    break;
                default:
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ClientLookupId).ToList() : data.OrderBy(p => p.ClientLookupId).ToList();
                    break;
            }
            return lst;
        }



        private List<DataContracts.Equipment> GetEquipmentsSearchResult(List<DataContracts.Equipment> equipmentList, string ClientLookupId, string Name, string Location, string SerialNumber, string Type, string Make, string Model, string LaborAccountClientLookupId, string AssetNumber, string Area_Desc, int AssetGroup1Id, int AssetGroup2Id, int AssetGroup3Id, string searchText)
        {
            if (equipmentList != null)
            {
                //#region TextSearch
                if (!string.IsNullOrEmpty(searchText))
                {
                    searchText = searchText.ToUpper();
                    equipmentList = equipmentList.Where(x => (!string.IsNullOrWhiteSpace(x.ClientLookupId) && x.ClientLookupId.ToUpper().Contains(searchText)) || (!string.IsNullOrWhiteSpace(x.Name) && x.Name.ToUpper().Contains(searchText)) || (!string.IsNullOrWhiteSpace(x.Location) && x.Location.ToUpper().Contains(searchText)) || (!string.IsNullOrWhiteSpace(x.Type) && x.Type.ToUpper().Contains(searchText))).ToList();
                }
                //#endregion

                #region AdvSearch
                if (!string.IsNullOrEmpty(ClientLookupId))
                {
                    ClientLookupId = ClientLookupId.ToUpper().Replace("%", "");
                    equipmentList = equipmentList.Where(x => (!string.IsNullOrWhiteSpace(x.ClientLookupId) && x.ClientLookupId.ToUpper().Contains(ClientLookupId))).ToList();
                }
                if (!string.IsNullOrEmpty(Name))
                {
                    Name = Name.ToUpper();
                    equipmentList = equipmentList.Where(x => (!string.IsNullOrWhiteSpace(x.Name) && x.Name.ToUpper().Contains(Name))).ToList();
                }
                if (userData.DatabaseKey.Client.BusinessType.ToUpper() != BusinessTypeConstants.Facilities)
                {
                    if (!string.IsNullOrEmpty(Location))
                    {
                        Location = Location.ToUpper();
                        equipmentList = equipmentList.Where(x => (!string.IsNullOrWhiteSpace(x.Location) && x.Location.ToUpper().Contains(Location))).ToList();
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(Location))
                    {
                        Location = Location.ToUpper();
                        equipmentList = equipmentList.Where(x => (!string.IsNullOrWhiteSpace(x.LocationIdClientLookupId) && x.LocationIdClientLookupId.ToUpper().Contains(Location))).ToList();
                    }
                }
                if (!string.IsNullOrEmpty(SerialNumber))
                {
                    SerialNumber = SerialNumber.ToUpper();
                    equipmentList = equipmentList.Where(x => (!string.IsNullOrWhiteSpace(x.SerialNumber) && x.SerialNumber.ToUpper().Contains(SerialNumber))).ToList();
                }
                if (!string.IsNullOrEmpty(Type))
                {
                    equipmentList = equipmentList.Where(x => (!string.IsNullOrWhiteSpace(x.Type) && x.Type.Equals(Type))).ToList();
                }
                if (!string.IsNullOrEmpty(Make))
                {
                    Make = Make.ToUpper();
                    equipmentList = equipmentList.Where(x => (!string.IsNullOrWhiteSpace(x.Make) && x.Make.ToUpper().Contains(Make))).ToList();
                }
                if (!string.IsNullOrEmpty(Model))
                {
                    Model = Model.ToUpper();
                    equipmentList = equipmentList.Where(x => (!string.IsNullOrWhiteSpace(x.Model) && x.Model.ToUpper().Contains(Model))).ToList();
                }
                if (!string.IsNullOrEmpty(LaborAccountClientLookupId))
                {
                    LaborAccountClientLookupId = LaborAccountClientLookupId.ToUpper();
                    equipmentList = equipmentList.Where(x => (!string.IsNullOrWhiteSpace(x.LaborAccountClientLookupId) && x.LaborAccountClientLookupId.ToUpper().Contains(LaborAccountClientLookupId))).ToList();
                }
                if (!string.IsNullOrEmpty(AssetNumber))
                {
                    AssetNumber = AssetNumber.ToUpper();
                    equipmentList = equipmentList.Where(x => (!string.IsNullOrWhiteSpace(x.AssetNumber) && x.AssetNumber.ToUpper().Contains(AssetNumber))).ToList();
                }

                if (AssetGroup1Id > 0)
                {
                    equipmentList = equipmentList.Where(x => x.AssetGroup1 == AssetGroup1Id).ToList();
                }
                if (AssetGroup2Id > 0)
                {
                    equipmentList = equipmentList.Where(x => x.AssetGroup2 == AssetGroup2Id).ToList();
                }
                if (AssetGroup3Id > 0)
                {
                    equipmentList = equipmentList.Where(x => x.AssetGroup3 == AssetGroup3Id).ToList();
                }

                #endregion
            }
            return equipmentList;
        }


        #endregion

        #region QR Equipment  
        [HttpPost]
        public PartialViewResult EqupmentDetailsQRcode(string[] EquipClientLookups)
        {
            EquipmentCombined objComb = new EquipmentCombined();
            QRCodeModel qRCodeModel = new QRCodeModel();
            List<string> equipmentClientLookUpNames = new List<string>();
            foreach (var e in EquipClientLookups)
            {
                equipmentClientLookUpNames.Add(Convert.ToString(e));
            }
            qRCodeModel.EquipmentIdsList = equipmentClientLookUpNames;
            objComb.qRCodeModel = qRCodeModel;
            LocalizeControls(objComb, LocalizeResourceSetConstants.EquipmentDetails);
            return PartialView("_EquipmentDetailsQRCode", objComb);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SetEquipmentIdlist(EquipmentCombined objComb)
        {
            TempData["QRCodeEquipmentIdList"] = objComb.qRCodeModel.EquipmentIdsList;
            return Json(new { JsonReturnEnum.success });
        }
        [EncryptedActionParameter]
        public ActionResult QRCodeGenerationUsingRotativa(bool SmallLabel)
        {
            var equipmentCombined = new EquipmentCombined();
            var qRCodeModel = new QRCodeModel();

            ViewBag.SmallQR = SmallLabel;
            if (TempData["QRCodeEquipmentIdList"] != null)
            {
                qRCodeModel.EquipmentIdsList = (List<string>)TempData["QRCodeEquipmentIdList"];
            }
            else
            {
                qRCodeModel.EquipmentIdsList = new List<string>();
            }
            equipmentCombined.qRCodeModel = qRCodeModel;

            return new PartialViewAsPdf("_EquipmentQRCodeTemplate", equipmentCombined)
            {
                PageMargins = { Left = 1, Right = 1, Top = 1, Bottom = 1 },
                PageHeight = SmallLabel ? 25 : 28,
                PageWidth = SmallLabel ? 54 : 89,
            };
        }
        #endregion

        #region BulkUpdate
        [HttpPost]
        public PartialViewResult ShowEquipBulkUpdate(string[] EquipmentIds)
        {
            EquipmentWrapper eWrapper = new EquipmentWrapper(userData);
            EquipmentCombined objComb = new EquipmentCombined();
            objComb.EquipModel = new EquipmentModel();

            EquipmentBulkUpdateModel eModel = new EquipmentBulkUpdateModel();
            string str = string.Empty;
            foreach (var v in EquipmentIds)
            {
                str += v + ",";
            }
            if (str != null)
            {
                str = str.Remove(str.Length - 1, 1);
            }
            eModel.EquipmentIdList = str;
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            List<DataContracts.LookupList> TypeList = new List<DataContracts.LookupList>();
            var AllLookUps = commonWrapper.GetAllLookUpList();
            if (AllLookUps != null)
            {
                TypeList = AllLookUps.Where(x => x.ListName == LookupListConstants.Equipment_EquipType).ToList();
            }
            if (TypeList != null)
            {
                eModel.LookupTypeList = TypeList.Select(x => new SelectListItem { Text = x.ListValue + " - " + x.Description, Value = x.ListValue }).ToList();
            }
            //V2-379
            var AcclookUpList = GetAccountByActiveState(true);
            if (AcclookUpList != null)
            {
                eModel.AccountList = AcclookUpList.Select(x => new SelectListItem { Text = x.Account + " - " + x.Name, Value = x.Account }).ToList();
            }
            var ast1 = eWrapper.GetAssetGroup1Dropdowndata();
            if (ast1 != null)
            {
                objComb.EquipModel.AssetGroup1List = ast1.Select(x => new SelectListItem { Text = x.AssetGroup1DescWithClientLookupId, Value = x.AssetGroup1Id.ToString() });
            }
            var ast2 = eWrapper.GetAssetGroup2Dropdowndata();
            if (ast2 != null)
            {
                objComb.EquipModel.AssetGroup2List = ast2.Select(x => new SelectListItem { Text = x.AssetGroup2DescWithClientLookupId, Value = x.AssetGroup2Id.ToString() });
            }
            var ast3 = eWrapper.GetAssetGroup3Dropdowndata();
            if (ast3 != null)
            {
                objComb.EquipModel.AssetGroup3List = ast3.Select(x => new SelectListItem { Text = x.AssetGroup3DescWithClientLookupId, Value = x.AssetGroup3Id.ToString() });
            }
            this.GetAssetGroupHeaderName(objComb);
            objComb.equipmentBulkUpdateModel = eModel;
            LocalizeControls(objComb, LocalizeResourceSetConstants.EquipmentDetails);
            return PartialView("~/Views/Equipment/_EquipmentBulkUpdate.cshtml", objComb);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult EquipmentBulkUpdate(EquipmentCombined objComb)
        {
            string ModelValidationFailedMessage = string.Empty;
            int updatedItemCount = 0;
            if (ModelState.IsValid)
            {
                EquipmentWrapper eWrapper = new EquipmentWrapper(userData);
                var objEqp = eWrapper.EqpBulkUpload(objComb.equipmentBulkUpdateModel);

                if (objEqp != null && objEqp.Count > 0)
                {
                    return Json(objEqp, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    if (!String.IsNullOrEmpty(objComb.equipmentBulkUpdateModel.EquipmentIdList))
                    {
                        updatedItemCount = objComb.equipmentBulkUpdateModel.EquipmentIdList.Split(',').ToList().Count();
                    }

                    return Json(new { Result = JsonReturnEnum.success.ToString(), UpdatedItemCount = updatedItemCount }, JsonRequestBehavior.AllowGet);
                }
            }
            ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
            return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
        }
        #endregion BulkUpdate

        #region Retrieve Department
        public List<Department> FetchActiveDept()
        {
            List<DataContracts.Department> DeptListCustom = new List<DataContracts.Department>();
            Department DeptCustom = new Department();
            DeptCustom.ClientId = userData.DatabaseKey.Client.ClientId;
            DeptCustom.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            DeptCustom.InactiveFlag = false;
            DeptListCustom = DeptCustom.RetrieveByInActiveFlag(userData.DatabaseKey);
            return DeptListCustom;
        }
        public List<Department> FetchDept()
        {
            List<DataContracts.Department> DeptListCustom = new List<DataContracts.Department>();
            Department DeptCustom = new Department();
            DeptCustom.ClientId = userData.DatabaseKey.Client.ClientId;
            DeptCustom.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            DeptListCustom = DeptCustom.RetrieveDepartmentByClientIdSiteId(userData.DatabaseKey);
            return DeptListCustom;
        }
        #endregion

        #region Add Edit Dynamic
        public ActionResult AddDynamic()
        {
            TempData["Mode"] = "adddynamic";
            return Redirect("/Equipment/Index?page=Maintenance_Assets");
        }
        [HttpPost]
        public ActionResult AddEquipmentDynamicView()
        {
            Task taskAllLookUp, taskAssetGroup1LookUp, taskAssetGroup2LookUp, taskAssetGroup3LookUp;
            EquipmentWrapper eWrapper = new EquipmentWrapper(userData);
            EquipmentCombined objComb = new EquipmentCombined();
            objComb.EditEquipment = new EditEquipmentModelDynamic();
            objComb.PlantLocation = userData.Site.PlantLocation;

            CommonWrapper commonWrapper = new CommonWrapper(userData);
            List<DataContracts.LookupList> TypeList = new List<DataContracts.LookupList>();
            List<DataContracts.LookupList> AllLookUps = new List<DataContracts.LookupList>();

            #region task          

            taskAllLookUp = Task.Factory.StartNew(() => AllLookUps = commonWrapper.GetAllLookUpList());
            List<AssetGroup1Model> astGroup1 = new List<AssetGroup1Model>();

            taskAssetGroup1LookUp = Task.Factory.StartNew(() => astGroup1 = eWrapper.GetAssetGroup1Dropdowndata());

            List<AssetGroup2Model> astGroup2 = new List<AssetGroup2Model>();
            taskAssetGroup2LookUp = Task.Factory.StartNew(() => astGroup2 = eWrapper.GetAssetGroup2Dropdowndata());

            List<AssetGroup3Model> astGroup3 = new List<AssetGroup3Model>();
            taskAssetGroup3LookUp = Task.Factory.StartNew(() => astGroup3 = eWrapper.GetAssetGroup3Dropdowndata());

            Task.WaitAll(taskAllLookUp, taskAssetGroup1LookUp, taskAssetGroup2LookUp, taskAssetGroup3LookUp);
            #endregion

            objComb.BusinessType = userData.DatabaseKey.Client.BusinessType;
            var AssetCategoryList = UtilityFunction.AssetCategoryList();
            if (AssetCategoryList != null)
            {
                objComb.AssetCategoryList = AssetCategoryList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
            }


            if (astGroup1 != null)
            {
                objComb.AssetGroup1List = astGroup1.Select(x => new SelectListItem { Text = x.AssetGroup1DescWithClientLookupId, Value = x.AssetGroup1Id.ToString() });
            }
            if (astGroup2 != null)
            {
                objComb.AssetGroup2List = astGroup2.Select(x => new SelectListItem { Text = x.AssetGroup2DescWithClientLookupId, Value = x.AssetGroup2Id.ToString() });
            }
            if (astGroup3 != null)
            {
                objComb.AssetGroup3List = astGroup3.Select(x => new SelectListItem { Text = x.AssetGroup3DescWithClientLookupId, Value = x.AssetGroup3Id.ToString() });
            }


            objComb.UIConfigurationDetails = new RetrieveDataForUIConfiguration().Retrieve(DataDictionaryViewNameConstant.AddAsset, userData);
            IList<string> LookupNames = objComb.UIConfigurationDetails.ToList()
                                            .Where(x => x.LookupType == "LookupList" && !string.IsNullOrEmpty(x.LookupName))
                                            .Select(s => s.LookupName)
                                            .ToList();
            if (AllLookUps != null)
            {
                objComb.AllRequiredLookUplist = AllLookUps.Where(x => LookupNames.Contains(x.ListName))
                                                          .Select(s => new UILookupList
                                                          { text = s.ListValue + " - " + s.Description, value = s.ListValue, lookupname = s.ListName })
                                                          .ToList();
            }

            objComb._userdata = userData;
            objComb.AssetGroup1Label = userData.Site.AssetGroup1Name;
            objComb.AssetGroup2Label = userData.Site.AssetGroup2Name;
            objComb.AssetGroup3Label = userData.Site.AssetGroup3Name;

            LocalizeControls(objComb, LocalizeResourceSetConstants.EquipmentDetails);
            return PartialView("EquipmentAddDynamic", objComb);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEquipmentDynamic(EquipmentCombined objEM, string Command)
        {
            EquipmentWrapper eWrapper = new EquipmentWrapper(userData);
            string ModelValidationFailedMessage = string.Empty;

            if (ModelState.IsValid)
            {
                Equipment equipment = new Equipment();
                equipment = eWrapper.AddEquipmentDynamic(objEM);
                if (equipment.ErrorMessages != null && equipment.ErrorMessages.Count > 0)
                {
                    return Json(equipment.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), Command = Command, EquipmentId = equipment.EquipmentId }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult EditEquipmentDynamic(long EquipmentId, string ClientLookupId, string Name, bool isRemoveFromService, string Status)
        {
            Task taskAllLookUp, taskAssetGroup1LookUp, taskAssetGroup2LookUp, taskAssetGroup3LookUp, taskAssetDetailsById;
            EquipmentWrapper eWrapper = new EquipmentWrapper(userData);
            EquipmentCombined objComb = new EquipmentCombined();
            objComb.EditEquipment = new EditEquipmentModelDynamic();
            objComb.PlantLocation = userData.Site.PlantLocation;

            CommonWrapper commonWrapper = new CommonWrapper(userData);
            List<DataContracts.LookupList> TypeList = new List<DataContracts.LookupList>();
            List<DataContracts.LookupList> AllLookUps = new List<DataContracts.LookupList>();

            #region task
            taskAssetDetailsById = Task.Factory.StartNew(() => objComb.EditEquipment = eWrapper.RetrieveEquipmentDetailsByEquipmentId(EquipmentId));

            taskAllLookUp = Task.Factory.StartNew(() => AllLookUps = commonWrapper.GetAllLookUpList());
            List<AssetGroup1Model> astGroup1 = new List<AssetGroup1Model>();

            taskAssetGroup1LookUp = Task.Factory.StartNew(() => astGroup1 = eWrapper.GetAssetGroup1Dropdowndata());

            List<AssetGroup2Model> astGroup2 = new List<AssetGroup2Model>();
            taskAssetGroup2LookUp = Task.Factory.StartNew(() => astGroup2 = eWrapper.GetAssetGroup2Dropdowndata());

            List<AssetGroup3Model> astGroup3 = new List<AssetGroup3Model>();
            taskAssetGroup3LookUp = Task.Factory.StartNew(() => astGroup3 = eWrapper.GetAssetGroup3Dropdowndata());

            Task.WaitAll(taskAssetDetailsById, taskAllLookUp, taskAssetGroup1LookUp, taskAssetGroup2LookUp, taskAssetGroup3LookUp);
            #endregion

            objComb.BusinessType = userData.DatabaseKey.Client.BusinessType;
            var AssetCategoryList = UtilityFunction.AssetCategoryList();
            if (AssetCategoryList != null)
            {
                objComb.AssetCategoryList = AssetCategoryList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
            }


            if (astGroup1 != null)
            {
                objComb.AssetGroup1List = astGroup1.Select(x => new SelectListItem { Text = x.AssetGroup1DescWithClientLookupId, Value = x.AssetGroup1Id.ToString() });
            }
            if (astGroup2 != null)
            {
                objComb.AssetGroup2List = astGroup2.Select(x => new SelectListItem { Text = x.AssetGroup2DescWithClientLookupId, Value = x.AssetGroup2Id.ToString() });
            }
            if (astGroup3 != null)
            {
                objComb.AssetGroup3List = astGroup3.Select(x => new SelectListItem { Text = x.AssetGroup3DescWithClientLookupId, Value = x.AssetGroup3Id.ToString() });
            }


            objComb.UIConfigurationDetails = new RetrieveDataForUIConfiguration()
                                            .Retrieve(DataDictionaryViewNameConstant.EditAsset, userData);
            IList<string> LookupNames = objComb.UIConfigurationDetails.ToList()
                                            .Where(x => x.LookupType == "LookupList" && !string.IsNullOrEmpty(x.LookupName))
                                            .Select(s => s.LookupName)
                                            .ToList();
            if (AllLookUps != null)
            {
                objComb.AllRequiredLookUplist = AllLookUps.Where(x => LookupNames.Contains(x.ListName))
                                                          .Select(s => new UILookupList
                                                          { text = s.ListValue + " - " + s.Description, value = s.ListValue, lookupname = s.ListName })
                                                          .ToList();
            }

            objComb._userdata = userData;
            objComb._EquipmentSummaryModel = GetEquipmentSummary(EquipmentId, ClientLookupId, Name, isRemoveFromService, Status);
            objComb.AssetGroup1Label = userData.Site.AssetGroup1Name;
            objComb.AssetGroup2Label = userData.Site.AssetGroup2Name;
            objComb.AssetGroup3Label = userData.Site.AssetGroup3Name;

            LocalizeControls(objComb, LocalizeResourceSetConstants.EquipmentDetails);
            return PartialView("EquipmentEditDynamic", objComb);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateEquipmentDynamicInfo(EquipmentCombined equip)
        {
            EquipmentWrapper eWrapper = new EquipmentWrapper(userData);
            Equipment equipment = new Equipment();

            if (ModelState.IsValid)
            {
                equipment = eWrapper.EditEquipmentDynamic(equip);
                if (equipment.ErrorMessages != null && equipment.ErrorMessages.Count > 0)
                {
                    return Json(equipment.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), equipmentid = equipment.EquipmentId }, JsonRequestBehavior.AllowGet);
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

        #region Asset Availability Remove
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RemoveAssetAvailability(EquipmentCombined equip)
        {
            EquipmentWrapper EqWrapper = new EquipmentWrapper(userData);
            Equipment equipment = new Equipment();
            if (ModelState.IsValid)
            {
                equipment = EqWrapper.RemoveAssetAvailability(equip);
                if (equipment.ErrorMessages != null && equipment.ErrorMessages.Count > 0)
                {
                    return Json(equipment.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), EquipmentId = equipment.EquipmentId }, JsonRequestBehavior.AllowGet);
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

        #region Return To Service
        [HttpPost]
        public ActionResult ReturnToservice(long EquipmentId)
        {
            EquipmentWrapper eqWrapper = new EquipmentWrapper(userData);
            Equipment equipment = new Equipment();
            equipment = eqWrapper.ReturnToservice(EquipmentId);
            if (equipment.ErrorMessages != null && equipment.ErrorMessages.Count > 0)
            {
                return Json(equipment.ErrorMessages, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Result = JsonReturnEnum.success.ToString(), EquipmentId = equipment.EquipmentId }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Asset Availability Update
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateAssetAvailability(EquipmentCombined equip)
        {
            EquipmentWrapper EqWrapper = new EquipmentWrapper(userData);
            Equipment equipment = new Equipment();
            if (ModelState.IsValid)
            {
                equipment = EqWrapper.UpdateAssetAvailability(equip);
                if (equipment.ErrorMessages != null && equipment.ErrorMessages.Count > 0)
                {
                    return Json(equipment.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), EquipmentId = equipment.EquipmentId }, JsonRequestBehavior.AllowGet);
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

        #region scrap equipment
        [HttpPost]
        public JsonResult ValidateEquipmentStatusScrapped(long _eqid, bool inactiveFlag, string clientLookupId)
        {
            EquipmentWrapper eWrapper = new EquipmentWrapper(userData);
            Equipment equipment = new Equipment();
            string flag = ActivationStatusConstant.InActivate;
            if (inactiveFlag)
            {
                flag = ActivationStatusConstant.Activate;
            }
            equipment = eWrapper.ValidateEquipmentScrapped(_eqid, flag, clientLookupId);
            if (equipment.ErrorMessages != null && equipment.ErrorMessages.Count > 0)
            {
                return Json(equipment.ErrorMessages, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { validationStatus = true, equipmentid = equipment.EquipmentId }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult UpdateEquipmentStatustoScrap(long _eqid, bool inactiveFlag)
        {
            Equipment equipment = new Equipment();
            EquipmentWrapper eWrapper = new EquipmentWrapper(userData);
            var errMsg = eWrapper.UpdateEquipmentStatustoScrap(_eqid, inactiveFlag);
            if (errMsg != null && errMsg.Count > 0)
            {
                return Json(equipment.ErrorMessages, JsonRequestBehavior.AllowGet);
            }
            else
            {
                if (errMsg != null && errMsg.Count > 0)
                {
                    return Json(equipment.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { result = JsonReturnEnum.success.ToString(), equipmentid = equipment.EquipmentId }, JsonRequestBehavior.AllowGet);
                }
            }
        }

        #endregion

        #region for Return Scrap Asset
        [HttpPost]
        public JsonResult UpdateAssetForReturnScrap(long _eqid, bool inactiveFlag)
        {
            Equipment equipment = new Equipment();
            EquipmentWrapper eWrapper = new EquipmentWrapper(userData);
            var errMsg = eWrapper.UpdateAssetStatusForReturnAsset(_eqid);
            if (errMsg != null && errMsg.Count > 0)
            {
                return Json(equipment.ErrorMessages, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { result = JsonReturnEnum.success.ToString(), equipmentid = equipment.EquipmentId }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Repairable Spare Asset
        [HttpPost]
        public PartialViewResult RepairableSpareAssetWizard(long EquipmentId)
        {
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            EquipmentCombined equipmentCombined = new EquipmentCombined();
            var repairableSpareModel = new RepairableSpareModel();
            List<DataContracts.LookupList> AllLookUps = new List<DataContracts.LookupList>();
            AllLookUps = commonWrapper.GetAllLookUpList();
            if (AllLookUps != null)
            {
                var TypeList = AllLookUps.Where(x => x.ListName == LookupListConstants.Equipment_EquipType).ToList();
                if (TypeList != null)
                {
                    equipmentCombined.LookupTypeList = TypeList.Select(x => new SelectListItem { Text = x.ListValue + " - " + x.Description, Value = x.ListValue }).ToList();
                }
            }
            var statusList = UtilityFunction.RepairableSpareAssignmentStatusList();
            equipmentCombined.RepairableSpareStatusList = statusList.Select(x => new SelectListItem { Text = x.text, Value = x.value }).ToList();
            repairableSpareModel.DetailsEquipmentId = EquipmentId;
            repairableSpareModel.ValidationMode = "Add";
            equipmentCombined.RepairableSpareModel = repairableSpareModel;
            LocalizeControls(equipmentCombined, LocalizeResourceSetConstants.EquipmentDetails);
            return PartialView("~/Views/Equipment/RepairableSpare/_RepairableSpareAssetWizard.cshtml", equipmentCombined);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddRepairableSpareAsset(EquipmentCombined equip)
        {
            EquipmentWrapper EqWrapper = new EquipmentWrapper(userData);
            if (ModelState.IsValid)
            {
                Equipment equipment = EqWrapper.AddRepairableSpareAsset(equip);
                if (equipment.ErrorMessages != null && equipment.ErrorMessages.Count > 0)
                {
                    return Json(equipment.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), EquipmentId = equipment.EquipmentId }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global),
                    JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult EditRepairableSpareAsset(long EquipmentId)
        {
            Task[] tasks = new Task[2];
            EquipmentWrapper eWrapper = new EquipmentWrapper(userData);
            EquipmentCombined objComb = new EquipmentCombined();

            CommonWrapper commonWrapper = new CommonWrapper(userData);
            List<DataContracts.LookupList> TypeList = new List<DataContracts.LookupList>();
            List<DataContracts.LookupList> AllLookUps = new List<DataContracts.LookupList>();

            #region task
            tasks[0] = Task.Factory.StartNew(() => objComb.EquipData = eWrapper.GetEquipmentDetailsById(EquipmentId));

            tasks[1] = Task.Factory.StartNew(() => AllLookUps = commonWrapper.GetAllLookUpList());
            Task.WaitAll(tasks);
            #endregion

            if (AllLookUps != null)
            {
                TypeList = AllLookUps.Where(x => x.ListName == LookupListConstants.Equipment_EquipType).ToList();
                if (TypeList != null)
                {
                    objComb.LookupTypeList = TypeList.Select(x => new SelectListItem { Text = x.ListValue + " - " + x.Description, Value = x.ListValue }).ToList();
                }
            }

            objComb._userdata = userData;
            objComb._EquipmentSummaryModel = GetEquipmentSummary(EquipmentId, objComb.EquipData.ClientLookupId, objComb.EquipData.Name,
                objComb.EquipData.RemoveFromService, objComb.EquipData.Status);

            LocalizeControls(objComb, LocalizeResourceSetConstants.EquipmentDetails);
            objComb.RepairableSpareModel = MapRepairableSpare(objComb);//RN
            return PartialView("~/Views/Equipment/RepairableSpare/RepariableSpareAssetEdit.cshtml", objComb);
        }
        private RepairableSpareModel MapRepairableSpare(EquipmentCombined equipData)
        {
            RepairableSpareModel objRepairableSpare = new RepairableSpareModel();
            objRepairableSpare.EquipmentId = equipData.EquipData.EquipmentId;
            objRepairableSpare.ClientLookupId = equipData.EquipData.ClientLookupId;
            objRepairableSpare.Name = equipData.EquipData.Name;
            objRepairableSpare.SerialNumber = equipData.EquipData.SerialNumber;
            objRepairableSpare.Type = equipData.EquipData.Type;
            objRepairableSpare.Make = equipData.EquipData.Make;
            objRepairableSpare.Model = equipData.EquipData.Model;
            objRepairableSpare.Material_AccountId = equipData.EquipData.Material_AccountId;
            objRepairableSpare.MaterialAccountClientLookupId = equipData.EquipData.MaterialAccountClientLookupId;
            objRepairableSpare.AssetNumber = equipData.EquipData.AssetNumber;
            objRepairableSpare.Maint_WarrantyExpire = equipData.EquipData.Maint_WarrantyExpire;
            objRepairableSpare.Maint_VendorId = equipData.EquipData.Maint_VendorId;
            objRepairableSpare.MaintVendorIdClientLookupId = equipData.EquipData.MaintVendorIdClientLookupId;
            objRepairableSpare.MaintVendorIdClientLookupIdWithName = equipData.EquipData.MaintVendorIdClientLookupId + "-" + equipData.EquipData.MaintVendorName; // V2-1211
            objRepairableSpare.Maint_WarrantyDesc = equipData.EquipData.Maint_WarrantyDesc;
            objRepairableSpare.AcquiredCost = equipData.EquipData.AcquiredCost;
            objRepairableSpare.AcquiredDate = equipData.EquipData.AcquiredDate;
            objRepairableSpare.CatalogNumber = equipData.EquipData.CatalogNumber;
            objRepairableSpare.CostCenter = equipData.EquipData.CostCenter;
            objRepairableSpare.CriticalFlag = equipData.EquipData.CriticalFlag;
            objRepairableSpare.InstallDate = equipData.EquipData.InstallDate;
            objRepairableSpare.Location = equipData.EquipData.Location;
            objRepairableSpare.AssignedAssetClientlookupId = equipData.EquipData.AssignedClientlookupid;
            objRepairableSpare.AssignedAssetId = equipData.EquipData.Assigned;
            objRepairableSpare.RepairableSpareStatus = equipData.EquipData.Status;

            return objRepairableSpare;
        }
        private EquipmentCombined RepairableSpareEquipmentDetails(long EquipmentId, Equipment EquipmentDetails)
        {
            Task[] tasks = new Task[2];
            EquipmentCombined objComb = new EquipmentCombined();
            ChangeEquipmentIDModel _ChangeEquipmentIDModel = new ChangeEquipmentIDModel();
            EquipmentWrapper eWrapper = new EquipmentWrapper(userData);
            EquipmentModel equipmentModel = new EquipmentModel();
            AssetAvailabilityRemoveModel assetAvailabilityRemoveModel = new AssetAvailabilityRemoveModel();
            AssetAvailabilityUpdateModel assetAvailabilityUpdateModel = new AssetAvailabilityUpdateModel();
            CommonWrapper objCommonWrapper = new CommonWrapper(userData);

            objComb.attachmentCount = objCommonWrapper.AttachmentCount(EquipmentId, AttachmentTableConstant.Equipment, userData.Security.Equipment.Edit);
            objComb.EquipData = EquipmentDetails;
            objComb._EquipmentSummaryModel = GetEquipmentSummary(EquipmentId, objComb.EquipData.ClientLookupId, objComb.EquipData.Name, objComb.EquipData.RemoveFromService, objComb.EquipData.Status);

            var equipmentDownTimeList = UtilityFunction.EquipmentDownTimeDatesList();
            if (equipmentDownTimeList != null)
            {
                objComb.equipmentDownTimeDateList = equipmentDownTimeList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
            }
            var workOrderTypeList = UtilityFunction.WorkOrderDatesList();
            if (workOrderTypeList != null)
            {
                objComb.workOrderTypeDateList = workOrderTypeList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
            }
            string temp = string.Empty;
            temp = EquipmentDetails.ClientLookupId + "][" + EquipmentDetails.Name + "][" + EquipmentDetails.SerialNumber + "][" + EquipmentDetails.Make + "][" + EquipmentDetails.Model;
            QRCodeModel qRCodeModel = new QRCodeModel();
            List<string> equipmentClientLookUpNames = new List<string>();
            equipmentClientLookUpNames.Add(temp);
            qRCodeModel.EquipmentIdsList = equipmentClientLookUpNames;
            objComb.qRCodeModel = qRCodeModel;
            equipmentModel.AlertFollowedEquipment = eWrapper.AlertFollowIdExist(EquipmentId);
            equipmentModel.BusinessType = userData.DatabaseKey.Client.BusinessType;

            _ChangeEquipmentIDModel.EquipmentId = Convert.ToInt64(objComb.EquipData.EquipmentId);
            _ChangeEquipmentIDModel.ClientLookupId = objComb.EquipData.ClientLookupId;
            _ChangeEquipmentIDModel.OldClientLookupId = objComb.EquipData.ClientLookupId;
            _ChangeEquipmentIDModel.UpdateIndex = objComb.EquipData.UpdateIndex;
            objComb.EquipModel = equipmentModel;
            objComb._ChangeEquipmentIDModel = _ChangeEquipmentIDModel;
            objComb._CreatedLastUpdatedModel = eWrapper.createdLastUpdatedModel(EquipmentId);
            objComb.security = userData.Security;
            objComb._userdata = userData;

            #region Asset availability
            var removefromServiceReasonCode = objCommonWrapper.GetListFromConstVals(LookupListConstants.RemoveFromServiceReasonCode);
            if (removefromServiceReasonCode != null)
            {
                objComb.LookupRemoveFromServiceReasonCode = removefromServiceReasonCode.Select(x => new SelectListItem { Text = x.text.ToString(), Value = x.value.ToString() });
                if (objComb.EquipData.RemoveFromServiceReasonCode != "")
                {
                    objComb.ServiceReasonCode = AssetStatusConstant.Scrapped;
                    if (objComb.EquipData.RemoveFromServiceReasonCode != AssetStatusConstant.Scrapped)
                    {
                        objComb.ServiceReasonCode = objComb.LookupRemoveFromServiceReasonCode.Where(x => x.Value == objComb.EquipData.RemoveFromServiceReasonCode).Select(x => x.Text).FirstOrDefault().ToString();
                    }
                }
            }

            assetAvailabilityRemoveModel.EquipmentId = EquipmentId;
            assetAvailabilityRemoveModel.RemoveFromService = objComb.EquipData.RemoveFromService;
            objComb._AssetAvailabilityRemoveModel = assetAvailabilityRemoveModel;
            assetAvailabilityUpdateModel.EquipmentId = EquipmentId;
            objComb._AssetAvailabilityUpdateModel = assetAvailabilityUpdateModel;
            objComb.IsAssetAvailability = userData.Security.Asset_Availability.Access;
            objComb.RemoveServiceDate = Convert.ToDateTime(objComb.EquipData.RemoveFromServiceDate).ToUserTimeZone(userData.Site.TimeZone);
            #endregion


            LocalizeControls(objComb, LocalizeResourceSetConstants.EquipmentDetails);
            return objComb;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateRepariableSpareAssetEdit(EquipmentCombined equipmentCombined)
        {
            string emptyValue = string.Empty;
            EquipmentWrapper eWrapper = new EquipmentWrapper(userData);
            Equipment equipment = new Equipment();

            if (ModelState.IsValid)
            {
                equipment = eWrapper.UpdateRepairableSpareAsset(equipmentCombined.RepairableSpareModel);
                if (equipment.ErrorMessages != null && equipment.ErrorMessages.Count > 0)
                {
                    return Json(equipment.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), EquipmentId = equipment.EquipmentId }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                string ModelValidationFailedMessage = string.Empty;
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }


        #region Assignment
        [HttpPost]
        public ActionResult UpdateAssignment(EquipmentCombined equip)
        {
            EquipmentWrapper EqWrapper = new EquipmentWrapper(userData);
            if (ModelState.IsValid)
            {
                Equipment equipment = EqWrapper.UpdateAssignmentSpareAsset(equip);
                if (equipment.ErrorMessages != null && equipment.ErrorMessages.Count > 0)
                {
                    return Json(equipment.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), EquipmentId = equipment.EquipmentId }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global),
                    JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region V2-637 AssignmentViewLog
        public JsonResult AssignmentViewLogLookupList(int? draw, int? start, int? length, long ObjectId, string TransactionDate, string Status, string PersonnelId = "", string Assigned = "", string Location = "", string ParentId = "", string AssetGroup1 = "", string AssetGroup2 = "", string AssetGroup3 = "")
        {
            List<AssignmentViewLogLookUpModel> modelList = new List<AssignmentViewLogLookUpModel>();

            EquipmentWrapper eWrapper = new EquipmentWrapper(userData);
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];

            start = start.HasValue
              ? start / length
              : 0;
            int skip = start * length ?? 0;

            TransactionDate = !string.IsNullOrEmpty(TransactionDate) ? TransactionDate.Trim() : string.Empty;
            modelList = eWrapper.PopulateAssignmentViewLogList(start.Value, length.Value, order, orderDir, ObjectId, TransactionDate, Status, PersonnelId, Assigned, Location, ParentId, AssetGroup1, AssetGroup2, AssetGroup3);

            long totalRecords = 0;
            long recordsFiltered = 0;
            if (modelList != null && modelList.Count > 0)
            {

                totalRecords = modelList.Count;
                recordsFiltered = modelList.Count;
            }
            var jsonResult = Json(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = modelList }, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        #endregion
        #endregion

        #region V2-716 Multiple Images
        public PartialViewResult GetImages(int currentpage, int? start, int? length, long EquipmentId)
        {
            EquipmentCombined equipment = new EquipmentCombined();
            List<ImageAttachmentModel> imgDatalist = new List<ImageAttachmentModel>();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            List<Attachment> Attachments = new List<Attachment>();
            equipment.security = this.userData.Security;
            equipment._userdata = this.userData;
            start = start.HasValue
            ? start / length
            : 0;
            int skip = start * length ?? 0;
            ViewBag.Start = skip;

            if (userData.DatabaseKey.Client.OnPremise)
            {
                Attachments = commonWrapper.GetOnPremiseMultipleImageUrl(skip, length ?? 0, EquipmentId, AttachmentTableConstant.Equipment);
            }
            else
            {
                Attachments = commonWrapper.GetAzureMultipleImageUrl(skip, length ?? 0, EquipmentId, AttachmentTableConstant.Equipment);
            }
            foreach (var attachment in Attachments)
            {
                ImageAttachmentModel imgdata = new ImageAttachmentModel();
                imgdata.AttachmentId = attachment.AttachmentId;
                imgdata.ObjectId = attachment.ObjectId;
                imgdata.Profile = attachment.Profile;
                imgdata.FileName = attachment.FileName;
                string SASImageURL = string.Empty;
                if (!string.IsNullOrEmpty(attachment.AttachmentURL))
                {
                    SASImageURL = attachment.AttachmentURL;
                }
                else
                {
                    commonWrapper.GetNoImageUrl();
                }
                imgdata.AttachmentURL = SASImageURL;
                imgdata.ObjectName = attachment.ObjectName;
                imgdata.TotalCount = attachment.TotalCount;
                imgDatalist.Add(imgdata);
            }
            var recordsFiltered = 0;
            recordsFiltered = imgDatalist.Select(o => o.TotalCount).FirstOrDefault();
            ViewBag.TotalRecords = imgDatalist.Select(o => o.TotalCount).FirstOrDefault();
            ViewBag.Length = length;
            ViewBag.CurrentPage = currentpage;
            equipment.imageAttachmentModels = imgDatalist;
            LocalizeControls(equipment, LocalizeResourceSetConstants.Global);

            return PartialView("~/Views/Equipment/_AllImages.cshtml", equipment);
        }
        #endregion

        #region Mobile Card View and details V2-835
        [HttpPost]
        public PartialViewResult GetequipmentGridData_Mobile(int currentpage = 0, int? start = 0, int? length = 0, int CustomQueryDisplayId = 0, string ClientLookupId = "", string Name = "", string Location = "", string SerialNumber = "", string Type = "", string Make = "", string Model = "", string LaborAccountClientLookupId = "", string AssetNumber = "", int AssetGroup1Id = 0, int AssetGroup2Id = 0, int AssetGroup3Id = 0, string AssetAvailability = "",
            string txtSearchval = "", string Order = "1", string orderDir = "asc")
        {
            EquipmentWrapper eWrapper = new EquipmentWrapper(userData);
            EquipmentCombined objComb = new EquipmentCombined();

            txtSearchval = txtSearchval.Replace("%", "[%]");
            ClientLookupId = ClientLookupId.Replace("%", "[%]");
            Name = Name.Replace("%", "[%]");
            Location = Location.Replace("%", "[%]");
            SerialNumber = SerialNumber.Replace("%", "[%]");
            Make = Make.Replace("%", "[%]");
            Model = Model.Replace("%", "[%]");
            AssetNumber = AssetNumber.Replace("%", "[%]");
            SerialNumber = SerialNumber.Replace("%", "[%]");

            LaborAccountClientLookupId = LaborAccountClientLookupId.Replace("%", "[%]");
            objComb._userdata = this.userData;

            EquipmentSearchModel eSearchModel;
            List<EquipmentSearchModel> eSearchModelList = new List<EquipmentSearchModel>();

            start = start.HasValue
               ? start / length
               : 0;
            int skip = start * length ?? 0;

            List<string> typeList = new List<string>();
            var equipmentList = eWrapper.GetEquipmentGridDataMobile(CustomQueryDisplayId, Order, orderDir, skip, length ?? 0, ClientLookupId, Name, Location, SerialNumber, Type, Make, Model, LaborAccountClientLookupId, AssetNumber, txtSearchval, AssetGroup1Id, AssetGroup2Id, AssetGroup3Id, AssetAvailability);

            var totalRecords = 0;
            var recordsFiltered = 0;

            if (equipmentList != null && equipmentList.Count > 0)
            {
                recordsFiltered = equipmentList[0].TotalCount;
                totalRecords = equipmentList[0].TotalCount;
            }
            else
            {
                recordsFiltered = 0;
                totalRecords = 0;
            }
            int initialPage = start.Value;

            var filteredResult = equipmentList
              .ToList();
            ViewBag.Start = skip;
            ViewBag.TotalRecords = totalRecords;
            ViewBag.Length = length;
            ViewBag.CurrentPage = currentpage;
            foreach (var item in filteredResult)
            {
                eSearchModel = new EquipmentSearchModel();
                eSearchModel.EquipmentId = item.EquipmentId;
                eSearchModel.ClientLookupId = item.ClientLookupId;
                eSearchModel.Name = item.Name;
                // RKL - 2020-12-11 - Not using the Location table anymore
                //eSearchModel.Location = (userData.DatabaseKey.Client.BusinessType.ToUpper() == BusinessTypeConstants.Facilities) ? item.LocationIdClientLookupId : item.Location;
                eSearchModel.Location = item.Location;
                eSearchModel.SerialNumber = item.SerialNumber;
                eSearchModel.Type = item.Type;
                eSearchModel.Make = item.Make;
                eSearchModel.Model = item.Model;
                eSearchModel.LaborAccountClientLookupId = item.LaborAccountClientLookupId;
                eSearchModel.AssetNumber = item.AssetNumber;
                eSearchModel.AssetGroup1ClientLookupId = Convert.ToString(item.AssetGroup1ClientLookupId);
                eSearchModel.AssetGroup2ClientLookupId = Convert.ToString(item.AssetGroup2ClientLookupId);
                eSearchModel.AssetGroup3ClientLookupId = Convert.ToString(item.AssetGroup3ClientLookupId);
                //V2-636 
                eSearchModel.RemoveFromService = item.RemoveFromService;
                if (item.RemoveFromServiceDate != null && item.RemoveFromServiceDate == default(DateTime))
                {
                    eSearchModel.RemoveFromServiceDate = null;
                }
                else
                {
                    eSearchModel.RemoveFromServiceDate = item.RemoveFromServiceDate;
                }
                eSearchModel.BusinessType = userData.DatabaseKey.Client.BusinessType;
                eSearchModelList.Add(eSearchModel);
            }
            var ClientOnPremise = userData.DatabaseKey.Client.OnPremise;
            CommonWrapper comWrapper = new CommonWrapper(userData);
            Parallel.ForEach(eSearchModelList, item =>
                {
                    if (ClientOnPremise)
                    {
                        var imageURL = comWrapper.GetOnPremiseImageUrl(item.EquipmentId, AttachmentTableConstant.Equipment);
                        item.imageURL = UtilityFunction.PhotoBase64ImgSrc(imageURL);
                    }
                    else
                    {
                        item.imageURL = comWrapper.GetAzureImageUrl(item.EquipmentId, AttachmentTableConstant.Equipment);
                    }
                });

            objComb.EquipmentCardList = eSearchModelList;
            EquipmentModel datas = new EquipmentModel();
            objComb.EquipModel = datas;
            this.GetAssetGroupHeaderName(objComb);
            LocalizeControls(objComb, LocalizeResourceSetConstants.EquipmentDetails);
            return PartialView("~/Views/Equipment/Mobile/_EquipmentGridCardView.cshtml", objComb);
        }
        public PartialViewResult EquipmentDetails_Mobile(long EquipmentId)
        {
            EquipmentWrapper eWrapper = new EquipmentWrapper(userData);
            var EquipmentDetails = eWrapper.GetEquipmentDetailsById(EquipmentId);

            Task[] tasks = new Task[1];

            EquipmentCombined objComb = new EquipmentCombined();
            ChangeEquipmentIDModel _ChangeEquipmentIDModel = new ChangeEquipmentIDModel();

            EquipmentModel equipmentModel = new EquipmentModel();
            AssetAvailabilityRemoveModel assetAvailabilityRemoveModel = new AssetAvailabilityRemoveModel();
            AssetAvailabilityUpdateModel assetAvailabilityUpdateModel = new AssetAvailabilityUpdateModel();

            CommonWrapper objCommonWrapper = new CommonWrapper(userData);
            tasks[0] = Task.Factory.StartNew(() => objComb.attachmentCount = objCommonWrapper.AttachmentCount(EquipmentId, AttachmentTableConstant.Equipment, userData.Security.Equipment.Edit));

            Task.WaitAll(tasks);
            objComb.EquipData = EquipmentDetails;
            if (objComb.EquipData.Maint_WarrantyExpire != null && objComb.EquipData.Maint_WarrantyExpire.Value == default(DateTime))
            {
                objComb.EquipData.Maint_WarrantyExpire = null;
            }
            objComb._EquipmentSummaryModel = GetEquipmentSummary(EquipmentId, objComb.EquipData.ClientLookupId, objComb.EquipData.Name, objComb.EquipData.RemoveFromService, objComb.EquipData.Status);
            var item = EquipmentDetails;
            EquipmentSearchModel eSearchModel = new EquipmentSearchModel();
            eSearchModel.EquipmentId = item.EquipmentId;
            eSearchModel.ClientLookupId = item.ClientLookupId;
            eSearchModel.Name = item.Name;
            // RKL - 2020-12-11 - Not using the Location table anymore
            //eSearchModel.Location = (userData.DatabaseKey.Client.BusinessType.ToUpper() == BusinessTypeConstants.Facilities) ? item.LocationIdClientLookupId : item.Location;
            eSearchModel.Location = item.Location;
            eSearchModel.SerialNumber = item.SerialNumber;
            eSearchModel.Type = item.Type;
            eSearchModel.Make = item.Make;
            eSearchModel.Model = item.Model;
            eSearchModel.LaborAccountClientLookupId = item.LaborAccountClientLookupId;
            eSearchModel.AssetNumber = item.AssetNumber;
            eSearchModel.AssetGroup1ClientLookupId = Convert.ToString(item.AssetGroup1ClientLookupId);
            eSearchModel.AssetGroup2ClientLookupId = Convert.ToString(item.AssetGroup2ClientLookupId);
            eSearchModel.AssetGroup3ClientLookupId = Convert.ToString(item.AssetGroup3ClientLookupId);
            //V2-636 
            eSearchModel.RemoveFromService = item.RemoveFromService;
            if (item.RemoveFromServiceDate != null && item.RemoveFromServiceDate == default(DateTime))
            {
                eSearchModel.RemoveFromServiceDate = null;
            }
            else
            {
                eSearchModel.RemoveFromServiceDate = item.RemoveFromServiceDate;
            }
            eSearchModel.BusinessType = userData.DatabaseKey.Client.BusinessType;
            objComb.EquipmentDetailsCard = eSearchModel;
            var equipmentDownTimeList = UtilityFunction.EquipmentDownTimeDatesList();
            if (equipmentDownTimeList != null)
            {
                objComb.equipmentDownTimeDateList = equipmentDownTimeList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
            }
            var workOrderTypeList = UtilityFunction.WorkOrderDatesList();
            if (workOrderTypeList != null)
            {
                objComb.workOrderTypeDateList = workOrderTypeList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
            }
            string temp = string.Empty;
            temp = EquipmentDetails.ClientLookupId + "][" + EquipmentDetails.Name + "][" + EquipmentDetails.SerialNumber + "][" + EquipmentDetails.Make + "][" + EquipmentDetails.Model;
            QRCodeModel qRCodeModel = new QRCodeModel();
            List<string> equipmentClientLookUpNames = new List<string>();
            equipmentClientLookUpNames.Add(temp);
            qRCodeModel.EquipmentIdsList = equipmentClientLookUpNames;
            objComb.qRCodeModel = qRCodeModel;
            equipmentModel.AlertFollowedEquipment = eWrapper.AlertFollowIdExist(EquipmentId);
            equipmentModel.BusinessType = userData.DatabaseKey.Client.BusinessType;

            _ChangeEquipmentIDModel.EquipmentId = Convert.ToInt64(objComb.EquipData.EquipmentId);
            _ChangeEquipmentIDModel.ClientLookupId = objComb.EquipData.ClientLookupId;
            _ChangeEquipmentIDModel.OldClientLookupId = objComb.EquipData.ClientLookupId;
            _ChangeEquipmentIDModel.UpdateIndex = objComb.EquipData.UpdateIndex;
            objComb.EquipModel = equipmentModel;
            objComb._ChangeEquipmentIDModel = _ChangeEquipmentIDModel;
            objComb._CreatedLastUpdatedModel = eWrapper.createdLastUpdatedModel(EquipmentId);
            objComb.security = this.userData.Security;
            objComb._userdata = this.userData;


            #region Asset availability
            var removefromServiceReasonCode = objCommonWrapper.GetListFromConstVals(LookupListConstants.RemoveFromServiceReasonCode);
            if (removefromServiceReasonCode != null)
            {
                objComb.LookupRemoveFromServiceReasonCode = removefromServiceReasonCode.Select(x => new SelectListItem { Text = x.text.ToString(), Value = x.value.ToString() });
                if (objComb.EquipData.RemoveFromServiceReasonCode != "")
                {
                    objComb.ServiceReasonCode = AssetStatusConstant.Scrapped;
                    if (objComb.EquipData.RemoveFromServiceReasonCode != AssetStatusConstant.Scrapped)
                    {
                        objComb.ServiceReasonCode = objComb.LookupRemoveFromServiceReasonCode.Where(x => x.Value == objComb.EquipData.RemoveFromServiceReasonCode).Select(x => x.Text).FirstOrDefault().ToString();
                    }
                }
            }

            assetAvailabilityRemoveModel.EquipmentId = EquipmentId;
            assetAvailabilityRemoveModel.RemoveFromService = objComb.EquipData.RemoveFromService;
            objComb._AssetAvailabilityRemoveModel = assetAvailabilityRemoveModel;
            assetAvailabilityUpdateModel.EquipmentId = EquipmentId;
            objComb._AssetAvailabilityUpdateModel = assetAvailabilityUpdateModel;
            objComb.IsAssetAvailability = userData.Security.Asset_Availability.Access;
            objComb.RemoveServiceDate = Convert.ToDateTime(objComb.EquipData.RemoveFromServiceDate).ToUserTimeZone(userData.Site.TimeZone);
            #endregion
            PartModel objPartModel = new PartModel();
            var AllLookUps = objCommonWrapper.GetAllLookUpList();
            if (AllLookUps != null)
            {
                var LookupStokeType = AllLookUps.Where(x => x.ListName == LookupListConstants.STOCK_TYPE).ToList();
                if (LookupStokeType != null)
                {
                    objPartModel.LookupStokeTypeList = LookupStokeType.Select(x => new SelectListItem { Text = x.ListValue + "  -  " + x.Description, Value = x.ListValue });
                }

            }
            objComb.PartModel = objPartModel;
            this.GetAssetGroupHeaderName(objComb);
            LocalizeControls(objComb, LocalizeResourceSetConstants.EquipmentDetails);

            return PartialView("~/Views/Equipment/Mobile/_EquipmentDetailMobile.cshtml", objComb);


        }
        [HttpPost]
        public PartialViewResult LoadViewAssetDetails_Mobile(long EquipmentId)
        {
            EquipmentCombined objECVM = new EquipmentCombined();
            EquipmentWrapper eWrapper = new EquipmentWrapper(userData);
            var EquipmentDetails = eWrapper.GetEquipmentDetailsById(EquipmentId);
            EquipmentUDF EquipmentUDF = new EquipmentUDF();
            Task[] tasks = new Task[2];
            objECVM.security = this.userData.Security;
            objECVM._userdata = this.userData;
            tasks[0] = Task.Factory.StartNew(() => EquipmentUDF = eWrapper.RetrieveEquipmentUDFByEquipmentId(EquipmentId));
            tasks[1] = Task.Factory.StartNew(() => objECVM.UIConfigurationDetails = new RetrieveDataForUIConfiguration().Retrieve(DataDictionaryViewNameConstant.ViewAssetWidget, userData));
            Task.WaitAll(tasks);
            objECVM.ViewEquipment = new ViewEquipmentModelDynamic();
            objECVM.ViewEquipment = eWrapper.MapEquipmentDataForView(objECVM.ViewEquipment, EquipmentDetails);
            objECVM.ViewEquipment = eWrapper.MapEquipmentUDFDataForView(objECVM.ViewEquipment, EquipmentUDF);

            LocalizeControls(objECVM, LocalizeResourceSetConstants.EquipmentDetails);
            return PartialView("~/Views/Equipment/Mobile/_EquipmentViewDynamic.cshtml", objECVM);
        }
        #region  Multiple Images
        public PartialViewResult GetImages_Mobile(int currentpage, int? start, int? length, long EquipmentId)
        {
            EquipmentCombined equipment = new EquipmentCombined();
            List<ImageAttachmentModel> imgDatalist = new List<ImageAttachmentModel>();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            List<Attachment> Attachments = new List<Attachment>();
            equipment.security = this.userData.Security;
            equipment._userdata = this.userData;
            start = start.HasValue
            ? start / length
            : 0;
            int skip = start * length ?? 0;
            ViewBag.Start = skip;

            if (userData.DatabaseKey.Client.OnPremise)
            {
                Attachments = commonWrapper.GetOnPremiseMultipleImageUrl(skip, length ?? 0, EquipmentId, AttachmentTableConstant.Equipment);
            }
            else
            {
                Attachments = commonWrapper.GetAzureMultipleImageUrl(skip, length ?? 0, EquipmentId, AttachmentTableConstant.Equipment);
            }
            foreach (var attachment in Attachments)
            {
                ImageAttachmentModel imgdata = new ImageAttachmentModel();
                imgdata.AttachmentId = attachment.AttachmentId;
                imgdata.ObjectId = attachment.ObjectId;
                imgdata.Profile = attachment.Profile;
                imgdata.FileName = attachment.FileName;
                string SASImageURL = string.Empty;
                if (!string.IsNullOrEmpty(attachment.AttachmentURL))
                {
                    SASImageURL = attachment.AttachmentURL;
                }
                else
                {
                    commonWrapper.GetNoImageUrl();
                }
                imgdata.AttachmentURL = SASImageURL;
                imgdata.ObjectName = attachment.ObjectName;
                imgdata.TotalCount = attachment.TotalCount;
                imgDatalist.Add(imgdata);
            }
            var recordsFiltered = 0;
            recordsFiltered = imgDatalist.Select(o => o.TotalCount).FirstOrDefault();
            ViewBag.TotalRecords = imgDatalist.Select(o => o.TotalCount).FirstOrDefault();
            ViewBag.Length = length;
            ViewBag.CurrentPage = currentpage;
            equipment.imageAttachmentModels = imgDatalist;
            LocalizeControls(equipment, LocalizeResourceSetConstants.Global);

            return PartialView("~/Views/Equipment/Mobile/_AllImages.cshtml", equipment);
        }
        #endregion

        #region Comment 
        [HttpPost]
        public PartialViewResult GetCommentsDetails_Mobile()
        {
            EquipmentCombined objECVM = new EquipmentCombined();
            objECVM._userdata = this.userData;
            LocalizeControls(objECVM, LocalizeResourceSetConstants.EquipmentDetails);
            return PartialView("~/Views/Equipment/Mobile/_CommentDetails.cshtml", objECVM);
        }
        [HttpPost]
        public PartialViewResult LoadComments_Mobile(long EquipmentId)
        {
            EquipmentCombined _EquipmentObj = new EquipmentCombined();
            CommonWrapper coWrapper = new CommonWrapper(userData);
            List<Personnel> personnelsList = new List<Personnel>();
            List<Notes> NotesList = new List<Notes>();
            UserMentionData userMentionData;
            List<UserMentionData> userMentionDatas = new List<UserMentionData>();
            Task[] tasks = new Task[2];
            tasks[0] = Task.Factory.StartNew(() => personnelsList = coWrapper.MentionList(""));
            tasks[1] = Task.Factory.StartNew(() => NotesList = coWrapper.PopulateComment(EquipmentId, AttachmentTableConstant.Equipment));
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
                _EquipmentObj.userMentionDatas = userMentionDatas;
            }
            if (!tasks[1].IsFaulted && tasks[1].IsCompleted)
            {
                _EquipmentObj.NotesList = NotesList;
            }
            LocalizeControls(_EquipmentObj, LocalizeResourceSetConstants.EquipmentDetails);
            return PartialView("/Views/Equipment/Mobile/_CommentsList.cshtml", _EquipmentObj);
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult AddOrUpdateComment_Mobile(long EquipmentId, string content, string EqClientLookupId, List<string> userList, long noteId = 0, long updatedindex = 0)
        {
            CommonWrapper coWrapper = new CommonWrapper(userData);
            var namelist = coWrapper.MentionList("");
            List<UserMentionData> userMentionDataList = new List<UserMentionData>();
            UserMentionData objUserMentionData;
            if (userList != null && userList.Count > 0)
            {
                foreach (var item in userList)
                {
                    objUserMentionData = new UserMentionData();
                    objUserMentionData.userId = namelist.Where(x => x.UserName == item).Select(y => y.PersonnelId).FirstOrDefault();
                    objUserMentionData.userName = item;
                    objUserMentionData.emailId = namelist.Where(x => x.UserName == item).Select(y => y.Email).FirstOrDefault();
                    userMentionDataList.Add(objUserMentionData);
                }
            }
            NotesModel notesModel = new NotesModel();
            notesModel.ObjectId = EquipmentId;
            notesModel.Content = content;
            notesModel.NotesId = noteId;
            notesModel.ClientLookupId = EqClientLookupId;
            notesModel.updatedindex = updatedindex;
            if (ModelState.IsValid)
            {
                string Mode = string.Empty;
                List<String> errorList = new List<string>();
                errorList = coWrapper.AddOrUpdateComment(notesModel, ref Mode, AttachmentTableConstant.Equipment, userMentionDataList);
                if (errorList != null && errorList.Count > 0)
                {
                    return Json(errorList, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), equipmentid = EquipmentId, mode = Mode }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                string ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        #region Add And Edit equipment
        [HttpPost]
        public ActionResult AddEquipmentDynamicView_Mobile()
        {
            Task taskAllLookUp, taskAssetGroup1LookUp, taskAssetGroup2LookUp, taskAssetGroup3LookUp;
            EquipmentWrapper eWrapper = new EquipmentWrapper(userData);
            EquipmentCombined objComb = new EquipmentCombined();
            objComb.EditEquipment = new EditEquipmentModelDynamic();
            objComb.PlantLocation = userData.Site.PlantLocation;

            CommonWrapper commonWrapper = new CommonWrapper(userData);
            List<DataContracts.LookupList> TypeList = new List<DataContracts.LookupList>();
            List<DataContracts.LookupList> AllLookUps = new List<DataContracts.LookupList>();

            #region task          

            taskAllLookUp = Task.Factory.StartNew(() => AllLookUps = commonWrapper.GetAllLookUpList());
            List<AssetGroup1Model> astGroup1 = new List<AssetGroup1Model>();

            taskAssetGroup1LookUp = Task.Factory.StartNew(() => astGroup1 = eWrapper.GetAssetGroup1Dropdowndata());

            List<AssetGroup2Model> astGroup2 = new List<AssetGroup2Model>();
            taskAssetGroup2LookUp = Task.Factory.StartNew(() => astGroup2 = eWrapper.GetAssetGroup2Dropdowndata());

            List<AssetGroup3Model> astGroup3 = new List<AssetGroup3Model>();
            taskAssetGroup3LookUp = Task.Factory.StartNew(() => astGroup3 = eWrapper.GetAssetGroup3Dropdowndata());

            Task.WaitAll(taskAllLookUp, taskAssetGroup1LookUp, taskAssetGroup2LookUp, taskAssetGroup3LookUp);
            #endregion

            objComb.BusinessType = userData.DatabaseKey.Client.BusinessType;
            var AssetCategoryList = UtilityFunction.AssetCategoryList();
            if (AssetCategoryList != null)
            {
                objComb.AssetCategoryList = AssetCategoryList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
            }


            if (astGroup1 != null)
            {
                objComb.AssetGroup1List = astGroup1.Select(x => new SelectListItem { Text = x.AssetGroup1DescWithClientLookupId, Value = x.AssetGroup1Id.ToString() });
            }
            if (astGroup2 != null)
            {
                objComb.AssetGroup2List = astGroup2.Select(x => new SelectListItem { Text = x.AssetGroup2DescWithClientLookupId, Value = x.AssetGroup2Id.ToString() });
            }
            if (astGroup3 != null)
            {
                objComb.AssetGroup3List = astGroup3.Select(x => new SelectListItem { Text = x.AssetGroup3DescWithClientLookupId, Value = x.AssetGroup3Id.ToString() });
            }


            objComb.UIConfigurationDetails = new RetrieveDataForUIConfiguration().Retrieve(DataDictionaryViewNameConstant.AddAsset, userData);
            IList<string> LookupNames = objComb.UIConfigurationDetails.ToList()
                                            .Where(x => x.LookupType == "LookupList" && !string.IsNullOrEmpty(x.LookupName))
                                            .Select(s => s.LookupName)
                                            .ToList();
            if (AllLookUps != null)
            {
                objComb.AllRequiredLookUplist = AllLookUps.Where(x => LookupNames.Contains(x.ListName))
                                                          .Select(s => new UILookupList
                                                          { text = s.ListValue + " - " + s.Description, value = s.ListValue, lookupname = s.ListName })
                                                          .ToList();
            }

            objComb._userdata = userData;
            objComb.AssetGroup1Label = userData.Site.AssetGroup1Name;
            objComb.AssetGroup2Label = userData.Site.AssetGroup2Name;
            objComb.AssetGroup3Label = userData.Site.AssetGroup3Name;

            LocalizeControls(objComb, LocalizeResourceSetConstants.EquipmentDetails);
            return PartialView("/Views/Equipment/Mobile/_EquipmentAddDynamic.cshtml", objComb);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEquipmentDynamic_Mobile(EquipmentCombined objEM, string Command)
        {
            EquipmentWrapper eWrapper = new EquipmentWrapper(userData);
            string ModelValidationFailedMessage = string.Empty;

            if (ModelState.IsValid)
            {
                Equipment equipment = new Equipment();
                equipment = eWrapper.AddEquipmentDynamic(objEM);
                if (equipment.ErrorMessages != null && equipment.ErrorMessages.Count > 0)
                {
                    return Json(equipment.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), Command = Command, EquipmentId = equipment.EquipmentId }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult EditEquipmentDynamic_Mobile(long EquipmentId, string ClientLookupId, string Name, bool isRemoveFromService, string Status)
        {
            Task taskAllLookUp, taskAssetGroup1LookUp, taskAssetGroup2LookUp, taskAssetGroup3LookUp, taskAssetDetailsById;
            EquipmentWrapper eWrapper = new EquipmentWrapper(userData);
            EquipmentCombined objComb = new EquipmentCombined();
            objComb.EditEquipment = new EditEquipmentModelDynamic();
            objComb.PlantLocation = userData.Site.PlantLocation;

            CommonWrapper commonWrapper = new CommonWrapper(userData);
            List<DataContracts.LookupList> TypeList = new List<DataContracts.LookupList>();
            List<DataContracts.LookupList> AllLookUps = new List<DataContracts.LookupList>();

            #region task
            taskAssetDetailsById = Task.Factory.StartNew(() => objComb.EditEquipment = eWrapper.RetrieveEquipmentDetailsByEquipmentId(EquipmentId));

            taskAllLookUp = Task.Factory.StartNew(() => AllLookUps = commonWrapper.GetAllLookUpList());
            List<AssetGroup1Model> astGroup1 = new List<AssetGroup1Model>();

            taskAssetGroup1LookUp = Task.Factory.StartNew(() => astGroup1 = eWrapper.GetAssetGroup1Dropdowndata());

            List<AssetGroup2Model> astGroup2 = new List<AssetGroup2Model>();
            taskAssetGroup2LookUp = Task.Factory.StartNew(() => astGroup2 = eWrapper.GetAssetGroup2Dropdowndata());

            List<AssetGroup3Model> astGroup3 = new List<AssetGroup3Model>();
            taskAssetGroup3LookUp = Task.Factory.StartNew(() => astGroup3 = eWrapper.GetAssetGroup3Dropdowndata());

            Task.WaitAll(taskAssetDetailsById, taskAllLookUp, taskAssetGroup1LookUp, taskAssetGroup2LookUp, taskAssetGroup3LookUp);
            #endregion

            objComb.BusinessType = userData.DatabaseKey.Client.BusinessType;
            var AssetCategoryList = UtilityFunction.AssetCategoryList();
            if (AssetCategoryList != null)
            {
                objComb.AssetCategoryList = AssetCategoryList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
            }


            if (astGroup1 != null)
            {
                objComb.AssetGroup1List = astGroup1.Select(x => new SelectListItem { Text = x.AssetGroup1DescWithClientLookupId, Value = x.AssetGroup1Id.ToString() });
            }
            if (astGroup2 != null)
            {
                objComb.AssetGroup2List = astGroup2.Select(x => new SelectListItem { Text = x.AssetGroup2DescWithClientLookupId, Value = x.AssetGroup2Id.ToString() });
            }
            if (astGroup3 != null)
            {
                objComb.AssetGroup3List = astGroup3.Select(x => new SelectListItem { Text = x.AssetGroup3DescWithClientLookupId, Value = x.AssetGroup3Id.ToString() });
            }


            objComb.UIConfigurationDetails = new RetrieveDataForUIConfiguration()
                                            .Retrieve(DataDictionaryViewNameConstant.EditAsset, userData);
            IList<string> LookupNames = objComb.UIConfigurationDetails.ToList()
                                            .Where(x => x.LookupType == "LookupList" && !string.IsNullOrEmpty(x.LookupName))
                                            .Select(s => s.LookupName)
                                            .ToList();
            if (AllLookUps != null)
            {
                objComb.AllRequiredLookUplist = AllLookUps.Where(x => LookupNames.Contains(x.ListName))
                                                          .Select(s => new UILookupList
                                                          { text = s.ListValue + " - " + s.Description, value = s.ListValue, lookupname = s.ListName })
                                                          .ToList();
            }

            objComb._userdata = userData;
            objComb._EquipmentSummaryModel = GetEquipmentSummary(EquipmentId, ClientLookupId, Name, isRemoveFromService, Status);
            objComb.AssetGroup1Label = userData.Site.AssetGroup1Name;
            objComb.AssetGroup2Label = userData.Site.AssetGroup2Name;
            objComb.AssetGroup3Label = userData.Site.AssetGroup3Name;

            LocalizeControls(objComb, LocalizeResourceSetConstants.EquipmentDetails);

            return PartialView("/Views/Equipment/Mobile/_EquipmentEditDynamic.cshtml", objComb);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateEquipmentDynamicInfo_Mobile(EquipmentCombined equip)
        {

            EquipmentWrapper eWrapper = new EquipmentWrapper(userData);
            Equipment equipment = new Equipment();

            if (ModelState.IsValid)
            {
                equipment = eWrapper.EditEquipmentDynamic(equip);
                if (equipment.ErrorMessages != null && equipment.ErrorMessages.Count > 0)
                {
                    return Json(equipment.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), equipmentid = equipment.EquipmentId }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                string ModelValidationFailedMessage = string.Empty;
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }
        public PartialViewResult GetAccountLookupList_Mobile()
        {
            return PartialView("~/Views/Equipment/Mobile/_AccountGridPopUp.cshtml");
        }

        [HttpPost]
        public JsonResult GetAccountLookupListchunksearch_Mobile(int Start, int Length, string Search = "")
        {
            var modelList = new List<AccountLookUpModel>();
            CommonWrapper commonWrapper = new CommonWrapper(userData);

            string order = "0";
            string orderDir = "asc";

            modelList = commonWrapper.GetAccountLookupListGridDataMobile(order, orderDir, Start, Length, Search, Search);
            return Json(new { recordsTotal = modelList.Count() > 0 ? modelList[0].TotalCount : 0, data = modelList });
        }

        public PartialViewResult EquipmentLookupListView_Mobile()
        {
            return PartialView("~/Views/Equipment/Mobile/_EquipmentGridPopUp.cshtml");
        }
        [HttpPost]
        public JsonResult GetEquipmentLookupListchunksearch_Mobile(int Start, int Length, string Search = "")
        {
            var modelList = new List<EquipmentLookupModel>();
            CommonWrapper commonWrapper = new CommonWrapper(userData);

            string order = "0";
            string orderDir = "asc";

            modelList = commonWrapper.GetEquipmentLookupListGridDataMobile(order, orderDir, Start, Length, Search, Search);

            return Json(new { recordsTotal = modelList.Count() > 0 ? modelList[0].TotalCount : 0, data = modelList });
        }

        public PartialViewResult GetVendorLookupList_Mobile()
        {
            return PartialView("~/Views/Equipment/Mobile/_VendorGridPopUp.cshtml");
        }

        [HttpPost]
        public JsonResult GetVendorLookupListchunksearch_Mobile(int Start, int Length, string Search = "")
        {
            var modelList = new List<VendorLookupModel>();
            CommonWrapper commonWrapper = new CommonWrapper(userData);

            string order = "0";
            string orderDir = "asc";

            modelList = commonWrapper.GetVendorLookupListGridData(order, orderDir, Start, Length, Search, Search);
            return Json(new { recordsTotal = modelList.Count() > 0 ? modelList[0].TotalCount : 0, data = modelList });
        }

        #endregion
        #endregion

        #region V2-919
        #region Attachment
        public PartialViewResult LoadAttachments_Mobile()
        {
            EquipmentCombined objECVM = new EquipmentCombined();
            objECVM.security = this.userData.Security;
            objECVM._userdata = this.userData;
            LocalizeControls(objECVM, LocalizeResourceSetConstants.EquipmentDetails);
            return PartialView("~/Views/Equipment/Mobile/_EquipmentAttachmentListSearch.cshtml", objECVM);
        }
        [HttpPost]
        public string PopulateAttachment_Mobile(int? draw, int? start, int? length, long EquipmentId)
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            CommonWrapper objCommonWrapper = new CommonWrapper(userData);
            var AttachmentList = objCommonWrapper.PopulateAttachments(EquipmentId, AttachmentTableConstant.Equipment, userData.Security.Equipment.Edit);
            if (AttachmentList != null)
            {
                AttachmentList = GetAllAttachmentsSortByColumnWithOrder(order, orderDir, AttachmentList);
            }
            var totalRecords = 0;
            var recordsFiltered = 0;
            start = start.HasValue
                ? start / length
                : 0;
            recordsFiltered = AttachmentList.Count();
            totalRecords = AttachmentList.Count();
            int initialPage = start.Value;
            var filteredResult = AttachmentList
                .Skip(initialPage * length ?? 0)
                .Take(length ?? 0)
                .ToList();
            var Attachsecurity = userData.Security.Equipment.Edit;
            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult, Attachsecurity = Attachsecurity }, JsonSerializer12HoursDateAndTimeSettings);
        }
        #region Add
        [HttpGet]
        public ActionResult ShowAddAttachment_Mobile(long EquipmentId, string ClientLookupId, string Name, bool isRemoveFromService, string Status)
        {
            EquipmentCombined _EquipmentObj = new EquipmentCombined();
            EquipmentWrapper EWrapper = new EquipmentWrapper(userData);
            AttachmentModel Attachment = new AttachmentModel();
            Attachment.EquipmentId = EquipmentId;
            Attachment.ClientLookupId = ClientLookupId;
            _EquipmentObj.attachmentModel = Attachment;
            _EquipmentObj._userdata = userData;
            LocalizeControls(_EquipmentObj, LocalizeResourceSetConstants.EquipmentDetails);
            return PartialView("Mobile/AttachmentAdd", _EquipmentObj);
        }
        #endregion
        #endregion

        #region TechSpecs
        public PartialViewResult LoadTechSpecs_Mobile()
        {
            EquipmentCombined objECVM = new EquipmentCombined();
            objECVM.security = this.userData.Security;
            objECVM._userdata = this.userData;
            LocalizeControls(objECVM, LocalizeResourceSetConstants.EquipmentDetails);
            return PartialView("~/Views/Equipment/Mobile/_EquipmentTechSpecsListSearch.cshtml", objECVM);
        }
        [HttpPost]
        public string GetEquipment_TechSpecs_Mobile(int? draw, int? start, int? length, long EquipmentId)
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            EquipmentWrapper eWrapper = new EquipmentWrapper(userData);
            var TechSpecs = eWrapper.GetEquipmentTechSpecs(EquipmentId);
            TechSpecs = this.GetAllTechSpecsSortByColumnWithOrder(order, orderDir, TechSpecs);
            var totalRecords = 0;
            var recordsFiltered = 0;
            start = start.HasValue
                ? start / length
                : 0;
            recordsFiltered = TechSpecs.Count();
            totalRecords = TechSpecs.Count();
            int initialPage = start.Value;
            var filteredResult = TechSpecs
                .Skip(initialPage * length ?? 0)
                .Take(length ?? 0)
                .ToList();
            var techSpecsSecurity = this.userData.Security.Equipment.Edit;
            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult, techSpecsSecurity = techSpecsSecurity }, JsonSerializer12HoursDateAndTimeSettings);
        }
        #region Add
        [HttpGet]
        public ActionResult AddTechSpecs_Mobile(long EquipmentId, string TechMode, string ClientLookupId, string Name, long TechSpecId = 0, bool isRemoveFromService = false, string Status = "")
        {
            EquipmentCombined objComb = new EquipmentCombined();
            TechSpecsModel model = new TechSpecsModel();
            string Mode = TechMode;
            EquipmentWrapper EWrapper = new EquipmentWrapper(userData);
            model.Mode = Mode;
            model.EquipmentId = EquipmentId;
            List<DataContracts.LookupList> AllLookUps = new List<DataContracts.LookupList>();
            List<DataContracts.LookupList> TechSpecList = new List<DataContracts.LookupList>();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            AllLookUps = commonWrapper.GetAllLookUpList();
            if (AllLookUps != null)
            {
                TechSpecList = AllLookUps.Where(x => x.ListName == LookupListConstants.TECH_SPEC).ToList();
                TechSpecList = TechSpecList.GroupBy(x => x.ListValue).Select(x => x.FirstOrDefault()).ToList();
            }
            objComb.techSpecsModel = EWrapper.TechSpecsAdd(EquipmentId, model, TechSpecList, TechSpecId);
            objComb._userdata = userData;
            LocalizeControls(objComb, LocalizeResourceSetConstants.EquipmentDetails);
            return PartialView("Mobile/AddTechSpecs", objComb);
        }
        #endregion
        #endregion

        #region Parts
        public PartialViewResult LoadParts_Mobile()
        {
            EquipmentCombined objECVM = new EquipmentCombined();
            objECVM.security = this.userData.Security;
            objECVM._userdata = this.userData;
            LocalizeControls(objECVM, LocalizeResourceSetConstants.EquipmentDetails);
            return PartialView("~/Views/Equipment/Mobile/_EquipmentPartsListSearch.cshtml", objECVM);
        }
        [HttpPost]
        public string GetEquipment_Parts_Mobile(int? draw, int? start, int? length, long EquipmentId)
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            EquipmentWrapper eWrapper = new EquipmentWrapper(userData);
            var Parts = eWrapper.GetEquipmentParts(EquipmentId, "", "", "");
            Parts = this.GetAllPartsSortByColumnWithOrder(order, orderDir, Parts);
            var totalRecords = 0;
            var recordsFiltered = 0;
            start = start.HasValue
                ? start / length
                : 0;
            recordsFiltered = Parts.Count();
            totalRecords = Parts.Count();
            int initialPage = start.Value;
            var filteredResult = Parts
                .Skip(initialPage * length ?? 0)
                .Take(length ?? 0)
                .ToList();
            var partSecurity = this.userData.Security.Equipment.Edit;
            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult, partSecurity = partSecurity }, JsonSerializer12HoursDateAndTimeSettings);
        }
        #region LookUpList
        public PartialViewResult PartLookupListView_Mobile()
        {
            return PartialView("~/Views/Equipment/Mobile/_AddPartIdPopupSearchGrid.cshtml");
        }
        public JsonResult GetPartLookupListchunksearch_Mobile(int Start, int Length, string Search = "", string Storeroomid = "")
        {
            var modelList = new List<PartXRefGridDataModel>();
            CommonWrapper commonWrapper = new CommonWrapper(userData);

            string order = "0";
            string orderDir = "asc";

            modelList = commonWrapper.GetPartLookupListGridData_Mobile(order, orderDir, Start, Length, Search, Search, Storeroomid);

            return Json(new { recordsTotal = modelList.Count() > 0 ? modelList[0].TotalCount : 0, data = modelList });
        }
        #endregion

        #region Add Edit
        public ActionResult PartsAdd_Mobile(long EquipmentId, string ClientLookupId, string Name, bool isRemoveFromService, string Status)
        {
            EquipmentCombined objComb = new EquipmentCombined();
            PartsSessionData objPartsSessionData = new PartsSessionData();
            objPartsSessionData.EquipmentId = EquipmentId;
            objComb.partsSessionData = objPartsSessionData;
            objComb._userdata = this.userData;
            LocalizeControls(objComb, LocalizeResourceSetConstants.EquipmentDetails);
            return PartialView("Mobile/AddParts", objComb);
        }
        public ActionResult PartsEdit_Mobile(long EquipmentId, long Equipment_Parts_XrefId, string ClientLookupId, string Name, bool isRemoveFromService, string Status)
        {
            EquipmentCombined objComb = new EquipmentCombined();
            EquipmentWrapper EWrapper = new EquipmentWrapper(userData);
            objComb = EWrapper.EditParts(EquipmentId, Equipment_Parts_XrefId, objComb);
            objComb._userdata = this.userData;
            LocalizeControls(objComb, LocalizeResourceSetConstants.EquipmentDetails);
            return PartialView("Mobile/EditParts", objComb);
        }
        #endregion
        #endregion

        #region DownTime
        public PartialViewResult LoadDowntime_Mobile()
        {
            EquipmentCombined objECVM = new EquipmentCombined();
            objECVM.security = this.userData.Security;
            objECVM._userdata = this.userData;
            LocalizeControls(objECVM, LocalizeResourceSetConstants.EquipmentDetails);
            return PartialView("~/Views/Equipment/Mobile/_EquipmentDowntimeListSearch.cshtml", objECVM);
        }
        [HttpPost]
        public string GetEquipment_Downtime_Mobile(int? draw, int? start, int? length, long EquipmentId)
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            EquipmentWrapper eWrapper = new EquipmentWrapper(userData);
            start = start.HasValue
                ? start / length
                : 0;
            int skip = start * length ?? 0;
            var Downtime = eWrapper.GetEquipmentDowntime_V2(EquipmentId, order, orderDir, skip, length ?? 0);

            var totalRecords = 0;
            var recordsFiltered = 0;
            start = start.HasValue
                ? start / length
                : 0;
            if (Downtime != null && Downtime.Count > 0)
            {
                recordsFiltered = Downtime[0].TotalCount;
                totalRecords = Downtime[0].TotalCount;
            }
            else
            {
                recordsFiltered = 0;
                totalRecords = 0;
            }

            int initialPage = start.Value;
            var filteredResult = Downtime
                .Skip(initialPage * length ?? 0)
                .Take(length ?? 0)
                .ToList();
            var secDownTimeAdd = this.userData.Security.Asset_Downtime.Create;
            var secDownTimeEdit = this.userData.Security.Asset_Downtime.Edit;
            var secDownTimeDelete = this.userData.Security.Asset_Downtime.Delete;
            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult, secDownTimeAdd = secDownTimeAdd, secDownTimeEdit = secDownTimeEdit, secDownTimeDelete = secDownTimeDelete }, JsonSerializerDateSettings);
        }
        #region Add Edit
        public ActionResult DownTimeAdd_Mobile(long EquipmentId, string ClientLookupId, string Name, bool isRemoveFromService, string Status)
        {
            EquipmentCombined objComb = new EquipmentCombined();
            DownTimeModel objDownTimeModel = new DownTimeModel();
            EquipmentWrapper EWrapper = new EquipmentWrapper(userData);
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            objDownTimeModel.EquipmentId = EquipmentId;
            objDownTimeModel.PersonnelClientLookupId = userData.DatabaseKey.Personnel.ClientLookupId;
            objDownTimeModel.DateDown = DateTime.Now;
            objComb.downTimeModel = objDownTimeModel;
            objComb._userdata = userData;
            var AllLookUps = commonWrapper.GetAllLookUpList();
            var ReasonforDown = AllLookUps.Where(x => x.ListName == LookupListConstants.WO_DOWN_REASON).ToList();
            if (ReasonforDown != null)
            {
                objComb.ReasonForDownList = ReasonforDown.Select(x => new SelectListItem { Text = x.ListValue + " - " + x.Description, Value = x.ListValue }).ToList();
            }
            LocalizeControls(objComb, LocalizeResourceSetConstants.EquipmentDetails);
            return PartialView("Mobile/AddDowntime", objComb);
        }
        public ActionResult ShowDownTimeEdit_Mobile(long EquipmentId, long DownTimeId, string ClientLookupId, string Name, bool isRemoveFromService, string Status)
        {

            EquipmentCombined objComb = new EquipmentCombined();
            DownTimeModel objDownTimeModel = new DownTimeModel();
            DowntimeSessionData _DowntimeSessionData = new DowntimeSessionData();
            EquipmentWrapper EWrapper = new EquipmentWrapper(userData);
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            objComb = EWrapper.ShowEditDownTime(EquipmentId, DownTimeId, objComb, objDownTimeModel);
            objComb._userdata = userData;
            var AllLookUps = commonWrapper.GetAllLookUpList();
            var ReasonforDown = AllLookUps.Where(x => x.ListName == LookupListConstants.WO_DOWN_REASON).ToList();
            if (ReasonforDown != null)
            {
                objComb.ReasonForDownList = ReasonforDown.Select(x => new SelectListItem { Text = x.ListValue + " - " + x.Description, Value = x.ListValue }).ToList();
            }
            LocalizeControls(objComb, LocalizeResourceSetConstants.EquipmentDetails);
            return PartialView("Mobile/EditDowntime", objComb);
        }
        #endregion
        #endregion

        #region PMList
        public PartialViewResult LoadPMList_Mobile()
        {
            EquipmentCombined objECVM = new EquipmentCombined();
            objECVM.security = this.userData.Security;
            objECVM._userdata = this.userData;
            LocalizeControls(objECVM, LocalizeResourceSetConstants.EquipmentDetails);
            return PartialView("~/Views/Equipment/Mobile/_EquipmentPMListSearch.cshtml", objECVM);
        }
        [HttpPost]
        public string GetEquipment_PMList_Mobile(int? draw, int? start, int? length, long EquipmentId)
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            EquipmentWrapper eWrapper = new EquipmentWrapper(userData);
            var PMList = eWrapper.GetEquipmentPMList(EquipmentId);
            PMList = this.GetAllPMListSortByColumnWithOrder(order, orderDir, PMList);
            var totalRecords = 0;
            var recordsFiltered = 0;
            start = start.HasValue
                ? start / length
                : 0;
            recordsFiltered = PMList.Count();
            totalRecords = PMList.Count();
            int initialPage = start.Value;
            var filteredResult = PMList
                .Skip(initialPage * length ?? 0)
                .Take(length ?? 0)
                .ToList();
            var pmSecurity = this.userData.Security.Equipment.Edit;
            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult, pmSecurity = pmSecurity }, JsonSerializer12HoursDateAndTimeSettings);
        }
        #endregion

        #region WOActive
        public PartialViewResult LoadWOActive_Mobile()
        {
            EquipmentCombined objECVM = new EquipmentCombined();
            objECVM.security = this.userData.Security;
            objECVM._userdata = this.userData;
            LocalizeControls(objECVM, LocalizeResourceSetConstants.EquipmentDetails);
            return PartialView("~/Views/Equipment/Mobile/_EquipmentWOActiveListSearch.cshtml", objECVM);
        }
        [HttpPost]
        public string GetEquipment_WOActive_Mobile(int? draw, int? start, int? length, long EquipmentId)
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            EquipmentWrapper eWrapper = new EquipmentWrapper(userData);
            List<WOActiveModel> WOActiveModelList = eWrapper.getDetailsWOActive(true, EquipmentId);
            WOActiveModelList = this.GetWOActiveGridSortByColumnWithOrder(order, orderDir, WOActiveModelList);
            var totalRecords = 0;
            var recordsFiltered = 0;
            start = start.HasValue
                ? start / length
                : 0;
            recordsFiltered = WOActiveModelList.Count();
            totalRecords = WOActiveModelList.Count();
            int initialPage = start.Value;
            var filteredResult = WOActiveModelList
                .Skip(initialPage * length ?? 0)
                .Take(length ?? 0)
                .ToList();
            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult }, JsonSerializerDateSettings);
        }
        #endregion

        #region WOComplete
        public PartialViewResult LoadWOComplete_Mobile()
        {
            EquipmentCombined objECVM = new EquipmentCombined();
            objECVM.security = this.userData.Security;
            objECVM._userdata = this.userData;
            LocalizeControls(objECVM, LocalizeResourceSetConstants.EquipmentDetails);
            return PartialView("~/Views/Equipment/Mobile/_EquipmentWOCompleteListSearch.cshtml", objECVM);
        }
        [HttpPost]
        public string GetEquipment_WOComplete_Mobile(int? draw, int? start, int? length, long EquipmentId)
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            EquipmentWrapper eWrapper = new EquipmentWrapper(userData);
            List<WOComplete> WOCompleteList = eWrapper.getDetailsWOComplete(false, EquipmentId);
            WOCompleteList = this.GetWOCompleteGridSortByColumnWithOrder(order, orderDir, WOCompleteList);
            var totalRecords = 0;
            var recordsFiltered = 0;
            start = start.HasValue
                ? start / length
                : 0;
            recordsFiltered = WOCompleteList.Count();
            totalRecords = WOCompleteList.Count();
            int initialPage = start.Value;
            var filteredResult = WOCompleteList
                .Skip(initialPage * length ?? 0)
                .Take(length ?? 0)
                .ToList();
            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult }, JsonSerializerDateSettings);
        }
        #endregion

        #region PartIssues
        public PartialViewResult LoadPartIssues_Mobile()
        {
            EquipmentCombined objECVM = new EquipmentCombined();
            objECVM.security = this.userData.Security;
            objECVM._userdata = this.userData;
            LocalizeControls(objECVM, LocalizeResourceSetConstants.EquipmentDetails);
            return PartialView("~/Views/Equipment/Mobile/_EquipmentPartIssuesListSearch.cshtml", objECVM);
        }
        [HttpPost]
        public string GetEquipment_PartIssued_Mobile(int? draw, int? start, int? length, long EquipmentId)
        {
            List<PartIssues> PartIssuesModelList = new List<PartIssues>();
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];

            EquipmentWrapper eWrapper = new EquipmentWrapper(userData);
            List<PartIssues> PartIssuesList = new List<PartIssues>();
            PartIssuesList = eWrapper.GetEquipmentPartIssued(EquipmentId);

            PartIssuesList = this.GetEquipment_PartIssuedGridSortByColumnWithOrder(order, orderDir, PartIssuesList);
            var totalRecords = 0;
            var recordsFiltered = 0;
            start = start.HasValue
                ? start / length
                : 0;
            recordsFiltered = PartIssuesList.Count();
            totalRecords = PartIssuesList.Count();

            int initialPage = start.Value;

            var filteredResult = PartIssuesList
                .Skip(initialPage * length ?? 0)
                .Take(length ?? 0)
                .ToList();

            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult }, JsonSerializerDateSettings);
        }
        #endregion
        #endregion

        #region V2-537
        [HttpPost]
        public string GetEquipment_SensorChunkSearch(int? draw, int? start, int? length, long EquipmentId)
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            start = start.HasValue
              ? start / length
              : 0;
            int skip = start * length ?? 0;
            EquipmentWrapper eWrapper = new EquipmentWrapper(userData);
            var Sensors = eWrapper.GetEquipment_SensorGridData(skip, length ?? 0, order, orderDir, EquipmentId);
            var totalRecords = 0;
            var recordsFiltered = 0;
            if (Sensors != null && Sensors.Count > 0)
            {
                recordsFiltered = Sensors[0].TotalCount;
                totalRecords = Sensors[0].TotalCount;
            }
            int initialPage = start.Value;
            var filteredResult = Sensors
              .ToList();
            var sensorCreate = this.userData.Security.Sensors.Create;
            var sensorEdit = this.userData.Security.Sensors.Create && this.userData.Security.Sensors.Edit;
            var sensorDelete = this.userData.Security.Sensors.Delete && this.userData.Security.Sensors.Edit;
            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult, sensorCreate = sensorCreate, sensorEdit = sensorEdit, sensorDelete = sensorDelete }, JsonSerializer12HoursDateAndTimeSettings);
        }
        #endregion

        #region V2-1089 DevExpress QRCode
        [EncryptedActionParameter]
        public ActionResult QRCodeGenerationUsingDevExpress(bool SmallLabel)
        {
            var EquipmentIdsList = new List<string>();
            if (TempData["QRCodeEquipmentIdList"] != null)
            {
                EquipmentIdsList = (List<string>)TempData["QRCodeEquipmentIdList"];
            }
            else
            {
                EquipmentIdsList = new List<string>();
            }
            // Generate QR code report for each equipment in the EquipmentIdsList
            var masterReport = new XtraReport();
            foreach (var equip in EquipmentIdsList)
            {
                var splitArray = equip.Split(new string[] { "][" }, StringSplitOptions.None);
                if (SmallLabel)
                {
                    // Create a small QR code report
                    var report = new EquipmentSmallQRCodeTemplate
                    {
                        DisplayName = "Equipment",
                        EquipmentBarCode = splitArray[0],
                        ClientLookupId = splitArray[0],
                        EquipName = splitArray[1],
                        SerialNumber = splitArray[2]
                    };

                    report.BindData();
                    report.CreateDocument();
                    masterReport.Pages.AddRange(report.Pages);
                }
                else
                {
                    // Create a large QR code report
                    var report = new EquipmentLargeQRCodeTemplate
                    {
                        DisplayName = "Equipment",
                        EquipmentBarCode = splitArray[0],
                        ClientLookupId = splitArray[0],
                        EquipName = splitArray[1],
                        SerialNumber = splitArray[2],
                        Make = splitArray[3],
                        Model = splitArray[4]
                    };

                    report.BindData();
                    report.CreateDocument();
                    masterReport.Pages.AddRange(report.Pages);
                }
            }

            // Set the default file name for the QR code report
            masterReport.PrintingSystem.ExportOptions.PrintPreview.DefaultFileName = "Somax | Asset QR Code";
            ViewBag.PageTitle = "Somax | Asset QR";
            // Return the QR code report view
            return View("DevExpressQRCodeReportViewer", masterReport);
        }
        #endregion

        #region V2-1115
        [HttpPost]
        public JsonResult SetEquipmentIdlistforEPM(string[] EquipClientLookups)
        {
            List<string> equipmentClientLookUpNames = new List<string>();
            foreach (var e in EquipClientLookups)
            {
                equipmentClientLookUpNames.Add(Convert.ToString(e));
            }
            TempData["QRCodeEquipmentIdList"] = equipmentClientLookUpNames;
            return Json(new { JsonReturnEnum.success });
        }
        public ActionResult GenerateEPMEquipmentQRcode()
        {
            var EquipmentIdsList = new List<string>();
            if (TempData["QRCodeEquipmentIdList"] != null)
            {
                EquipmentIdsList = (List<string>)TempData["QRCodeEquipmentIdList"];
            }
            // Generate QR code report for each equipment in the EquipmentIdsList
            var masterReport = new XtraReport();
            foreach (var equip in EquipmentIdsList)
            {
                var splitArray = equip.Split(new string[] { "][" }, StringSplitOptions.None);
                var report = new EPMEquipmentQRCodeTemplate
                {
                    DisplayName = "Equipment",
                    EquipmentBarCode = splitArray[0],
                    ClientLookupId = splitArray[0]
                };

                report.BindData();
                report.CreateDocument();
                masterReport.Pages.AddRange(report.Pages);
            }
            // Set the default file name for the QR code report
            masterReport.PrintingSystem.ExportOptions.PrintPreview.DefaultFileName = "Somax | Asset QR Code";
            ViewBag.PageTitle = "Somax | Asset QR";
            // Return the QR code report view
            return View("DevExpressQRCodeReportViewer", masterReport);
        }
        #endregion

        #region V2-1159
        public PartialViewResult GetCardViewData(int currentpage, int? start, int? length, string currentorderedcolumn,
            string currentorder, int CustomQueryDisplayId = 0, string ClientLookupId = "", string Name = "", string Location = "", string SerialNumber = "", string Type = "", string Make = "", string Model = "", string LaborAccountClientLookupId = "", string AssetNumber = "", int AssetGroup1Id = 0, int AssetGroup2Id = 0, int AssetGroup3Id = 0, string AssetAvailability = "", string SearchText = "")
        {
            EquipmentSearchModel eSearchModel;
            EquipmentCombined objComb = new EquipmentCombined();
            List<EquipmentSearchModel> eSearchModelList = new List<EquipmentSearchModel>();
            SearchText = SearchText.Replace("%", "[%]");
            ClientLookupId = ClientLookupId.Replace("%", "[%]");
            Name = Name.Replace("%", "[%]");
            Location = Location.Replace("%", "[%]");
            SerialNumber = SerialNumber.Replace("%", "[%]");
            Make = Make.Replace("%", "[%]");
            Model = Model.Replace("%", "[%]");
            AssetNumber = AssetNumber.Replace("%", "[%]");
            SerialNumber = SerialNumber.Replace("%", "[%]");
            LaborAccountClientLookupId = LaborAccountClientLookupId.Replace("%", "[%]");
            start = start.HasValue
               ? start / length
               : 0;
            int skip = start * length ?? 0;
            List<string> typeList = new List<string>();
            EquipmentWrapper eWrapper = new EquipmentWrapper(userData);
            var cardData = eWrapper.GetEquipmentGridData(CustomQueryDisplayId, currentorder, currentorderedcolumn, skip, length ?? 0, ClientLookupId, Name, Location, SerialNumber, Type, Make, Model, LaborAccountClientLookupId, AssetNumber, SearchText, AssetGroup1Id, AssetGroup2Id, AssetGroup3Id, AssetAvailability);

            var totalRecords = 0;
            var recordsFiltered = 0;

            if (cardData != null && cardData.Count > 0)
            {
                recordsFiltered = cardData[0].TotalCount;
                totalRecords = cardData[0].TotalCount;
            }
            else
            {
                recordsFiltered = 0;
                totalRecords = 0;
            }
            int initialPage = start.Value;

            var filteredResult = cardData
              .ToList();
            foreach (var item in filteredResult)
            {
                eSearchModel = new EquipmentSearchModel();
                eSearchModel.EquipmentId = item.EquipmentId;
                eSearchModel.ClientLookupId = item.ClientLookupId;
                eSearchModel.Name = item.Name;
                eSearchModel.Location = item.Location;
                eSearchModel.SerialNumber = item.SerialNumber;
                eSearchModel.Type = item.Type;
                eSearchModel.Make = item.Make;
                eSearchModel.Model = item.Model;
                eSearchModel.LaborAccountClientLookupId = item.LaborAccountClientLookupId;
                eSearchModel.AssetNumber = item.AssetNumber;
                eSearchModel.AssetGroup1ClientLookupId = Convert.ToString(item.AssetGroup1ClientLookupId);
                eSearchModel.AssetGroup2ClientLookupId = Convert.ToString(item.AssetGroup2ClientLookupId);
                eSearchModel.AssetGroup3ClientLookupId = Convert.ToString(item.AssetGroup3ClientLookupId);
                //V2-636 
                eSearchModel.RemoveFromService = item.RemoveFromService;
                if (item.RemoveFromServiceDate != null && item.RemoveFromServiceDate == default(DateTime))
                {
                    eSearchModel.RemoveFromServiceDate = null;
                }
                else
                {
                    eSearchModel.RemoveFromServiceDate = item.RemoveFromServiceDate;
                }
                eSearchModel.BusinessType = userData.DatabaseKey.Client.BusinessType;
                eSearchModelList.Add(eSearchModel);
            }
            ViewBag.Start = skip;
            ViewBag.TotalRecords = recordsFiltered;
            ViewBag.Length = length;
            ViewBag.CurrentPage = currentpage;

            Parallel.ForEach(cardData, item =>
            {
                item.ImageUrl = EquipmentImageUrl(item.EquipmentId);
            });
            objComb.EquipmentCardList = eSearchModelList;
            EquipmentModel datas = new EquipmentModel();
            objComb.EquipModel = datas;
            this.GetAssetGroupHeaderName(objComb);
            LocalizeControls(objComb, LocalizeResourceSetConstants.EquipmentDetails);
            return PartialView("~/Views/Equipment/_EquipmentGridCardView.cshtml", objComb);
        }
        public string EquipmentImageUrl(long EquipmentId)
        {
            string ImageUrl = string.Empty;
            CommonWrapper objCommonWrapper = new CommonWrapper(userData);
            bool ClientOnPremise = userData.DatabaseKey.Client.OnPremise;
            if (ClientOnPremise)
            {
                ImageUrl = objCommonWrapper.GetOnPremiseImageUrl(EquipmentId, AttachmentTableConstant.Equipment);
                ImageUrl = UtilityFunction.PhotoBase64ImgSrc(ImageUrl);
            }
            else
            {
                ImageUrl = objCommonWrapper.GetAzureImageUrl(EquipmentId, AttachmentTableConstant.Equipment);
            }
            return ImageUrl;

        }
        #endregion

        #region V2-1202
        /// <summary>
        /// This method is responsible for preparing the data required to display the Asset Model Information Wizard.
        /// It retrieves equipment details, lookup lists, and asset group data asynchronously.
        /// </summary>
        /// <param name="EquipmentId">The ID of the equipment to retrieve details for.</param>
        /// <param name="ClientLookupId">The client lookup ID of the equipment.</param>
        /// <param name="Name">The name of the equipment.</param>
        /// <param name="isRemoveFromService">Indicates whether the equipment is removed from service.</param>
        /// <param name="Status">The status of the equipment.</param>
        /// <returns>A partial view containing the Asset Model Information Wizard.</returns>
        [HttpPost]
        public ActionResult CompleteAssetModelFromWizard(long EquipmentId, string ClientLookupId, string Name, bool isRemoveFromService, string Status)
        {
            // Define tasks for asynchronous data retrieval
            Task taskAllLookUp, taskAssetGroup1LookUp, taskAssetGroup2LookUp, taskAssetGroup3LookUp, taskAssetDetailsById;

            // Initialize wrapper and combined model
            EquipmentWrapper eWrapper = new EquipmentWrapper(userData);
            EquipmentCombined objComb = new EquipmentCombined();
            objComb.AddEquipment = new AddEquipmentModelDynamic();
            objComb.PlantLocation = userData.Site.PlantLocation;

            // Initialize common wrapper and lookup lists
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            List<DataContracts.LookupList> TypeList = new List<DataContracts.LookupList>();
            List<DataContracts.LookupList> AllLookUps = new List<DataContracts.LookupList>();

            #region Asynchronous Data Retrieval
            // Retrieve equipment details
            taskAssetDetailsById = Task.Factory.StartNew(() => objComb.AddEquipment = eWrapper.RetrieveEquipmentDetailsForAssteModelByEquipmentId(EquipmentId));

            // Retrieve all lookup lists
            taskAllLookUp = Task.Factory.StartNew(() => AllLookUps = commonWrapper.GetAllLookUpList());

            // Retrieve asset group data
            List<AssetGroup1Model> astGroup1 = new List<AssetGroup1Model>();
            taskAssetGroup1LookUp = Task.Factory.StartNew(() => astGroup1 = eWrapper.GetAssetGroup1Dropdowndata());

            List<AssetGroup2Model> astGroup2 = new List<AssetGroup2Model>();
            taskAssetGroup2LookUp = Task.Factory.StartNew(() => astGroup2 = eWrapper.GetAssetGroup2Dropdowndata());

            List<AssetGroup3Model> astGroup3 = new List<AssetGroup3Model>();
            taskAssetGroup3LookUp = Task.Factory.StartNew(() => astGroup3 = eWrapper.GetAssetGroup3Dropdowndata());

            // Wait for all tasks to complete
            Task.WaitAll(taskAssetDetailsById, taskAllLookUp, taskAssetGroup1LookUp, taskAssetGroup2LookUp, taskAssetGroup3LookUp);
            #endregion

            // Set business type and asset category list
            objComb.BusinessType = userData.DatabaseKey.Client.BusinessType;
            var AssetCategoryList = UtilityFunction.AssetCategoryList();
            if (AssetCategoryList != null)
            {
                objComb.AssetCategoryList = AssetCategoryList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
            }

            // Populate asset group lists
            if (astGroup1 != null)
            {
                objComb.AssetGroup1List = astGroup1.Select(x => new SelectListItem { Text = x.AssetGroup1DescWithClientLookupId, Value = x.AssetGroup1Id.ToString() });
            }
            if (astGroup2 != null)
            {
                objComb.AssetGroup2List = astGroup2.Select(x => new SelectListItem { Text = x.AssetGroup2DescWithClientLookupId, Value = x.AssetGroup2Id.ToString() });
            }
            if (astGroup3 != null)
            {
                objComb.AssetGroup3List = astGroup3.Select(x => new SelectListItem { Text = x.AssetGroup3DescWithClientLookupId, Value = x.AssetGroup3Id.ToString() });
            }

            // Retrieve UI configuration details
            objComb.UIConfigurationDetails = new RetrieveDataForUIConfiguration()
                                                .Retrieve(DataDictionaryViewNameConstant.AddAsset, userData);

            // Filter required lookup lists
            IList<string> LookupNames = objComb.UIConfigurationDetails.ToList()
                                            .Where(x => x.LookupType == "LookupList" && !string.IsNullOrEmpty(x.LookupName))
                                            .Select(s => s.LookupName)
                                            .ToList();
            if (AllLookUps != null)
            {
                objComb.AllRequiredLookUplist = AllLookUps.Where(x => LookupNames.Contains(x.ListName))
                                                          .Select(s => new UILookupList
                                                          { text = s.ListValue + " - " + s.Description, value = s.ListValue, lookupname = s.ListName })
                                                          .ToList();
            }
            // Populate additional data for the view
            objComb._userdata = userData;
            objComb._EquipmentSummaryModel = GetEquipmentSummary(EquipmentId, ClientLookupId, Name, isRemoveFromService, Status);
            objComb.AssetGroup1Label = userData.Site.AssetGroup1Name;
            objComb.AssetGroup2Label = userData.Site.AssetGroup2Name;
            objComb.AssetGroup3Label = userData.Site.AssetGroup3Name;

            // Retrieve the list of asset categories
            var AssetCategoryForModelList = UtilityFunction.AssetCategoryList();

            // Check if the list is not null
            if (AssetCategoryForModelList != null)
            {
                // Filter the list to include only items where the value is "Equipment"
                // and map it to a SelectListItem for UI dropdowns
                objComb.AssetCategory1List = AssetCategoryForModelList
                    .Where(m => m.value == "Equipment")
                    .Select(x => new SelectListItem
                    {
                        Text = x.text,
                        Value = x.value.ToString()
                    });
            }
            // Localize controls and return the partial view
            LocalizeControls(objComb, LocalizeResourceSetConstants.EquipmentDetails);
            return PartialView("~/Views/Equipment/AssetModelWizard/_AssetModelInformationWizard.cshtml", objComb);
        }


        /// <summary>
        /// Handles the completion of the asset model batch process from the wizard.
        /// Validates the model state, processes the asset model, and returns the result as JSON.
        /// </summary>
        /// <param name="objEM">The combined equipment model containing asset model data.</param>
        /// <param name="Command">The command indicating the action to perform.</param>
        /// <returns>A JSON result indicating success or failure, along with any error messages.</returns>
        [ValidateAntiForgeryToken]
        public JsonResult CompleteAssetModelBatchFromWizard(EquipmentCombined objEM, string Command)
        {
            // Initialize the EquipmentWrapper for handling equipment-related operations
            EquipmentWrapper eWrapper = new EquipmentWrapper(userData);
            string ModelValidationFailedMessage = string.Empty;

            // Check if the model state is valid
            if (ModelState.IsValid)
            {
                // Initialize an Equipment object to store the result
                Equipment equipment = new Equipment();

                // Add a new asset model dynamically using the provided data
                equipment = eWrapper.AddNewAssetModelDynamic(objEM);

                // Check if there are any error messages returned
                if (equipment.ErrorMessages != null && equipment.ErrorMessages.Count > 0)
                {
                    // Return the error messages as a JSON response
                    return Json(equipment.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    // Return a success response with the equipment ID
                    return Json(new { Result = JsonReturnEnum.success.ToString(), Command = Command, EquipmentId = equipment.EquipmentId }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                // Retrieve a localized validation failure message
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);

                // Return the validation failure message as a JSON response
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }
        /// <summary>
        /// Generates a dynamic hierarchy tree for Work Order Asset Models.
        /// This method retrieves all equipment, filters out inactive ones, and organizes them into a hierarchical structure.
        /// </summary>
        /// <returns>A partial view containing the hierarchy tree of Work Order Asset Models.</returns>
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult WorkOrderAssetModelHierarchyTreeDynamic()
        {
            // Initialize a hash set to store parent equipment
            HashSet<Equipment> P_Equipment = new HashSet<Equipment>();

            // Retrieve all equipment from the database
            Equipment EquipmentLists = new Equipment();
            List<Equipment> equipments = EquipmentLists.GetAllEquipment(this.userData.DatabaseKey)
                .Where(w => w.InactiveFlag == false) // Filter out inactive equipment
                .OrderBy(o => o.ClientLookupId) // Sort by ClientLookupId
                .ToList();

            // Iterate through the top-level equipment (ParentId = 0)
            foreach (var item in equipments.Where(w => w.ParentId == 0))
            {
                // Create a new Equipment object for the parent
                Equipment objEquip = new Equipment();
                objEquip.EquipmentId = item.EquipmentId;
                objEquip.ParentId = item.ParentId;
                objEquip.ClientLookupId = item.ClientLookupId + " ( " + item.Name + " )";

                // Add the parent equipment to the hash set
                P_Equipment.Add(objEquip);

                // Recursively add child equipment to the hierarchy
                SetLevel(item, equipments, P_Equipment);
            }

            // Convert the hash set to a list
            var x = P_Equipment.ToList();

            // Prepare the view model for the hierarchy tree
            WorkOrderVM workOrderVM = new WorkOrderVM();
            WoEquipmentTreeModel objWoEquipmentTreeModel = new WoEquipmentTreeModel();
            objWoEquipmentTreeModel.Children = x;
            workOrderVM.woEquipmentTreeModel = objWoEquipmentTreeModel;

            // Localize the controls for the view
            LocalizeControls(workOrderVM, LocalizeResourceSetConstants.PurchaseRequest);

            // Return the partial view with the hierarchy tree
            return PartialView("~/Views/Equipment/AssetModelWizard/_WorkOrderAssetModelTreeDynamic.cshtml", workOrderVM);
        }
        private void SetLevel(Equipment equipment, List<Equipment> eList, HashSet<Equipment> hashSet)
        {
            var x = eList.Where(w => w.ParentId == equipment.EquipmentId).OrderBy(o => o.ClientLookupId);
            if (x == null)
                return;
            else
            {
                foreach (var item in x)
                {
                    Equipment objEquip = new Equipment();
                    objEquip.EquipmentId = item.EquipmentId;
                    objEquip.ParentId = item.ParentId;
                    objEquip.ClientLookupId = item.ClientLookupId + " ( " + item.Name + " )";
                    hashSet.Add(objEquip);
                    SetLevel(item, eList, hashSet);
                }
            }
            return;
        }
        #endregion
    }
}