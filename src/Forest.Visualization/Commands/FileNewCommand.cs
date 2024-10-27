using System;
using System.Windows.Input;
using Forest.Gui;

namespace Forest.Visualization.Commands
{
    public class FileNewCommand : ICommand
    {
        private readonly ForestGui gui;
        public FileNewCommand(ForestGui gui)
        {
            this.gui = gui;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            gui.GuiProjectServices.NewProject();
        }

        public event EventHandler CanExecuteChanged;
    }
}