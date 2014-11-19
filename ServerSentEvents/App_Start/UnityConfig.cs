using System.Web.Http;
using Microsoft.Practices.Unity;
using ServerSentEvents.Messaging;
using ServerSentEvents.Process;
using Unity.WebApi;

namespace ServerSentEvents.App_Start
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();
            
            // register all your components with the container here
            // it is NOT necessary to register your controllers
            
            // e.g. container.RegisterType<ITestService, TestService>();
            container
                .RegisterInstance<IMessenger>(new Messenger())
                .RegisterType<IProcess, FakeProcess>();
            
            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}