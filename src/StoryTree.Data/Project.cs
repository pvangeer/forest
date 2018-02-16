using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace StoryTree.Data
{
    public class Project
    {
        public Project()
        {
            EventTrees = new ObservableCollection<EventTree>();
        }

        public string Name { get; set; }

        public ObservableCollection<EventTree> EventTrees { get; }

        public IEnumerable<FrequencyLinePoint> FrequencyLine { get; set; }
    }
}
