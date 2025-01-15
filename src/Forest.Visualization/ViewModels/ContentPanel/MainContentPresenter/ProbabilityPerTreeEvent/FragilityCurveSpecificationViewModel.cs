using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using Forest.Data.Estimations.PerTreeEvent;

namespace Forest.Visualization.ViewModels.ContentPanel.MainContentPresenter.ProbabilityPerTreeEvent
{
    public class FragilityCurveSpecificationViewModel : ProbabilitySpecificationViewModelBase
    {
        private readonly ObservableCollection<FragilityCurveElementViewModel> fragilityCurveViewModels;

        public FragilityCurveSpecificationViewModel(
            TreeEventProbabilityEstimate estimate,
            ViewModelFactory factory) : base(estimate, factory)
        {
            fragilityCurveViewModels =
                new ObservableCollection<FragilityCurveElementViewModel>(
                    estimate.FragilityCurve.Select(e => new FragilityCurveElementViewModel(e)));
            fragilityCurveViewModels.CollectionChanged += FragilityCurveViewModelsCollectionChanged;
        }

        public ObservableCollection<FragilityCurveElementViewModel> FragilityCurve => fragilityCurveViewModels;

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