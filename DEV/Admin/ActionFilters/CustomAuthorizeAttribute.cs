using Admin.SecurityService;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Admin.ActionFilters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        public new string[] Roles { get; set; }
        protected virtual CustomPrincipal user
        {
            get { return HttpContext.Current.User as CustomPrincipal; }
        }
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (SkipAuthorization(filterContext)) return;

            if (filterContext.HttpContext.Request.IsAuthenticated)
            {
                if (!this.Roles.Contains("*") && !user.IsInRole(this.Roles))
                {
                    filterContext.Result = new RedirectResult("/error/NotAuthorized");
                }
            }
            else
            {
                this.HandleUnauthorizedRequest(filterContext);
            }
        }
        private static bool SkipAuthorization(AuthorizationContext filterContext)
        {
            return filterContext.ActionDescriptor.GetCustomAttributes(typeof(AllowAnonymousAttribute), true).Any()
                   || filterContext.ActionDescriptor.ControllerDescriptor.GetCustomAttributes(typeof(AllowAnonymousAttribute), true).Any();
        }
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                filterContext.Result = new HttpStatusCodeResult((System.Net.HttpStatusCode)302, "Authentication timeout");
            }
            else
            {
                var principal = (HttpContext.Current.User as CustomPrincipal);
                string Rurl = !string.IsNullOrWhiteSpace(filterContext.HttpContext.Request.RawUrl) ? filterContext.HttpContext.Request.RawUrl : "";
                filterContext.Result = principal != null ? new RedirectResult("/error/index") : new RedirectResult("/Admin/LogIn/AdminLogIn" + "?returnUrl=" + Rurl);
            }
        }
    }
}