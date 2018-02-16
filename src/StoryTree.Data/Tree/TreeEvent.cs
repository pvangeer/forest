namespace StoryTree.Data.Tree
{
    public class TreeEvent
    {
        public TreeEvent()
        {
        }
        
        public string Name { get; set; }

        public TreeEvent FalseEvent { get; set; }

        public TreeEvent TrueEvent { get; set; }

        public string Description { get; set; }

        public IProbabilitySpecification ProbabilityInformation { get; set; }
    }
}
