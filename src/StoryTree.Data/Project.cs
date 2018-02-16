using System.Collections;
using System.Collections.Generic;
using StoryTree.Data.Tree;

namespace StoryTree.Data
{
    public class Project
    {
        public Project()
        {
            Name = "Project 1";
            MainTreeEvent = new TreeEvent();
        }

        public string Name { get; set; }

        public TreeEvent MainTreeEvent { get; }

        public IEnumerable<FrequencyLinePoint> FrequencyLine { get; set; }
    }
}
