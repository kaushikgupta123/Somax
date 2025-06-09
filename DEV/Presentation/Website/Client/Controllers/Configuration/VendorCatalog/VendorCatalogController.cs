using Client.ActionFilters;
using Client.BusinessWrapper.Configuration.VendorCatalog;
using Client.Controllers.Common;
using Client.Models.Configuration.VendorCatalog;
using Common.Constants;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Client.Controllers.Configuration.VendorCatalog
{
    public class VendorCatalogController : ConfigBaseController
    {
        [CheckUserSecurity(securityType = SecurityConstants.VendorCatalog)]
        public ActionResult Index()
        {
            VendorCatalogVM vendorCatalogVM = new VendorCatalogVM();
            //vendorCatalogVM.security = this.userData.Security;
            LocalizeControls(vendorCatalogVM, LocalizeResourceSetConstants.ConfigMasterDetail);
            return View("~/Views/Configuration/VendorCatalog/index.cshtml", vendorCatalogVM);
        }

        private List<VendorCatalogModel> GetSearchResult(List<VendorCatalogModel> VendorCatalogModelList, string SearchText, string partID, string description, string manufacturer, string manufacturerPartNumber, string category, string categoryDescription, decimal UnitCost, string PurchaseUnit, string VendorPartNumber, string VendorName, string VendorID)
        {
            //-------------text search--------------
            SearchText = SearchText.ToUpper();

            VendorCatalogModelList = VendorCatalogModelList.Where(x =>
                (!string.IsNullOrWhiteSpace(x.ClientLookupId) && x.ClientLookupId.ToUpper().Contains(SearchText))
                                            || (!string.IsNullOrWhiteSpace(x.LongDescription) && x.LongDescription.ToUpper().Contains(SearchText))
                                            || (!string.IsNullOrWhiteSpace(x.Manufacturer) && x.Manufacturer.ToUpper().Contains(SearchText))
                                            || (!string.IsNullOrWhiteSpace(x.ManufacturerId) && x.ManufacturerId.ToUpper().Contains(SearchText))
                                            || (!string.IsNullOrWhiteSpace(x.Category) && x.Category.ToUpper().Contains(SearchText))
                                            || (!string.IsNullOrWhiteSpace(x.CM_Description) && x.CM_Description.ToUpper().Contains(SearchText))
                                            || (x.UnitCost != 0 && x.UnitCost.Equals(SearchText))
                                            || (!string.IsNullOrWhiteSpace(x.VI_PurchaseUOM) && x.VI_PurchaseUOM.ToUpper().Contains(SearchText))
                                            || (!string.IsNullOrWhiteSpace(x.VCI_PartNumber) && x.VCI_PartNumber.ToUpper().Contains(SearchText))
                                            || (!string.IsNullOrWhiteSpace(x.VendorName) && x.VendorName.ToUpper().Contains(SearchText))
                                            || (!string.IsNullOrWhiteSpace(x.VendorClientLookupId) && x.VendorClientLookupId.ToUpper().Contains(SearchText))
                ).ToList();
            //-----------end of text search------------
            
            if (VendorCatalogModelList != null)
            {
                if (!string.IsNullOrEmpty(partID))
                {
                    partID = partID.ToUpper();
                    VendorCatalogModelList = VendorCatalogModelList.Where(x => (!string.IsNullOrWhiteSpace(x.ClientLookupId) && x.ClientLookupId.ToUpper().Contains(partID))).ToList();
                }
                if (!string.IsNullOrEmpty(description))
                {
                    description = description.ToUpper();
                    VendorCatalogModelList = VendorCatalogModelList.Where(x => (!string.IsNullOrWhiteSpace(x.LongDescription) && x.LongDescription.ToUpper().Contains(description))).ToList();
                }
                if (!string.IsNullOrEmpty(manufacturer))
                {
                    manufacturer = manufacturer.ToUpper();
                    VendorCatalogModelList = VendorCatalogModelList.Where(x => (!string.IsNullOrWhiteSpace(x.Manufacturer) && x.Manufacturer.ToUpper().Contains(manufacturer))).ToList();
                }
                if (!string.IsNullOrEmpty(manufacturerPartNumber))
                {
                    manufacturerPartNumber = manufacturerPartNumber.ToUpper();
                    VendorCatalogModelList = VendorCatalogModelList.Where(x => (!string.IsNullOrWhiteSpace(x.ManufacturerId) && x.ManufacturerId.ToUpper().Contains(manufacturerPartNumber))).ToList();
                }
                if (!string.IsNullOrEmpty(category))
                {
                    category = category.ToUpper();
                    VendorCatalogModelList = VendorCatalogModelList.Where(x => (!string.IsNullOrWhiteSpace(x.Category) && x.Category.ToUpper().Contains(category))).ToList();
                }
                if (!string.IsNullOrEmpty(categoryDescription))
                {
                    categoryDescription = categoryDescription.ToUpper();
                    VendorCatalogModelList = VendorCatalogModelList.Where(x => (!string.IsNullOrWhiteSpace(x.CM_Description) && x.CM_Description.ToUpper().Contains(categoryDescription))).ToList();
                }
                if (UnitCost != null)
                {
                    VendorCatalogModelList = VendorCatalogModelList.Where(x => Convert.ToString(x.UnitCost).Contains(Convert.ToString(UnitCost))).ToList();
                }
                if (!string.IsNullOrEmpty(PurchaseUnit))
                {
                    PurchaseUnit = PurchaseUnit.ToUpper();
                    VendorCatalogModelList = VendorCatalogModelList.Where(x => (!string.IsNullOrWhiteSpace(x.VI_PurchaseUOM) && x.VI_PurchaseUOM.ToUpper().Contains(PurchaseUnit))).ToList();
                }
                if (!string.IsNullOrEmpty(VendorPartNumber))
                {
                    VendorPartNumber = VendorPartNumber.ToUpper();
                    VendorCatalogModelList = VendorCatalogModelList.Where(x => (!string.IsNullOrWhiteSpace(x.VCI_PartNumber) && x.VCI_PartNumber.ToUpper().Contains(VendorPartNumber))).ToList();
                }
                if (!string.IsNullOrEmpty(VendorName))
                {
                    VendorName = VendorName.ToUpper();
                    VendorCatalogModelList = VendorCatalogModelList.Where(x => (!string.IsNullOrWhiteSpace(x.VendorName) && x.VendorName.ToUpper().Contains(VendorName))).ToList();
                }
                if (!string.IsNullOrEmpty(VendorID))
                {
                    VendorID = VendorID.ToUpper();
                    VendorCatalogModelList = VendorCatalogModelList.Where(x => (!string.IsNullOrWhiteSpace(x.VendorClientLookupId) && x.VendorClientLookupId.ToUpper().Contains(VendorID))).ToList();
                }
            }
            return VendorCatalogModelList;
        }

        [HttpPost]
        public string GetVendorCatalogGrid(int? draw, int? start, int? length, string srchText = "", string partID = "", string description = "", string manufacturer = "", string manufacturerPartNumber = "", string category = "", string categoryDescription = "", decimal UnitCost = 0, string PurchaseUnit = "", string VendorPartNumber = "", string VendorName = "", string VendorID = "")
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            var colname = Request.Form.GetValues("columns[" + order + "][name]");
            VendorCatalogWrapper vcWrapper = new VendorCatalogWrapper(userData);
            var vcList = vcWrapper.GetVendorCatData(srchText);
            if (vcList != null)
            {
                vcList = GetSearchResult(vcList, srchText,  partID,  description,  manufacturer,  manufacturerPartNumber,  category,  categoryDescription,  UnitCost,  PurchaseUnit,  VendorPartNumber,  VendorName,  VendorID);
                vcList = GetVendorCatalogGridSortByColumnWithOrder(colname[0], orderDir, vcList);
            }
            var totalRecords = 0;
            var recordsFiltered = 0;
            start = start.HasValue
                ? start / length
                : 0;
            recordsFiltered = vcList.Count();
            totalRecords = vcList.Count();
            int initialPage = start.Value;
            var filteredResult = vcList
                .Skip(initialPage * length ?? 0)
                .Take(length ?? 0)
                .ToList();
            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult }, JsonSerializerDateSettings);
        }

        private List<VendorCatalogModel> GetVendorCatalogGridSortByColumnWithOrder(string order, string orderDir, List<VendorCatalogModel> data)
        {
            List<VendorCatalogModel> lst = new List<VendorCatalogModel>();
            if (data != null)
            {
                switch (order)
                {
                    case "0":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ClientLookupId).ToList() : data.OrderBy(p => p.ClientLookupId).ToList();
                        break;
                    case "1":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.LongDescription).ToList() : data.OrderBy(p => p.LongDescription).ToList();
                        break;
                    case "2":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Manufacturer).ToList() : data.OrderBy(p => p.Manufacturer).ToList();
                        break;
                    case "3":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ManufacturerId).ToList() : data.OrderBy(p => p.ManufacturerId).ToList();
                        break;
                    case "4":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Category).ToList() : data.OrderBy(p => p.Category).ToList();
                        break;
                    case "5":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.CM_Description).ToList() : data.OrderBy(p => p.CM_Description).ToList();
                        break;
                    case "6":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.UnitCost).ToList() : data.OrderBy(p => p.UnitCost).ToList();
                        break;
                    case "7":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.VI_PurchaseUOM).ToList() : data.OrderBy(p => p.VI_PurchaseUOM).ToList();
                        break;
                    case "8":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.VCI_PartNumber).ToList() : data.OrderBy(p => p.VCI_PartNumber).ToList();
                        break;
                    case "9":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.VendorName).ToList() : data.OrderBy(p => p.VendorName).ToList();
                        break;
                    case "10":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.VendorClientLookupId).ToList() : data.OrderBy(p => p.VendorClientLookupId).ToList();
                        break;
                    default:
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ClientLookupId).ToList() : data.OrderBy(p => p.ClientLookupId).ToList();
                        break;
                }
            }
            return lst;
        }
    }
}