using System;
using System.Windows.Input;
using Forest.Gui;

namespace Forest.Visualization.Commands.Project
{
    public class OpenProjectCommand : ICommand
    {
        private readonly ForestGui gui;

        public OpenProjectCommand(ForestGui gui)
        {
            this.gui = gui;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            gui.GuiProjectServices.OpenProject();
        }

        public event EventHandler CanExecuteChanged;
    }
}