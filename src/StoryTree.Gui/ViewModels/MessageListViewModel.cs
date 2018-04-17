using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using StoryTree.Gui.Command;
using StoryTree.Messaging;
using StoryTree.Messaging.Properties;

namespace StoryTree.Gui.ViewModels
{
    public class MessageListViewModel : INotifyPropertyChanged
    {
        public MessageListViewModel(MessageList messageList)
        {
            this.MessageList = messageList;
            MessageList.CollectionChanged += MessageListCollectionChanged;
        }

        private void MessageListCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(RemoveAllMessagesCommand));
            OnPropertyChanged(nameof(MessageList));
        }

        public MessageList MessageList { get; }

        public ICommand RemoveAllMessagesCommand => new RemoveAllMessagesCommand(this);

        public void ClearAllMessages()
        {
            MessageList.Clear();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}