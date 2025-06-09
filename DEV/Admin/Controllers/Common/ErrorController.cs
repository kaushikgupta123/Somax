using System.Web.Mvc;
using System.Web;

using Microsoft.AspNet.Identity;

namespace Admin.Controllers
{
    public class ErrorController : Controller
    {
        public ActionResult Index()
        {
            return View("error");
        }
        public ActionResult NotFound()
        {
            HttpContext.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie,
                                         DefaultAuthenticationTypes.ExternalCookie);
            Session.Clear();
            Session.Abandon();
            return View("NotFound");
        }
        public ActionResult NotAuthorized()
        {
            return View();
        }
        public ActionResult SessionExpired(string returnUrl)
        {
            ViewBag.returnUrl = returnUrl;
            return View("SessionExpired");
        }
    }
}