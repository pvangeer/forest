using System;
using System.ComponentModel;
using System.Windows.Input;
using Forest.Data.Services;
using Forest.Data.Tree;
using Forest.Gui;

namespace Forest.Visualization.Commands.ProbabilityEstimations
{
    public class AddProbabilityEstimationPerTreeEventCommand : ICommand
    {
        private readonly ForestGui gui;

        public AddProbabilityEstimationPerTreeEventCommand(ForestGui gui)
        {
            this.gui = gui;
            gui.SelectionManager.PropertyChanged += SelectionChanged;
        }

        public bool CanExecute(object parameter)
        {
            return gui.SelectionManager.Selection is EventTree;
        }

        public void Execute(object parameter)
        {
            var service = new AnalysisManipulationService(gui.ForestAnalysis);
            service.AddProbabilityEstimationPerTreeEvent(gui.SelectionManager.Selection as EventTree);
        }

        public event EventHandler CanExecuteChanged;

        private void SelectionChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(SelectionManager.Selection):
                    if (CanExecuteChanged != null)
                        CanExecuteChanged.Invoke(this, new EventArgs());
                    break;
            }
        }
    }
}