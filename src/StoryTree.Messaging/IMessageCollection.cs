using System.Collections.ObjectModel;
using System.ComponentModel;

namespace StoryTree.Messaging
{
    public interface IMessageCollection : INotifyPropertyChanged
    {
        ObservableCollection<LogMessage> Messages { get; }

        void OnPropertyChanged(string propertyName = null);
    }
}