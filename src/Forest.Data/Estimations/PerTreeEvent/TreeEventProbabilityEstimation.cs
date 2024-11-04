using System.Collections.ObjectModel;
using Forest.Data.Probabilities;
using Forest.Data.Tree;

namespace Forest.Data.Estimations.PerTreeEvent
{
    public class TreeEventProbabilityEstimation : Entity
    {
        public TreeEventProbabilityEstimation(TreeEvent treeEvent)
        {
            TreeEvent = treeEvent;
            FixedProbability = (Probability)1;
            ClassProbabilitySpecification = new ObservableCollection<ExpertClassEstimation>();
            FragilityCurve = new FragilityCurve();
        }

        public TreeEvent TreeEvent { get; }

        public ObservableCollection<ExpertClassEstimation> ClassProbabilitySpecification { get; }

        public Probability FixedProbability { get; set; }

        public FragilityCurve FragilityCurve { get; set; }

        public ProbabilitySpecificationType ProbabilitySpecificationType { get; set; }
    }
}