using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Forest.Data;
using Forest.Data.Estimations.PerTreeEvent;
using Forest.Data.Hydrodynamics;
using Forest.Data.Tree;

namespace Forest.Visualization.ViewModels
{
    public class EstimationSpecificationViewModelFactory
    {
        public EstimationSpecificationViewModelFactory(ForestAnalysis forestAnalysis)
        {
            ForestAnalysis = forestAnalysis;
        }

        public ForestAnalysis ForestAnalysis { get; }

        public ProbabilitySpecificationViewModelBase CreateViewModel(TreeEvent treeEvent,
            TreeEventProbabilityEstimate[] estimations,
            ObservableCollection<HydrodynamicCondition> hydrodynamicConditions)
        {
            var estimation = estimations?.FirstOrDefault(e => e.TreeEvent == treeEvent);
            if (estimation == null)
                return null;

            switch (estimation.ProbabilitySpecificationType)
            {
                case ProbabilitySpecificationType.Classes:
                    return new ClassesProbabilitySpecificationViewModel(treeEvent, estimation);
                case ProbabilitySpecificationType.FixedValue:
                    return new FixedProbabilitySpecificationViewModel(treeEvent, estimation);
                case ProbabilitySpecificationType.FixedFrequency:
                    return new FixedFragilityCurveSpecificationViewModel(treeEvent, estimation, hydrodynamicConditions);
                default:
                    throw new InvalidEnumArgumentException();
            }
        }
    }
}