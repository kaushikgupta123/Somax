using InterfaceAPI.BusinessWrapper.Implementation;
using InterfaceAPI.BusinessWrapper.Implementation.BBU;
using InterfaceAPI.BusinessWrapper.Interface;
using InterfaceAPI.BusinessWrapper.Interface.BBU;
using System.Web.Http;
using Unity;
using Unity.WebApi;

namespace InterfaceAPI
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
            var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();

            container.RegisterType<ICommonWrapper, CommonWrapper>();
            container.RegisterType<BusinessWrapper.Interface.IPOImportWrapper, BusinessWrapper.Implementation.POImportWrapper>();
            container.RegisterType<IReceiptImportWrapper, ReceiptImportWrapper>();
            container.RegisterType<IAccountImportWrapper, AccountImportWrapper>();
            container.RegisterType<BusinessWrapper.Interface.IVendorMasterImportWrapper, BusinessWrapper.Implementation.VendorMasterImportWrapper>();
            container.RegisterType<IImportCSVWrapper, ImportCSVWrapper>();
            container.RegisterType<IPartMasterImportWrapper, PartMasterImportWrapper>();
            container.RegisterType<IPartMasterRequestExportWrapper, PartMasterRequestExportWrapper>();
            container.RegisterType<IPartMasterResponseImportWrapper, PartMasterResponseImportWrapper>();
            container.RegisterType<BusinessWrapper.Interface.BBU.IPOImportWrapper, BusinessWrapper.Implementation.BBU.POImportWrapper>();
            container.RegisterType<IPOReceiptImportWrapper, POReceiptImportWrapper>();
            container.RegisterType<IPurchaseRequestExportWrapper, PurchaseRequestExportWrapper>();
            container.RegisterType<IVendorCatalogImportWrapper, VendorCatalogImportWrapper>();
            container.RegisterType<BusinessWrapper.Interface.BBU.IVendorMasterImportWrapper, BusinessWrapper.Implementation.BBU.VendorMasterImportWrapper>();
            container.RegisterType<IPartCategoryMasterImportWrapper, PartCategoryMasterImportWrapper>();
            container.RegisterType<IIoTReadingImportWrapper, IoTReadingImportWrapper>();
            container.RegisterType<IMonnitIoTReadingImportWrapper, MonnitIoTReadingImportWrapper>();
            container.RegisterType<IEPMPOEDIExportWrapper, EPMPOEDIExportWrapper>();
            container.RegisterType<IEPMInvoiceImportWrapper, EPMInvoiceImportWrapper>();
            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}