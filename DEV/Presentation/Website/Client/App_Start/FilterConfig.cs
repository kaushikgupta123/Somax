﻿using Client.ActionFilters;
using System.Web;
using System.Web.Mvc;

namespace Client
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new CustomHandleErrorAttribute());
        }
    }
}
