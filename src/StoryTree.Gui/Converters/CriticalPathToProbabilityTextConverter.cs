using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using StoryTree.Calculators;

namespace StoryTree.Gui.Converters
{
    public class CriticalPathToProbabilityTextConverter : CriticalPathConverter, IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (ExtractInput(values, out var hydraulics, out var curves, out var criticalPath))
            {
                return values;
            }

            return !hydraulics.Any() || !curves.Any()
                ? "NaN"
                : string.Format("1/{0}",
                    (int) (1.0 / ClassEstimationFragilityCurveCalculator.CalculateProbability(hydraulics, curves)));
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}