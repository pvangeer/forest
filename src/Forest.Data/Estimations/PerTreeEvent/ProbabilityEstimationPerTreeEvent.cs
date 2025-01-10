using System.Collections.ObjectModel;
using Forest.Data.Probabilities;
using Forest.Data.Tree;

namespace Forest.Data.Estimations.PerTreeEvent
{
    public class ProbabilityEstimationPerTreeEvent : ProbabilityEstimation
    {
        public ProbabilityEstimationPerTreeEvent()
        {
            HydrodynamicConditions = new ObservableCollection<FragilityCurveElement>();
            Estimates = new ObservableCollection<TreeEventProbabilityEstimate>();
        }

        public EventTree EventTree { get; set; }

        public ObservableCollection<FragilityCurveElement> HydrodynamicConditions { get; }

        public ObservableCollection<TreeEventProbabilityEstimate> Estimates { get; }
    }
}