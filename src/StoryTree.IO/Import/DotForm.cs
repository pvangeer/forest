using System;

namespace StoryTree.IO.Import
{
    public class DotForm
    {
        public string EventTreeName { get; set; }
        public string ExpertName { get; set; }

        public DateTime Date { get; set; }

        public DotNode[] Nodes { get; set; }
    }
}
