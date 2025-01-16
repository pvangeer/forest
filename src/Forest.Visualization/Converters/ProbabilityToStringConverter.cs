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

            if (str.Contains("/"))
            {
                var parts = str.Split('/');
                if (parts.Length != 2)
                    return value;
                if (!double.TryParse(parts[0],out var firstNumber) || !double.TryParse(parts[1],out var secondNumber))
                    return value;
                return (Probability)(firstNumber / secondNumber);
            }

            if (!double.TryParse(str, NumberStyles.Float, CultureInfo.InvariantCulture, out var probabilityValue))
                return value;

            return (Probability)probabilityValue;
        }
    }
}