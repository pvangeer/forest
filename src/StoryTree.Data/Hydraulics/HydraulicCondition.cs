using System.ComponentModel;
using System.Runtime.CompilerServices;
using StoryTree.Data.Annotations;
using StoryTree.Data.Estimations.Classes;

namespace StoryTree.Data.Hydraulics
{
    public class HydraulicCondition : INotifyPropertyChanged
    {
        public HydraulicCondition() : this(0,(Probability)(1/1000.0),0.0,0.0){ }

        public HydraulicCondition(double waterLevel, Probability probability, double waveHeight, double wavePeriod)
        {
            Probability = probability;
            WaterLevel = waterLevel;
            WaveHeight = waveHeight;
            WavePeriod = wavePeriod;
        }

        public double WavePeriod { get; set; }

        public double WaveHeight { get; set; }

        public Probability Probability { get; set; }

        public double WaterLevel { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}