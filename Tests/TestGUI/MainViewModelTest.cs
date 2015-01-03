using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FakeItEasy;
using IPST_Engine;
using IPST_Engine.Repository;
using IPST_GUI.Model;
using IPST_GUI.ViewModel;
using Microsoft.Practices.Unity;
using NFluent;
using Xunit;

namespace TestGUI
{
    public class MainViewModelTest
    {
        private UnityContainer container;
        private IDataService dataService;

        public MainViewModelTest()
        {
            container = new UnityContainer();
            dataService = A.Fake<IDataService>();
        }

        [Fact]
        public void TestConnectionCommand()
        {
            var gmailEngine = A.Fake<IIPSTEngine>();
            var repository = A.Fake<IPortalSubmissionRepository>();
            var viewModel = new MainViewModel(gmailEngine, repository);
            Check.ThatCode(() => viewModel.ConnectCommand.Execute(null)).DoesNotThrow();
            A.CallTo(()=>gmailEngine.ConnectAsync()).MustHaveHappened();
        }

        [Fact]
        public void TestConnectionCommandFailed()
        {
            var gmailEngine = A.Fake<IIPSTEngine>();
            A.CallTo(() => gmailEngine.ConnectAsync()).Throws<Exception>();
            var repository = A.Fake<IPortalSubmissionRepository>();
            var viewModel = new MainViewModel(gmailEngine, repository);
            Check.ThatCode(() => viewModel.ConnectCommand.Execute(null)).Throws<Exception>();
        }

        [Fact]
        public void TestCheckEmailCommand()
        {
            var gmailEngine = A.Fake<IIPSTEngine>();
            var repository = A.Fake<IPortalSubmissionRepository>();
            var viewModel = new MainViewModel(gmailEngine, repository);
            Check.ThatCode(() => viewModel.CheckEmailsCommand.Execute(null)).DoesNotThrow();
        }

        [Fact]
        public void ShowPortalCommand_Test()
        {
            var gmailEngine = A.Fake<IIPSTEngine>();
            var repository = A.Fake<IPortalSubmissionRepository>();
            var viewModel = new MainViewModel(gmailEngine, repository);
            var portals = new List<PortalSubmission>
            {
                new PortalSubmission(),
                new PortalSubmission(),
                new PortalSubmission(),
            };
            Check.ThatCode(() => viewModel.ShowPortalsCommand.Execute(portals)).DoesNotThrow();
        }
    }
}
