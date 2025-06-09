using DevExpress.XtraReports.Web;
using DevExpress.XtraReports.Web.Azure.ReportDesigner;
using DevExpress.XtraReports.Web.Azure.WebDocumentViewer;
using DevExpress.XtraReports.Web.WebDocumentViewer;

using System;
using System.Configuration;
using System.Linq;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

using Utility;

namespace Client
{
    public class MvcApplication : System.Web.HttpApplication
    {


        RedisPubSubHelper redisPubSubHelper = new RedisPubSubHelper();
        protected void Application_Start()
        {
            ConfigureViewEngines();
            MvcHandler.DisableMvcResponseHeader = true;

            //V2-1177 Registering the DashboardConfig to use DevExpress Dashboard
            DashboardConfig.RegisterService(RouteTable.Routes);

            //AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            BundleConfig.RegisterBundles(BundleTable.Bundles);
            string AccountName = System.Configuration.ConfigurationManager.AppSettings["AzureAccountName"].ToString();
            string AccountKey = System.Configuration.ConfigurationManager.AppSettings["AzureAccountKey"].ToString();
            //AccountManager.RegisterAzure("FileManagerAzureAccount", AccountName, AccountKey);

            #region  Custom Provider for Display name localization
            ModelMetadataProviders.Current = new Client.Localization.MetadataProvider();
            #endregion

            #region Custom Provider for Data annotation error message localization

            var provider = ModelValidatorProviders.Providers.FirstOrDefault(p => p.GetType() == typeof(DataAnnotationsModelValidatorProvider));
            if (provider != null)
            {
                ModelValidatorProviders.Providers.Remove(provider);
            }
            ModelValidatorProviders.Providers.Add(new Client.Localization.LocalizableModelValidatorProvider());

            #endregion

            var binder = new Client.Models.CustomModelBinder.DateTimeModelBinder();
            ModelBinders.Binders.Add(typeof(DateTime), binder);
            ModelBinders.Binders.Add(typeof(DateTime?), binder);

            #region Redis Pub/Sub Connect & Subscribe

            redisPubSubHelper.Connect();
            redisPubSubHelper.Subscribe();
            #endregion

            #region Devexpress
            DevExpress.Utils.AzureCompatibility.Enable = true;

            DevExpress.XtraPrinting.PrintingOptions.Pdf.RenderingEngine = DevExpress.XtraPrinting.XRPdfRenderingEngine.Skia;

            // Needed to retrieve the value from web.config only because we will only be able to fetch the client details after login
            // So we can't use here Client.OnPresmise
            #region Cache saving
            bool? Type = Convert.ToBoolean(ConfigurationManager.AppSettings["ClientOnPremise"].ToString());
            if (Type != null && Type == true)
            {
                DevexpressFileSystemDataSave();
            }
            else
            {
                DevexpressAzureDataSave();
            }

            var cacheCleanerSettings = new CacheCleanerSettings(TimeSpan.FromMinutes(1), TimeSpan.FromSeconds(50), TimeSpan.FromMinutes(15), TimeSpan.FromMinutes(15));

            var storageCleanerSettings = new StorageCleanerSettings(TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(15), TimeSpan.FromMinutes(20), TimeSpan.FromMinutes(20), TimeSpan.FromMinutes(20));

            DefaultWebDocumentViewerContainer.RegisterSingleton<StorageCleanerSettings>(storageCleanerSettings);
            DefaultWebDocumentViewerContainer.RegisterSingleton<CacheCleanerSettings>(cacheCleanerSettings);
            #endregion Cache saving

            ASPxWebDocumentViewer.StaticInitialize();
            ASPxReportDesigner.StaticInitialize();

            //DevExpress.XtraReports.Web.ClientControls.LoggerService.Initialize(new DevexpressLoggerService());
            #endregion Devexpress

        }
        private void DevexpressFileSystemDataSave()
        {
            DefaultWebDocumentViewerContainer.UseFileDocumentStorage(Server.MapPath("~/ViewerStorages/Documents"));
            DefaultWebDocumentViewerContainer.UseFileExportedDocumentStorage(Server.MapPath("~/ViewerStorages/ExportedDocuments"), StorageSynchronizationMode.InterThread);
            DefaultWebDocumentViewerContainer.UseFileReportStorage(Server.MapPath("~/ViewerStorages/Reports"), StorageSynchronizationMode.InterThread);
            DefaultWebDocumentViewerContainer.UseCachedReportSourceBuilder();
        }
        private void DevexpressAzureDataSave()
        {

            string cloudStorageConnectionString = ConfigurationManager.AppSettings["AzureStorageAccountDevexpressCS"].ToString();
            
            AzureReportDesignerContainer.UseAzureEnvironment(cloudStorageConnectionString);
            AzureWebDocumentViewerContainer.UseAzureEnvironment(cloudStorageConnectionString);
            AzureWebDocumentViewerContainer.UseCachedReportSourceBuilder(cloudStorageConnectionString, StorageSynchronizationMode.InterProcess);
            //AzureWebDocumentViewerContainer.UseCachedReportSourceBuilder(cloudStorageConnectionString);

        }
        protected void Application_BeginRequest()
        {
            //Response.Cache.SetCacheability(HttpCacheability.NoCache);
            //Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
            //Response.Cache.SetNoStore();
        }
        //protected void Application_PostAuthenticateRequest(Object sender, EventArgs e)
        //{
        //    HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
        //    if (authCookie != null)
        //    {
        //        FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);

        //        UserInfoDetails serializeModel = JsonConvert.DeserializeObject<UserInfoDetails>(authTicket.UserData);
        //        CustomPrincipal newUser = new CustomPrincipal(authTicket.Name);
        //        newUser.UserName = serializeModel.UserName;
        //        newUser.Roles = serializeModel.UserType;

        //        HttpContext.Current.User = newUser;
        //    }
        //}

        protected void Application_End(object sender, EventArgs e)
        {
            //Redis Pub/Sub Unsubscribe
            redisPubSubHelper.Unsubscribe();
        }
        /// <summary>
        /// Configures the view engines. By default, Asp.Net MVC includes the Web Forms (WebFormsViewEngine) and 
        /// Razor (RazorViewEngine) view engines that supports both C# (.cshtml) and VB (.vbhtml). You can remove view 
        /// engines you are not using here for better performance and include a custom Razor view engine that only 
        /// supports C#.
        /// </summary>
        private static void ConfigureViewEngines()
        {
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new RazorViewEngine());
        }


    }
}
