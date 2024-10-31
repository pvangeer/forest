using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using Forest.Data;
using Forest.Data.Estimations.PerTreeEvent;

namespace Forest.Visualization.ViewModels
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

            Hydrodynamics.CollectionChanged += ExpertsViewModelCollectionChanged;
        }

        public ObservableCollection<HydrodynamicConditionViewModel> Hydrodynamics { get; }

        private void ExpertsViewModelCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (var expertViewModel in e.NewItems.OfType<HydrodynamicConditionViewModel>())
                        estimationObject.AddHydrodynamicCondition(expertViewModel.HydrodynamicCondition);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (var expertViewModel in e.OldItems.OfType<HydrodynamicConditionViewModel>())
                        estimationObject.AddHydrodynamicCondition(expertViewModel.HydrodynamicCondition);
                    break;
                case NotifyCollectionChangedAction.Reset:
                    estimationObject.Experts.Clear();
                    break;
            }
        }
    }
}