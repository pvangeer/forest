using System;
using System.Windows.Input;
using Forest.Gui;

namespace Forest.Visualization.Commands.Project
{
    public class SaveProjectCommand : ICommand
    {
        private readonly ForestGui gui;

        public SaveProjectCommand(ForestGui gui)
        {
            this.gui = gui;
        }

        public bool CanExecute(object parameter)
        {
            return gui.ForestAnalysis != null;
        }

        public void Execute(object parameter)
        {
            gui.GuiProjectServices.SaveProject();
        }

        public event EventHandler CanExecuteChanged;
    }
}