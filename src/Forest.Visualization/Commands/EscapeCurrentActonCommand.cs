using System;
using System.Windows.Input;
using Forest.Gui;

namespace Forest.Visualization.Commands
{
    public class EscapeCurrentActionCommand : ICommand
    {
        private readonly ForestGui gui;

        public EscapeCurrentActionCommand(ForestGui gui)
        {
            this.gui = gui;
        }

        public bool CanExecute(object parameter)
        {
            return gui.IsSaveToImage;
        }

        public void Execute(object parameter)
        {
            if (gui.IsSaveToImage)
            {
                gui.IsSaveToImage = false;
                gui.OnPropertyChanged(nameof(ForestGui.IsSaveToImage));
            }
        }

        public event EventHandler CanExecuteChanged;
    }
}