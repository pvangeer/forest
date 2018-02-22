using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using StoryTree.Data.Properties;

namespace StoryTree.Data
{
    public class Project : INotifyPropertyChanged
    {
        public Project()
        {
            EventTrees = new ObservableCollection<EventTree>();
        }

        public string Name { get; set; }

        public ObservableCollection<EventTree> EventTrees { get; }

        public IEnumerable<FrequencyLinePoint> FrequencyLine { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
