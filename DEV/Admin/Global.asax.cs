using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Admin
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            UnityConfig.RegisterComponents();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            #region  Custom Provider for Display name localization
            ModelMetadataProviders.Current = new Localization.MetadataProvider();
            #endregion

            #region Custom Provider for Data annotation error message localization

            var provider = ModelValidatorProviders.Providers.FirstOrDefault(p => p.GetType() == typeof(DataAnnotationsModelValidatorProvider));
            if (provider != null)
            {
                ModelValidatorProviders.Providers.Remove(provider);
            }
            ModelValidatorProviders.Providers.Add(new Admin.Localization.LocalizableModelValidatorProvider());

            #endregion

            #region Date conversion
            var binder = new Models.CustomModelBinder.DateTimeModelBinder();
            ModelBinders.Binders.Add(typeof(DateTime), binder);
            ModelBinders.Binders.Add(typeof(DateTime?), binder);
            #endregion 
        }

    }
}
