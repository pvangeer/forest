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
            ClassProbabilitySpecifications = new ObservableCollection<ExpertClassEstimation>();
            FragilityCurve = new FragilityCurve();
        }

        public TreeEvent TreeEvent { get; }

        public ObservableCollection<ExpertClassEstimation> ClassProbabilitySpecifications { get; }

        public Probability FixedProbability { get; set; }

        public FragilityCurve FragilityCurve { get; set; }

        public ProbabilitySpecificationType ProbabilitySpecificationType { get; set; }
    }
}