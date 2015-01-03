using System.Windows;
using IPST_Engine;
using IPST_GUI.Converters;
using NFluent;
using Xunit;

namespace TestGUI
{
    public class SubmissionStatusToVisibilityTest
    {
        [Fact]
        public void ConvertTest()
        {
            var target = new SubmissionStatusToVisibility();
            var result = target.Convert(SubmissionStatus.Pending, typeof(Visibility), "Pending", null);
            Check.That(result).Equals(Visibility.Visible);
            result = target.Convert(SubmissionStatus.Accepted, typeof(Visibility), "Pending", null);
            Check.That(result).Equals(Visibility.Collapsed);
        }
    }
}
