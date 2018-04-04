using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
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
