using IPST_GUI.Converters;
using NFluent;
using Xunit;

namespace TestGUI
{
    public class InvertBooleanConverterTest
    {
        [Fact]
        public void ConvertTest()
        {
            var target = new InvertBooleanConverter();
            Check.That(target.Convert(true, typeof (bool), null, null)).Equals(false);
            Check.That(target.Convert(false, typeof (bool), null, null)).Equals(true);
        }
    }
}
