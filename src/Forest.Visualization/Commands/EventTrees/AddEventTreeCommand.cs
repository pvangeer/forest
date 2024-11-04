using System;
using System.Windows.Input;
using Forest.Data.Services;
using Forest.Gui;

namespace Forest.Visualization.Commands.EventTrees
{
    public class AddEventTreeCommand : ICommand
    {
        private readonly ForestGui gui;

        public AddEventTreeCommand(ForestGui gui)
        {
            this.gui = gui;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var service = new AnalysisManipulationService(gui.ForestAnalysis);
            gui.SelectionManager.SetSelection(service.AddEventTree());
        }

        public event EventHandler CanExecuteChanged;
    }
}