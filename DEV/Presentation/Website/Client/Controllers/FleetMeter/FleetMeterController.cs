using Client.ActionFilters;
using Client.BusinessWrapper.Common;
using Client.BusinessWrapper.FleetMeter;
using Client.Common;
using Client.Controllers.Common;
using Client.Models.FleetMeter;
using Common.Constants;
using Newtonsoft.Json;
using Rotativa;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Client.Controllers.FleetMeter
{
    public class FleetMeterController : SomaxBaseController
    {
        #region Search
        [CheckUserSecurity(securityType = SecurityConstants.Fleet_Meter_History)]
        public ActionResult Index()
        {
            FleetMeterVM fleetMeterVM = new FleetMeterVM();
            FleetMeterWrapper fleetMeterWrapper = new FleetMeterWrapper(userData);
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            fleetMeterVM.fleetMeterModel = new FleetMeterModel();
            fleetMeterVM.security = this.userData.Security;
            fleetMeterVM.DateRangeDropListForFMreadingDate = UtilityFunction.GetTimeRangeDrop().Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
            LocalizeControls(fleetMeterVM, LocalizeResourceSetConstants.EquipmentDetails);
            return View(fleetMeterVM);
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public string FleetMeterGridData(int? draw, int? start, int? length, string ClientLookupId = "", string Name = "", string Make = "", string Model = "", string VIN = "", DateTime? StartReadingDate = null, DateTime? EndReadingDate = null, string SearchText = "")
        {

            List<FleetMeterModel> fltMtrModelList = new List<FleetMeterModel>();
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
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
            FleetMeterWrapper fleetMeterWrapper = new FleetMeterWrapper(userData);
            string _startReadingDate = string.Empty;
            string _endReadingDate = string.Empty;
            _startReadingDate = StartReadingDate.HasValue ? StartReadingDate.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : string.Empty;
            _endReadingDate = EndReadingDate.HasValue ? EndReadingDate.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : string.Empty;
            fltMtrModelList = fleetMeterWrapper.GetFleetMeterGridData( order, orderDir, skip, length ?? 0, ClientLookupId, Name, Make, Model, VIN, _startReadingDate, _endReadingDate, SearchText);
            var totalRecords = 0;
            var recordsFiltered = 0;
            if (fltMtrModelList != null && fltMtrModelList.Count > 0)
            {
                recordsFiltered = fltMtrModelList[0].TotalCount;
                totalRecords = fltMtrModelList[0].TotalCount;
            }
            int initialPage = start.Value;
            var filteredResult = fltMtrModelList
              .ToList();

            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult }, JsonSerializer12HoursDateAndTimeUptoMinuteSettings);
        }
        #endregion
        #region Add Meter
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddNewMeterReadings(FleetMeterVM fleetMeterVM)
        {
            FleetMeterWrapper fleetMeterWrapper = new FleetMeterWrapper(userData);
            List<string> ErrorList = new List<string>();
            string ModelValidationFailedMessage = string.Empty;
            if (ModelState.IsValid)
            {
                ErrorList = fleetMeterWrapper.FleetMeterAdd(fleetMeterVM.fleetMeterModel);
                if (ErrorList != null && ErrorList.Count > 0)
                {
                    return Json(ErrorList, JsonRequestBehavior.AllowGet);
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

        #region VoidUnVoid
        [HttpPost]
        public ActionResult VoidFleetMeter(long fleetMeterReadingId)
        {
            FleetMeterWrapper fleetMeterWrapper = new FleetMeterWrapper(userData);
            var result = fleetMeterWrapper.VoidFleetMeter(fleetMeterReadingId);
            if (result)
            {
                return Json(JsonReturnEnum.success.ToString(), JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(JsonReturnEnum.failed.ToString(), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult UnvoidFleetMeter(long fleetMeterReadingId)
        {
            FleetMeterWrapper fleetMeterWrapper = new FleetMeterWrapper(userData);
            var result = fleetMeterWrapper.UnvoidFleetMeter(fleetMeterReadingId);
            if (result!=null && result.Count>0)
            {
                return Json(result[0], JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(JsonReturnEnum.success.ToString(), JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Print
        [HttpPost]
        [EncryptedActionParameter]
        public JsonResult SetPrintData(FleetMeterPrintParams fleetMeterPrintParams)
        {
            Session["FLEETMETERPRINTPARAMS"] = fleetMeterPrintParams;
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        [NoDirectAccess]
        public ActionResult ExportASPDF()
        {
            FleetMeterVM fltMeterVM = new FleetMeterVM();
            FleetMeterWrapper fmWrapper = new FleetMeterWrapper(userData);
            FleetMeterPDFPrintModel fleetMeterPDFPrintModel = new FleetMeterPDFPrintModel();
            List<FleetMeterPDFPrintModel> fleetMeterPDFPrintModelList = new List<FleetMeterPDFPrintModel>();
            FleetMeterPrintParams fleetMeterPrintParams = (FleetMeterPrintParams)Session["FLEETMETERPRINTPARAMS"];
            FleetMeterModel fleetMeterModel = new FleetMeterModel();
            string order = fleetMeterPrintParams.colname;
            string orderDir = fleetMeterPrintParams.coldir;
            string _startReadingDate = string.Empty;
            string _endReadingDate = string.Empty;
            string SearchText = fleetMeterPrintParams.SearchText;
            string ClientLookupId = fleetMeterPrintParams.ClientLookupId;
            string Name = fleetMeterPrintParams.Name;
            string Make = fleetMeterPrintParams.Make;
            string Model = fleetMeterPrintParams.Model;
            string VIN = fleetMeterPrintParams.VIN;
            _startReadingDate = fleetMeterPrintParams.StartReadingDate.HasValue ? fleetMeterPrintParams.StartReadingDate.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : string.Empty;
            _endReadingDate = fleetMeterPrintParams.EndReadingDate.HasValue ? fleetMeterPrintParams.EndReadingDate.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : string.Empty;
            List<FleetMeterModel> fleetModelList = fmWrapper.GetFleetMeterGridData(order, orderDir, 0, 100000, ClientLookupId, Name, Make, Model, VIN, _startReadingDate, _endReadingDate, SearchText);
            foreach (var item in fleetModelList)
            {
                fleetMeterPDFPrintModel = new FleetMeterPDFPrintModel();
                fleetMeterPDFPrintModel.ClientLookupId = item.ClientLookupId;
                fleetMeterPDFPrintModel.Name = item.Name;
                fleetMeterPDFPrintModel.EquipmentImage = item.EquipmentImage;
                fleetMeterPDFPrintModel.NoOfDays = item.NoOfDays;
                fleetMeterPDFPrintModel.ReadingLine1 = item.ReadingLine1;
                fleetMeterPDFPrintModel.ReadingLine2 = item.ReadingLine2;
                fleetMeterPDFPrintModel.ReadingDate = item.ReadingDate;
                fleetMeterPDFPrintModel.SourceType = item.SourceType;
                fleetMeterPDFPrintModel.Meter2Indicator = item.Meter2Indicator;
                fleetMeterPDFPrintModelList.Add(fleetMeterPDFPrintModel);

            }
            fltMeterVM.fleetMeterPDFPrintModel = fleetMeterPDFPrintModelList;

            LocalizeControls(fltMeterVM, LocalizeResourceSetConstants.EquipmentDetails);
            return new ViewAsPdf("FleetMeterGridPdfPrintTemplate", fltMeterVM)
            {
                PageSize = Rotativa.Options.Size.A4,
                PageMargins = { Left = 3, Right = 3, Top = 3, Bottom = 3 },
            };

        }

        [NoDirectAccess]
        public ActionResult PrintASPDF()
        {
            FleetMeterVM fltMeterVM = new FleetMeterVM();
            FleetMeterWrapper fmWrapper = new FleetMeterWrapper(userData);
            FleetMeterPDFPrintModel fleetMeterPDFPrintModel = new FleetMeterPDFPrintModel();
            List<FleetMeterPDFPrintModel> fleetMeterPDFPrintModelList = new List<FleetMeterPDFPrintModel>();
            FleetMeterPrintParams fleetMeterPrintParams = (FleetMeterPrintParams)Session["FLEETMETERPRINTPARAMS"];
            FleetMeterModel fleetMeterModel = new FleetMeterModel();
            string order = fleetMeterPrintParams.colname;
            string orderDir = fleetMeterPrintParams.coldir;
            string _startReadingDate = string.Empty;
            string _endReadingDate = string.Empty;
            string SearchText = fleetMeterPrintParams.SearchText;
            string ClientLookupId = fleetMeterPrintParams.ClientLookupId;
            string Name = fleetMeterPrintParams.Name;
            string Make = fleetMeterPrintParams.Make;
            string Model = fleetMeterPrintParams.Model;
            string VIN = fleetMeterPrintParams.VIN;
            _startReadingDate = fleetMeterPrintParams.StartReadingDate.HasValue ? fleetMeterPrintParams.StartReadingDate.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : string.Empty;
            _endReadingDate = fleetMeterPrintParams.EndReadingDate.HasValue ? fleetMeterPrintParams.EndReadingDate.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : string.Empty;
            List<FleetMeterModel> fleetModelList = fmWrapper.GetFleetMeterGridData(order, orderDir, 0, 100000, ClientLookupId, Name, Make, Model, VIN, _startReadingDate, _endReadingDate, SearchText);
            foreach (var item in fleetModelList)
            {
                fleetMeterPDFPrintModel = new FleetMeterPDFPrintModel();
                fleetMeterPDFPrintModel.ClientLookupId = item.ClientLookupId;
                fleetMeterPDFPrintModel.Name = item.Name;
                fleetMeterPDFPrintModel.EquipmentImage = item.EquipmentImage;
                fleetMeterPDFPrintModel.NoOfDays = item.NoOfDays;
                fleetMeterPDFPrintModel.ReadingLine1 = item.ReadingLine1;
                fleetMeterPDFPrintModel.ReadingLine2 = item.ReadingLine2;
                fleetMeterPDFPrintModel.ReadingDate = item.ReadingDate;
                fleetMeterPDFPrintModel.SourceType = item.SourceType;
                fleetMeterPDFPrintModel.Meter2Indicator = item.Meter2Indicator;
                fleetMeterPDFPrintModelList.Add(fleetMeterPDFPrintModel);

            }
            fltMeterVM.fleetMeterPDFPrintModel = fleetMeterPDFPrintModelList;

            LocalizeControls(fltMeterVM, LocalizeResourceSetConstants.EquipmentDetails);
            return new PartialViewAsPdf("FleetMeterGridPdfPrintTemplate", fltMeterVM)
            {
                PageSize = Rotativa.Options.Size.A4,
                FileName = "FleetMeter.pdf",
                PageMargins = { Left = 3, Right = 3, Top = 3, Bottom = 3 },
            };
        }
        #endregion
    }
}