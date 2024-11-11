using System.Collections.Generic;
using System.Linq;
using Forest.Data.Estimations.PerTreeEvent;
using Forest.Data.Tree;
using Forest.Gui;

namespace Forest.Visualization.ViewModels.ContentPanel.MainContentPresenter.ProbabilityPerTreeEvent
{
    public class EstimationPerTreeEventSpecificationViewModel : ViewModelBase
    {
        private readonly ProbabilityEstimationPerTreeEvent estimation;
        private readonly ForestGui gui;

        public EstimationPerTreeEventSpecificationViewModel(ProbabilityEstimationPerTreeEvent estimation, ForestGui gui, ViewModelFactory factory) : base(factory)
        {
            this.gui = gui;
            this.estimation = estimation;
        }

        public IEnumerable<TreeEventEstimationViewModel> AllTreeEvents => GetAllEventsRecursive();

        public TreeEventEstimationViewModel SelectedTreeEvent
        {
            get => FindTreeEventViewModel(gui.SelectionManager.SelectedTreeEvent[estimation.EventTree]);
            set
            {
                if (value != null)
                    gui.SelectionManager.SelectTreeEvent(estimation.EventTree, value.GetTreeEvent());
                OnPropertyChanged();
            }
        }

        private TreeEventEstimationViewModel FindTreeEventViewModel(TreeEvent treeEvent)
        {
            return treeEvent == null ? null : AllTreeEvents?.FirstOrDefault(e => e.IsViewModelFor(treeEvent));
        }

        private IEnumerable<TreeEventEstimationViewModel> GetAllEventsRecursive()
        {
            return estimation.Estimates.Select(treeEventProbabilityEstimate =>
                ViewModelFactory.CreateTreeEventEstimationViewModel(treeEventProbabilityEstimate, estimation));
        }
    }
}