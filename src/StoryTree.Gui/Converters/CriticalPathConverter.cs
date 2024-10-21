using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using StoryTree.Calculators;
using StoryTree.Data.Hydraulics;
using StoryTree.Data.Tree;
using StoryTree.Gui.ViewModels;

namespace StoryTree.Gui.Converters
{
    public class CriticalPathConverter
    {
        protected static bool ExtractInput(object[] values, out HydraulicCondition[] hydraulicConditions,
            out CriticalPathElement[] elements, out TreeEvent[] treeEvents)
        {
            hydraulicConditions = null;
            elements = new CriticalPathElement[] { };
            treeEvents = new TreeEvent[] { };

            if (values.Length != 2)
                return true;

            var hydraulicConditionViewModels = values[1] as ObservableCollection<HydraulicConditionViewModel>;
            if (!(values[0] is TreeEvent[] criticalPath) ||
                hydraulicConditionViewModels == null)
                return true;

            var orderedWaterLevels = hydraulicConditionViewModels.Select(h => h.WaterLevel).Distinct();

            hydraulicConditions = hydraulicConditionViewModels.Select(vm => vm.HydraulicCondition).OrderBy(c => c.WaterLevel).ToArray();
            var allElements = new List<CriticalPathElement>();
            for (var i = 0; i < criticalPath.Length; i++)
            {
                var failElement = true;
                if (i < criticalPath.Length - 1)
                    failElement = criticalPath[i].FailingEvent != null && criticalPath[i].FailingEvent == criticalPath[i + 1];

                allElements.Add(
                    new CriticalPathElement(criticalPath[i], criticalPath[i].GetFragilityCurve(orderedWaterLevels), failElement));
            }

            elements = allElements.ToArray();
            treeEvents = criticalPath;
            return false;
        }
    }
}