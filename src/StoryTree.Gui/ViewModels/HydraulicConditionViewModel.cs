using System.ComponentModel;
using System.Runtime.CompilerServices;
using StoryTree.Data.Annotations;
using StoryTree.Data.Estimations.Classes;
using StoryTree.Data.Hydraulics;

namespace StoryTree.Gui.ViewModels
{
    public class HydraulicConditionViewModel : INotifyPropertyChanged
    {
        public HydraulicConditionViewModel():this(new HydraulicCondition()) { }

        public HydraulicConditionViewModel(HydraulicCondition condition)
        {
            HydraulicCondition = condition;
        }

        public HydraulicCondition HydraulicCondition { get; }

        public double WaterLevel
        {
            get => HydraulicCondition.WaterLevel;
            set
            {
                HydraulicCondition.WaterLevel = value;
                OnPropertyChanged();
            }
        }

        public Probability Probability
        {
            get => HydraulicCondition.Probability;
            set
            {
                HydraulicCondition.Probability = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(ProbabilityDouble));
            }
        }

        public double ProbabilityDouble => HydraulicCondition.Probability;

        public double WavePeriod
        {
            get => HydraulicCondition.WavePeriod;
            set
            {
                HydraulicCondition.WavePeriod = value;
                OnPropertyChanged();
            }
        }

        public double WaveHeight
        {
            get => HydraulicCondition.WaveHeight;
            set
            {
                HydraulicCondition.WaveHeight = value;
                OnPropertyChanged();
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
