using Client.ActionFilters;
using Client.BusinessWrapper;
using Client.Common;
using Client.Controllers.Common;
using Client.Models.Configuration.ManufacturerMaster;
using Common.Constants;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Client.Controllers.Configuration.ManufacturerMaster
{
  public class ManufacturerMasterController : ConfigBaseController
  {
    // GET: ManufacturerMaster
    #region Search
    [CheckUserSecurity(securityType = SecurityConstants.ManufacturerMaster)]
    public ActionResult Index()
    {
      ManufacturerMasterVM mmVM = new ManufacturerMasterVM();
      mmVM.security = userData.Security;
      LocalizeControls(mmVM, LocalizeResourceSetConstants.Global);
      return View("~/Views/Configuration/ManufacturerMaster/index.cshtml", mmVM);
    }
    public string GetGridDataforManufacturerMaster(int? draw, int? start, int? length, string ClientLookupId = "", string Name = null, bool Inactive = false)
    {
      ManufacturerModel mSearchModel;
      List<ManufacturerModel> mSearchModelList = new List<ManufacturerModel>();
      ManufacturerMasterVM mmVM = new ManufacturerMasterVM();
      string order = Request.Form.GetValues("order[0][column]")[0];
      string orderDir = Request.Form.GetValues("order[0][dir]")[0];
      var colname = Request.Form.GetValues("columns[" + order + "][name]");
      List<string> typeList = new List<string>();
      ManufacturerMasterWrapper mWrapper = new ManufacturerMasterWrapper(userData);
      var mList = mWrapper.RetrieveManufacturerMasterDetails(Inactive);
      if (mList != null)
      {
        mList = this.GetAllManMasterSortByColumnWithOrder(colname[0], orderDir, mList);
      }
      #region Advance-Search
      if (!string.IsNullOrEmpty(ClientLookupId))
      {
        ClientLookupId = ClientLookupId.ToUpper();
        mList = mList.Where(x => (!string.IsNullOrWhiteSpace(x.ClientLookupId) && x.ClientLookupId.ToUpper().Contains(ClientLookupId))).ToList();
      }
      if (!string.IsNullOrEmpty(Name))
      {
        Name = Name.ToUpper();
        mList = mList.Where(x => (!string.IsNullOrWhiteSpace(x.Name) && x.Name.ToUpper().Contains(Name))).ToList();
      }
      #endregion
      var totalRecords = 0;
      var recordsFiltered = 0;
      start = start.HasValue
          ? start / length
          : 0;
      recordsFiltered = mList.Count();
      totalRecords = mList.Count();
      int initialPage = start.Value;
      var filteredResult = mList
          .Skip(initialPage * length ?? 0)
          .Take(length ?? 0)
          .ToList();
      foreach (var item in filteredResult)
      {
        mSearchModel = new ManufacturerModel();
        mSearchModel.ManufacturerID = item.ManufacturerID;
        mSearchModel.ClientLookupId = item.ClientLookupId;
        mSearchModel.Name = item.Name;
        //mSearchModel.Inactive = item.Inactive;
        mSearchModelList.Add(mSearchModel);
      }
      bool showAddBtn = userData.Security.ManufacturerMaster.Create;
      bool showEditBtn = userData.Security.ManufacturerMaster.Edit;
      bool showDeleteBtn = userData.Security.ManufacturerMaster.Delete;
      return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = mSearchModelList, typeList = typeList, showAddBtn= showAddBtn, showEditBtn= showEditBtn, showDeleteBtn= showDeleteBtn });
    }
    private List<ManufacturerModel> GetAllManMasterSortByColumnWithOrder(string order, string orderDir, List<ManufacturerModel> data)
    {
      List<ManufacturerModel> lst = new List<ManufacturerModel>();
      if (lst != null)
      {
        switch (order)
        {
          case "0":
            lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ClientLookupId).ToList() : data.OrderBy(p => p.ClientLookupId).ToList();
            break;
          case "1":
            lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Name).ToList() : data.OrderBy(p => p.Name).ToList();
            break;
        }
      }
      return lst;
    }
    #endregion
    #region Add/Edit
    [HttpGet]
    public PartialViewResult AddManMaster()
    {
      ManufacturerMasterWrapper mWrapper = new ManufacturerMasterWrapper(userData);
      ManufacturerMasterVM mmVM = new ManufacturerMasterVM();
      ManufacturerModel manufacturerModel = new ManufacturerModel();
      mmVM.manufacturerModel = manufacturerModel;
      LocalizeControls(mmVM, LocalizeResourceSetConstants.Global);
      return PartialView("~/Views/Configuration/ManufacturerMaster/AddOrEditManufacturerMaster.cshtml", mmVM);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult AddOrEditManMaster(ManufacturerMasterVM objVM, string Command)
    {
      if (ModelState.IsValid)
      {
        ManufacturerMasterWrapper prtWrapper = new ManufacturerMasterWrapper(userData);
        string Mode = string.Empty;

        List<String> errorList = prtWrapper.AddOrEditManMasterRecord(objVM.manufacturerModel);
        if (errorList != null && errorList.Count > 0)
        {
          return Json(errorList, JsonRequestBehavior.AllowGet);
        }
        else
        {
          if (objVM.manufacturerModel.ManufacturerID == 0)
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
    public ActionResult EditManMaster(long ManufacturerID, string Name, bool Inactive, string ClientLookupId)
    {
      ManufacturerMasterWrapper mWrapper = new ManufacturerMasterWrapper(userData);
      ManufacturerMasterVM manMasterVM = new ManufacturerMasterVM();
      ManufacturerModel manufacturerModel = new ManufacturerModel();
      manufacturerModel.ClientLookupId = ClientLookupId;

      var mList = mWrapper.RetrieveManufacturerMasterDetails(Inactive);
      manMasterVM.manufacturerModel = manufacturerModel;
      //manMasterVM.manufacturerModel.edit = 1;
      manMasterVM.manufacturerModel.ManufacturerID = ManufacturerID;
      manMasterVM.manufacturerModel.ClientLookupId = ClientLookupId;
      manMasterVM.manufacturerModel.Name = Name;
      manMasterVM.manufacturerModel.Inactive = Inactive;
      LocalizeControls(manMasterVM, LocalizeResourceSetConstants.Global);
      return PartialView("~/Views/Configuration/ManufacturerMaster/AddOrEditManufacturerMaster.cshtml", manMasterVM);
    }

    [HttpPost]
    public ActionResult DeleteManufacturerMaster(long ManufacturerID)
    {
      ManufacturerMasterWrapper _PartsObj = new ManufacturerMasterWrapper(userData);
      List<String> errorList = _PartsObj.DeleteManMasterRecord(ManufacturerID);
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
    public string GetManufacturerMasterPrintData(string colname, string coldir, string _manufacturerID, string _name = null, bool _inactiveFlag = false)
    {
      List<ManufacturerMasterPrintModel> manMasterPrintModelList = new List<ManufacturerMasterPrintModel>();
      ManufacturerMasterPrintModel objmanMasterPrintModel;
      ManufacturerMasterWrapper mWrapper = new ManufacturerMasterWrapper(userData);
      var mList = mWrapper.RetrieveManufacturerMasterDetails(_inactiveFlag);
      if (mList != null)
      {
        mList = this.GetAllManMasterSortByColumnWithOrder(colname, coldir, mList);
      }
      if (mList != null)
      {
        if (!string.IsNullOrEmpty(_manufacturerID))
        {
          _manufacturerID = _manufacturerID.ToUpper();
          mList = mList.Where(x => (!string.IsNullOrWhiteSpace(x.ClientLookupId) && x.ClientLookupId.ToUpper().Contains(_manufacturerID))).ToList();
        }
        if (!string.IsNullOrEmpty(_name))
        {
          _name = _name.ToUpper();
          mList = mList.Where(x => (!string.IsNullOrWhiteSpace(x.Name) && x.Name.ToUpper().Contains(_name))).ToList();
        }
        #region Advance-Search
        if (!string.IsNullOrEmpty(_manufacturerID))
        {
          _manufacturerID = _manufacturerID.ToUpper();
          mList = mList.Where(x => (!string.IsNullOrWhiteSpace(x.ClientLookupId) && x.ClientLookupId.ToUpper().Contains(_manufacturerID))).ToList();
        }
        if (!string.IsNullOrEmpty(_name))
        {
          _name = _name.ToUpper();
          mList = mList.Where(x => (!string.IsNullOrWhiteSpace(x.Name) && x.Name.ToUpper().Contains(_name))).ToList();
        }
        #endregion

        foreach (var p in mList)
        {
          objmanMasterPrintModel = new ManufacturerMasterPrintModel();
          objmanMasterPrintModel.ClientLookupId = p.ClientLookupId;
          objmanMasterPrintModel.Name = p.Name;
          //objmanMasterPrintModel.Inactive = p.Inactive;
          manMasterPrintModelList.Add(objmanMasterPrintModel);
        }
      }

      return JsonConvert.SerializeObject(new { data = manMasterPrintModelList }, JsonSerializerDateSettings);
    }
    #endregion

  }
}