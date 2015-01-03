using System;
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
        public void CanExecuteOrderByTest()
        {
            var target = new PortalsViewModel();
            Check.That(target.OrderByCommand.CanExecute("Submission")).IsTrue();
            Check.That(target.OrderByCommand.CanExecute("Desc")).IsTrue();
            target.PortalSubmissions = new ObservableCollection<PortalViewModel> {new PortalViewModel(new PortalSubmission {SubmissionStatus = SubmissionStatus.Accepted}, null)};
            Check.That(target.OrderByCommand.CanExecute("Accepted")).IsTrue();
            Check.That(target.OrderByCommand.CanExecute("Rejected")).IsFalse();
            target.PortalSubmissions = new ObservableCollection<PortalViewModel> {new PortalViewModel(new PortalSubmission {SubmissionStatus = SubmissionStatus.Rejected}, null)};
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
            target.PortalSubmissions = new ObservableCollection<PortalViewModel>
            { 
                new PortalViewModel(portalSubmission1, null), 
                new PortalViewModel(portalSubmission2, null), 
                new PortalViewModel(portalSubmission3, null), 
                new PortalViewModel(portalSubmission4, null), 
            };
            target.SubmissionChecked = true;
            Check.ThatCode(() => target.OrderByCommand.Execute("Submission")).DoesNotThrow();

            Check.That(target.PortalSubmissions.Extracting("SubmissionDate"))
                .ContainsExactly(new DateTime(2014, 12, 25), new DateTime(2014, 12, 12), new DateTime(2014, 11, 25),
                    new DateTime(2014, 10, 25));
            target.DescChecked = false;
            target.OrderByCommand.Execute("Submission");
            Check.That(target.PortalSubmissions.Extracting("SubmissionDate"))
                .ContainsExactly(new DateTime(2014, 10, 25), new DateTime(2014, 11, 25), new DateTime(2014, 12, 12), new DateTime(2014, 12, 25));

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
            target.PortalSubmissions = new ObservableCollection<PortalViewModel>
            { 
                new PortalViewModel(portalSubmission1, null), 
                new PortalViewModel(portalSubmission2, null), 
                new PortalViewModel(portalSubmission3, null), 
                new PortalViewModel(portalSubmission4, null), 
            };
            target.AcceptedChecked = true;
            Check.ThatCode(() => target.OrderByCommand.Execute("Accepted")).DoesNotThrow();

            Check.That(target.PortalSubmissions.Extracting("AcceptedDate"))
                .ContainsExactly(new DateTime(2014, 12, 25), new DateTime(2014, 12, 12), new DateTime(2014, 11, 25),
                    new DateTime(2014, 10, 25));
            target.DescChecked = false;
            target.OrderByCommand.Execute("Accepted");
            Check.That(target.PortalSubmissions.Extracting("AcceptedDate"))
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
            target.PortalSubmissions = new ObservableCollection<PortalViewModel>
            { 
                new PortalViewModel(portalSubmission1, null), 
                new PortalViewModel(portalSubmission2, null), 
                new PortalViewModel(portalSubmission3, null), 
                new PortalViewModel(portalSubmission4, null), 
            };
            target.RejectedChecked = true;
            Check.ThatCode(() => target.OrderByCommand.Execute("Rejected")).DoesNotThrow();

            Check.That(target.PortalSubmissions.Extracting("RejectedDate"))
                .ContainsExactly(new DateTime(2014, 12, 25), new DateTime(2014, 12, 12), new DateTime(2014, 11, 25),
                    new DateTime(2014, 10, 25));
            target.DescChecked = false;
            target.OrderByCommand.Execute("Rejected");
            Check.That(target.PortalSubmissions.Extracting("RejectedDate"))
                .ContainsExactly(new DateTime(2014, 10, 25), new DateTime(2014, 11, 25), new DateTime(2014, 12, 12), new DateTime(2014, 12, 25));

        }
    }
}
