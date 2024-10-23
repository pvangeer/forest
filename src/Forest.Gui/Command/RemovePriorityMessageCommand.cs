using System;
using System.Windows.Input;
using Forest.Gui.ViewModels;

namespace Forest.Gui.Command
{
    public class RemovePriorityMessageCommand : ICommand
    {
        public RemovePriorityMessageCommand(MainWindowViewModel mainWindowViewModel)
        {
            ViewModel = mainWindowViewModel;
        }

        private MainWindowViewModel ViewModel { get; }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            ViewModel.PriorityMessage = null;
            ViewModel.OnPropertyChanged(nameof(MainWindowViewModel.PriorityMessage));
        }

        public event EventHandler CanExecuteChanged;
    }
}