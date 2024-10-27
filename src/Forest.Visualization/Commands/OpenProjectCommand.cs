using System;
using System.Windows.Input;
using Forest.Gui;
using Forest.Visualization.ViewModels;

namespace Forest.Visualization.Commands
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