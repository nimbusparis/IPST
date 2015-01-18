using System;
using FakeItEasy;
using IPST_Engine;
using IPST_Engine.Repository;
using IPST_GUI.ViewModel;
using NFluent;
using Xunit;

namespace TestGUI
{
    public class ManualAcceptViewModelTest
    {
        [Fact]
        public void SaveCommandTest()
        {
            var repository = A.Fake<IPortalSubmissionRepository>();
            var portalSubmission = new PortalSubmission()
            {
                SubmissionStatus = SubmissionStatus.Appealed,
            };
            var target = new ManualAcceptViewModel(portalSubmission, repository);
            target.SaveCommand.Execute(null);
            A.CallTo(()=>repository.Save(A<PortalSubmission>.That.Matches(p=>PortalSubmissionSaveCheck(p)))).MustHaveHappened();
        }

        private bool PortalSubmissionSaveCheck(PortalSubmission portalSubmission)
        {
            Check.That(portalSubmission.SubmissionStatus).Equals(SubmissionStatus.Accepted);
            return true;
        }

        [Fact]
        public void PortalUrlTest()
        {
            var repository = A.Fake<IPortalSubmissionRepository>();
            var portalSubmission = new PortalSubmission()
            {
                SubmissionStatus = SubmissionStatus.Appealed,
            };
            var target = new ManualAcceptViewModel(portalSubmission, repository);
            target.PortalUrl = new Uri("http://www.test.com");
            Check.That(target.PortalUrl).Equals(new Uri("http://www.test.com"));
            Check.That(portalSubmission.PortalUrl).Equals(target.PortalUrl);
        }
        [Fact]
        public void PortalAcceptedDateTest()
        {
            var repository = A.Fake<IPortalSubmissionRepository>();
            var portalSubmission = new PortalSubmission()
            {
                SubmissionStatus = SubmissionStatus.Appealed,
            };
            var target = new ManualAcceptViewModel(portalSubmission, repository);
            target.AcceptedDate = DateTime.Today;
            Check.That(target.AcceptedDate).Equals(DateTime.Today);
            Check.That(portalSubmission.DateAccept).Equals(target.AcceptedDate);
        }
        [Fact]
        public void PostalAddressTest()
        {
            var repository = A.Fake<IPortalSubmissionRepository>();
            var portalSubmission = new PortalSubmission()
            {
                SubmissionStatus = SubmissionStatus.Appealed,
            };
            var target = new ManualAcceptViewModel(portalSubmission, repository);
            target.PostalAddress = "TheAddress";
            Check.That(target.PostalAddress).Equals("TheAddress");
            Check.That(portalSubmission.PostalAddress).Equals(target.PostalAddress);
        }

        [Fact]
        public void PortalImageTest()
        {
            var repository = A.Fake<IPortalSubmissionRepository>();
            var portalSubmission = new PortalSubmission()
            {
                SubmissionStatus = SubmissionStatus.Appealed,
                ImageUrl = new Uri("http://lh5.ggpht.com/X86qj-jQN3fZf05XtY7zOTZU7QHrxN947uXlZkB0CUtFi0phczUO1N6ip4Gwx_yAkiF_Cxd3V8rjmwvuanZp"),
            };
            var target = new ManualAcceptViewModel(portalSubmission, repository);
            Check.That(target.PortalImage).IsNotNull();
        }

        [Fact]
        public void PortalTitleTest()
        {
            var repository = A.Fake<IPortalSubmissionRepository>();
            var portalSubmission = new PortalSubmission()
            {
                SubmissionStatus = SubmissionStatus.Appealed,
                Title = "The Title",
            };
            var target = new ManualAcceptViewModel(portalSubmission, repository);
            Check.That(target.PortalTitle).Equals("The Title");

        }
    }
}
