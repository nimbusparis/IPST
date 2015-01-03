using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;

namespace IPST_GUI.Converters
{
    public class BooleanToVisibilityWithParamConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var invert = parameter is bool && (bool) parameter;
            var val = value is bool && (bool) value;
            val = invert ? !val : val;
            return new BooleanToVisibilityConverter().Convert(val, targetType, null, culture);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}