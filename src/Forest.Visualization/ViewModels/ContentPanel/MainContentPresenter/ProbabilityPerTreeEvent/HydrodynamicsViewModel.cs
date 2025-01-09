using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using Forest.Data;
using Forest.Data.Estimations.PerTreeEvent;

namespace Forest.Visualization.ViewModels.ContentPanel.MainContentPresenter.ProbabilityPerTreeEvent
{
    public class HydrodynamicsViewModel : Entity
    {
        private readonly ProbabilityEstimationPerTreeEvent estimationObject;

        public HydrodynamicsViewModel(ProbabilityEstimationPerTreeEvent probabilityEstimationPerTreeEvent)
        {
            estimationObject = probabilityEstimationPerTreeEvent;
            Hydrodynamics = new ObservableCollection<HydrodynamicConditionViewModel>();
            foreach (var condition in probabilityEstimationPerTreeEvent.HydrodynamicConditions)
                Hydrodynamics.Add(new HydrodynamicConditionViewModel(condition));

            Hydrodynamics.CollectionChanged += HydrodynamicsViewModelCollectionChanged;
        }

        public ObservableCollection<HydrodynamicConditionViewModel> Hydrodynamics { get; }

        private void HydrodynamicsViewModelCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (var hydrodynamicConditionViewModel in e.NewItems.OfType<HydrodynamicConditionViewModel>())
                        estimationObject.AddHydrodynamicCondition(hydrodynamicConditionViewModel.HydrodynamicCondition);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (var hydrodynamicConditionsViewModel in e.OldItems.OfType<HydrodynamicConditionViewModel>())
                        estimationObject.AddHydrodynamicCondition(hydrodynamicConditionsViewModel.HydrodynamicCondition);
                    break;
                case NotifyCollectionChangedAction.Reset:
                    estimationObject.HydrodynamicConditions.Clear();
                    break;
            }
        }
    }
}