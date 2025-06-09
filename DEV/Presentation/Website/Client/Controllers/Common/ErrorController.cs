using Client.Models.Account;

using Microsoft.AspNet.Identity;

using System.Web;
using System.Web.Mvc;

namespace Client.Controllers
{

    public class ErrorController : Controller
    {
        public ActionResult Index()
        {
            return View("error");
        }
        public ActionResult NotFound()
        {
            //FormsAuthentication.SignOut();
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
        public ActionResult UnAuthError()
        {
            ExternalLoginUnaothorizedModel unAuthData = new ExternalLoginUnaothorizedModel();
            if (TempData["UNAUTHDATA"] != null)
            {
                unAuthData = (ExternalLoginUnaothorizedModel)TempData["UNAUTHDATA"];
            }
            else
            {
                return RedirectToAction("somaxlogin", "login");
            }
            foreach (var element in System.Runtime.Caching.MemoryCache.Default)
            {
                System.Runtime.Caching.MemoryCache.Default.Remove(element.Key);
            }
            Session.Clear();
            Session.Abandon();
            HttpContext.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie,
                                            DefaultAuthenticationTypes.ExternalCookie);
            return View("SSOLoginError", unAuthData);
        }
    }
}