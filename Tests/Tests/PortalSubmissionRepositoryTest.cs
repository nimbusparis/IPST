using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using FakeItEasy;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using IPST_Engine;
using IPST_Engine.Mapping;
using IPST_Engine.Repository;
using NFluent;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Criterion.Lambda;
using NHibernate.Tool.hbm2ddl;
using Xunit;

namespace Tests
{
    public class PortalSubmissionRepositoryTest
    {
        private ISession _session;
        private ISessionFactory _sessionFactory;

        public PortalSubmissionRepositoryTest()
        {
            _session = A.Fake<ISession>();
        }
        [Fact]
        public void RepositoryConstructor_Test()
        {
            var repository = new PortalSubmissionRepository(_session);
            Check.That(repository).IsNotNull();
        }

        [Fact]
        public void Save_Test()
        {
            var repository = new PortalSubmissionRepository(_session);
            var entity = new PortalSubmission
            {
                Title = "Title",
            };
            repository.Save(entity);
            A.CallTo(()=>_session.SaveOrUpdate(A<object>._)).MustHaveHappened();
        }

        [Fact]
        public void SaveMany_Test()
        {
            var transaction = A.Fake<ITransaction>();
            A.CallTo(() => _session.BeginTransaction()).Returns(transaction);
            var repository = new PortalSubmissionRepository(_session);
            var entities = new List<PortalSubmission>
            {
                new PortalSubmission(),
                new PortalSubmission(),
                new PortalSubmission(),
            };
            repository.Save(entities);
            A.CallTo(()=>transaction.Commit()).MustHaveHappened();
            A.CallTo(()=>_session.BeginTransaction()).MustHaveHappened(Repeated.Exactly.Once);
            A.CallTo(()=>_session.SaveOrUpdate(A<PortalSubmission>._)).MustHaveHappened(Repeated.Exactly.Times(3));
        }
        [Fact]
        public void GetAllWithCriteria_Test()
        {
            var queryOver = A.Fake<IQueryOver<PortalSubmission, PortalSubmission>>();
            A.CallTo(() => _session.QueryOver<PortalSubmission>()).Returns(queryOver);
            var portals = new List<PortalSubmission>
            {
                new PortalSubmission(),
                new PortalSubmission(),
                new PortalSubmission(),
            };
            A.CallTo(() => queryOver.Where(A<Expression<Func<PortalSubmission, bool>>>._)).Returns(queryOver);
            A.CallTo(() => queryOver.List<PortalSubmission>()).Returns(portals);
            var repository = new PortalSubmissionRepository(_session);
            var result = repository.GetAll(submission => submission.Id == 1);
            Check.That(result).ContainsExactly(portals);
        }
        [Fact]
        public void GetAll_Test()
        {
            var queryOver = A.Fake<IQueryOver<PortalSubmission, PortalSubmission>>();
            A.CallTo(() => _session.QueryOver<PortalSubmission>()).Returns(queryOver);
            var portals = new List<PortalSubmission>
            {
                new PortalSubmission(),
                new PortalSubmission(),
                new PortalSubmission(),
            };
            A.CallTo(() => queryOver.Where(A<Expression<Func<PortalSubmission, bool>>>._)).Returns(queryOver);
            A.CallTo(() => queryOver.List<PortalSubmission>()).Returns(portals);
            var repository = new PortalSubmissionRepository(_session);
            var result = repository.GetAll();
            Check.That(result).ContainsExactly(portals);
        }

        [Fact]
        public void Get_Test()
        {
             PortalSubmission expected =
                new PortalSubmission
                {
                    Id = 1,
                    Title = "Title"
                };
            A.CallTo(() => _session.Get<PortalSubmission>(1)).Returns(expected);
           var repository = new PortalSubmissionRepository(_session);
            var result = repository.Get(1);
            A.CallTo(()=>_session.Get<PortalSubmission>(1)).MustHaveHappened();
            Check.That(result).Equals(expected);
        }

        [Fact]
        public void GetByPhotoUrl_Test()
        {
            PortalSubmission expected =
               new PortalSubmission
               {
                   Id = 1,
                   Title = "Title",
                   ImageUrl = new Uri("http://theUrl.com/TOTO")
               };
           var repository = new PortalSubmissionRepository(_session);
            var querySql = A.Fake<ISQLQuery>();
            A.CallTo(() => _session.CreateSQLQuery(A<string>.That.Matches(q=>q.Contains("/TOTO")))).Returns(querySql);
            A.CallTo(() => querySql.List<PortalSubmission>()).Returns(new List<PortalSubmission> { expected });
            var result = repository.GetByImageUrl(new Uri("http://theUrl.com/TOTO"));
            Check.That(result).Equals(expected);
        }

        [Fact]
        public void CompareUri_Test()
        {
            Check.That(PortalSubmissionRepository.CompareUri(null, null)).IsTrue();

            var uri1 = new Uri("http://b1.img.com/Photo1");
            var uri2 = new Uri("http://b2.img.com/Photo1");
            Check.That(PortalSubmissionRepository.CompareUri(null, uri2)).IsFalse();
            Check.That(PortalSubmissionRepository.CompareUri(uri1, uri2)).IsTrue();
        }
        [Fact]
        public void ValidatePortalByImageUrl_Test()
        {
            var repository = new PortalSubmissionRepository(_session);
            var result = repository.ChangePortalStatus(new Uri("http://theUrl.com/TOTO"), SubmissionStatus.Accepted);
            Check.That(result.SubmissionStatus).Equals(SubmissionStatus.Accepted);
            Check.That(result.DateAccept.Value.Date).Equals(DateTime.Today);
        }
        [Fact]
        public void RejectPortalByImageUrl_Test()
        {
            var repository = new PortalSubmissionRepository(_session);
            var result = repository.ChangePortalStatus(new Uri("http://theUrl.com/TOTO"), SubmissionStatus.Rejected, RejectionReason.TooClose);
            Check.That(result.SubmissionStatus).Equals(SubmissionStatus.Rejected);
            Check.That(result.DateReject.Value.Date).Equals(DateTime.Today);
            Check.That(result.RejectionReason).Equals(RejectionReason.TooClose);
        }

        [Fact]
        public void GetSubmissionByStatus()
        {
            var repository = new PortalSubmissionRepository(_session);
            var queryOver = A.Fake<IQueryOver<PortalSubmission, PortalSubmission>>();
            A.CallTo(() => _session.QueryOver<PortalSubmission>()).Returns(queryOver);
            A.CallTo(() => queryOver
                .Where(A<Expression<Func<PortalSubmission, bool>>>._))
                .Returns(queryOver);
            var lstExpected = new List<PortalSubmission>
            {
                new PortalSubmission(),
                new PortalSubmission(),
            };
            A.CallTo(() => queryOver.List<PortalSubmission>()).Returns(lstExpected);
            var result = repository.GetAllByStatus(SubmissionStatus.Pending);
            Check.That(result.Count).Equals(2);
        }

        [Fact(Skip = "")]
        public void GetLastSubmissionTime()
        {
            var queryOver = A.Fake<IQueryOver<PortalSubmission, PortalSubmission>>();
            A.CallTo(() => _session.QueryOver<PortalSubmission>()).Returns(queryOver);
            var queryOverOrder = A.Fake<IQueryOverOrderBuilder<PortalSubmission, PortalSubmission>>();
            A.CallTo(() => queryOver
                .OrderBy(A<Expression<Func<PortalSubmission, object>>>._))
                .Returns(queryOverOrder);
            A.CallTo(() => queryOverOrder.Desc).Returns(queryOver);
            var lstExpected = new List<PortalSubmission>
            {
                new PortalSubmission {DateSubmission = new DateTime(2014,09,11)},
                new PortalSubmission {DateAccept = new DateTime(2014, 10, 01)},
                new PortalSubmission {DateReject = new DateTime(2014, 09, 01)},
            };
            A.CallTo(() => queryOver.List<PortalSubmission>()).Returns(lstExpected);
            var repository = new PortalSubmissionRepository(_session);
            Check.That(repository.GetLastSubmissionDateTime()).Equals(new DateTime(2014, 10, 01));
        }

        [Fact]
        public void GetNbPortalSubmissionByDatesTest()
        {
            Configuration configuration = null;
            _sessionFactory = Fluently.Configure().Database(SQLiteConfiguration.Standard.InMemory().ShowSql)
                .Mappings(m => m.FluentMappings.Add<PortalSubmissionMapping>())
                .ExposeConfiguration(cfg=>configuration=cfg)
                .BuildSessionFactory();
            using (var session = _sessionFactory.OpenSession())
            {
                var export = new SchemaExport(configuration);
                export.Execute(true, true, false, session.Connection, null);

                var portalsSubmissions = new List<PortalSubmission>
                {
                    new PortalSubmission {SubmissionStatus = SubmissionStatus.Pending, DateSubmission = new DateTime(2014, 01, 01)},
                    new PortalSubmission {SubmissionStatus = SubmissionStatus.Pending, DateSubmission = new DateTime(2014, 01, 01)},
                    new PortalSubmission {SubmissionStatus = SubmissionStatus.Pending, DateSubmission = new DateTime(2014, 01, 01)},
                    new PortalSubmission {SubmissionStatus = SubmissionStatus.Pending, DateSubmission = new DateTime(2014, 02, 01)},
                    new PortalSubmission {SubmissionStatus = SubmissionStatus.Pending, DateSubmission = new DateTime(2014, 02, 01)},
                    new PortalSubmission {SubmissionStatus = SubmissionStatus.Accepted, DateSubmission = new DateTime(2014, 02, 01)},
                    new PortalSubmission {SubmissionStatus = SubmissionStatus.Accepted, DateSubmission = new DateTime(2014, 02, 01)},
                    new PortalSubmission {SubmissionStatus = SubmissionStatus.Rejected, DateSubmission = new DateTime(2014, 02, 01)},
                    new PortalSubmission {SubmissionStatus = SubmissionStatus.Rejected, DateSubmission = new DateTime(2014, 02, 01)},
                };
                portalsSubmissions.ForEach(p => session.Save(p));
                var repository = new PortalSubmissionRepository(session);
                var nbPortalSubmissionByDates = repository.GetNbPortalSubmissionByDates(SubmissionStatus.Pending);
                Check.That(nbPortalSubmissionByDates.Extracting("Date")).ContainsExactly(new DateTime(2014, 01, 01), new DateTime(2014, 02, 01));
                Check.That(nbPortalSubmissionByDates.Extracting("Nb")).ContainsExactly(3, 2);

                
            }
        }
        [Fact]
        public void GetNbPortalAcceptedByDatesTest()
        {
            Configuration configuration = null;
            _sessionFactory = Fluently.Configure().Database(SQLiteConfiguration.Standard.InMemory().ShowSql)
                .Mappings(m => m.FluentMappings.Add<PortalSubmissionMapping>())
                .ExposeConfiguration(cfg=>configuration=cfg)
                .BuildSessionFactory();
            using (var session = _sessionFactory.OpenSession())
            {
                var export = new SchemaExport(configuration);
                export.Execute(true, true, false, session.Connection, null);

                var portalsSubmissions = new List<PortalSubmission>
                {
                    new PortalSubmission {SubmissionStatus = SubmissionStatus.Pending, DateSubmission = new DateTime(2014, 01, 01)},
                    new PortalSubmission {SubmissionStatus = SubmissionStatus.Pending, DateSubmission = new DateTime(2014, 01, 01)},
                    new PortalSubmission {SubmissionStatus = SubmissionStatus.Pending, DateSubmission = new DateTime(2014, 01, 01)},
                    new PortalSubmission {SubmissionStatus = SubmissionStatus.Pending, DateSubmission = new DateTime(2014, 02, 01)},
                    new PortalSubmission {SubmissionStatus = SubmissionStatus.Pending, DateSubmission = new DateTime(2014, 02, 01)},
                    new PortalSubmission {SubmissionStatus = SubmissionStatus.Accepted, DateSubmission = new DateTime(2014, 02, 01), DateAccept = new DateTime(2014, 02, 01)},
                    new PortalSubmission {SubmissionStatus = SubmissionStatus.Accepted, DateSubmission = new DateTime(2014, 02, 01), DateAccept = new DateTime(2014, 02, 01)},
                    new PortalSubmission {SubmissionStatus = SubmissionStatus.Accepted, DateSubmission = new DateTime(2014, 03, 01), DateAccept = new DateTime(2014, 03, 01)},
                    new PortalSubmission {SubmissionStatus = SubmissionStatus.Accepted, DateSubmission = new DateTime(2014, 03, 01), DateAccept = new DateTime(2014, 03, 01)},
                    new PortalSubmission {SubmissionStatus = SubmissionStatus.Rejected, DateSubmission = new DateTime(2014, 02, 01)},
                    new PortalSubmission {SubmissionStatus = SubmissionStatus.Rejected, DateSubmission = new DateTime(2014, 02, 01)},
                    new PortalSubmission {SubmissionStatus = SubmissionStatus.Accepted, DateSubmission = new DateTime(2014, 05, 01), DateAccept = new DateTime(2014, 05, 01)},
                    new PortalSubmission {SubmissionStatus = SubmissionStatus.Accepted, DateSubmission = new DateTime(2014, 05, 01), DateAccept = new DateTime(2014, 05, 01)},
                };                                                                                                                  
                portalsSubmissions.ForEach(p => session.Save(p));                                                                   
                var repository = new PortalSubmissionRepository(session);                                                                                                     
                var nbPortalSubmissionByDates = repository.GetNbPortalSubmissionByDates(SubmissionStatus.Accepted);
                Check.That(nbPortalSubmissionByDates.Extracting("Date")).ContainsExactly(new DateTime(2014, 02, 01), new DateTime(2014, 03, 01), new DateTime(2014, 05, 01));
                Check.That(nbPortalSubmissionByDates.Extracting("Nb")).ContainsExactly(2, 2, 2);

                
            }
        }
        [Fact]
        public void GetNbPortalRejectedByDatesTest()
        {
            Configuration configuration = null;
            _sessionFactory = Fluently.Configure().Database(SQLiteConfiguration.Standard.InMemory().ShowSql)
                .Mappings(m => m.FluentMappings.Add<PortalSubmissionMapping>())
                .ExposeConfiguration(cfg=>configuration=cfg)
                .BuildSessionFactory();
            using (var session = _sessionFactory.OpenSession())
            {
                var export = new SchemaExport(configuration);
                export.Execute(true, true, false, session.Connection, null);

                var portalsSubmissions = new List<PortalSubmission>
                {
                    new PortalSubmission {SubmissionStatus = SubmissionStatus.Pending, DateSubmission = new DateTime(2014, 01, 01)},
                    new PortalSubmission {SubmissionStatus = SubmissionStatus.Pending, DateSubmission = new DateTime(2014, 01, 01)},
                    new PortalSubmission {SubmissionStatus = SubmissionStatus.Pending, DateSubmission = new DateTime(2014, 01, 01)},
                    new PortalSubmission {SubmissionStatus = SubmissionStatus.Pending, DateSubmission = new DateTime(2014, 02, 01)},
                    new PortalSubmission {SubmissionStatus = SubmissionStatus.Pending, DateSubmission = new DateTime(2014, 02, 01)},
                    new PortalSubmission {SubmissionStatus = SubmissionStatus.Accepted, DateSubmission = new DateTime(2014, 02, 01)},
                    new PortalSubmission {SubmissionStatus = SubmissionStatus.Accepted, DateSubmission = new DateTime(2014, 02, 01)},
                    new PortalSubmission {SubmissionStatus = SubmissionStatus.Accepted, DateSubmission = new DateTime(2014, 03, 01)},
                    new PortalSubmission {SubmissionStatus = SubmissionStatus.Accepted, DateSubmission = new DateTime(2014, 03, 01)},
                    new PortalSubmission {SubmissionStatus = SubmissionStatus.Rejected, DateSubmission = new DateTime(2014, 02, 01), DateReject = new DateTime(2014, 02, 01)},
                    new PortalSubmission {SubmissionStatus = SubmissionStatus.Rejected, DateSubmission = new DateTime(2014, 02, 01), DateReject = new DateTime(2014, 02, 01)},
                    new PortalSubmission {SubmissionStatus = SubmissionStatus.Accepted, DateSubmission = new DateTime(2014, 05, 01)},
                    new PortalSubmission {SubmissionStatus = SubmissionStatus.Accepted, DateSubmission = new DateTime(2014, 05, 01)},
                };
                portalsSubmissions.ForEach(p => session.Save(p));
                var repository = new PortalSubmissionRepository(session);
                var nbPortalSubmissionByDates = repository.GetNbPortalSubmissionByDates(SubmissionStatus.Rejected);
                Check.That(nbPortalSubmissionByDates.Extracting("Date")).ContainsExactly(new DateTime(2014, 02, 01));
                Check.That(nbPortalSubmissionByDates.Extracting("Nb")).ContainsExactly(2);

                
            }
        }

    }
}
