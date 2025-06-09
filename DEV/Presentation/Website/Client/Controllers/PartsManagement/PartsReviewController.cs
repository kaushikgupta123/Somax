using Client.ActionFilters;
using Client.BusinessWrapper;
using Client.BusinessWrapper.Common;
using Client.BusinessWrapper.PartsManagement;
using Client.Common;
using Client.Controllers.Common;
using Client.Models.PartsManagement.PartsReview;
using Common.Constants;

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Client.Controllers.PartsManagement
{
    public class PartsReviewController : SomaxBaseController
    {
        [CheckUserSecurity(securityType = SecurityConstants.PartMasterRequest_Review)]
        public ActionResult Index()
        {
            PartsReviewVM partsReviewVM = new PartsReviewVM();
            LocalizeControls(partsReviewVM, LocalizeResourceSetConstants.Global);
            return View(partsReviewVM);
        }
        [HttpPost]
        public string GetPartsReviewMainGrid(int? draw, int? start, int? length, string SearchText = "")
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            var colname = Request.Form.GetValues("columns[" + order + "][name]");
            List<PartsReviewModel> partList;
            PartsReviewWrapper pWrapper = new PartsReviewWrapper(userData);
            CommonWrapper comWrapper = new CommonWrapper(userData);
            //if (!string.IsNullOrWhiteSpace(SearchText))
            //{
            partList = pWrapper.GetPartsReview(SearchText);
                if (partList != null)
                {
                    partList = PartsReviewSortByColumnWithOrder(colname[0], orderDir, partList);
                }
            //}
            //else
            //{
            //    partList = new List<PartsReviewModel>();
            //}
            var totalRecords = 0;
            var recordsFiltered = 0;
            start = start.HasValue
                ? start / length
                : 0;
            recordsFiltered = partList.Count();
            totalRecords = partList.Count();
            int initialPage = start.Value;
            var filteredResult = partList
                .Skip(initialPage * length ?? 0)
                .Take(length ?? 0)
                .ToList();

            bool ClientOnPremise = userData.DatabaseKey.Client.OnPremise;
            foreach (var item in filteredResult)
            {
                if (!string.IsNullOrEmpty(item.ImageURL))
                {
                    if (ClientOnPremise)
                    {
                        item.ImageURL = UtilityFunction.PhotoBase64ImgSrc(item.ImageURL);
                    }
                }
                else
                {
                    item.ImageURL = comWrapper.GetNoImageUrl();
                }
            }
            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult }, JsonSerializerDateSettings);
        }
        private List<PartsReviewModel> PartsReviewSortByColumnWithOrder(string order, string orderDir, List<PartsReviewModel> data)
        {
            List<PartsReviewModel> lst = new List<PartsReviewModel>();
            switch (order)
            {
                case "2":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ClientLookupId).ToList() : data.OrderBy(p => p.ClientLookupId).ToList();
                    break;
                case "3":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.LongDescription).ToList() : data.OrderBy(p => p.LongDescription).ToList();
                    break;
                case "4":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Manufacturer).ToList() : data.OrderBy(p => p.Manufacturer).ToList();
                    break;
                case "5":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ManufacturerId).ToList() : data.OrderBy(p => p.ManufacturerId).ToList();
                    break;
                case "6":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Category).ToList() : data.OrderBy(p => p.Category).ToList();
                    break;
                case "7":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.CategoryMasterDescription).ToList() : data.OrderBy(p => p.CategoryMasterDescription).ToList();
                    break;
                default:
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ClientLookupId).ToList() : data.OrderBy(p => p.ClientLookupId).ToList();
                    break;
            }
            return lst;
        }
        public ActionResult PopulateReviewSite(long PartMasterId)
        {
            PartsWrapper pWrapper = new PartsWrapper(userData);
            PartsReviewVM partsReviewVM = new PartsReviewVM();
            var ReviewSite = pWrapper.PopulateReviewSite(PartMasterId);
            partsReviewVM.reviewSiteModelList = ReviewSite;
            LocalizeControls(partsReviewVM, LocalizeResourceSetConstants.PartDetails);
            return View("_PartsReviewInnerGrid", partsReviewVM);
        }
    }
}