using System.ComponentModel;
using Forest.Data.Estimations.PerTreeEvent;

namespace Forest.Visualization.ViewModels.ContentPanel.MainContentPresenter.ProbabilityPerTreeEvent
{
    public class ProbabilitySpecificationViewModelBase : ViewModelBase
    {
        public ProbabilitySpecificationViewModelBase(TreeEventProbabilityEstimate estimate, ViewModelFactory factory) : base(factory)
        {
            Estimate = estimate;
            Estimate.PropertyChanged += EstimationPropertyChanged;
        }

        protected TreeEventProbabilityEstimate Estimate { get; }

        public ProbabilitySpecificationType Type
        {
            get => Estimate.ProbabilitySpecificationType;
            set => Estimate.ChangeProbabilityEstimationType(value);
        }

        private void EstimationPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(TreeEventProbabilityEstimate.ProbabilitySpecificationType):
                    OnPropertyChanged(nameof(Type));
                    break;
            }
            OnEstimationPropertyChanged(e.PropertyName);
        }

        protected virtual void OnEstimationPropertyChanged(string propertyName)
        {
        }
    }
}