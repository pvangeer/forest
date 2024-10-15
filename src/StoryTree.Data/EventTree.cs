using System.ComponentModel;
using System.Runtime.CompilerServices;
using StoryTree.Data.Properties;
using StoryTree.Data.Tree;

namespace StoryTree.Data
{
    public class EventTree : INotifyPropertyChanged
    {
        public TreeEvent MainTreeEvent { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        public virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}