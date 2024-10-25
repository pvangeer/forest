using System;
using System.Linq;
using System.Windows.Input;
using Forest.Visualization.ViewModels;

namespace Forest.Visualization.Commands
{
    public class RemoveAllMessagesCommand : ICommand
    {
        private readonly MessageListViewModel messageListViewModel;

        public RemoveAllMessagesCommand(MessageListViewModel messageListViewModel)
        {
            this.messageListViewModel = messageListViewModel;
        }


        public bool CanExecute(object parameter)
        {
            return messageListViewModel.MessageList.Any();
        }

        public void Execute(object parameter)
        {
            messageListViewModel.ClearAllMessages();
        }

        public event EventHandler CanExecuteChanged;
    }
}