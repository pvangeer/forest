using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using Forest.Data.Estimations.PerTreeEvent;
using Forest.Data.Probabilities;

namespace Forest.Visualization.ViewModels.ContentPanel.MainContentPresenter.ProbabilityPerTreeEvent
{
    public class FragilityCurveSpecificationViewModel : ProbabilitySpecificationViewModelBase
    {
        public FragilityCurveSpecificationViewModel(
            TreeEventProbabilityEstimate estimate,
            ViewModelFactory factory) : base(estimate, factory)
        {
            FragilityCurve =
                new ObservableCollection<FragilityCurveElementViewModel>(
                    estimate.FragilityCurve.Select(e => new FragilityCurveElementViewModel(e)));
            FragilityCurve.CollectionChanged += FragilityCurveViewModelsCollectionChanged;
        }

        public ObservableCollection<FragilityCurveElementViewModel> FragilityCurve {get; }

        private void FragilityCurveViewModelsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
                foreach (var item in e.NewItems.OfType<FragilityCurveElementViewModel>())
                    Estimate.FragilityCurve.Add(item.FragilityCurveElement);

            if (e.Action == NotifyCollectionChangedAction.Remove)
                foreach (var item in e.OldItems.OfType<FragilityCurveElementViewModel>())
                    Estimate.FragilityCurve.Remove(item.FragilityCurveElement);
        }
    }
}