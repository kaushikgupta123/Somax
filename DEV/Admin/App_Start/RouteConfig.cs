using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Admin
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");


#if DEBUG
            routes.MapRoute(
               name: "Default",
               url: "Admin/{controller}/{action}/{id}",
               defaults: new { controller = "Login", action = "AdminLogin", id = UrlParameter.Optional }
           );
            routes.MapRoute(
               name: "Default1",
               url: "{controller}/{action}/{id}",
               defaults: new { controller = "Login", action = "AdminLogin", id = UrlParameter.Optional }
           );
#else
           routes.MapRoute(
               name: "Default",
               url: "{controller}/{action}/{id}",
               defaults: new { controller = "Login", action = "AdminLogin", id = UrlParameter.Optional }
           );
#endif
        }
    }
}
