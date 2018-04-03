using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using StoryTree.Calculators;
using StoryTree.Data;
using StoryTree.Data.Hydraulics;
using StoryTree.Data.Tree;
using StoryTree.Gui.ViewModels;

namespace StoryTree.Gui
{
    public class CriticalPathToProbabilityTextConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (ExtractInput(values, out var hydraulics, out var curves))
            {
                return values;
            }

            var probability = ClassEstimationFragilityCurveCalculator.CalculateProbability(hydraulics, curves);

            return string.Format("1/{0}", (int) (1.0 / probability));
        }

        private static bool ExtractInput(object[] values, out HydraulicCondition[] hydraulicConditions,
            out FragilityCurve[] curves)
        {
            hydraulicConditions = null;
            curves = new FragilityCurve[] { };
            
            if (values.Length != 2)
            {
                return true;
            }

            var hydraulicConditionViewModels = values[1] as ObservableCollection<HydraulicConditionViewModel>;
            if (!(values[0] is TreeEvent[] criticalPath) ||
                hydraulicConditionViewModels == null)
            {
                return true;
            }

            var orderedWaterLevels = hydraulicConditionViewModels.Select(h => h.WaterLevel).Distinct();

            hydraulicConditions = hydraulicConditionViewModels.Select(vm => vm.HydraulicCondition).OrderBy(c => c.WaterLevel).ToArray();
            curves = criticalPath.Select(p => p.ProbabilityInformation.GetFragilityCurve(orderedWaterLevels)).ToArray();
            return false;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}