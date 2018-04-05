using System.Collections.Generic;

namespace StoryTree.Data.Estimations
{
    public interface IProbabilitySpecification
    {
        ProbabilitySpecificationType Type { get; }

        FragilityCurve GetFragilityCurve(IEnumerable<double> waterLevels);
    }
}