using StoryTree.Data.Estimations.Classes;
using StoryTree.Data.Tree;

namespace StoryTree.Data
{
    public class FrequencyLinePoint
    {
        public FrequencyLinePoint(Probability probability, double waterLevel)
        {
            Probability = probability;
            WaterLevel = waterLevel;
        }
        public Probability Probability { get; }

        public double WaterLevel { get; }
    }
}