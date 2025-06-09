using Client.ActionFilters;
using Client.Controllers.Common;
using Common.Constants;
using System.Web.Mvc;

namespace Client.Controllers
{

    public class WorkOrderStatusController : SomaxBaseController
    {
        [CheckUserSecurity(securityType = SecurityConstants.Analytics_WorkOrderStatus)]
        public ActionResult Index()
        {
            return View();
        }
    }
}
