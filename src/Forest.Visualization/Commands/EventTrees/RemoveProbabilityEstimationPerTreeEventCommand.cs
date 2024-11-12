using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using Forest.Data.Estimations.PerTreeEvent;
using Forest.Data.Services;
using Forest.Gui;

namespace Forest.Visualization.Commands.EventTrees
{
    public class RemoveProbabilityEstimationPerTreeEventCommand : ICommand
    {
        private readonly ForestGui gui;
        private ProbabilityEstimationPerTreeEvent estimation;

        public RemoveProbabilityEstimationPerTreeEventCommand(ForestGui gui, ProbabilityEstimationPerTreeEvent estimation)
        {
            this.estimation = estimation;
            if (estimation == null)
            {
                this.estimation = gui?.SelectionManager.Selection as ProbabilityEstimationPerTreeEvent;
                if (gui != null)
                    gui.SelectionManager.PropertyChanged += SelectionManagerPropertyChanged;
            }

            this.gui = gui;
            if (gui != null)
                gui.ForestAnalysis.ProbabilityEstimationsPerTreeEvent.CollectionChanged += EstimationsCollectionChanged;
        }

        public bool CanExecute(object parameter)
        {
            return estimation != null;
        }

        public void Execute(object parameter)
        {
            var index = gui.ForestAnalysis.ProbabilityEstimationsPerTreeEvent.IndexOf(estimation);

            var service = new AnalysisManipulationService(gui.ForestAnalysis);
            service.RemoveProbabilityEstimationPerTreeEvent(estimation);

            if (gui.SelectionManager.Selection == estimation)
            {
                if (index >= gui.ForestAnalysis.ProbabilityEstimationsPerTreeEvent.Count)
                    index = gui.ForestAnalysis.ProbabilityEstimationsPerTreeEvent.Count - 1;

                gui.SelectionManager.SetSelection(index > -1 ? gui.ForestAnalysis.ProbabilityEstimationsPerTreeEvent.ElementAt(index) : null);
            }
        }

        public event EventHandler CanExecuteChanged;

        private void SelectionManagerPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(SelectionManager.Selection):
                    estimation = gui?.SelectionManager.Selection as ProbabilityEstimationPerTreeEvent;
                    if (CanExecuteChanged != null)
                        CanExecuteChanged.Invoke(this, EventArgs.Empty);
                    break;
            }
        }

        private void EstimationsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (CanExecuteChanged != null)
                CanExecuteChanged.Invoke(this, e);
        }
    }
}