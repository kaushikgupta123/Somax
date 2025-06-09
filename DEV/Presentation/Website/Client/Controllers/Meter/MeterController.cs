using Client.ActionFilters;
using Client.BusinessWrapper.Meters;
using Client.Common;
using Client.Controllers.Common;
using Client.Models;
using Client.Models.Meters;
using Common.Constants;
using Newtonsoft.Json;
using Rotativa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Client.Controllers.Meters
{
    public class MeterController : SomaxBaseController
    {
        [CheckUserSecurity(securityType = SecurityConstants.Meters)]
        public ActionResult Index()
        {
            MetersVM metersVM = new MetersVM();
            MetersModel metersModel = new MetersModel();
            metersVM.Meters = metersModel;
            metersVM.security = this.userData.Security;
            LocalizeControls(metersVM, LocalizeResourceSetConstants.MeterDetails);
            return View(metersVM);
        }
        #region Search
        public string GetMeterGridData(int? draw, int? start, int? length, bool inactiveFlag = false, string MeterClientLookUpId = "", string Name = "", string ReadingCurrent = "", DateTime? ReadingDate = null, string ReadingBy = "",
            string ReadingLife = "", string MaxReading = "", string ReadingUnits = "")
        {
            MetersWrapper metersWrapper = new MetersWrapper(userData);
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            var colname = Request.Form.GetValues("columns[" + order + "][name]");
            var metersList = metersWrapper.GetMeterGridData();
            if (metersList != null && metersList.Count() > 0)
            {
                metersList = metersList.Where(x => x.InActive == inactiveFlag).ToList();
                metersList = GetMeterSearchResult(metersList, MeterClientLookUpId, Name, ReadingCurrent, ReadingDate, ReadingBy, ReadingLife, MaxReading, ReadingUnits);
                metersList = GetAllMetersSortByColumnWithOrder(colname[0], orderDir, metersList);
            }
            var totalRecords = 0;
            var recordsFiltered = 0;
            start = start.HasValue
                ? start / length
                : 0;
            recordsFiltered = metersList.Count();
            totalRecords = metersList.Count();
            int initialPage = start.Value;
            var filteredResult = metersList
                .Skip(initialPage * length ?? 0)
                .Take(length ?? 0)
                .ToList();
            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult }, JsonSerializerDateSettings);
        }
        private List<MetersModel> GetMeterSearchResult(List<MetersModel> meters, string MeterClientLookUpId = "", string Name = "", string ReadingCurrent = "", DateTime? ReadingDate = null, string ReadingBy = "",
            string ReadingLife = "", string MaxReading = "", string ReadingUnits = "")
        {
            if (meters != null)
            {
                if (!string.IsNullOrEmpty(MeterClientLookUpId))
                {
                    MeterClientLookUpId = MeterClientLookUpId.ToUpper();
                    meters = meters.Where(x => (!string.IsNullOrWhiteSpace(x.MeterClientLookUpId)) && x.MeterClientLookUpId.ToUpper().Contains(MeterClientLookUpId)).ToList();
                }
                if (!string.IsNullOrEmpty(Name))
                {
                    Name = Name.ToUpper();
                    meters = meters.Where(x => (!string.IsNullOrWhiteSpace(x.MeterName)) && x.MeterName.ToUpper().Contains(Name)).ToList();
                }
                if (!string.IsNullOrEmpty(ReadingCurrent))
                {
                    decimal temp = 0;
                    bool parseResult = decimal.TryParse(ReadingCurrent, out temp);
                    if (parseResult)
                    {
                        meters = meters.Where(x => x.ReadingCurrent.Equals(temp)).ToList();
                    }
                }
                if (ReadingDate != null)
                {
                    meters = meters.Where(x => (x.ReadingDate != null && x.ReadingDate.Value.Date.Equals(ReadingDate.Value.Date))).ToList();
                }
                if (!string.IsNullOrEmpty(ReadingLife))
                {
                    decimal temp = 0;
                    bool parseResult = decimal.TryParse(ReadingLife, out temp);
                    if (parseResult)
                    {
                        meters = meters.Where(x => x.ReadingLife.Equals(temp)).ToList();
                    }
                }
                if (!string.IsNullOrEmpty(ReadingBy))
                {
                    ReadingBy = ReadingBy.ToUpper();
                    meters = meters.Where(x => (!string.IsNullOrWhiteSpace(x.PersonnelClientLookUpId)) && x.PersonnelClientLookUpId.ToUpper().Contains(ReadingBy)).ToList();
                }
                if (!string.IsNullOrEmpty(MaxReading))
                {
                    decimal temp = 0;
                    bool parseResult = decimal.TryParse(MaxReading, out temp);
                    if (parseResult)
                    {
                        meters = meters.Where(x => x.MaxReading.Equals(temp)).ToList();
                    }
                }
                if (!string.IsNullOrEmpty(ReadingUnits))
                {
                    ReadingUnits = ReadingUnits.ToUpper();
                    meters = meters.Where(x => (!string.IsNullOrWhiteSpace(x.ReadingUnits)) && x.ReadingUnits.ToUpper().Contains(ReadingUnits)).ToList();
                }
            }
            return meters;
        }
        private List<MetersModel> GetAllMetersSortByColumnWithOrder(string order, string orderDir, List<MetersModel> data)
        {
            List<MetersModel> lst = new List<MetersModel>();
            switch (order)
            {
                case "0":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.MeterClientLookUpId).ToList() : data.OrderBy(p => p.MeterClientLookUpId).ToList();
                    break;
                case "1":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.MeterName).ToList() : data.OrderBy(p => p.MeterName).ToList();
                    break;
                case "2":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ReadingCurrent).ToList() : data.OrderBy(p => p.ReadingCurrent).ToList();
                    break;
                case "3":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ReadingDate).ToList() : data.OrderBy(p => p.ReadingDate).ToList();
                    break;
                case "4":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.PersonnelClientLookUpId).ToList() : data.OrderBy(p => p.PersonnelClientLookUpId).ToList();
                    break;
                case "5":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ReadingLife).ToList() : data.OrderBy(p => p.ReadingLife).ToList();
                    break;
                case "6":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.MaxReading).ToList() : data.OrderBy(p => p.MaxReading).ToList();
                    break;
                case "7":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ReadingUnits).ToList() : data.OrderBy(p => p.ReadingUnits).ToList();
                    break;
                default:
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.MeterClientLookUpId).ToList() : data.OrderBy(p => p.MeterClientLookUpId).ToList();
                    break;
            }
            return lst;
        }
        public string GetMeterPrintData(string colname, string coldir, bool inactiveFlag = true, string MeterClientLookUpId = "", string Name = "", string ReadingCurrent = "", DateTime? ReadingDate = null, string ReadingBy = "",
            string ReadingLife = "", string MaxReading = "", string ReadingUnits = "")
        {
            MetersWrapper metersWrapper = new MetersWrapper(userData);
            List<MetersPrintModel> meters = new List<MetersPrintModel>();
            var metersList = metersWrapper.GetMeterGridData();
            if (metersList != null && metersList.Count() > 0)
            {
                MetersPrintModel metersPrintModel;
                metersList = metersList.Where(x => x.InActive == inactiveFlag).ToList();
                metersList = GetMeterSearchResult(metersList, MeterClientLookUpId, Name, ReadingCurrent, ReadingDate, ReadingBy, ReadingLife, MaxReading, ReadingUnits);
                metersList = GetAllMetersSortByColumnWithOrder(colname, coldir, metersList);
                foreach (var item in metersList)
                {
                    metersPrintModel = new MetersPrintModel();
                    metersPrintModel.MeterClientLookUpId = item.MeterClientLookUpId;
                    metersPrintModel.MeterName = item.MeterName;
                    metersPrintModel.ReadingCurrent = item.ReadingCurrent;
                    metersPrintModel.ReadingDate = item.ReadingDate;
                    metersPrintModel.PersonnelClientLookUpId = item.PersonnelClientLookUpId;
                    metersPrintModel.ReadingLife = item.ReadingLife;
                    metersPrintModel.MaxReading = item.MaxReading;
                    metersPrintModel.ReadingUnits = item.ReadingUnits;
                    meters.Add(metersPrintModel);
                }
            }
            return JsonConvert.SerializeObject(new { data = meters }, JsonSerializerDateSettings);
        }
        #endregion
        #region Add or Edit
        public ActionResult AddMeter()
        {
            MetersVM objVM = new MetersVM();
            MetersModel metersModel = new MetersModel();
            MetersWrapper mWrapper = new MetersWrapper(userData);
            var readingUnitsList = mWrapper.GetReadingUnitList();
            if (readingUnitsList != null)
            {
                metersModel.ReadingUnitsList = readingUnitsList.Select(x => new SelectListItem { Text = x.ListValue.ToString() + "-" + x.Description, Value = x.ListValue.ToString() });
            }
            objVM.Meters = metersModel;
            objVM.security = this.userData.Security;
            LocalizeControls(objVM, LocalizeResourceSetConstants.MeterDetails);
            return PartialView("~/Views/Meter/_MeterAdd.cshtml", objVM);
        }


        public ActionResult EditMeter(long MeterId = 0)
        {
            MetersVM objVM = new MetersVM();
            MetersWrapper mWrapper = new MetersWrapper(userData);
            var meter = mWrapper.populateMeterDetails(MeterId);
            var readingUnitsList = mWrapper.GetReadingUnitList();
            if (readingUnitsList != null)
            {
                meter.ReadingUnitsList = readingUnitsList.Select(x => new SelectListItem { Text = x.ListValue.ToString() + "-" + x.Description, Value = x.ListValue.ToString() });
            }
            objVM.Meters = meter;
            objVM.security = this.userData.Security;
            LocalizeControls(objVM, LocalizeResourceSetConstants.MeterDetails);
            return PartialView("~/Views/Meter/_MeterEdit.cshtml", objVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddMeter(MetersVM objVM, string Command)
        {
            if (ModelState.IsValid)
            {
                List<String> errorList = new List<string>();
                MetersWrapper mWrapper = new MetersWrapper(userData);
                string Mode = string.Empty;
                long MeterId = 0;
                Mode = "add";
                errorList = mWrapper.AddMeter(objVM.Meters, ref MeterId);
                if (errorList != null && errorList.Count > 0)
                {
                    return Json(errorList, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), MeterId = MeterId, mode = Mode, Command = Command }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                string ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditMeter(MetersVM objVM, string Command)
        {
            if (ModelState.IsValid)
            {
                List<String> errorList = new List<string>();
                MetersWrapper mWrapper = new MetersWrapper(userData);
                string Mode = string.Empty;
                long MeterId = 0;
                Mode = "edit";
                errorList = mWrapper.EditMeter(objVM.Meters, ref MeterId);
                if (errorList != null && errorList.Count > 0)
                {
                    return Json(errorList, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), MeterId = MeterId, mode = Mode, Command = Command }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                string ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Details
        public PartialViewResult MeterDetail(long MeterId = 0)
        {
            MetersVM objVM = new MetersVM();
            MetersModel metersModel = new MetersModel();
            MetersResetModel metersResetModel = new MetersResetModel();
            MetersReadingModel metersReadingModel = new MetersReadingModel();
            MetersWrapper mWrapper = new MetersWrapper(userData);
            metersModel = mWrapper.populateMeterDetails(MeterId);
            objVM.Meters = metersModel;
            objVM.security = this.userData.Security;
            metersResetModel.MeterId = metersModel.MeterId;
            metersResetModel.MeterClientLookUpId = metersModel.MeterClientLookUpId;
            metersResetModel.Reading = 1;
            objVM.metersResetModel = metersResetModel;
            metersReadingModel.MeterId= metersModel.MeterId;
            metersReadingModel.MeterClientLookUpId= metersModel.MeterClientLookUpId;
            objVM.Readings = metersReadingModel;
            LocalizeControls(objVM, LocalizeResourceSetConstants.MeterDetails);
            return PartialView("MeterDetails", objVM);
        }
        [HttpPost]
        public JsonResult MakeActiveInactive(bool InActiveFlag, long MeterId)
        {
            MetersWrapper mWrapper = new MetersWrapper(userData);
            var IsActivated = mWrapper.MakeActiveInactive(InActiveFlag, MeterId);
            return Json(IsActivated, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Readings
        public string PopulateReadings(int? draw, int? start, int? length, long meterId, decimal Reading =0, string ReadByClientLookupId = "", DateTime? DateRead = null, string Reset = "")
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            MetersWrapper mWrapper = new MetersWrapper(userData);
            var ReadingsList = mWrapper.GetMeterReadingsList(meterId);
            if (ReadingsList != null && ReadingsList.Count() > 0)
            {
                ReadingsList = this.GetAllReadingsSortByColumnWithOrder(order, orderDir, ReadingsList);
                ReadingsList = GetReadingSearchResult(ReadingsList, Reading, ReadByClientLookupId, DateRead, Reset);

            }
            var totalRecords = 0;
            var recordsFiltered = 0;
            start = start.HasValue
                ? start / length
                : 0;
            recordsFiltered = ReadingsList.Count();
            totalRecords = ReadingsList.Count();
            int initialPage = start.Value;
            var filteredResult = ReadingsList
                .Skip(initialPage * length ?? 0)
                .Take(length ?? 0)
                .ToList();
            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult }, JsonSerializerDateSettings);
        }

        private List<MetersReadingModel> GetReadingSearchResult(List<MetersReadingModel> readings, decimal Reading = 0, string ReadByClientLookupId = "",  DateTime? DateRead = null, string Reset = ""
        )
        {
            if (readings != null)
            {
                if (Reading != 0)
                {
                    readings = readings.Where(x => x.Reading.Equals(Reading)).ToList();
                }
                if (!string.IsNullOrEmpty(ReadByClientLookupId))
                {
                    ReadByClientLookupId = ReadByClientLookupId.ToUpper();
                    readings = readings.Where(x => (!string.IsNullOrWhiteSpace(x.ReadByClientLookupId)) && x.ReadByClientLookupId.ToUpper().Contains(ReadByClientLookupId)).ToList();
                }               
                if (DateRead != null)
                {
                    readings = readings.Where(x => (x.DateRead != null && x.DateRead.Value.Date.Equals(DateRead.Value.Date))).ToList();
                }
                if (Reset != null && Reset != "")
                {
                    var v = false;
                    if (Reset.Equals("true"))
                    {
                        v = true;
                    }
                    else
                    {
                        v = false;
                    }                   
                    readings = readings.Where(x => x.Reset == v).ToList();
                }              
               
            }
            return readings;
        }
        private List<MetersReadingModel> GetAllReadingsSortByColumnWithOrder(string order, string orderDir, List<MetersReadingModel> data)
        {
            List<MetersReadingModel> lst = new List<MetersReadingModel>();
            switch (order)
            {
                case "0":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Reading).ToList() : data.OrderBy(p => p.Reading).ToList();
                    break;
                case "1":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ReadByClientLookupId).ToList() : data.OrderBy(p => p.ReadByClientLookupId).ToList();
                    break;
                case "2":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.DateRead).ToList() : data.OrderBy(p => p.DateRead).ToList();
                    break;
                case "3":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Reset).ToList() : data.OrderBy(p => p.Reset).ToList();
                    break;
            }
            return lst;
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddReadings(MetersVM objVM, string Command)
        {
            if (ModelState.IsValid)
            {
                List<String> errorList = new List<string>();
                MetersWrapper mWrapper = new MetersWrapper(userData);
                string Mode = string.Empty;
                long MeterId = objVM.Readings.MeterId;
                Mode = "add";
                string woLookupids = "";

                errorList = mWrapper.AddReadings(objVM.Readings, ref woLookupids);

                if (errorList != null && errorList.Count > 0)
                {
                    return Json(errorList, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), MeterId = MeterId, mode = Mode, Command = Command, woLookupids = woLookupids }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                string ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region ResetMeter
        public JsonResult ResetMeter(MetersVM objVM)
        {
            MetersWrapper metersWrapper = new MetersWrapper(userData);
            if (ModelState.IsValid)
            {
                var resetResult = metersWrapper.ResetMeter(objVM.metersResetModel);
                if (resetResult.Count > 0)
                {
                    return Json(resetResult, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), MeterId= objVM.metersResetModel.MeterId }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                string ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region QRCode         
       
        [HttpPost]
        public PartialViewResult MeterDetailsQRcode(string[] MeterClientLookups)
        {
            MetersVM objVM = new MetersVM();
            QRCodeModel qRCodeModel = new QRCodeModel();
            if (MeterClientLookups != null)
            {
                qRCodeModel.MeterIdsList = new List<string>(MeterClientLookups);
            }
            else
            {
                qRCodeModel.MeterIdsList = new List<string>();
            }
            objVM.qRCodeModel = qRCodeModel;
            LocalizeControls(objVM, LocalizeResourceSetConstants.MeterDetails);
            return PartialView("_MeterDetailsQRCode", objVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SetMeterIdlist(MetersVM objVM)
        {
            TempData["QRCodeMeterIdList"] = objVM.qRCodeModel.MeterIdsList;
            return Json(new { JsonReturnEnum.success });
        }
        [EncryptedActionParameter]
        public ActionResult QRCodeGenerationUsingRotativa(bool SmallLabel)
        {
            MetersVM objVM = new MetersVM();
            var qRCodeModel = new QRCodeModel();

            ViewBag.SmallQR = SmallLabel;
            if (TempData["QRCodeMeterIdList"] != null)
            {
                qRCodeModel.MeterIdsList = (List<string>)TempData["QRCodeMeterIdList"];
            }
            else
            {
                qRCodeModel.MeterIdsList = new List<string>();
            }
            objVM.qRCodeModel = qRCodeModel;

            return new PartialViewAsPdf("_MeterQRCodeTemplate", objVM)
            {
                PageMargins = { Left = 1, Right = 1, Top = 1, Bottom = 1 },
                PageHeight = SmallLabel ? 25 : 28,
                PageWidth = SmallLabel ? 54 : 89,
            };
        }
        #endregion
    }
}