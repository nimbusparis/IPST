using System.Reflection;
using System.Windows.Media;
using FakeItEasy;
using GalaSoft.MvvmLight.Messaging;
using IPST_Engine;
using IPST_Engine.Repository;
using IPST_GUI.Message;
using IPST_GUI.ViewModel;
using NFluent;
using Xunit;

namespace TestGUI
{
    public class PortalViewModelTest
    {
        [Fact]
        public void DefaultThumbnailColor_Test()
        {
            PortalSubmission portalSubmission = new PortalSubmission
            {
                SubmissionStatus = SubmissionStatus.Pending,
            };
            var target = new PortalViewModel(portalSubmission, null);
            Check.That(((SolidColorBrush) target.DefaultThumbnailColor).Color).Equals(Colors.Gray);
            portalSubmission = new PortalSubmission
            {
                SubmissionStatus = SubmissionStatus.Accepted,
            };
            target = new PortalViewModel(portalSubmission, null);
            Check.That(((SolidColorBrush)target.DefaultThumbnailColor).Color).Equals(Colors.Green);
            portalSubmission = new PortalSubmission
            {
                SubmissionStatus = SubmissionStatus.Rejected,
            };
            target = new PortalViewModel(portalSubmission, null);
            Check.That(((SolidColorBrush)target.DefaultThumbnailColor).Color).Equals(Colors.Red);

        }

        [Fact]
        public void DefaultThumbnailSymbol_Test()
        {
            PortalSubmission portalSubmission = new PortalSubmission
            {
                SubmissionStatus = SubmissionStatus.Pending,
            };
            var target = new PortalViewModel(portalSubmission, null);
            Check.That(target.DefaultThumbnailSymbol).Equals("s");
            portalSubmission = new PortalSubmission
            {
                SubmissionStatus = SubmissionStatus.Accepted,
            };
            target = new PortalViewModel(portalSubmission, null);
            Check.That(target.DefaultThumbnailSymbol).Equals("a");
            portalSubmission = new PortalSubmission
            {
                SubmissionStatus = SubmissionStatus.Rejected,
            };
            target = new PortalViewModel(portalSubmission, null);
            Check.That(target.DefaultThumbnailSymbol).Equals("r");
        }

        [Fact]
        public void OpenSubmissionCommandTest()
        {
            var target = new PortalViewModel(new PortalSubmission(), null);
            var messenger = A.Fake<IMessenger>();
            var piMessengerInstance = typeof(PortalViewModel).GetProperty("MessengerInstance",
                BindingFlags.Instance | BindingFlags.NonPublic);
            piMessengerInstance.SetValue(target, messenger);
            Check.ThatCode(() => target.OpenSubmissionCommand.Execute(target)).DoesNotThrow();
            A.CallTo(() => messenger.Send(A<MessageNavigatePortal>._)).MustHaveHappened();
        }


        [Fact]
        public void ExecuteIgnoreSubmissionCommandTest()
        {
            var portalRepository = A.Fake<IPortalSubmissionRepository>();
            var target = new PortalViewModel(new PortalSubmission { SubmissionStatus = SubmissionStatus.Pending }, portalRepository);
            Check.ThatCode(() => target.IgnoreSubmissionCommand.Execute(target)).DoesNotThrow();
            Check.That(target.SubmissionStatus).Equals(SubmissionStatus.Ignored);
            A.CallTo(()=>portalRepository.Save(A<PortalSubmission>._)).MustHaveHappened();
        }

    }
}
