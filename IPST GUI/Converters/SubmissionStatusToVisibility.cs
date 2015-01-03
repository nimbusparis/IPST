using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using IPST_Engine;

namespace IPST_GUI.Converters
{
    public class SubmissionStatusToVisibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is SubmissionStatus && targetType == typeof(Visibility) && parameter is string)
            {
                SubmissionStatus expected = (SubmissionStatus) Enum.Parse(typeof (SubmissionStatus), (string) parameter);
                return ((SubmissionStatus)value) == expected ? Visibility.Visible : Visibility.Collapsed;
            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
