using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Forest.Calculators;
using Forest.Data.Estimations.PerTreeEvent;
using Forest.Data.Probabilities;
using Forest.Data.Tree;
using Forest.Visualization.ViewModels;

namespace Forest.Visualization.Converters
{
    public class CriticalPathConverter
    {
        // TODO: This needs to be refactored. Part of this code should be done by the calculator.
        protected static bool ExtractInput(object[] values,
            out FragilityCurveElement[] hydraulicConditions,
            out TreeEventProbabilityEstimate[] estimations,
            out CriticalPathElement[] elements,
            out TreeEvent[] treeEvents)
        {
            hydraulicConditions = null;
            elements = new CriticalPathElement[] { };
            estimations = new TreeEventProbabilityEstimate[] { };
            treeEvents = new TreeEvent[] { };

            if (values.Length != 3)
                return true;

            var hydrodynamicConditionViewModels = values[1] as ObservableCollection<FragilityCurveElementViewModel>;
            if (values[2] is ObservableCollection<TreeEventProbabilityEstimate> estimatesCollection)
                estimations = estimatesCollection.ToArray();

            if (!(values[0] is TreeEvent[] criticalPath) ||
                hydrodynamicConditionViewModels == null || estimations == null)
                return true;

            var orderedWaterLevels = hydrodynamicConditionViewModels.Select(h => h.WaterLevel).Distinct().ToArray();

            hydraulicConditions = hydrodynamicConditionViewModels.Select(vm => vm.FragilityCurveElement)
                .OrderBy(c => c.WaterLevel)
                .ToArray();
            var allElements = new List<CriticalPathElement>();
            for (var i = 0; i < criticalPath.Length; i++)
            {
                var failElement = true;
                if (i < criticalPath.Length - 1)
                    // TODO: A critical path can also contain a passing event.
                    failElement = criticalPath[i].FailingEvent != null && criticalPath[i].FailingEvent == criticalPath[i + 1];

                var estimation = estimations.FirstOrDefault(e => e.TreeEvent == criticalPath[i]);

                allElements.Add(
                    new CriticalPathElement(criticalPath[i], estimation.GetFragilityCurve(orderedWaterLevels), failElement));
            }

            elements = allElements.ToArray();
            treeEvents = criticalPath;
            return false;
        }
    }
}