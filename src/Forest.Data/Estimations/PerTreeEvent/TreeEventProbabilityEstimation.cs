using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Forest.Data.Probabilities;
using Forest.Data.Tree;

namespace Forest.Data.Estimations.PerTreeEvent
{
    public class TreeEventProbabilityEstimation : INotifyPropertyChanged
    {
        public TreeEventProbabilityEstimation(TreeEvent treeEvent)
        {
            TreeEvent = treeEvent;
            FixedProbability = (Probability)1;
            ClassProbabilitySpecification = new ObservableCollection<ExpertClassEstimation>();
            FragilityCurve = new FragilityCurve();
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