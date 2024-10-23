using System;
using System.Windows.Input;
using Forest.Visualization.ViewModels;

namespace Forest.Visualization.Commands
{
    public class SaveProjectCommand : ICommand
    {
        public SaveProjectCommand(RibbonViewModel viewModel)
        {
            ViewModel = viewModel;
        }

        public RibbonViewModel ViewModel { get; }

        public bool CanExecute(object parameter)
        {
            return ViewModel.CanSaveProject();
        }

        public void Execute(object parameter)
        {
            ViewModel.SaveProject();
        }

        public event EventHandler CanExecuteChanged;
    }
}