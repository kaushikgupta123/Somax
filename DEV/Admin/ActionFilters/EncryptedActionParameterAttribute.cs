using Admin.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Admin.ActionFilters
{
    [AttributeUsage(AttributeTargets.Method)]
    public class EncryptedActionParameterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            try
            {
                Dictionary<string, object> decryptedParameters = new Dictionary<string, object>();
                if (HttpContext.Current.Request.QueryString.Get("q") != null)
                {
                    string encryptedQueryString = HttpContext.Current.Request.QueryString.Get("q");
                    string decrptedString = encryptedQueryString.Decrypt();
                    //string decrptedString = new HRMS_UtilityLayer.EncryptionUL().Decrypt(encryptedQueryString.ToString());
                    string[] paramsArrs = decrptedString.Split('?');

                    for (int i = 0; i < paramsArrs.Length; i++)
                    {
                        string[] paramArr = paramsArrs[i].Split('=');
                        decryptedParameters.Add(paramArr[0], paramArr[1]);
                    }
                }

                var ActionInfo = filterContext.ActionDescriptor;
                ParameterDescriptor[] pars = ActionInfo.GetParameters();

                for (int i = 0; i < decryptedParameters.Count; i++)
                {
                    for (int j = 0; j < pars.Length; j++)
                    {
                        if (pars[j].ParameterName.ToLower() == Convert.ToString(decryptedParameters.Keys.ElementAt(i)).ToLower())
                        {
                            Type type = pars[j].ParameterType;

                            TypeConverter typeConverter = TypeDescriptor.GetConverter(type);
                            object propValue = typeConverter.ConvertFromString(Convert.ToString(decryptedParameters.Values.ElementAt(i)));
                            filterContext.ActionParameters[decryptedParameters.Keys.ElementAt(i)] = propValue;

                            break;
                        }
                    }
                }
                base.OnActionExecuting(filterContext);
            }
            catch
            {
                filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary
                        {
                            { "controller", "Error" },
                            { "action", "index" }
                        });
            }
        }
    }
}