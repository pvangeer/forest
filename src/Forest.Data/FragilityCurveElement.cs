using System.ComponentModel;
using System.Runtime.CompilerServices;
using Forest.Data.Properties;

namespace Forest.Data
{
    public class FragilityCurveElement : INotifyPropertyChanged
    {
        public FragilityCurveElement(double waterLevel, Probability probability)
        {
            Probability = probability;
            WaterLevel = waterLevel;
        }

        public double WaterLevel { get; set; }

        public Probability Probability { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        public virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}