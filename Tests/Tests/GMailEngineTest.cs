using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using FakeItEasy;
using Google.Apis.Gmail.v1.Data;
using IPST_Engine;
using IPST_Engine.Repository;
using NFluent;
using Xunit;

namespace Tests
{
    public class GMailEngineTest
    {
        private IPortalSubmissionRepository repository;
        private IPortalSubmissionParser parser;
        private IProgress<Tuple<int, int>> _progress;

        public GMailEngineTest()
        {
            repository = A.Fake<IPortalSubmissionRepository>();
            parser = A.Fake<IPortalSubmissionParser>();
            _progress = A.Fake<IProgress<Tuple<int, int>>>();
;
        }

        [Fact]
        public void Constructor_Test()
        {
            var engine = new IPSTEngine(repository, parser);
            Check.That(engine).IsNotNull();
        }

        [Fact]
        public void Connect_Test()
        {
            var engine = new IPSTEngine(repository, parser);
            engine.ConnectAsync();
        }

        [Fact(Skip = "QUOTA LIMITED")]
        public void GetPortalsSumission_Test()
        {
            var engine = new IPSTEngine(repository, parser);
            engine.ConnectAsync().Wait();
            var portalSubmissionTask = engine.GetPortalEmails("\"Ingress Portal\"", DateTime.MinValue, _progress);
            portalSubmissionTask.Wait();
            Check.That(portalSubmissionTask).IsNotNull();
            var portalSubmissions = portalSubmissionTask.Result;
            Check.That(portalSubmissions).IsNotNull();
            Check.That(portalSubmissions.Count).IsNotEqualTo(0);
            foreach (var portalSubmission in portalSubmissions)
            {
                Check.That(portalSubmission.SubmissionStatus).Equals(SubmissionStatus.Pending);
            }
            //A.CallTo(()=>parser.ParseMessage(A<Message>._)).Returns(new PortalSubmission(){SubmissionStatus = SubmissionStatus.NewAccepted});
            //portalSubmissionTask = engine.GetPortalEmails("\"Ingress Portal\"", DateTime.MinValue);
            //portalSubmissionTask.Wait();
            //portalSubmissions = portalSubmissionTask.Result;
            //Check.That(portalSubmissions.Count).IsEqualTo(0);
        }
        [Fact]
        public void GetPortalsSubmission_NoResult_Test()
        {
            var engine = new IPSTEngine(repository, parser);
            engine.ConnectAsync().Wait();
            var portalSubmissionTask = engine.GetPortalEmails("\"Ingress Portal\"", DateTime.Today.AddDays(1), _progress);
            portalSubmissionTask.Wait();
            Check.That(portalSubmissionTask).IsNotNull();
            var portalSubmissions = portalSubmissionTask.Result;
            Check.That(portalSubmissions).IsNotNull();
            Check.That(portalSubmissions.Count).IsEqualTo(0);
        }


        private bool PortalDuplicateSavePredicate(PortalSubmission portalSubmission)
        {
            Check.That(portalSubmission.DateReject).Equals(DateTime.Today);
            Check.That(portalSubmission.SubmissionStatus).Equals(SubmissionStatus.Rejected);
            Check.That(portalSubmission.PortalUrl).IsNull();
            Check.That(portalSubmission.RejectionReason).Equals(RejectionReason.Duplicate);
            return true;
        }

        private bool PortalLiveSavePredicate(PortalSubmission portalSubmission)
        {
            Check.That(portalSubmission.DateAccept).Equals(DateTime.Today);
            Check.That(portalSubmission.SubmissionStatus).Equals(SubmissionStatus.Accepted);
            Check.That(portalSubmission.PortalUrl).IsNotNull();
            Check.That(portalSubmission.DateSubmission).Equals(DateTime.Today.AddDays(-1));
            return true;
        }


        [Fact(Skip = "QUOTA LIMITED")]
        public void CheckSubmissionsPending_Test()
        {
            A.CallTo(() => repository.GetLastSubmissionDateTime()).Returns(new DateTime(2014, 10, 01));
            A.CallTo(() => parser.ParseMessage(A<Message>._))
                .Returns(new PortalSubmission {SubmissionStatus = SubmissionStatus.Pending});
            var engine = new IPSTEngine(repository, parser);
            engine.ConnectAsync().Wait();

            var submissionsTask = engine.CheckSubmissions(_progress);
            submissionsTask.Wait();
            A.CallTo(() => repository.GetLastSubmissionDateTime()).MustHaveHappened(Repeated.Exactly.Once);
            Check.That(engine.NewPendings.Count).IsNotEqualTo(0);
            A.CallTo(()=>repository.Save(A<PortalSubmission>.That.Matches(p=>PortalSubmissionPredicate(p)))).MustHaveHappened();
        }

        private bool PortalSubmissionPredicate(PortalSubmission portalSubmission)
        {
            Check.That(portalSubmission.SubmissionStatus).Equals(SubmissionStatus.Pending);
            return true;
        }

        [Fact(Skip = "QUOTA LIMITED")]
        public void CheckSubmissionsAccept_Test()
        {
            A.CallTo(() => repository.GetLastSubmissionDateTime()).Returns(new DateTime(2014, 10, 01));
            A.CallTo(() => repository.GetByImageUrl(A<Uri>._)).Returns(new PortalSubmission { DateSubmission = DateTime.Today.AddDays(-1) });
            A.CallTo(() => parser.ParseMessage(A<Message>._))
                .Returns(new PortalSubmission {SubmissionStatus = SubmissionStatus.Accepted, DateAccept = DateTime.Today, PortalUrl = new Uri("http://www.ingress.com/portal")});
            var engine = new IPSTEngine(repository, parser);
            engine.ConnectAsync().Wait();

            var submissionsTask = engine.CheckSubmissions(_progress);
            submissionsTask.Wait();
            A.CallTo(() => repository.GetLastSubmissionDateTime()).MustHaveHappened(Repeated.Exactly.Once);
            Check.That(engine.NewAccepted.Count).IsNotEqualTo(0);
            A.CallTo(() => repository.Save(A<PortalSubmission>.That.Matches(p => PortalLiveSavePredicate(p)))).MustHaveHappened();
            A.CallTo(() => repository.GetByImageUrl(A<Uri>._)).MustHaveHappened();
        }
        [Fact(Skip = "QUOTA LIMITED")]
        public void CheckSubmissionsReject_Test()
        {
            A.CallTo(() => repository.GetLastSubmissionDateTime()).Returns(new DateTime(2014, 10, 01));
            A.CallTo(() => repository.GetByImageUrl(A<Uri>._)).Returns(new PortalSubmission { DateSubmission = DateTime.Today.AddDays(-1) });
            A.CallTo(() => parser.ParseMessage(A<Message>._))
                .Returns(new PortalSubmission {SubmissionStatus = SubmissionStatus.Rejected, DateReject = DateTime.Today, RejectionReason = RejectionReason.NotMeetCriteria});
            var engine = new IPSTEngine(repository, parser);
            engine.ConnectAsync().Wait();

            var submissionsTask = engine.CheckSubmissions(_progress);
            submissionsTask.Wait();
            A.CallTo(() => repository.GetLastSubmissionDateTime()).MustHaveHappened(Repeated.Exactly.Once);
            Check.That(engine.NewRejected.Count).IsNotEqualTo(0);
            A.CallTo(() => repository.Save(A<PortalSubmission>.That.Matches(p => PortalRejectedSavePredicate(p)))).MustHaveHappened();
            A.CallTo(() => repository.GetByImageUrl(A<Uri>._)).MustHaveHappened();
        }

        private bool PortalRejectedSavePredicate(PortalSubmission portalSubmission)
        {
            Check.That(portalSubmission.DateReject).Equals(DateTime.Today);
            Check.That(portalSubmission.SubmissionStatus).Equals(SubmissionStatus.Rejected);
            Check.That(portalSubmission.PortalUrl).IsNull();
            Check.That(portalSubmission.RejectionReason).Equals(RejectionReason.NotMeetCriteria);
            return true;
        }

        [Fact(Skip = "QUOTA LIMITED")]
        public void CannotFindSubmissionForLivePortal_Test()
        {
            A.CallTo(() => repository.GetByImageUrl(A<Uri>._)).Returns(null);
            // DateSubmission is set in the mock parser to re-use the predicate
            A.CallTo(() => parser.ParseMessage(A<Message>._))
                .Returns(new PortalSubmission { SubmissionStatus = SubmissionStatus.Accepted, DateSubmission = DateTime.Today.AddDays(-1), DateAccept = DateTime.Today, PortalUrl = new Uri("http://www.ingress.com/portal") });
            var engine = new IPSTEngine(repository, parser);
            engine.ConnectAsync().Wait();
            var submissionsTask = engine.CheckSubmissions(_progress);
            submissionsTask.Wait();
            A.CallTo(() => repository.Save(A<PortalSubmission>.That.Matches(p => PortalLiveSavePredicate(p)))).MustHaveHappened();
        }
        [Fact(Skip = "QUOTA LIMITED")]
        public void CannotFindSubmissionForRejectedPortal_Test()
        {
            A.CallTo(() => repository.GetByImageUrl(A<Uri>._)).Returns(null);
            // DateSubmission is set in the mock parser to re-use the predicate
            A.CallTo(() => parser.ParseMessage(A<Message>._))
                .Returns(new PortalSubmission { SubmissionStatus = SubmissionStatus.Rejected, DateSubmission = DateTime.Today.AddDays(-1), DateReject = DateTime.Today, RejectionReason = RejectionReason.NotMeetCriteria});
            var engine = new IPSTEngine(repository, parser);
            engine.ConnectAsync().Wait();
            var submissionsTask = engine.CheckSubmissions(_progress);
            submissionsTask.Wait();
            A.CallTo(() => repository.Save(A<PortalSubmission>.That.Matches(p => PortalRejectedSavePredicate(p)))).MustHaveHappened();
        }

        [Fact]
        public void Pending_Test()
        {
            IList<PortalSubmission> pending = new List<PortalSubmission>
            {
                new PortalSubmission(),
                new PortalSubmission(),
                new PortalSubmission(),
            };
            A.CallTo(() => repository.GetAll(A<Expression<Func<PortalSubmission, bool>>>._)).Returns(pending);
            var engine = new IPSTEngine(repository, parser);

            IList<PortalSubmission> results = null;
            Check.ThatCode(() => results = engine.Pending).DoesNotThrow();
            Check.That(results).ContainsExactly(pending);
        }
        [Fact]
        public void Accepted_Test()
        {
            IList<PortalSubmission> pending = new List<PortalSubmission>
            {
                new PortalSubmission(),
                new PortalSubmission(),
                new PortalSubmission(),
            };
            A.CallTo(() => repository.GetAll(A<Expression<Func<PortalSubmission, bool>>>._)).Returns(pending);
            var engine = new IPSTEngine(repository, parser);

            IList<PortalSubmission> results = null;
            Check.ThatCode(() => results = engine.Accepted).DoesNotThrow();
            Check.That(results).ContainsExactly(pending);
        }
        [Fact]
        public void Rejected_Test()
        {
            IList<PortalSubmission> pending = new List<PortalSubmission>
            {
                new PortalSubmission(),
                new PortalSubmission(),
                new PortalSubmission(),
            };
            A.CallTo(() => repository.GetAll(A<Expression<Func<PortalSubmission, bool>>>._)).Returns(pending);
            var engine = new IPSTEngine(repository, parser);

            IList<PortalSubmission> results = null;
            Check.ThatCode(() => results = engine.Rejected).DoesNotThrow();
            Check.That(results).ContainsExactly(pending);
        }

        [Fact(Skip = "QUOTA LIMITED")]
        public void CheckEmailProgress_Test()
        {
            A.CallTo(() => repository.GetLastSubmissionDateTime()).Returns(DateTime.Today.AddDays(-2));
            var target = new IPSTEngine(repository, parser);
            target.ConnectAsync().Wait();
            target.CheckSubmissions(_progress).Wait();
            A.CallTo(()=>_progress.Report(A<Tuple<int, int>>._)).MustHaveHappened();
        }
    }
}
