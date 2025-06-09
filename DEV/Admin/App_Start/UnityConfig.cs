using Admin.BusinessWrapper;
using Admin.BusinessWrapper.Knowledgebase;
using System.Web.Mvc;
using Unity;
using Unity.Mvc5;

namespace Admin
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
            var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();
            container.RegisterType<ILoginWrapper, LoginWrapper>();
            container.RegisterType<IKnowledgebaseWrapper, KnowledgebaseWrapper>();
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}