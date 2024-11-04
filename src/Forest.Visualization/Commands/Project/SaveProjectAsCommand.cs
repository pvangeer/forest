using System;
using System.Windows.Input;
using Forest.Gui;

namespace Forest.Visualization.Commands.Project
{
    public class SaveProjectAsCommand : ICommand
    {
        private readonly ForestGui gui;

        public SaveProjectAsCommand(ForestGui gui)
        {
            this.gui = gui;
        }

        public bool CanExecute(object parameter)
        {
            return gui.ForestAnalysis != null;
        }

        public void Execute(object parameter)
        {
            gui.GuiProjectServices.SaveProjectAs();
        }

        public event EventHandler CanExecuteChanged;
    }
}