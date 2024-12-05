using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using Forest.Data.Tree;

namespace Forest.Visualization.Converters
{
    public class TreeEventTypeToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is TreeEventType type)
                switch (type)
                {
                    case TreeEventType.MainEvent:
                        return Visibility.Collapsed;
                    case TreeEventType.Passing:
                    case TreeEventType.Failing:
                        return Visibility.Visible;
                }

            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}