using System;
using System.Globalization;
using System.Windows.Data;

namespace Forest.Visualization
{
    public class IsProneToExportConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is bool isSaveToImage && isSaveToImage;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}