using System.ComponentModel;
using Forest.Data.Estimations.PerTreeEvent;

namespace Forest.Visualization.ViewModels
{
    public class ProbabilitySpecificationViewModelBase : ViewModelBase
    {
        public ProbabilitySpecificationViewModelBase(TreeEventProbabilityEstimate estimate, ViewModelFactory factory) : base(factory)
        {
            Estimate = estimate;
            Estimate.PropertyChanged += EstimationPropertyChanged;
        }

        public TreeEventProbabilityEstimate Estimate { get; }

        public ProbabilitySpecificationType Type => Estimate.ProbabilitySpecificationType;

        public event PropertyChangedEventHandler PropertyChanged;

        private void EstimationPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnEstimationPropertyChanged(e.PropertyName);
        }

        protected virtual void OnEstimationPropertyChanged(string propertyName)
        {
        }
    }
}