using Client.ActionFilters;
using Client.BusinessWrapper;
using Client.Controllers.Common;
using Client.Models;

using Common.Constants;

using System.Web.Mvc;

namespace Client.Controllers.Yurbi
{
    public class YurbiController  : SomaxBaseController
    {
        [CheckUserSecurity(securityType = SecurityConstants.Reports)]
        public ActionResult Index()
        {
            BusinessIntelligenceVM objVM = new BusinessIntelligenceVM();
            BusinessIntelligenceWrapper biWrapper = new BusinessIntelligenceWrapper(userData);
            YurbiReportModel ojYurbiReportModel = new YurbiReportModel();
            ojYurbiReportModel = biWrapper.RetrieveYurbiReportDetails();
            objVM._YurbiReportModel = ojYurbiReportModel;
            LocalizeControls(objVM, LocalizeResourceSetConstants.Report);
            return View(objVM);
        }
    }
}