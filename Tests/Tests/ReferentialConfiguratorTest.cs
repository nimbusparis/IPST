using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using IPST_Engine.Mapping;
using Microsoft.Practices.Unity;
using NFluent;
using NHibernate;
using Xunit;

namespace Tests
{
    public class ReferentialConfiguratorTest
    {
        [Fact]
        public void InitSingleton_Test()
        {
            Check.That(ReferentialConfigurator.Singleton).IsNotNull().And.IsInstanceOf<UnityContainer>();

        }

        [Fact]
        public void CheckSessionFactory()
        {
            Check.That(ReferentialConfigurator.GetSessionFactory()).IsNotNull();
        }
        [Fact]
        public void RegisterMappings_Test()
        {
            //Check.That(ReferentialConfigurator.GetRepository<PortalSubmission>()).IsNotNull();
        }
    }


    public class ReferentialConfigurator
    {
        public static IUnityContainer Singleton;

        static ReferentialConfigurator()
        {
            Singleton = new UnityContainer();
            Singleton.RegisterInstance(Fluently.Configure().Database(SQLiteConfiguration.Standard.InMemory().ShowSql));

            RegisterRepositories();
        }

        private static void RegisterRepositories()
        {
            var configuration = Singleton.Resolve<FluentConfiguration>();
            configuration.Mappings(c => c.FluentMappings.Add<PortalSubmissionMapping>());
        }

        //public static IRepository GetRepository<T>()
        //{
        //    return Singleton.Resolve<IRepository>();
        //}
        public static ISessionFactory GetSessionFactory()
        {
            return Singleton.Resolve<FluentConfiguration>().BuildSessionFactory();
        }
    }
}
