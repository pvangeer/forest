using System.Collections.Generic;

namespace StoryTree.Data.Tree
{
    public interface IProbabilitySpecification
    {
        ProbabilitySpecificationType Type { get; }

        Dictionary<double, Probability> Probabilities { get; }
    }
}