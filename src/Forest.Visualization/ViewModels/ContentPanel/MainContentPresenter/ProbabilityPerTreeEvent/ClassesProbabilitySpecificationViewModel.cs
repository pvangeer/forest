using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using Forest.Data.Estimations.PerTreeEvent;

namespace Forest.Visualization.ViewModels.ContentPanel.MainContentPresenter.ProbabilityPerTreeEvent
{
    public class ClassesProbabilitySpecificationViewModel : ProbabilitySpecificationViewModelBase
    {
        public ClassesProbabilitySpecificationViewModel(TreeEventProbabilityEstimate estimate, ViewModelFactory factory) : base(estimate, factory)
        {
            ClassesProbabilitySpecification = new ObservableCollection<ExpertClassEstimationViewModel>(
                estimate.ClassProbabilitySpecifications.Select(e => ViewModelFactory.CreateExpertClassEstimationViewModel(e)));

            estimate.ClassProbabilitySpecifications.CollectionChanged += SpecificationsCollectionChanged;
        }

        public ObservableCollection<ExpertClassEstimationViewModel> ClassesProbabilitySpecification { get; }

        private void SpecificationsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (var expertClassEstimation in e.NewItems.OfType<ExpertClassEstimation>())
                        ClassesProbabilitySpecification.Add(ViewModelFactory.CreateExpertClassEstimationViewModel(expertClassEstimation));
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (var expertClassEstimation in e.OldItems.OfType<ExpertClassEstimation>())
                    {
                        var itemToRemove = ClassesProbabilitySpecification.FirstOrDefault(vm => vm.IsViewModelFor(expertClassEstimation));
                        if (itemToRemove != null)
                            ClassesProbabilitySpecification.Remove(itemToRemove);
                    }

                    break;
            }
        }
    }
}