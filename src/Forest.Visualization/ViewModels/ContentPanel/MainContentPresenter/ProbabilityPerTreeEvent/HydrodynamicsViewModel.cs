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
            Hydrodynamics = new ObservableCollection<FragilityCurveElementViewModel>();
            foreach (var condition in probabilityEstimationPerTreeEvent.HydrodynamicConditions)
                Hydrodynamics.Add(new FragilityCurveElementViewModel(condition));

            Hydrodynamics.CollectionChanged += HydrodynamicsViewModelCollectionChanged;
        }

        public ObservableCollection<FragilityCurveElementViewModel> Hydrodynamics { get; }

        private void HydrodynamicsViewModelCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (var hydrodynamicConditionViewModel in e.NewItems.OfType<FragilityCurveElementViewModel>())
                        estimationObject.AddHydrodynamicCondition(hydrodynamicConditionViewModel.FragilityCurveElement);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (var hydrodynamicConditionsViewModel in e.OldItems.OfType<FragilityCurveElementViewModel>())
                        estimationObject.AddHydrodynamicCondition(hydrodynamicConditionsViewModel.FragilityCurveElement);
                    break;
                case NotifyCollectionChangedAction.Reset:
                    estimationObject.HydrodynamicConditions.Clear();
                    break;
            }
        }
    }
}