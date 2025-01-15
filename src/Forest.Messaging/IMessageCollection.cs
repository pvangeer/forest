using System.ComponentModel;

namespace Forest.Messaging
{
    public interface IMessageCollection : INotifyPropertyChanged
    {
        MessageList Messages { get; }

        void OnPropertyChanged(string propertyName = null);
    }
}