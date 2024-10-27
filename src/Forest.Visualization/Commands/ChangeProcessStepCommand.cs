using System;
using System.Windows.Input;
using Forest.Gui;

namespace Forest.Visualization.Commands
{
    public class ChangeProcessStepCommand : ICommand
    {
        private readonly ForestGui gui;

        public ChangeProcessStepCommand(ForestGui gui)
        {
            this.gui = gui;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            if (!(parameter is ForestGuiState guiState))
                return;

            gui.SelectedState = guiState;
            gui.OnPropertyChanged(nameof(ForestGui.SelectedState));
        }

        public event EventHandler CanExecuteChanged;
    }
}