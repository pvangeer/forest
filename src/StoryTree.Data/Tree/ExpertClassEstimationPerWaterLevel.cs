using StoryTree.Data.Estimations.Classes;

namespace StoryTree.Data.Tree
{
    public class ExpertClassEstimationPerWaterLevel
    {
        
        public double WaterLevel { get; set; }

        public ProbabilityClass MinEstimation { get; set; }

        public ProbabilityClass AverageEstimation { get; set; }

        public ProbabilityClass MaxEstimation { get; set; }
    }
}