using Forest.Data.Experts;
using System.Collections.ObjectModel;
using Forest.Data.Hydrodynamics;
using Forest.Data.Tree;

namespace Forest.Data.Estimations
{
    public class ProbabilityEstimationPerTreeEvent : ProbabilityEstimation
    {
        public ProbabilityEstimationPerTreeEvent()
        {
            Experts = new ObservableCollection<Expert>();
            HydrodynamicConditions = new ObservableCollection<HydrodynamicCondition>();
            Estimations = new ObservableCollection<TreeEventProbabilityEstimation>();
        }

        public string Name { get; set; }

        public EventTree EventTree { get; set; }

        public ObservableCollection<Expert> Experts { get; }

        public ObservableCollection<HydrodynamicCondition> HydrodynamicConditions { get; }

        public ObservableCollection<TreeEventProbabilityEstimation> Estimations { get; }
    }
}
