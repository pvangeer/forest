using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using StoryTree.Data.Properties;
using StoryTree.Gui.Command;
using StoryTree.Messaging;

namespace StoryTree.Gui.ViewModels
{
    public class MessageListViewModel : INotifyPropertyChanged
    {
        public MessageListViewModel(MessageList messageList)
        {
            MessageList = messageList;
            MessageList.CollectionChanged += MessageListCollectionChanged;
        }

        public MessageList MessageList { get; }

        public ICommand RemoveAllMessagesCommand => new RemoveAllMessagesCommand(this);

        public event PropertyChangedEventHandler PropertyChanged;

        private void MessageListCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(RemoveAllMessagesCommand));
            OnPropertyChanged(nameof(MessageList));
        }

        public void ClearAllMessages()
        {
            MessageList.Clear();
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}