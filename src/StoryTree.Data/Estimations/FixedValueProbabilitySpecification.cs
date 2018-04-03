using StoryTree.Data.Estimations.Classes;

namespace StoryTree.Data.Estimations
{
    public class FixedValueProbabilitySpecification : IProbabilitySpecification
    {
        public FixedValueProbabilitySpecification(double probability = 1)
        {
            FixedValueProbability = (Probability)probability;
        }

        public ProbabilitySpecificationType Type => ProbabilitySpecificationType.FixedValue;

        public Probability GetProbability()
        {
            return FixedValueProbability;
        }

        public Probability GetProbabilityForWaterLevel(double waterlevel)
        {
            return FixedValueProbability;
        }

        public Probability FixedValueProbability { get; set; }
    }
}
