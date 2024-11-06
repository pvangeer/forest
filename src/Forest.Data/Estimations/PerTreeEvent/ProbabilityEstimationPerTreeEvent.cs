using System.Collections.ObjectModel;
using Forest.Data.Experts;
using Forest.Data.Hydrodynamics;
using Forest.Data.Tree;

namespace Forest.Data.Estimations.PerTreeEvent
{
    public class ProbabilityEstimationPerTreeEvent : ProbabilityEstimation
    {
        public ProbabilityEstimationPerTreeEvent()
        {
            Experts = new ObservableCollection<Expert>();
            HydrodynamicConditions = new ObservableCollection<HydrodynamicCondition>();
            Estimates = new ObservableCollection<TreeEventProbabilityEstimate>();
        }

        public EventTree EventTree { get; set; }

        public ObservableCollection<Expert> Experts { get; }

        public ObservableCollection<HydrodynamicCondition> HydrodynamicConditions { get; }

        public ObservableCollection<TreeEventProbabilityEstimate> Estimates { get; }
    }
}