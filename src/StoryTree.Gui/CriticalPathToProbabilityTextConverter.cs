using System;
using System.Globalization;
using System.Windows.Data;
using StoryTree.Calculators;

namespace StoryTree.Gui
{
    public class CriticalPathToProbabilityTextConverter : CriticalPathConverter, IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (ExtractInput(values, out var hydraulics, out var curves, out var criticalPath))
            {
                return values;
            }

            var probability = ClassEstimationFragilityCurveCalculator.CalculateProbability(hydraulics, curves);

            return string.Format("1/{0}", (int) (1.0 / probability));
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}