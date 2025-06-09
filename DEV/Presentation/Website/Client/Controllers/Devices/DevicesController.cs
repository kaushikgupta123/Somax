using Client.BusinessWrapper;
using Client.BusinessWrapper.Common;
using Client.Common;
using Client.Controllers.Common;
using Client.Models;
using Client.Models.Devices;
using Common.Constants;
using DataContracts;

using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace Client.Controllers.Devices
{
    public class DevicesController : SomaxBaseController
    {
        #region Search
        public ActionResult Index()
        {
            DeviceVM deviceVM = new DeviceVM();
            DeviceModel dvModel = new DeviceModel();
            BusinessWrapper.DevicesWrapper dvWrapper = new BusinessWrapper.DevicesWrapper(userData);
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            string mode = Convert.ToString(TempData["Mode"]);
            if (mode == "DetailFromEquipment")
            {
                int ioTDeviceId = Convert.ToInt32(TempData["IoTDeviceId"]);             
                deviceVM.IsFromEquipment = true;                
                ChangeDeviceIDModel _ChangeDeviceIDModel = new ChangeDeviceIDModel();
                string EquipmentClientLookupId = Convert.ToString(TempData["EquipmentClientLookupId"]);
                long EquipmentId = Convert.ToInt64(TempData["EquipmentId"]);
                var deviceDetails = dvWrapper.GetDevicedetails(ioTDeviceId);
                deviceVM.deviceModel = deviceDetails;
                deviceVM.security = this.userData.Security;
                deviceVM.APMval = this.IsAPMOnly(userData);                
                deviceVM.EquipmentClientLookupId = EquipmentClientLookupId;
                deviceVM.EquipmentId = EquipmentId;
                _ChangeDeviceIDModel.IoTDeviceId = ioTDeviceId;
                _ChangeDeviceIDModel.ClientLookupId = deviceVM.deviceModel.ClientLookupID;
                _ChangeDeviceIDModel.OldClientLookupId = deviceVM.deviceModel.ClientLookupID;
                _ChangeDeviceIDModel.UpdateIndex = deviceVM.deviceModel.UpdateIndex;
                deviceVM.changeDeviceIDModel = _ChangeDeviceIDModel;
                
                LocalizeControls(deviceVM, LocalizeResourceSetConstants.DeviceDetails);             

            }
            else
            {
                var AllLookUps = commonWrapper.GetAllLookUpList();
                if (AllLookUps != null)
                {
                    var OnDemandList = AllLookUps.Where(x => x.ListName == LookupListConstants.IOT_TYPE).ToList();
                    if (OnDemandList != null)
                    {
                        dvModel.SensorTypeList = OnDemandList.Select(x => new SelectListItem { Text = x.ListValue + "  -  " + x.Description, Value = x.ListValue });
                    }
                }
                dvModel.DeviceCategoryList = commonWrapper.PopulateCustomQueryDisplayForDevice("Devices", true);
                deviceVM.deviceModel = dvModel;
                deviceVM.security = this.userData.Security;
                deviceVM.APMval = this.IsAPMOnly(userData);
                LocalizeControls(deviceVM, LocalizeResourceSetConstants.DeviceDetails);
            }
            return View(deviceVM);
        }

        [HttpPost]
        public string GetDeviceMainGrid(int? draw, int? start, int? length, int customQueryDisplayId = 0,
          string deviceClientLookupId = "", string name = "", string assetClentLookupId = "", string assetCategory = "", string sensorType = "", string txtSearchval = "")
        {
            DevicesWrapper dvWrapper = new DevicesWrapper(userData);
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            var colname = Request.Form.GetValues("columns[" + order + "][name]");
            start = start.HasValue
              ? start / length
              : 0;
            int skip = start * length ?? 0;
            List<DeviceModel> deviceModelList = dvWrapper.GetDeviceGridData(customQueryDisplayId, out IEnumerable<SelectListItem> AssetCategoryList, out IEnumerable<SelectListItem> SensorTypeList, skip, length ?? 0, colname[0], orderDir, deviceClientLookupId, name, assetClentLookupId, assetCategory, sensorType, txtSearchval);
            //deviceModelList = this.GetAllDeviceSortByColumnWithOrder(colname[0], orderDir, deviceModelList);
            var totalRecords = 0;
            var recordsFiltered = 0;
            if (deviceModelList != null && deviceModelList.Count > 0)
            {
                recordsFiltered = deviceModelList[0].TotalCount;
                totalRecords = deviceModelList[0].TotalCount;

            }

            int initialPage = start.Value;
            var filteredResult = deviceModelList.ToList();
            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult, assetCategoryList = AssetCategoryList, sensorTypeList = SensorTypeList }, JsonSerializerDateSettings);
        }

        public string DevicePrintData(string colname, string coldir, int customQueryDisplayId = 0, string deviceClientLookupId = "", string name = "", string assetClentLookupId = "", string assetCategory = "", string sensorType = "", string txtSearchval = "")
        {
            DevicesWrapper dvWrapper = new DevicesWrapper(userData);
            int printlenth = 100000;
            List<DeviceModel> dvList = dvWrapper.GetDeviceGridData(customQueryDisplayId, out IEnumerable<SelectListItem> AssetCategoryList, out IEnumerable<SelectListItem> SensorTypeList, 0, printlenth, colname, coldir, deviceClientLookupId, name, assetClentLookupId, assetCategory, sensorType, txtSearchval);

            //var dvList = dvWrapper.GetDevicePrintData(customQueryDisplayId, txtSearchval);

            //dvList = this.GetAllDeviceSortByColumnWithOrder(colname, coldir, dvList);
            List<DevicePrintModel> DevicePrintModelList = new List<DevicePrintModel>();
            DevicePrintModel objDevicePrintModel;
            if (dvList != null)
            {
                //if (!string.IsNullOrEmpty(deviceClientLookupId))
                //{
                //    deviceClientLookupId = deviceClientLookupId.ToUpper();
                //    dvList = dvList.Where(x => (!string.IsNullOrWhiteSpace(x.ClientLookupID) && x.ClientLookupID.ToUpper().Contains(deviceClientLookupId))).ToList();
                //}
                //if (!string.IsNullOrEmpty(name))
                //{
                //    name = name.ToUpper();
                //    dvList = dvList.Where(x => (!string.IsNullOrWhiteSpace(x.Name) && x.Name.ToUpper().Contains(name))).ToList();
                //}
                //if (!string.IsNullOrEmpty(assetClentLookupId))
                //{
                //    assetClentLookupId = assetClentLookupId.ToUpper();
                //    dvList = dvList.Where(x => (!string.IsNullOrWhiteSpace(x.AssetID) && x.AssetID.ToUpper().Contains(assetClentLookupId))).ToList();
                //}
                //if (!string.IsNullOrEmpty(assetCategory))
                //{
                //    assetCategory = assetCategory.ToUpper();
                //    dvList = dvList.Where(x => (!string.IsNullOrWhiteSpace(x.IoTDeviceCategory) && x.IoTDeviceCategory.ToUpper().Equals(assetCategory))).ToList();
                //}
                //if (!string.IsNullOrEmpty(sensorType))
                //{
                //    sensorType = sensorType.ToUpper();
                //    dvList = dvList.Where(x => (!string.IsNullOrWhiteSpace(x.SensorType) && x.SensorType.ToUpper().Equals(sensorType))).ToList();
                //}
                foreach (var item in dvList)
                {
                    objDevicePrintModel = new DevicePrintModel();
                    objDevicePrintModel.ClientLookupID = item.ClientLookupID;
                    objDevicePrintModel.Name = item.Name;
                    objDevicePrintModel.SensorType = item.SensorType;
                    objDevicePrintModel.AssetID = item.AssetID;
                    objDevicePrintModel.AssetName = item.AssetName;
                    objDevicePrintModel.LastReading = item.LastReading;
                    objDevicePrintModel.LastReadingDate = item.LastReadingDate;
                    objDevicePrintModel.InactiveFlag = item.InactiveFlag;
                    DevicePrintModelList.Add(objDevicePrintModel);
                }
            }
            return JsonConvert.SerializeObject(new { data = DevicePrintModelList }, JsonSerializerDateSettings);
        }
        //private List<DeviceModel> GetAllDeviceSortByColumnWithOrder(string order, string orderDir, List<DeviceModel> data)
        //{
        //    List<DeviceModel> lst = new List<DeviceModel>();
        //    if (data != null)
        //    {
        //        switch (order)
        //        {
        //            case "0":
        //                lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ClientLookupID).ToList() : data.OrderBy(p => p.ClientLookupID).ToList();
        //                break;
        //            case "1":
        //                lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Name).ToList() : data.OrderBy(p => p.Name).ToList();
        //                break;
        //            case "2":
        //                lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.IoTDeviceCategory).ToList() : data.OrderBy(p => p.IoTDeviceCategory).ToList();
        //                break;
        //            case "3":
        //                lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.SensorType).ToList() : data.OrderBy(p => p.SensorType).ToList();
        //                break;
        //            case "4":
        //                lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.AssetID).ToList() : data.OrderBy(p => p.AssetID).ToList();
        //                break;
        //            case "5":
        //                lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.AssetName).ToList() : data.OrderBy(p => p.AssetName).ToList();
        //                break;
        //            case "6":
        //                lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.LastReading).ToList() : data.OrderBy(p => p.LastReading).ToList();
        //                break;
        //            default:
        //                lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ClientLookupID).ToList() : data.OrderBy(p => p.ClientLookupID).ToList();
        //                break;
        //        }
        //    }
        //    return lst;
        //}
        #endregion

        #region Details
        [HttpPost]
        public PartialViewResult DeviceDetails(long ioTDeviceId)
        {
            DevicesWrapper dvWrapper = new DevicesWrapper(userData);
            ChangeDeviceIDModel _ChangeDeviceIDModel = new ChangeDeviceIDModel();
            DeviceVM deviceVM = new DeviceVM();
            var deviceDetails = dvWrapper.GetDevicedetails(ioTDeviceId);
            deviceVM.deviceModel = deviceDetails;
            deviceVM.security = this.userData.Security;
            deviceVM.APMval = this.IsAPMOnly(userData);
            _ChangeDeviceIDModel.IoTDeviceId = ioTDeviceId;
            _ChangeDeviceIDModel.ClientLookupId = deviceVM.deviceModel.ClientLookupID;
            _ChangeDeviceIDModel.OldClientLookupId = deviceVM.deviceModel.ClientLookupID;
            _ChangeDeviceIDModel.UpdateIndex = deviceVM.deviceModel.UpdateIndex;
            deviceVM.changeDeviceIDModel = _ChangeDeviceIDModel;
            LocalizeControls(deviceVM, LocalizeResourceSetConstants.DeviceDetails);
            return PartialView("~/Views/Devices/_DeviceDetails.cshtml", deviceVM);
        }

        private bool IsAPMOnly(UserData userData)
        {
            if (userData.Site.APM == true)
            {
                return true;
            }
            return false;
        }

        public PartialViewResult EditDevice(long ioTDeviceId)
        {
            DevicesWrapper dvWrapper = new DevicesWrapper(userData);
            DeviceVM deviceVM = new DeviceVM();
            DeviceModel deviceModel = new DeviceModel();
            deviceModel = dvWrapper.GetDevicedetails(ioTDeviceId);
            SensorUnit sensorUnit = new SensorUnit();
            Type type = sensorUnit.GetType();
            PropertyInfo[] properties = type.GetProperties();
            List<string> tempList = new List<string>();
            Dictionary<string, string> My_dict1 =
                      new Dictionary<string, string>();
            foreach (var item in properties)
            {
                My_dict1.Add(item.Name.ToString(), item.GetValue(sensorUnit, null).ToString());
            }
            deviceModel.SensorUnitList = My_dict1.Select(y => new SelectListItem { Text = y.Key.ToString(), Value = y.Value.ToString() });
            deviceVM.deviceModel = deviceModel;
            LocalizeControls(deviceVM, LocalizeResourceSetConstants.DeviceDetails);
            return PartialView("~/Views/Devices/_EditDevice.cshtml", deviceVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult EditDevice(DeviceVM deviceVM)
        {
            string ModelValidationFailedMessage = string.Empty;
            string command = string.Empty;
            if (ModelState.IsValid)
            {
                DevicesWrapper dvWrapper = new DevicesWrapper(userData);
                IoTDevice ioTDevice = new IoTDevice();

                if (deviceVM.deviceModel.IoTDeviceId == 0)
                {
                    command = "add";
                    //----------code for add
                }
                else
                {
                    command = "update";
                    ioTDevice = dvWrapper.EditDeviceData(deviceVM);
                }
                if (ioTDevice.ErrorMessages != null && ioTDevice.ErrorMessages.Count > 0)
                {
                    return Json(ioTDevice.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { data = JsonReturnEnum.success.ToString(), Command = command, IotDeviceId = deviceVM.deviceModel.IoTDeviceId, deviceVM.deviceModel.SensorId }, JsonRequestBehavior.AllowGet);
                }
            }
            ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
            return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult UpdateDeviceStatus(long deviceId, bool inactiveFlag)
        {
            DevicesWrapper dvWrapper = new DevicesWrapper(userData);
            var errMsg = dvWrapper.UpdateDeviceActiveStatus(deviceId, inactiveFlag);
            if (errMsg != null && errMsg.Count > 0)
            {
                return Json(errMsg, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { result = JsonReturnEnum.success.ToString(), deviceId = deviceId }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult ChangeIoTDeviceId(DeviceVM deviceVM)
        {
            if (ModelState.IsValid)
            {
                DevicesWrapper dvWrapper = new DevicesWrapper(userData);
                List<String> errorList = new List<string>();
                errorList = dvWrapper.ChangeIoTDeviceId(deviceVM.changeDeviceIDModel);
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
        #endregion

        #region Event
        [HttpPost]
        public string PopulateEvent(int? draw, int? start, int? length, long ioTDeviceId, string deviceClientLookupId)
        {
            bool ActionSecurity = false;
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            DevicesWrapper dvWrapper = new DevicesWrapper(userData);
            DeviceVM deviceVM = new DeviceVM();
            var eventList = dvWrapper.GetEventdetails(ioTDeviceId, deviceClientLookupId);
            if (eventList != null)
            {
                eventList = this.GetAllEventsSortByColumnWithOrder(order, orderDir, eventList);
            }
            var totalRecords = 0;
            var recordsFiltered = 0;
            start = start.HasValue
                ? start / length
                : 0;
            recordsFiltered = eventList.Count();
            totalRecords = eventList.Count();
            int initialPage = start.Value;
            var filteredResult = eventList
                .Skip(initialPage * length ?? 0)
                .Take(length ?? 0)
                .ToList();
            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult, ActionSecurity = ActionSecurity }, JsonSerializer12HoursDateAndTimeSettings);
        }

        private List<DeviceEventModel> GetAllEventsSortByColumnWithOrder(string order, string orderDir, List<DeviceEventModel> data)
        {
            List<DeviceEventModel> lst = new List<DeviceEventModel>();
            switch (order)
            {
                case "0":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.EventID).ToList() : data.OrderBy(p => p.EventID).ToList();
                    break;
                case "1":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Description).ToList() : data.OrderBy(p => p.Description).ToList();
                    break;
                case "2":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Status).ToList() : data.OrderBy(p => p.Status).ToList();
                    break;
                case "3":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Disposition).ToList() : data.OrderBy(p => p.Disposition).ToList();
                    break;
                case "4":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Created).ToList() : data.OrderBy(p => p.Created).ToList();
                    break;
                default:
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.EventID).ToList() : data.OrderBy(p => p.EventID).ToList();
                    break;
            }
            return lst;
        }
        #endregion

        #region Gauge
        // GET: ArcGauge
        public ActionResult GenerateArcGauge(long ioTDeviceId, string deviceClientLookupId)
        {
            DevicesWrapper dvWrapper = new DevicesWrapper(userData);
            DeviceVM deviceVM = new DeviceVM();
            var deviceDetails = dvWrapper.GetDevicedetails(ioTDeviceId);
            GaugeModel objGaugeModel = new GaugeModel();
            objGaugeModel.IoTDeviceId = ioTDeviceId;
            objGaugeModel.DeviceClientLookupId = deviceClientLookupId;
            objGaugeModel.LastReading = deviceDetails.LastReading;
            objGaugeModel.Unit = deviceDetails.SensorUnit;
            if (objGaugeModel.Unit == "C" || objGaugeModel.Unit == "F")
            {
                char ch = (char)176;
                objGaugeModel.ReadingWithUnit = Convert.ToString(deviceDetails.LastReading) + " " + ch + deviceDetails.SensorUnit;
            }
            else
            {
                objGaugeModel.ReadingWithUnit = Convert.ToString(deviceDetails.LastReading) + " " + deviceDetails.SensorUnit;
            }
            deviceVM.gaugeModel = objGaugeModel;
            return Json(objGaugeModel, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Time Series Chart
        public JsonResult GetSensorRedingData(int SensorId)
        {
            string plotLevel = string.Empty;
            List<ArrayList> arrayLists = new List<ArrayList>();
            if (SensorId != 0)
            {
                DataContracts.SensorReading sensorReading = new DataContracts.SensorReading()
                {
                    ClientId = userData.DatabaseKey.Client.ClientId,
                    SiteId = userData.DatabaseKey.User.DefaultSiteId,
                    SensorID = SensorId
                };
                var result = sensorReading.RetrieveBySensorIDForTimeSeries(userData.DatabaseKey, userData.Site.TimeZone);

                //Griddata griddata;


                // List<Griddata> data = new List<Griddata>();
                if (result != null && result.Count > 0)
                {
                    plotLevel = result.Select(x => x.PlotLabels).FirstOrDefault();
                }
                foreach (var item in result)
                {
                    //griddata = new Griddata();
                    //griddata.MessageDate = item.MessageDate.Value.ToString("dd-MMM-y");
                    //griddata.PlotValues = item.PlotValues;
                    //data.Add(griddata);

                    ArrayList a = new ArrayList()
                {
                    item.MessageDate.Value.ToString("dd-MMM-y HH:mm:ss"),
                    //item.MessageDate.Value,
                    item.PlotValues
                };
                    arrayLists.Add(a);
                }
            }
            return Json(new { arrayLists, plotLevel }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region V2-536
        #region Add
        public PartialViewResult PopulateDeviceCatagory()
        {
            DeviceVM deviceVM = new DeviceVM();
            deviceVM.DeviceCategoryModel = new DeviceCategoryModel();
            var DeviceCatagoryList = UtilityFunction.GetDeviceCategory();
            deviceVM.DeviceCategoryModel.DeviceCategoryList = DeviceCatagoryList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
            LocalizeControls(deviceVM, LocalizeResourceSetConstants.DeviceDetails);
            return PartialView("_DeviceCatagoryList", deviceVM);
        }
        [HttpPost]
        public JsonResult SelectDeviceCatagory(DeviceVM deviceVM)
        {
            if (ModelState.IsValid)
            {
                return Json(new { data = JsonReturnEnum.success.ToString(), Result = deviceVM.DeviceCategoryModel.DeviceCategory }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                string ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }
        public PartialViewResult AddDevice(string Category)
        {
            DeviceVM objDeviceVM = new DeviceVM();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            List<DataContracts.LookupList> AllLookUps = new List<DataContracts.LookupList>();
            objDeviceVM.security = this.userData.Security;
            var ViewName = "";
            if (Category == "Meter")
            {
                if (userData.Site.APM && !userData.Site.CMMS)
                {
                    ViewName = "_AddDeviceAPMOnly";
                    objDeviceVM.DeviceAPMOnlyModel = new DeviceAPMOnlyModel();
                    AllLookUps = commonWrapper.GetAllLookUpList();
                    if (AllLookUps != null)
                    {
                        var SensorUnit = AllLookUps.Where(x => x.ListName == LookupListConstants.IOT_UNIT).ToList();
                        if (SensorUnit != null)
                        {
                            objDeviceVM.DeviceAPMOnlyModel.SensorUnitList = SensorUnit.Select(x => new SelectListItem { Text = x.ListValue + ' ' + '|' + ' ' + x.Description, Value = x.ListValue.ToString() });
                        }
                    }
                }
                else if (userData.Site.APM && userData.Site.CMMS)
                {
                    ViewName = "_AddDeviceAPMCMMS";
                    objDeviceVM.DeviceAPMCMMSModel = new DeviceAPMCMMSModel();
                }
            }
            else
            {
                AllLookUps = commonWrapper.GetAllLookUpList();
                if (Category == "MonnitSensor")
                {
                    ViewName = "_AddDeviceSensorMonnitSensor";
                    objDeviceVM.DeviceMonnitSensorModel = new DeviceMonnitSensorModel();
                    if (AllLookUps != null)
                    {
                        var SensorUnit = AllLookUps.Where(x => x.ListName == LookupListConstants.IOT_UNIT).ToList();
                        if (SensorUnit != null)
                        {
                            objDeviceVM.DeviceMonnitSensorModel.SensorUnitList = SensorUnit.Select(x => new SelectListItem { Text = x.ListValue + ' ' + '|' + ' ' + x.Description, Value = x.ListValue.ToString() });
                        }
                        var SensorType = AllLookUps.Where(x => x.ListName == LookupListConstants.IOT_TYPE).ToList();
                        if (SensorType != null)
                        {
                            objDeviceVM.DeviceMonnitSensorModel.SensorTypeList = SensorType.Select(x => new SelectListItem { Text = x.ListValue + ' ' + '|' + ' ' + x.Description, Value = x.ListValue.ToString() });
                        }
                    }
                }
                else
                {
                    ViewName = "_AddDeviceSensor";
                    objDeviceVM.DeviceSensorModel = new DeviceSensorModel();
                    if (AllLookUps != null)
                    {
                        var SensorUnit = AllLookUps.Where(x => x.ListName == LookupListConstants.IOT_UNIT).ToList();
                        if (SensorUnit != null)
                        {
                            objDeviceVM.DeviceSensorModel.SensorUnitList = SensorUnit.Select(x => new SelectListItem { Text = x.ListValue + ' ' + '|' + ' ' + x.Description, Value = x.ListValue.ToString() });
                        }
                        var SensorType = AllLookUps.Where(x => x.ListName == LookupListConstants.IOT_TYPE).ToList();
                        if (SensorType != null)
                        {
                            objDeviceVM.DeviceSensorModel.SensorTypeList = SensorType.Select(x => new SelectListItem { Text = x.ListValue + ' ' + '|' + ' ' + x.Description, Value = x.ListValue.ToString() });
                        }
                    }
                }
            }
            LocalizeControls(objDeviceVM, LocalizeResourceSetConstants.DeviceDetails);
            return PartialView(ViewName, objDeviceVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddDeviceAPMOnly(DeviceVM mrVM, string Command)
        {
            string ModelValidationFailedMessage = string.Empty;
            string Mode = string.Empty;
            if (ModelState.IsValid)
            {
                DevicesWrapper mrWrapper = new DevicesWrapper(userData);
                if (mrVM.DeviceAPMOnlyModel.IoTDeviceId == 0)
                {
                    Mode = "add";
                }
                var objDevice = mrWrapper.SaveDeviceAPMOnly(mrVM.DeviceAPMOnlyModel);

                if (objDevice.ErrorMessages != null && objDevice.ErrorMessages.Count > 0)
                {
                    return Json(objDevice.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), Command = Command, IoTDeviceId = objDevice.IoTDeviceId, SensorId = objDevice.SensorID, mode = Mode }, JsonRequestBehavior.AllowGet);
                }
            }
            ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
            return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddDeviceAPMCMMS(DeviceVM mrVM, string Command)
        {
            string ModelValidationFailedMessage = string.Empty;
            string Mode = string.Empty;
            if (ModelState.IsValid)
            {
                DevicesWrapper mrWrapper = new DevicesWrapper(userData);
                if (mrVM.DeviceAPMCMMSModel.IoTDeviceId == 0)
                {
                    Mode = "add";
                }
                var objDevice = mrWrapper.SaveDeviceAPMCMMS(mrVM.DeviceAPMCMMSModel);

                if (objDevice.ErrorMessages != null && objDevice.ErrorMessages.Count > 0)
                {
                    return Json(objDevice.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), Command = Command, IoTDeviceId = objDevice.IoTDeviceId, SensorId = objDevice.SensorID, mode = Mode }, JsonRequestBehavior.AllowGet);
                }
            }
            ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
            return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddDeviceMonnitSensor(DeviceVM mrVM, string Command)
        {
            string ModelValidationFailedMessage = string.Empty;
            string Mode = string.Empty;
            if (ModelState.IsValid)
            {
                DevicesWrapper mrWrapper = new DevicesWrapper(userData);
                if (mrVM.DeviceMonnitSensorModel.IoTDeviceId == 0)
                {
                    Mode = "add";
                }
                var objDevice = mrWrapper.SaveDeviceMonnitSensor(mrVM.DeviceMonnitSensorModel);

                if (objDevice.ErrorMessages != null && objDevice.ErrorMessages.Count > 0)
                {
                    return Json(objDevice.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), Command = Command, IoTDeviceId = objDevice.IoTDeviceId, SensorId = objDevice.SensorID, mode = Mode }, JsonRequestBehavior.AllowGet);
                }
            }
            ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
            return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddDeviceSensor(DeviceVM mrVM, string Command)
        {
            string ModelValidationFailedMessage = string.Empty;
            string Mode = string.Empty;
            if (ModelState.IsValid)
            {
                DevicesWrapper mrWrapper = new DevicesWrapper(userData);
                if (mrVM.DeviceSensorModel.IoTDeviceId == 0)
                {
                    Mode = "add";
                }
                var objDevice = mrWrapper.SaveDeviceSensor(mrVM.DeviceSensorModel);

                if (objDevice.ErrorMessages != null && objDevice.ErrorMessages.Count > 0)
                {
                    return Json(objDevice.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), Command = Command, IoTDeviceId = objDevice.IoTDeviceId, SensorId = objDevice.SensorID, mode = Mode }, JsonRequestBehavior.AllowGet);
                }
            }
            ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
            return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
        }
        public PartialViewResult EditDeviceCategory(long IoTDeviceId)
        {
            DeviceVM objDeviceVM = new DeviceVM();
            DevicesWrapper deviceWrapper = new DevicesWrapper(userData);
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            List<DataContracts.LookupList> AllLookUps = new List<DataContracts.LookupList>();
            objDeviceVM.security = this.userData.Security;
            var deviceDetails = deviceWrapper.GetDevicedetails(IoTDeviceId);
            var ViewName = "";
            if (deviceDetails.IoTDeviceCategory == "Meter")
            {
                if (deviceDetails.CMMSMeterId == 0)
                {
                    ViewName = "_AddDeviceAPMOnly";
                    objDeviceVM.DeviceAPMOnlyModel = deviceWrapper.GetDetailsForApmOnlyEdit(deviceDetails);
                    AllLookUps = commonWrapper.GetAllLookUpList();
                    if (AllLookUps != null)
                    {
                        var SensorUnit = AllLookUps.Where(x => x.ListName == LookupListConstants.IOT_UNIT).ToList();
                        if (SensorUnit != null)
                        {
                            objDeviceVM.DeviceAPMOnlyModel.SensorUnitList = SensorUnit.Select(x => new SelectListItem { Text = x.ListValue + ' ' + '|' + ' ' + x.Description, Value = x.ListValue.ToString() });
                        }
                    }
                }
                else
                {
                    ViewName = "_AddDeviceAPMCMMS";
                    objDeviceVM.DeviceAPMCMMSModel = deviceWrapper.GetDetailsForApmCMMSEdit(deviceDetails);
                    objDeviceVM.DeviceAPMCMMSModel.Mode = "Edit";
                }
            }
            else
            {
                AllLookUps = commonWrapper.GetAllLookUpList();
                if (deviceDetails.IoTDeviceCategory == "MonnitSensor")
                {
                    ViewName = "_AddDeviceSensorMonnitSensor";
                    objDeviceVM.DeviceMonnitSensorModel = deviceWrapper.GetDetailsForMonnitSensorEdit(deviceDetails);
                    objDeviceVM.DeviceMonnitSensorModel.Mode = "Edit";
                    if (AllLookUps != null)
                    {
                        var SensorUnit = AllLookUps.Where(x => x.ListName == LookupListConstants.IOT_UNIT).ToList();
                        if (SensorUnit != null)
                        {
                            objDeviceVM.DeviceMonnitSensorModel.SensorUnitList = SensorUnit.Select(x => new SelectListItem { Text = x.ListValue + ' ' + '|' + ' ' + x.Description, Value = x.ListValue.ToString() });
                        }
                        var SensorType = AllLookUps.Where(x => x.ListName == LookupListConstants.IOT_TYPE).ToList();
                        if (SensorType != null)
                        {
                            objDeviceVM.DeviceMonnitSensorModel.SensorTypeList = SensorType.Select(x => new SelectListItem { Text = x.ListValue + ' ' + '|' + ' ' + x.Description, Value = x.ListValue.ToString() });
                        }
                    }
                }
                else
                {
                    ViewName = "_AddDeviceSensor";
                    objDeviceVM.DeviceSensorModel = deviceWrapper.GetDetailsForSensorEdit(deviceDetails);
                    objDeviceVM.DeviceSensorModel.Mode = "Edit";
                    if (AllLookUps != null)
                    {
                        var SensorUnit = AllLookUps.Where(x => x.ListName == LookupListConstants.IOT_UNIT).ToList();
                        if (SensorUnit != null)
                        {
                            objDeviceVM.DeviceSensorModel.SensorUnitList = SensorUnit.Select(x => new SelectListItem { Text = x.ListValue + ' ' + '|' + ' ' + x.Description, Value = x.ListValue.ToString() });
                        }
                        var SensorType = AllLookUps.Where(x => x.ListName == LookupListConstants.IOT_TYPE).ToList();
                        if (SensorType != null)
                        {
                            objDeviceVM.DeviceSensorModel.SensorTypeList = SensorType.Select(x => new SelectListItem { Text = x.ListValue + ' ' + '|' + ' ' + x.Description, Value = x.ListValue.ToString() });
                        }
                    }
                }
            }
            LocalizeControls(objDeviceVM, LocalizeResourceSetConstants.DeviceDetails);
            return PartialView(ViewName, objDeviceVM);
        }
        #endregion
        #endregion

        #region V2-1103
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddRecordReadingIoTDevice(DeviceVM deviceVM)
        {
            if (ModelState.IsValid)
            {
                DevicesWrapper dvWrapper = new DevicesWrapper(userData);
                List<String> errorList = new List<string>();
                deviceVM.addRecordReadingModal.IoTDeviceId = deviceVM.deviceModel.IoTDeviceId;
                errorList = dvWrapper.AddRecordReading(deviceVM.addRecordReadingModal);
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
        #endregion
        public RedirectResult DetailFromEquipment(long IoTDeviceId, long EquipmentId, string EquipmentClientLookupId)
        {
            TempData["Mode"] = "DetailFromEquipment";        
            TempData["IoTDeviceId"] = IoTDeviceId;
            TempData["EquipmentId"] = EquipmentId;
            TempData["EquipmentClientLookupId"] = EquipmentClientLookupId;
            return Redirect("Index?page= Monitoring_Device_Search");
        }
    }
    
    //public class Griddata
    //{
    //    public string MessageDate { get; set; }
    //    public decimal PlotValues { get; set; }
    //}
    public class Schema
    {
        public string name { get; set; }
        public string type { get; set; }
        public string format { get; set; }
    }


}
