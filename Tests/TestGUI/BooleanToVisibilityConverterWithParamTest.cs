using System.Windows;
using IPST_GUI.Converters;
using NFluent;
using Xunit;

namespace TestGUI
{
    public class BooleanToVisibilityConverterWithParamTest
    {
        [Fact]
        public void ConvertTest()
        {
            var target = new BooleanToVisibilityWithParamConverter();
            Check.That(target.Convert(true, typeof (Visibility), false, null)).Equals(Visibility.Visible);
            Check.That(target.Convert(true, typeof (Visibility), true, null)).Equals(Visibility.Collapsed);
        }
    }
}
