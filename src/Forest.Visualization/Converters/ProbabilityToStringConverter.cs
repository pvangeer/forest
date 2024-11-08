using System;
using System.Globalization;
using System.Windows.Data;
using Forest.Data.Probabilities;

namespace Forest.Visualization.Converters
{
    public class ProbabilityToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is Probability probability))
                return value;

            return probability.Value.ToString("E2", CultureInfo.CurrentUICulture);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is string str))
                return value;

            if (!double.TryParse(str, out var probabilityValue))
                return value;

            return (Probability)probabilityValue;
        }
    }
}