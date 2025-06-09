using Client.SecurityService;

using DataContracts;

using Microsoft.AspNet.Identity;

using System;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Client.ActionFilters
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
                else 
                {
                    #region check user Inactive status
                    var data = filterContext.HttpContext.Session["userData"];
                    var userData = (UserData)data;
                    long ClientId = userData.Site.ClientId;
                    long UserInfoid = userData.DatabaseKey.User.UserInfoId;
                    var connectionstring = userData.DatabaseKey.ClientConnectionString;
                    INTDataLayer.BAL.UserInfoBAL uBal = new INTDataLayer.BAL.UserInfoBAL();
                    DataTable dt = new DataTable();
                    dt = uBal.GetUserActiveStatusCount(ClientId, UserInfoid, connectionstring);
                    int UserstatusActiveCount = 0;
                    if (dt.Rows.Count > 0)
                    {
                        UserstatusActiveCount = Convert.ToInt32(dt.Rows[0]["UserActiveStausCount"].ToString());
                    }
                    if (UserstatusActiveCount == 0)
                    {
                        this.HandleUnauthorizedRequest(filterContext);                       

                    }
                    #endregion check user Inactive status
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
                filterContext.Result = principal != null ? new RedirectResult("/error/index") : new RedirectResult("/LogIn/SomaxLogIn" + "?returnUrl=" + Rurl);
            }
        }
    }
}