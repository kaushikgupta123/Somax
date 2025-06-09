using Admin.Controllers;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ViewModels;

namespace Admin.ActionFilters
{
    public class SomaxMenuSelectionAttribute : ActionFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            var menulist = (List<MenuVM>)filterContext.HttpContext.Session["AdminMenuDetails"];
            if (menulist!=null && menulist.Any(x => x.MenuType.ToLower() == "config"))
            {
                var menuReturnList = LoginController.GetMenuList("site");
                if (!(menuReturnList.Count == 0 || menuReturnList.Count < 0))
                {
                    filterContext.HttpContext.Session["AdminMenuDetails"] = menuReturnList;
                }
            }
        }
    }
}