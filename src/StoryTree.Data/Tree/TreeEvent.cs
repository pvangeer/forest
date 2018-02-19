using System.ComponentModel;
using System.Runtime.CompilerServices;
using StoryTree.Data.Annotations;

namespace StoryTree.Data.Tree
{
    public class TreeEvent : INotifyPropertyChanged
    {
        public TreeEvent()
        {
        }
        
        public string Name { get; set; }

        public TreeEvent FalseEvent { get; set; }

        public TreeEvent TrueEvent { get; set; }

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
