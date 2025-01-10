using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Forest.Calculators;
using Forest.Data.Estimations.PerTreeEvent;
using Forest.Data.Probabilities;
using Forest.Data.Tree;
using Forest.Visualization.ViewModels.ContentPanel.MainContentPresenter.ProbabilityPerTreeEvent;

namespace Forest.Visualization.Converters
{
    public class CriticalPathConverter
    {
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

            var hydrodynamicConditionViewModels = values[1] as ObservableCollection<HydrodynamicConditionViewModel>;
            if (values[2] is ObservableCollection<TreeEventProbabilityEstimate> estimatesCollection)
                estimations = estimatesCollection.ToArray();

            if (!(values[0] is TreeEvent[] criticalPath) ||
                hydrodynamicConditionViewModels == null || estimations == null)
                return true;

            // TODO: Temp, since this needs to be refactored
            return false;
            var orderedWaterLevels = hydrodynamicConditionViewModels.Select(h => h.WaterLevel).Distinct().ToArray();

            hydraulicConditions = hydrodynamicConditionViewModels.Select(vm => vm.HydrodynamicCondition)
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