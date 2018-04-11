using System.ComponentModel;
using System.Runtime.CompilerServices;
using StoryTree.Data;
using StoryTree.Data.Annotations;

namespace StoryTree.Gui.ViewModels
{
    public class FragilityCurveElementViewModel : INotifyPropertyChanged
    {
        public FragilityCurveElementViewModel(FragilityCurveElement element)
        {
            FragilityCurveElement = element;
        }

        public FragilityCurveElement FragilityCurveElement { get; }

        public double ProbabilityDouble => FragilityCurveElement.Probability;

        public double WaterLevel => FragilityCurveElement.WaterLevel;

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
