using Admin.Common;
using Admin.Models;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
namespace Admin.ActionFilters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class SiteUnderMaintennceAttribute : FilterAttribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            int a = 0;
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Session["AdminUserData"] != null)
            {
                var userdata = (DataContracts.UserData)filterContext.HttpContext.Session["AdminUserData"];
                var mydata = userdata.SiteMaint;
                var timeUtc = DateTime.UtcNow;
                TimeZoneInfo easternZone = TimeZoneInfo.FindSystemTimeZoneById("US Eastern Standard Time");
                DateTime easternTime = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, easternZone);
                TimeSpan DowntimeStarttime = mydata.DowntimeStart.Date.TimeOfDay;
                TimeSpan currenttime = easternTime.TimeOfDay;
                TimeSpan DowntimeEndtime = mydata.DowntimeEnd.Date.TimeOfDay;

                var starttimeUtc = mydata.DowntimeStart;
                DateTime starteasternTime = TimeZoneInfo.ConvertTimeFromUtc(starttimeUtc, easternZone);
                TimeSpan starttime = starteasternTime.TimeOfDay;

                DateTime endeasternTime = TimeZoneInfo.ConvertTimeFromUtc(mydata.DowntimeEnd, easternZone);
                TimeSpan endtime = endeasternTime.TimeOfDay;
                if (mydata.SiteMaintenanceId != 0)
                {
                    if (easternTime >= starteasternTime && easternTime <= endeasternTime)
                    {
                        filterContext.Result = new RedirectResult("/Login/UnderMaintenencePage");
                    }
                }
            }
        }
    }
}