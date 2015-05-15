using System.Web.Mvc;
using FluentNHibernate.Cfg;
using IPST_Engine;
using IPST_Engine.Mapping;
using IPST_Engine.Repository;
using Microsoft.Practices.Unity;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using Unity.Mvc5;

namespace IPST_Web
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();
            
            // register all your components with the container here
            // it is NOT necessary to register your controllers
            container.RegisterType<IIPSTEngine, IPSTEngine>();
            container.RegisterType<IPortalSubmissionRepository, PortalSubmissionRepository>();
            container.RegisterType<IPortalSubmissionParser, PortalSubmissionParser>();
            // e.g. container.RegisterType<ITestService, TestService>();
            ConfigureDatabase(container);
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }

        private static void ConfigureDatabase(IUnityContainer container)
        {
            Configuration cfg = new Configuration().Configure();
            var sessionFactory = Fluently.Configure(cfg)
            .Mappings(m => m.FluentMappings.Add<PortalSubmissionMapping>())
            .ExposeConfiguration(c => new SchemaUpdate(c).Execute(false, true))
            .BuildSessionFactory();
            container.RegisterInstance(sessionFactory);

            var session = sessionFactory.OpenSession();
            container.RegisterInstance(session);

        }
    }
}