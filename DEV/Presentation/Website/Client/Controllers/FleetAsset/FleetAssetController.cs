using Client.ActionFilters;
using Client.BusinessWrapper;
using Client.BusinessWrapper.Common;
using Client.BusinessWrapper.FleetAsset;
using Client.BusinessWrapper.FleetService;
using Client.Common;
using Client.Controllers.Common;
using Client.Models;
using Client.Models.FleetAsset;
using Client.Models.FleetService;
using Common.Constants;
using DataContracts;
using Newtonsoft.Json;
using QRCoder;

using Rotativa;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Results;
using System.Web.Mvc;
using static Client.Models.Common.UserMentionDataModel;

namespace Client.Controllers.FleetAsset
{
    public class FleetAssetController : SomaxBaseController
    {
        #region Fleet Asset Search
        [CheckUserSecurity(securityType = SecurityConstants.Fleet_Assets)]
        public ActionResult Index()
        {
            FleetAssetWrapper fltAstWrapper = new FleetAssetWrapper(userData);
            FleetAssetVM fltAstVM = new FleetAssetVM();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            fltAstVM.FleetAssetModel = new FleetAssetModel();
            fltAstVM.security = this.userData.Security;
            List<DataContracts.LookupList> VehicleTypeList = new List<DataContracts.LookupList>();
            List<DataContracts.LookupList> AllLookUps = new List<DataContracts.LookupList>();
            var StatusList = UtilityFunction.InactiveActiveStatusTypesforFleetAsset();
            if (StatusList != null)
            {
                fltAstVM.InactiveFlagList = StatusList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
            }
            AllLookUps = commonWrapper.GetAllLookUpList();
            if (AllLookUps != null)
            {
                VehicleTypeList = AllLookUps.Where(x => x.ListName == LookupListConstants.FA_VEHICLETYPE).ToList();
                if (VehicleTypeList != null)
                {
                    fltAstVM.LookupVehicleTypeList = VehicleTypeList.Select(x => new SelectListItem { Text = x.ListValue + " - " + x.Description, Value = x.ListValue }).ToList();
                }

            }
            var FleetAvailability = commonWrapper.GetListFromConstVals(LookupListConstants.Asset_Availability);
            if (FleetAvailability != null)
            {
                fltAstVM.LookupAssetAvailability = FleetAvailability.Select(x => new SelectListItem { Text = x.text.ToString(), Value = x.value.ToString() });
            }
            LocalizeControls(fltAstVM, LocalizeResourceSetConstants.EquipmentDetails);
            return View(fltAstVM);
        }


        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public string GetFleetAssetGridData(int? draw, int? start, int? length, int customQueryDisplayId = 1, string ClientLookupId = "", string Name = "", string Make = "", string Model = "", string VIN = "", string VehicleType = "",string AssetAvailability="", string SearchText = "", string order = "2", string orderDir = "asc")
        {

            List<FleetAssetSearchModel> fltAstSearchModelList = new List<FleetAssetSearchModel>();
            //string order = Request.Form.GetValues("order[0][column]")[0];
            //string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            var colname = Request.Form.GetValues("columns[" + order + "][name]");
            SearchText = SearchText.Replace("%", "[%]");
            ClientLookupId = ClientLookupId.Replace("%", "[%]");
            Name = Name.Replace("%", "[%]");
            Make = Make.Replace("%", "[%]");
            Model = Model.Replace("%", "[%]");
            VIN = VIN.Replace("%", "[%]");
            start = start.HasValue
               ? start / length
               : 0;
            int skip = start * length ?? 0;
            List<string> typeList = new List<string>();
            FleetAssetWrapper fltAstWrapper = new FleetAssetWrapper(userData);
            List<FleetAssetSearchModel> fleetAssetList = fltAstWrapper.GetFleetAssetGridData(customQueryDisplayId, order, orderDir, skip, length ?? 0, ClientLookupId, Name, Make, Model, VIN, VehicleType, AssetAvailability, SearchText);
            var totalRecords = 0;
            var recordsFiltered = 0;
            if (fleetAssetList != null && fleetAssetList.Count > 0)
            {
                recordsFiltered = fleetAssetList[0].TotalCount;
                totalRecords = fleetAssetList[0].TotalCount;
            }
            int initialPage = start.Value;
            var filteredResult = fleetAssetList
              .ToList();
            
            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult }, JsonSerializerDateSettings);
        }


        public string GetFleetAssetPrintData(string _colname, string _coldir, int? draw, int? start, int? length, int customQueryDisplayId = 1, string _ClientLookupId = "", string _Name = "", string _Make = "", string _Model = "", string _VIN = "", string _VehicleType = "",string AssetAvailability="", string _searchText = "")
        {
            FleetAssetPrintModel objFleetAssetPrintModel;
            List<FleetAssetPrintModel> FleetAssetPrintModelList = new List<FleetAssetPrintModel>();
            _searchText = _searchText.Replace("%", "[%]");
            _ClientLookupId = _ClientLookupId.Replace("%", "[%]");
            _Name = _Name.Replace("%", "[%]");
            _Make = _Make.Replace("%", "[%]");
            _Model = _Model.Replace("%", "[%]");
            _VIN = _VIN.Replace("%", "[%]");
            start = start.HasValue
               ? start / length
               : 0;
            int skip = start * length ?? 0;
            int lengthForPrint = 100000;

            FleetAssetWrapper fltAstWrapper = new FleetAssetWrapper(userData);
            List<FleetAssetSearchModel> fleetAssetList = fltAstWrapper.GetFleetAssetGridData(customQueryDisplayId, _colname, _coldir, 0, lengthForPrint, _ClientLookupId, _Name, _Make, _Model, _VIN, _VehicleType, AssetAvailability, _searchText);
            foreach (var item in fleetAssetList)
            {
                objFleetAssetPrintModel = new FleetAssetPrintModel();
                objFleetAssetPrintModel.ClientLookupId = item.ClientLookupId;
                objFleetAssetPrintModel.Name = item.Name;
                objFleetAssetPrintModel.VIN = item.VIN;
                objFleetAssetPrintModel.VehicleType = item.VehicleType;
                if (item.RemoveFromService == false)
                {
                    objFleetAssetPrintModel.RemoveFromService = AssetAvailabilityStatusConstant.InService;
                }
                else
                {
                    objFleetAssetPrintModel.RemoveFromService = AssetAvailabilityStatusConstant.OutService;
                }
                if (item.RemoveFromServiceDate != null && item.RemoveFromServiceDate != default(DateTime))
                {
                    objFleetAssetPrintModel.RemoveFromServiceDate = item.RemoveFromServiceDate.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                }
                else
                {
                    objFleetAssetPrintModel.RemoveFromServiceDate = "";
                }
                objFleetAssetPrintModel.VehicleYear = item.VehicleYear;
                objFleetAssetPrintModel.Make = item.Make;
                objFleetAssetPrintModel.Model = item.Model;
                FleetAssetPrintModelList.Add(objFleetAssetPrintModel);
            }
            return JsonConvert.SerializeObject(new { data = FleetAssetPrintModelList }, JsonSerializerDateSettings);
        }

        #endregion

        #region Fleet Asset Details
        public PartialViewResult FleetAssetDetails(long EquipmentId)
        {
            FleetAssetVM fltAstVM = new FleetAssetVM();
            ChangeFleetAssetIDModel _ChangeFleetAssetIDModel = new ChangeFleetAssetIDModel();
            AssetAvailabilityModel _AssetAvailabilityModel = new AssetAvailabilityModel();
            FleetAssetWrapper fltAstWrapper = new FleetAssetWrapper(userData);
            var FleetAssetDetails = fltAstWrapper.GetFleetAssetDetailsById(EquipmentId);
            FleetAssetModel fleetAssetModel = new FleetAssetModel();
            Task attTask;
            CommonWrapper objCommonWrapper = new CommonWrapper(userData);
            attTask = Task.Factory.StartNew(() => fltAstVM.attachmentCount = objCommonWrapper.AttachmentCount(EquipmentId, AttachmentTableConstant.Equipment, userData.Security.Equipment.Edit));
            fltAstVM.FleetAssetModel = FleetAssetDetails;
            fltAstVM.FleetAssetModel.EquID = EquipmentId;
            fltAstVM._FleetAssetSummaryModel = GetFleetAssetSummary(EquipmentId, fltAstVM.FleetAssetModel.ClientLookupId, fltAstVM.FleetAssetModel.Name);
            fltAstVM._FleetAssetSummaryModel.RemoveFromService = fltAstVM.FleetAssetModel.RemoveFromService;
            _ChangeFleetAssetIDModel.EquipmentId = Convert.ToInt64(fltAstVM.FleetAssetModel.EquipmentID);
            _ChangeFleetAssetIDModel.ClientLookupId = fltAstVM.FleetAssetModel.ClientLookupId;
            _ChangeFleetAssetIDModel.UpdateIndex = fltAstVM.FleetAssetModel.UpdateIndex;
            fltAstVM._ChangeFleetAssetIDModel = _ChangeFleetAssetIDModel;
            fltAstVM._CreatedLastUpdatedFleetAssetModel = fltAstWrapper.createdLastUpdatedModel(EquipmentId);
            fltAstVM.security = this.userData.Security;
            string temp = string.Empty;
            temp = FleetAssetDetails.ClientLookupId + "][" + FleetAssetDetails.Name + "][" + FleetAssetDetails.SerialNumber + "][" + FleetAssetDetails.Make + "][" + FleetAssetDetails.ModelNumber;
            FleetAssetQRCodeModel qRCodeModel = new FleetAssetQRCodeModel();
            List<string> equipmentClientLookUpNames = new List<string>();
            equipmentClientLookUpNames.Add(temp);
            qRCodeModel.EquipmentIdsList = equipmentClientLookUpNames;
            _AssetAvailabilityModel.EquipmentId = Convert.ToInt64(fltAstVM.FleetAssetModel.EquipmentID);
            _AssetAvailabilityModel.RemoveFromService = fltAstVM.FleetAssetModel.RemoveFromService;
            //_AssetAvailabilityModel.ExpectedReturnToService = fltAstVM.FleetAssetModel.ExpectedReturnToService;
            fltAstVM._AssetAvailabilityModel = _AssetAvailabilityModel;
            fltAstVM.qRCodeModel = qRCodeModel;
            fltAstVM._userdata = this.userData;
            attTask.Wait();
            fltAstVM.FleetAssetModel.IsAssetAvailability = userData.Security.Asset_Availability.Access;
            LocalizeControls(fltAstVM, LocalizeResourceSetConstants.EquipmentDetails);
            return PartialView("_FleetAssetDetails", fltAstVM);
        }
        #endregion

        #region Fleet Asset Add-Edit
        public PartialViewResult AddFleetAsset()
        {
            FleetAssetWrapper faWrapper = new FleetAssetWrapper(userData);
            FleetAssetVM fltAstVM = new FleetAssetVM();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            List<DataContracts.LookupList> VehicleTypeList = new List<DataContracts.LookupList>();
            List<DataContracts.LookupList> FuelTypeList = new List<DataContracts.LookupList>();
            List<DataContracts.LookupList> AllLookUps = new List<DataContracts.LookupList>();
            fltAstVM.FleetAssetModel = new FleetAssetModel();

            AllLookUps = commonWrapper.GetAllLookUpList();
            if (AllLookUps != null)
            {
                VehicleTypeList = AllLookUps.Where(x => x.ListName == LookupListConstants.FA_VEHICLETYPE).ToList();
                if (VehicleTypeList != null)
                {
                    fltAstVM.LookupVehicleTypeList = VehicleTypeList.Select(x => new SelectListItem { Text = x.ListValue + " - " + x.Description, Value = x.ListValue }).ToList();
                }

                FuelTypeList = AllLookUps.Where(x => x.ListName == LookupListConstants.FA_FUELTYPE).ToList();
                if (FuelTypeList != null)
                {
                    fltAstVM.LookupFuelTypeList = FuelTypeList.Select(x => new SelectListItem { Text = x.ListValue + " - " + x.Description, Value = x.ListValue }).ToList();
                }

                var FleetFuelUnits = commonWrapper.GetListFromConstVals(LookupListConstants.FLEET_FUEL_UNITS);
                if (FleetFuelUnits != null)
                {
                    fltAstVM.LookupFleetFuelUnits = FleetFuelUnits.Select(x => new SelectListItem { Text = x.text.ToString(), Value = x.value.ToString() });
                }
                // RKL - 2021-Feb-13 - Both types should show up in both columns
                var FleetMeterTypes = commonWrapper.GetListFromConstVals(LookupListConstants.FLEET_METER_TYPE);                
                if (FleetMeterTypes != null)
                {
                    fltAstVM.LookupFleetMeter1Types = FleetMeterTypes.Select(x => new SelectListItem { Text = x.text.ToString(), Value = x.value.ToString() });
                    fltAstVM.LookupFleetMeter2Types = FleetMeterTypes.Select(x => new SelectListItem { Text = x.text.ToString(), Value = x.value.ToString() });
                    //fltAstVM.LookupFleetMeter1Types = FleetMeterTypes.Select(x => new SelectListItem { Text = x.text.ToString(), Value = x.value.ToString() }).Where(x => x.Text == "Odometer");
                    //fltAstVM.LookupFleetMeter2Types = FleetMeterTypes.Select(x => new SelectListItem { Text = x.text.ToString(), Value = x.value.ToString() }).Where(x => x.Text == "Hour");
                }
                // RKL - 2021-Feb-13 - Both types should show up in both columns
                var FleetMeterUnits = commonWrapper.GetListFromConstVals(LookupListConstants.FLEET_METER_UNITS);
                if (FleetMeterUnits != null)
                {
                    fltAstVM.LookupFleetMeter1Units = FleetMeterUnits.Select(x => new SelectListItem { Text = x.text.ToString(), Value = x.value.ToString() });
                    fltAstVM.LookupFleetMeter2Units = FleetMeterUnits.Select(x => new SelectListItem { Text = x.text.ToString(), Value = x.value.ToString() });
                    //fltAstVM.LookupFleetMeter1Units = FleetMeterUnits.Select(x => new SelectListItem { Text = x.text.ToString(), Value = x.value.ToString() }).Where(x => x.Text != "Hours");
                    //fltAstVM.LookupFleetMeter2Units = FleetMeterUnits.Select(x => new SelectListItem { Text = x.text.ToString(), Value = x.value.ToString() }).Where(x => x.Text == "Hours");
                }
                var FleetReadingSourceType = commonWrapper.GetListFromConstVals(LookupListConstants.FLEET_READING_SOURCE_TYPE);
                if (FleetReadingSourceType != null)
                {
                    fltAstVM.LookupFleetReadingSourceType = FleetReadingSourceType.Select(x => new SelectListItem { Text = x.text.ToString(), Value = x.value.ToString() });
                }

            }
            LocalizeControls(fltAstVM, LocalizeResourceSetConstants.EquipmentDetails);
            return PartialView("~/Views/FleetAsset/_FleetAssetAdd.cshtml", fltAstVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddFleetAsset(FleetAssetVM objFA, string Command)
        {
            List<string> ErrorList = new List<string>();
            FleetAssetWrapper faWrapper = new FleetAssetWrapper(userData);
            string ModelValidationFailedMessage = string.Empty;

            if (ModelState.IsValid)
            {
                Equipment equipment = new Equipment();
                string FA_ClientLookupId = objFA.FleetAssetModel.EquipmentID.ToUpper().Trim();
                equipment = faWrapper.AddFleetAsset(FA_ClientLookupId, objFA);
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
        public ActionResult EditFleetAsset(long EquipmentId, string ClientLookupId, string Name)
        {
            FleetAssetWrapper eWrapper = new FleetAssetWrapper(userData);
            FleetAssetVM fltAstVM = new FleetAssetVM();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            List<DataContracts.LookupList> VehicleTypeList = new List<DataContracts.LookupList>();
            List<DataContracts.LookupList> FuelTypeList = new List<DataContracts.LookupList>();
            List<DataContracts.LookupList> AllLookUps = new List<DataContracts.LookupList>();
            fltAstVM.FleetAssetModel = new FleetAssetModel();
            fltAstVM.FleetAssetModel = eWrapper.GetEditFleetAssetDetailsById(EquipmentId);
            fltAstVM._userdata = userData;
            fltAstVM._FleetAssetSummaryModel = GetFleetAssetSummary(EquipmentId, ClientLookupId, Name);
            AllLookUps = commonWrapper.GetAllLookUpList();
            if (AllLookUps != null)
            {
                VehicleTypeList = AllLookUps.Where(x => x.ListName == LookupListConstants.FA_VEHICLETYPE).ToList();
                if (VehicleTypeList != null)
                {
                    fltAstVM.LookupVehicleTypeList = VehicleTypeList.Select(x => new SelectListItem { Text = x.ListValue + " - " + x.Description, Value = x.ListValue }).ToList();
                }

                FuelTypeList = AllLookUps.Where(x => x.ListName == LookupListConstants.FA_FUELTYPE).ToList();
                if (FuelTypeList != null)
                {
                    fltAstVM.LookupFuelTypeList = FuelTypeList.Select(x => new SelectListItem { Text = x.ListValue + " - " + x.Description, Value = x.ListValue }).ToList();
                }
                var FleetFuelUnits = commonWrapper.GetListFromConstVals(LookupListConstants.FLEET_FUEL_UNITS);
                if (FleetFuelUnits != null)
                {
                    fltAstVM.LookupFleetFuelUnits = FleetFuelUnits.Select(x => new SelectListItem { Text = x.text.ToString(), Value = x.value.ToString() });
                }
                var FleetMeterTypes = commonWrapper.GetListFromConstVals(LookupListConstants.FLEET_METER_TYPE);
                if (FleetMeterTypes != null)
                {
                    fltAstVM.LookupFleetMeter1Types = FleetMeterTypes.Select(x => new SelectListItem { Text = x.text.ToString(), Value = x.value.ToString() }).Where(x => x.Text == "Odometer");
                    fltAstVM.LookupFleetMeter2Types = FleetMeterTypes.Select(x => new SelectListItem { Text = x.text.ToString(), Value = x.value.ToString() }).Where(x => x.Text == "Hour");
                }
                var FleetMeterUnits = commonWrapper.GetListFromConstVals(LookupListConstants.FLEET_METER_UNITS);
                if (FleetMeterUnits != null)
                {
                    fltAstVM.LookupFleetMeter1Units = FleetMeterUnits.Select(x => new SelectListItem { Text = x.text.ToString(), Value = x.value.ToString() }).Where(x => x.Text != "Hours");
                    fltAstVM.LookupFleetMeter2Units = FleetMeterUnits.Select(x => new SelectListItem { Text = x.text.ToString(), Value = x.value.ToString() }).Where(x => x.Text == "Hours");
                }
                var FleetReadingSourceType = commonWrapper.GetListFromConstVals(LookupListConstants.FLEET_READING_SOURCE_TYPE);
                if (FleetReadingSourceType != null)
                {
                    fltAstVM.LookupFleetReadingSourceType = FleetReadingSourceType.Select(x => new SelectListItem { Text = x.text.ToString(), Value = x.value.ToString() });
                }

            }
            LocalizeControls(fltAstVM, LocalizeResourceSetConstants.EquipmentDetails);

            return PartialView("_FleetAssetEdit", fltAstVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateFleetAssetInfo(FleetAssetVM equip)
        {
            string emptyValue = string.Empty;
            FleetAssetVM objComb = new FleetAssetVM();
            FleetAssetWrapper faWrapper = new FleetAssetWrapper(userData);
            Equipment equipment = new Equipment();

            if (ModelState.IsValid)
            {
                equipment = faWrapper.UpdateFleetAsset(equip);
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

        #region common functionalities 
        [HttpPost]
        public JsonResult ValidateFleetAssetStatusChange(long _eqid, bool inactiveFlag, string clientLookupId)
        {
            FleetAssetWrapper fltAstWrapper = new FleetAssetWrapper(userData);
            Equipment equipment = new Equipment();
            string flag = ActivationStatusConstant.InActivate;
            if (inactiveFlag)
            {
                flag = ActivationStatusConstant.Activate;
            }
            equipment = fltAstWrapper.ValidateFltAstStatusChange(_eqid, flag, clientLookupId);
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
        public JsonResult UpdateFleetAssetStatus(long _eqid, bool inactiveFlag)
        {
            Equipment equipment = new Equipment();
            FleetAssetWrapper fltAstWrapper = new FleetAssetWrapper(userData);
            var errMsg = fltAstWrapper.UpdateFltAstActiveStatus(_eqid, inactiveFlag);
            if (errMsg != null && errMsg.Count > 0)
            {
                return Json(equipment.ErrorMessages, JsonRequestBehavior.AllowGet);
            }
            else
            {
                errMsg = fltAstWrapper.CreateAssetEvent(_eqid, inactiveFlag);
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
        [HttpPost]
        public JsonResult ChangeFleetAssetId(long _eqid, string eqlookupid)
        {
            FleetAssetVM objComb = new FleetAssetVM();
            CreatedLastUpdatedFleetAssetModel _CreatedLastUpdatedFleetAssetModel = new CreatedLastUpdatedFleetAssetModel();
            ChangeFleetAssetIDModel _ChangeFleetAssetIDModel = new ChangeFleetAssetIDModel();
            FleetAssetWrapper faWrapper = new FleetAssetWrapper(userData);
            var EquipmentDetails = faWrapper.GetFltAstDetailsById(_eqid);
            FleetAssetModel fleetAssetModel = new FleetAssetModel();
            objComb.FleetAssetData = EquipmentDetails;
            objComb._FleetAssetSummaryModel.Equipment_ClientLookupId = objComb.FleetAssetData.ClientLookupId;
            objComb._FleetAssetSummaryModel.EquipmentName = objComb.FleetAssetData.Name;
            objComb._FleetAssetSummaryModel.ImageUrl = objComb.FleetAssetData.ImageUrl;
            _ChangeFleetAssetIDModel.EquipmentId = Convert.ToInt64(objComb.FleetAssetData.EquipmentId);
            _ChangeFleetAssetIDModel.ClientLookupId = eqlookupid;
            _ChangeFleetAssetIDModel.UpdateIndex = objComb.FleetAssetData.UpdateIndex;
            objComb.FleetAssetModel = fleetAssetModel;
            objComb._ChangeFleetAssetIDModel = _ChangeFleetAssetIDModel;
            objComb._CreatedLastUpdatedFleetAssetModel = faWrapper.createdLastUpdatedModel(_eqid);
            string result = string.Empty;
            List<String> errorList = new List<string>();
            FleetAssetModel objFA = new FleetAssetModel();
            errorList = faWrapper.ChangeFltAstId(_eqid, _ChangeFleetAssetIDModel);
            if (errorList != null && errorList.Count > 0)
            {
                return Json(errorList, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Result = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

        #region QR Fleet Asset
        [HttpPost]
        public PartialViewResult FleetAssetDetailsQRcode(string[] EquipClientLookups)
        {
            FleetAssetVM objComb = new FleetAssetVM();
            FleetAssetQRCodeModel qRCodeModel = new FleetAssetQRCodeModel();
            List<string> equipmentClientLookUpNames = new List<string>();
            foreach (var e in EquipClientLookups)
            {
                equipmentClientLookUpNames.Add(Convert.ToString(e));
            }
            qRCodeModel.EquipmentIdsList = equipmentClientLookUpNames;
            objComb.qRCodeModel = qRCodeModel;
            LocalizeControls(objComb, LocalizeResourceSetConstants.EquipmentDetails);
            return PartialView("_FleetAssetDetailsQRCode", objComb);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SetAssetIdlist(FleetAssetVM fleetAssetVM)
        {
            TempData["QRCodeFleetAssetIdList"] = fleetAssetVM.qRCodeModel.EquipmentIdsList;
            return Json(new { JsonReturnEnum.success });
        }
        [EncryptedActionParameter]
        public ActionResult QRCodeGenerationUsingRotativa(bool SmallLabel)
        {
            var fleetAssetVM = new FleetAssetVM();
            var qRCodeModel = new FleetAssetQRCodeModel();

            ViewBag.SmallQR = SmallLabel;
            if (TempData["QRCodeFleetAssetIdList"] != null)
            {
                qRCodeModel.EquipmentIdsList = (List<string>)TempData["QRCodeFleetAssetIdList"];
            }
            else
            {
                qRCodeModel.EquipmentIdsList = new List<string>();
            }
            fleetAssetVM.qRCodeModel = qRCodeModel;

            return new PartialViewAsPdf("_FleetAssetQRCodeTemplate", fleetAssetVM)
            {
                PageMargins = { Left = 1, Right = 1, Top = 1, Bottom = 1 },
                PageHeight = SmallLabel ? 25 : 28,
                PageWidth = SmallLabel ? 54 : 89,
            };
        }
        
        #endregion
        #region Comment       
        [HttpPost]
        public PartialViewResult LoadComments(long EquipmentId)
        {
            FleetAssetVM _FleetAssetObj = new FleetAssetVM();
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
                _FleetAssetObj.userMentionDatas = userMentionDatas;
            }
            if (!tasks[1].IsFaulted && tasks[1].IsCompleted)
            {
                _FleetAssetObj.NotesList = NotesList;
            }
            LocalizeControls(_FleetAssetObj, LocalizeResourceSetConstants.EquipmentDetails);
            return PartialView("_CommentsList", _FleetAssetObj);
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
                    objUserMentionData = new UserMentionData(); // UserMentionData();
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
                errorList = coWrapper.AddOrUpdateComment(notesModel, ref Mode, "Equipment", userMentionDataList);
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
            var AttachmentList = objCommonWrapper.PopulateAttachments(EquipmentId, "Equipment", userData.Security.Equipment.Edit);
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

            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult }, JsonSerializer12HoursDateAndTimeSettings);
        }
        [HttpGet]
        public ActionResult ShowAddAttachment(long EquipmentId, string ClientLookupId, string Name)
        {
            FleetAssetVM _FleetAssetObj = new FleetAssetVM();
            FleetAssetWrapper FAWrapper = new FleetAssetWrapper(userData);
            _FleetAssetObj.FleetAssetModel = new FleetAssetModel();
            _FleetAssetObj.FleetAssetModel = FAWrapper.GetEditFleetAssetDetailsById(EquipmentId);
            _FleetAssetObj._FleetAssetSummaryModel = GetFleetAssetSummary(EquipmentId, ClientLookupId, Name);
            AttachmentModel Attachment = new AttachmentModel();
            Attachment.EquipmentId = EquipmentId;
            Attachment.ClientLookupId = ClientLookupId;
            _FleetAssetObj.attachmentModel = Attachment;
            _FleetAssetObj._userdata = userData;
            LocalizeControls(_FleetAssetObj, LocalizeResourceSetConstants.EquipmentDetails);
            return PartialView("_AttachmentAdd", _FleetAssetObj);
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
                attachmentModel.TableName = "Equipment";
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
                        return Json(new { Result = JsonReturnEnum.success.ToString(), equipmentid = Convert.ToInt64(Request.Form["attachmentModel.EquipmentId"]) }, JsonRequestBehavior.AllowGet);
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

        #region Parts
        [HttpPost]
        public string GetFleetAsset_Parts(int? draw, int? start, int? length, long EquipmentId)
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            FleetAssetWrapper faWrapper = new FleetAssetWrapper(userData);
            var Parts = faWrapper.GetFleetAssetParts(EquipmentId);
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
            var partSecurity = this.userData.Security.Parts.Part_Equipment_XRef;
            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult, partSecurity = partSecurity }, JsonSerializer12HoursDateAndTimeSettings);
        }
        private List<FleetAssetPartsModel> GetAllPartsSortByColumnWithOrder(string order, string orderDir, List<FleetAssetPartsModel> data)
        {
            List<FleetAssetPartsModel> lst = new List<FleetAssetPartsModel>();

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
        public ActionResult PartsAdd(long EquipmentId, string ClientLookupId, string Name)
        {
            FleetAssetVM objComb = new FleetAssetVM();
            PartsSessionData objPartsSessionData = new PartsSessionData();
            FleetAssetWrapper FAWrapper = new FleetAssetWrapper(userData);
            objComb.FleetAssetModel = new FleetAssetModel();
            objComb.FleetAssetModel = FAWrapper.GetEditFleetAssetDetailsById(EquipmentId);
            objComb._FleetAssetSummaryModel = GetFleetAssetSummary(EquipmentId, ClientLookupId, Name);
            objPartsSessionData.EquipmentId = EquipmentId;
            objComb.partsSessionData = objPartsSessionData;
            objComb._userdata = this.userData;
            LocalizeControls(objComb, LocalizeResourceSetConstants.EquipmentDetails);
            return PartialView("_PartsAdd", objComb);
        }
        public ActionResult PartsEdit(long EquipmentId, long Equipment_Parts_XrefId, string ClientLookupId, string Name)
        {
            FleetAssetVM objComb = new FleetAssetVM();
            FleetAssetWrapper FAWrapper = new FleetAssetWrapper(userData);
            PartsSessionData objPartsSessionData = new PartsSessionData();
            objComb.FleetAssetModel = new FleetAssetModel();
            objComb.FleetAssetModel = FAWrapper.GetEditFleetAssetDetailsById(EquipmentId);
            objComb._FleetAssetSummaryModel = GetFleetAssetSummary(EquipmentId, ClientLookupId, Name);
            objComb = FAWrapper.EditParts(EquipmentId, Equipment_Parts_XrefId, objComb);
            objComb._userdata = this.userData;
            LocalizeControls(objComb, LocalizeResourceSetConstants.EquipmentDetails);
            return PartialView("_PartsEdit", objComb);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult PartsEdit(FleetAssetVM ec)
        {
            FleetAssetWrapper FAWrapper = new FleetAssetWrapper(userData);
            string ModelValidationFailedMessage = string.Empty;
            if (ModelState.IsValid)
            {
                ec = FAWrapper.UpdatePart(ec);
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
        public JsonResult PartsAdd(FleetAssetVM ec)
        {
            string ModelValidationFailedMessage = string.Empty;
            FleetAssetWrapper FAWrapper = new FleetAssetWrapper(userData);
            if (ModelState.IsValid)
            {
                ec = FAWrapper.AddPart(ec);

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
            FleetAssetWrapper FAWrapper = new FleetAssetWrapper(userData);
            if (FAWrapper.DeleteParts(_EquipmentPartSpecsId))
            {
                return Json(new { Result = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Result = JsonReturnEnum.failed.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Private method
        private FleetAssetSummaryModel GetFleetAssetSummary(long EquipmentId, string ClientLookupId, string Name)
        {
            int Flag = 1;
            long thisCount = 0;
            FleetAssetSummaryModel summary = new FleetAssetSummaryModel();

            summary.Equipment_ClientLookupId = ClientLookupId;
            summary.EquipmentName = Name;
            FleetAssetWrapper faWrapper = new FleetAssetWrapper(userData);
            CommonWrapper comWrapper = new CommonWrapper(userData);           
            summary.ClientOnPremise = userData.DatabaseKey.Client.OnPremise;
            if (summary.ClientOnPremise)
            {
                summary.ImageUrl = comWrapper.GetOnPremiseImageUrl(EquipmentId, AttachmentTableConstant.Equipment);
            }
            else
            {
                summary.ImageUrl = comWrapper.GetAzureImageUrl(EquipmentId, AttachmentTableConstant.Equipment);
            }
            List <KeyValuePair<string, long>> widgetsCount = DashboardReports.WorkOrder_RetrieveByWOStatusForChart_1(userData.DatabaseKey, Flag, EquipmentId);
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
            return summary;
        }
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
        #endregion

        #region Service Order Retrieve By EquipmentId
        [HttpPost]
        public string GetFleetAsset_ServiceOrder(int? draw, int? start, int? length, long EquipmentId)
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            FleetAssetWrapper faWrapper = new FleetAssetWrapper(userData);
            var ServiceOrder = faWrapper.GetFleetAssetServiceOrder(EquipmentId);
            //Parts = this.GetAllPartsSortByColumnWithOrder(order, orderDir, Parts);
            var totalRecords = 0;
            var recordsFiltered = 0;
            start = start.HasValue
                ? start / length
                : 0;
            recordsFiltered = ServiceOrder.Count();
            totalRecords = ServiceOrder.Count();
            int initialPage = start.Value;
            var filteredResult = ServiceOrder
                .Skip(initialPage * length ?? 0)
                .Take(length ?? 0)
                .ToList();

            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult }, JsonSerializerDateSettings);
        }
        #endregion
        #region ServiceOrder Line Item
        public ActionResult GetServiceOrderInnerGrid(long ServiceOrderID)
        {
            FleetServiceVM objFleetServiceVM = new FleetServiceVM();
            FleetServiceWrapper FSWrapper = new FleetServiceWrapper(userData);
            objFleetServiceVM.FleetServiceLineItemModelList = FSWrapper.PopulateLineitems(ServiceOrderID);
            LocalizeControls(objFleetServiceVM, LocalizeResourceSetConstants.FleetServiceOrder);
            return View("_InnerGridSOLineItem", objFleetServiceVM);
        }
        #endregion

        #region Scheduled Service Retrieve By EquipmentId
        [HttpPost]
        public string GetFleetAsset_ScheduledService(int? draw, int? start, int? length, long EquipmentId)
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            FleetAssetWrapper faWrapper = new FleetAssetWrapper(userData);
            var ScheduledService = faWrapper.GetFleetAssetScheduledService(EquipmentId);
            var totalRecords = 0;
            var recordsFiltered = 0;
            start = start.HasValue
                ? start / length
                : 0;
            recordsFiltered = ScheduledService.Count();
            totalRecords = ScheduledService.Count();
            int initialPage = start.Value;
            var filteredResult = ScheduledService
                .Skip(initialPage * length ?? 0)
                .Take(length ?? 0)
                .ToList();

            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult }, JsonSerializerDateSettings);
        }
        #endregion

        #region Fleet Fuel Retrieve By EquipmentId
        [HttpPost]
        public string GetFleetAsset_FuelTracking(int? draw, int? start, int? length, long EquipmentId)
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            FleetAssetWrapper faWrapper = new FleetAssetWrapper(userData);
            var fuelTracking = faWrapper.GetFleetAssetFuelTracking(EquipmentId);
            var totalRecords = 0;
            var recordsFiltered = 0;
            start = start.HasValue
                ? start / length
                : 0;
            recordsFiltered = fuelTracking.Count();
            totalRecords = fuelTracking.Count();
            int initialPage = start.Value;
            var filteredResult = fuelTracking
                .Skip(initialPage * length ?? 0)
                .Take(length ?? 0)
                .ToList();

            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult }, JsonSerializer12HoursDateAndTimeUptoMinuteSettings);
        }
        #endregion

        #region Fleet Meter Retrieve By EquipmentId
        [HttpPost]
        public string GetFleetAsset_FuelMeter(int? draw, int? start, int? length, long EquipmentId)
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            FleetAssetWrapper faWrapper = new FleetAssetWrapper(userData);
            var fleetMeter = faWrapper.GetFleetAssetMeterReading(EquipmentId);
            var totalRecords = 0;
            var recordsFiltered = 0;
            start = start.HasValue
                ? start / length
                : 0;
            recordsFiltered = fleetMeter.Count();
            totalRecords = fleetMeter.Count();
            int initialPage = start.Value;
            var filteredResult = fleetMeter
                .Skip(initialPage * length ?? 0)
                .Take(length ?? 0)
                .ToList();

            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult }, JsonSerializer12HoursDateAndTimeUptoMinuteSettings);
        }
        #endregion

        #region Fleet Issue Retrieve By EquipmentId
        [HttpPost]
        public string GetFleetAsset_FleetIssue(int? draw, int? start, int? length, long EquipmentId)
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            FleetAssetWrapper faWrapper = new FleetAssetWrapper(userData);
            var fleetMeter = faWrapper.GetFleetAssetFleetIssue(EquipmentId);
            var totalRecords = 0;
            var recordsFiltered = 0;
            start = start.HasValue
                ? start / length
                : 0;
            recordsFiltered = fleetMeter.Count();
            totalRecords = fleetMeter.Count();
            int initialPage = start.Value;
            var filteredResult = fleetMeter
                .Skip(initialPage * length ?? 0)
                .Take(length ?? 0)
                .ToList();

            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult }, JsonSerializer12HoursDateAndTimeUptoMinuteSettings);
        }
        #endregion

        #region Asset Availability
        [HttpPost]
        public ActionResult AssetAvailability(FleetAssetVM equip)
        {
            FleetAssetWrapper faWrapper = new FleetAssetWrapper(userData);
            Equipment equipment = new Equipment();
            if (ModelState.IsValid)
            {
                equipment = faWrapper.AssetAvailability(equip);
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

        [HttpPost]
        public ActionResult ReturnToInservice(long EquipmentId)
        {
            FleetAssetWrapper faWrapper = new FleetAssetWrapper(userData);
            Equipment equipment = new Equipment();
            equipment = faWrapper.ReturnToInservice(EquipmentId);
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
    }

}