using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Forest.Data.Tree;

namespace Forest.Data.Estimations
{
    public class TreeEventProbabilityEstimation : INotifyPropertyChanged
    {
        public TreeEventProbabilityEstimation(TreeEvent treeEvent)
        {
            TreeEvent = treeEvent;
            ClassProbabilitySpecification = new ObservableCollection<ExpertClassEstimation>();
        }

        public TreeEvent TreeEvent { get; }

        public ObservableCollection<ExpertClassEstimation> ClassProbabilitySpecification { get; }

        public Probability FixedProbability { get; set; }

        public FragilityCurve FragilityCurve { get; set; }

        public ProbabilitySpecificationType ProbabilitySpecificationType { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
                return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}