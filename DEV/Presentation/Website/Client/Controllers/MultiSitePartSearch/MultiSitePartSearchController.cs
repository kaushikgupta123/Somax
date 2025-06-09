using Client.ActionFilters;
using Client.BusinessWrapper.MultiSitePartSearch;
using Client.Controllers.Common;
using Client.Models.MultiSitePartSearch;
using Common.Constants;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Client.Controllers.MultiSitePartSearch
{
    public class MultiSitePartSearchController : SomaxBaseController
    {
        [CheckUserSecurity(securityType = SecurityConstants.Parts_Multi_Site_Search)]
        public ActionResult Index()
        {
            MultiSitePartSearchVM multiSitePartSearchVM = new MultiSitePartSearchVM();
            LocalizeControls(multiSitePartSearchVM, LocalizeResourceSetConstants.PartDetails);
            return View("~/Views/MultiSitePartSearch/index.cshtml", multiSitePartSearchVM);
        }

        private List<MultiSitePartSearchModel> GetSearchResult(List<MultiSitePartSearchModel> MultiSitePartSearchModelList, string ClientLookupId, string Description, decimal Quantity, string Manufacturer, string ManufacturerId, string City, string State, string Name)
        {


            if (MultiSitePartSearchModelList != null)
            {
                if (!string.IsNullOrEmpty(ClientLookupId))
                {
                    ClientLookupId = ClientLookupId.ToUpper();
                    MultiSitePartSearchModelList = MultiSitePartSearchModelList.Where(x => (!string.IsNullOrWhiteSpace(x.ClientLookupId) && x.ClientLookupId.ToUpper().Contains(ClientLookupId))).ToList();
                }
                if (!string.IsNullOrEmpty(Description))
                {
                    Description = Description.ToUpper();
                    MultiSitePartSearchModelList = MultiSitePartSearchModelList.Where(x => (!string.IsNullOrWhiteSpace(x.Description) && x.Description.ToUpper().Contains(Description))).ToList();
                }
                if (Quantity != null)
                {
                    MultiSitePartSearchModelList = MultiSitePartSearchModelList.Where(x => Convert.ToString(x.Quantity).Contains(Convert.ToString(Quantity))).ToList();
                }
                if (!string.IsNullOrEmpty(Manufacturer))
                {
                    Manufacturer = Manufacturer.ToUpper();
                    MultiSitePartSearchModelList = MultiSitePartSearchModelList.Where(x => (!string.IsNullOrWhiteSpace(x.Manufacturer) && x.Manufacturer.ToUpper().Contains(Manufacturer))).ToList();
                }
                if (!string.IsNullOrEmpty(ManufacturerId))
                {
                    ManufacturerId = ManufacturerId.ToUpper();
                    MultiSitePartSearchModelList = MultiSitePartSearchModelList.Where(x => (!string.IsNullOrWhiteSpace(x.ManufacturerId) && x.ManufacturerId.ToUpper().Contains(ManufacturerId))).ToList();
                }
                if (!string.IsNullOrEmpty(City))
                {
                    City = City.ToUpper();
                    MultiSitePartSearchModelList = MultiSitePartSearchModelList.Where(x => (!string.IsNullOrWhiteSpace(x.City) && x.City.ToUpper().Contains(City))).ToList();
                }
                if (!string.IsNullOrEmpty(State))
                {
                    State = State.ToUpper();
                    MultiSitePartSearchModelList = MultiSitePartSearchModelList.Where(x => (!string.IsNullOrWhiteSpace(x.State) && x.State.ToUpper().Contains(State))).ToList();
                }
                if (!string.IsNullOrEmpty(Name))
                {
                    Name = Name.ToUpper();
                    MultiSitePartSearchModelList = MultiSitePartSearchModelList.Where(x => (!string.IsNullOrWhiteSpace(x.Name) && x.Name.ToUpper().Contains(Name))).ToList();
                }
            }
            return MultiSitePartSearchModelList;
        }
        [HttpPost]
        public string GetMultiSitePartSearchGrid(int? draw, int? start, int? length, string srchText = "", string ClientLookupId = "", string Description = "", decimal Quantity = 0, string Manufacturer = "", string ManufacturerId = "", string City = "", string State = "", string Name = "")
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            var colname = Request.Form.GetValues("columns[" + order + "][name]");
            MultiSitePartSearchWrapper multiSitePartWrapper = new MultiSitePartSearchWrapper(userData);
            var multiSitePartList = multiSitePartWrapper.GetMultiSitePartSearchData(srchText);
            if (multiSitePartList != null)
            {
                multiSitePartList = GetSearchResult(multiSitePartList, ClientLookupId, Description, Quantity, Manufacturer, ManufacturerId, City, State, Name);
                multiSitePartList = GetMultiSitePartSearchGridSortByColumnWithOrder(colname[0], orderDir, multiSitePartList);
            }
            var totalRecords = 0;
            var recordsFiltered = 0;
            start = start.HasValue
                ? start / length
                : 0;
            recordsFiltered = multiSitePartList.Count();
            totalRecords = multiSitePartList.Count();
            int initialPage = start.Value;
            var filteredResult = multiSitePartList
                .Skip(initialPage * length ?? 0)
                .Take(length ?? 0)
                .ToList();
            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult }, JsonSerializerDateSettings);
        }
        private List<MultiSitePartSearchModel> GetMultiSitePartSearchGridSortByColumnWithOrder(string order, string orderDir, List<MultiSitePartSearchModel> data)
        {
            List<MultiSitePartSearchModel> lst = new List<MultiSitePartSearchModel>();
            if (data != null)
            {
                switch (order)
                {
                    case "0":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ClientLookupId).ToList() : data.OrderBy(p => p.ClientLookupId).ToList();
                        break;
                    case "1":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Description).ToList() : data.OrderBy(p => p.Description).ToList();
                        break;
                    case "2":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Quantity).ToList() : data.OrderBy(p => p.Quantity).ToList();
                        break;
                    case "3":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Manufacturer).ToList() : data.OrderBy(p => p.Manufacturer).ToList();
                        break;
                    case "4":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ManufacturerId).ToList() : data.OrderBy(p => p.ManufacturerId).ToList();
                        break;
                    case "5":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.City).ToList() : data.OrderBy(p => p.City).ToList();
                        break;
                    case "6":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.State).ToList() : data.OrderBy(p => p.State).ToList();
                        break;
                    case "7":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Name).ToList() : data.OrderBy(p => p.Name).ToList();
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