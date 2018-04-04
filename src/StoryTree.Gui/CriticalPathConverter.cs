using System.Collections.ObjectModel;
using System.Linq;
using StoryTree.Data;
using StoryTree.Data.Hydraulics;
using StoryTree.Data.Tree;
using StoryTree.Gui.ViewModels;

namespace StoryTree.Gui
{
    public class CriticalPathConverter
    {
        protected static bool ExtractInput(object[] values, out HydraulicCondition[] hydraulicConditions, out FragilityCurve[] curves, out TreeEvent[] treeEvents)
        {
            hydraulicConditions = null;
            curves = new FragilityCurve[] { };
            treeEvents = new TreeEvent[] { };

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
            treeEvents = criticalPath;
            return false;
        }
    }
}