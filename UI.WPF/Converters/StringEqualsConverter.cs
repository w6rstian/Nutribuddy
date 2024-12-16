using System;
using System.Globalization;
using System.Windows.Data;

namespace Nutribuddy.UI.WPF.Converters
{
    public class StringEqualsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Compare value with the converter parameter
            return value != null && value.ToString() == parameter.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null; // No need for this in this case
        }
    }
}