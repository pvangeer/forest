namespace StoryTree.Data.Tree
{
    public interface IProbabilitySpecification
    {
        ProbabilitySpecificationType Type { get; }

        Probability Probability { get; }
    }
}