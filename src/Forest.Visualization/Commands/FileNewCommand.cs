using System;
using System.Windows.Input;
using Forest.Visualization.ViewModels;

namespace Forest.Visualization.Commands
{
    public class FileNewCommand : ICommand
    {
        public FileNewCommand(RibbonViewModel viewModel)
        {
            ViewModel = viewModel;
        }

        private RibbonViewModel ViewModel { get; }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            ViewModel.NewProject();
        }

        public event EventHandler CanExecuteChanged;
    }
}