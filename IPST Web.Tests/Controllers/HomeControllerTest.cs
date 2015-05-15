using System.Threading;
using System.Web.Mvc;
using FakeItEasy;
using IPST_Engine;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IPST_Web.Controllers;
using NFluent;
using Xunit;

namespace IPST_Web.Tests.Controllers
{
    public class HomeControllerTest
    {
        [Fact]
        public void Index()
        {
            // Arrange
            HomeController controller = new HomeController(null);

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Check.That(result).IsNotNull();
        }

        [Fact]
        public void About()
        {
            // Arrange
            HomeController controller = new HomeController(null);

            // Act
            ViewResult result = controller.About() as ViewResult;

            // Assert
            Check.That(result.ViewBag.Message).Equals("Your application description page.");
        }

        [Fact]
        public void Contact()
        {
            // Arrange
            HomeController controller = new HomeController(null);

            // Act
            ViewResult result = controller.Contact() as ViewResult;

            // Assert
            Check.That(result).IsNotNull();
        }

        [Fact]
        public void CheckEmailAsyncTest()
        {
            var container = new UnityContainer();
            var ipstEngine = A.Fake<IIPSTEngine>();
            container.RegisterInstance(ipstEngine);
            var cancellationToken = new CancellationToken();

            HomeController controller = new HomeController(ipstEngine);

            var task = controller.CheckEmailAsync(cancellationToken);

            task.ContinueWith(t => Check.That(t.Result).IsNotNull());

            Check.That(task.IsFaulted).IsFalse();


        }
    }
}
