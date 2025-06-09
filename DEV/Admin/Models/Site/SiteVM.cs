using DataContracts;
//using PagedList;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Admin.Models.Site
{
    public class SiteVM : LocalisationBaseVM
    {
        public SiteVM()
        {
            _SiteSummaryModel = new SiteSummaryModel();
            SiteData = new DataContracts.Site();
        }
        public SiteSummaryModel _SiteSummaryModel { get; set; }
        public DataContracts.Site SiteData { get; set; }
        public SiteModel SiteModel { get; set; }
        public SitePrintModel SitePrintModel { get; set; }
        public CreatedLastUpdatedModel _CreatedLastUpdatedModel { get; set; }
        public Security security { get; set; }

        public UserValidateModel UserValidateModel { get; set; }
    }
}