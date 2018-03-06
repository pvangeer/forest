using System.ComponentModel;
using System.Runtime.CompilerServices;
using StoryTree.Data.Properties;

namespace StoryTree.Data.Tree
{
    public class TreeEvent : INotifyPropertyChanged
    {
        public TreeEvent()
        {
        }
        
        public string Name { get; set; }

        public TreeEvent FailingEvent { get; set; }

        public TreeEvent PassingEvent { get; set; }

        public string Description { get; set; }

        public IProbabilitySpecification ProbabilityInformation { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        public virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
