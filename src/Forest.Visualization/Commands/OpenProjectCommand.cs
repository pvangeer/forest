using System;
using System.Windows.Input;
using Forest.Visualization.ViewModels;

namespace Forest.Visualization.Commands
{
    public class OpenProjectCommand : ICommand
    {
        private readonly RibbonViewModel viewModel;

        public OpenProjectCommand(RibbonViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            viewModel.OpenProject();
        }

        public event EventHandler CanExecuteChanged;
    }
}