using System.ComponentModel;
using System.Runtime.CompilerServices;
using Forest.Data;
using Forest.Data.Estimations;
using Forest.Data.Properties;

namespace Forest.Visualization.ViewModels
{
    public class FragilityCurveElementViewModel : INotifyPropertyChanged
    {
        public FragilityCurveElementViewModel(FragilityCurveElement element)
        {
            FragilityCurveElement = element;
        }

        public FragilityCurveElement FragilityCurveElement { get; }

        public double ProbabilityDouble => FragilityCurveElement.Probability;

        public double WaterLevel
        {
            get => FragilityCurveElement.WaterLevel;
            set
            {
                FragilityCurveElement.WaterLevel = value;
                OnPropertyChanged();
            }
        }

        public Probability Probability
        {
            get => FragilityCurveElement.Probability;
            set
            {
                FragilityCurveElement.Probability = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(ProbabilityDouble));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}