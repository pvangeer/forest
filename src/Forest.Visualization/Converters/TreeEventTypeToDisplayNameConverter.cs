using System;
using System.Globalization;
using System.Windows.Data;
using Forest.Data.Tree;

namespace Forest.Visualization.Converters
{
    public class TreeEventTypeToDisplayNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is TreeEventType type)
            {
                switch (type)
                {
                    case TreeEventType.MainEvent:
                        return "";
                    case TreeEventType.Passing:
                        return "Nee";
                    case TreeEventType.Failing:
                        return "Ja";
                }
            }
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}