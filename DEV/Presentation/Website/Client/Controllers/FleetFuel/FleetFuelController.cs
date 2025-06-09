using Client.ActionFilters;
using Client.BusinessWrapper;
using Client.BusinessWrapper.Common;
using Client.BusinessWrapper.FleetFuel;
using Client.Common;
using Client.Controllers.Common;
using Client.Models;
using Client.Models.FleetFuel;
using Common.Constants;
using DataContracts;
using Newtonsoft.Json;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using Rotativa;
namespace Client.Controllers.FleetFuel
{
    public class FleetFuelController : SomaxBaseController
    {
        #region Fleet Fuel Search
        [CheckUserSecurity(securityType = SecurityConstants.Fleet_Fuel_Tracking)]
        public ActionResult Index()
        {
            FleetFuelWrapper fltFuelWrapper = new FleetFuelWrapper(userData);
            FleetFuelVM fltFuelVM = new FleetFuelVM();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            fltFuelVM.FleetFuelModel = new FleetFuelModel();
            fltFuelVM.security = this.userData.Security;
            fltFuelVM.FleetFuelModel.DateRangeDropListForFFReadingdate = UtilityFunction.GetTimeRangeDrop().Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
            LocalizeControls(fltFuelVM, LocalizeResourceSetConstants.EquipmentDetails);
            return View(fltFuelVM);
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public string GetFleetFuelGridData(int? draw, int? start, int? length, string ClientLookupId = "", string Name = "", string Make = "", string Model = "", string VIN = "", DateTime? StartReadingDate = null, DateTime? EndReadingDate = null, string SearchText = ""
     )
        {
            List<FleetFuelSearchModel> fltFuelSearchModelList = new List<FleetFuelSearchModel>();
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            var colname = Request.Form.GetValues("columns[" + order + "][name]");
            string _startReadingDate = string.Empty;
            string _endReadingDate = string.Empty;
            SearchText = SearchText.Replace("%", "[%]");
            ClientLookupId = ClientLookupId.Replace("%", "[%]");
            Name = Name.Replace("%", "[%]");
            Make = Make.Replace("%", "[%]");
            Model = Model.Replace("%", "[%]");
            VIN = VIN.Replace("%", "[%]");
            _startReadingDate = StartReadingDate.HasValue ? StartReadingDate.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : string.Empty;
            _endReadingDate = EndReadingDate.HasValue ? EndReadingDate.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : string.Empty;
            start = start.HasValue
               ? start / length
               : 0;
            int skip = start * length ?? 0;
            List<string> typeList = new List<string>();
            FleetFuelWrapper fltFuelWrapper = new FleetFuelWrapper(userData);
            List<FleetFuelSearchModel> fleetFuelList = fltFuelWrapper.GetFleetFuelGridData(order, orderDir, skip, length ?? 0, ClientLookupId, Name, Make, Model, VIN, _startReadingDate, _endReadingDate, SearchText);
            var totalRecords = 0;
            var recordsFiltered = 0;
            if (fleetFuelList != null && fleetFuelList.Count > 0)
            {
                recordsFiltered = fleetFuelList[0].TotalCount;
                totalRecords = fleetFuelList[0].TotalCount;
            }
            int initialPage = start.Value;
            var filteredResult = fleetFuelList
              .ToList();
            bool IsFleetFuelAccessSecurity = userData.Security.Fleet_RecordFuelEntry.Access;
          
            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult, IsFleetFuelAccessSecurity = IsFleetFuelAccessSecurity}, JsonSerializer12HoursDateAndTimeUptoMinuteSettings);
        }
        #endregion
        #region Fuel Add or Edit
        public PartialViewResult FleetFuelAddOrEdit(long EquipmentId,long FuelTrackingId)
        {
            FleetFuelWrapper ffWrapper = new FleetFuelWrapper(userData);
            FleetFuelVM fltfuelVM = new FleetFuelVM();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            List<DataContracts.LookupList> FuelTypeList = new List<DataContracts.LookupList>();
            List<DataContracts.LookupList> AllLookUps = new List<DataContracts.LookupList>();
            fltfuelVM.FleetFuelModel = new FleetFuelModel(); 
            fltfuelVM.FleetFuelModel.Pagetype = "Add";
            if (EquipmentId!=0 && FuelTrackingId != 0)
            {
                fltfuelVM.FleetFuelModel = ffWrapper.GetEditFleetFuelDetailsById(EquipmentId, FuelTrackingId);
                fltfuelVM.FleetFuelModel.Pagetype = "Edit";
            }
            AllLookUps = commonWrapper.GetAllLookUpList();
            if (AllLookUps != null)
            {
                FuelTypeList = AllLookUps.Where(x => x.ListName == LookupListConstants.FA_FUELTYPE).ToList();
                if (FuelTypeList != null)
                {
                    fltfuelVM.LookupFuelTypeList = FuelTypeList.Select(x => new SelectListItem { Text = x.ListValue + " - " + x.Description, Value = x.ListValue }).ToList();
                }
            }
            
            LocalizeControls(fltfuelVM, LocalizeResourceSetConstants.EquipmentDetails);
            return PartialView("~/Views/FleetFuel/_FleetFuelAddOrEdit.cshtml", fltfuelVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FleetFuelAddOrEdit(FleetFuelVM objFF, string Command)
        {
            List<string> ErrorList = new List<string>();
            FleetFuelWrapper ffWrapper = new FleetFuelWrapper(userData);
            string ModelValidationFailedMessage = string.Empty;
            string Mode = string.Empty;
            if (ModelState.IsValid)
            {
                if(objFF.FleetFuelModel.FuelTrackingId>0)
                {
                    Mode = "Edit";
                }
                else
                {
                    Mode = "Add";
                }
                Equipment equipment = new Equipment();
                string FF_ClientLookupId = objFF.FleetFuelModel.ClientLookupId.ToUpper().Trim();
                equipment = ffWrapper.AddOrEditFleetFuel(FF_ClientLookupId, objFF);
                if (equipment.ErrorMessages != null && equipment.ErrorMessages.Count > 0)
                {
                    return Json(equipment.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), Command = Command, EquipmentId = equipment.EquipmentId, Mode= Mode }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }

       
        #endregion

        #region Delete FuelTracking
        public ActionResult DeleteFuelTracking(long fuelTrackingId)
        {
                FleetFuelWrapper ffWrapper = new FleetFuelWrapper(userData);          
                var deleteResult = ffWrapper.DeleteFuelTracking(fuelTrackingId);
                if (deleteResult)
                {
                    return Json(JsonReturnEnum.success.ToString(), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(JsonReturnEnum.failed.ToString(), JsonRequestBehavior.AllowGet);
                }            
        }
        #endregion
        #region FleetFuel Void Unvoid
        [HttpPost]
        public JsonResult ValidateFleetFuelForUnvoid(long _eqid, int mtrreadingid, bool Meter2Indicator)
        {
            FleetFuelWrapper fltfuelWrapper = new FleetFuelWrapper(userData);
            
            var result = fltfuelWrapper.ValidateFleetFuelForUnvoid(_eqid, mtrreadingid, Meter2Indicator);
            if (result != null && result.Count > 0)
            {
                return Json(result[0], JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(JsonReturnEnum.success.ToString(), JsonRequestBehavior.AllowGet);
            }
           
        }

        [HttpPost]
        public JsonResult UpdateFleetFuelForvoid(long _eqid, int mtrreadingid,string FF_ClientLookupId)
        {
            FleetFuelWrapper fltfuelWrapper = new FleetFuelWrapper(userData);
            Equipment equipment = new Equipment();
            equipment = fltfuelWrapper.ValidateFleetFuelForvoid(_eqid, mtrreadingid, FF_ClientLookupId);
            if (equipment.ErrorMessages != null && equipment.ErrorMessages.Count > 0)
            {
                return Json(equipment.ErrorMessages, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { validationStatus = true, equipmentid = equipment.EquipmentId }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        #region Print
        [HttpPost]
        [EncryptedActionParameter]
        public JsonResult SetPrintData(FleetFuelPrintParams fleetFuelPrintParams)
        {
            Session["FLEETFUELPRINTPARAMS"] = fleetFuelPrintParams;
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        [NoDirectAccess]
        public ActionResult ExportASPDF()
        {
            FleetFuelVM fltfuelVM = new FleetFuelVM();
            FleetFuelWrapper ffWrapper = new FleetFuelWrapper(userData);
            FleetFuelPDFPrintModel fleetFuelPDFPrintModel = new FleetFuelPDFPrintModel();
            List<FleetFuelPDFPrintModel> fleetFuelPDFPrintModelList = new List<FleetFuelPDFPrintModel>();
            FleetFuelPrintParams fleetFuelPrintParams = (FleetFuelPrintParams)Session["FLEETFUELPRINTPARAMS"];
            FleetFuelSearchModel fleetFuelSearchModel = new FleetFuelSearchModel();
            string order = fleetFuelPrintParams.colname;
            string orderDir = fleetFuelPrintParams.coldir;
            string _startReadingDate = string.Empty;
            string _endReadingDate = string.Empty;
            string SearchText = fleetFuelPrintParams.SearchText;
            string ClientLookupId = fleetFuelPrintParams.ClientLookupId;
            string Name = fleetFuelPrintParams.Name;
            string Make = fleetFuelPrintParams.Make;
            string Model = fleetFuelPrintParams.Model;
            string VIN = fleetFuelPrintParams.VIN;
            _startReadingDate = fleetFuelPrintParams.StartReadingDate.HasValue ? fleetFuelPrintParams.StartReadingDate.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : string.Empty;
            _endReadingDate = fleetFuelPrintParams.EndReadingDate.HasValue ? fleetFuelPrintParams.EndReadingDate.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : string.Empty;
            List<FleetFuelSearchModel> fleetFuelList = ffWrapper.GetFleetFuelGridData(order, orderDir, 0, 100000, ClientLookupId, Name, Make, Model, VIN, _startReadingDate, _endReadingDate, SearchText);
            foreach (var item in fleetFuelList)
            {
                fleetFuelPDFPrintModel = new FleetFuelPDFPrintModel();
                fleetFuelPDFPrintModel.ClientLookupId = item.ClientLookupId;
                fleetFuelPDFPrintModel.Name = item.Name;
                fleetFuelPDFPrintModel.ImageUrl = item.ImageUrl;
                fleetFuelPDFPrintModel.ReadingDate = item.ReadingDate;
                fleetFuelPDFPrintModel.FuelAmount = item.FuelAmount;
                fleetFuelPDFPrintModel.FuelUnits = item.FuelUnits;
                fleetFuelPDFPrintModel.UnitCost = item.UnitCost;
                fleetFuelPDFPrintModel.TotalCost = item.TotalCost;
                fleetFuelPDFPrintModelList.Add(fleetFuelPDFPrintModel);

            }
            fltfuelVM.fleetFuelPDFPrintModel = fleetFuelPDFPrintModelList;
            
            LocalizeControls(fltfuelVM, LocalizeResourceSetConstants.EquipmentDetails);
            return new ViewAsPdf("FleetFuelGridPdfPrintTemplate", fltfuelVM)
            {
                PageSize = Rotativa.Options.Size.A4,
                PageMargins = { Left = 3, Right = 3, Top = 3, Bottom = 3 },
            };
        }

        [NoDirectAccess]
        public ActionResult PrintASPDF()
        {
            FleetFuelVM fltfuelVM = new FleetFuelVM();
            FleetFuelWrapper ffWrapper = new FleetFuelWrapper(userData);
            FleetFuelPDFPrintModel fleetFuelPDFPrintModel = new FleetFuelPDFPrintModel();
            List<FleetFuelPDFPrintModel> fleetFuelPDFPrintModelList = new List<FleetFuelPDFPrintModel>();
            FleetFuelPrintParams fleetFuelPrintParams = (FleetFuelPrintParams)Session["FLEETFUELPRINTPARAMS"];
            FleetFuelSearchModel fleetFuelSearchModel = new FleetFuelSearchModel();
            string order = fleetFuelPrintParams.colname;
            string orderDir = fleetFuelPrintParams.coldir;
            string _startReadingDate = string.Empty;
            string _endReadingDate = string.Empty;
            string SearchText = fleetFuelPrintParams.SearchText;
            string ClientLookupId = fleetFuelPrintParams.ClientLookupId;
            string Name = fleetFuelPrintParams.Name;
            string Make = fleetFuelPrintParams.Make;
            string Model = fleetFuelPrintParams.Model;
            string VIN = fleetFuelPrintParams.VIN;
            _startReadingDate = fleetFuelPrintParams.StartReadingDate.HasValue ? fleetFuelPrintParams.StartReadingDate.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : string.Empty;
            _endReadingDate = fleetFuelPrintParams.EndReadingDate.HasValue ? fleetFuelPrintParams.EndReadingDate.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : string.Empty;
            List<FleetFuelSearchModel> fleetFuelList = ffWrapper.GetFleetFuelGridData(order, orderDir, 0, 100000, ClientLookupId, Name, Make, Model, VIN, _startReadingDate, _endReadingDate, SearchText);
            foreach (var item in fleetFuelList)
            {
                fleetFuelPDFPrintModel = new FleetFuelPDFPrintModel();
                fleetFuelPDFPrintModel.ClientLookupId = item.ClientLookupId;
                fleetFuelPDFPrintModel.Name = item.Name;
                fleetFuelPDFPrintModel.ImageUrl = item.ImageUrl;
                fleetFuelPDFPrintModel.ReadingDate = item.ReadingDate;
                fleetFuelPDFPrintModel.FuelAmount = item.FuelAmount;
                fleetFuelPDFPrintModel.FuelUnits = item.FuelUnits;
                fleetFuelPDFPrintModel.UnitCost = item.UnitCost;
                fleetFuelPDFPrintModel.TotalCost = item.TotalCost;
                fleetFuelPDFPrintModelList.Add(fleetFuelPDFPrintModel);

            }
            fltfuelVM.fleetFuelPDFPrintModel = fleetFuelPDFPrintModelList;
          
            LocalizeControls(fltfuelVM, LocalizeResourceSetConstants.EquipmentDetails);

            return new PartialViewAsPdf("FleetFuelGridPdfPrintTemplate", fltfuelVM)
            {
                PageSize = Rotativa.Options.Size.A4,
                FileName = "FleetFuel.pdf",
                PageMargins = { Left = 3, Right = 3, Top = 3, Bottom = 3 },
            };
        }
        #endregion
    }
}