using Client.Controllers;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ViewModels;

namespace Client.ActionFilters
{
    public class SomaxMenuSelectionAttribute : ActionFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            var menulist = (List<MenuVM>)filterContext.HttpContext.Session["MenuDetails"];
            if (menulist!=null && menulist.Any(x => x.MenuType.ToLower() == "config"))
            {
                var menuReturnList = LogInController.GetMenuList("site");
                if (!(menuReturnList.Count == 0 || menuReturnList.Count < 0))
                {
                    filterContext.HttpContext.Session["MenuDetails"] = menuReturnList;
                }
            }
        }
    }
}