using System;
using System.Web;
using System.Web.Mvc;

namespace Admin.ActionFilters
{
    public class NoCacheActionFilterAttribute : ActionFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            filterContext.HttpContext.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            filterContext.HttpContext.Response.Cache.SetCacheability(HttpCacheability.NoCache); //Cache-Control : no-cache, Pragma : no-cache
            filterContext.HttpContext.Response.Cache.SetExpires(DateTime.Now.AddDays(-1)); //Expires : date time
            filterContext.HttpContext.Response.Cache.SetNoStore(); //Cache-Control :  no-store
            filterContext.HttpContext.Response.Cache.SetProxyMaxAge(new TimeSpan(0, 0, 0)); //Cache-Control: s-maxage=0
            filterContext.HttpContext.Response.Cache.SetValidUntilExpires(false);
            filterContext.HttpContext.Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);//Cache-Control:  must-revalidate
            filterContext.HttpContext.Response.Cache.SetAllowResponseInBrowserHistory(false);
        }
    }
}