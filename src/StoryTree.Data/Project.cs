using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using StoryTree.Data.Properties;

namespace StoryTree.Data
{
    public class Project : INotifyPropertyChanged
    {
        private string name;

        public Project()
        {
            EventTrees = new ObservableCollection<EventTree>();
            Experts = new ObservableCollection<Expert>();
        }

        public string Name
        {
            get => name;
            set
            {
                name = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<EventTree> EventTrees { get; }

        public ObservableCollection<Expert> Experts { get; }

        /*public IEnumerable<FrequencyLinePoint> FrequencyLine { get; set; }*/

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
