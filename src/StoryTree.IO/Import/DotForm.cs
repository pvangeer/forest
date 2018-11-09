using System;
using System.IO;

namespace StoryTree.IO.Import
{
    public class DotForm
    {
        public string EventTreeName { get; set; }

        public string ExpertName { get; set; }

        public FileStream EventImageFile { get; set; }

        public DateTime Date { get; set; }

        public DotNode[] Nodes { get; set; }
    }
}
