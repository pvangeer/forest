using System;
using System.Windows.Input;
using Forest.Gui.ViewModels;

namespace Forest.Gui.Command
{
    public class ShowMessageListCommand : ICommand
    {
        public ShowMessageListCommand(GuiViewModel guiViewModel)
        {
            ViewModel = guiViewModel;
        }

        public GuiViewModel ViewModel { get; }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            ViewModel.ShowMessages = ViewModel.MessagesViewModel.MessageList.Count != 0;
            ViewModel.OnPropertyChanged(nameof(GuiViewModel.ShowMessages));
        }

        public event EventHandler CanExecuteChanged;
    }
}