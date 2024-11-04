namespace Forest.Data.Tree
{
    public class TreeEvent : Entity
    {
        public TreeEvent(string name)
        {
            Name = name;
        }

        public string Name { get; set; }

        public TreeEvent FailingEvent { get; set; }

        public TreeEvent PassingEvent { get; set; }

        public string Summary { get; set; }

        public string Information { get; set; }
    }
}