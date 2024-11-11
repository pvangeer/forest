using System;
using System.Windows.Input;
using Forest.Visualization.ViewModels;
using Forest.Visualization.ViewModels.StatusBar;

namespace Forest.Visualization.Commands.Taskbar
{
    public class RemovePriorityMessageCommand : ICommand
    {
        public RemovePriorityMessageCommand(StatusBarViewModel statusBarViewModel)
        {
            ViewModel = statusBarViewModel;
        }

        private StatusBarViewModel ViewModel { get; }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            ViewModel.PriorityMessage = null;
            ViewModel.OnPropertyChanged(nameof(StatusBarViewModel.PriorityMessage));
        }

        public event EventHandler CanExecuteChanged;
    }
}