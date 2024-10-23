using System;
using System.Windows.Input;
using Forest.Gui.Components;

namespace Forest.Gui.ViewModels
{
    public class ChangeProcessStepCommand : ICommand
    {
        public ChangeProcessStepCommand(GuiViewModel guiViewModel)
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
            if (!(parameter is ForestGuiState process))
                return;

            ViewModel.SelectedState = process;
        }

        public event EventHandler CanExecuteChanged;
    }
}