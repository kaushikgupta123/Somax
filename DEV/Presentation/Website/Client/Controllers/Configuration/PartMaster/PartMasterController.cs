using Client.ActionFilters;
using Client.BusinessWrapper.Common;
using Client.BusinessWrapper.Configuration;
//using Client.BusinessWrapper.Configuration.PartMaster;
using Client.Common;
using Client.Controllers.Common;
using Client.Models.Configuration.PartMaster;
using Common.Constants;
using Data.DataContracts;
using DataContracts;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Client.Controllers.Configuration.PartMaster
{
    public class PartMasterController : ConfigBaseController
    {
        public static CommonWrapper comWrapper { get; set; }
        [CheckUserSecurity(securityType = SecurityConstants.PartMaster)]
        public ActionResult Index()
        {
            comWrapper = new CommonWrapper(userData);
            PartMasterVM partMasterVM = new PartMasterVM();
            partMasterVM.security = userData.Security;
            LocalizeControls(partMasterVM, LocalizeResourceSetConstants.ConfigMasterDetail);
            return View("~/Views/Configuration/PartMaster/index.cshtml", partMasterVM);
        }

        #region Search

        private List<PartMasterModel> GetEqMasterSearchResult(List<PartMasterModel> PMasterList, string partid, string manufacturerid, string manufacturer, string category, string catdescription, string description, bool inactiveFlag)
        {
            if (PMasterList != null)
            {
                if (!string.IsNullOrEmpty(partid))
                {
                    partid = partid.ToUpper();
                    PMasterList = PMasterList.Where(x => (!string.IsNullOrWhiteSpace(x.ClientLookupId) && x.ClientLookupId.ToUpper().Contains(partid))).ToList();
                }

                if (!string.IsNullOrEmpty(manufacturerid))
                {
                    manufacturerid = manufacturerid.ToUpper();
                    PMasterList = PMasterList.Where(x => (!string.IsNullOrWhiteSpace(x.ManufacturerId) && x.ManufacturerId.ToUpper().Equals(manufacturerid))).ToList();
                }
                if (!string.IsNullOrEmpty(manufacturer))
                {
                    manufacturer = manufacturer.ToUpper();
                    PMasterList = PMasterList.Where(x => (!string.IsNullOrWhiteSpace(x.Manufacturer) && x.Manufacturer.ToUpper().Equals(manufacturer))).ToList();
                }
                if (!string.IsNullOrEmpty(category))
                {
                    category = category.ToUpper();
                    PMasterList = PMasterList.Where(x => (!string.IsNullOrWhiteSpace(x.Category) && x.Category.ToUpper().Equals(category))).ToList();
                }
                if (!string.IsNullOrEmpty(catdescription))
                {
                    catdescription = catdescription.ToUpper();
                    PMasterList = PMasterList.Where(x => (!string.IsNullOrWhiteSpace(x.CategoryDescription) && x.CategoryDescription.ToUpper().Equals(catdescription))).ToList();
                }
                if (!string.IsNullOrEmpty(description))
                {
                    description = description.ToUpper();
                    PMasterList = PMasterList.Where(x => (!string.IsNullOrWhiteSpace(x.ShortDescription) && x.ShortDescription.ToUpper().Contains(description))).ToList();
                }
            }
            return PMasterList;
        }


        [HttpPost]
        public string GetPartMasterGrid(int? draw, int? start, int? length, string partid = "", string manufacturerid = "", string manufacturer = "", string category = "", string catdescription = "", string description = "", bool inactiveFlag = false)
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            var colname = Request.Form.GetValues("columns[" + order + "][name]");
            PartMasterWrapper partMasterWrapper = new PartMasterWrapper(userData);
            var PMasterList = partMasterWrapper.GetPartMasterData(inactiveFlag);
            if (PMasterList != null)
            {
                PMasterList = GetEqMasterSearchResult(PMasterList, partid, manufacturerid, manufacturer, category, catdescription, description, inactiveFlag);
                //PMasterList = this.GetEquipMastGridSortByColumnWithOrder(colname[0], orderDir, PMasterList);
            }

            //Configure Dropdown for Adv Search
            var ManufactureAdvList = PMasterList.Where(m => m.Manufacturer != String.Empty).Select(m => m.Manufacturer).Distinct().ToList();
            var ManufacturePartIdAdvList = PMasterList.Where(m => m.ManufacturerId != String.Empty).Select(m => m.ManufacturerId).Distinct().ToList();
            var CategoryDescriptionAdvList = PMasterList.Where(m => m.CategoryDescription != String.Empty).Select(m => m.CategoryDescription).Distinct().ToList();
            var catList = PMasterList.Where(m => m.Category != String.Empty).Select(m => m.Category).Distinct().ToList();

            PMasterList = this.GetAllPartMasterSortByColumnWithOrder(colname[0], orderDir, PMasterList);

            var totalRecords = 0;
            var recordsFiltered = 0;
            start = start.HasValue
                ? start / length
                : 0;
            recordsFiltered = PMasterList.Count();
            totalRecords = PMasterList.Count();
            int initialPage = start.Value;
            var filteredResult = PMasterList
                .Skip(initialPage * length ?? 0)
                .Take(length ?? 0)
                .ToList();
            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, catList = catList, ManufactureAdvList = ManufactureAdvList, ManufacturePartIdAdvList = ManufacturePartIdAdvList, CategoryDescriptionAdvList = CategoryDescriptionAdvList, data = filteredResult }, JsonSerializerDateSettings);
        }


        private List<PartMasterModel> GetAllPartMasterSortByColumnWithOrder(string order, string orderDir, List<PartMasterModel> data)
        {
            List<PartMasterModel> lst = new List<PartMasterModel>();
            switch (order)
            {
                case "0":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ClientLookupId).ToList() : data.OrderBy(p => p.ClientLookupId).ToList();
                    break;
                case "1":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Manufacturer).ToList() : data.OrderBy(p => p.Manufacturer).ToList();
                    break;
                case "2":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ManufacturerId).ToList() : data.OrderBy(p => p.ManufacturerId).ToList();
                    break;

                case "3":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Category).ToList() : data.OrderBy(p => p.Category).ToList();
                    break;
                case "4":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.CategoryDescription).ToList() : data.OrderBy(p => p.CategoryDescription).ToList();
                    break;
                case "5":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ShortDescription).ToList() : data.OrderBy(p => p.ShortDescription).ToList();
                    break;
                default:
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.PartId).ToList() : data.OrderBy(p => p.PartId).ToList();
                    break;
            }
            return lst;
        }


        public string GetPartMasterPrintData(string colname, string coldir, string partid = "", string manufacturer = "", string manufacturerid = "", string category = "", string catdescription = "", string description = "", bool inactiveFlag = false)
        {
            PartMasterPrintModel partMasterPrintModel;
            List<PartMasterPrintModel> partMasterPrintModelList = new List<PartMasterPrintModel>();
            PartMasterWrapper partMasterWrapper = new PartMasterWrapper(userData);

            var PMasterList = partMasterWrapper.GetPartMasterData(inactiveFlag);
            if (PMasterList != null)
            {
                PMasterList = GetEqMasterSearchResult(PMasterList, partid, manufacturerid, manufacturer, category, catdescription, description, inactiveFlag);
                PMasterList = this.GetAllPartMasterSortByColumnWithOrder(colname, coldir, PMasterList);
            }
            foreach (var pm in PMasterList)
            {
                partMasterPrintModel = new PartMasterPrintModel();
                partMasterPrintModel.ClientLookupId = pm.ClientLookupId;
                partMasterPrintModel.Manufacturer = pm.Manufacturer;
                partMasterPrintModel.ManufacturerId = pm.ManufacturerId;
                partMasterPrintModel.Category = pm.Category;
                partMasterPrintModel.CategoryDescription = pm.CategoryDescription;
                partMasterPrintModel.ShortDescription = pm.ShortDescription;
                partMasterPrintModelList.Add(partMasterPrintModel);
            }

            return JsonConvert.SerializeObject(new { data = partMasterPrintModelList }, JsonSerializerDateSettings);
        }


        #endregion Search


        #region Details
        public PartialViewResult PartMasterDetails(long partMasterId, bool delf = false)
        {
            PartMasterVM partMasterVM = new PartMasterVM();
            PartMasterWrapper partMasterWrapper = new PartMasterWrapper(userData);
            PartMasterModel partMasterModel = new PartMasterModel();
            partMasterModel = partMasterWrapper.GetPartMasterDetails(partMasterId);
            partMasterVM.security = this.userData.Security;

            List<PartCategoryMaster> obj_PMlist = partMasterWrapper.GetCategoryList();
            List<DataContracts.LookupList> UoMeasure = partMasterWrapper.GetUnitofMeasureList().Where(x => x.ListName == LookupListConstants.Unit_Of_Measure).ToList();

            var CategoryType = obj_PMlist.Where(m => m.ClientLookupId == partMasterModel.Category).FirstOrDefault();
            if (CategoryType != null)
            {
                partMasterModel.CategoryDescription = CategoryType.Description;
            }

            var UnitofMeasureType = UoMeasure.Where(m => m.ListValue == partMasterModel.UnitOfMeasure).FirstOrDefault();
            if (UnitofMeasureType != null)
            {
                partMasterModel.UnitofMeasureDescription = UnitofMeasureType.Description;
            }
            partMasterVM.security = userData.Security;
            partMasterVM.delf = delf;
            partMasterVM.ShowDeleteBtnAfterUpload = userData.Security.PartMaster.Edit;
            bool ClientOnPremise = userData.DatabaseKey.Client.OnPremise;
            if (ClientOnPremise)
            {
                partMasterModel.ImageURL = comWrapper.GetOnPremiseImageUrl(partMasterId, AttachmentTableConstant.PartMaster);
            }
            else
            {
                partMasterModel.ImageURL = comWrapper.GetAzureImageUrl(partMasterId, AttachmentTableConstant.PartMaster);
            }           
            partMasterVM.PartMasterModel = partMasterModel;

            LocalizeControls(partMasterVM, LocalizeResourceSetConstants.ConfigMasterDetail);
            return PartialView("~/Views/Configuration/PartMaster/_PartMasterDetails.cshtml", partMasterVM);
        }
        public JsonResult GetImageURL(long partMasterId)
        {
            PartMasterWrapper partMasterWrapper = new PartMasterWrapper(userData);
            PartMasterModel partMasterModel = new PartMasterModel();
            partMasterModel = partMasterWrapper.GetPartMasterDetails(partMasterId);
            return null;
        }
        #endregion Details
        #region Photos
        public JsonResult DeleteImageFromAzure(string _PartMasterId, string TableName, bool Profile, bool Image)
        {
            string isSuccess = string.Empty;
            comWrapper.DeleteAzureImage(Convert.ToInt64(_PartMasterId), AttachmentTableConstant.PartMaster, ref isSuccess);
            return Json(isSuccess.ToLower(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult DeleteImageFromOnPremise(string _PartMasterId, string TableName, bool Profile, bool Image)
        {
            string isSuccess = string.Empty;
            comWrapper.DeleteOnPremiseImage(Convert.ToInt64(_PartMasterId), AttachmentTableConstant.PartMaster, ref isSuccess);
            return Json(isSuccess.ToLower(), JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Add_Edit
        public ActionResult AddPartMaster()
        {
            PartMasterVM partMasterVM = new PartMasterVM();
            PartMasterModel partMasterModel = new PartMasterModel();
            PartMasterWrapper partWrapper = new PartMasterWrapper(userData);
            List<DataContracts.LookupList> Type = new List<DataContracts.LookupList>();

            //Configure Category
            List<PartCategoryMaster> obj_PMlist = partWrapper.GetCategoryList();
            PartCategoryMaster categoryMaster = new PartCategoryMaster();
            if (obj_PMlist != null)
            {
                partMasterModel.CategoryDescriptionList = obj_PMlist.Select(x => new SelectListItem { Text = x.ClientLookupId.ToString() + "-" + x.Description, Value = x.ClientLookupId.ToString() });
            }
            partMasterVM.PartMasterModel = partMasterModel;

            //Configure UnitofMeasure
            List<DataContracts.LookupList> UoMeasure = new List<DataContracts.LookupList>();
            UoMeasure = partWrapper.GetUnitofMeasureList();
            if (UoMeasure != null)
            {
                UoMeasure = UoMeasure.Where(x => x.ListName == LookupListConstants.Unit_Of_Measure).ToList();
                partMasterModel.UnitofMeasureList = UoMeasure.Select(x => new SelectListItem { Text = x.ListValue.ToString() + "-" + x.Description, Value = x.ListValue.ToString() });
            }

            LocalizeControls(partMasterVM, LocalizeResourceSetConstants.ConfigMasterDetail);
            return PartialView("~/Views/Configuration/PartMaster/AddOrEditPartMaster.cshtml", partMasterVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddPartMaster(PartMasterVM objVM, string Command)
        {
            if (ModelState.IsValid)
            {
                PartMasterWrapper prtWrapper = new PartMasterWrapper(userData);
                string Mode = string.Empty;
                long partMasterId = 0;
                List<String> errorList = prtWrapper.AddOrEditPartMaster(objVM.PartMasterModel, ref Mode, ref partMasterId);
                if (errorList != null && errorList.Count > 0)
                {
                    return Json(errorList, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), partMasterId = partMasterId, Command = Command, mode = Mode }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                string ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult EditPartMaster(long partid, string clientlookupid, string manufacturer, string manufacturerid, string category, string description, decimal unitcost, string unitofmeasure, string longdescription, long altpartid, string upccode, bool oempart, bool inactiveFlag)
        {
            PartMasterVM partMasterVM = new PartMasterVM();
            PartMasterWrapper partWrapper = new PartMasterWrapper(userData);
            PartMasterModel partMasterModel = new PartMasterModel()
            {
                ClientLookupId = clientlookupid,
                PartMasterId = partid,
                Manufacturer = manufacturer,
                ManufacturerId = manufacturerid,
                Category = category,
                ShortDescription = description,
                UnitCost = unitcost,
                UnitOfMeasure = unitofmeasure,
                LongDescription = longdescription,
                EXPartId = altpartid,
                UPCCode = upccode,
                OEMPart = oempart,
                InactiveFlag = inactiveFlag
            };
            //Configure Category
            List<PartCategoryMaster> obj_PMlist = partWrapper.GetCategoryList();
            PartCategoryMaster categoryMaster = new PartCategoryMaster();
            if (obj_PMlist != null)
            {
                partMasterModel.CategoryDescriptionList = obj_PMlist.Select(x => new SelectListItem { Text = x.ClientLookupId.ToString() + "-" + x.Description, Value = x.ClientLookupId.ToString() });
            }


            //Configure UnitofMeasure
            List<DataContracts.LookupList> UoMeasure = new List<DataContracts.LookupList>();
            UoMeasure = partWrapper.GetUnitofMeasureList();
            if (UoMeasure != null)
            {
                UoMeasure = UoMeasure.Where(x => x.ListName == LookupListConstants.Unit_Of_Measure).ToList();
                partMasterModel.UnitofMeasureList = UoMeasure.Select(x => new SelectListItem { Text = x.ListValue.ToString() + "-" + x.Description, Value = x.ListValue.ToString() });
            }
            partMasterVM.PartMasterModel = partMasterModel;
            LocalizeControls(partMasterVM, LocalizeResourceSetConstants.ConfigMasterDetail);
            return PartialView("~/Views/Configuration/PartMaster/AddOrEditPartMaster.cshtml", partMasterVM);
        }


        #endregion Add_Edit


        public string GetPartManufacturerLookupList(int? draw, int? start, int? length, string ClientLookupId = "", string Name = "")
        {
            List<ManufacturerMasterModel> model = new List<ManufacturerMasterModel>();
            PartMasterWrapper partMasterWrapper = new PartMasterWrapper(userData);
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];

            start = start.HasValue
              ? start / length
              : 0;
            int skip = start * length ?? 0;

            ClientLookupId = !string.IsNullOrEmpty(ClientLookupId) ? ClientLookupId.Trim() : string.Empty;
            Name = !string.IsNullOrEmpty(Name) ? Name.Trim() : string.Empty;


            model = partMasterWrapper.PopulateManufacturerIds(start.Value, length.Value, order, orderDir, ClientLookupId, Name);
            long totalRecords = 0;
            long recordsFiltered = 0;
            if (model != null && model.Count > 0)
            {
                totalRecords = model[0].TotalCount;
                recordsFiltered = model[0].TotalCount;
            }

            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = model });
        }


    }
}