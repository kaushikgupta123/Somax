using Client.ActionFilters;
using Client.BusinessWrapper.Configuration;
using Client.Common;
using Client.Controllers.Common;
using Client.Models.Configuration.CategoryMaster;
using Client.Models.Configuration.ShipToAddress;

using Common.Constants;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Client.Controllers.Configuration.ShipToAddress
{
    public class ShipToAddressController : ConfigBaseController
    {
        // GET: ShipToAddress
        #region Search
        [CheckUserSecurity(securityType = SecurityConstants.ShipToAddress)]
        public ActionResult Index()
        {
            ShipToVM shipToAddressVM = new ShipToVM();
            shipToAddressVM.security = userData.Security;
            LocalizeControls(shipToAddressVM, LocalizeResourceSetConstants.Global);
            return View("~/Views/Configuration/ShipToAddress/index.cshtml", shipToAddressVM);
        }

        

public string GetGridDataforShipToAddress(int? draw, int? start, int? length = 0, string Order = "0", string ClientLookupId = "", string Address1 = null, string AddressCity = null, string AddressState = null)
        {
            string order = Request.Form["order[0][column]"];
            string orderDir = Request.Form["order[0][dir]"];
            start = start.HasValue
                ? start / length
                : 0;
            int skip = start * length ?? 0;
            string colname = Request.Form["columns[" + order + "][name]"];

            ShipToWrapper wrapper = new ShipToWrapper(userData);
            List<ShipToModel> list = wrapper.GetShipToAddressList(order, length ?? 0, orderDir, skip, ClientLookupId, Address1, AddressCity, AddressState);

            int totalRecords = list.FirstOrDefault()?.TotalCount ?? 0;
            int recordsFiltered = totalRecords;

            bool showAddBtn = userData.Security.ShipToAddress.Create;
            bool showEditBtn = userData.Security.ShipToAddress.Edit;
            bool showDeleteBtn = userData.Security.ShipToAddress.Delete;

            return JsonConvert.SerializeObject(new
            {
                draw = draw,
                recordsTotal = totalRecords,
                recordsFiltered = recordsFiltered,
                data = list,
                showAddBtn = showAddBtn,
                showEditBtn = showEditBtn,
                showDeleteBtn = showDeleteBtn
            });
        }
        #endregion

        #region Add/Edit
        [HttpGet]
        public PartialViewResult AddShipToAddress()
        {
            ShipToVM sTVM = new ShipToVM();
            ShipToModel Model = new ShipToModel();
            sTVM.shipToModel = Model;
            LocalizeControls(sTVM, LocalizeResourceSetConstants.Global);
            return PartialView("~/Views/Configuration/ShipToAddress/AddOrEditShipToAddress.cshtml", sTVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddOrEditShipToAddress(ShipToVM objVM, string Command)
        {
            if (ModelState.IsValid)
            {
                ShipToWrapper wrapper = new ShipToWrapper(userData);
                string Mode = string.Empty;

                List<String> errorList = wrapper.AddOrEditShipToRecord(objVM.shipToModel);
                if (errorList != null && errorList.Count > 0)
                {
                    return Json(errorList, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    if (objVM.shipToModel.ShipToId == 0)
                    {
                        Mode = "add";
                    }
                    return Json(new { Result = JsonReturnEnum.success.ToString(), mode = Mode, Command = Command }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                string ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult EditShipToAddress(long ShipToId)
        {
            ShipToWrapper wrapper = new ShipToWrapper(userData);
            ShipToVM shipToVM = new ShipToVM();

            ShipToModel shipToModel = wrapper.RetrieveShipToDetailsById(ShipToId);
            shipToVM.shipToModel = shipToModel;

            LocalizeControls(shipToVM, LocalizeResourceSetConstants.Global);
            return PartialView("~/Views/Configuration/ShipToAddress/AddOrEditShipToAddress.cshtml", shipToVM);
        }

        #endregion

        #region Delete
        [HttpPost]
        public ActionResult DeleteShipToAddress(long ShipToAddressId)
        {
            ShipToWrapper wrapper = new ShipToWrapper(userData);
            List<String> errorList = wrapper.DeleteShipToAddress(ShipToAddressId);
            if (errorList != null && errorList.Count > 0)
            {
                return Json(new { Result = JsonReturnEnum.failed.ToString() }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Result = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);

            }
        }
        #endregion


        #region Printdata
        [HttpGet]
        public string GetShipToAddressPrintData(string _colname, string _coldir, string _clientLookupId = "", string _address1 = null, string _addressCity = null, string _addressState = null)
        {
            List<ShipToPrintModel> printModelList = new List<ShipToPrintModel>();
            ShipToPrintModel printModel;
            ShipToWrapper wrapper = new ShipToWrapper(userData);
            var shipToList = wrapper.GetShipToAddressList(_colname, 100000, _coldir, 0, _clientLookupId, _address1, _addressCity, _addressState);
            if (shipToList != null)
            {
                foreach (var p in shipToList)
                {
                    printModel = new ShipToPrintModel();
                    printModel.ClientLookupId = p.ClientLookupId;
                    printModel.Address1 = p.Address1;
                    printModel.AddressCity = p.AddressCity;
                    printModel.AddressState = p.AddressState;
                    printModelList.Add(printModel);
                }
            }
            return JsonConvert.SerializeObject(new { data = printModelList }, JsonSerializerDateSettings);
        }
        #endregion
    }
}