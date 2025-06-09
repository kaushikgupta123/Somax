using Admin.ActionFilters;
using Admin.Common;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;

namespace Admin.ActionFilters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class SessionExpiredAttribute : FilterAttribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            int a = 0;
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.ActionDescriptor.GetCustomAttributes(typeof(SkipSessionExpiaryActionFilterAttribute), false).Any())
            {
                return;
            }

            if (!filterContext.HttpContext.Request.IsAjaxRequest())
            {
                if (filterContext.HttpContext.Session["AdminUserData"] == null)
                {
                    string Rurl = !string.IsNullOrWhiteSpace(filterContext.HttpContext.Request.RawUrl) ? filterContext.HttpContext.Request.RawUrl : "";
                    FormsAuthentication.SignOut();
                    filterContext.Result = new RedirectResult(SomaxAppConstants.SessionExpiaryUrl);
                }
            }
            else
            {
                if (filterContext.HttpContext.Session["AdminUserData"] == null)
                {
                    FormsAuthentication.SignOut();
                    filterContext.HttpContext.Response.StatusCode = 999;
                    filterContext.HttpContext.Response.End();
                }
            }       
        }
    }
}