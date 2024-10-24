using Forest.Data.Experts;
using System.Collections.ObjectModel;
using Forest.Data.Tree;

namespace Forest.Data.Estimations
{
    public class ProbabilityEstimationPerTreeEvent : ProbabilityEstimation
    {
        public ProbabilityEstimationPerTreeEvent()
        {
            Experts = new ObservableCollection<Expert>();
            Estimations = new ObservableCollection<TreeEventProbabilityEstimation>();
        }

        public EventTree EventTree { get; set; }

        public ObservableCollection<Expert> Experts { get; }

        public ObservableCollection<TreeEventProbabilityEstimation> Estimations { get; }
    }
}
