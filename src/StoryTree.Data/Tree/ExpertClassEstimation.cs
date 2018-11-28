using StoryTree.Data.Estimations;
using StoryTree.Data.Hydraulics;

namespace StoryTree.Data.Tree
{
    public class ExpertClassEstimation
    {
        public Expert Expert { get; set; }

        public HydraulicCondition HydraulicCondition { get; set; }

        public ProbabilityClass MinEstimation { get; set; }

        public ProbabilityClass AverageEstimation { get; set; }

        public ProbabilityClass MaxEstimation { get; set; }

        public string Comment { get; set; }
    }
}