using System;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;

namespace Tests.Nhibernate
{
    public class BaseNHibernateTest<MappingClass> : IDisposable where MappingClass : class
    {
        protected ISessionFactory _sessionFactory;
        protected Configuration _configuration;
        public BaseNHibernateTest()
        {
            _sessionFactory = Fluently.Configure().Database(SQLiteConfiguration.Standard.InMemory().ShowSql)
                .Mappings(m => m.FluentMappings.Add<MappingClass>())
                .ExposeConfiguration(c => _configuration = c)
                .BuildSessionFactory();
        }

        protected ISession OpenSession()
        {
            ISession session = _sessionFactory.OpenSession();
            var export = new SchemaExport(_configuration);
            export.Execute(true, true, false, session.Connection, null);

            return session;
        }
        public void Dispose()
        {
            if (_sessionFactory != null)
            {
                _sessionFactory.Dispose();
                
            }
        }
    }
}