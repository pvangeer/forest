using StoryTree.Data.Hydraulics;

namespace StoryTree.Gui.ViewModels
{
    public class HydraulicConditionViewModel : FragilityCurveElementViewModel
    {
        public HydraulicConditionViewModel() : this(new HydraulicCondition())
        {
        }

        public HydraulicConditionViewModel(HydraulicCondition condition) : base(condition)
        {
            HydraulicCondition = condition;
        }

        public HydraulicCondition HydraulicCondition { get; }

        public double WavePeriod
        {
            get => HydraulicCondition.WavePeriod;
            set
            {
                HydraulicCondition.WavePeriod = value;
                HydraulicCondition.OnPropertyChanged();
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
                HydraulicCondition.OnPropertyChanged();
            }
        }
    }
}