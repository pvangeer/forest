using System.Collections.Generic;
using StoryTree.Data.Estimations.Classes;

namespace StoryTree.Data.Estimations
{
    public interface IProbabilitySpecification
    {
        ProbabilitySpecificationType Type { get; }

        Dictionary<double, Probability> Probabilities { get; }
    }
}