using System.Collections.Generic;
using StoryTree.Data.Estimations.Classes;

namespace StoryTree.Data.Estimations
{
    public interface IProbabilitySpecification
    {
        ProbabilitySpecificationType Type { get; }

        Probability GetProbability();

        Probability GetProbabilityForWaterLevel(double waterlevel);
    }
}