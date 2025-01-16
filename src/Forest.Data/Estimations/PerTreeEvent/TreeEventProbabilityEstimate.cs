using Forest.Data.Probabilities;
using Forest.Data.Tree;

namespace Forest.Data.Estimations.PerTreeEvent
{
    public class TreeEventProbabilityEstimate : Entity
    {
        public TreeEventProbabilityEstimate(TreeEvent treeEvent)
        {
            TreeEvent = treeEvent;
            FixedProbability = (Probability)1;
            FragilityCurve = new FragilityCurve();
        }

        public TreeEvent TreeEvent { get; }

        public Probability FixedProbability { get; set; }

        public FragilityCurve FragilityCurve { get; }

        public ProbabilitySpecificationType ProbabilitySpecificationType { get; set; }
    }
}