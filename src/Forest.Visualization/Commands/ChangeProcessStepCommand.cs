using System;
using System.Windows.Input;
using Forest.Gui.Components;
using Forest.Visualization.ViewModels;

namespace Forest.Visualization.Commands
{
    public class ChangeProcessStepCommand : ICommand
    {
        private readonly RibbonViewModel viewModel;

        public ChangeProcessStepCommand(RibbonViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            if (!(parameter is ForestGuiState guiState))
                return;

            viewModel.SelectedState = guiState;
        }

        public event EventHandler CanExecuteChanged;
    }
}