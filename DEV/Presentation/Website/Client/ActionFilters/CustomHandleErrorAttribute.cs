using Client.Common;
using DataContracts;
using System;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace Client.ActionFilters
{
    public class CustomHandleErrorAttribute : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            UserData user = new UserData();
            string fPath = "~/ErrorLog/" + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString("00") + DateTime.Now.Day.ToString("00");
            string errorFilePath = string.Empty;
            int statusCode = 0;

            if (filterContext.ExceptionHandled || !filterContext.HttpContext.IsCustomErrorEnabled)
            {
                return;
            }

            if (new HttpException(null, filterContext.Exception).GetHttpCode() != 500)
            {
                return;
            }

            if (!ExceptionType.IsInstanceOfType(filterContext.Exception))
            {
                return;
            }

            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                filterContext.HttpContext.Response.StatusCode = 888;
                filterContext.HttpContext.Response.End();
            }
            else
            {
                var controllerName = (string)filterContext.RouteData.Values["controller"];
                var actionName = (string)filterContext.RouteData.Values["action"];
                var model = new HandleErrorInfo(filterContext.Exception, controllerName, actionName);

                filterContext.Result = new ViewResult
                {
                    ViewName = View,
                    MasterName = Master,
                    ViewData = new ViewDataDictionary(model),
                    TempData = filterContext.Controller.TempData
                };
                statusCode = 500;
                filterContext.ExceptionHandled = true;
                filterContext.HttpContext.Response.Clear();
                filterContext.HttpContext.Response.StatusCode = statusCode;

                filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
            }
            try
            {
                if (filterContext.HttpContext.Session["userData"] != null)
                {
                    user = (UserData)filterContext.HttpContext.Session["userData"];
                }
                Exception ex = filterContext.Exception;

                string errorFolderPath = filterContext.HttpContext.Server.MapPath(fPath);

                if (!Directory.Exists(errorFolderPath))
                {
                    Directory.CreateDirectory(errorFolderPath);
                }

                if (user != null && user.DatabaseKey != null)
                {
                    errorFilePath = fPath + "/" + user.DatabaseKey.UserName + ".txt";
                }
                else
                {
                    errorFilePath = fPath + "/error.txt";
                }

                UtilityFunction.LogException(errorFilePath, ex, user);

            }
            catch (Exception ex)
            {
                string errorFolderPath = filterContext.HttpContext.Server.MapPath(fPath);

                if (!Directory.Exists(errorFolderPath))
                {
                    Directory.CreateDirectory(errorFolderPath);
                }
                errorFilePath = fPath + "/error.txt";

                UtilityFunction.LogException(errorFilePath, ex, user);

            }
        }
    }
}