using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using Forest.Data;
using Forest.Data.Estimations.PerTreeEvent;

namespace Forest.Visualization.ViewModels.ContentPanel.MainContentPresenter.ProbabilityPerTreeEvent
{
    public class ExpertsViewModel : Entity
    {
        private readonly ProbabilityEstimationPerTreeEvent estimationObject;

        public ExpertsViewModel(ProbabilityEstimationPerTreeEvent probabilityEstimationPerTreeEvent)
        {
            estimationObject = probabilityEstimationPerTreeEvent;
            Experts = new ObservableCollection<ExpertViewModel>();
            foreach (var expert in probabilityEstimationPerTreeEvent.Experts)
                Experts.Add(new ExpertViewModel(expert));

            Experts.CollectionChanged += ExpertsViewModelCollectionChanged;
        }

        public ObservableCollection<ExpertViewModel> Experts { get; }

        private void ExpertsViewModelCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (var expertViewModel in e.NewItems.OfType<ExpertViewModel>())
                        estimationObject.AddExpert(expertViewModel.Expert);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (var expertViewModel in e.OldItems.OfType<ExpertViewModel>())
                        estimationObject.RemoveExpert(expertViewModel.Expert);
                    break;
                case NotifyCollectionChangedAction.Reset:
                    estimationObject.Experts.Clear();
                    break;
            }
        }
    }
}