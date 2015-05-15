using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using IPST_Engine;
using IPST_GUI.ViewModel;
using NFluent;
using Xunit;

namespace TestGUI
{
    public class PortalsViewModelTest
    {
        [Fact]
        public void CanExecuteQueryTest()
        {
            var target = new PortalsViewModel();
            Check.That(target.QueryCommand.CanExecute("Toto")).IsFalse();
            var portalSubmission1 = new PortalSubmission
            {
                SubmissionStatus = SubmissionStatus.Pending,
                DateSubmission = new DateTime(2014, 12, 25)
            };
            target.PortalSubmissions = new List<PortalSubmission>()
            {
                portalSubmission1,
            };
            Check.That(target.QueryCommand.CanExecute("Toto")).IsTrue();
        }
        [Fact]
        public void ExecuteQueryTest()
        {
            var target = new PortalsViewModel();
            var portalSubmission1 = new PortalSubmission
            {
                Title = "Tata",
            };
            var portalSubmission2 = new PortalSubmission
            {
                Title = "Toto1",
            };
            var portalSubmission3 = new PortalSubmission
            {
                Title = "Titi",
            };
            var portalSubmission4 = new PortalSubmission
            {
                Title = "Toto2",
            };
            var portalSubmissions = new List<PortalSubmission>()
            {
                portalSubmission1,
                portalSubmission2,
                portalSubmission3,
                portalSubmission4,
            };
            target.PortalSubmissions = portalSubmissions;
            Check.ThatCode(() => target.QueryCommand.Execute("Toto")).DoesNotThrow();
            Check.That(target.DisplayedPortalSubmissions.Extracting("Title")).ContainsExactly("Toto1", "Toto2");
        }
        [Fact]
        public void ExecuteQueryCaseInsensitiveTest()
        {
            var target = new PortalsViewModel();
            var portalSubmission1 = new PortalSubmission
            {
                Title = "Tata",
            };
            var portalSubmission2 = new PortalSubmission
            {
                Title = "Toto1",
            };
            var portalSubmission3 = new PortalSubmission
            {
                Title = "Titi",
            };
            var portalSubmission4 = new PortalSubmission
            {
                Title = "Toto2",
            };
            var portalSubmissions = new List<PortalSubmission>()
            {
                portalSubmission1,
                portalSubmission2,
                portalSubmission3,
                portalSubmission4,
            };
            target.PortalSubmissions = portalSubmissions;
            Check.ThatCode(() => target.QueryCommand.Execute("toto")).DoesNotThrow();
            Check.That(target.DisplayedPortalSubmissions.Extracting("Title")).ContainsExactly("Toto1", "Toto2");
        }
        [Fact]
        public void CanExecuteOrderByTest()
        {
            var target = new PortalsViewModel();
            Check.That(target.OrderByCommand.CanExecute("Submission")).IsTrue();
            Check.That(target.OrderByCommand.CanExecute("Desc")).IsTrue();
            target.PortalSubmissions = new List<PortalSubmission>(){new PortalSubmission {SubmissionStatus = SubmissionStatus.Accepted}};
            Check.That(target.OrderByCommand.CanExecute("Accepted")).IsTrue();
            Check.That(target.OrderByCommand.CanExecute("Rejected")).IsFalse();
            target.PortalSubmissions = new List<PortalSubmission>(){new PortalSubmission {SubmissionStatus = SubmissionStatus.Rejected}};
            Check.That(target.OrderByCommand.CanExecute("Accepted")).IsFalse();
            Check.That(target.OrderByCommand.CanExecute("Rejected")).IsTrue();
        }

        [Fact]
        public void ExecuteOrderBySubmissionTest()
        {
            var target = new PortalsViewModel();
            var portalSubmission1 = new PortalSubmission
            {
                SubmissionStatus = SubmissionStatus.Pending, DateSubmission = new DateTime(2014,12,25)
            };
            var portalSubmission2 = new PortalSubmission
            {
                SubmissionStatus = SubmissionStatus.Pending, DateSubmission = new DateTime(2014,11,25)
            };
            var portalSubmission3 = new PortalSubmission
            {
                SubmissionStatus = SubmissionStatus.Pending, DateSubmission = new DateTime(2014,10,25)
            };
            var portalSubmission4 = new PortalSubmission
            {
                SubmissionStatus = SubmissionStatus.Pending, DateSubmission = new DateTime(2014,12,12)
            };
            target.PortalSubmissions = new List<PortalSubmission>()
            { 
                portalSubmission1, 
                portalSubmission2, 
                portalSubmission3, 
                portalSubmission4, 
            };
            target.SubmissionChecked = true;
            Check.ThatCode(() => target.OrderByCommand.Execute("Submission")).DoesNotThrow();

            Check.That(target.DisplayedPortalSubmissions.Extracting("SubmissionDate"))
                .ContainsExactly(new DateTime(2014, 12, 25), new DateTime(2014, 12, 12), new DateTime(2014, 11, 25),
                    new DateTime(2014, 10, 25));
            target.DescChecked = false;
            target.OrderByCommand.Execute("Submission");
            Check.That(target.DisplayedPortalSubmissions.Extracting("SubmissionDate"))
                .ContainsExactly(new DateTime(2014, 10, 25), new DateTime(2014, 11, 25), new DateTime(2014, 12, 12), new DateTime(2014, 12, 25));

        }

        [Fact]
        public void QueryModifiedTest()
        {
            var target = new PortalsViewModel();
            var portalSubmissions = new List<PortalSubmission>
            {
                new PortalSubmission() {Title = "Toto"},
                new PortalSubmission() {Title = "Tata"},
            };
            target.PortalSubmissions = portalSubmissions;
            target.Query = "Toto";
            Check.That(target.DisplayedPortalSubmissions.Extracting("Title")).ContainsExactly("Toto");
            Check.That(target.Query).Equals("Toto");
        }
        [Fact]
        public void ExecuteOrderByAcceptedTest()
        {
            var target = new PortalsViewModel();
            var portalSubmission1 = new PortalSubmission
            {
                SubmissionStatus = SubmissionStatus.Accepted, 
                DateAccept = new DateTime(2014,12,25)
            };
            var portalSubmission2 = new PortalSubmission
            {
                SubmissionStatus = SubmissionStatus.Accepted,
                DateAccept = new DateTime(2014, 11, 25)
            };
            var portalSubmission3 = new PortalSubmission
            {
                SubmissionStatus = SubmissionStatus.Accepted,
                DateAccept = new DateTime(2014, 10, 25)
            };
            var portalSubmission4 = new PortalSubmission
            {
                SubmissionStatus = SubmissionStatus.Accepted,
                DateAccept = new DateTime(2014, 12, 12)
            };
            target.PortalSubmissions = new List<PortalSubmission>()
            { 
                portalSubmission1, 
                portalSubmission2, 
                portalSubmission3, 
                portalSubmission4, 
            };
            target.AcceptedChecked = true;
            Check.ThatCode(() => target.OrderByCommand.Execute("Accepted")).DoesNotThrow();

            Check.That(target.DisplayedPortalSubmissions.Extracting("AcceptedDate"))
                .ContainsExactly(new DateTime(2014, 12, 25), new DateTime(2014, 12, 12), new DateTime(2014, 11, 25),
                    new DateTime(2014, 10, 25));
            target.DescChecked = false;
            target.OrderByCommand.Execute("Accepted");
            Check.That(target.DisplayedPortalSubmissions.Extracting("AcceptedDate"))
                .ContainsExactly(new DateTime(2014, 10, 25), new DateTime(2014, 11, 25), new DateTime(2014, 12, 12), new DateTime(2014, 12, 25));

        }
        [Fact]
        public void ExecuteOrderByRejectedTest()
        {
            var target = new PortalsViewModel();
            var portalSubmission1 = new PortalSubmission
            {
                SubmissionStatus = SubmissionStatus.Rejected, 
                DateReject = new DateTime(2014,12,25)
            };
            var portalSubmission2 = new PortalSubmission
            {
                SubmissionStatus = SubmissionStatus.Rejected,
                DateReject = new DateTime(2014, 11, 25)
            };
            var portalSubmission3 = new PortalSubmission
            {
                SubmissionStatus = SubmissionStatus.Rejected,
                DateReject = new DateTime(2014, 10, 25)
            };
            var portalSubmission4 = new PortalSubmission
            {
                SubmissionStatus = SubmissionStatus.Rejected,
                DateReject = new DateTime(2014, 12, 12)
            };
            target.PortalSubmissions = new List<PortalSubmission>()
            { 
                portalSubmission1, 
                portalSubmission2, 
                portalSubmission3, 
                portalSubmission4, 
            };
            target.RejectedChecked = true;
            Check.ThatCode(() => target.OrderByCommand.Execute("Rejected")).DoesNotThrow();

            Check.That(target.DisplayedPortalSubmissions.Extracting("RejectedDate"))
                .ContainsExactly(new DateTime(2014, 12, 25), new DateTime(2014, 12, 12), new DateTime(2014, 11, 25),
                    new DateTime(2014, 10, 25));
            target.DescChecked = false;
            target.OrderByCommand.Execute("Rejected");
            Check.That(target.DisplayedPortalSubmissions.Extracting("RejectedDate"))
                .ContainsExactly(new DateTime(2014, 10, 25), new DateTime(2014, 11, 25), new DateTime(2014, 12, 12), new DateTime(2014, 12, 25));

        }
    }
}
