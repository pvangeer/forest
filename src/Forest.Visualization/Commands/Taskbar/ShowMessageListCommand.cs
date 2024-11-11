using System;
using System.Windows.Input;
using Forest.Visualization.ViewModels;
using Forest.Visualization.ViewModels.StatusBar;

namespace Forest.Visualization.Commands.Taskbar
{
    public class ShowMessageListCommand : ICommand
    {
        public ShowMessageListCommand(StatusBarViewModel statusBarViewModel)
        {
            ViewModel = statusBarViewModel;
        }

        public StatusBarViewModel ViewModel { get; }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            ViewModel.ShowMessages = ViewModel.MessagesViewModel.MessageList.Count != 0;
            ViewModel.OnPropertyChanged(nameof(StatusBarViewModel.ShowMessages));
        }

        public event EventHandler CanExecuteChanged;
    }
}