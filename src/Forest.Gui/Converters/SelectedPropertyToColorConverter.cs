using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Forest.Gui.Converters
{
    public class SelectedPropertyToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is bool))
                return new SolidColorBrush(Colors.Transparent);

            var selected = (bool)value;
            return new SolidColorBrush(selected ? Colors.Red : Colors.Transparent);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}